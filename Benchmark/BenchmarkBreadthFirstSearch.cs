using AnnoDesigner.Core.Models;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark
{
    [SimpleJob(RuntimeMoniker.Net472)]
    [MemoryDiagnoser]
    [RankColumn]
    [MarkdownExporter]
    public class BenchmarkBreadthFirstSearch
    {
        private List<AnnoObject> placedObjects;
        private List<AnnoObject> startObjects;

        [GlobalSetup]
        public void Setup()
        {
            placedObjects = new AnnoDesigner.Core.Layout.LayoutLoader().LoadLayout(Path.Combine(AppContext.BaseDirectory, "Bigger_City_with_Palace_World_Fair_Centered.ad"), true);

            startObjects = placedObjects.Where(o => o.InfluenceRange > 0.5).ToList();
        }

        [Benchmark(Baseline = true)]
        public void Original()
        {
            var gridDictionary = Benchmark.BreadthFirst.Original.PrepareGridDictionary(placedObjects);

            Benchmark.BreadthFirst.Original.BreadthFirstSearch(placedObjects, placedObjects.Where(o => o.InfluenceRange > 0.5), o => o.InfluenceRange, gridDictionary: gridDictionary);

            foreach (var item in startObjects)
            {
                Benchmark.BreadthFirst.Original.BreadthFirstSearch(
                    placedObjects,
                    Enumerable.Repeat(item, 1),
                    o => o.InfluenceRange - 1,
                    gridDictionary: gridDictionary);
            }
        }

        [Benchmark]
        public void OriginalPR()
        {
            var gridDictionary = Benchmark.BreadthFirst.Original_PR.PrepareGridDictionary(placedObjects);

            Benchmark.BreadthFirst.Original_PR.BreadthFirstSearch(placedObjects, placedObjects.Where(o => o.InfluenceRange > 0.5), o => o.InfluenceRange, gridDictionary: gridDictionary);

            foreach (var item in startObjects)
            {
                Benchmark.BreadthFirst.Original_PR.BreadthFirstSearch(
                    placedObjects,
                    Enumerable.Repeat(item, 1),
                    o => o.InfluenceRange - 1,
                    gridDictionary: gridDictionary);
            }
        }
    }
}
