package eeee.dgar;

import org.openjdk.jmh.annotations.*;

import java.lang.reflect.Array;
import java.util.Arrays;
import java.util.Random;
import java.util.concurrent.TimeUnit;

@State(Scope.Benchmark)
@Fork(value = 1, warmups = 1)
@Warmup(iterations = 1)
@BenchmarkMode(Mode.AverageTime)
@OutputTimeUnit(TimeUnit.MILLISECONDS)
public class Benchmarking {
    @Param({"100", "1000", "10000", "100000"})
    public int length;
    public static int[] array;

    @Setup(Level.Invocation)
    public void setUp() {
        array = new int[length];
    }

    @Setup(Level.Iteration)
    public void generate() {
        array = new int[length];
        Random rd = new Random();
        for (int i = 0; i < length; i++) {
            array[i] = rd.nextInt();
        }
    }

    @org.openjdk.jmh.annotations.Benchmark
    public void mergeSort() {
        Algo.mergeSort(array, 0, length - 1);
    }

    @org.openjdk.jmh.annotations.Benchmark
    public void quickSort() {
        Algo.quickSort(array, 0, length - 1);
    }

    @org.openjdk.jmh.annotations.Benchmark
    public void defaultSort() {
        Arrays.sort(array);
    }
}
