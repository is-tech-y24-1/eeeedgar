using BenchmarkDotNet.Attributes;
using GeneticAlgo.Shared;
using GeneticAlgo.Shared.Models;
using GeneticAlgo.Shared.Tools;

namespace GeneticAlgo.Benchmarking
{
    [SimpleJob(launchCount: 1, warmupCount: 1, targetCount: 10)]
    public class GeneticAlgoBenchmark
    {
        public IExecutionContext Context;
        
        public int CirclesCount = 3;
        
        [Params(5000, 50000)]
        public int PointsCount;

        [Params(  1e-1, 1e-2)]
        public double Epsilon;
        
        public int MaximumValue = 10;

        public int IterationLimit = 10000;
        
        public double PartToReplace = 0.2;

        [GlobalSetup]
        public void SetUp()
        {
            Context = new SolutionFindingExecutionContext(
                PointsCount,
                MaximumValue,
                CirclesCount,
                IterationLimit,
                Epsilon,
                PartToReplace);
        }

        [IterationSetup]
        public void IterationSetUp()
        {
            Context.Reset();
        }

        [Benchmark]
        public async Task Execute()
        {
            IterationResult result;
            do
            {
                result = await Context.ExecuteIterationAsync();
            } while (result == IterationResult.IterationFinished);
    
            Console.WriteLine(result);
        }
    }
}