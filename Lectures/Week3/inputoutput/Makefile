#RM = rm --force (this is default definition)

out.txt : main.exe Makefile
	mono main.exe hej -numbers:1,2,3,4,5 -input:infile.txt -output:output.txt > $@ 2> log

main.exe: main.cs input.dll
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

input.dll : input.cs
	mcs -target:library -out:$@ $(filter %.cs,$^)

infile.txt : Makefile
	echo 1 > $@
	echo 2 >> $@
	echo 3 >> $@

clean: 
	$(RM) *.exe *.dll [Oo]ut* log output.txt

test: 
	echo $(RM)