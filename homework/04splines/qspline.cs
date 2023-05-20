using System;
using System.Threading;
using static System.Console;
using static System.Math; 

public class qspline {
	double[] xs;
    double[] ys;
    public double[] bs;
    public double[] cs;

	public qspline(double[] xs,double[] ys){
		this.xs=new double[xs.Length]; 
        xs.CopyTo(this.xs,0);
        this.ys=new double[ys.Length]; 
        ys.CopyTo(this.ys,0);
        
        bs = new double[xs.Length-1];
        cs = new double[xs.Length-1];

        double[] ps = new double[xs.Length-1];

        //calculate b and c
        cs[0] = 0;
        for(int i = 0; i < cs.Length; i++){
            double dy = ys[i+1]-ys[i];
            double dx = xs[i+1]-xs[i];
            ps[i] = dy/dx;
        }
        for(int i = 0; i < cs.Length-1; i++){
            double dxp1 = xs[i+2]-xs[i+1];
            double dx = xs[i+1]-xs[i];
            cs[i+1] = 1.0/dxp1*(ps[i+1]-ps[i]-cs[i]*dx);
        }
        //now recalculate cs by running backwards through the array
        cs[cs.Length-1] = cs[cs.Length-1]/2;
        for(int i = 0; i < cs.Length-1; i++){
            int j = cs.Length-2-i;
            double dxp1 = xs[j+2]-xs[j+1];
            double dx = xs[j+1]-xs[j];
            cs[j] = 1.0/dx*(ps[j+1]-ps[j]-cs[j+1]*dxp1);
        }

        //calculate bs
        for(int i = 0; i < bs.Length; i++){
            double dx = xs[i+1]-xs[i];
            bs[i]=ps[i]-cs[i]*dx;
        }
	}

	public int binsearch(double z){
    /* locates the interval for z by bisection */ 
		if(!(xs[0]<=z && z<=xs[xs.Length-1])) throw new Exception("binsearch: bad z");
		int i=0, j=xs.Length-1;
		while(j-i>1){
			int mid=(i+j)/2;
			if(z>xs[mid]) i=mid; else j=mid;
			}
		return i;
	}

	public double evaluate(double z){
        int i = binsearch(z);
        return ys[i] + bs[i]*(z-xs[i]) + cs[i]*(z-xs[i])*(z-xs[i]);

    }
	public double derivative(double z){
        int i = binsearch(z);
        return bs[i] + 2*cs[i]*(z-xs[i]);
    }

	public double integral(double z){
        int lastIndex = binsearch(z);
		double area = 0;
		for(int i = 0; i <= lastIndex; i++){
			if(i == lastIndex){
				//calculate area between x[lastIndex] and z
				area += ys[i]*(z-xs[i])+0.5*bs[i]*Pow((z-xs[i]),2) + 1.0/3.0 * cs[i] * Pow((z-xs[i]),3);
			}
			else{
				//calculate area between x[i] and x[i+1]
				area += ys[i]*(xs[i+1]-xs[i])+0.5*bs[i]*Pow((xs[i+1]-xs[i]),2) + 1.0/3.0 * cs[i] * Pow((xs[i+1]-xs[i]),3);
			}
		}
		return area;
    }
}