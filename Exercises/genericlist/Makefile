out.txt : main.exe 
	mono main.exe > out.txt < input.txt

main.exe : main.cs genlist.dll
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

genlist.dll : genlist.cs
	mcs -target:library -out:$@ $^

clean : 
	rm -f main.exe out.txt genlist.dll