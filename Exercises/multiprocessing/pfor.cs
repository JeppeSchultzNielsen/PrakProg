using System;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;
using static System.Math; 

public static class main{


	public static int Main(string[] args){
        int nterms = (int)5e8, nthreads = 1; 
        foreach(var arg in args){
            var words = arg.Split(":"); 
            if(words[0]=="-terms") nterms = (int)float.Parse(words[1]);
            if(words[0]=="-threads") nthreads = (int)float.Parse(words[1]);
        }
        WriteLine($"Calculating with Parallel.For nterms={nterms} nthreads={nthreads}");

        double sum = 0; 
        //for(int i = 1; i < nterms+1; i++){sum+=1.0/i;}

        Parallel.For(1, nterms+1, delegate(int i){sum += 1.0/i;});

        WriteLine($"nterms = {nterms}, sum = {sum}");

        WriteLine("Parallel.For gives wrong results (and is slower) because in the way it is currently written using a global variable which all of the threads try to access which is a race condition. It is also implemented using a delegate, which is slower than just doing the arithmetic in the ordinary for-loops because of the function call.");
        return 0;
	}
}