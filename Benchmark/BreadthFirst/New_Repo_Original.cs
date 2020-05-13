using System;
using System.Collections.Generic;
using System.Linq;
using AnnoDesigner.Core.Models;

namespace Benchmark.BreadthFirst
{
    public static class New_Repo_Original
    {
        private static void DoNothing(AnnoObject objectInRange) { }

        public static AnnoObject[][] PrepareGridDictionary(IEnumerable<AnnoObject> placedObjects)
        {
            var maxX = (int)placedObjects.Max(o => o.Position.X + o.Size.Width) + 1;
            var maxY = (int)placedObjects.Max(o => o.Position.Y + o.Size.Height) + 1;

            var result = Enumerable.Range(0, maxX).Select(x => new AnnoObject[maxY]).ToArray();

            foreach (var placedObject in placedObjects)
            {
                var x = (int)placedObject.Position.X;
                var y = (int)placedObject.Position.Y;
                for (var i = 0; i < placedObject.Size.Width; i++)
                {
                    for (var j = 0; j < placedObject.Size.Height; j++)
                    {
                        result[x + i][y + j] = placedObject;
                    }
                }
            }

            return result;
        }

        public static bool[][] BreadthFirstSearch(
            IEnumerable<AnnoObject> placedObjects,
            IEnumerable<AnnoObject> startObjects,
            Func<AnnoObject, double> rangeGetter,
            Action<AnnoObject> inRangeAction = null,
            AnnoObject[][] gridDictionary = null)
        {
            inRangeAction = inRangeAction ?? DoNothing;
            gridDictionary = gridDictionary ?? PrepareGridDictionary(placedObjects);

            var visitedCells = Enumerable.Range(0, gridDictionary.Length).Select(i => new bool[gridDictionary[0].Length]).ToArray();

            startObjects = startObjects.Where(o => rangeGetter(o) > 0.5).ToList();
            if (startObjects.Count() == 0)
                return visitedCells;

            var searchedCells = new Queue<(double remainingDistance, int x, int y)>(placedObjects.Count());
            var visitedObjects = new HashSet<AnnoObject>();

            foreach (var startObject in startObjects)
            {
                for (var i = 0; i < startObject.Size.Width; i++)
                {
                    searchedCells.Enqueue((rangeGetter(startObject), i + (int)startObject.Position.X, (int)startObject.Position.Y - 1));
                    searchedCells.Enqueue((rangeGetter(startObject), i + (int)startObject.Position.X, (int)(startObject.Position.Y + startObject.Size.Height)));
                    visitedCells[i + (int)startObject.Position.X][(int)startObject.Position.Y - 1] = true;
                    visitedCells[i + (int)startObject.Position.X][(int)(startObject.Position.Y + startObject.Size.Height)] = true;
                }
                for (var i = 0; i < startObject.Size.Height; i++)
                {
                    searchedCells.Enqueue((rangeGetter(startObject), (int)startObject.Position.X - 1, i + (int)startObject.Position.Y));
                    searchedCells.Enqueue((rangeGetter(startObject), (int)(startObject.Position.X + startObject.Size.Width), i + (int)startObject.Position.Y));
                    visitedCells[(int)startObject.Position.X - 1][i + (int)startObject.Position.Y] = true;
                    visitedCells[(int)(startObject.Position.X + startObject.Size.Width)][i + (int)startObject.Position.Y] = true;
                }

                visitedObjects.Add(startObject);
                for (var i = 0; i < startObject.Size.Width; i++)
                    for (var j = 0; j < startObject.Size.Height; j++)
                        visitedCells[(int)startObject.Position.X + i][(int)startObject.Position.Y + j] = true;
            }

            void Enqueue(double distance, int x, int y)
            {
                if (!visitedCells[x][y])
                {
                    var inRangeObject = gridDictionary[x][y];
                    if (inRangeObject != null)
                    {
                        searchedCells.Enqueue((distance, x, y));
                        if (!inRangeObject.Road && !visitedObjects.Contains(inRangeObject))
                        {
                            inRangeAction(inRangeObject);
                        }
                        visitedObjects.Add(inRangeObject);
                    }
                }
                visitedCells[x][y] = true;
            }

            while (searchedCells.Count > 0)
            {
                var (distance, x, y) = searchedCells.Dequeue();
                var obj = gridDictionary[x][y];
                if (distance > 0 && obj != null && obj.Road)
                {
                    Enqueue(distance - 1, x + 1, y);
                    Enqueue(distance - 1, x - 1, y);
                    Enqueue(distance - 1, x, y + 1);
                    Enqueue(distance - 1, x, y - 1);
                }
            }

            return visitedCells;
        }
    }
}
