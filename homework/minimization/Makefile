out.txt : main.exe Makefile
	mono main.exe > out.txt

main.exe : main.cs matrix.dll min.dll
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

min.dll : min.cs matrix.dll
	mcs -target:library -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

clean : 
	rm -f main.exe out.txt