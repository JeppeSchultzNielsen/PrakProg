using System;
using System.Threading;
using static System.Console;
using static System.Math; 
using System.IO;

public static class main{
	public static void Main(){
		double[] xs = new double[10];
		double[] ys = new double[xs.Length];

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

		toWrite = "";
		var qsp = new qspline(xs,ys);
		for(int i = 0; i < intPoints; i++){
			double x = 1.0*i/(intPoints-1)*2*PI;
			toWrite += $"{x}\t{qsp.evaluate(x)}\t{qsp.derivative(x)}\t{qsp.integral(x)}\n";
		}
		File.WriteAllText("txts/qinterp.txt", toWrite);

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



	}
}
