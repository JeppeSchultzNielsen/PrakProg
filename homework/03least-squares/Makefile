out.txt Plots/fit.svg : fit.gpi main.exe Makefile txts/data.txt
	mono main.exe > out.txt
	gnuplot fit.gpi

main.exe : main.cs matrix.dll qr.dll fitter.dll
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

fitter.dll : fitter.cs matrix.dll qr.dll
	mcs -target:library -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

txts/data.txt : Makefile
	>$@
	echo 1\\t117\\t5 >> $@
	echo 2\\t100\\t5 >> $@
	echo 3\\t88\\t5 >> $@
	echo 4\\t72\\t4 >> $@
	echo 6\\t53\\t4 >> $@
	echo 9\\t29.5\\t3 >> $@
	echo 10\\t25.2\\t3 >> $@
	echo 13\\t15.2\\t2 >> $@
	echo 15\\t11.1\\t2 >> $@

clean : 
	rm -f main.exe out.txt fitter.dll txts/* Plots/*