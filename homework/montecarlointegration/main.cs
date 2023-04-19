using System;
using System.Threading;
using static System.Console;
using static System.Math; 
using System.IO;

public static class main{

	public static double circle(vector x){
		if(x[0]*x[0]+x[1]*x[1] <= 1) return 1;
		else return 0;
	}

	public static double heavy(vector x){
		if(x[0] <= 0.8) return 1;
		if(x[1] <= 0.8) return 1;
		return 0;
	}

	public static void Main(){
		WriteLine("Calculating some test integrals with plain Monte Carlo integrator with 1000 points: ");
		
		vector a = new vector(2);
		vector b = new vector(2);
		b[0] = 1;
		b[1] = PI;
		a[0] = 0;
		a[1] = 0;
		(double res, double err) = mcint.plainmc(x => x[0]*x[0]*Sin(x[1]), a, b, 1000);
		WriteLine($"Int( r^2sin(theta) ) with r from 0 to 1 and theta from 0 to 2pi is calculated as: {res} +- {err}" );
		WriteLine($"It should be 1/(2pi) * 4/3 * pi * 1^3 = 2/3 = {2.0/3.0}");
		b[0] = 1;
		b[1] = 1;
		a[0] = 0;
		a[1] = 0;
		(res, err) = mcint.plainmc(x => (Exp( - (x[0]*x[0] + x[1]*x[1])) ), a, b, 1000);
		WriteLine($"Int( e^(x^2+y^2) ) with x from 0 to 1 and y from 0 to 1 is calculated as: {res} +- {err}");
		WriteLine($"It should be 0.557746");
		a = new vector(3);
		b = new vector(3);
		b[1] = PI;
		b[2] = PI;
		b[0] = PI;
		a[0] = 0;
		a[1] = 0;
		a[2] = 0;
		(res, err) = mcint.plainmc(x => ( 1/Pow(PI,3)* 1 / (1 - Cos(x[0])*Cos(x[1])*Cos(x[2])) ), a, b, 1000);
		WriteLine($"Int( 1/pi^3 * 1 / (1 - cos(x)*cos(y)*cos(z) ) ) with all limits from 0 to pi is calculated as: {res} +- {err}");
		WriteLine($"It should be 1.393203929685676");
		WriteLine($"");
		WriteLine("Calculating some test integrals with plain Monte Carlo integrator with 100000 points: ");
		a = new vector(2);
		b = new vector(2);
		b[0] = 1;
		b[1] = PI;
		a[0] = 0;
		a[1] = 0;
		(res, err) = mcint.plainmc(x => x[0]*x[0]*Sin(x[1]), a, b, 100000);
		WriteLine($"Int( r^2sin(theta) ) with r from 0 to 1 and theta from 0 to 2pi is calculated as: {res} +- {err}" );
		WriteLine($"It should be 1/(2pi) * 4/3 * pi * 1^3 = 2/3 = {2.0/3.0}");
		b[0] = 1;
		b[1] = 1;
		a[0] = 0;
		a[1] = 0;
		(res, err) = mcint.plainmc(x => (Exp( - (x[0]*x[0] + x[1]*x[1])) ), a, b, 100000);
		WriteLine($"Int( e^(x^2+y^2) ) with x from 0 to 1 and y from 0 to 1 is calculated as: {res} +- {err}");
		WriteLine($"It should be 0.557746");
		a = new vector(3);
		b = new vector(3);
		b[1] = PI;
		b[2] = PI;
		b[0] = PI;
		a[0] = 0;
		a[1] = 0;
		a[2] = 0;
		(res, err) = mcint.plainmc(x => ( 1/Pow(PI,3)* 1 / (1 - Cos(x[0])*Cos(x[1])*Cos(x[2])) ), a, b, 100000);
		WriteLine($"Int( 1/pi^3 * 1 / (1 - cos(x)*cos(y)*cos(z) ) ) with all limits from 0 to pi is calculated as: {res} +- {err}");
		WriteLine($"It should be 1.393203929685676");
		WriteLine($"");
		WriteLine("Now instead of sampling completely random, I will sample using quasirandom numbers from Halton sequences.");
		WriteLine("I will estimate the error by comparing the result with sampling from Halton sequences in a different bases.");
		WriteLine("Hopefully this will decrease the discrepancy and the integral should converge faster.");
		WriteLine($"");
		WriteLine("Calculating some test integrals with Halton Monte Carlo integrator with 1000 points: ");
		a = new vector(2);
		b = new vector(2);
		b[0] = 1;
		b[1] = PI;
		a[0] = 0;
		a[1] = 0;
		(res, err) = mcint.haltonint(x => x[0]*x[0]*Sin(x[1]), a, b, 1000);
		WriteLine($"Int( r^2sin(theta) ) with r from 0 to 1 and theta from 0 to 2pi is calculated as: {res} +- {err}" );
		WriteLine($"It should be 1/(2pi) * 4/3 * pi * 1^3 = 2/3 = {2.0/3.0}");
		b[0] = 1;
		b[1] = 1;
		a[0] = 0;
		a[1] = 0;
		(res, err) = mcint.haltonint(x => (Exp( - (x[0]*x[0] + x[1]*x[1])) ), a, b, 1000);
		WriteLine($"Int( e^(x^2+y^2) ) with x from 0 to 1 and y from 0 to 1 is calculated as: {res} +- {err}");
		WriteLine($"It should be 0.557746");
		WriteLine($"");
		WriteLine("Calculating some test integrals with Halton Monte Carlo integrator with 100000 points: ");
		a = new vector(2);
		b = new vector(2);
		b[0] = 1;
		b[1] = PI;
		a[0] = 0;
		a[1] = 0;
		(res, err) = mcint.haltonint(x => x[0]*x[0]*Sin(x[1]), a, b, 100000);
		WriteLine($"Int( r^2sin(theta) ) with r from 0 to 1 and theta from 0 to 2pi is calculated as: {res} +- {err}" );
		WriteLine($"It should be 1/(2pi) * 4/3 * pi * 1^3 = 2/3 = {2.0/3.0}");
		b[0] = 1;
		b[1] = 1;
		a[0] = 0;
		a[1] = 0;
		(res, err) = mcint.haltonint(x => (Exp( - (x[0]*x[0] + x[1]*x[1])) ), a, b, 100000);
		WriteLine($"Int( e^(x^2+y^2) ) with x from 0 to 1 and y from 0 to 1 is calculated as: {res} +- {err}");
		WriteLine($"It should be 0.557746");
		WriteLine($"");
		WriteLine("Using the Halton sequence seems to allow much faster convergence than plain Monte Carlo;");
		WriteLine("In plain Monte Carlo, going from 1k to 100k samples improved precision by a factor 10, with Halton sequence its almost a factor 100.");
		WriteLine($"");
		WriteLine("Now I will solve the integrals with stratified sampling.");
		b[0] = 1;
		b[1] = 1;
		a[0] = 0;
		a[1] = 0;
		string path = "test.txt";
		using (StreamWriter sw = File.CreateText(path)){}
		(res, err) = mcint.strataint(circle, a, b, 100000,1000,path);
		b[0] = 1;
		b[1] = 1;
		a[0] = 0;
		a[1] = 0;
		WriteLine($"Integrating a circle of radius 1 and height 1 with stratified sampling with x and y from 0 to 1 is calculated as: {res} +- {err} with 100000 points");
		(res, err) = mcint.plainmc(circle, a, b, 100000);
		WriteLine($"Integrating a circle of radius 1 and height 1 with plain sampling with x and y from 0 to 1 is calculated as: {res} +- {err} with 100000 points");
		(res, err) = mcint.strataint(circle, a, b, 1000000,10000);
		WriteLine($"Integrating a circle of radius 1 and height 1 with stratified sampling with x and y from 0 to 1 is calculated as: {res} +- {err} with 1e6 points");
		(res, err) = mcint.plainmc(circle, a, b, 1000000);
		WriteLine($"Integrating a circle of radius 1 and height 1 with plain sampling with x and y from 0 to 1 is calculated as: {res} +- {err} with 1e6 points");
		WriteLine($"It should be pi/4, {PI/4}");
		WriteLine($"A comparison of convergence has been saved to a plot.");
		string toWrite = "";
		for(int i = 0; i < 3; i++){
			for(int j = 0; j < 5; j++){
				int N = 10000*(2*j+1)*((int)Pow(10,i));
				(double resPlain, double errPlain) = mcint.plainmc(circle, a, b, N);
				(double resHalton, double errHalton) = mcint.haltonint(circle, a, b, N);
				(double resStrata, double errStrata) = mcint.strataint(circle, a, b, N,(int)N/100);
				toWrite += $"{N}\t{errPlain}\t{errHalton}\t{errStrata}\n";
			}
		}
		File.WriteAllText("convergence.txt", toWrite);
	}
}
