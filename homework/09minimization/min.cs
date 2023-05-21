using System;
using System.Threading;
using static System.Console;
using static System.Math; 
using System.IO;
using System.Collections.Generic;

public static class minimizer{
    public static vector fit(
        Func<vector,double> f,
        vector guesses,
        List<double> xs,
        List<double> ys,
        List<double> yerrs,
        double acc
    ){
        Func<vector,double> toMin = par => {
            //assume first parameter to f is x-value. 
            double sum = 0;
            vector parvector = new vector(guesses.size+1);
            for(int i = 0; i < xs.Count; i++){
                parvector[0] = xs[i];
                for(int j = 0; j < guesses.size; j++){
                    parvector[j+1] = par[j];
                }
                sum += Pow( (f(parvector)-ys[i])/ yerrs[i] ,2);
            }
            return sum;
        };
        //D(m,Γ,A)=Σi[(F(Ei|m,Γ,A)-σi)/Δσi]2 .
        vector fitparams = qnewton(toMin,guesses,acc);
        return fitparams;
    }


    public static vector qnewton(
        Func<vector,double> f, /* objective function */
        vector start, /* starting point */
        double acc /* accuracy goal, on exit |gradient| should be < acc */
    ){
        int dim = start.size;
        matrix B = new matrix(dim,dim);
        for(int i = 0; i < dim; i++){
            B[i,i] = 1;
        }

        for(int i = 0; i < dim; i++){
            //avoid division by zero in grad
            if(Abs(start[i] - 0) < 1.0/Pow(2,26)){
                start[i] = 1.0/Pow(2,20);
            }
        }

        vector gradx = grad(f,start);
        vector s = new vector(dim);
        vector u = new vector(dim);
        vector y = new vector(dim);
        matrix dB = new matrix(dim,dim);

        while(gradx.norm() > acc){

            vector dxvector = -B*gradx;
            double lambda = 1;
            while(true){
                s = lambda*dxvector;
                if(f(start + s) < f(start)){
                    //accept step; update x, hessian matrix, grad(f(x))
                    start = start + s;
                    vector oldGrad = gradx;
                    gradx = grad(f,start);
                    //update B
                    y = gradx - oldGrad;  
                    u = s - B*y;
                    double uy = u.dot(y); 
                    if(Abs(uy) > 1e-6){
                        //update only if uy is large enough to avoid "division by zero"
                        dB = matrix.outer(u,u)/uy;
                        B = B + dB;
                    }
                    break;
                }
                lambda = lambda / 2; 
                if(lambda < 1.0/Pow(2,16)){
                    //not making process - take step to get out, update x, re initialize hessian matrix, grad(f(x))
                    start = start + s;
                    gradx = grad(f,start);
                    B = new matrix(dim,dim);
                    for(int i = 0; i < dim; i++){
                        B[i,i] = 1;
                    }
                    break;
                }
            }
        }
        return start;
    }

    private static vector grad(Func<vector,double> f, vector x){
        int dim = x.size;
        vector grad = new vector(dim);
        for(int i = 0; i < dim; i++){
            vector newx = x.copy();
            double dx = Abs(x[i])*Pow(2,-26);
            newx[i] = x[i] + dx;
            grad[i] = (f(newx) - f(x))/dx;
        }
        return grad;
     }

    public static vector downhillSimplex(
        Func<vector,double> f,
        List<vector> starts,
        double stdgoal
    ){
        vector pce = new vector(starts[0].size);
        vector pre = new vector(starts[0].size);
        vector pex = new vector(starts[0].size);
        vector pco = new vector(starts[0].size);
        vector plo = new vector(starts[0].size);
        vector phi = new vector(starts[0].size);
        double high = 0;
        double low = 0;
        int highIndex = 0;
        int lowIndex = 0;
        double fpre = 0;
        double fplo = 0;
        double fpex = 0;
        double fpco = 0;
        double fphi = 0;
        double mean = 0;
        double std = 0;

        while(true){
            //according to wikipedia, Nelder and Mead used sample standard deviation as measure of termination.
            high = double.MinValue;
            low = double.MaxValue;
            mean = 0;
            pce = new vector(starts[0].size);

            for(int i = 0; i < starts.Count; i++){
                double val = f(starts[i]);
                //WriteLine($"Point {i} at {starts[i][0]},{starts[i][1]} has value {val}");
                if(val > high){
                    high = val;
                    highIndex = i;
                }
                if(val < low){
                    low = val;
                    lowIndex = i;
                }
                mean += val;
            }

            mean /= starts.Count;
            std = 0;

            for(int i = 0; i < starts.Count; i++){
                std += (f(starts[i])-mean)*(f(starts[i])-mean);
                if(i == highIndex) continue;
                pce += starts[i];
            }
            std = Sqrt(std/starts.Count);

            plo = starts[lowIndex];
            phi = starts[highIndex];
            pce /= (starts.Count-1);
            pre = pce + (pce - phi);
            fpre = f(pre);
            fplo = f(plo);
            fphi = f(phi);

            if(std < stdgoal){
                return plo;
            }

            //WriteLine($"fpre is {fpre}");
            if(fpre < fplo){
                pex = pce + 2.0*(pce - phi);
                fpex = f(pex);
                if(fpex < fplo){
                    //WriteLine($"Expansion of point {highIndex}");
                    starts[highIndex] = pex;
                    continue;
                }
                else{
                    //WriteLine($"Reflection of point {highIndex}");
                    starts[highIndex] = pre;
                    continue;
                }
            }
            else{
                pco = pce + 1.0/2.0 * (phi - pce);
                fpco = f(pco);
                //WriteLine($"fpco is {fpco}");
                if(fpco < fphi){
                    //WriteLine($"Contraction of point {highIndex}");
                    starts[highIndex] = pco;
                    continue;
                }
                else{
                    //reduction
                    //WriteLine($"Reduction");
                    for(int i = 0; i < starts.Count; i++){
                        if(i == lowIndex) continue;
                        starts[i] = 1.0/2.0 * (starts[i] + plo);
                    }
                    continue;
                }
            }
        }
    }

    public static vector simplexfit(
        Func<vector,double> f,
        List<vector> guesses,
        List<double> xs,
        List<double> ys,
        List<double> yerrs,
        double acc
    ){
        Func<vector,double> toMin = par => {
            //assume first parameter to f is x-value. 
            double sum = 0;
            vector parvector = new vector(guesses[0].size+1);
            for(int i = 0; i < xs.Count; i++){
                parvector[0] = xs[i];
                for(int j = 0; j < guesses[0].size; j++){
                    parvector[j+1] = par[j];
                }
                sum += Pow( (f(parvector)-ys[i])/ yerrs[i] ,2);
            }
            return sum;
        };
        //D(m,Γ,A)=Σi[(F(Ei|m,Γ,A)-σi)/Δσi]2 .
        vector fitparams = downhillSimplex(toMin,guesses,acc);
        return fitparams;
    }

}