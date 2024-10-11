namespace adventofcode2023;

public class Grid<T>
{
    private Grid(List<List<T>> items)
    {
        Items = items.ToList();
        Cols = items[0].Count;
        Rows = items.Count;
    }

    public int Cols { get; }

    public List<List<T>> Items { get; }

    public int Rows { get; }

    public static Grid<T> Create(IEnumerable<IEnumerable<T>> items) => new(items.Select(it => it.ToList()).ToList());

    public virtual T ValueAt(Cell cell) => Items[cell.Row][cell.Col];
}
