namespace adventofcode2023;

public class Grid<T>
{
    public Grid(IEnumerable<IEnumerable<T>> items) : this(items.Select(it => it.ToList()).ToList())
    {
    }

    public Grid(List<List<T>> items)
    {
        Items = items.ToList();
        Cols = items[0].Count;
        Rows = items.Count;
    }

    public int Cols { get; }

    public IEnumerable<GridEntry<T>> Entries => Items.SelectMany((row, rowIndex) => row.Select((value, colIndex) => new GridEntry<T>(new(rowIndex, colIndex), value)));

    public List<List<T>> Items { get; }

    public int Rows { get; }

    public static Grid<T> Create(IEnumerable<IEnumerable<T>> items) => new(items);

    public virtual GridEntry<T> EntryAt(Cell cell) => new(cell, ValueAt(cell));

    public virtual T ValueAt(Cell cell) => Items[cell.Row][cell.Col];
}

public record GridEntry<T>(Cell Cell, T Value);
