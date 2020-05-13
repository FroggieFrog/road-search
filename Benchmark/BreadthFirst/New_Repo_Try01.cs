using System;
using System.Collections.Generic;
using System.Linq;
using AnnoDesigner.Core.Models;

namespace Benchmark.BreadthFirst
{
    public static class New_Repo_Try01
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

            startObjects = startObjects.Where(o => rangeGetter(o) > 0.5);
            if (!startObjects.Any())
            {
                return visitedCells;
            }

            var searchedCells = new Queue<(double remainingDistance, int x, int y)>(placedObjects.Count());
            var visitedObjects = new HashSet<AnnoObject>();

            foreach (var startObject in startObjects)
            {
                var startSize = startObject.Size;
                var startPosition = startObject.Position;
                var startRemainingDistance = rangeGetter(startObject);

                for (var i = 0; i < startSize.Width; i++)
                {
                    searchedCells.Enqueue((startRemainingDistance, i + (int)startPosition.X, (int)startPosition.Y - 1));
                    searchedCells.Enqueue((startRemainingDistance, i + (int)startPosition.X, (int)(startPosition.Y + startSize.Height)));

                    visitedCells[i + (int)startPosition.X][(int)startPosition.Y - 1] = true;
                    visitedCells[i + (int)startPosition.X][(int)(startPosition.Y + startSize.Height)] = true;
                }

                for (var i = 0; i < startSize.Height; i++)
                {
                    searchedCells.Enqueue((startRemainingDistance, (int)startPosition.X - 1, i + (int)startPosition.Y));
                    searchedCells.Enqueue((startRemainingDistance, (int)(startPosition.X + startSize.Width), i + (int)startPosition.Y));

                    visitedCells[(int)startPosition.X - 1][i + (int)startPosition.Y] = true;
                    visitedCells[(int)(startPosition.X + startSize.Width)][i + (int)startPosition.Y] = true;
                }

                visitedObjects.Add(startObject);

                for (var i = 0; i < startSize.Width; i++)
                {
                    for (var j = 0; j < startSize.Height; j++)
                    {
                        visitedCells[(int)startPosition.X + i][(int)startPosition.Y + j] = true;
                    }
                }
            }

            while (searchedCells.Count > 0)
            {
                var (distance, x, y) = searchedCells.Dequeue();
                var obj = gridDictionary[x][y];
                if (distance > 0 && obj != null && obj.Road)
                {
                    Enqueue(distance - 1, x + 1, y, inRangeAction, gridDictionary, visitedCells, searchedCells, visitedObjects);
                    Enqueue(distance - 1, x - 1, y, inRangeAction, gridDictionary, visitedCells, searchedCells, visitedObjects);
                    Enqueue(distance - 1, x, y + 1, inRangeAction, gridDictionary, visitedCells, searchedCells, visitedObjects);
                    Enqueue(distance - 1, x, y - 1, inRangeAction, gridDictionary, visitedCells, searchedCells, visitedObjects);
                }
            }

            return visitedCells;
        }

        private static void Enqueue(double distance, int x, int y, Action<AnnoObject> inRangeAction, AnnoObject[][] gridDictionary, bool[][] visitedCells, Queue<(double remainingDistance, int x, int y)> searchedCells, HashSet<AnnoObject> visitedObjects)
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
    }
}
