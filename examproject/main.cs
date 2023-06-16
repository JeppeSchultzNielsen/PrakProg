using System;
using static System.Console;
using static System.Math; 
using System.IO;

public static class main{


	public static void Main(){
		Func<double, double, double> f = (x,y) => {return 1;};
		Func<double, double> u = x => {return Sqrt(1-x*x);};
		Func<double, double> d = x => {return -Sqrt(1-x*x);};
		integrator2d.integ2D(f,1,1,u,d);
	}
}
