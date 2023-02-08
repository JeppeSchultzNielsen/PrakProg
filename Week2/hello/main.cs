using System;
using static System.Console;
using static System.Math;
class main{
	public static void Main(){
	Write("hello from main\n");
	static_hello.print();
	static_world.print();
	static_hello.greeting="new hello from main\n";
	static_world.greeting="new world from main\n";
	static_hello.print();
	static_world.print();
	}
}
