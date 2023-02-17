out.txt : main.exe
	mono main.exe > out.txt

main.exe : main.cs cmath.dll
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

cmath.dll : cmath.cs complex.cs
	mcs -target:library -out:./cmath.dll $^

clean: 
	rm -f main.exe out.txt cmath.dll