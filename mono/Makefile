CFLAGS=$(shell pkg-config --cflags mono-2)
LDFLAGS=$(shell pkg-config --libs mono-2) -lm

demo1:
	gcc test-embed.c -o test-embed $(CFLAGS) $(LDFLAGS)
	gmcs test.cs
	./test-embed test.exe

demo2:
	gcc test-invoke.c -o test-invoke $(CFLAGS) $(LDFLAGS)
	gmcs invoke.cs
	./test-invoke invoke.exe
