using System;
using System.IO;
using System.Linq;

namespace Performance
{
    class Program
    {
        static void Main(string[] args)
        {
            var placedObjects = new AnnoDesigner.Core.Layout.LayoutLoader().LoadLayout(Path.Combine(AppContext.BaseDirectory, "Bigger_City_with_Palace_World_Fair_Centered.ad"), true);

            var startObjects = placedObjects.Where(o => o.InfluenceRange > 0.5).ToList();
            for (int i = 0; i < 1000; i++)
            {
                var gridDictionary = AnnoDesigner.Helper.RoadSearchHelper.PrepareGridDictionary(placedObjects);

                AnnoDesigner.Helper.RoadSearchHelper.BreadthFirstSearch(placedObjects, placedObjects.Where(o => o.InfluenceRange > 0.5), o => o.InfluenceRange, gridDictionary: gridDictionary);

                //Parallel.ForEach(startObjects, (item) =>
                foreach (var item in startObjects)
                {
                    AnnoDesigner.Helper.RoadSearchHelper.BreadthFirstSearch(
                        placedObjects,
                        Enumerable.Repeat(item, 1),
                        o => o.InfluenceRange - 1,
                        gridDictionary: gridDictionary);
                }//);
            }
        }
    }
}
