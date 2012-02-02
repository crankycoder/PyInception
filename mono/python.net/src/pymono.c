#include "pymono.h"

#ifndef FALSE
#define FALSE 0
#endif

/*
 * Simple mono embedding example.
 * We show how to create objects and invoke methods and set fields in them.
 * Compile with: 
 *     gcc -Wall -o test-invoke test-invoke.c `pkg-config --cflags --libs mono-2` -lm
 *     mcs invoke.cs
 * Run with:
 *     ./test-invoke invoke.exe
 */

MonoClass* load_class(MonoImage* image, char* ns, char* clsname)
{
    MonoClass *klass;
    klass = mono_class_from_name(image, ns, clsname);
    if (!klass) {
        fprintf (stderr, "Can't find MyType in assembly %s\n", mono_image_get_filename(image));
        exit(1);
    }
    return klass;
}

void free_object_handle(int gc_handle)
{
    mono_gchandle_free(gc_handle);
}

MonoObject* get_target(int gc_handle)
{
    return mono_gchandle_get_target(gc_handle);
}

int create_object_handle(MonoDomain* domain, MonoClass* klass)
{
    MonoObject *obj;

    obj = mono_object_new(domain, klass);

    /* mono_object_new () only allocates the storage: 
     * it doesn't run any constructor. Tell the runtime to run
     * the default argumentless constructor.
     */
    mono_runtime_object_init(obj);

    return mono_gchandle_new(obj, 0);
}

MonoDomain* get_domain(MonoObject* obj)
{
    return mono_object_get_domain(obj);
}

MonoObject* create_object(MonoDomain* domain, MonoClass* klass)
{
    MonoObject *obj;

    obj = mono_object_new(domain, klass);
    /* mono_object_new () only allocates the storage: 
     * it doesn't run any constructor. Tell the runtime to run
     * the default argumentless constructor.
     */
    mono_runtime_object_init(obj);

    return obj;
}

/*
uint32_t      mono_gchandle_new         (MonoObject *obj, mono_bool pinned);
uint32_t      mono_gchandle_new_weakref (MonoObject *obj, mono_bool track_resurrection);
MonoObject*  mono_gchandle_get_target  (uint32_t gchandle);
void         mono_gchandle_free        (uint32_t gchandle);
*/

MonoMethod* find_method(MonoDomain* domain, MonoObject* obj, char* method_name)
{
    MonoClass *klass;
    MonoMethod *method = NULL;
    void* iter;

    attach_thread(domain);
    klass = mono_object_get_class(obj);

    /* retrieve all the methods we need */
    iter = NULL;
    while ((method = mono_class_get_methods (klass, &iter))) {
        if (strcmp (mono_method_get_name(method), method_name) == 0) {
            //MonoMethodSignature * sig = mono_method_signature(method);
            //if (mono_signature_get_param_count (sig) == param_count) {
                /* Now we'll call method () on obj: since it takes no arguments 
                 * we can pass NULL as the third argument to mono_runtime_invoke ().
                 * The method will print the updated value.
                 */
                return method;
            //}
        } 
    }
    return NULL;
}

MonoObject* new_invoke_method(MonoDomain* domain, MonoObject* obj, MonoMethod* method, int param_count, char* args[])
{
    /* Now we'll call method () on obj: since it takes no arguments 
     * we can pass NULL as the third argument to mono_runtime_invoke ().
     * The method will print the updated value.
     */
    void* mono_args[100];

    for (int idx = 0; idx < param_count; idx++) 
    {
        mono_args[idx] = mono_string_new(domain, args[idx]);
    }

    if (method == NULL) {
        printf("!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n");
        printf("!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n");
        printf("!!! How can this be null??? \n");
        printf("!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n");
        printf("!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n");
        return NULL;
    }
    return mono_runtime_invoke(method, obj, mono_args, NULL);
}


void main_function(MonoObject* obj)
{
}

void* load_assembly(MonoDomain* domain, char* assembly_name)
{
    MonoAssembly *assembly;

    /* Loading an assembly makes the runtime setup everything
     * needed to execute it. If we're just interested in the metadata
     * we'd use mono_image_load (), instead and we'd get a MonoImage*.
     */
    assembly = mono_domain_assembly_open (domain, assembly_name);

    if (!assembly)
    {
        exit (2);
    }
    return assembly;
}

MonoImage* load_image(MonoAssembly* assembly)
{
    MonoImage *image = mono_assembly_get_image(assembly);
    return image;
}

void* initialize_vm(char* assembly)
{
    MonoDomain *domain;
    /*
     * mono_jit_init() creates a domain: each assembly is
     * loaded and run in a MonoDomain.
     */
    domain = mono_jit_init(assembly);

    /* JFC: you have to load the Mono configuration before you
     * construct the root domain or else Mono won't apply mappings
     * between msvcrt->libc
     * */
    mono_config_parse (NULL);


    return domain;
}

int cleanup_vm(void* domain)
{
    int retval;
    retval = mono_environment_exitcode_get();
    mono_jit_cleanup (domain);
    return retval;
}


int attach_thread(MonoDomain* domain)
{
    return (int) mono_thread_attach(domain);
}
