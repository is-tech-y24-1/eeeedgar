#include "com_eeee_Main.h"

JNIEXPORT void JNICALL Java_com_eeee_Main_sayHello (JNIEnv * env, jobject thisObject, jint x)
{
    std::cout << "Hello " << x << std::endl;
}
