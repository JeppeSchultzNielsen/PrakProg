using System;
using System.Threading;
using static System.Console;
using static System.Math; 

public class cspline {
	double[] xs;
    double[] ys;
    public vector bs;
    public vector cs;
    public vector ds;

	public cspline(double[] xs,double[] ys){
		this.xs=new double[xs.Length]; 
        xs.CopyTo(this.xs,0);
        this.ys=new double[ys.Length]; 
        ys.CopyTo(this.ys,0);
        
        bs = new vector(xs.Length);
        cs = new vector(xs.Length-1);
        ds = new vector(xs.Length-1);

        double[] ps = new double[xs.Length-1];
        double[] hs = new double[xs.Length-1];

        //calculate b, c and d.
        cs[0] = 0;
        for(int i = 0; i < cs.size; i++){
            double dy = ys[i+1]-ys[i];
            double dx = xs[i+1]-xs[i];
            ps[i] = dy/dx;
            hs[i]=dx;
        }

        //make matrix and vector
        matrix A = new matrix(xs.Length,xs.Length);
        vector B = new vector(xs.Length);
        A[0,0] = 2;
        A[xs.Length-1,xs.Length-1] = 2;
        B[0] = 3*ps[0];
        B[xs.Length-1] = 3*ps[xs.Length-2];
        A[0,1] = 1;
        A[1,0] = 1;
        for(int i = 0; i < xs.Length-1; i++){
            if(i > 0){
                A[i,i] = 2*hs[i-1]/hs[i]+2;
                B[i] = 3*(ps[i-1]+ps[i]*hs[i-1]/hs[i]);
                A[i,i+1] = hs[i-1]/hs[i];
            }
            
            A[i+1,i] = 1;
        }
        bs = solvetridiag(A,B);
        for(int i = 0; i < cs.size; i++){
            cs[i] = (- 2*bs[i] - bs[i+1]+3*ps[i])/hs[i];
            ds[i] = (bs[i] + bs[i+1] - 2*ps[i])/hs[i]/hs[i];
        }
	}

    public vector solvetridiag(matrix m, vector d){
        int dim = m.size1;
        vector x = new vector(dim);
        vector cp = new vector(dim);
        vector dp = new vector(dim);
        vector a = new vector(dim);
        vector b = new vector(dim);
        vector c = new vector(dim);
        cp[0] = m[0,1]/m[0,0];
        dp[0] = d[0]/m[0,0];
        for(int i = 0; i < dim; i++){
            b[i] = m[i,i];
            if(i < dim-1) a[i+1] = m[i+1,i];
            if(i > 0) c[i-1] = m[i-1,i];
        }
        for(int i = 1; i < dim; i++){
            cp[i] = c[i]/(b[i] - a[i]*cp[i-1]);
            dp[i] = (d[i]- a[i]*dp[i-1])/(b[i]-a[i]*cp[i-1]);
        }
        x[dim-1] = dp[dim-1];
        for(int i = 1; i < dim; i++){
            int j = dim- 1 - i;
            x[j] = dp[j]-cp[j]*x[j+1];
        }
        return x; 
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
        return ys[i] + bs[i]*(z-xs[i]) + cs[i]*(z-xs[i])*(z-xs[i]) + ds[i]*(z-xs[i])*(z-xs[i])*(z-xs[i]);

    }
	public double derivative(double z){
        int i = binsearch(z);
        return bs[i] + 2*cs[i]*(z-xs[i]) + 3*ds[i]*(z-xs[i])*(z-xs[i]);
    }

	public double integral(double z){
        int lastIndex = binsearch(z);
		double area = 0;
		for(int i = 0; i <= lastIndex; i++){
			if(i == lastIndex){
				//calculate area between x[lastIndex] and z
				area += ys[i]*(z-xs[i])+0.5*bs[i]*Pow((z-xs[i]),2) + 1.0/3.0 * cs[i] * Pow((z-xs[i]),3) + 1.0/4.0 * cs[i] * Pow((z-xs[i]),4);
			}
			else{
				//calculate area between x[i] and x[i+1]
				area += ys[i]*(xs[i+1]-xs[i])+0.5*bs[i]*Pow((xs[i+1]-xs[i]),2) + 1.0/3.0 * cs[i] * Pow((xs[i+1]-xs[i]),3) + 1.0/4.0 * cs[i] * Pow((xs[i+1]-xs[i]),4);
			}
		}
		return area;
    }
}