#include "resman2.h"
#include <string>

const char* GenerateUID(
    const char* appName)
{
    std::string a(appName);
    return _strdup(a.c_str());
}


