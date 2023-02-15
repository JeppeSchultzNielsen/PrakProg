using System;
using static System.Console;
using static System.Math;

public class vec{
	public double x,y,z;
	public vec(double a, double b, double c){
		x=a;
		y=b;
		z=c; 
	}

	public vec(){ x=y=z=0;}

	public void print(string s){
		Write(s);
		Write($"{x} {y} {z} \n");
	}

	public static vec operator+(vec u, vec v){
		/* u + v */
		return new vec(u.x+v.x, u.y+v.y, u.z+v.z);
	}

	public static vec operator-(vec u, vec v){
                /* u + v */
                return new vec(u.x-v.x, u.y-v.y, u.z-v.z);
        }

        public static vec operator-(vec v){
                /* -u */
                return new vec(-v.x, -v.y, -v.z);
        }


	public static vec operator*(vec u, double c){
		/* u*c */
		return new vec(u.x*c, u.y*c, u.z*c);
	}

	public static vec operator*(double c, vec u){
                /* c*u */
                return new vec(u.x*c, u.y*c, u.z*c);
        }

        public static double operator%(vec v, vec u){
                /* v dot u */
                return u.x*v.x + u.y*v.y + u.z*v.z;
        }

        public double dot(vec other){
                /* v dot u */
                return this%other;
        }

	public double norm(){
		/* norm of vector*/
		return Sqrt(this%this);
	}

	public vec cross(vec other){
		/*crossproduct*/
		double x = this.y*other.z - this.z*other.y;
		double y = this.z*other.x - this.x*other.y;
		double z = this.x*other.y - this.y*other.x;
		return new vec(x,y,z);
	}


	public static vec cross(vec a, vec b){
		return a.cross(b);
	}

	static bool approx(double a,double b,double acc=1e-9,double eps=1e-9){
		if(Abs(a-b)<acc)return true;
		if(Abs(a-b)<(Abs(a)+Abs(b))*eps)return true;
		return false;
	}

	public bool approx(vec other){
		if(!approx(this.x,other.x)) return false;
		if(!approx(this.y,other.y)) return false;
		if(!approx(this.z,other.z)) return false;
		return true;
	}

	public override string ToString(){
		return $"{x} {y} {z}"; 
	}
}
