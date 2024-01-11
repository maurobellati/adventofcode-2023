namespace adventofcode2023.Day22;

using System.Collections;

public static class Day22
{
    public static int Part1(string file)
    {
        var grid = new Grid(File.ReadAllLines(file).Select((line, id) => ParseBrick(line)));
        var moved = MakeAllBricksFallTilGround(grid);

        // copy the bricks otherwise we get a concurrent modification exception
        var bricks = moved.ToList();
        return bricks.Count(it => IsRemovable(it, moved));
    }

    public static int Part2(string file)
    {
        var grid = new Grid(File.ReadAllLines(file).Select((line, id) => ParseBrick(line)));
        var moved = MakeAllBricksFallTilGround(grid);

        // copy the bricks otherwise we get a concurrent modification exception
        var bricks = moved.ToList();
        return bricks.Sum(brick => HowManyFallWhenRemoving(moved, brick));
    }

    private static int HowManyFallWhenRemoving(Grid grid, Brick brick)
    {
        // remove brick, so we can try to move something down
        grid.Remove(brick);

        var moved = MakeAllBricksFallOneStep(grid);
        var result = grid.CountDiffs(moved);

        // re-add brick back
        grid.Add(brick);

        return result;
    }

    private static bool IsRemovable(Brick brick, Grid grid) => HowManyFallWhenRemoving(grid, brick) == 0;

    private static Grid MakeAllBricksFallOneStep(Grid input)
    {
        Grid result = [];
        foreach (var brick in input.OrderBy(brick => brick.A.Z))
        {
            result.Add(TryMoveDown(result, brick) ?? brick);
        }

        return result;
    }

    private static Grid MakeAllBricksFallTilGround(Grid grid)
    {
        // move until we can't move anymore
        var moved = MakeAllBricksFallOneStep(grid);
        while (!SameGrid(moved, grid))
        {
            grid = moved;
            moved = MakeAllBricksFallOneStep(grid);
        }

        return moved;
    }

    private static Brick ParseBrick(string line) => Brick.Create(ParsePoint(line.Split('~')[0]), ParsePoint(line.Split('~')[1]));

    private static Point ParsePoint(string input) => new(int.Parse(input.Split(',')[0]), int.Parse(input.Split(',')[1]), int.Parse(input.Split(',')[2]));

    private static bool SameGrid(Grid x, Grid y) => x.CountDiffs(y) == 0;

    private static Brick? TryMoveDown(Grid grid, Brick brick)
    {
        var moved = brick.MoveDown(1);
        if (moved.A.Z < 1 || moved.B.Z < 1 || grid.AnyOverlaps(moved))
        {
            return null;
        }

        return moved;
    }

    internal class Grid : IEnumerable<Brick>
    {
        private readonly Dictionary<Point, Brick> pointToBrick = new();

        public Grid() : this(Array.Empty<Brick>())
        {
        }

        public Grid(IEnumerable<Brick> bricks)
        {
            foreach (var brick in bricks)
            {
                Add(brick);
            }
        }

        public IEnumerator<Brick> GetEnumerator() => pointToBrick.Values.Distinct().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(Brick brick) => brick.Points.ForEach(point => pointToBrick[point] = brick);

        public bool AnyOverlaps(Brick brick) => brick.Points.Exists(point => pointToBrick.ContainsKey(point));

        public int CountDiffs(Grid other)
        {
            var thisSet = pointToBrick.Values.ToHashSet();
            var otherSet = other.pointToBrick.Values.ToHashSet();
            return thisSet.Except(otherSet).Count();
        }

        public void Remove(Brick brick) => brick.Points.ForEach(point => pointToBrick.Remove(point));
    }

    internal readonly record struct Brick : IComparable<Brick>, IComparable
    {
        private Brick(Point a, Point b)
        {
            A = a;
            B = b;
            var p = A;
            var dx = A.X == B.X ? 0 : 1;
            var dy = A.Y == B.Y ? 0 : 1;
            var dz = A.Z == B.Z ? 0 : 1;

            Points.Add(A);
            do
            {
                p = new(p.X + dx, p.Y + dy, p.Z + dz);
                Points.Add(p);
            } while (p != B);
        }

        public Point A { get; }

        public Point B { get; }

        public List<Point> Points { get; } = new();

        public int CompareTo(Brick other)
        {
            var aComparison = A.Z.CompareTo(other.A.Z);
            return aComparison != 0 ? aComparison : B.Z.CompareTo(other.B.Z);
        }

        public int CompareTo(object? obj) => CompareTo((Brick)obj!);

        public static Brick Create(Point a, Point b)
        {
            // make sure A is always the lowest point
            var swapped = a.Z.CompareTo(b.Z) > 0;
            return new(swapped ? b : a, swapped ? a : b);
        }

        public override string ToString() => $"({A}-{B})";

        public Brick MoveDown(int step) => Create(A.MoveDown(step), B.MoveDown(step));
    }

    internal readonly record struct Point(int X, int Y, int Z) //: IComparable<Point>, IComparable
    {
        public Point MoveDown(int step) => this with { Z = Z - step };
    }
}
