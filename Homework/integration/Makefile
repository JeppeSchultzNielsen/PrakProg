out.txt txts/errorTabulated.txt Erf.svg ErfResiduals.svg: main.exe Makefile erf.gpi erfResiduals.gpi
	>txts/errorTabulated.txt
	echo 0 0 >> txts/errorTabulated.txt
	echo 0.1 0.112462916 >> txts/errorTabulated.txt
	echo 0.2 0.222702589 >> txts/errorTabulated.txt
	echo 0.3 0.328626759 >> txts/errorTabulated.txt
	echo 0.5 0.520499878 >> txts/errorTabulated.txt
	echo 0.8 0.742100965 >> txts/errorTabulated.txt
	echo 1 0.842700793 >> txts/errorTabulated.txt
	echo 2 0.995322265 >> txts/errorTabulated.txt
	echo 3 0.999977910 >> txts/errorTabulated.txt
	mono main.exe > out.txt
	gnuplot erf.gpi
	gnuplot erfResiduals.gpi

main.exe : main.cs integrator.dll
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

integrator.dll : integrator.cs
	mcs -target:library -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

clean : 
	rm -f main.exe out.txt integrator.dll txts/* *.svg