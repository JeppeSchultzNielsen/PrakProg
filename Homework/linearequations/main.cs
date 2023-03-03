using System;
using System.Threading;
using static System.Console;
using static System.Math; 

public static class main{

	public static void Main(){
		matrix A = new matrix("3,4,5;7,8,9;10,11,12;17,14,15;16,17,19");
		matrix R = new matrix(A.size2,A.size2);
		WriteLine("Matrix A is:");
		A.print();
		qr.QRGSdecomp(A,R);
		WriteLine("After QR decomposition, Q:");
		A.print();
		WriteLine("After QR decomposition, R:");
		R.print();
		WriteLine("Q^T * Q:");
		(A.transpose() * A).print();
		WriteLine("Q*R");
		matrix C = A*R;
		C.print();

		var rnd = new System.Random(1); /* or any other seed */

		WriteLine("Large randomly generated matrix to solve:");
		int size = 20;
		matrix rand = new matrix(size,size);
		matrix tri = new matrix(size,size);
		for(int i = 0; i < size; i++){
			for(int j = 0; j < size; j++){
				rand[i,j] = 2*rnd.NextDouble() - 1;
			}
		}
		rand.print();

		WriteLine("Large randomly generated vector to solve system of equations with:");
		vector randVec = new vector(size);
		for(int i=0; i < size; i++){
			randVec[i] = 2*rnd.NextDouble() - 1;
		}
		randVec.print();

		WriteLine("Solving with QRdecomposition for x:");
		qr.QRGSdecomp(rand,tri);
		vector x = qr.QRGSsolve(rand,tri,randVec);
		x.print();
		WriteLine("We now have A*x = Q*R*x =");
		vector solVec = rand*tri*x;
		(solVec).print();
		WriteLine("Which is the same as the previously randomly generated vector, indicating we have the solution for x.");
		WriteLine("Part 2:");
		WriteLine("The inverse of the previously generated large matrix is");
		matrix inv = qr.QRGSinvert(rand,tri);
		inv.print();
		WriteLine("The product of the large matrix with its inverse is");
		matrix id = rand*tri*inv;
		id.print();
		WriteLine("Which is the identity matrix, which must mean that we have found the inverse.");
	}
}
