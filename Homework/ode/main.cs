using System;
using System.Threading;
using static System.Console;
using static System.Math; 
using System.IO;
using System.Collections.Generic;

public static class main{

	static vector harm(double x, vector ys){
		//differential equation u''=-u can be rewritten as 
		//y2 = u', y1 = u => y1' = y2, y2' = -y1
		return new vector(ys[1],-ys[0]);
	}

	static vector dampedPend(double x, vector ys){
		//assume x is time, y1 is theta and y2 is omega. Assume same damping constants as in python example..
		return new vector(ys[1],-0.25*ys[1]-5*Sin(ys[0]));
	}

	public static void Main(){
		var xlistInit=new List<double>();
		var ylistInit=new List<vector>();
		vector ya = new vector(0,1);
		var points = odesolver.driver(harm,0,ya,4*PI,1e-3,1e-3,1e-3,xlistInit, ylistInit);
		var xs = points.Item1;
		var ys = points.Item2;
		string toWrite = "";
		for(int i = 0; i < xs.Count; i++){
			toWrite += $"{xs[i]}\t{ys[i][0]}\t{ys[i][1]}\n";
		}
		File.WriteAllText("txts/harmonicDiff.txt", toWrite);

		xlistInit=new List<double>();
		ylistInit=new List<vector>();
		ya = new vector(PI-0.1,0);
		points = odesolver.driver(dampedPend,0,ya,10,1e-3,1e-3,1e-3,xlistInit, ylistInit);
		xs = points.Item1;
		ys = points.Item2;
		toWrite = "";
		for(int i = 0; i < xs.Count; i++){
			toWrite += $"{xs[i]}\t{ys[i][0]}\t{ys[i][1]}\n";
		}
		File.WriteAllText("txts/dampedpend.txt", toWrite);

		toWrite = "";
		for(int i = 1; i < 10; i++){
			points = odesolver.driver(dampedPend,0,ya,i,1e-3,1e-3,1e-3);
			double x = points.Item1[0];
			double y1 = points.Item2[0][0];
			double y2 = points.Item2[0][1];
			toWrite += $"{x}\t{y1}\t{y2}\n";
		}
		File.WriteAllText("txts/dampedpendEndpoints.txt", toWrite);
	}
}
