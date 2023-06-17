using System;
using System.Threading;
using static System.Console;
using static System.Math; 
using System.IO;

public static class mcint{
    public static Random rnd = new Random();

    public static (double,double) plainmc(Func<vector,double> f,vector a,vector b,int N, string diagString = null){
        if(N == 0){
            //this must mean function has constant value here; just return some number with zero uncertainty
            
            return (plainmc(f,a,b,1,diagString).Item1,0);
        }

        int dim=a.size; double V=1; 
        for(int i=0;i<dim;i++)V*=b[i]-a[i];

        double sum=0,sum2=0;

	    var x=new vector(dim);

        for(int i=0;i<N;i++){
            for(int k=0;k<dim;k++)x[k]=a[k]+rnd.NextDouble()*(b[k]-a[k]);
            if(!(diagString==null)){
                using (StreamWriter sw = File.AppendText(diagString))
                    {
                        sw.WriteLine($"{x[0]}\t{x[1]}");
                    }	
            }
            double fx=f(x); sum+=fx; sum2+=fx*fx;
        }
        
        double mean=sum/N, sigma=Sqrt(sum2/N-mean*mean);
        var result=(mean*V,sigma*V/Sqrt(N));
        return result;
    }

    


    public static (double,double) haltonint(Func<vector,double> f,vector a,vector b,int N){
        int dim=a.size; double V=1; 
        for(int i=0;i<dim;i++)V*=b[i]-a[i];

        double sum=0,sum2=0;

	    var x=new vector(dim);
        var x2=new vector(dim);

        for(int i=0;i<N;i++){
            vector n1 = halton(i,dim);
            vector n2 = halton(i,dim,true);
            for(int k=0;k<dim;k++)x[k]=a[k]+n1[k]*(b[k]-a[k]);
            for(int k=0;k<dim;k++)x2[k]=a[k]+n2[k]*(b[k]-a[k]);
            sum+=f(x); sum2+=f(x2);
        }

        var result=(sum*V/N,Abs(sum*V/N-sum2*V/N));
        return result;
    }

    private static double corput(int n, int b){
        double q=0, bk=(double ) 1 / b ;
        while ( n>0){
            q += ( n % b ) * bk; 
            n /= b; 
            bk /= b;
        }
        return q; 
    }

    private static vector halton(int n, int d, bool rev = false){
        int[] bs = {2,3,5,7,11,13,17,19,23,29,31,37,41,43,47,53,59,61,67};
        if(d > bs.Length){
            throw new Exception("Too high dimension in Halton sequence.");
        }
        vector xs = new vector(d);
        for(int i = 0; i < d; i++){
            if(!rev){
                xs[i] = corput(n,bs[i]);
            }
            else{
                xs[i] = corput(n,bs[bs.Length-1-i]); //for changing base for error estimate
            }
        }
        return xs;
    }

    public static (double,double) strataint(Func<vector,double> f,vector a,vector b,int N, int nmin, string diagString = null){
        if(N < nmin){
            return plainmc(f,a,b,N,diagString);
        }

        int dim = a.size;
        var x=new vector(dim);
        vector leftn = new vector(dim), rightn = new vector(dim), rightf = new vector(dim), leftf = new vector(dim), rightf2 = new vector(dim), leftf2 = new vector(dim);
        vector leftvar = new vector(dim), rightvar = new vector(dim), leftmean = new vector(dim), rightmean = new vector(dim);

        double V=1; 
        for(int i=0;i<dim;i++)V*=b[i]-a[i];

        for(int i = 0; i < nmin; i++){
            for(int k=0;k<dim;k++)x[k]=a[k]+rnd.NextDouble()*(b[k]-a[k]);
            //WriteLine($"{x[0]}, {x[1]},");
            double fx=f(x);
            if(!(diagString==null)){
                using (StreamWriter sw = File.AppendText(diagString))
                    {
                        sw.WriteLine($"{x[0]}\t{x[1]}");
                    }	
            }
            for(int j = 0; j < dim; j++){
                if(x[j] > (a[j] + 0.5 * (b[j]-a[j]))){
                    //right half in this dimension.
                    rightn[j]++;
                    rightf[j]+=fx;
                    rightf2[j]+=fx*fx;
                }
                else{
                    //left half 
                    leftn[j]++;
                    leftf[j]+=fx;
                    leftf2[j]+=fx*fx;
                }
            }
        }
        double maxvar = 0;
        int dimmax = 0;
//
        for(int i = 0; i < dim; i++){
            leftmean[i] = leftf[i]/leftn[i];
            rightmean[i] = rightf[i]/rightn[i];
            leftvar[i] = Sqrt(leftf2[i]/leftn[i] - leftmean[i]*leftmean[i])*V/2/Sqrt(leftn[i]);
            rightvar[i] = Sqrt(rightf2[i]/rightn[i] - rightmean[i]*rightmean[i])*V/2/Sqrt(rightn[i]);
            double dvar = Abs(rightvar[i] - leftvar[i]);
            if(dvar > maxvar){
                maxvar = dvar;
                dimmax = i;
            }
        }
        //dimension of maximal variance from this level is subdivided; put N - nmin on each side in weighted manner
        int Nleft = (int)((N-nmin) * Abs(leftvar[dimmax])/(Abs(leftvar[dimmax])+Abs(rightvar[dimmax])));
        int Nright = (N-nmin)-Nleft;

        double cmax = a[dimmax] + 0.5 * (b[dimmax]-a[dimmax]);
        var newa = a.copy();
        var newb = b.copy();
        newa[dimmax] = cmax;
        newb[dimmax] = cmax;

        (double resLeft, double errLeft) = strataint(f, a, newb, Nleft, nmin, diagString);
        (double resRight, double errRight) = strataint(f, newa, b, Nright, nmin, diagString);

        //result from this level is
        double integThis = (leftf[dimmax]+rightf[dimmax])/nmin * V;
        double varThis = Sqrt(Pow(rightvar[dimmax],2) + Pow(leftvar[dimmax],2));
        /*WriteLine("Start");
        WriteLine($"Borders x: {a[0]} {b[0]}");
        WriteLine($"Borders y: {a[1]} {b[1]}");
        WriteLine($"Cut along axis {dimmax}");
        WriteLine($"Points left {N}");
        WriteLine($"Nleftratio {Abs(leftvar[dimmax])/(Abs(leftvar[dimmax])+Abs(rightvar[dimmax]))}");
        WriteLine($"Dispatch Nleft {Nleft}");
        WriteLine($"Dispatch Nright {Nright}");
        WriteLine($"Left n in maxdim {leftn[dimmax]}");
        WriteLine($"Right n in maxdim {rightn[dimmax]}");
        WriteLine($"leftvarx {leftvar[0]}");
        WriteLine($"rightvarx {rightvar[0]}");
        WriteLine($"leftvary {leftvar[1]}");
        WriteLine($"rightvary {rightvar[1]}");
        WriteLine($"leftf2/leftn in dimmax {leftf2[dimmax]/leftn[dimmax]}");
        WriteLine($"leftmean^2 in dimmax {leftmean[dimmax]*leftmean[dimmax]}");
        WriteLine($"rightf2/rightn in dimmax {rightf2[dimmax]/rightn[dimmax]}");
        WriteLine($"rightmean^2 in dimmax {rightmean[dimmax]*rightmean[dimmax]}");
        WriteLine($"integ right { (rightf[dimmax])/nmin * V }");
        WriteLine($"integ left { (leftf[dimmax])/nmin * V }");
        WriteLine(integThis);
        WriteLine($"from next level we got in right {resRight} after calling with N = {Nright}");
        WriteLine($"from next level we got in left {resLeft} after calling with N = {Nleft}");
        WriteLine($"");*/

        //result from next level is
        double integNext = resLeft + resRight;
        double varNext = Sqrt(Pow(errLeft,2) + Pow(errRight,2));

        //total result is weighted by number used to calculate this level and next level:
        double res = ((N-nmin)*integNext + nmin*integThis)/(N);
        double var = 1.0/N * Sqrt(Pow( (N-nmin)*varNext ,2) + Pow( nmin*varThis ,2));

        return (res,var);
    }
}