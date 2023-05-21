out.txt Plots/Wavefunctions.svg Plots/RminconvergenceImp.svg Plots/RmaxconvergenceImp.svg Plots/RelaccconvergenceImp.svg Plots/AbsaccconvergenceImp.svg Plots/WavefunctionsImp.svg: main.exe Makefile wavefunctions.gpi rminconvergence.gpi rmaxconvergence.gpi relaccconvergence.gpi absaccconvergence.gpi rminconvergenceImp.gpi rmaxconvergenceImp.gpi relaccconvergenceImp.gpi absaccconvergenceImp.gpi wavefunctionsImp.gpi
	mono main.exe > out.txt
	gnuplot wavefunctions.gpi
	gnuplot rminconvergence.gpi
	gnuplot rmaxconvergence.gpi
	gnuplot relaccconvergence.gpi
	gnuplot absaccconvergence.gpi
	gnuplot rminconvergenceImp.gpi
	gnuplot rmaxconvergenceImp.gpi
	gnuplot relaccconvergenceImp.gpi
	gnuplot absaccconvergenceImp.gpi
	gnuplot wavefunctionsImp.gpi

main.exe : main.cs matrix.dll rootfinder.dll odesolver.dll genlist.dll
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

rootfinder.dll : rootfinder.cs matrix.dll qr.dll
	mcs -target:library -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

clean : 
	rm -f main.exe out.txt rootfinder.dll txts/* *.svg Plots/*