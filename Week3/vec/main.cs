using System;
using static System.Console;
using static System.Math; 

public static class main{
	public static void Main(){
		vec v = new vec(1,2,3);
		vec u = new vec(2,3,4);
		u.print("u=");
		v.print("v=");
		(u+v).print("u+v=");
		
		(2*u).print("2*u");
		vec w = u*2;
		(w).print("u*2");

		vec w2 = u + 6*v - w;
		w2.print("w2=");

		(-u).print("-u=");

		double dotproduct = u%v;
		WriteLine($"u dot v is {dotproduct}");
		WriteLine($"u dot v with method is {u.dot(v)}");
	}
}
