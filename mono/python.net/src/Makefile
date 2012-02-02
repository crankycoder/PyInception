CFLAGS=$(shell pkg-config --cflags mono)
LDFLAGS=$(shell pkg-config --libs mono) -lm

all:
	javac -cp poi-3.8b1.jar HelloWorld.java
	ikvmc HelloWorld.class
	gcc test-invoke.c -o test-invoke $(CFLAGS) $(LDFLAGS)
	gmcs invoke.cs -r:HelloWorld.exe

test: test-invoke
	./test-invoke

clean:
	rm -f *.exe *.class test-invoke
