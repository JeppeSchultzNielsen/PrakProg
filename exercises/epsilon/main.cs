using System;
using static System.Console;
using static System.Math; 

public static class main{
	public static bool approx(double a, double b, double acc = 1e-9, double eps = 1e-9){
		if(Abs(b-a) < acc) return true;
		else if(Abs(b-a) < Max(Abs(a),Abs(b))*eps) return true;
		else return false;
	}


	public static void Main(){
		int i = 1; 
		while(i+1>i){
			i++;
		}
		WriteLine($"my max int = {i}");
		WriteLine($"int.MaxValue = {int.MaxValue}");
	
                i = 1;
                while(i-1<i){
                        i--;
                }
                WriteLine($"my min int = {i}");
                WriteLine($"int.MinValue = {int.MinValue}");

		//calculate machine epsilon for double
		double x = 1;
		while((1+x)!=1){
			x/=2;
		}
		x*=2;
		WriteLine($"The machine epsilon for doubles is {x}");
		
		//calculate machine epsilon for floats
		float y = 1F; 
		while( (float)(1F+y) != 1F){
			y/=2F;
		}
		y*=2F;
		WriteLine($"The machine epsilon for floats is {y}");

		//Exercise 3 about tiny numbers. I do it with machine epsilon for floats. 
		int n=(int)1e6;
		double tiny=x/2;
		double sumA=0,sumB=0;

		sumA+=1; 
		for(i=0;i<n;i++){
			sumA+=tiny;
		}
	
		for(i=0;i<n;i++){
			sumB+=tiny;
		} sumB+=1;


		WriteLine($"Machine epsilon is {x:e}, epsilon/2 is {tiny}");
		WriteLine($"sumA-1 = {sumA-1:e} should be {n*tiny:e}");
		WriteLine($"sumB-1 = {sumB-1:e} should be {n*tiny:e}");
		WriteLine("The reason for this fact is that epsilon/2 was found in exercise 2 to be exactly the value which results in 1 when added with 1. So adding it to 1 many times (as done in sumA) results in 1+epsilon/2 many times, and each time it is 1. But if we add many epsilon/2 it becomes a 'large' number, which does not result in 1 when added to 1.");


		double d1 = 0.1+0.1+0.1+0.1+0.1+0.1+0.1+0.1;
		double d2 = 8*0.1;

		WriteLine($"d1=0.1+0.1+...+0.1 is {d1:e15}");
		WriteLine($"d2=8*0.1 is {d2:e15}");
		WriteLine($"d1==d2 evaluates as {d1 == d2}");
		WriteLine($"approx(d1,d2) evaluates as {approx(d1,d2)} with relative and absolute precision 1e-9");
	}
}
