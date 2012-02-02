#include <mono/jit/jit.h>
#include <mono/metadata/object.h>
#include <mono/metadata/environment.h>
#include <mono/metadata/assembly.h>
#include <mono/metadata/debug-helpers.h>
#include <string.h>
#include <stdlib.h>

#ifndef FALSE
#define FALSE 0
#endif


int main (int argc, char* argv[]) {
    MonoDomain *domain;
    const char *file;
    int retval;
    
    /*
     * mono_jit_init() creates a domain: each assembly is
     * loaded and run in a MonoDomain.
     */
    domain = mono_jit_init ("Demo.exe");
    printf("jit_init of Mono OK\n");

    retval = mono_environment_exitcode_get();
    
    mono_jit_cleanup (domain);
    printf("shutdown of Mono OK\n");
    printf("status: %d", retval);
    return retval;
}

