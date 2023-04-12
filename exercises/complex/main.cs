using System;
using static System.Console;
using static System.Math;
using static cmath;

class main{
    public static int Main(string[] args){
        WriteLine($"Sqrt(-1) should be i, is {sqrt(new complex(-1,0))}");
        WriteLine($"Sqrt(i) should be (1+i)/sqrt(2), is {sqrt(I)}");
        WriteLine($"e^i should be cos(1) + i sin(1), is {exp(I)}");
        WriteLine($"e^(pi i) should be -1, is {exp(I*PI)}");
        WriteLine($"i^i should be e^(-pi/2)=0.208, is {I.pow(I)}");
        WriteLine($"ln(i) should be i pi / 2, is {log(I)}");
        WriteLine($"sin(i pi) should be i*sinh(pi) = 11.54i, is {sin(I*PI)}");
        return 0;
    }
}