using System;
using System.Threading;
using static System.Console;
using static System.Math; 

public static class timeTester{
	public static void Main(string[] args){
        int size = 0;
        foreach(var arg in args){
            var words = arg.Split(":"); 
            if(words[0]=="-size") size = (int)float.Parse(words[1]);
        }
        //create large random matrix
        var rnd = new System.Random(1); /* or any other seed */
        matrix rand = new matrix(size,size);
		matrix tri = new matrix(size,size);
        for(int i = 0; i < size; i++){
			for(int j = 0; j < size; j++){
				rand[i,j] = 2*rnd.NextDouble() - 1;
			}
		}
        qr.QRGSdecomp(rand,tri);
	}   
}
