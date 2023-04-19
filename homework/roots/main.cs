using System;
using System.Threading;
using static System.Console;
using static System.Math; 
using System.IO;
using System.Collections.Generic;

public static class main{
	public static vector poly2(vector x){
		return new vector(x[0]*x[0],x[1]*x[1]);
	}

	public static vector rosenbrockGrad(vector x){
		//(1-x)2+100(y-x2)2
		return new vector(-2*(1-x[0]*x[0])-200*(x[1]-x[0]*x[0])*2*x[0],100*(x[1]-x[0]*x[0]));
	}

	public static double poly3(double x){
		return 4*Pow(x,3)+5*Pow(x,2)+6*Pow(x,1)+7;
	}

	public static double Me(double E, double rmin = 0.001, double rmax = 8, double absacc = 0.01, double epsacc = 0.01, List<double> rs = null, List<vector> ys = null){
			Func<double,vector,vector> schr  = (x,y) => {
				vector dydr = new vector(2);
				// diff eq. is f'' = - 2 *(E+1/r)f. If y_1 = f', y_0 = f this gives
				// y_0' = y_1
				// y_1' = -2*(E+1/r)*y_0
				dydr[0] = y[1];
				dydr[1] = -2*(E+1/x)*y[0];
				return dydr;
			};

			//initial conditions; at r_min, y0 = f(rmin) = r_min-r_min^2, y1 = f' = 1-2*r_min
			vector init = new vector(2);
			init[0] = rmin - rmin*rmin;
			init[1] = 1-2*rmin;
			(List<double> xmax, List<vector> ymax) = odesolver.driver(schr, rmin, init, rmax, 0.01, absacc, epsacc, rs, ys);
			return ymax[0][0]; //final point in the solution (at rmax), this is what should be zero if we find correct E. 
	}

	public static void Main(){
		vector x0 = rootfinder.newton(poly2,new vector(7,7),1e-4);
		vector val = poly2(x0);
		WriteLine("Exercise A:");
		WriteLine($"Root of f(x,y) -> (x^2,y^2) is ({x0[0]},{x0[1]}), which gives ({val[0]},{val[1]})");
		x0 = rootfinder.newton(rosenbrockGrad,new vector(1.1,2),1e-2);
		val = rosenbrockGrad(x0);
		WriteLine($"Root of grad(Rosenbrock) is ({x0[0]},{x0[1]}), which gives ({val[0]},{val[1]})");
		WriteLine("Test of overriding function for 1d root finding");
		double xval = rootfinder.newton(poly3,0,1e-4);
		WriteLine($"Root of 4*x^3+5*x^2+6*x+7 is {xval}, where the function is {poly3(xval)}");
		WriteLine("");
		WriteLine("Part b)");
		Func<double, double> toMin = x => Me(x,0.001,8, 0.01, 0.01);
		double eval = rootfinder.newton(toMin,-1,1e-4);
		WriteLine($"By shooting method, binding energy of lowest bound S-electron in H is {eval} Hartree. Expected value is 0.5");

		List<double> rs = new List<double>();
		List<vector> ys = new List<vector>();
		Me(eval,0.001,8, 0.01, 0.01, rs, ys);
		string toWrite = "";
		for(int i = 0; i < rs.Count; i++){
			toWrite += $"{rs[i]}\t{ys[i][0]}\t{rs[i]*Exp(-rs[i])}\n";
		}
		File.WriteAllText("txts/wavefunctions.txt",toWrite);
		WriteLine($"The wavefunction corresponding to this eigenenergy is plotted on Wavefunctions.svg, along with the analytical solution. They only differ at the very end.");

		//Investigate the convergence of your solution towards the exact result with respect to the rmax and rmin parameters (separately) as well as with respect to the parameters acc and eps of your ODE integrator.
		toWrite = "";
		for(int i = 0; i < 100; i++){
			double rmin = 0.0001*(i+1);
			toMin = x => Me(x,rmin,8, 0.01, 0.01);
			eval = rootfinder.newton(toMin,-1,1e-4);
			toWrite += $"{rmin}\t{Abs(eval+0.5)/0.5}\n";
		}
		File.WriteAllText("txts/rminconvergence.txt",toWrite);

		toWrite = "";
		for(int i = 0; i < 100; i++){
			double rmax = 6+0.04*(i);
			toMin = x => Me(x,0.01,rmax, 0.01, 0.01);
			eval = rootfinder.newton(toMin,-1,1e-4);
			toWrite += $"{rmax}\t{Abs(eval+0.5)/0.5}\n";
		}
		File.WriteAllText("txts/rmaxconvergence.txt",toWrite);

		toWrite = "";
		for(int i = 0; i < 100; i++){
			double absacc = 0.0001*(i+1);
			toMin = x => Me(x,0.01,8, absacc, 0.01);
			eval = rootfinder.newton(toMin,-1,1e-4);
			toWrite += $"{absacc}\t{Abs(eval+0.5)/0.5}\n";
		}
		File.WriteAllText("txts/absaccconvergence.txt",toWrite);

		toWrite = "";
		for(int i = 0; i < 100; i++){
			double epsacc = 0.0001*(i+1);
			toMin = x => Me(x,0.01,8, 0.01, epsacc);
			eval = rootfinder.newton(toMin,-1,1e-4);
			toWrite += $"{epsacc}\t{Abs(eval+0.5)/0.5}\n";
		}
		File.WriteAllText("txts/epsaccconvergence.txt",toWrite);
	}
}
