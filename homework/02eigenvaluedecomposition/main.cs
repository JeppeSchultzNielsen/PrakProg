using System;
using System.Threading;
using static System.Console;
using static System.Math; 
using System.IO;

public static class main{

	public static void Main(){
		var rnd = new System.Random(1); /* or any other seed */
		WriteLine("---------");
		WriteLine("Part a)"); 
		WriteLine("Large randomly generated symmetric matrix to solve, A:");
		int size = 20;
		matrix rand = new matrix(size,size);
		matrix v = new matrix(size,size);
		for(int i = 0; i < size; i++){
			v[i,i] = 1;
			for(int j = i; j < size; j++){
				var nr = 2*rnd.NextDouble() - 1;
				rand[i,j] = nr;
				rand[j,i] = nr; 
			}
		}
		matrix A = rand.copy();

		rand.print();
		WriteLine("Find solution using Jacobi eigenvalue algorithm using the cyclic sweeps");
		jacobi.cyclic(rand,v);
		WriteLine("D is diagonal:");
		rand.print();

		WriteLine("V^T A V is the same as D: ");
		matrix prod = v.transpose() * A * v;
		prod.print();

		WriteLine("V D V^T is the same as A:");
		prod = v * rand * v.transpose();
		prod.print();

		WriteLine("V V^T is identity:");
		prod = v * v.transpose();
		prod.print();

		WriteLine("V^T V is identity:");
		prod = v.transpose() * v;
		prod.print();

		WriteLine("---------");
		WriteLine("Part b)"); 

		WriteLine("Creating txt-file for plotting e0 for several different dr with r_max = 10 at txts/dr.txt");
		string toWrite = "";
		for(int i = 0; i < 50; i++){
			double dr = (i+1)/100.0+0.05;
			hydrogen.solveHydrogen(10,dr);
			toWrite += $"{dr}\t{hydrogen.lowestE}\n";
		}
		File.WriteAllText("txts/dr.txt", toWrite);

		WriteLine("Creating txt-file for plotting e0 for several different r_max with dr = 0.1 at txts/rmax.txt");
		toWrite = "";
		for(int i = 0; i < 50; i++){
			double r_max = 2*i/10.0+2;
			hydrogen.solveHydrogen(r_max,0.1);
			toWrite += $"{r_max}\t{hydrogen.lowestE}\n";
		}
		File.WriteAllText("txts/rmax.txt", toWrite);

		WriteLine("Creating txt-file for storing wave function with r_max = 5 and dr = 0.1 at txts/rmax05dr01.txt");
		hydrogen.solveHydrogen(5,0.1);
		toWrite = "";
		for(int i = 0; i < hydrogen.r.size; i++){
			toWrite += $"{hydrogen.r[i]}\t{hydrogen.lowestWaveFunction[i]}\t{hydrogen.lowestWaveFunction[i]/hydrogen.r[i]}\n";
		}
		File.WriteAllText("txts/rmax05dr01.txt", toWrite);

				WriteLine("Creating txt-file for storing wave function with r_max = 10 and dr = 0.1 at txts/rmax10dr01.txt");
		hydrogen.solveHydrogen(10,0.1);
		toWrite = "";
		for(int i = 0; i < hydrogen.r.size; i++){
			toWrite += $"{hydrogen.r[i]}\t{hydrogen.lowestWaveFunction[i]}\t{hydrogen.lowestWaveFunction[i]/hydrogen.r[i]}\n";
		}
		File.WriteAllText("txts/rmax10dr01.txt", toWrite);

		WriteLine("Creating txt-file for storing wave function with r_max = 5 and dr = 0.2 at txts/rmax05dr02.txt");
		hydrogen.solveHydrogen(5,0.2);
		toWrite = "";
		for(int i = 0; i < hydrogen.r.size; i++){
			toWrite += $"{hydrogen.r[i]}\t{hydrogen.lowestWaveFunction[i]}\t{hydrogen.lowestWaveFunction[i]/hydrogen.r[i]}\n";
		}
		File.WriteAllText("txts/rmax05dr02.txt", toWrite);

		WriteLine("Creating txt-file for storing wave function with r_max = 10 and dr = 0.2 at txts/rmax10dr02.txt");
		hydrogen.solveHydrogen(10,0.2);
		toWrite = "";
		for(int i = 0; i < hydrogen.r.size; i++){
			toWrite += $"{hydrogen.r[i]}\t{hydrogen.lowestWaveFunction[i]}\t{hydrogen.lowestWaveFunction[i]/hydrogen.r[i]}\n";
		}
		File.WriteAllText("txts/rmax10dr02.txt", toWrite);

		WriteLine("The analytical wavefunction is just an exponential function, writing to txts/analytical.txt");
		toWrite = "";
		for(int i = 0; i < 100; i++){
			toWrite += $"{i/10.0}\t{Exp(-i/10.0)}\n";
		}
		File.WriteAllText("txts/analytical.txt", toWrite);
		WriteLine("Barring normalization, we see that the found numerical solutions agree with the analytical.");
	}
}
