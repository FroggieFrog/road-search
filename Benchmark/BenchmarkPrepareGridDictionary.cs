using AnnoDesigner.Core.Models;
using Benchmark.PrepareGrid;
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
    public class BenchmarkPrepareGridDictionary
    {
        private List<AnnoObject> startObjects;

        [GlobalSetup]
        public void Setup()
        {
            var placedObjects = new AnnoDesigner.Core.Layout.LayoutLoader().LoadLayout(Path.Combine(AppContext.BaseDirectory, "Bigger_City_with_Palace_World_Fair_Centered.ad"), true);

            startObjects = placedObjects.Where(o => o.InfluenceRange > 0.5).ToList();
        }

        [Benchmark(Baseline = true)]
        public void Original()
        {
            var helper = new Original();
            var result = helper.PrepareGridDictionary(startObjects);
        }

        [Benchmark]
        public void Try01()
        {
            var helper = new Try01();
            var result = helper.PrepareGridDictionary(startObjects);
        }

        [Benchmark]
        public void Try02()
        {
            var helper = new Try02();
            var result = helper.PrepareGridDictionary(startObjects);
        }

        [Benchmark]
        public void Try03()
        {
            var helper = new Try03();
            var result = helper.PrepareGridDictionary(startObjects);
        }

        [Benchmark]
        public void Try04()
        {
            var helper = new Try04();
            var result = helper.PrepareGridDictionary(startObjects);
        }
    }
}
