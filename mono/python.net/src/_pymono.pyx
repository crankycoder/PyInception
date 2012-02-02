"""
This is Cython binding to the Mono runtime.

Basic goals:

    * Create a VM
    * Destroy a VM
    * Instantiate objects
    * Invoke methods with basic types:
        * string
        * int
        * double

Longer term goals:

    * Full access to the .NET runtime
    * Dynamic recompilation of Java code to .NET via IKVM
"""

cimport libc.stdlib

cdef extern from "pymono.h":
    # MonoV init and clean up
    cdef void* initialize_vm(char* assembly)
    cdef int cleanup_vm(void* domain)

    # Assembly management
    cdef void* load_assembly(void* domain, char* assembly_name)
    cdef void* load_image(void* assembly)

    # Class loading and object creation
    cdef void* load_class(void* image, char* ns, char* clsname)
    cdef void* create_object(void* domain, void* klass)

    # The main entry point into the test case
    cdef void main_function(void* obj)
    cdef void* find_method(void* domain, void* obj, char* method_name)
    cdef void* new_invoke_method(void* domain, void* obj, void* method, int param_count, char* args[])

    cdef int attach_thread(void* domain)
    cdef void* get_target(int gc_handle)
    cdef void free_object_handle(int gc_handle)
    cdef int create_object_handle(void* domain, void* klass)
    cdef void* get_domain(void* obj)


cdef class CyPyMono:
    cdef void* _monodomain
    cdef void* _assembly
    cdef void* _image

    def __init__(self, char* assembly_name):
        """
        This only supports a single assembly for now.
        """
        self._monodomain = initialize_vm(assembly_name)
        self._assembly = load_assembly(self._monodomain, assembly_name)
        self._image = load_image(self._assembly)

    def __dealloc__(self):
        attach_thread(self._monodomain)
        result = cleanup_vm(self._monodomain)

    def create_slideshow(self):
        # void main_function(MonoDomain *domain, void* assembly, char* ns, char* clsname)
        cdef PyMonoGCHandle py_hobj

        attach_thread(self._monodomain)
        py_hobj = make_object(self._monodomain, self._image, 'Embed', 'MyType')

        slide = PySlideShow(py_hobj)
        return slide



cdef class PyMonoMethod:
    """
    A wrapper around methods
    """
    cdef void* _domain
    cdef void* _method
    cdef void* _obj
    cdef int _param_count

    cdef call(self, char* args[]):
        attach_thread(self._domain)
        new_invoke_method(self._domain, self._obj, self._method, self._param_count, args)

cdef class PyMonoGCHandle:
    cdef int _gc_handle

    def __cinit__(self, int gc_handle):
        self._gc_handle = gc_handle

    def __dealloc__(self):
        free_object_handle(self._gc_handle)

cdef class PyMonoObject:
    cdef void* _obj

cdef class PySlideShow:
    '''
    This object can create slideshows
    '''
    cdef PyMonoGCHandle _py_hobj
    cdef void* _domain
    cdef int _last_slide

    def __init__(self, PyMonoGCHandle py_hobj):
        cdef void* obj
        self._py_hobj = py_hobj

        obj = get_target(self._py_hobj._gc_handle)
        self._domain = get_domain(obj)

    def init_template(self, char* template):
        cdef void *obj

        attach_thread(self._domain)
        obj = get_target(self._py_hobj._gc_handle)

        py_method = make_method(self._domain, obj, 'init_template', 1)
        py_method.call([template])
        self._last_slide = 0

    def set_slidedeck_info(self, char* title, char* description):
        cdef void *obj

        attach_thread(self._domain)
        obj = get_target(self._py_hobj._gc_handle)

        py_method = make_method(self._domain, obj, 'set_slidedeck_info', 2)
        py_method.call([title, description])

    def add_slide(self, char* title, char* png_file):
        cdef void *obj

        attach_thread(self._domain)
        obj = get_target(self._py_hobj._gc_handle)

        py_method = make_method(self._domain, obj, 'add_slide', 2)
        py_method.call([title, png_file])

    def flush(self, char* filename):
        cdef void *obj

        attach_thread(self._domain)
        obj = get_target(self._py_hobj._gc_handle)

        py_method = make_method(self._domain, obj, 'flush', 1)
        py_method.call([filename])



cdef PyMonoGCHandle make_object(void* domain, void* image, char* ns, char* cls_name):
    cdef PyMonoGCHandle  py_hobj
    cdef void* cls

    cls = load_class(image, ns, cls_name)
    py_hobj = PyMonoGCHandle(create_object_handle(domain, cls))

    return py_hobj

cdef PyMonoMethod make_method(void* domain, void* obj, char* method_name, int param_count):
    """
    This is going to be kind of slow.  The results of the method
    lookup should really be cached somewhere
    """
    cdef PyMonoMethod py_method = PyMonoMethod()
    # TODO: ???
    py_method._domain = domain
    py_method._method = find_method(domain, obj, method_name)
    py_method._obj = obj
    py_method._param_count = param_count

    return py_method



