Gamma.svg : gamma.gpi gamma.data factorials.data Error.svg Lngamma.svg
	gnuplot $<

Error.svg : error.gpi error.data errorTabulated.data
	gnuplot $< 

Lngamma.svg : lngamma.gpi lngamma.data
	gnuplot $< 

errorTabulated.data : Makefile
	>$@
	echo 0 0 >> $@
	echo 1 0.842700793 >> $@
	echo 2 0.995322265 >> $@
	echo 3 0.999977910 >> $@

error.data : main.exe Makefile
	mono main.exe -type:error > $@

gamma.data : main.exe Makefile
	mono main.exe -type:gamma > $@

lngamma.data : main.exe Makefile
	mono main.exe -type:lngamma > $@

main.exe : main.cs sfuns.dll factorials.data
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

sfuns.dll : gamma.cs erf.cs
	mcs -target:library -out:$@ $^

factorials.data : Makefile
	>$@
	echo 0 1 >> $@
	echo 1 1 >> $@
	echo 2 2 >> $@
	echo 3 6 >> $@
	echo 4 24 >> $@
	echo 5 120 >> $@

clean : 
	rm -f main.exe out.txt gamma.data factorials.data sfuns.dll Gamma.svg Error.svg error.data errorTabulated.data lngamma.data Lngamma.svg
