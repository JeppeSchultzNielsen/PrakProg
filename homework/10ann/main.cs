using System;
using System.Threading;
using static System.Console;
using static System.Math; 
using System.IO;

public static class main{

	public static double g(double x){
		return Cos(5*x-1)*Exp(-x*x);
	}

	public static double sin(double x){
		return Sin(x);
	}

	public static double harmonicDiff(vector d){
		return d[1]+d[3];
	}

	public static double exp(vector d){
		return d[0]-d[2];
	}

	public static void Main(){
		WriteLine("--------");
		WriteLine("Part a)");
		//create training set 
		int dataSize = 20;
		vector xs = new vector(dataSize);
		vector ys = new vector(dataSize);

		WriteLine("Making training points..");
		string toWrite = "";
		for(int i = 0; i < dataSize; i++){
			double x = -1 + 2.0 * i / dataSize;
			xs[i] = x;
			ys[i] = g(x);
			toWrite += $"{xs[i]}\t{ys[i]}\n";
		}
		File.WriteAllText("txts/trainingPoints.txt",toWrite);
		WriteLine("Making interpolation with 4 neurons...");
		ann myAnn = new ann(4);
		myAnn.train(xs,ys);

		//create interpolation set
		toWrite = "";
		int interpolationPoints = 1000;
		for(int i = 0; i < interpolationPoints; i++){
			double x = -1 + 2.0 * i / interpolationPoints;
			toWrite += $"{x}\t{myAnn.response(x)}\n";
		}
		File.WriteAllText("txts/7neuronInterp.txt",toWrite);

		WriteLine("Interpolation can be seen in Interpolation.svg");
		WriteLine("--------");
		WriteLine("Part b)");
		WriteLine("Figures with interpolations and derivatives have been made. Both with Cos(5x-1)*exp(-x^2) and sin(x).");

		toWrite = "";
		for(int i = 0; i < interpolationPoints; i++){
			double x = -1 + 2.0 * i / interpolationPoints;
			toWrite += $"{x}\t{myAnn.integral(0,x)}\t{myAnn.derivative(x)}\t{myAnn.derivative2(x)}\n";
		}
		File.WriteAllText("txts/derivatives.txt",toWrite);

		dataSize = 20;
		xs = new vector(dataSize);
		ys = new vector(dataSize);
		toWrite = "";
		for(int i = 0; i < dataSize; i++){
			double x = 2.0*PI * i / dataSize;
			xs[i] = x;
			ys[i] = sin(x);
			toWrite += $"{x}\t{sin(x)}\n";
		}
		File.WriteAllText("txts/sinTraining.txt",toWrite);

		myAnn = new ann(4);
		myAnn.train(xs,ys);
		toWrite = "";
		for(int i = 0; i < interpolationPoints; i++){
			double x = 2.0*PI * i / interpolationPoints;
			toWrite += $"{x}\t{myAnn.integral(0,x)}\t{myAnn.derivative(x)}\t{myAnn.derivative2(x)}\t{myAnn.response(x)}\n";
		}
		File.WriteAllText("txts/sinDerivatives.txt",toWrite);
		WriteLine("--------");
		WriteLine("Part c)");
		WriteLine("Added option of training for solving differential equation.");
		WriteLine("Training on function u''+u=0 from 0 to 2pi with y(0)=0 and y'(0)=1...");
		myAnn = new ann(7);
		myAnn.trainDiffEq(harmonicDiff,0,2.0*PI,0,0,1,1,1);

		toWrite = "";
		for(int i = 0; i < interpolationPoints; i++){
			double x = 2.0*PI * i / interpolationPoints;
			toWrite += $"{x}\t{myAnn.response(x)}\t{myAnn.derivative(x)}\t{myAnn.derivative2(x)}\n";
		}
		File.WriteAllText("txts/diffEqSin.txt",toWrite);
		WriteLine("As can be seen on the plot DiffEqSin.svg, this results in a sine function, as expected.");
		WriteLine("Training on function u''+u=0 from 0 to 2pi with y(pi)=-1 and y'(pi)=0...");
		myAnn = new ann(7);
		myAnn.trainDiffEq(harmonicDiff,0,2.0*PI,PI,-1,0,1,1);

		toWrite = "";
		for(int i = 0; i < interpolationPoints; i++){
			double x = 2.0*PI * i / interpolationPoints;
			toWrite += $"{x}\t{myAnn.response(x)}\t{myAnn.derivative(x)}\t{myAnn.derivative2(x)}\n";
		}
		File.WriteAllText("txts/diffEqCos.txt",toWrite);
		WriteLine("As can be seen on the plot DiffEqCos.svg, this results in a cos function, as expected.");

	}
}
