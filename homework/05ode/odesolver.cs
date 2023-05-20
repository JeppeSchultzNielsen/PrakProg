using System;
using System.Threading;
using static System.Console;
using static System.Math; 

public static class odesolver{
    public static (vector,vector) rkstep12(
        Func<double,vector,vector> f, /* the f from dy/dx=f(x,y) */
        double x,                    /* the current value of the variable */
        vector y,                    /* the current value y(x) of the sought function */
        double h                     /* the step to be taken */
    ){
        vector k0 = f(x,y);              /* embedded lower order formula (Euler) */
        vector k1 = f(x+h/2,y+k0*(h/2)); /* higher order formula (midpoint) */
        vector yh = y+k1*h;              /* y(x+h) estimate */
        vector er = (k1-k0)*h;           /* error estimate */
        return (yh,er);
    }

    public static (genlist<double>,genlist<vector>) driver(
        Func<double,vector,vector> f, /* the f from dy/dx=f(x,y) */
        double a,                    /* the start-point a */
        vector ya,                   /* y(a) */
        double b,                    /* the end-point of the integration */
        double h=0.01,               /* initial step-size */
        double acc=0.01,             /* absolute accuracy goal */
        double eps=0.01,              /* relative accuracy goal */
        genlist<double> xlist=null, 
        genlist<vector> ylist=null
    ){
        if(a>b) throw new ArgumentException("driver: a>b");
        double x=a; vector y=ya.copy();
        if(!(xlist == null || ylist == null)){
            xlist.add(x);
            ylist.add(y);
            do{
                if(x>=b) return (xlist,ylist); /* job done */
                if(x+h>b) h=b-x;               /* last step should end at b */
                var (yh,erv) = rkstep12(f,x,y,h);
                vector tol = new vector(yh.size);
                for(int i=0;i<yh.size;i++){
                    tol[i]=Max(acc,Abs(yh[i])*eps)*Sqrt(h/(b-a));
                }
                bool ok=true;
                for(int i=0;i<yh.size;i++){
                    if(! (erv[i]<tol[i])) ok=false;
                }
                if(ok){ // accept step
                    x+=h; y=yh;
                    xlist.add(x);
                    ylist.add(y);
                }
                double factor = tol[0]/Abs(erv[0]);
                for(int i=1;i<y.size;i++){
                    factor=Min(factor,tol[i]/Abs(erv[i]));
                }
                h *= Min( Pow(factor,0.25)*0.95 ,2); // readjust stepsize
            }while(true);
        }
        else{
            do{
                if(x>=b){
                    xlist=new genlist<double>();
		            ylist=new genlist<vector>();
                    xlist.add(x);
                    ylist.add(y);
                    return (xlist,ylist);/* job done */
                }
                if(x+h>b) h=b-x;               /* last step should end at b */
                var (yh,erv) = rkstep12(f,x,y,h);
                vector tol = new vector(yh.size);
                for(int i=0;i<yh.size;i++){
                    tol[i]=Max(acc,Abs(yh[i])*eps)*Sqrt(h/(b-a));
                }
                bool ok=true;
                for(int i=0;i<yh.size;i++){
                    if(! (erv[i]<tol[i])) ok=false;
                }
                if(ok){ // accept step
                    x+=h; y=yh;
                }
                double factor = tol[0]/Abs(erv[0]);
                for(int i=1;i<y.size;i++){
                    factor=Min(factor,tol[i]/Abs(erv[i]));
                }
                h *= Min( Pow(factor,0.25)*0.95 ,2); // readjust stepsize
            }while(true);
        }
    }//driver

}