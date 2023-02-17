#RM = rm --force (this is default definition)

out.txt : main.exe Makefile input.txt input2.txt
	mono main.exe -numbers:1,2,3,4,5 -input:input2.txt -output:output.txt < input.txt > $@ 2> log

main.exe: main.cs
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))


clean: 
	$(RM) *.exe *.dll [Oo]ut* log input.txt output.txt input2.txt

input.txt: Makefile
	echo 2 3 4 5 6 > input.txt

input2.txt: Makefile
	echo  "3\n4\n5\n6\n7" > input2.txt
