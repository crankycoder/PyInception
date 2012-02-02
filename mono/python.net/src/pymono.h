#include <mono/jit/jit.h>
#include <mono/metadata/assembly.h>
#include <mono/metadata/debug-helpers.h>
#include <mono/metadata/environment.h>
#include <mono/metadata/mono-config.h>
#include <mono/metadata/object.h>
#include <mono/metadata/threads.h>
#include <stdlib.h>
#include <string.h>

int cleanup_vm(void* domain);
void main_function(MonoObject* obj);
void* initialize_vm(char* assembly);
void* load_assembly(MonoDomain* domain, char* assembly_name);
MonoImage* load_image(MonoAssembly* assembly);
MonoClass* load_class(MonoImage* image, char* ns, char* clsname);
MonoObject* create_object(MonoDomain* domain, MonoClass* klass);

// You have to use mono_gc_handle references when using objects
int create_object_handle(MonoDomain* domain, MonoClass* klass);
void free_object_handle(int gc_handle);
MonoObject* get_target(int gc_handle);

MonoMethod* find_method(MonoDomain* domain, MonoObject* obj, char* method_name);
MonoObject* new_invoke_method(MonoDomain* domain, MonoObject* obj, MonoMethod* method, int param_count, char* args[]);

int attach_thread(MonoDomain* domain);
MonoDomain* get_domain(MonoObject* obj);
