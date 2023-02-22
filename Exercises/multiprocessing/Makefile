TIME = time --portability --append --output $@
N = 5e8

out.txt : main.exe pfor.exe Makefile
	>$@
	$(TIME) mono main.exe -terms:$N -threads:1 >>$@
	$(TIME) mono main.exe -terms:$N -threads:2 >>$@
	$(TIME) mono main.exe -terms:$N -threads:3 >>$@
	$(TIME) mono main.exe -terms:$N -threads:4 >>$@
	$(TIME) mono pfor.exe -terms:$N >>$@

main.exe : main.cs
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

pfor.exe: pfor.cs
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

clean : 
	rm -f main.exe out.txt out* pfor.exe

test: out1 out2 out3 out4 Makefile

out1: main.exe; @mono main.exe -terms:$N -threads:1 >$@
out2: main.exe; @mono main.exe -terms:$N -threads:1 >$@
out3: main.exe; @mono main.exe -terms:$N -threads:1 >$@
out4: main.exe; @mono main.exe -terms:$N -threads:1 >$@
