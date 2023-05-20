out.txt Plots/Sin.svg Plots/DampedPendulum.svg Plots/Threebody.gif: main.exe Makefile sin.gpi dampedPendulum.gpi threebody.gpi
	mono main.exe > out.txt
	gnuplot sin.gpi
	gnuplot dampedPendulum.gpi
	gnuplot threebody.gpi


main.exe : main.cs matrix.dll odesolver.dll genlist.dll
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

odesolver.dll : odesolver.cs matrix.dll genlist.dll
	mcs -target:library -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

clean : 
	rm -f main.exe out.txt odesolver.dll txts/* Plots/*