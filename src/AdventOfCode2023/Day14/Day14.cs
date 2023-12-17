namespace adventofcode2023.Day14;

using System.Diagnostics;
using System.Text;
using CSharpFunctionalExtensions;

public static class Day14
{
    public static int Part1(string file)
    {
        var platform = Platform.Parse(File.ReadAllLines(file));
        platform.TiltNorth();

        Console.WriteLine(platform.ToString());

        return NorthLoad(platform);
    }

    public static int Part2(string file)
    {
        var platform = Platform.Parse(File.ReadAllLines(file));
        var previousLoad = 0;
        int load;
        var cycles = 0;

        // keep cyclying the platform until the load shows a repeating pattern, like values that keep repeating

        var start = Stopwatch.GetTimestamp();
        do
        {
            platform.Cycle();
            cycles++;
            load = NorthLoad(platform);
            Console.WriteLine($"Cycle {cycles}: {load}");
        } while (previousLoad != load && cycles < 1000);

        var end = Stopwatch.GetElapsedTime(start);
        Console.WriteLine($"Elapsed time: {end}");
        return load;
    }

    private static int NorthLoad(Platform platform) =>
        platform.Items.Where(it => it.IsRock)
            .Select(it => platform.RowCount - it.Row)
            .Sum();

    public class Item : ValueObject
    {
        public int Col { get; set; }

        public bool IsRock => Symbol == 'O';

        public int Row { get; set; }

        public char Symbol { get; init; }

        public override string ToString() => $"{Symbol} ({Row}, {Col})";

        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            yield return Symbol;
            yield return Row;
            yield return Col;
        }
    }

    public class Platform
    {
        public required int ColumnCount { get; init; }

        public required List<Item> Items { get; init; }

        public required int RowCount { get; init; }

        public static Platform Parse(string input) => Parse(input.Split("\n").ToList());

        public static Platform Parse(IList<string> lines)
        {
            // lines are in the form of:
            // O....#....
            // O.OO#....#
            // .....##...

            // O are rocks
            // # are cubes
            // . are empty spaces

            List<Item> items = [];
            var rowCount = lines.Count;
            var colCount = lines[0].Length;

            for (var row = 0; row < rowCount; row++)
            {
                for (var col = 0; col < colCount; col++)
                {
                    if (lines[row][col] == '.')
                    {
                        continue;
                    }

                    items.Add(
                        new()
                        {
                            // Id = row * colCount + col,
                            Col = col,
                            Row = row,
                            Symbol = lines[row][col]
                        });
                }
            }

            return new()
            {
                Items = items,
                RowCount = rowCount,
                ColumnCount = colCount
            };
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            var items = Items.ToDictionary(it => (it.Row, it.Col), it => it.Symbol);
            foreach (var row in Enumerable.Range(0, RowCount))
            {
                foreach (var col in Enumerable.Range(0, ColumnCount))
                {
                    result.Append(items.TryFind((row, col)).GetValueOrDefault('.'));
                }

                result.AppendLine();
            }

            return result.ToString();
        }

        public void Cycle()
        {
            TiltNorth();
            TiltWest();
            TiltSouth();
            TiltEast();
        }

        public void TiltEast()
        {
            foreach (var row in Items.GroupBy(it => it.Row))
            {
                TiltEast(row);
            }
        }

        public void TiltNorth()
        {
            foreach (var column in Items.GroupBy(it => it.Col))
            {
                TiltNorth(column);
            }
        }

        public void TiltSouth()
        {
            foreach (var column in Items.GroupBy(it => it.Col))
            {
                TiltSouth(column);
            }
        }

        public void TiltWest()
        {
            foreach (var row in Items.GroupBy(it => it.Row))
            {
                TiltWest(row);
            }
        }

        private void TiltEast(IEnumerable<Item> row)
        {
            var currentCol = ColumnCount;
            foreach (var item in row.OrderByDescending(it => it.Col))
            {
                if (item.IsRock)
                {
                    item.Col = --currentCol;
                }
                else
                {
                    currentCol = item.Col;
                }
            }
        }

        private void TiltNorth(IEnumerable<Item> column)
        {
            var currentRow = -1;
            foreach (var item in column.OrderBy(it => it.Row))
            {
                if (item.IsRock)
                {
                    item.Row = ++currentRow;
                }
                else
                {
                    currentRow = item.Row;
                }
            }
        }

        private void TiltSouth(IEnumerable<Item> column)
        {
            var currentRow = RowCount;
            foreach (var item in column.OrderByDescending(it => it.Row))
            {
                if (item.IsRock)
                {
                    item.Row = --currentRow;
                }
                else
                {
                    currentRow = item.Row;
                }
            }
        }

        private void TiltWest(IEnumerable<Item> row)
        {
            var currentCol = -1;
            foreach (var item in row.OrderBy(it => it.Col))
            {
                if (item.IsRock)
                {
                    item.Col = ++currentCol;
                }
                else
                {
                    currentCol = item.Col;
                }
            }
        }
    }
}
