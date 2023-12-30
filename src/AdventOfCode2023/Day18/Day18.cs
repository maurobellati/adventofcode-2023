namespace adventofcode2023.Day18;

using System.Globalization;
using CSharpFunctionalExtensions;

public static class Day18
{
    public static class Part1
    {
        public static int Solve(string file)
        {
            var instructions = File.ReadAllLines(file)
                .Select(ParseInstruction)
                .ToList();

            var loop = BuildLoop(instructions);
            return FindTotalAreaByFlooding(loop);
        }

        internal static Instruction ParseInstruction(string line)
        {
            // line is in the format: "R 6 (#70c710)"
            var parts = line.Split(' ');
            return new(
                parts[0] switch
                {
                    "R" => Direction.R,
                    "L" => Direction.L,
                    "U" => Direction.U,
                    "D" => Direction.D,
                    _ => throw new ArgumentException($"Invalid direction: {parts[0]}")
                },
                int.Parse(parts[1]));
        }

        private static List<Cell> BuildLoop(List<Instruction> instructions)
        {
            var start = new Cell(0, 0);
            // for each instruction, add the cells to the list
            List<Cell> cells = [start];
            foreach (var instruction in instructions)
            {
                var previousCell = cells[^1];
                for (var step = 1; step <= instruction.Value; step++)
                {
                    cells.Add(previousCell.Move(instruction.Direction, step));
                }
            }

            if (cells[^1] != start)
            {
                throw new ArgumentException("The loop does not end at the start");
            }

            return cells[..^1];
        }

        private static int FindTotalAreaByFlooding(List<Cell> loop)
        {
            // for each point diagonally near the origin, try to flood the space until reach the loop
            var origin = loop.First();
            foreach (var start in new[]
                     {
                         origin.Move(1, 1),
                         origin.Move(-1, 1),
                         origin.Move(1, -1),
                         origin.Move(-1, -1)
                     })
            {
                var area = Flood(start, loop);
                if (area > 0)
                {
                    return area + loop.Count;
                }
            }

            throw new ArgumentException("Unable to find a valid area");
        }

        private static int Flood(Cell start, List<Cell> loop)
        {
            var visited = new HashSet<Cell>();
            var queue = new Queue<Cell>();
            queue.Enqueue(start);
            while (queue.Count > 0)
            {
                if (visited.Count > loop.Count * loop.Count / 16)
                {
                    // over flooding
                    return 0;
                }

                var cell = queue.Dequeue();
                if (visited.Contains(cell))
                {
                    continue;
                }

                if (loop.Contains(cell))
                {
                    continue;
                }

                visited.Add(cell);

                foreach (var direction in new[]
                         {
                             Direction.U,
                             Direction.D,
                             Direction.L,
                             Direction.R
                         })
                {
                    queue.Enqueue(cell.Move(direction, 1));
                }
            }

            return visited.Count;
        }
    }

    public static class Part2
    {
        public static long Solve(string file)
        {
            var instructions = File.ReadAllLines(file).Select(ParseInstruction).ToList();

            var loop = BuildLoop(instructions);
            var loopWithSubsegments = SplitWithSubSegments(loop);

            // find all the rectangles inside the loop
            var rectangles = BuildRectangles(loopWithSubsegments).Where(it => IsInside(loopWithSubsegments, it)).ToList();

            // calculate the area of the rectangles without the borders to avoid double counting
            var rectangleAreaWithoutBorders = rectangles.Sum(it => it.InnerArea);

            // calculate the area of the segments without the endpoints to avoid double counting
            var allSegments = rectangles.SelectMany(it => it.Segments()).Distinct().ToList();
            var segmentsAreaWithoutEndpoints = allSegments.Select(it => it.Length - 2L).Sum();

            // calculate the area of the endpoints (each endpoint is a 1x1 rectangle)
            var endpointsArea = allSegments.SelectMany(it => it.Cells).Distinct().Count();

            // return the total area
            return rectangleAreaWithoutBorders + segmentsAreaWithoutEndpoints + endpointsArea;
        }

        private static List<Segment> BuildLoop(List<Instruction> instructions)
        {
            var start = new Cell(0, 0);
            var result = new List<Segment>();
            foreach (var instruction in instructions)
            {
                var end = start.Move(instruction.Direction, instruction.Value);
                result.Add(new(start, end));
                start = end;
            }

            return result;
        }

        private static IEnumerable<Rectangle> BuildRectangles(List<Segment> loop)
        {
            var cells = loop.SelectMany(segment => segment.Cells).ToList();
            var allXs = cells.Select(it => it.X).Distinct().Order().ToList();
            var allYs = cells.Select(it => it.Y).Distinct().Order().ToList();

            for (var yIndex = 0; yIndex < allYs.Count - 1; yIndex++)
            {
                for (var xIndex = 0; xIndex < allXs.Count - 1; xIndex++)
                {
                    var topLeft = new Cell(allXs[xIndex], allYs[yIndex]);
                    var bottomRight = new Cell(allXs[xIndex + 1], allYs[yIndex + 1]);
                    yield return new(topLeft, bottomRight);
                }
            }
        }

        private static bool IsInside(List<Segment> loop, Rectangle rectangle)
        {
            var center = new Cell((rectangle.TopLeft.X + rectangle.BottomRight.X) / 2, (rectangle.TopLeft.Y + rectangle.BottomRight.Y) / 2);
            return IsInside(loop, center);
        }

        private static bool IsInside(List<Segment> loop, Cell cell)
        {
            // cast a ray from the cell to the right and count the number of segments that:
            var count = loop
                .Where(segment => segment.A.X == segment.B.X) // are vertical
                .Where(segment => segment.A.X > cell.X) // are at the right of the cell
                .Where(segment => cell.Y.Between(segment.A.Y, segment.B.Y)) // intersects the horizontal ray
                .ToList();
            // if the number of segments is odd, the cell is inside the loop
            return count.Count % 2 == 1;
        }

        private static Instruction ParseInstruction(string line)
        {
            // lines in the format: "R 6 (#70c710)"
            // Each hexadecimal code is six hexadecimal digits long.
            // The first five hexadecimal digits encode the distance in meters as a five-digit hexadecimal number.
            // The last hexadecimal digit encodes the direction to dig: 0 means R, 1 means D, 2 means L, and 3 means U.
            var code = line.Split(' ')[2];
            var metersInHex = code[2..^2];
            var directionCode = code[^2..^1];
            return new(
                directionCode switch
                {
                    "0" => Direction.R,
                    "1" => Direction.D,
                    "2" => Direction.L,
                    "3" => Direction.U,
                    _ => throw new ArgumentException($"Invalid direction: {directionCode}")
                },
                int.Parse(metersInHex, NumberStyles.HexNumber));
        }

        private static List<Segment> SplitWithSubSegments(List<Segment> loop)
        {
            // build a grid of all the cells and split each segment in subsegments
            var cells = loop.SelectMany(segment => segment.Cells).ToList();
            var allXs = cells.Select(it => it.X).Distinct().Order().ToList();
            var allYs = cells.Select(it => it.Y).Distinct().Order().ToList();
            return loop.SelectMany(segment => SplitWithSubSegments(segment, allXs, allYs)).ToList();
        }

        private static IEnumerable<Segment> SplitWithSubSegments(Segment segment, List<int> allXs, List<int> allYs)
        {
            if (segment.IsVertical)
            {
                // vertical segment
                var x = segment.A.X;
                var yMin = Math.Min(segment.A.Y, segment.B.Y);
                var yMax = Math.Max(segment.A.Y, segment.B.Y);
                var yMinIndex = allYs.IndexOf(yMin);
                var yMaxIndex = allYs.IndexOf(yMax);
                for (var yIndex = yMinIndex; yIndex < yMaxIndex; yIndex++)
                {
                    Cell b = new(x, allYs[yIndex + 1]);
                    yield return new(new(x, allYs[yIndex]), b);
                }
            }
            else
            {
                // horizontal segment
                var y = segment.A.Y;
                var xMin = Math.Min(segment.A.X, segment.B.X);
                var xMax = Math.Max(segment.A.X, segment.B.X);
                var xMinIndex = allXs.IndexOf(xMin);
                var xMaxIndex = allXs.IndexOf(xMax);
                for (var xIndex = xMinIndex; xIndex < xMaxIndex; xIndex++)
                {
                    Cell b = new(allXs[xIndex + 1], y);
                    yield return new(new(allXs[xIndex], y), b);
                }
            }
        }
    }

    internal class Cell : ValueObject
    {
        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }

        public int Y { get; }

        public override string ToString() => $"({X}, {Y})";

        public Cell Move(Direction direction, int steps) => new(X + direction.DeltaX * steps, Y + direction.DeltaY * steps);

        public Cell Move(int deltaX, int deltaY) => new(X + deltaX, Y + deltaY);

        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            yield return X;
            yield return Y;
        }
    }

    internal class Segment : ValueObject
    {
        public Segment(Cell a, Cell b)
        {
            var swapped = a.CompareTo(b) > 0;
            A = swapped ? b : a;
            B = swapped ? a : b;
        }

        public Cell A { get; }

        public Cell B { get; }

        public IEnumerable<Cell> Cells => new[]
        {
            A,
            B
        };

        public bool IsHorizontal => A.Y == B.Y;

        public bool IsVertical => A.X == B.X;

        public int Length => IsVertical ? B.Y - A.Y + 1 : B.X - A.X + 1;

        public override string ToString() => $"({A} -> {B}) L={Length}";

        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            yield return A;
            yield return B;
        }
    }

    internal class Rectangle : ValueObject
    {
        public Rectangle(Cell topLeft, Cell bottomRight)
        {
            var swapped = topLeft.CompareTo(bottomRight) > 0;
            TopLeft = swapped ? bottomRight : topLeft;
            BottomRight = swapped ? topLeft : bottomRight;
        }

        public Cell BottomRight { get; }

        public long InnerArea => (BottomRight.X - TopLeft.X - 1L) * (BottomRight.Y - TopLeft.Y - 1L);

        public Cell TopLeft { get; }

        public override string ToString() => $"({TopLeft} -> {BottomRight})";

        public IEnumerable<Segment> Segments()
        {
            yield return new(TopLeft, new(BottomRight.X, TopLeft.Y));
            yield return new(new(BottomRight.X, TopLeft.Y), BottomRight);
            yield return new(BottomRight, new(TopLeft.X, BottomRight.Y));
            yield return new(new(TopLeft.X, BottomRight.Y), TopLeft);
        }

        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            yield return TopLeft;
            yield return BottomRight;
        }
    }

    internal record Instruction(Direction Direction, int Value);

    public record Direction(int DeltaX, int DeltaY)
    {
        public static readonly Direction U = new(0, -1);
        public static readonly Direction D = new(0, 1);
        public static readonly Direction L = new(-1, 0);
        public static readonly Direction R = new(1, 0);
    }
}
