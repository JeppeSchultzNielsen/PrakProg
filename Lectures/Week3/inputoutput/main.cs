using System;
using static System.Console;
using static System.Math;

class main{
    public static int Main(string[] args){
        Write("hello\n");

        foreach(string arg in args){
            WriteLine(arg);
        }
        double[] numbers = input.get_numbers_from__args(args);

        foreach(double number in numbers){
            WriteLine($"{number:0.00e+00}");
        }
        return 0;
    }
}