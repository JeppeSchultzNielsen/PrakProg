using System;
using System.Threading;
using static System.Console;
using static System.Math; 

public static class hydrogen{
    public static vector r;
    public static vector lowestWaveFunction;
    public static double lowestE;

    public static void solveHydrogen(double rmax, double dr){

        //create matrix
        int npoints = (int)(rmax/dr)-1;
        r = new vector(npoints);
        for(int i=0;i<npoints;i++)r[i]=dr*(i+1);
        matrix H = new matrix(npoints,npoints);
        matrix v = new matrix(npoints,npoints);
        for(int i=0;i<npoints-1;i++){
            v[i,i] = 1;
            H[i,i]  =-2;
            H[i,i+1]= 1;
            H[i+1,i]= 1;
        }
        v[npoints-1,npoints-1] = 1;
        H[npoints-1,npoints-1]=-2;
        matrix.scale(H, -0.5/dr/dr );
        for(int i=0;i<npoints;i++)H[i,i]+=-1/r[i];

        //solve eigenvalues
        jacobi.cyclic(H,v);

        //find smallest eigenvalue
        lowestE = 1e10;
        int lowestIndex = 0;
        for(int i = 0; i < npoints; i++){
            if(H[i,i] < lowestE){
                lowestE = H[i,i];
                lowestIndex = i;
            }
        }
        //eigenvector is corresponding column in V.
        lowestWaveFunction = v[lowestIndex];
    }
}