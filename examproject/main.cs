using System;
using static System.Console;
using static System.Math; 
using System.IO;

public static class main{


	public static void Main(){
		WriteLine("-----");
		WriteLine("Part a)");
		WriteLine("Unless otherwise stated, the absolute and relative accuracy goal is 1e-5.");
		WriteLine("Testing integrator...");
		WriteLine("");

		Func<double, double, double> f = (x,y) => {return 1;};
		double res = integrator2d.integ2Drectangle(f,-5,5,-5,5);
		WriteLine($"Integrating f(x,y)=1 with x in (-5,5) and y in (-5,5) gives result {res}, it should be 100");
		WriteLine("");

		f = (x,y) => {return x*y*y;};
		res = integrator2d.integ2Drectangle(f,0,2,0,1);
		WriteLine($"Integrating f(x,y)=x*y*y with x in (0,2) and y in (0,1) gives result {res}, it should be (1/2*2^2)*(1/3*1^3)=2/3");
		WriteLine("");

		f = (x,y) => {return Cos(x*y);};
		res = integrator2d.integ2Drectangle(f,-PI,PI,-PI,PI);
		WriteLine($"Integrating f(x,y)=x*y*y with x in (-PI,PI) and y in (-PI,PI) gives result {res}. According to WolframAlpha, it should be Si(pi^2)=6.659");
		WriteLine("");

		WriteLine("Since the implementation made in the homework allows infinite limits, we can also test if this works in 2d.");
		f = (x,y) => {return Exp(-x*x-y*y);};
		res = integrator2d.integ2Drectangle(f,-double.PositiveInfinity,double.PositiveInfinity,-double.PositiveInfinity,double.PositiveInfinity);
		WriteLine($"Integrating f(x,y)=Exp(-x*x-y*y) with x in (-inf,inf) and y in (-inf,inf) gives result {res}, it should be pi.");
		WriteLine("");

		WriteLine("-----");
		WriteLine("Part b)");
		WriteLine("Testing integrator...");
		Func<double, double> u = x => {return Sqrt(1-x*x);};
		Func<double, double> d = x => {return -Sqrt(1-x*x);};
		f = (x,y) => {return 1;};
		res = integrator2d.integ2D(f,-1,1,d,u);
		WriteLine($"Integrating f(x,y)=1 with x in (-1,1) and y in (-Sqrt(1-x*x),Sqrt(1-x*x)) (a circle of radius 1) gives result {res}");
		f = (r,v) => {return r;};
		res = integrator2d.integ2Drectangle(f,0,1,0,2*PI);
		WriteLine($"In polar coordinates this can be restated as a rectangular integral with x->cos(v), y->sin(v), dxdy -> rdrdv, v in (0,2pi) and r in (0,1).");
		WriteLine($"This then gives {res}");
		WriteLine("");

		f = (x,y) => {return x*y;};
		res = integrator2d.integ2D(f,-1,1,d,u);
		WriteLine($"Integrating f(x,y)=x*y with x in (-1,1) and y in (-Sqrt(1-x*x),Sqrt(1-x*x)) (a circle of radius 1) gives result {res}");
		f = (r,v) => {return r*r*r*Cos(v)*Sin(v);};
		res = integrator2d.integ2Drectangle(f,0,1,0,2*PI);
		WriteLine($"The same integral in polar coordinates is ∫∫r²sin(v)cos(v)rdrdv. This gives {res}");
		WriteLine("");

		f = (x,y) => {return Exp(x)*Exp(y);};
		res = integrator2d.integ2D(f,-1,1,d,u);
		WriteLine($"Integrating f(x,y)=Exp(x)*Exp(y) with x in (-1,1) and y in (-Sqrt(1-x*x),Sqrt(1-x*x)) (a circle of radius 1) gives result {res}");
		f = (r,v) => {return Exp(r*(Cos(v)+Sin(v)))*r;};
		res = integrator2d.integ2Drectangle(f,0,1,0,2*PI);
		WriteLine($"The same integral in polar coordinates is ∫∫exp( r*(sin(v)cos(v)) )rdrdv. This gives {res}");
		WriteLine("");

		WriteLine("We can also test this with infinite boundaries on x.");
		f = (x,y) => {return Exp(-x*x-y*y);};
		u = x => {return Exp(-x*x);};
		d = x => {return -Exp(-x*x);};
		res = integrator2d.integ2D(f,-double.PositiveInfinity,double.PositiveInfinity,d,u);
		WriteLine($"Integrating f(x,y)=Exp(-x*x-y*y) with x in (-inf,inf) and y in (-Exp(-x*x),Exp(x*x)) gives result {res}, according to WolframAlpha it is 2.03519");
		WriteLine("");

		WriteLine("-----");
		WriteLine("Part c)");
		WriteLine("The Halton-sequence integrator of homework 7 has been extended to take arguments similar to the adaptive 2d integrator.");
		WriteLine("The code is heavily inspired by that of homework 7.");
		WriteLine("The size of the sampling box is given by the user.");
		WriteLine("Points will be sampled within the box. If they are not within the integration boundary, they are discarded.");
		WriteLine("Testing this with the integral f(x,y)=Exp(x)*Exp(y) with x in (-1,1) and y in (-Sqrt(1-x*x),Sqrt(1-x*x)) (a circle of radius 1) with 10k points");

		f = (r,v) => {return Exp(r*(Cos(v)+Sin(v)))*r;};
		u = x => {return Sqrt(1-x*x);};
		d = x => {return -Sqrt(1-x*x);};
		(double resHalton, double errHalton, int fcalls) = integrator2d.halton2Dint(f,-1,1,-1,1,d,u,10000);
		WriteLine($"The result is {resHalton}+-{errHalton}, with {fcalls} calls to f. The boundaries functions have each been evaluated 20k times.");
	}
}
