out.txt Higgs.svg: main.exe Makefile higgs.gpi
	mono main.exe < higgs.data 1> out.txt
	gnuplot higgs.gpi

main.exe : main.cs matrix.dll min.dll
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

min.dll : min.cs matrix.dll
	mcs -target:library -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

clean : 
	rm -f main.exe out.txt