out.txt : main.exe Makefile
	mono main.exe > out.txt

main.exe : main.cs 2dintegrator.dll integrator.dll
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

2dintegrator.dll : 2dintegrator.cs integrator.dll matrix.dll
	mcs -target:library -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

clean : 
	rm -f main.exe out.txt 2dintegrator.dll