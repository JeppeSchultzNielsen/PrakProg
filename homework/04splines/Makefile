out.txt Plots/Linterp.svg Plots/Qinterp.svg Plots/Cinterp.scg: main.exe Makefile linterp.gpi qinterp.gpi cinterp.gpi
	mono main.exe > out.txt
	gnuplot linterp.gpi
	gnuplot qinterp.gpi
	gnuplot cinterp.gpi
	rm -f table_100

main.exe : main.cs linearInterpolator.dll qspline.dll cspline.cs matrix.dll
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

linearInterpolator.dll : linearInterpolator.cs
	mcs -target:library -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

qspline.dll : qspline.cs
	mcs -target:library -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

cspline.dll : cspline.cs matrix.dll
	mcs -target:library -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

clean : 
	rm -f main.exe out.txt linearInterpolator.dll txts/* Linterp.svg Qinterp.svg qspline.dll Cinterp.svg table_100 Plots/*
