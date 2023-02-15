out.txt : main.exe Makefile
	mono main.exe hej -numbers:1,2,3,4,5 > $@

main.exe: main.cs input.dll
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

input.dll : input.cs
	mcs -target:library -out:$@ $(filter %.cs,$^)

clean: 
	rm -f Out.txt main.exe input.dll