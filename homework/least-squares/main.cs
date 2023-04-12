using System;
using System.Threading;
using static System.Console;
using static System.Math; 
using System.IO;

public static class main{

	public static void Main(){

		//we're fitting to ln(y)=ln(a)-λt with errors δln(y)=δy/y.
		var fs = new Func<double,double>[2];
		fs[0] = x => 1;
		fs[1] = x => -x; 
		string data = File.ReadAllText("data.txt");
		string[] points = data.Split("\n");
		int noPoints = points.Length - 1; //last line is empty
		vector xs = new vector(noPoints);
		vector ys = new vector(noPoints);
		vector dys = new vector(noPoints); 

		for(int i = 0; i < noPoints; i++){
			string[] splitted = points[i].Split("\t");
			double currentY = double.Parse(splitted[1]);
			xs[i] = double.Parse(splitted[0]);
			ys[i] = Log(currentY);
			dys[i] = double.Parse(splitted[2])/currentY;
		}

		var poptpcov = fitter.lsfit(fs,xs,ys,dys);
		var c = poptpcov.Item1;
		var pcov = poptpcov.Item2;
		WriteLine("Covariance matrix:");
		pcov.print();
		double dlambda = Sqrt(pcov[1,1]);
		double doffset = Sqrt(pcov[0,0]);
		WriteLine($"fitted T1/2 = {Log(2)/c[1]:e3}+-{Log(2)*dlambda/c[1]/c[1]:e0} d, modern value is 3.6319+-0.0023 d.");
		WriteLine("Means we're not entirely within an errorbar of modern value.");

		//Write fitted function data to txt file
		string toWrite = "";
		for(int i = 0; i < 1000; i++){
			double t = i/1000.0*(xs[noPoints-1]+1);
			double n = 0;
			double nError1 = c[0]+doffset-c[1]*t;
			double nError2 = c[0]-doffset-c[1]*t;
			double nError3 = c[0]-(c[1]+dlambda)*t;
			double nError4 = c[0]-(c[1]-dlambda)*t;
			for(int j = 0; j < fs.Length; j++){
				n+=c[j]*fs[j](t);
			}
			n = Exp(n);
			nError1 = Exp(nError1);
			nError2 = Exp(nError2);
			nError3 = Exp(nError3);
			nError4 = Exp(nError4);
			toWrite += $"{t}\t{n}\t{nError1}\t{nError2}\t{nError3}\t{nError4}\n";
		}
		File.WriteAllText("evaluatedFit.txt", toWrite);

	}
}
