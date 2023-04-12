using System;
using System.Threading;
using static System.Console;
using static System.Math; 

public static class mcint{
    public static Random rnd = new Random();

    public static (double,double) plainmc(Func<vector,double> f,vector a,vector b,int N){
        int dim=a.size; double V=1; 
        for(int i=0;i<dim;i++)V*=b[i]-a[i];

        double sum=0,sum2=0;

	    var x=new vector(dim);

        for(int i=0;i<N;i++){
            for(int k=0;k<dim;k++)x[k]=a[k]+rnd.NextDouble()*(b[k]-a[k]);
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

    public static (double,double) stratifiedint(Func<vector,double> f,vector a,vector b,int N, int nmin){
        if(N <= nmin){
            WriteLine("nmin call");
            return plainmc(f,a,b,N);
        }
        else{
            //calculate, for each dimension, the variance in two halves of its area. Select the highest variance.
            int dimmax = 0;
            double errmax = 0;
            double lowerErr = 0;
            double upperErr = 0;
            double cmax = 0;
            int dim = a.size;
            for(int i = 0; i < dim; i++){
                double olda = a[i];
                double oldb = b[i];
                double newc = a[i] + 0.5 * (b[i]-a[i]);
                //first calculate with upper subvolume of this dimension
                a[i] = newc;
                (double res1, double err1) = plainmc(f,a,b,nmin/2);
                //reset a, calculate with lower subvolume
                a[i] = olda;
                b[i] = newc;
                (double res2, double err2) = plainmc(f,a,b,nmin/2);
                //reset b
                b[i] = oldb;
                if(err1 > errmax){
                    errmax = err1;
                    dimmax = i;
                    cmax = newc;
                    upperErr = err1;
                    lowerErr = err2;
                }
                if(err2 > errmax){
                    errmax = err2;
                    dimmax = i;
                    cmax = newc;
                    upperErr = err1;
                    lowerErr = err2;
                }
            }
            //I now know the dimension which dimension has volume with largest sub-variance. Split points weighted among volumes
            WriteLine(upperErr);
            WriteLine(lowerErr);
            int Nupper = (int)Round(N * upperErr/(lowerErr+upperErr));
            int Nlower = N-Nupper;
            WriteLine(Nupper);
            WriteLine(Nlower);
            WriteLine(N);
            //calculate mean and variance in sub-volumes
            double oldamax = a[dimmax];
            double oldbmax = b[dimmax];
            a[dimmax] = cmax;
            var result1 = stratifiedint(f, a, b, Nupper, nmin);
            a[dimmax] = oldamax;
            b[dimmax] = cmax;
            var result2 = stratifiedint(f, a, b, Nlower, nmin);
            WriteLine("Results:");
            WriteLine(result1.Item1);
            WriteLine(result2.Item1);
            b[dimmax] = oldbmax;
            double mean = result1.Item1 + result2.Item1, sigma = Sqrt(result1.Item2 * result1.Item2 + result2.Item2 * result2.Item2);
            return (mean, sigma);
        }
    }
}