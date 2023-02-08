using static System.Math;
class main{
	
	static void print(string toPrint){
		System.Console.Write(toPrint + "\n");
	}

	public static int Main(){
		double sqrt2 = Sqrt(2);
		double pow215 = Pow(2.0, 1.0/5);
		double etopi = Pow(E,PI);
		double pitoe = Pow(PI,E);
		print($"sqrt2 is {Sqrt(2)}, sqrt2*sqrt2 is {sqrt2*sqrt2}");
		print($"2^(1/5) is {pow215}, (2^(1/5))^5 is {Pow(pow215,5)}");
		print($"e^pi is {etopi} and (e^pi)^(1/pi) is {Pow(etopi,1.0/PI)}");
		print($"pi^e is {pitoe} and (pi^e)^(1/e) is {Pow(pitoe, 1.0/E)}");
		
		print($"gamma(1) is {sfuns.gamma(1)}");
		print($"gamma(2) is {sfuns.gamma(2)}");
		print($"gamma(3) is {sfuns.gamma(3)}");
		print($"gamma(10) is {sfuns.gamma(10)}");

		print($"gamma(7) is {sfuns.gamma(7)}");
		print($"lngamma(7) is {sfuns.lngamma(7)}");

		print($"gamma(14) is {sfuns.gamma(14)}");
		print($"lngamma(14) is {sfuns.lngamma(14)}");


		print($"gamma(-0.1) is {sfuns.gamma(-0.1)}");
                print($"lngamma(-0.1) is {sfuns.lngamma(-0.1)}");

		return 0;
	}
}
