using System;
using System.Threading;
using static System.Console;
using static System.Math; 

public static class qr{
    public static void QRGSdecomp(matrix A, matrix R){
        for(int i =0; i<A.size2; i ++){
            R[i,i]=A[i].norm();
            A[i]/=R[i,i]; //normalize each vector in A. 
            for (int j=i+1; j<A.size2; j ++){
                R[i,j]=A[i].dot(A[j]); //find projection of A_j on A_i and subtract it from A_j, making it orthogonal to A_i.
                A[j]-=A[i]*R[i,j];     
            }
            //in the end, R upper triangular
        }
    }

    public static vector QRGSsolve(matrix Q, matrix R, vector b){
        //We have A*x = b => Q*R*x = b => R * x= Q^T b
        //R is upper triangular
        b = Q.transpose() * b;
        for(int i = R.size1-1; i >= 0; i--){ //start at bottom
            double sum = 0;
            for(int j = i+1; j < R.size2 ; j++){
                sum += R[i,j] * b[j]; //use all previously found x's to calculate next x; back-substitution 
            }
            b[i] = (b[i] - sum)/R[i,i];
        }
        return b; 
    }

    public static matrix QRGSinvert(matrix Q, matrix R){
        matrix inverted = new matrix(Q.size1,Q.size2);
        //for each column in B there is a system of equations to solve, with the B-column unknown.
        for(int i = 0; i < Q.size1; i++){
            //for this i, the vector is 0, 0 ... i 0's ... 1 , 0, 0...
            vector solVec = new vector(Q.size1);
            solVec[i] = 1; 
            //now solve the problem for the i'th column. 
            inverted[i] = QRGSsolve(Q,R,solVec); 
        }
        return inverted;
    }
}
