out.txt : main.exe Makefile
	mono main.exe > out.txt

main.exe : main.cs
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

clean : 
	rm -f main.exe out.txt