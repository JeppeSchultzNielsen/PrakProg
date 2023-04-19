out.txt Wavefunctions.svg Rminconvergence.svg Rmaxconvergence.svg Relaccconvergence.svg Absaccconvergence.scg: main.exe Makefile wavefunctions.gpi rminconvergence.gpi rmaxconvergence.gpi relaccconvergence.gpi absaccconvergence.gpi
	mono main.exe > out.txt
	gnuplot wavefunctions.gpi
	gnuplot rminconvergence.gpi
	gnuplot rmaxconvergence.gpi
	gnuplot relaccconvergence.gpi
	gnuplot absaccconvergence.gpi

main.exe : main.cs matrix.dll rootfinder.dll odesolver.dll
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

rootfinder.dll : rootfinder.cs matrix.dll qr.dll
	mcs -target:library -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

clean : 
	rm -f main.exe out.txt rootfinder.dll txts/* *.svg