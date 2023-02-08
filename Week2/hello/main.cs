using System;
using static System.Console;
using static System.Math;
class main{
	static string s = "class scope s";
	public static void print_s(){WriteLine(s);}

	public static void Main(){
		string s = "method scope s"; 
		print_s();
		WriteLine(s);
		{
			string ss = "block scope";
			WriteLine(ss);
		}
	Write("hello from main\n");
	static_hello.print();
	static_world.print();
	static_hello.greeting="new hello from main\n";
	static_world.greeting="new world from main\n";
	static_hello.print();
	static_world.print();
	hello hello1 = new hello("hello1");
	hello world1 = new hello("world1");
	hello1.print();
	world1.print();
	hello another_hello = hello1;
	another_hello.greeting = "another greeting";
	hello1.print();
	hello test = new hello();
	test.print();
	Write($"the value of pi in Math is {PI} \n");
	Write("the value of pi in Math is {0} and e is {1}\n", System.Math.PI, E);
	double sqrt2 = Sqrt(2);
	Write($"sqrt2^2 = {sqrt2*sqrt2}\n");
	Write($"1/2 = {1/2}\n");
	Write($"1./2 = {1.0/2}\n");
	}
}
