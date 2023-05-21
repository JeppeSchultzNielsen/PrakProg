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

	public static void Main(){
		WriteLine("Part A");
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
		WriteLine("Making interpolation with 7 neurons...");
		ann myAnn = new ann(7);
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
		WriteLine("");
		WriteLine("Part B");
		WriteLine("Figures have been made. Both with Cos(5x-1)*exp(-x^2) and sin(2x).");
		WriteLine("Calculating integral can be done in a very stable way. 1st derivative looks somewhat convincing, 2nd derivative is very bad.");
		WriteLine("This is fair because we are really limited by few gaussian wavelets, which may have extreme second derivatives.");

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

		myAnn = new ann(10);
		myAnn.train(xs,ys);
		toWrite = "";
		for(int i = 0; i < interpolationPoints; i++){
			double x = 2.0*PI * i / interpolationPoints;
			toWrite += $"{x}\t{myAnn.integral(0,x)}\t{myAnn.derivative(x)}\t{myAnn.derivative2(x)}\t{myAnn.response(x)}\n";
		}
		File.WriteAllText("txts/sinDerivatives.txt",toWrite);

	}
}
