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

        [Benchmark]
        public void OriginalPR_updated()
        {
            var gridDictionary = Benchmark.BreadthFirst.Original_PR_Updated.PrepareGridDictionary(placedObjects);

            Benchmark.BreadthFirst.Original_PR_Updated.BreadthFirstSearch(placedObjects,
                placedObjects.Where(o => o.InfluenceRange > 0.5),
                o => o.InfluenceRange + 1,
                gridDictionary: gridDictionary);

            foreach (var item in startObjects)
            {
                Benchmark.BreadthFirst.Original_PR_Updated.BreadthFirstSearch(
                    placedObjects,
                    Enumerable.Repeat(item, 1),
                    o => o.InfluenceRange,
                    gridDictionary: gridDictionary);
            }
        }

        [Benchmark]
        public void OriginalPR_updated_Parallel()
        {
            var gridDictionary = Benchmark.BreadthFirst.Original_PR_Updated.PrepareGridDictionary(placedObjects);

            Benchmark.BreadthFirst.Original_PR_Updated.BreadthFirstSearch(placedObjects,
                placedObjects.Where(o => o.InfluenceRange > 0.5),
                o => o.InfluenceRange + 1,
                gridDictionary: gridDictionary);

            Parallel.ForEach(startObjects, item =>
            {
                Benchmark.BreadthFirst.Original_PR_Updated.BreadthFirstSearch(
                    placedObjects,
                    Enumerable.Repeat(item, 1),
                    o => o.InfluenceRange,
                    gridDictionary: gridDictionary);
            });
        }

        [Benchmark]
        public void OriginalPR_updated_Try01()
        {
            var gridDictionary = Benchmark.BreadthFirst.Original_PR_Updated_Try01.PrepareGridDictionary(placedObjects);

            Benchmark.BreadthFirst.Original_PR_Updated_Try01.BreadthFirstSearch(placedObjects,
                placedObjects.Where(o => o.InfluenceRange > 0.5),
                o => o.InfluenceRange + 1,
                gridDictionary: gridDictionary);

            foreach (var item in startObjects)
            {
                Benchmark.BreadthFirst.Original_PR_Updated_Try01.BreadthFirstSearch(
                    placedObjects,
                    Enumerable.Repeat(item, 1),
                    o => o.InfluenceRange,
                    gridDictionary: gridDictionary);
            }
        }

        [Benchmark]
        public void OriginalPR_updated_Try02()
        {
            var gridDictionary = Benchmark.BreadthFirst.Original_PR_Updated_Try02.PrepareGridDictionary(placedObjects);

            Benchmark.BreadthFirst.Original_PR_Updated_Try02.BreadthFirstSearch(placedObjects,
                placedObjects.Where(o => o.InfluenceRange > 0.5),
                o => o.InfluenceRange + 1,
                gridDictionary: gridDictionary);

            foreach (var item in startObjects)
            {
                Benchmark.BreadthFirst.Original_PR_Updated_Try02.BreadthFirstSearch(
                    placedObjects,
                    Enumerable.Repeat(item, 1),
                    o => o.InfluenceRange,
                    gridDictionary: gridDictionary);
            }
        }

        [Benchmark]
        public void OriginalPR_updated_Try02_Parallel()
        {
            var gridDictionary = Benchmark.BreadthFirst.Original_PR_Updated_Try02.PrepareGridDictionary(placedObjects);

            Benchmark.BreadthFirst.Original_PR_Updated_Try02.BreadthFirstSearch(placedObjects,
                placedObjects.Where(o => o.InfluenceRange > 0.5),
                o => o.InfluenceRange + 1,
                gridDictionary: gridDictionary);

            Parallel.ForEach(startObjects, item =>
            {
                Benchmark.BreadthFirst.Original_PR_Updated_Try02.BreadthFirstSearch(
                    placedObjects,
                    Enumerable.Repeat(item, 1),
                    o => o.InfluenceRange,
                    gridDictionary: gridDictionary);
            });
        }

        [Benchmark]
        public void New_Repo_Original()
        {
            var gridDictionary = Benchmark.BreadthFirst.New_Repo_Original.PrepareGridDictionary(placedObjects);

            Benchmark.BreadthFirst.New_Repo_Original.BreadthFirstSearch(placedObjects, placedObjects.Where(o => o.InfluenceRange > 0.5), o => o.InfluenceRange, gridDictionary: gridDictionary);

            foreach (var item in startObjects)
            {
                Benchmark.BreadthFirst.New_Repo_Original.BreadthFirstSearch(
                    placedObjects,
                    Enumerable.Repeat(item, 1),
                    o => o.InfluenceRange - 1,
                    gridDictionary: gridDictionary);
            }
        }

        [Benchmark]
        public void New_Repo_Original_Parallel()
        {
            var gridDictionary = Benchmark.BreadthFirst.New_Repo_Original.PrepareGridDictionary(placedObjects);

            Benchmark.BreadthFirst.New_Repo_Original.BreadthFirstSearch(placedObjects, placedObjects.Where(o => o.InfluenceRange > 0.5), o => o.InfluenceRange, gridDictionary: gridDictionary);

            Parallel.ForEach(startObjects, item =>
            {
                Benchmark.BreadthFirst.New_Repo_Original.BreadthFirstSearch(
                    placedObjects,
                    Enumerable.Repeat(item, 1),
                    o => o.InfluenceRange - 1,
                    gridDictionary: gridDictionary);
            });
        }

        [Benchmark]
        public void New_Repo_Try01()
        {
            var gridDictionary = Benchmark.BreadthFirst.New_Repo_Try01.PrepareGridDictionary(placedObjects);

            Benchmark.BreadthFirst.New_Repo_Try01.BreadthFirstSearch(placedObjects, placedObjects.Where(o => o.InfluenceRange > 0.5), o => o.InfluenceRange, gridDictionary: gridDictionary);

            foreach (var item in startObjects)
            {
                Benchmark.BreadthFirst.New_Repo_Try01.BreadthFirstSearch(
                    placedObjects,
                    Enumerable.Repeat(item, 1),
                    o => o.InfluenceRange - 1,
                    gridDictionary: gridDictionary);
            }
        }

        [Benchmark]
        public void New_Repo_Try01_Parallel()
        {
            var gridDictionary = Benchmark.BreadthFirst.New_Repo_Try01.PrepareGridDictionary(placedObjects);

            Benchmark.BreadthFirst.New_Repo_Try01.BreadthFirstSearch(placedObjects, placedObjects.Where(o => o.InfluenceRange > 0.5), o => o.InfluenceRange, gridDictionary: gridDictionary);

            Parallel.ForEach(startObjects, item =>
            {
                Benchmark.BreadthFirst.New_Repo_Try01.BreadthFirstSearch(
                    placedObjects,
                    Enumerable.Repeat(item, 1),
                    o => o.InfluenceRange - 1,
                    gridDictionary: gridDictionary);
            });
        }

        [Benchmark]
        public void New_Repo_Try02()
        {
            var gridDictionary = Benchmark.BreadthFirst.New_Repo_Try02.PrepareGridDictionary(placedObjects);

            Benchmark.BreadthFirst.New_Repo_Try02.BreadthFirstSearch(placedObjects, placedObjects.Where(o => o.InfluenceRange > 0.5), o => o.InfluenceRange, gridDictionary: gridDictionary);

            foreach (var item in startObjects)
            {
                Benchmark.BreadthFirst.New_Repo_Try02.BreadthFirstSearch(
                    placedObjects,
                    Enumerable.Repeat(item, 1),
                    o => o.InfluenceRange - 1,
                    gridDictionary: gridDictionary);
            }
        }

        [Benchmark]
        public void New_Repo_Try02_Parallel()
        {
            var gridDictionary = Benchmark.BreadthFirst.New_Repo_Try02.PrepareGridDictionary(placedObjects);

            Benchmark.BreadthFirst.New_Repo_Try02.BreadthFirstSearch(placedObjects, placedObjects.Where(o => o.InfluenceRange > 0.5), o => o.InfluenceRange, gridDictionary: gridDictionary);

            Parallel.ForEach(startObjects, item =>
            {
                Benchmark.BreadthFirst.New_Repo_Try02.BreadthFirstSearch(
                    placedObjects,
                    Enumerable.Repeat(item, 1),
                    o => o.InfluenceRange - 1,
                    gridDictionary: gridDictionary);
            });
        }
    }
}
