dr.svg : dr.gpi out.txt rmax.svg wfuncs.svg
	gnuplot $< 

rmax.svg : rmax.gpi out.txt
	gnuplot $< 

wfuncs.svg : wfuncs.gpi out.txt
	gnuplot $< 

out.txt : main.exe Makefile 
	mono main.exe > out.txt

main.exe : main.cs matrix.dll jacobi.dll hydrogen.cs
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

jacobi.dll : jacobi.cs matrix.dll
	mcs -target:library -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))



clean : 
	rm -f main.exe out.txt jacobi.dll txts/* *.svg