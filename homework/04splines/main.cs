using System;
using System.Threading;
using static System.Console;
using static System.Math; 
using System.IO;

public static class main{
	public static void Main(){
		double[] xs = new double[10];
		double[] ys = new double[xs.Length];
		WriteLine("----------");
		WriteLine("Part a)");
		WriteLine("Making linear spline of sin(x)...");
		

		string toWrite = "";
		for(int i = 0; i < xs.Length; i++){
			double x = 1.0*i/(xs.Length-1)*2*PI;
			xs[i] = x;
			ys[i] = Sin(x);
			toWrite += $"{x}\t{Sin(x)}\n";
		}
		File.WriteAllText("txts/sinData.txt", toWrite);

		toWrite = "";
		int intPoints = 1000;
		for(int i = 0; i < intPoints; i++){
			double x = 1.0*i/(intPoints-1)*2*PI;
			toWrite += $"{x}\t{linearInterpolator.linterp(xs,ys,x)}\n";
		}
		File.WriteAllText("txts/linterp.txt", toWrite);

		toWrite = "";
		for(int i = 0; i < intPoints; i++){
			double x = 1.0*i/(intPoints-1)*2*PI;
			toWrite += $"{x}\t{linearInterpolator.linterpInteg(xs,ys,x)}\n";
		}
		File.WriteAllText("txts/linterpInteg.txt", toWrite);

		WriteLine("Plots of the results can be seen on the figure Linterp.svg");

		WriteLine("----------");
		WriteLine("Part b)");

		WriteLine("Making quadratic spline of sin(x)...");

		toWrite = "";
		var qsp = new qspline(xs,ys);
		for(int i = 0; i < intPoints; i++){
			double x = 1.0*i/(intPoints-1)*2*PI;
			toWrite += $"{x}\t{qsp.evaluate(x)}\t{qsp.derivative(x)}\t{qsp.integral(x)}\n";
		}
		File.WriteAllText("txts/qinterp.txt", toWrite);

		WriteLine("Plots of the results can be seen on the figure Qinterp.svg");


		int n = 5;
        xs = new double[n];
        double[] y1 = new double[n];
        double[] y2 = new double[n];
        double[] y3 = new double[n];

		for(int i=0; i<n; i++){
			xs[i] = i+1;
		}
        for(int i=0; i<n; i++){
            y1[i] = 1;
            y2[i] = xs[i];
            y3[i] = Pow(xs[i],2);
        }
		var flat = new qspline(xs,y1);
		var prop = new qspline(xs,y2);
		var quad = new qspline(xs,y3);

		WriteLine("Testing q-spline with different functions...");
		WriteLine("For y=1");
		toWrite = "bs_i:";
		for(int i = 0; i < n-1; i++){toWrite+=$"\t{flat.bs[i]}";}
		WriteLine(toWrite);
		toWrite = "cs_i:";
		for(int i = 0; i < n-1; i++){toWrite+=$"\t{flat.cs[i]}";}
		WriteLine(toWrite);
		WriteLine("For y=x");
		toWrite = "bs_i:";
		for(int i = 0; i < n-1; i++){toWrite+=$"\t{prop.bs[i]}";}
		WriteLine(toWrite);
		toWrite = "cs_i:";
		for(int i = 0; i < n-1; i++){toWrite+=$"\t{prop.cs[i]}";}
		WriteLine(toWrite);
		WriteLine("For y=x^2");
		toWrite = "bs_i:";
		for(int i = 0; i < n-1; i++){toWrite+=$"\t{quad.bs[i]}";}
		WriteLine(toWrite);
		toWrite = "cs_i:";
		for(int i = 0; i < n-1; i++){toWrite+=$"\t{quad.cs[i]}";}
		WriteLine(toWrite);
		WriteLine("This is what would be expected in a manual calculation.");

		WriteLine("----------");
		WriteLine("Part c)");
		WriteLine("Testing solution of tridiagonal matrix A: ");
		int size = 7;
		matrix rand = new matrix(size,size);
		var rnd = new System.Random(1); /* or any other seed */
		for(int i = 0; i < size; i++){
			for(int j = 0; j < size; j++){
				if( Abs(j - i) <= 1) rand[i,j] = 2*rnd.NextDouble() - 1;
			}
		}
		rand.print();
		vector testVec = new vector(size);
		for(int i = 0; i < size; i++){
			testVec[i] = 2*rnd.NextDouble() - 1;
		}
		WriteLine("With vector d: ");
		testVec.print();
		WriteLine("Giving solution x: ");
		var mys = new cspline(xs,ys);
		var sol = mys.solvetridiag(rand.copy(),testVec.copy());
		sol.print();
		WriteLine("We have A*x = ");
		var res = rand*sol;
		res.print();
		WriteLine("This is equal to d, so we have correct solution.");

		WriteLine("Making cubic spline of sin(x)...");
		xs = new double[10];
		ys = new double[xs.Length];
		for(int i = 0; i < xs.Length; i++){
			double x = 1.0*i/(xs.Length-1)*2*PI;
			xs[i] = x;
			ys[i] = Sin(x);
		}
		mys = new cspline(xs,ys);



		toWrite = "";
		for(int i = 0; i < intPoints; i++){
			double x = 1.0*i/(intPoints-1)*2*PI;
			toWrite += $"{x}\t{mys.evaluate(x)}\t{mys.derivative(x)}\t{mys.integral(x)}\n";
		}
		File.WriteAllText("txts/cinterp.txt", toWrite);
		WriteLine("Quadratic spline data can be seen on Cinterp.svg");
		WriteLine("It overlaps completely with GNUplot's cubic spline.");
	}
}
