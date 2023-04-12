using System;
using static System.Console;
using static System.Math;
using System.Numerics;


public static class main{
	public static int Main(){
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
		WriteLine($"u%v is {dotproduct}");
		WriteLine($"u.dot(v) with method is {u.dot(v)}");

		WriteLine($"u.norm() is {u.norm()}");
		(u.cross(v)).print("u.cross(v) = ");
		vec.cross(u,v).print("vec.cross(u,v) = ");

		WriteLine($"u and v are equal: {u.approx(v)}");
		vec w3 = new vec(4.0/2.0, 9.0/3.0, 16.0/4.0);
		w3.print("w3 = ");
		WriteLine($"u and w3 are equal: {u.approx(w3)}");

		WriteLine($"Test of ToString of w3: {w3}");
		return 0;
	}
}	
