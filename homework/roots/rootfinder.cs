using System;
using System.Threading;
using static System.Console;
using static System.Math; 
using System.IO;

public static class rootfinder{
    private static double mag(vector v){
        double magnt = 0;
        for(int i = 0; i < v.size; i++){
            magnt += v[i]*v[i];
        }
        return Sqrt(magnt);
    }

    public static double newton(Func<double,double>f, double x, double ε=1e-2, int maxeval = (int)1e6){
        Func<vector,vector> fvec = z => new vector(f(z[0]));
        vector xvec = new vector(1);
        xvec[0] = x;
        return newton(fvec,xvec,ε, maxeval)[0];
    }

    public static vector newton(Func<vector,vector>f, vector x, double ε=1e-2, int maxeval = (int)1e6){
        int dim = x.size;
        double magfxdx = 0;
        vector fx  = f(x);
        double magfx = mag(fx);
        matrix jmatrix = new matrix(dim,dim);
        matrix R = new matrix(dim,dim);
        vector dxvector = new vector(dim);
        double lambda = 1;
        double dx = 0;
        int count = 0; 
        while(magfx > ε){
            for(int i = 0; i < dim; i++){
                for(int j = 0; j < dim; j++){
                    vector newx = x.copy();
                    dx = Abs(x[i])*Pow(2,-26);
                    newx[j] = x[j] + dx;
                    jmatrix[i,j] = (f(newx)[i] - fx[i])/dx;
                }
            }
            qr.QRGSdecomp(jmatrix,R);
            dxvector = qr.QRGSsolve(jmatrix,R,-fx);



            lambda = 1; 
            magfxdx = mag(f(x + lambda * dxvector));
            while(magfxdx > (1-lambda/2.0)*magfx && lambda > 1.0/32){
                lambda = lambda/2.0;
                magfxdx = mag(f(x + lambda * dxvector));
            }

            x = x + lambda * dxvector;
            fx = f(x);
            magfx = mag(fx);
            if(count > maxeval){
                WriteLine($"Max number of evaluations reached, magfx = {magfx}");
                break;
            }
            count++;
        }
        //WriteLine(count);
        return x; 
    }
}