using AnnoDesigner.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.PrepareGrid
{
    public class Try01
    {
        public AnnoObject[][] PrepareGridDictionary(IEnumerable<AnnoObject> placedObjects)
        {
            var maxX = (int)placedObjects.Max(o => o.Position.X + o.Size.Width) + 1;
            var maxY = (int)placedObjects.Max(o => o.Position.Y + o.Size.Height) + 1;

            var result = Enumerable.Range(0, maxX).Select(x => new AnnoObject[maxY]).ToArray();

            foreach (var placedObject in placedObjects)
            {
                var x = (int)placedObject.Position.X;
                var y = (int)placedObject.Position.Y;

                Parallel.For(0, (int)placedObject.Size.Width, i =>
                {
                    for (var j = 0; j < placedObject.Size.Height; j++)
                    {
                        result[x + i][y + j] = placedObject;
                    }
                });
            }

            return result;
        }
    }
}
