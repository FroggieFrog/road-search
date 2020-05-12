using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var summary = BenchmarkRunner.Run<BenchmarkPrepareGridDictionary>();

            var summary = BenchmarkRunner.Run<BenchmarkBreadthFirstSearch>();

            Console.ReadLine();
        }
    }
}
