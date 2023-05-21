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

	public static double Me(double E, double rmin = 0.001, double rmax = 8, double absacc = 0.01, double epsacc = 0.01, genlist<double> rs = null, genlist<vector> ys = null){
			Func<double,vector,vector> schr  = (x,y) => {
				vector dydr = new vector(2);
				// diff eq. is f'' = - 2 *(E+1/r)f. If y_1 = f', y_0 = f this gives
				// y_0' = y_1
				// y_1' = -2*(E+1/r)*y_0
				dydr[0] = y[1];
				dydr[1] = -2.0*(E+1.0/x)*y[0];
				return dydr;
			};

			//initial conditions; at r_min, y0 = f(rmin) = r_min-r_min^2, y1 = f' = 1-2*r_min
			vector init = new vector(2);
			init[0] = rmin - rmin*rmin;
			init[1] = 1.0-2.0*rmin;
			(genlist<double> xmax, genlist<vector> ymax) = odesolver.driver(schr, rmin, init, rmax, 0.01, absacc, epsacc, rs, ys);
			return ymax[0][0]; //final point in the solution (at rmax), this is what should be zero if we find correct E. 
	}

	public static double MeImproved(double E, double rmin = 0.001, double rmax = 8, double absacc = 1e-5, double epsacc = 1e-5, genlist<double> rs = null, genlist<vector> ys = null){
			Func<double,vector,vector> schr  = (x,y) => {
				vector dydr = new vector(2);
				// diff eq. is f'' = - 2 *(E+1/r)f. If y_1 = f', y_0 = f this gives
				// y_0' = y_1
				// y_1' = -2*(E+1/r)*y_0
				dydr[0] = y[1];
				dydr[1] = -2.0*(E+1.0/x)*y[0];
				return dydr;
			};

			//initial conditions; at r_min, y0 = f(rmin) = r_min-r_min^2, y1 = f' = 1-2*r_min
			vector init = new vector(2);
			init[0] = rmin - rmin*rmin;
			init[1] = 1.0-2.0*rmin;
			(genlist<double> xmax, genlist<vector> ymax) = odesolver.driver(schr, rmin, init, rmax, 0.0001, absacc, epsacc, rs, ys);
			double f_rmax = ymax[0][0]; //final point in the solution (at rmax)
			double dfdr_rmax = ymax[0][1]; //1st derivative at final point in the solution (at rmax)
			double k = Sqrt(2.0*Abs(E));
			//matching the derivative with the analytic solution, we should have dfdr_rmax = e^(-k * rmax)-rmax*k*e^(-k * rmax)
			//=> dfdr_rmax*rmax - f_rmax + rmax*k*f_rmax = 0
			return f_rmax - Exp(-k*rmax)*rmax;//dfdr_rmax + rmax*k*f_rmax - f_rmax; f_rmax - Exp(-k*rmax)*rmax
	}

	public static void Main(){
		vector x0 = rootfinder.newton(poly2,new vector(7,7),1e-4);
		vector val = poly2(x0);
		WriteLine("------");
		WriteLine("Part a)");
		WriteLine("Testing Newton's method for some functions.");
		WriteLine($"Root of f(x,y) -> (x^2,y^2) is ({x0[0]},{x0[1]}) to a precision of 1e-4, where the function evaluates as ({val[0]},{val[1]})");
		x0 = rootfinder.newton(rosenbrockGrad,new vector(1.1,2),1e-4);
		val = rosenbrockGrad(x0);
		WriteLine($"Root of grad(Rosenbrock) is ({x0[0]},{x0[1]}) to a precision of 1e-4, where the function evaluates as ({val[0]},{val[1]})");
		WriteLine("Test of overriding function for 1d root finding:");
		double xval = rootfinder.newton(poly3,0,1e-4);
		WriteLine($"Root of 4*x^3+5*x^2+6*x+7 is {xval} to a precision of 1e-4, where the function evaluates as is {poly3(xval)}");
		WriteLine("------");
		WriteLine("Part b)");
		Func<double, double> toMin = x => Me(x,0.001,8, 1e-5, 1e-5);
		double eval = rootfinder.newton(toMin,-1,1e-4);
		WriteLine($"By shooting method, binding energy of lowest bound S-electron in H is {eval} Hartree. Expected value is 0.5");

		genlist<double> rs = new genlist<double>();
		genlist<vector> ys = new genlist<vector>();
		Me(eval,0.001,8, 0.01, 0.01, rs, ys);
		string toWrite = "";
		for(int i = 0; i < rs.size; i++){
			toWrite += $"{rs[i]}\t{ys[i][0]}\t{rs[i]*Exp(-rs[i])}\n";
		}
		File.WriteAllText("txts/wavefunctions.txt",toWrite);
		WriteLine($"The wavefunction corresponding to this eigenenergy is plotted on Wavefunctions.svg, along with the analytical solution. They only differ at the very end.");
		WriteLine($"The convergence to the ground state energy with respect to different parameters has been plotted.");
		//Investigate the convergence of your solution towards the exact result with respect to the rmax and rmin parameters (separately) as well as with respect to the parameters acc and eps of your ODE integrator.
		toWrite = "";
		WriteLine("Varying rmin...");
		for(int i = 0; i < 1000; i++){
			double rmin = 0.0001*(i+1);
			toMin = x => Me(x,rmin,8);
			eval = rootfinder.newton(toMin,-1,1e-4);
			toWrite += $"{rmin}\t{Abs(eval+0.5)/0.5}\n";
		}
		File.WriteAllText("txts/rminconvergence.txt",toWrite);
		WriteLine("Varying rmax...");
		toWrite = "";
		for(int i = 0; i < 10; i++){
			double rmax = 2+2*(i);
			toMin = x => Me(x,0.01,rmax);
			eval = rootfinder.newton(toMin,-1,1e-4);
			toWrite += $"{rmax}\t{Abs(eval+0.5)/0.5}\n";
		}
		File.WriteAllText("txts/rmaxconvergence.txt",toWrite);
		WriteLine("Varying abs accuracy...");
		toWrite = "";
		for(int i = 0; i < 10; i++){
			double absacc = 1e-5*(i+1);
			toMin = x => Me(x,0.01,8, absacc, 1e-5);
			eval = rootfinder.newton(toMin,-1,1e-4);
			toWrite += $"{absacc}\t{Abs(eval+0.5)/0.5}\n";
		}
		File.WriteAllText("txts/absaccconvergence.txt",toWrite);
		WriteLine("Varying relative accuracy...");
		toWrite = "";
		for(int i = 0; i < 10; i++){
			double epsacc = 1e-5*(i+1);
			toMin = x => Me(x,0.01,8, 1e-5, epsacc);
			eval = rootfinder.newton(toMin,-1,1e-4);
			toWrite += $"{epsacc}\t{Abs(eval+0.5)/0.5}\n";
		}
		File.WriteAllText("txts/epsaccconvergence.txt",toWrite);

		WriteLine("------");
		WriteLine("Part c)");
		WriteLine("A more precise boundary condition has been implemented. Convergence plots similar to in part b) has been made.");
		WriteLine("Varying rmin...");
		toWrite = "";
		for(int i = 0; i < 10; i++){
			double rmin = 0.001*(i+1);
			toMin = x => MeImproved(x,rmin,8);
			eval = rootfinder.newton(toMin,-1,1e-4);
			toWrite += $"{rmin}\t{Abs(eval+0.5)/0.5}\n";
		}
		File.WriteAllText("txts/rminconvergenceImp.txt",toWrite);
		WriteLine("Varying rmax...");
		toWrite = "";
		for(int i = 0; i < 10; i++){
			double rmax = 2+2*(i);
			toMin = x => MeImproved(x,0.01,rmax);
			eval = rootfinder.newton(toMin,-1,1e-4);
			toWrite += $"{rmax}\t{Abs(eval+0.5)/0.5}\n";
		}
		File.WriteAllText("txts/rmaxconvergenceImp.txt",toWrite);
		WriteLine("Varying abs accuracy...");
		toWrite = "";
		for(int i = 0; i < 10; i++){
			double absacc = 1e-5*(i+1);
			toMin = x => MeImproved(x,0.01,8, absacc, 1e-5);
			eval = rootfinder.newton(toMin,-1,1e-4);
			toWrite += $"{absacc}\t{Abs(eval+0.5)/0.5}\n";
		}
		File.WriteAllText("txts/absaccconvergenceImp.txt",toWrite);
		WriteLine("Varying relative accuracy...");
		toWrite = "";
		for(int i = 0; i < 10; i++){
			double epsacc = 1e-5*(i+1);
			toMin = x => MeImproved(x,0.01,8, 1e-5, epsacc);
			eval = rootfinder.newton(toMin,-1,1e-4);
			toWrite += $"{epsacc}\t{Abs(eval+0.5)/0.5}\n";
		}
		File.WriteAllText("txts/epsaccconvergenceImp.txt",toWrite);
		WriteLine($"Attempting to find higher excited states. They should be at energies -0.5/n^2: {-0.5/1}, {-0.5/4} and {-0.5/9}");
		toMin = x => MeImproved(x,0.01,15, 1e-5, 1e-5);
		double eval1 = rootfinder.newton(toMin,-1,1e-4);
		double eval2 = rootfinder.newton(toMin,-0.1,1e-4);
		double eval3 = rootfinder.newton(toMin,-0.05,1e-4);
		WriteLine($"With initial guess for E=-1, found excited state at {eval1}");
		WriteLine($"With initial guess for E=-0.1, found excited state at {eval2}");
		WriteLine($"With initial guess for E=-0.05, found excited state at {eval3}");

		genlist<double> rs1 = new genlist<double>();
		genlist<vector> ys1 = new genlist<vector>();
		MeImproved(eval1,0.01,15, 1e-5, 1e-5, rs1, ys1);

		genlist<double> rs2 = new genlist<double>();
		genlist<vector> ys2 = new genlist<vector>();
		MeImproved(eval2,0.01,15, 1e-5, 1e-5, rs2, ys2);

		genlist<double> rs3 = new genlist<double>();
		genlist<vector> ys3 = new genlist<vector>();
		MeImproved(eval3,0.01,15, 1e-5, 1e-5, rs3, ys3);

		toWrite = "";
		for(int i = 0; i < rs1.size; i++){
			toWrite += $"{rs1[i]}\t{ys1[i][0]}\t{rs1[i]*Exp(-rs1[i])}\n";
		}
		File.WriteAllText("txts/groundstate.txt",toWrite);
		toWrite = "";
		for(int i = 0; i < rs2.size; i++){
			toWrite += $"{rs2[i]}\t{ys2[i][0]}\t\n";
		}
		File.WriteAllText("txts/1stexcited.txt",toWrite);
		toWrite = "";
		for(int i = 0; i < rs3.size; i++){
			toWrite += $"{rs3[i]}\t{ys3[i][0]}\t\n";
		}
		File.WriteAllText("txts/2ndexcited.txt",toWrite);
		WriteLine($"Their reduced radial wavefunctions can be seen on wavefunctionsImp");
	}
}