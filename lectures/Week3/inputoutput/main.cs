using System;
using static System.Console;
using static System.Math;

class main{
    public static int Main(string[] args){
        Write("hello\n");

        string infile = null;
        string outfile = null;
        foreach(string arg in args){
            WriteLine(arg);
            var words = arg.Split(":");
            if(words[0] == "-input"){
                infile = words[1]; 
            }
            if(words[0] == "-output"){
                outfile = words[1]; 
            }
        }

        if(infile == null){
            Error.WriteLine("no input file");
            return 1; 
        }

        if(outfile == null){
            Error.WriteLine("no output file");
            return 1; 
        }


        var inputstream = new System.IO.StreamReader(infile);
        var outputstream  = new System.IO.StreamWriter(outfile,append:false);

        for(string line = inputstream.ReadLine(); line != null; line = inputstream.ReadLine()){
            double x = double.Parse(line);
            outputstream.WriteLine($"{x} {Sin(x)}");
        }

        inputstream.Close();
        outputstream.Close();


        double[] numbers = input.get_numbers_from__args(args);

        foreach(double number in numbers){
            WriteLine($"{number:0.00e+00}");
        }

        System.Console.Error.WriteLine("return code 0");
        return 0;
    }
}