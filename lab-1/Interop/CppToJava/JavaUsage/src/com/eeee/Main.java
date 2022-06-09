package com.eeee;

public class Main {

    static {
        System.load("$PROJECT_ROOT/bin/Library.so");
    }

    public static void main(String[] args) {
        new Main().sayHello(5);
    }

    private native void sayHello(int x);
}