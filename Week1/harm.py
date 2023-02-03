def f(x): return 1/x

n = int(1e7)
print(n)
s = 0.0

for i in range(1,n+1) : s += f(i)

print(s)
