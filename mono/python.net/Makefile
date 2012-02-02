CFLAGS=$(shell pkg-config --cflags mono-2)
LDFLAGS=$(shell pkg-config --libs mono-2) -lm
IKVMC=/opt/mono-2.10/bin/mono /opt/ikvm/bin/ikvmc.exe
IKVM_LIB=/opt/ikvm/bin
GMCS=/opt/mono-2.10/bin/gmcs
MONO_BIN=/opt/mono-2.10/bin/mono

all: invoke.dll

Demo.class: Demo.java
	javac -cp poi-3.8b1.jar Demo.java 

Demo.dll: Demo.class poi-3.8b1.dll
	$(IKVMC) Demo.class -r:poi-3.8b1.dll -target:library -lib:$(IKVM_LIB)

poi-3.8b1.dll:
	$(IKVMC) poi-3.8b1.jar -target:library

java_test: Demo.class
	java -cp poi-3.8b1.jar:. Demo

invoke.dll: invoke.cs poi-3.8b1.dll Demo.dll
	$(GMCS) invoke.cs -r:Demo.dll -target:library -r:IKVM.OpenJDK.Core.dll -lib:$(IKVM_LIB) -r:poi-3.8b1.dll

clean:
	rm -f *.dll *.class

