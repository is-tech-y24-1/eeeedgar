#ifndef CSHARPLIBRARY_LIBRARY_H
#define CSHARPLIBRARY_LIBRARY_H


#define LIBRARY_EXPORT __declspec(dllexport)
extern "C" LIBRARY_EXPORT int Sum(int a, int b);

#endif //CSHARPLIBRARY_LIBRARY_H
