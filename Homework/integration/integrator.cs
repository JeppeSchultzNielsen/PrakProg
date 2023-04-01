using System;
using System.Threading;
using static System.Console;
using static System.Math; 
using System.IO;
using static System.Double;

public static class integrator{

    public static int nEvals = 0; //stores the number of calls to integrate function. 

    public static double integrate(Func<double,double> f, double a, double b,
    double δ=1e-3, double ε=1e-3, double f2=NaN, double f3=NaN)
    {
        double h=b-a;
        if(IsNaN(f2)){ f2=f(a+2*h/6); f3=f(a+4*h/6); nEvals = 0;} // first call, no points to reuse
        double f1=f(a+h/6), f4=f(a+5*h/6);
        double Q = (2*f1+f2+f3+2*f4)/6*(b-a); // higher order rule
        double q = (f1+f2+f3+f4)/4*(b-a); // lower order rule
        double err = Abs(Q-q);
        nEvals += 1;
        if (err <= δ+ε*Abs(Q)) return Q;
        else return integrate(f,a,(a+b)/2,δ/Sqrt(2),ε,f1,f2)+
                    integrate(f,(a+b)/2,b,δ/Sqrt(2),ε,f3,f4);
    }

    public static double integrateCC(Func<double,double> f, double a, double b,
    double δ=1e-3, double ε=1e-3){
        Func<double,double> newFunc = x => f( (a+b)/2+(b-a)/2*Cos(x)  )*Sin(x)*(b-a)/2;
        return integrate(newFunc,0,PI,δ,ε);
    }
}