using System;
using System.Threading;
using static System.Console;
using static System.Math; 
using System.IO;

public static class main{

	public static double f1(double x){
		return 1/Sqrt(x);
	}

	public static double f2(double x){
		return 4*Sqrt(1-x*x);
	}

	public static double f3(double x){
		return Log(x)/Sqrt(x);
	}

	public static double erf(double x){
		if(x < 0) return -erf(-x);
		double absErf = 0;
		if(x <= 1){
			absErf = 2/Sqrt(PI)*integrator.integrate(z => Exp(-z*z),0,x,1e-7,1e-6);
		}
		else{
			absErf = 1-2/Sqrt(PI)*integrator.integrate(t => Exp(-Pow(x+(1-t)/t,2))/t/t,0,1,1e-7,1e-6);
		}
		return absErf;
	}

	public static double erfApprox(double x){
        /// single precision error function (Abramowitz and Stegun, from Wikipedia)
        if(x<0) return -erfApprox(-x);
        double[] a={0.254829592,-0.284496736,1.421413741,-1.453152027,1.061405429};
        double t=1/(1+0.3275911*x);
        double sum=t*(a[0]+t*(a[1]+t*(a[2]+t*(a[3]+t*a[4]))));/* the right thing */
        return 1-sum*Exp(-x*x);
    } 

	public static void Main(){
		WriteLine("Testing integrator...");
		WriteLine($"sqrt(x) integrated from 0 to 1 is {integrator.integrate(Sqrt,0,1)}, should be {2.0/3}. Done in {integrator.nEvals} steps.");
		WriteLine("");
		WriteLine($"1/sqrt(x) integrated from 0 to 1 is {integrator.integrate(f1,0,1)}, should be {2.0}. Done in {integrator.nEvals} steps.");
		WriteLine("");
		WriteLine($"4/sqrt(1-x^2) integrated from 0 to 1 is {integrator.integrate(f2,0,1)}, should be {PI}. Done in {integrator.nEvals} steps.");
		WriteLine("");
		WriteLine($"ln(x)/sqrt(x) integrated from 0 to 1 is {integrator.integrate(f3,0,1)}, should be {-4}. Done in {integrator.nEvals} steps.");
		WriteLine("");
		WriteLine($"4/sqrt(1-x^2) integrated from 0 to 1 using Clenshaw-Curtis transformation is {integrator.integrateCC(f2,0,1)}, should be {PI}. Done in {integrator.nEvals} steps.");
		WriteLine("");
		WriteLine($"ln(x)/sqrt(x) integrated from 0 to 1 using Clenshaw-Curtis transformation is {integrator.integrateCC(f3,0,1)}, should be {-4}. Done in {integrator.nEvals} steps.");
		WriteLine("");
		WriteLine($"1/sqrt(x) integrated from 0 to 1 using Clenshaw-Curtis transformation with accuracy equal to scipy.integrate.quad is {integrator.integrateCC(f1,0,1,1.48e-8,1.48e-8)}, should be {2.0}. Done in {integrator.nEvals} steps.");
		WriteLine("");
		WriteLine("Results from python (accuracy could appearantly not be lowered from default, 1.48e-8)");
		WriteLine($"1/sqrt(x) integrated from 0 to 1 using scipy.integrate.quad is {1.9999999999999984}, should be {2.0}. Done in {231} steps.");
		WriteLine("");
		WriteLine($"ln(x)/sqrt(x)  integrated from 0 to 1 using scipy.integrate.quad is {-4.000000000000085}, should be {4}. Done in {315} steps.");
		WriteLine("scipy.integrate.quad seems to have more efficient methods for integrals.");


		string toWrite = "";
		for(int i = 0; i < 1000; i++){
			double x = -3+6.0*i/1000;
			toWrite += $"{x}\t{erf(x)}\t{erfApprox(x)}\n";
		}
		File.WriteAllText("txts/calculatedErf.txt", toWrite);
		

		string tabulatedPoints = File.ReadAllText("txts/errorTabulated.txt");
		string[] lines = tabulatedPoints.Split("\n");
		toWrite = "";
		for(int i = 0; i < lines.Length-1; i++){
			double x = double.Parse(lines[i].Split(" ")[0]);
			double tabVal = double.Parse(lines[i].Split(" ")[1]);
			toWrite += $"{x}\t{Abs(erf(x)-tabVal)}\t{Abs(erfApprox(x)-tabVal)}\n";
		}
		File.WriteAllText("txts/erfResiduals.txt", toWrite);
	}
}
