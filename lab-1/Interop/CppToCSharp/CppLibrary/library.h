#ifndef CPPLIBRARY_LIBRARY_H
#define CPPLIBRARY_LIBRARY_H


#define LIBRARY_EXPORT __declspec(dllexport)
extern "C" LIBRARY_EXPORT int Sum(int a, int b);

#endif //CPPLIBRARY_LIBRARY_H
