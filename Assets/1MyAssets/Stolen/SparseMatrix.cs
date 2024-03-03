using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class SparseMatrix<T> : IEnumerable<T>
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public long Size { get; private set; }

    private readonly Dictionary<long, T> _cells = new();

    public SparseMatrix(int w, int h)
    {
        this.Width = w;
        this.Height = h;
        this.Size = w * h;
    }

    public T this[Cell cell]
    {
        get
        {
            long index = cell.x * Width + cell.z;
            _cells.TryGetValue(index, out T result);
            return result;
        }
        set
        {
            long index = cell.x * Width + cell.z;
            _cells[index] = value;
        }
    }
    public int Count => _cells.Count;
    public T ValueAt(int index)
    {
        return _cells.ElementAt(index).Value;
    }
    public void Remove(Cell cell)
    {
        long index = cell.x * Width + cell.z;
        _cells.Remove(index);
    }
    public IEnumerator<T> GetEnumerator()
    {
        return _cells.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }
}
