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


    public static (double,double) vdcint(Func<vector,double> f,vector a,vector b,int N){
        int dim=a.size; double V=1; 
        for(int i=0;i<dim;i++)V*=b[i]-a[i];

        double sum=0,sum2=0;

	    var x=new vector(dim);
        var x2=new vector(dim);

        for(int i=0;i<N;i++){
            for(int k=0;k<dim;k++)x[k]=a[k]+corput(i,N)*(b[k]-a[k]);
            for(int k=0;k<dim;k++)x2[k]=a[k]+corput(i,N)*(b[k]-a[k]);
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

    private static double halton(int n, int d){
        
    }
}