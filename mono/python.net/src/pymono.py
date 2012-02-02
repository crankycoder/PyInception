from _pymono import CyPyMono
from threading import Lock

_mono = None
_lock = Lock()

def PyMono(assembly_name):
    global _lock
    with _lock:
        global _mono
        if _mono is None:
            _mono = CyPyMono(assembly_name)
    return _mono

