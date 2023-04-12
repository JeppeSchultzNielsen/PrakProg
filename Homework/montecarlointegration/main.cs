using System;
using System.Threading;
using static System.Console;
using static System.Math; 
using System.IO;

public static class main{

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
		WriteLine("Now instead of sampling completely random, I will sample using quasirandom numbers from a Van der Corput sequence.");
		WriteLine("I will estimate the error by comparing the result with sampling from a Van der Corput sequence in a different base.");
		WriteLine("Hopefully this will decrease the discrepancy and the integral should converge faster.");
		WriteLine($"");
		WriteLine("Calculating some test integrals with Van der Corput Monte Carlo integrator with 1000 points: ");
		a = new vector(2);
		b = new vector(2);
		b[0] = 1;
		b[1] = PI;
		a[0] = 0;
		a[1] = 0;
		(res, err) = mcint.vdcint(x => x[0]*x[0]*Sin(x[1]), a, b, 1000);
		WriteLine($"Int( r^2sin(theta) ) with r from 0 to 1 and theta from 0 to 2pi is calculated as: {res} +- {err}" );
		WriteLine($"It should be 1/(2pi) * 4/3 * pi * 1^3 = 2/3 = {2.0/3.0}");
		b[0] = 1;
		b[1] = 1;
		a[0] = 0;
		a[1] = 0;
		(res, err) = mcint.vdcint(x => (Exp( - (x[0]*x[0] + x[1]*x[1])) ), a, b, 1000);
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
		(res, err) = mcint.vdcint(x => ( 1/Pow(PI,3)* 1 / (1 - Cos(x[0])*Cos(x[1])*Cos(x[2])) ), a, b, 1000);
		WriteLine($"Int( 1/pi^3 * 1 / (1 - cos(x)*cos(y)*cos(z) ) ) with all limits from 0 to pi is calculated as: {res} +- {err}");
		WriteLine($"It should be 1.393203929685676");
		WriteLine($"");

		WriteLine("Calculating some test integrals with Van der Corput Monte Carlo integrator with 100000 points: ");
		a = new vector(2);
		b = new vector(2);
		b[0] = 1;
		b[1] = PI;
		a[0] = 0;
		a[1] = 0;
		(res, err) = mcint.vdcint(x => x[0]*x[0]*Sin(x[1]), a, b, 1000000);
		WriteLine($"Int( r^2sin(theta) ) with r from 0 to 1 and theta from 0 to 2pi is calculated as: {res} +- {err}" );
		WriteLine($"It should be 1/(2pi) * 4/3 * pi * 1^3 = 2/3 = {2.0/3.0}");
		b[0] = 1;
		b[1] = 1;
		a[0] = 0;
		a[1] = 0;
		(res, err) = mcint.vdcint(x => (Exp( - (x[0]*x[0] + x[1]*x[1])) ), a, b, 1000000);
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
		(res, err) = mcint.vdcint(x => ( 1/Pow(PI,3)* 1 / (1 - Cos(x[0])*Cos(x[1])*Cos(x[2])) ), a, b, 1000000);
		WriteLine($"Int( 1/pi^3 * 1 / (1 - cos(x)*cos(y)*cos(z) ) ) with all limits from 0 to pi is calculated as: {res} +- {err}");
		WriteLine($"It should be 1.393203929685676");
		WriteLine($"");
	}
}
