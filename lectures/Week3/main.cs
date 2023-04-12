using System;
using static System.Console;
using static System.Math; 

public static class main{
	public static void Main(){
		int n = 9;
		double[] a = new double[n];
		for(int i = 0; i < n; i++){
			Write($"a[{i}] is {a[i]} \n");
		}
		for(int i = 0; i < n; i++){
			a[i] = i; 
		}
	}
}
