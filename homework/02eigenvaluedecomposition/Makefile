Plots/Dr.svg Plots/Rmax.svg Plots/Wfuncs.svg out.txt : dr.gpi rmax.gpi wfuncs.gpi main.exe
	mono main.exe > out.txt
	gnuplot dr.gpi
	gnuplot rmax.gpi
	gnuplot wfuncs.gpi



main.exe : main.cs matrix.dll jacobi.dll hydrogen.cs
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

jacobi.dll : jacobi.cs matrix.dll
	mcs -target:library -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))



clean : 
	rm -f main.exe out.txt jacobi.dll txts/* *.svg Plots/*