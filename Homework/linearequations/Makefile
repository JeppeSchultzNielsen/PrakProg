out.txt : main.exe Makefile
	mono main.exe > out.txt

main.exe : main.cs qr.dll matrix.dll
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

qr.dll : qr.cs matrix.dll
	mcs -target:library -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

	


clean : 
	rm -f main.exe out.txt qr.dll