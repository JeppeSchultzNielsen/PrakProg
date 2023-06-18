out.txt : main.exe Makefile errorFcalls.gpi errorBoundCalls.gpi results.gpi errorFcallsDiscon.gpi errorBoundCallsDiscon.gpi resultsDiscon.gpi
	mono main.exe > out.txt
	gnuplot errorFcalls.gpi
	gnuplot errorBoundCalls.gpi
	gnuplot results.gpi
	gnuplot errorFcallsDiscon.gpi
	gnuplot errorBoundCallsDiscon.gpi
	gnuplot resultsDiscon.gpi


main.exe : main.cs 2dintegrator.dll integrator.dll
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

2dintegrator.dll : 2dintegrator.cs integrator.dll matrix.dll
	mcs -target:library -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

clean : 
	rm -f main.exe out.txt 2dintegrator.dll txts/* plots/*