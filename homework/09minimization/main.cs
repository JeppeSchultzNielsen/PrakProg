using System;
using System.Threading;
using static System.Console;
using static System.Math; 
using System.IO;
using System.Collections.Generic;

public static class main{
	public static double poly2(vector x){
		return x[0]*x[0]+6;
	}

	public static double rosenbrock(vector x){
		return Pow(1-x[0],2)+100*Pow(x[1]- x[0]*x[0],2);
	}

	public static double himmelblau(vector x){
		return Pow(x[0]*x[0]+x[1]-11,2)+Pow(x[0]+x[1]*x[1]-7,2);
	}

	public static double breitWigner(vector par){
		double x = par[0];
		double A = par[1];
		double xm = par[2]; 
		double d = par [3];
		return A/( (x-xm)*(x-xm) + d*d/4  );
	}

	public static void Main(){
		WriteLine("------");
		WriteLine("Part a)");
		WriteLine("Testing minimizer:");
		vector x0 = new vector(1);
		x0[0] = 2;
		vector res = minimizer.qnewton(poly2,x0,1e-7);
		WriteLine($"Minimizer found minimum of x^2+6 at {res[0]}, it should be at 0.");
		x0 = new vector(2);
		x0[0] = 2;
		x0[1] = 2;
		res = minimizer.qnewton(rosenbrock,x0,1e-7);
		WriteLine($"Minimizer found minimum of Rosenbrock function at ({res[0]},{res[1]}), it should be at (1,1).");
		x0[0] = 0;
		x0[1] = 0; 
		res = minimizer.qnewton(himmelblau,x0,1e-7);
		WriteLine($"Minimizer found minimum of Himmelblau function at ({res[0]},{res[1]}), it should be at (3,2).");
		WriteLine("Other minima of Himmelblau should also exist. We can get them by using different initial guesses.");
		x0[0] = -3;
		x0[1] = 3; 
		res = minimizer.qnewton(himmelblau,x0,1e-7); 
		WriteLine($"With initial guess (-3,3) found minimum of Himmelblau function at ({res[0]},{res[1]}), it should be at (-2.805,3.131).");
		x0[0] = -3;
		x0[1] = -3; 
		res = minimizer.qnewton(himmelblau,x0,1e-7); 
		WriteLine($"With initial guess (-3,-3) found minimum of Himmelblau function at ({res[0]},{res[1]}), it should be at (-3.779,-3.283).");
		x0[0] = 3;
		x0[1] = -3; 
		res = minimizer.qnewton(himmelblau,x0,1e-7); 
		WriteLine($"With initial guess (3,-3) found minimum of Himmelblau function at ({res[0]},{res[1]}), it should be at (3.584,-1.848).");
		WriteLine($"---------");
		WriteLine("Part b)");
		var energy = new List<double>();
		var signal = new List<double>();
		var error  = new List<double>();
		var separators = new char[] {' ','\t'};
		var options = StringSplitOptions.RemoveEmptyEntries;
		do{
				string line=Console.In.ReadLine();
				if(line==null)break;
				string[] words=line.Split(separators,options);
				energy.Add(double.Parse(words[0]));
				signal.Add(double.Parse(words[1]));
				error .Add(double.Parse(words[2]));
		}while(true);
		WriteLine("Fitting Breit-Wigner to higgs data with guesses A = 16, m = 126, Γ = 1");
		vector guesses = new vector(new double[3]{16,126,1}); 
		vector fitparams = minimizer.fit(breitWigner,guesses,energy, signal, error, 1e-4);
		WriteLine($"Fit concluded with parameters A = {fitparams[0]}, m = {fitparams[1]}, Γ = {fitparams[2]}");
		WriteLine("Fit plotted on Higgs.svg");
		string toWrite = "";
		for(int i = 0; i < 1000; i++){
			double x = energy[0] + (energy[energy.Count-1] - energy[0])*i/1000.0;
			vector par = new vector(new double[4]{x,fitparams[0],fitparams[1],fitparams[2]});
			double y = breitWigner(par);
			toWrite += $"{x}\t{y}\n";
		}
		File.WriteAllText("txts/fittedData.dat",toWrite);
		WriteLine($"---------");
		WriteLine("Part c)");
		WriteLine("Attempting to find minimum of Rosenbrock function with downhill simplex...");
		List<vector> starts = new List<vector>();
		starts.Add(new vector(new double[]{1,-3}));
		starts.Add(new vector(new double[]{-5,0}));
		starts.Add(new vector(new double[]{0,5}));
		vector simplexRes = minimizer.downhillSimplex(rosenbrock,starts,1e-20);
		WriteLine($"With initial simplex (1,-3), (-5,0), (0,5), found minimal point using downhill simplex at: ({simplexRes[0]},{simplexRes[1]})");
		starts = new List<vector>();
		starts.Add(new vector(new double[]{0,1}));
		starts.Add(new vector(new double[]{1,0}));
		starts.Add(new vector(new double[]{2,2}));
		simplexRes = minimizer.downhillSimplex(rosenbrock,starts,1e-20);
		WriteLine($"With initial simplex (0,1), (1,0), (2,2), found minimal point using downhill simplex at: ({simplexRes[0]},{simplexRes[1]})");
		starts = new List<vector>();
		starts.Add(new vector(new double[]{-2,-1}));
		starts.Add(new vector(new double[]{0,4}));
		starts.Add(new vector(new double[]{-2,3}));
		simplexRes = minimizer.downhillSimplex(rosenbrock,starts,1e-20);
		WriteLine($"With initial simplex (-2,-1), (0,4), (-2,3), found minimal point using downhill simplex at: ({simplexRes[0]},{simplexRes[1]})");
		WriteLine("Attempting to fit Breit-Wigner to Higgs boson data using downhill simplex...");
		starts = new List<vector>();
		starts.Add(new vector(new double[]{16,126,1}));
		starts.Add(new vector(new double[]{20,126.5,2}));
		starts.Add(new vector(new double[]{25,127,3}));
		fitparams = minimizer.fit(breitWigner,guesses,energy, signal, error, 1e-17);
		WriteLine($"With initial simplex (16,126,1), (20,126.5,2), (25,127,3), found Breit-Wigner parameters: A = {fitparams[0]}, m = {fitparams[1]}, Γ = {fitparams[2]}");
		WriteLine("This is the same result as in b).");
	}
}
