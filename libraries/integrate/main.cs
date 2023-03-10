using static System.Math;
using static System.Console;
class main{
const double inf=double.PositiveInfinity;
public static bool approx
(double x, double y, double acc=1e-6, double eps=1e-6){
        if(Abs(x-y)<acc)return true;
        if(Abs(x-y)<eps*(Abs(x)+Abs(y))/2)return true;
        return false;
}
static int Main(){

int ncalls=0,ierr=0;
double q,exact,acc,eps;
System.Func<double,double> f;


acc=1e-6; eps=0; exact = 2.0/5*(1-Exp(-PI));
WriteLine($"quad: testing ∫_0^PI Exp(-x)Sin(x)^2dx={exact},acc={acc},eps={eps}");
f = delegate(double x){ ncalls++; return Exp(-x)*Sin(x)*Sin(x);};
ncalls=0; q=integrate.o8a(f,0,PI,acc,eps);
WriteLine($"result = {q}, relative error ={Abs(q-exact)/exact} ncalls={ncalls}");
if(approx(q,exact,acc,eps))WriteLine("test passed\n");
else {ierr++;WriteLine("test failed\n");}

acc=1e-6; eps=0; exact = Sqrt(PI);
WriteLine($"quad: testing ∫_-inf^inf exp(-x^2)dx={exact},acc={acc},eps={eps}");
f = delegate(double x){ ncalls++; return Exp(-x*x);};
ncalls=0; q=integrate.quad(f,-inf,inf,acc,eps);
WriteLine($"result = {q}, relative error ={Abs(q-exact)/exact} ncalls={ncalls}");
if(approx(q,exact,acc,eps))WriteLine("test passed\n");
else {ierr++;WriteLine("test failed\n");}

acc=1e-6; eps=0; exact = Sqrt(PI)/2;
WriteLine($"quad: testing ∫_0^inf exp(-x^2)dx={exact},acc={acc},eps={eps}");
f = delegate(double x){ ncalls++; return Exp(-x*x);};
ncalls=0; q=integrate.quad(f,0,inf,acc,eps);
WriteLine($"result = {q}, relative error ={Abs(q-exact)/exact} ncalls={ncalls}");
if(approx(q,exact,acc,eps))WriteLine("test passed\n");
else {ierr++;WriteLine("test failed\n");}

acc=1e-6; eps=0; exact = 2;
WriteLine($"quad: testing ∫_0^1 1/Sqrt(x)dx={exact}, acc={acc} eps={eps}");
f = delegate(double x){ ncalls++; return 1/Sqrt(x);};
ncalls=0; q=integrate.quad(f,0,1,acc,eps);
WriteLine($"result = {q}, relative error ={Abs(q-exact)/exact} ncalls={ncalls}");
if(approx(q,exact,acc,eps))WriteLine("test passed\n");
else {ierr++;WriteLine("test failed\n");}

acc=1e-9; eps=0; exact = -4;
WriteLine($"quad: ∫_0^1 Log(x)/Sqrt(x)dx={exact}, acc={acc} eps={eps}");
f = delegate(double x){ ncalls++; return Log(x)/Sqrt(x);};
ncalls=0; q=integrate.quad(f,0,1,acc,eps);
WriteLine($"result = {q}, relative error ={Abs(q-exact)/exact} ncalls={ncalls}");
if(approx(q,exact,acc,eps))WriteLine("test passed\n");
else {ierr++;WriteLine("test failed\n");}

WriteLine($"failed tests: {ierr}");

acc=1e-6; eps=0; exact = 2.0/5*(1-Exp(-PI));
WriteLine($"adapt3o: testing ∫_0^PI Exp(-x)Sin(x)^2dx={exact},acc={acc},eps={eps}");
f = delegate(double x){ ncalls++; return Exp(-x)*Sin(x)*Sin(x);};
ncalls=0; q=quad.adapt3o(f,0,PI,acc,eps);
WriteLine($"result = {q}, relative error ={Abs(q-exact)/exact} ncalls={ncalls}");
if(approx(q,exact,acc,eps))WriteLine("test passed\n");
else {ierr++;WriteLine("test failed\n");}

return ierr;
}
}
