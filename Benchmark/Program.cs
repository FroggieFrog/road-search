using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AnnoDesigner.Core.Layout;
using AnnoDesigner.Core.Models;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace Benchmark
{
    public class Program
    {
        static void Main(string[] args)
        {
            /*var a = new Benchmark();
            a.Setup();
            for (int i = 0; i < 1000; i++)
            {
                a.Baseline_S();
            }*/

            BenchmarkRunner.Run<Benchmark>();
        }
    }

    [SimpleJob(RuntimeMoniker.Net472)]
    [MemoryDiagnoser]
    [RankColumn]
    [MarkdownExporter]
    public class Benchmark
    {
        private List<AnnoObject> placedObjects;
        private List<AnnoObject> startObjects;

        [GlobalSetup]
        public void Setup()
        {
            placedObjects = new LayoutLoader().LoadLayout(Path.Combine(AppContext.BaseDirectory, "Bigger_City_with_Palace_World_Fair_Centered.ad"), true);

            startObjects = placedObjects.Where(o => o.InfluenceRange > 0.5).ToList();
        }

        [Benchmark(Baseline = true)]
        public void Baseline_S()
        {
            var gridDictionary = Implementations.Baseline.PrepareGridDictionary(placedObjects);

            Implementations.Baseline.BreadthFirstSearch(placedObjects, placedObjects.Where(o => o.InfluenceRange > 0.5), o => (int)o.InfluenceRange + 1, gridDictionary);

            foreach (var item in startObjects)
            {
                var visitedPoints = Implementations.Baseline.BreadthFirstSearch(placedObjects, Enumerable.Repeat(item, 1), o => (int)o.InfluenceRange, gridDictionary);

                Implementations.Baseline.GetBoundaryPoints(visitedPoints);
            }
        }

        [Benchmark]
        public void Baseline_P()
        {
            var gridDictionary = Implementations.Baseline.PrepareGridDictionary(placedObjects);

            Implementations.Baseline.BreadthFirstSearch(placedObjects, placedObjects.Where(o => o.InfluenceRange > 0.5), o => (int)o.InfluenceRange + 1, gridDictionary);

            Parallel.ForEach(startObjects, item =>
            {
                var visitedPoints = Implementations.Baseline.BreadthFirstSearch(placedObjects, Enumerable.Repeat(item, 1), o => (int)o.InfluenceRange, gridDictionary);

                Implementations.Baseline.GetBoundaryPoints(visitedPoints);
            });
        }

        [Benchmark]
        public void WithoutOutVar_S()
        {
            var gridDictionary = Implementations.WithoutOutVar.PrepareGridDictionary(placedObjects);

            Implementations.WithoutOutVar.BreadthFirstSearch(placedObjects, placedObjects.Where(o => o.InfluenceRange > 0.5), o => (int)o.InfluenceRange + 1, gridDictionary);

            foreach (var item in startObjects)
            {
                var visitedPoints = Implementations.WithoutOutVar.BreadthFirstSearch(placedObjects, Enumerable.Repeat(item, 1), o => (int)o.InfluenceRange, gridDictionary);

                Implementations.WithoutOutVar.GetBoundaryPoints(visitedPoints);
            }
        }

        [Benchmark]
        public void WithoutOutVar_P()
        {
            var gridDictionary = Implementations.WithoutOutVar.PrepareGridDictionary(placedObjects);

            Implementations.WithoutOutVar.BreadthFirstSearch(placedObjects, placedObjects.Where(o => o.InfluenceRange > 0.5), o => (int)o.InfluenceRange + 1, gridDictionary);

            Parallel.ForEach(startObjects, item =>
            {
                var visitedPoints = Implementations.WithoutOutVar.BreadthFirstSearch(placedObjects, Enumerable.Repeat(item, 1), o => (int)o.InfluenceRange, gridDictionary);

                Implementations.WithoutOutVar.GetBoundaryPoints(visitedPoints);
            });
        }
    }
}
