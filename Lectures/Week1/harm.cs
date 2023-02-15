class main{
static double f(int i){return 1.0/i;}

static int Main(){
int n = (int)1e7;
double s = 0; 
for(int i = 1; i <= n; i++){
s += f(i);
}
System.Console.WriteLine("s is equal to {0}",s);
return 0;
}
}
