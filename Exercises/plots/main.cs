using System;
using System.Threading;
using static System.Console;
using static System.Math; 

public static class main{


	public static void Main(string[] args){
		string type = null;

		foreach(var arg in args){
	        var words = arg.Split(':');
	        if(words[0]=="-type"){
                type=words[1];
            }
        }

		if(type == "gamma"){
			for(double x = -5 + 1.0/64; x<=5; x += 1.0/32){
				WriteLine($"{x} {sfuns.gamma(x)}");
			}
		}

		if(type == "error"){
			for(double x = -3.5 + 1.0/64; x<=3.5; x += 1.0/32){
				WriteLine($"{x} {sfuns.erf(x)}");
			}
		}

		if(type == "lngamma"){
			for(double x = 0 + 1.0/64; x<=20; x += 1.0/8){
				WriteLine($"{x} {sfuns.lngamma(x)}");
			}
		}

	}
}
