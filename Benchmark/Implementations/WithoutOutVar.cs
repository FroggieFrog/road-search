using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnnoDesigner.Core.Models;

namespace Benchmark.Implementations
{
    /// <summary>
    /// Breadth search
    ///   - (int, int) coordinates
    ///   - AnnoObject[][] gridDictionary
    ///   - caches property calls
    ///   - returns bool[][]
    /// Boundary search
    ///   - (int, int) coordinates
    /// </summary>
    static class WithoutOutVar
    {
        public static AnnoObject[][] PrepareGridDictionary(IEnumerable<AnnoObject> placedObjects)
        {
            var maxX = (int)placedObjects.Max(o => o.Position.X + o.Size.Width) + 1;
            var maxY = (int)placedObjects.Max(o => o.Position.Y + o.Size.Height) + 1;

            var result = Enumerable.Range(0, maxX).Select(i => new AnnoObject[maxY]).ToArray();

            Parallel.ForEach(placedObjects, placedObject =>
            {
                var x = (int)placedObject.Position.X;
                var y = (int)placedObject.Position.Y;
                var w = placedObject.Size.Width;
                var h = placedObject.Size.Height;
                for (var i = 0; i < w; i++)
                    for (var j = 0; j < h; j++)
                        if (x + i >= 0 && y + j >= 0)
                            result[x + i][y + j] = placedObject;
            });

            return result;
        }

        public static bool[][] BreadthFirstSearch(
            IEnumerable<AnnoObject> placedObjects,
            IEnumerable<AnnoObject> startObjects,
            Func<AnnoObject, int> rangeGetter,
            AnnoObject[][] gridDictionary)
        {
            if (startObjects.Count() == 0)
                return new bool[0][];

            gridDictionary = gridDictionary ?? PrepareGridDictionary(placedObjects);

            var visitedObjects = new HashSet<AnnoObject>();
            var visitedCells = Enumerable.Range(0, gridDictionary.Length).Select(i => new bool[gridDictionary[0].Length]).ToArray();

            var distanceToStartObjects = startObjects.ToLookup(o => rangeGetter(o));
            var remainingDistance = distanceToStartObjects.Max(g => g.Key);
            var currentCells = new List<(int x, int y)>();
            var nextCells = new List<(int x, int y)>();

            void ProcessCell(int x, int y)
            {
                if (!visitedCells[x][y] && gridDictionary[x][y] != null)
                {
                    var cellObject = gridDictionary[x][y];
                    if (cellObject.Road)
                    {
                        if (remainingDistance > 1)
                        {
                            nextCells.Add((x, y));
                        }
                    }
                    else if (visitedObjects.Add(cellObject))
                    {
                        // nothing
                    }
                }
                visitedCells[x][y] = true;
            }

            while (remainingDistance > 1)
            {
                if (distanceToStartObjects.Contains(remainingDistance))
                {
                    // queue cells adjecent to starting objects, also sets cells inside of all start objects as visited, to exclude them from the search
                    foreach (var startObject in distanceToStartObjects[remainingDistance])
                    {
                        var initRange = rangeGetter(startObject);
                        var startX = (int)startObject.Position.X;
                        var startY = (int)startObject.Position.Y;
                        var leftX = startX - 1;
                        var rightX = (int)(startX + startObject.Size.Width);
                        var topY = startY - 1;
                        var bottomY = (int)(startY + startObject.Size.Height);

                        // queue top and bottom edges
                        for (var i = 0; i < startObject.Size.Width; i++)
                        {
                            var x = i + startX;

                            if (x >= 0 && topY >= 0 && gridDictionary[x][topY]?.Road == true)
                            {
                                nextCells.Add((x, topY));
                                visitedCells[x][topY] = true;
                            }

                            if (x >= 0 && bottomY >= 0 && gridDictionary[x][bottomY]?.Road == true)
                            {
                                nextCells.Add((x, bottomY));
                                visitedCells[x][bottomY] = true;
                            }

                        }
                        // queue left and right edges
                        for (var i = 0; i < startObject.Size.Height; i++)
                        {
                            var y = i + startY;

                            if (leftX >= 0 && y >= 0 && gridDictionary[leftX][y]?.Road == true)
                            {
                                nextCells.Add((leftX, y));
                                visitedCells[leftX][y] = true;
                            }

                            if (rightX >= 0 && y >= 0 && gridDictionary[rightX][y]?.Road == true)
                            {
                                nextCells.Add((rightX, y));
                                visitedCells[rightX][y] = true;
                            }
                        }

                        // visit all cells under start object
                        for (var i = 0; i < startObject.Size.Width; i++)
                            for (var j = 0; j < startObject.Size.Height; j++)
                                if (startX + i >= 0 && startY + j >= 0)
                                    visitedCells[startX + i][startY + j] = true;
                    }
                }

                var temp = nextCells;
                nextCells = currentCells;
                currentCells = temp;

                foreach (var (x, y) in currentCells)
                {
                    ProcessCell(x + 1, y);
                    if (x > 0)
                        ProcessCell(x - 1, y);
                    ProcessCell(x, y + 1);
                    if (y > 0)
                        ProcessCell(x, y - 1);
                }
                currentCells.Clear();
                remainingDistance--;
            }

            return visitedCells;
        }

        private enum Direction
        {
            Up,
            Left,
            Down,
            Right
        }

        private static (int x, int y) GetLeftCell((int X, int Y) point, Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return (point.X - 1, point.Y - 1);
                case Direction.Left:
                    return (point.X - 1, point.Y);
                case Direction.Down:
                    return point;
                default:
                    return (point.X, point.Y - 1);
            }
        }

        private static (int x, int y) GetRightCell((int X, int Y) point, Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return (point.X, point.Y - 1);
                case Direction.Left:
                    return (point.X - 1, point.Y - 1);
                case Direction.Down:
                    return (point.X - 1, point.Y);
                default:
                    return point;
            }
        }

        private static (int x, int y) MoveForward((int X, int Y) point, Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return (point.X, point.Y - 1);
                case Direction.Left:
                    return (point.X - 1, point.Y);
                case Direction.Down:
                    return (point.X, point.Y + 1);
                default:
                    return (point.X + 1, point.Y);
            }
        }

        private static (int x, int y) FindMin(bool[][] insidePoints)
        {
            for (int i = 0; i < insidePoints.Length; i++)
            {
                for (int j = 0; j < insidePoints[i].Length; j++)
                {
                    if (insidePoints[i][j])
                    {
                        return (i, j);
                    }
                }
            }
            return (-1, -1);
        }

        public static IList<(int, int)> GetBoundaryPoints(bool[][] insidePoints)
        {
            var result = new List<(int, int)>();

            if (insidePoints.Sum(column => column.Count()) == 0)
                return result;

            var maxX = insidePoints.Length;
            var maxY = insidePoints[0].Length;
            var startPoint = FindMin(insidePoints);
            var point = startPoint;
            var direction = Direction.Down;
            result.Add(point);

            do
            {
                var (leftX, leftY) = GetLeftCell(point, direction);
                var (rightX, rightY) = GetRightCell(point, direction);

                if (leftX >= 0 && leftX < maxX && leftY >= 0 && leftY < maxY && insidePoints[leftX][leftY])
                {
                    if (rightX >= 0 && rightX < maxX && rightY >= 0 && rightY < maxY && insidePoints[rightX][rightY])// turn right
                    {
                        result.Add(point);

                        direction = (Direction)(((int)direction + 3) % 4);
                    }
                    // else keep moving forward
                }
                else// turn left
                {
                    result.Add(point);

                    direction = (Direction)(((int)direction + 1) % 4);
                }

                point = MoveForward(point, direction);
            }
            while (point != startPoint);

            return result;
        }
    }
}
