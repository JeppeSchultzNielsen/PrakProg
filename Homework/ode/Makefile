out.txt Sin.svg DampedPendulum: main.exe Makefile sin.gpi dampedPendulum.gpi
	mono main.exe > out.txt
	gnuplot sin.gpi
	gnuplot dampedPendulum.gpi


main.exe : main.cs matrix.dll odesolver.dll
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

odesolver.dll : odesolver.cs matrix.dll
	mcs -target:library -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

clean : 
	rm -f main.exe out.txt odesolver.dll Sin.svg harmonicDiff.txt DampedPendulum.svg txts/*