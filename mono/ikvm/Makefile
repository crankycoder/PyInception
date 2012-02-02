CFLAGS=$(shell pkg-config --cflags mono-2)
LDFLAGS=$(shell pkg-config --libs mono-2) -lm
IKVMC=/opt/mono-2.10/bin/mono /opt/ikvm/bin/ikvmc.exe
IKVM_LIB=/opt/ikvm/bin
GMCS=/opt/mono-2.10/bin/gmcs
MONO_BIN=/opt/mono-2.10/bin/mono

all: test-invoke invoke.exe Demo.exe


java_test: Demo.class
	java -cp poi-3.8b1.jar:. Demo

Demo.class: Demo.java
	javac -cp poi-3.8b1.jar Demo.java 

Demo.exe: Demo.class poi-3.8b1.exe
	$(IKVMC) Demo.class -r:poi-3.8b1.exe -r:IKVM.OpenJDK.SwingAWT.dll -r:IKVM.OpenJDK.Core.dll -lib:/opt/ikvm/bin

poi-3.8b1.exe:
	$(IKVMC) poi-3.8b1.jar

test-invoke: test-invoke.c
	gcc test-invoke.c -o test-invoke $(CFLAGS) $(LDFLAGS)

invoke.exe: invoke.cs
	$(GMCS) invoke.cs -r:Demo.exe -r:poi-3.8b1.exe -r:mscorlib -r:IKVM.OpenJDK.SwingAWT.dll -r:IKVM.OpenJDK.Core.dll -r:IKVM.Runtime.dll -r:IKVM.Runtime.JNI.dll -lib:/opt/ikvm/bin/


test: test-invoke invoke.exe
	./test-invoke invoke.exe

mono_demo: test-invoke Demo.exe
	$(MONO_BIN) Demo.exe

clean:
	rm -f *.exe *.class test-invoke
