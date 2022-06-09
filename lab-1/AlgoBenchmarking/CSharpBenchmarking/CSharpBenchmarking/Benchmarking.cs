using BenchmarkDotNet.Attributes;

namespace CSharpBenchmarking
{
    public class Benchmarking
    {
        [Params(100, 1000, 10000, 100000)]
        public int Length;

        private int[] _array;

        [GlobalSetup]
        public void Setup()
        {
            _array = new int[Length];
        }

        [IterationSetup]
        public void IterationSetup()
        {
            var random = new Random();
            for (var i = 0; i < Length; i++)
            {
                _array[i] = random.Next();
            }
        }

        [Benchmark]
        public void QuickSort() => Algo.QuickSort(_array, 0, Length - 1);
            
        [Benchmark]
        public void MergeSort() => Algo.MergeSort(_array, 0, Length - 1);

        [Benchmark]
        public void DefaultSort() => Array.Sort(_array);
    }
}