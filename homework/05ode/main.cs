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

	static vector threebody(double x, vector ys){
		//assume m_i = 1, G = 1. The differential equations are then
		// v1' = r1'' = (r2-r1) / (r1-r2)^3 + (r3-r1)/ (r1-r3)^3
		//and permutations of that. Defining y2_1 = r_1', y1_1 = r_1, we get first derivatives:
		//v1' = r1'' = y2_1' = + (y1_2-y1_1)/(y1_1-y1_2)^3 + (y1_3-y1_1)/(y1_1-y1_3)^2
		// y1_1' = y2_1 = v_1
		//ys will be a 12d vector: for each body, 2 position and 2 velocity entries. 
		double vx_1 = ys[0];
		double vy_1 = ys[1]; 
		double vx_2 = ys[2];
		double vy_2 = ys[3];
		double vx_3 = ys[4];
		double vy_3 = ys[5];
		double xx_1 = ys[6];
		double xy_1 = ys[7];
		double xx_2 = ys[8];
		double xy_2 = ys[9];
		double xx_3 = ys[10];
		double xy_3 = ys[11];
		double d12 = Sqrt( (xx_1-xx_2)*(xx_1-xx_2)+(xy_1-xy_2)*(xy_1-xy_2));
		double d13 = Sqrt( (xx_1-xx_3)*(xx_1-xx_3)+(xy_1-xy_3)*(xy_1-xy_3));
		double d23 = Sqrt( (xx_3-xx_2)*(xx_3-xx_2)+(xy_3-xy_2)*(xy_3-xy_2));
		return new vector( new double[] {
			(xx_2 - xx_1)/Pow(d12,3) +(xx_3 - xx_1)/Pow(d13,3),
			(xy_2 - xy_1)/Pow(d12,3) +(xy_3 - xy_1)/Pow(d13,3),
			(xx_1 - xx_2)/Pow(d12,3) +(xx_3 - xx_2)/Pow(d23,3),
			(xy_1 - xy_2)/Pow(d12,3) +(xy_3 - xy_2)/Pow(d23,3),
			(xx_1 - xx_3)/Pow(d13,3) +(xx_2 - xx_3)/Pow(d23,3),
			(xy_1 - xy_3)/Pow(d13,3) +(xy_2 - xy_3)/Pow(d23,3),
			vx_1,
			vy_1,
			vx_2,
			vy_2,
			vx_3,
			vy_3
	});

	}

	public static void Main(){
		var xlistInit=new genlist<double>();
		var ylistInit=new genlist<vector>();
		vector ya = new vector(0,1);
		WriteLine("-------");
		WriteLine("Part a)");
		WriteLine("Solving u''=-u with initial conditions u0 = 0, u0'=1, should give sin(x). Plotted on Sin.svg.");
		var points = odesolver.driver(harm,0,ya,4*PI,1e-3,1e-3,1e-3,xlistInit, ylistInit);
		var xs = points.Item1;
		var ys = points.Item2;
		string toWrite = "";
		for(int i = 0; i < xs.size; i++){
			toWrite += $"{xs[i]}\t{ys[i][0]}\t{ys[i][1]}\n";
		}
		File.WriteAllText("txts/harmonicDiff.txt", toWrite);

		xlistInit=new genlist<double>();
		ylistInit=new genlist<vector>();
		ya = new vector(PI-0.1,0);
		WriteLine("Solving damped pendulum with same parameters as in scipy.integrate.odeint manual. Plotted on DampedPendulum.svg.");
		points = odesolver.driver(dampedPend,0,ya,10,1e-3,1e-3,1e-3,xlistInit, ylistInit);
		xs = points.Item1;
		ys = points.Item2;
		toWrite = "";
		for(int i = 0; i < xs.size; i++){
			toWrite += $"{xs[i]}\t{ys[i][0]}\t{ys[i][1]}\n";
		}
		File.WriteAllText("txts/dampedpend.txt", toWrite);

		WriteLine("-------");
		WriteLine("Part b)");
		WriteLine("Driver has been improved. Now has option to give only endpoints, which has been plotted on DampedPendulum.svg.");
		toWrite = "";
		for(int i = 1; i < 10; i++){
			points = odesolver.driver(dampedPend,0,ya,i,1e-3,1e-3,1e-3);
			double x = points.Item1[0];
			double y1 = points.Item2[0][0];
			double y2 = points.Item2[0][1];
			toWrite += $"{x}\t{y1}\t{y2}\n";
		}
		File.WriteAllText("txts/dampedpendEndpoints.txt", toWrite);

		WriteLine("-------");
		WriteLine("Part c)");
		WriteLine("Solving three body problem with initial conditions as given on wikipedia...");
		ya = new vector(new double[]{0.4662036850,
			0.4323657300,
			-0.93240737,
			-0.86473146,
			0.4662036850,
			0.4323657300,
			-0.97000436,
			0.24308753,
			0,
			0,
			0.97000436,
			-0.24308753
		});
		xlistInit=new genlist<double>();
		ylistInit=new genlist<vector>();
		points = odesolver.driver(threebody,0,ya,6.3259,1e-3,1e-3,1e-3,xlistInit, ylistInit);
		xs = points.Item1;
		ys = points.Item2;
		toWrite = $"{xs[0]}\t{ys[0][6]}\t{ys[0][7]}\t{ys[0][8]}\t{ys[0][9]}\t{ys[0][10]}\t{ys[0][11]}\n";
		//to ensure "time correct animation", discard some points.
		double currentTime = 0;
		double dt = 0.02;
		for(int i = 0; i < xs.size; i++){
			if(xs[i] > currentTime+dt){
				toWrite += $"{xs[i]}\t{ys[i][6]}\t{ys[i][7]}\t{ys[i][8]}\t{ys[i][9]}\t{ys[i][10]}\t{ys[i][11]}\n";
				currentTime = xs[i];
			}
		}
		File.WriteAllText("txts/threebody.txt", toWrite);
		WriteLine("Solution shown in animation Threebody.gif");
	}
}
