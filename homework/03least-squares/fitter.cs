using System;
using System.Threading;
using static System.Console;
using static System.Math; 

public static class fitter{

	public static (vector,matrix) lsfit(Func<double,double>[] fs, vector x, vector y, vector dy){
		//create matrix A_ik:
		matrix A = new matrix(x.size, fs.Length);
		vector b = new vector(y.size);
		for(int i = 0; i < x.size; i++){
			b[i] = y[i]/dy[i];
			for(int j = 0; j < fs.Length; j++){
				A[i,j] = fs[j](x[i])/dy[i];
			}
		}
		//solve for parameter vector
		matrix v = new matrix(fs.Length, fs.Length);
		matrix A2 = A.transpose() * A;
		matrix v2 = new matrix(A2.size1, A2.size1);
		qr.QRGSdecomp(A,v);
		qr.QRGSdecomp(A2,v2);
		var s = qr.QRGSinvert(A2,v2);
		return (qr.QRGSsolve(A,v,b),s);
	}

}
