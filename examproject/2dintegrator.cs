using System;
using static System.Math; 
using static System.Console;
using static System.Double;

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

    public static (double,double,int,int) integ2DWErr(
        Func<double,double,double> f,
        double a, double b,
        Func<double,double> d,
        Func<double,double> u,
        double acc =1e-5, double eps=1e-5)
        {
        int nEvals = 0;
        int nBoundEvals = 0;
        Func<double,(double,double)> g = x => {
            Func<double,double> integHere = y => {
                return f(x,y);
            };
            (double resG, double errG) = integrator.integrateGeneral(integHere,d(x),u(x),acc,eps);
            nEvals += integrator.nEvals;
            nBoundEvals++;
            return (resG,errG);
        };
        (double res, double err) = recursiveIntegratorWErr(g,a,b,acc,eps);
        return (res, err, nEvals, nBoundEvals);
    }

    private static (double,double) recursiveIntegratorWErr(Func<double,(double,double)> f, double a, double b,
    double δ=1e-3, double ε=1e-3, double res2=NaN, double res3=NaN, double err2 = NaN, double err3 = NaN)
    {
        double h=b-a;
        if(IsNaN(res2)){
            var f2 = f(a+2*h/6);
            var f3 = f(a+4*h/6);
            res2 = f2.Item1;
            err2 = f2.Item2;
            res3 = f3.Item1;
            err3 = f3.Item2;
        } // first call, no points to reuse
        var f1 = f(a+h/6);
        double res1 = f1.Item1;
        double err1 = f1.Item2; 
        var f4 = f(a+5*h/6);
        double res4 = f4.Item1;
        double err4 = f4.Item2;
        double Q = (2*res1+res2+res3+2*res4)/6*(b-a); // higher order rule 
        double q = (res1+res2+res3+res4)/4*(b-a); // lower order rule
        double Qerr = 1.0/6.0*(b-a)*Sqrt( Pow(2*err1,2) + Pow(err2,2) + Pow(err3,2) + Pow(2*err4,2) );
        double qerr = 1.0/4.0*(b-a)*Sqrt( Pow(err1,2) + Pow(err2,2) + Pow(err3,2) + Pow(err4,2) );
        double diff = Abs(Q-q);
        double errInDiff = Abs(Sqrt(Qerr*Qerr + qerr*qerr));
        double err = diff + errInDiff;
        if(err <= δ+ε*Abs(Q)) return (Q,err);
        else{
            var (resLeft, errLeft) = recursiveIntegratorWErr(f,a,(a+b)/2,δ/Sqrt(2),ε,res1,res2,err1,err2);
            var (resRight, errRight) = recursiveIntegratorWErr(f,(a+b)/2,b,δ/Sqrt(2),ε,res3,res4,err3,err4);
            return (resLeft+resRight,errLeft + errRight);
        }
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