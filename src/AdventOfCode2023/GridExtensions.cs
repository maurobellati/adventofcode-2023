namespace adventofcode2023;

public static class GridExtensions
{
    public static bool Contains<T>(this Grid<T> grid, Cell cell) => grid.Contains(cell.Row, cell.Col);

    public static bool Contains<T>(this Grid<T> grid, int row, int col) => row.Between(0, grid.Rows - 1) && col.Between(0, grid.Cols - 1);

    public static Cell? Find<T>(this Grid<T> grid, Predicate<T> predicate)
    {
        for (var row = 0; row < grid.Rows; row++)
        {
            for (var col = 0; col < grid.Cols; col++)
            {
                if (predicate(grid.Items[row][col]))
                {
                    return new(row, col);
                }
            }
        }

        return null;
    }

    public static Cell? Find<T>(this Grid<T> grid, T value) => grid.Find(it => EqualityComparer<T>.Default.Equals(it, value));

    public static Box GetBox<T>(this Grid<T> grid) => new(Cell.Origin, new(grid.Rows - 1, grid.Cols - 1));
}
