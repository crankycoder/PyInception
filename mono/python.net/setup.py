from distutils.core import setup
from distutils.extension import Extension
from subprocess import Popen, PIPE
from Cython.Distutils import build_ext

def get_flags(cmd):
    p = Popen([cmd], shell=True, stdin=PIPE, stdout=PIPE, close_fds=True)
    (child_stdout, child_stdin) = (p.stdout, p.stdin)
    return child_stdout.read().strip()

CFLAGS=get_flags("pkg-config --cflags mono-2")
LDFLAGS=get_flags("pkg-config --libs mono-2")

ext_modules=[ 
        Extension("_pymono",
            extra_compile_args=CFLAGS.split() + ['-std=gnu99'],
            extra_link_args = LDFLAGS.split(),
            language="c",
            sources = [
                # The actual Cython binding,
                "src/_pymono.pyx",

                # Core PyMono modules
                "src/pymono.c", 
                ],
            ),
]

setup(
  name = 'Python.Mono',
  cmdclass = {'build_ext': build_ext},
  author='Victor Ng',
  author_email = 'crankycoder@gmail.com',
  description = 'A Cython binding to an embedded Mono VM',
  url = "http://bitbucket.org/crankycoder/python.mono",
  version='1.0',
  license = 'New BSD License',
  package_dir = {'': 'src'},
  py_modules = ['pymono'],
  ext_modules = ext_modules,
)


