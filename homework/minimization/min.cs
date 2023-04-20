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
/*
    set the inverse Hessian matrix to unity, B = 1
    repeat until converged (e.g. ∥∇ϕ∥ < tolerance) :
        calculate the Newton’s step ∆x = −B∇ϕ
        do linesearch starting with λ = 1 :
            if ϕ(x + λ∆x) < ϕ(x) accept the step:
                x = x + λ∆x
                update B = B + δB
                break linesearch
            λ = λ/2
            if λ is too small accept the step and reset B:
                x = x + λ∆x
                B = 1
                break linesearch
            continue linesearch*/
}