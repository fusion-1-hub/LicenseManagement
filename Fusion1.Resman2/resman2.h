#pragma once

#ifdef RESMAN2_EXPORTS
#define RESMAN2_API __declspec(dllexport)
#else
#define RESMAN2_API __declspec(dllimport)
#endif

extern "C" RESMAN2_API const char* GenerateUID(
	const char* appName);
