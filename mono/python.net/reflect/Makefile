CFLAGS=$(shell pkg-config --cflags mono-2)
LDFLAGS=$(shell pkg-config --libs mono-2) -lm
IKVMC=/opt/mono-2.10/bin/mono /opt/ikvm/bin/ikvmc.exe
IKVM_LIB=/opt/ikvm/bin
GMCS=/opt/mono-2.10/bin/gmcs
MONO_BIN=/opt/mono-2.10/bin/mono

all: sample.exe

sample.exe: sample.cs
	$(GMCS) sample.cs -r:mscorlib.dll

test: sample.exe
	$(MONO_BIN) sample.exe

clean:
	rm -f *.dll *.class

