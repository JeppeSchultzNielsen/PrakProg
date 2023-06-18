using System;
using static System.Console;
using static System.Math; 
using System.IO;

public static class main{


	public static void Main(){
		WriteLine("-----");
		WriteLine("Part a)");
		WriteLine("Unless otherwise stated, the absolute and relative accuracy goal is 1e-5.");
		WriteLine("Testing integrator on rectangular areas...");
		WriteLine("");

		Func<double, double, double> f = (x,y) => {return 1;};
		(double res, int nEvals, int boundEvals) = integrator2d.integ2Drectangle(f,-5,5,-5,5);
		WriteLine($"Integrating f(x,y)=1 with x in (-5,5) and y in (-5,5) gives result {res}, it should be 100");
		WriteLine("");

		f = (x,y) => {return x*y*y;};
		(res, nEvals, boundEvals) = integrator2d.integ2Drectangle(f,0,2,0,1);
		WriteLine($"Integrating f(x,y)=x*y*y with x in (0,2) and y in (0,1) gives result {res}, it should be (1/2*2^2)*(1/3*1^3)=2/3");
		WriteLine("");

		f = (x,y) => {return Cos(x*y);};
		(res, nEvals, boundEvals) = integrator2d.integ2Drectangle(f,-PI,PI,-PI,PI);
		WriteLine($"Integrating f(x,y)=x*y*y with x in (-PI,PI) and y in (-PI,PI) gives result {res}. According to WolframAlpha, it should be Si(pi^2)=6.659");
		WriteLine("");

		WriteLine("Since the implementation made in the homework allows infinite limits, we can also test if this works in 2d.");
		f = (x,y) => {return Exp(-x*x-y*y);};
		(res, nEvals, boundEvals) = integrator2d.integ2Drectangle(f,-double.PositiveInfinity,double.PositiveInfinity,-double.PositiveInfinity,double.PositiveInfinity);
		WriteLine($"Integrating f(x,y)=Exp(-x*x-y*y) with x in (-inf,inf) and y in (-inf,inf) gives result {res}, it should be pi.");
		WriteLine("");

		WriteLine("-----");
		WriteLine("Part b)");
		WriteLine("Testing integrator on arbitrary areas...");
		Func<double, double> u = x => {return Sqrt(1-x*x);};
		Func<double, double> d = x => {return -Sqrt(1-x*x);};
		f = (x,y) => {return 1;};
		(res, nEvals, boundEvals) = integrator2d.integ2D(f,-1,1,d,u);
		WriteLine($"Integrating f(x,y)=1 with x in (-1,1) and y in (-Sqrt(1-x*x),Sqrt(1-x*x)) (a circle of radius 1) gives result {res}");
		f = (r,v) => {return r;};
		(res, nEvals, boundEvals) = integrator2d.integ2Drectangle(f,0,1,0,2*PI);
		WriteLine($"In polar coordinates this can be restated as a rectangular integral with x->cos(v), y->sin(v), dxdy -> rdrdv, v in (0,2pi) and r in (0,1).");
		WriteLine($"This then gives {res}");
		WriteLine("");

		f = (x,y) => {return x*y;};
		(res, nEvals, boundEvals) = integrator2d.integ2D(f,-1,1,d,u);
		WriteLine($"Integrating f(x,y)=x*y with x in (-1,1) and y in (-Sqrt(1-x*x),Sqrt(1-x*x)) (a circle of radius 1) gives result {res}");
		f = (r,v) => {return r*r*r*Cos(v)*Sin(v);};
		(res, nEvals, boundEvals) = integrator2d.integ2Drectangle(f,0,1,0,2*PI);
		WriteLine($"The same integral in polar coordinates is ∫∫r²sin(v)cos(v)rdrdv. This gives {res}");
		WriteLine("");

		f = (x,y) => {return Exp(x)*Exp(y);};
		(res, nEvals, boundEvals) = integrator2d.integ2D(f,-1,1,d,u);
		WriteLine($"Integrating f(x,y)=Exp(x)*Exp(y) with x in (-1,1) and y in (-Sqrt(1-x*x),Sqrt(1-x*x)) (a circle of radius 1) gives result {res}");
		f = (r,v) => {return Exp(r*(Cos(v)+Sin(v)))*r;};
		(res, nEvals, boundEvals) = integrator2d.integ2Drectangle(f,0,1,0,2*PI);
		WriteLine($"The same integral in polar coordinates is ∫∫exp( r*(sin(v)cos(v)) )rdrdv. This gives {res}");
		WriteLine("");

		WriteLine("We can also test this with infinite boundaries on x.");
		f = (x,y) => {return Exp(-x*x-y*y);};
		u = x => {return Exp(-x*x);};
		d = x => {return -Exp(-x*x);};
		(res, nEvals, boundEvals) = integrator2d.integ2D(f,-double.PositiveInfinity,double.PositiveInfinity,d,u);
		WriteLine($"Integrating f(x,y)=Exp(-x*x-y*y) with x in (-inf,inf) and y in (-Exp(-x*x),Exp(x*x)) gives result {res}, according to WolframAlpha it is 2.03519");
		WriteLine("");

		WriteLine("-----");
		WriteLine("Part c)");
		WriteLine("The Halton-sequence integrator of homework 7 has been extended to take arguments similar to the adaptive 2d integrator.");
		WriteLine("The code is heavily inspired by that of homework 7.");
		WriteLine("Points will be sampled within the box. If they are not within the integration boundary, they are discarded.");
		WriteLine("The size of the sampling box is given by the user.");
		WriteLine("");

		WriteLine("Testing this with the integral f(x,y)=Exp(x)*Exp(y) with x in (-1,1) and y in (-Sqrt(1-x*x),Sqrt(1-x*x)) (a circle of radius 1) with 10k points");

		f = (x,y) => {return Exp(x)*Exp(y);};
		u = x => {return Sqrt(1-x*x);};
		d = x => {return -Sqrt(1-x*x);};
		(double resHalton, double errHalton, int fcalls) = integrator2d.halton2Dint(f,-1,1,-1,1,d,u,10000);
		WriteLine($"The result is {resHalton}+-{errHalton}, with {fcalls} calls to f. The boundary functions have each been evaluated 20k times (once for each Halton-series -- two are used when estimating the error).");
		WriteLine("");

		WriteLine("Now, an implementation must be made which can calculate the error.");
		WriteLine("The transformation ∫_a ^b dx ∫_d(x) ^u(x) dy f(x,y) -> ∫_a ^b dx g(x) with g(x) = ∫_d(x) ^u(x) dy f(x,y) is not a full analogy when calculating the error.");
		WriteLine("This is due to the fact that g(x) also comes with an error from the 1d-integrator.");
		WriteLine("Therefore I extend the calculation of error to include this error using standard error propagation.");
		WriteLine("I define the error in the calculation as Abs(q-Q)+Sqrt(q_err^2+Q_err^2) where Q is the higher order rule and q is the lower order rule.");
		WriteLine("This should calculate the worst case error. This changes the convergence slightly because the term Sqrt(q_err^2+Q_err^2) was ignored earlier.");
		WriteLine("");

		double err = 0;

		(res, err, nEvals, boundEvals) = integrator2d.integ2DWErr(f,-1,1,d,u);
		WriteLine($"Integrating f(x,y)=Exp(x)*Exp(y) with x in (-1,1) and y in (-Sqrt(1-x*x),Sqrt(1-x*x)) (a circle of radius 1) gives result {res}+-{err}");
		WriteLine($"Here we had 2*{nEvals} calls to f, and {boundEvals} calls to each boundary condition.");
		WriteLine("");

		f = (x,y) => {return Exp(x)*Exp(y);};
		(res, nEvals, boundEvals) = integrator2d.integ2D(f,-1,1,d,u);
		WriteLine($"With the old convergence we get result {res} after 2*{nEvals} calls to f, and {boundEvals} calls to each boundary condition.");
		WriteLine("");

		WriteLine("We are now in a position to compare the precision for a given number of function calls in Monte Carlo 2D integration and adaptive 2D integration.");
		WriteLine("To examine this, I integrate an arbitrary polynomium (2x^2+6*x^4-7)(9y+7y^3+6) over a circle of radius 1.");
		u = x => {return Sqrt(1-x*x);};
		d = x => {return -Sqrt(1-x*x);};
		f = (x,y) => {return (2*Pow(x,2)+6*Pow(x,4)-7)*(9*y+7*Pow(y,3)+6);};
		(resHalton, errHalton, fcalls) = integrator2d.halton2Dint(f,-1,1,-1,1,d,u,10000);
		(res, err, nEvals, boundEvals) = integrator2d.integ2DWErr(f,-1,1,d,u);
		WriteLine($"The adaptive integrator gives {res}+-{err} for this, and the quasi random Monte Carlo integrator gives {resHalton}+-{errHalton}. They agree within the error.");
		WriteLine("Calculating the integral with different precisions in adaptive integrator...");

		string toWrite = "";
		for(int i = 0; i < 4; i++){
			for(int j = 0; j < 5; j++){
				double precision = (2*j+1)*Pow(10,i)*1e-6;
				(res, err, nEvals, boundEvals) = integrator2d.integ2DWErr(f,-1,1,d,u,precision,precision);
				toWrite += $"{res}\t{err}\t{2*nEvals}\t{boundEvals}\t{precision}\n";
			}
		}
		File.WriteAllText("txts/adaptiveIntegrator.txt",toWrite);

		WriteLine("Calculating the integral with different number of samples in MC integrator...");

		toWrite = "";
		for(int i = 0; i < 4; i++){
			for(int j = 0; j < 5; j++){
				int N = (2*j+1)*200*(int)Pow(10,i);
				(resHalton, errHalton, fcalls) = integrator2d.halton2Dint(f,-1,1,-1,1,d,u,N);
				toWrite += $"{resHalton}\t{errHalton}\t{fcalls}\t{2*N}\t{N}\n";
			}
		}
		File.WriteAllText("txts/monteCarloIntegrator.txt",toWrite);

		WriteLine("Plots have been made comparing the convergence of the two methods in the plots directory.");


	}
}
