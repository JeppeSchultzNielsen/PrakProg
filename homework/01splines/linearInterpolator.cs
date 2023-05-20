using System;
using System.Threading;
using static System.Console;
using static System.Math; 

public static class linearInterpolator{

	public static double linterp(double[] x, double[] y, double z){
        int i=binsearch(x,z);
        double dx=x[i+1]-x[i]; if(!(dx>0)) throw new Exception("uups...");
        double dy=y[i+1]-y[i];
        return y[i]+dy/dx*(z-x[i]);
    }

	public static int binsearch(double[] x, double z){
    /* locates the interval for z by bisection */ 
		if(!(x[0]<=z && z<=x[x.Length-1])) throw new Exception("binsearch: bad z");
		int i=0, j=x.Length-1;
		while(j-i>1){
			int mid=(i+j)/2;
			if(z>x[mid]) i=mid; else j=mid;
			}
		return i;
	}

    public static double linterpInteg(double[] x, double[] y, double z){
		int lastIndex = binsearch(x,z);
		double finalY = linterp(x,y,z);
		double area = 0;
		for(int i = 0; i <= lastIndex; i++){
			if(i == lastIndex){
				//calculate area between x[lastIndex] and z
				area += twoPointInt(x[lastIndex],z,y[lastIndex],finalY);
			}
			else{
				//calculate area between x[i] and x[i+1]
				area += twoPointInt(x[i],x[i+1],y[i],y[i+1]);
			}
		}
		return area;
	}

	static double twoPointInt(double x1, double x2, double y1, double y2){
		double p = (y2-y1)/(x2-x1);
		return y1*(x2-x1) + 0.5 * p * (x2-x1) * (x2-x1);
	}
}
