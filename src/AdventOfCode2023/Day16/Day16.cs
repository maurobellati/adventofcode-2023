#pragma warning disable CS8524 // The switch expression does not handle some values of its input type (it is not exhaustive) involving an unnamed enum value.
namespace adventofcode2023.Day16;

public static class Day16
{
    private static readonly Dictionary<char, Dictionary<Direction, List<Direction>>> HitMap = new()
    {
        ['.'] = new()
        {
            [Direction.N] = [Direction.N],
            [Direction.S] = [Direction.S],
            [Direction.E] = [Direction.E],
            [Direction.W] = [Direction.W]
        },
        ['/'] = new()
        {
            [Direction.N] = [Direction.E],
            [Direction.E] = [Direction.N],
            [Direction.S] = [Direction.W],
            [Direction.W] = [Direction.S]
        },
        ['\\'] = new()
        {
            [Direction.N] = [Direction.W],
            [Direction.E] = [Direction.S],
            [Direction.S] = [Direction.E],
            [Direction.W] = [Direction.N]
        },
        ['|'] = new()
        {
            [Direction.N] = [Direction.N],
            [Direction.S] = [Direction.S],
            [Direction.E] = [Direction.N, Direction.S],
            [Direction.W] = [Direction.N, Direction.S]
        },
        ['-'] = new()
        {
            [Direction.N] = [Direction.W, Direction.E],
            [Direction.S] = [Direction.W, Direction.E],
            [Direction.E] = [Direction.E],
            [Direction.W] = [Direction.W]
        }
    };

    public static List<PathStep> Hit(Grid<char> grid, PathStep step)
    {
        var symbol = grid.ValueAt(step.Cell);
        var newDirections = HitMap[symbol][step.Direction];
        return newDirections.Select(dir => step with { Direction = dir }).ToList();
    }

    public static int Part1(string file)
    {
        var grid = ParseGrid(file);
        var step = new PathStep(Cell.Origin, Direction.E);
        return CountEnergizedCells(step, grid);
    }

    public static int Part2(string file)
    {
        var grid = ParseGrid(file);

        // consider all possible origins: for each cell in the border (all verticals, all horizontals)
        var startingSteps = new List<PathStep>();
        for (var row = 0; row < grid.Rows; row++)
        {
            startingSteps.Add(new Cell(row, 0).Towards(Direction.E));
            startingSteps.Add(new Cell(row, grid.Cols - 1).Towards(Direction.W));
        }

        for (var col = 0; col < grid.Cols; col++)
        {
            startingSteps.Add(new Cell(0, col).Towards(Direction.S));
            startingSteps.Add(new Cell(grid.Rows - 1, col).Towards(Direction.N));
        }

        return startingSteps.Select(step => CountEnergizedCells(step, grid)).Max();
    }

    private static int CountEnergizedCells(PathStep origin, Grid<char> grid)
    {
        var firsts = Hit(grid, origin);
        HashSet<PathStep> visitedSteps = [];
        var pathFrontier = new Queue<PathStep>([..firsts]);

        while (pathFrontier.IsNotEmpty())
        {
            var currentStep = pathFrontier.Dequeue();
            // Console.Write("Exploring {0}", currentStep);

            visitedSteps.Add(currentStep);
            var nextStep = currentStep.Move();
            if (!grid.Contains(nextStep.Cell))
            {
                continue;
            }

            var outboundSteps = Hit(grid, nextStep);

            foreach (var outboundStep in outboundSteps)
            {
                if (visitedSteps.Add(outboundStep))
                {
                    // Console.WriteLine(" -> {0} enqueued", next);
                    pathFrontier.Enqueue(outboundStep);
                }
                // Console.WriteLine(" -> {0} already visited", next);
            }
            // Console.WriteLine($" -> hit the edge at {nextCell}");
        }

        return visitedSteps.Select(it => it.Cell).Distinct().Count();
    }

    private static Grid<char> ParseGrid(string file) => Grid<char>.Create(File.ReadLines(file));
}
