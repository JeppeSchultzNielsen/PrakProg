out.txt Plots/Interpolation.svg Plots/SinInterpolation.svg Plots/DiffEqSin.svg Plots/DiffEqCos.svg: main.exe Makefile interpolation.gpi sinInterpolation.gpi diffEqSin.gpi diffEqCos.gpi
	mono main.exe > out.txt
	gnuplot interpolation.gpi
	gnuplot sinInterpolation.gpi
	gnuplot diffEqSin.gpi
	gnuplot diffEqCos.gpi

main.exe : main.cs ann.dll matrix.dll min.dll
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

ann.dll : ann.cs matrix.dll min.dll integrator.dll
	mcs -target:library -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

clean : 
	rm -f main.exe out.txt txts/* Interpolation.svg SinInterpolation.svg Plots/*