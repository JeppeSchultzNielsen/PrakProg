out.txt Timing.svg: main.exe Makefile txts/times.data timing.gpi
	mono main.exe > out.txt
	gnuplot timing.gpi
	rm -f fit.log

main.exe : main.cs qr.dll matrix.dll
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

qr.dll : qr.cs matrix.dll
	mcs -target:library -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

QRGStimetester.exe : QRGStimetester.cs qr.dll matrix.dll
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))
	
txts/times.data : QRGStimetester.exe Makefile
	>$@
	for N in $$(seq 1 100 601); do \
		time --format "$$N %e" --output $@ --append \
		mono $< -size:$$N;\
	done


clean : 
	rm -f main.exe out.txt qr.dll QRGStimetester.exe times.data fit.log Plots/*