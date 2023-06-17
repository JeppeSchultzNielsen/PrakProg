using System;
using static System.Math; 
using static System.Console;

public static class integrator2d{
    

    public static (double,int,int) integ2Drectangle(
        Func<double,double,double> f,
        double a, double b,
        double c, double d,
        double acc =1e-5, double eps=1e-5)
        {
        int nEvals = 0;
        int nBoundEvals = 0;
        Func<double,double> g = x => {
            Func<double,double> integHere = y => {
                return f(x,y);
            };
            (double resG, double errG) = integrator.integrateGeneral(integHere,c,d,acc,eps);
            nEvals += integrator.nEvals;
            nBoundEvals++;
            return resG;
        };
        (double res, double err) = integrator.integrateGeneral(g,a,b,acc,eps);
        
        return (res, nEvals, nBoundEvals);
    }

    public static (double,int,int) integ2D(
        Func<double,double,double> f,
        double a, double b,
        Func<double,double> d,
        Func<double,double> u,
        double acc =1e-5, double eps=1e-5)
        {
        int nEvals = 0;
        int nBoundEvals = 0;
        Func<double,double> g = x => {
            Func<double,double> integHere = y => {
                return f(x,y);
            };
            (double resG, double errG) = integrator.integrateGeneral(integHere,d(x),u(x),acc,eps);
            nEvals += integrator.nEvals;
            nBoundEvals++;
            return resG;
        };
        (double res, double err) = integrator.integrateGeneral(g,a,b,acc,eps);
        return (res, nEvals, nBoundEvals);
    }

    public static (double,double,int) halton2Dint(Func<double,double,double> f,
        double a, double b,
        double ymin, double ymax, 
        Func<double,double> d,
        Func<double,double> u,
        int N)
        {
        int dim=2; double V=(b-a)*(ymax-ymin); 

        double sum=0,sum2=0;

	    var x=new vector(dim);
        var x2=new vector(dim);

        int fcalls1 = 0;
        int fcalls2 = 0;

        for(int i=0;i<N;i++){
            vector n1 = halton(i,dim);
            vector n2 = halton(i,dim,true);

            x[0]=a+n1[0]*(b-a);
            x[1]=ymin+n1[1]*(ymax-ymin);

            x2[0]=a+n2[0]*(b-a);
            x2[1]=ymin+n2[1]*(ymax-ymin);

            if(x[1] > d(x[0]) &&  x[1] < u(x[0])){
                sum+=f(x[0],x[1]); 
                fcalls1++;
            }
            if(x2[1] > d(x[0]) &&  x2[1] < u(x[0])){
                sum2+=f(x2[0],x2[1]); 
                fcalls2++;
                
            }
        }

        var result=(sum*V/N,Abs(sum*V/N-sum2*V/N),fcalls1+fcalls2);
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
}