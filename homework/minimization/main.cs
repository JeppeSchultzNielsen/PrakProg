using System;
using System.Threading;
using static System.Console;
using static System.Math; 
using System.IO;

public static class main{
	public static double poly2(vector x){
		return x[0]*x[0];
	}

	public static void Main(){
		minimizer.qnewton(poly2,new vector(7),1e-7);
	}
}
