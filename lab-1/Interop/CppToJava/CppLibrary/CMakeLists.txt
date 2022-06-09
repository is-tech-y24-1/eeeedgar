cmake_minimum_required(VERSION 3.20)
project(Library)

set(CMAKE_CXX_STANDARD 14)

add_library(Library SHARED com_eeee_Main.cpp)

include_directories(C:/Program Files/Java/jdk-17.0.2/include)
include_directories(C:/Program Files/Java/jdk-17.0.2/include/win32)
link_directories(C:/Program Files/Java/jdk-17.0.2/include)
link_directories(C:/Program Files/Java/jdk-17.0.2/include/win32)