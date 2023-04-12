Out.txt : main.exe 
	mono main.exe > Out.txt

main.exe : main.cs sfuns.dll
	mcs main.cs -reference:sfuns.dll


sfuns.dll : sfuns.cs 
	mcs $^ -target:library -out:sfuns.dll
