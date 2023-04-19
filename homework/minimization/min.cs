using System;
using System.Threading;
using static System.Console;
using static System.Math; 
using System.IO;

public static class minimizer{
    public static vector qnewton(
        Func<vector,double> f, /* objective function */
        vector start, /* starting point */
        double acc /* accuracy goal, on exit |gradient| should be < acc */
    ){
        int dim = start.size;
        matrix B = new matrix(dim,dim);
        for(int i = 0; i < dim; i++){
            B[i,i] = 1;
        }
        
        return new vector(7);
    }
/*
    set the inverse Hessian matrix to unity, B = 1
    repeat until converged (e.g. ∥∇ϕ∥ < tolerance) :
        calculate the Newton’s step ∆x = −B∇ϕ
        do linesearch starting with λ = 1 :
            if ϕ(x + λ∆x) < ϕ(x) accept the step:
                x = x + λ∆x
                update B = B + δB
                break linesearch
            λ = λ/2
            if λ is too small accept the step and reset B:
                x = x + λ∆x
                B = 1
                break linesearch
            continue linesearch*/
}