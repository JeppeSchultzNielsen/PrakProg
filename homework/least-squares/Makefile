fit.svg : fit.gpi out.txt
	gnuplot $< 

out.txt : main.exe Makefile 
	mono main.exe > out.txt

main.exe : main.cs matrix.dll qr.dll fitter.dll data.txt
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

fitter.dll : fitter.cs matrix.dll qr.dll
	mcs -target:library -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

data.txt : Makefile
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
	rm -f main.exe out.txt data.txt fitter.dll evaluatedFit.txt fit.svg