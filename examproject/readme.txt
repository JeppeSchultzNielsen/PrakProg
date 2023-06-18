This directory contains the examination project for Jeppe Schultz Nielsen, 20190834. The exercise is 34mod26=8: Adaptive two-dimensional integrator.

Implement a two-dimensional integrator for integrals in the form

∫_a ^b dx ∫_d(x) ^u(x) dy f(x,y)

which applies your favourite adaptive one-dimensional integrator along each of the two dimensions. The signature might be something like

static double integ2D(
	Func<double,double,double> f,
	double a, double b,
	Func<double,double> d,
	Func<double,double> u,
	double acc, double eps)

________

The general idea is to transform the 2-dimensional problem into two 1-dimensional problems:
∫_a ^b dx ∫_d(x) ^u(x) dy f(x,y) -> ∫_a ^b dx g(x) with g(x) = ∫_d(x) ^u(x) dy f(x,y)
Then the adaptive integrator from the homework can be used. 
Here adaptive integration is done by comparing results obtained by the trapezium and rectangle rules.
I have made the following exercises for myself:

_________

Part A (6 points):
Implement a two-dimensional integrator for integrals on rectangular areas in the xy-plane in the form
∫_a ^b dx ∫_c ^d dy f(x,y)
Make some tests to see if it works as it should. 

Part B (3 points):
Implement a two-dimensional integrator for integrals on general areas in the xy-plane in the form
∫_a ^b dx ∫_d(x) ^u(x) dy f(x,y)
Test it by integrating some functions on circles in the x,y plane in both polar and cartesian coordinates.

Part C (1 point):
Extend the two-dimensional integrator to also return the error and the number of function calls. 
Extend the N-dimensional Monte-Carlo integrator from the homework to be able to calculate integrals of the same form. 
Compare the error as a function of function calls for the two integrators.

________

I have completed all the tasks. The results can be seen in "out.txt" and the "plots" directory. 
The command "make" takes approximately 11s on my machine. 