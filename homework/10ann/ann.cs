using System;
using System.Threading;
using static System.Console;
using static System.Math; 
using System.IO;

public class ann{
    public int n; /* number of hidden neurons */
    public Func<double,double> f = x => x*Exp(-x*x); /* activation function */
    public Func<double,double> deriv1 = x => (1-2*x*x)*Exp(-x*x); /* derivative of activation function */
    public Func<double,double> deriv2 = x => Exp(-x*x)*(4*x*x*x-6*x); /* 2nd derivative of activation function */
    public vector p; /* network parameters */
    public ann(int n){
        this.n = n; 
        int numParams = 3*n; //n a_i, n b_i, n w's
        p = new vector(numParams);
    }
    public double response(double x){
        double result = 0;
        for(int i = 0; i < n; i++){
            double a_i = p[i];
            double b_i = p[n+i];
            double w_i = p[2*n+i];
            result += w_i * f( (x-a_i)/b_i );
        }
        return result;
    }

    public double antiderivative(double x){
        //antiderivative of gaussian wavelet (x-a)/b * exp( ((x-a)/b)^2 ) is -b/2 * exp( ((x-a)/b)^2 )
        double result = 0;
        for(int i = 0; i < n; i++){
            double a_i = p[i];
            double b_i = p[n+i];
            double w_i = p[2*n+i];
            result += -w_i*b_i/2.0 * Exp( -Pow((x-a_i)/b_i,2) );
        }
        return result;
    }

    public double integral(double a, double b){
        return antiderivative(b) - antiderivative(a);
    }


    public double derivative(double x){
        //derivative of gaussian wavelet (x-a)/b * exp( ((x-a)/b)^2 ) is (2*(a-x)^2+b^2)/b^3 * exp( ((x-a)/b)^2 )
        double result = 0;
        for(int i = 0; i < n; i++){
            double a_i = p[i];
            double b_i = p[n+i];
            double w_i = p[2*n+i];
            result += w_i/b_i*deriv1( (x-a_i)/b_i ) ;
        }
        return result;
    }

    public double derivative2(double x){
        //derivative of gaussian wavelet (x-a)/b * exp( ((x-a)/b)^2 ) is 2*(a-x)*(2*(a-x)^2-3*b^2)/b^3 * exp( ((x-a)/b)^2 )
        double result = 0;
        for(int i = 0; i < n; i++){
            double a_i = p[i];
            double b_i = p[n+i];
            double w_i = p[2*n+i];
            result += w_i/b_i/b_i*deriv2( (x-a_i)/b_i ) ;
        }
        return result;
    }

    public void train(vector x,vector y){
        double minx = double.MaxValue;
        double maxx = double.MinValue;
        for(int i = 0; i < x.size; i++){
            if(x[i] > maxx) maxx = x[i];
            if(x[i] < minx) minx = x[i];
        }


        for(int i = 0; i < p.size; i++){
            //up to n is the a_i, they should be placed evenly across the interval. 
            if(i < n) p[i] = minx + (maxx-minx)*i/n;
            else p[i] = 1; 
        }


        Func<vector,double> c = ps => {
            double toMin = 0; 
            for(int j = 0; j < x.size; j++){
                double result = 0; 
                for(int i = 0; i < n; i++){
                    double a_i = ps[i];
                    double b_i = ps[n+i];
                    double w_i = ps[2*n+i];
                    result += w_i * f( (x[j]-a_i)/b_i );
                }
                toMin += Pow(result-y[j],2);
            }
            return toMin/n;
        };
        p = minimizer.qnewton(c,p,0.001);
    }
    public void trainDiffEq(Func<vector,double> phi, double a, double b, double x0, double y0, double y0p, double alpha = 1, double beta = 1){
        for(int i = 0; i < p.size; i++){
            //up to n is the a_i, they should be placed evenly across the interval. 
            if(i < n) p[i] = a + (b-a)*i/n;
            else p[i] = 1; 
        }

        Func<vector,double> c = ps => {            
            Func<double,double> Fp = x => {
                double result = 0;
                for(int i = 0; i < n; i++){
                    double a_i = ps[i];
                    double b_i = ps[n+i];
                    double w_i = ps[2*n+i];
                    result += w_i * f( (x-a_i)/b_i );
                }
                return result;
            };

            Func<double,double> Fpprime = x => {
                double result = 0;
                for(int i = 0; i < n; i++){
                    double a_i = ps[i];
                    double b_i = ps[n+i];
                    double w_i = ps[2*n+i];
                    result += w_i/b_i*deriv1( (x-a_i)/b_i ) ;
                }
                return result;
            };

            Func<double,double> Fp2prime = x => {
                double result = 0;
                for(int i = 0; i < n; i++){
                    double a_i = ps[i];
                    double b_i = ps[n+i];
                    double w_i = ps[2*n+i];
                    result += w_i/b_i/b_i*deriv2( (x-a_i)/b_i ) ;
                }
                return result;
            };

            Func<double,double> toIntegrate = x => {
                vector d = new vector(4);
                d[0] = x;
                d[1] = Fp(x);
                d[2] = Fpprime(x);
                d[3] = Fp2prime(x);
                return phi(d)*phi(d);
            };
            double toMin = 0; 
            toMin += integrator.integrate(toIntegrate,a,b,1e-4,1e-4);
            toMin += alpha*(Fp(x0)-y0)*(Fp(x0)-y0);
            toMin += beta*(Fpprime(x0)-y0p)*(Fpprime(x0)-y0p);

            return toMin;
        };
        p = minimizer.qnewton(c,p,0.01);
    }
}