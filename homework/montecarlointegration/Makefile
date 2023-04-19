out.txt Convergence.svg: main.exe Makefile convergence.gpi
	mono main.exe > out.txt
	gnuplot convergence.gpi


main.exe : main.cs mcint.dll matrix.dll
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

mcint.dll : mcint.cs matrix.dll
	mcs -target:library -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

clean : 
	rm -f main.exe out.txt mcint.dll Convergence.svg convergence.txt test.txt