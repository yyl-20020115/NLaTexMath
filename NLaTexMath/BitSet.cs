namespace NLaTexMath;

using System.Collections;

public class BitSet : ISet<int>
{
    public const int BitsPerByte = 8;
    public const int BitsPerLong = sizeof(long) * BitsPerByte;
    public static long[] CreateBuffer(int count) => new long[(count + BitsPerLong - 1) / BitsPerLong];

    public static int GetLastAlignedInnerIndex(int Count) => (Count - 1) % BitsPerLong;
    public static int GetLastAlignedStoreIndex(int Count) => (Count - 1) / BitsPerLong;
    public static int TrimLast(long[] Bits, int Count)
    {
        var ls = GetLastAlignedStoreIndex(Count) * BitsPerLong;
        Count = Count <= ls ? Count : ls;
        if (Bits.Length > 0)
        {
            var LastStore = Bits[^1];
            for (int i = GetLastAlignedInnerIndex(Count);
                i >= 0;
                i--)
                LastStore &= ~(1L << i);

            Bits[^1] = LastStore;
        }
        return Count;
    }
    public long[] Buffer => this.buffer;
    protected long[] buffer;
    public bool this[int index]
    {
        get => index >= 0 && index < Count
            ? 0 != (this.buffer[index / BitsPerLong] & (1 << index % BitsPerLong))
            : throw new IndexOutOfRangeException(nameof(index))
            ;
        set
        {
            if (index >= 0 && index < Count)
                if (value)
                    this.buffer[index / BitsPerLong] |= (1L << index % BitsPerLong);
                else
                    this.buffer[index / BitsPerLong] &= ~(1L << index % BitsPerLong);
            else
                throw new IndexOutOfRangeException(nameof(index));
        }
    }
    public virtual int Count { get => count; protected set => this.count = value; }
    protected int count = 0;
    public virtual bool IsReadOnly => false;
    public BitSet(int count = 0, bool defValue = false)
    {
        this.buffer = CreateBuffer(this.count = count);
        if (defValue)
        {
            for (int i = 0; i < this.buffer.Length; i++)
                this.buffer[i] = ~0L;
        }
    }
    public BitSet(IEnumerable<int> e)
        : this((e.Max(m => m) + 1))
    {
        if (e is BitSet s)
            this.count = TrimLast(
                this.buffer = (long[])s.buffer.Clone(), s.count);
        else
            foreach (var i in e)
                if (i >= 0 && i < count)
                    this[i] = true;
                else throw new ArgumentOutOfRangeException(
                    $"elements in e should be within [0..{count}]");
    }
    public BitSet(int[] e)
        : this((e.Max(m => m) + 1))
    {
        foreach (var i in e)
            if (i >= 0 && i < count)
                this[i] = true;
            else throw new ArgumentOutOfRangeException(
                $"elements in e should be within [0..{count}]");
    }

    public bool Add(int item) => item < 0 || item >= this.count
            ? throw new ArgumentOutOfRangeException(nameof(item))
            : this[item] ? false : (this[item] = true);
    void ICollection<int>.Add(int item) => this.Add(item);
    public bool Remove(int item)
        => item < 0 || item >= count
        ? throw new ArgumentOutOfRangeException(nameof(item))
        : !(this[item] = false);
    public void Clear()
        => Array.Clear(this.buffer, 0, this.buffer.Length);
    public bool Contains(int item)
        => this[item];
    public void CopyTo(int[] array, int arrayIndex)
    {
        for (int i = 0; i < Count; i++)
            if (this[i])
            {
                array[arrayIndex++] = i;
                if (arrayIndex >= array.Length)
                    throw new ArgumentOutOfRangeException(
                        nameof(array),
                        "This BinarySet contains more elements " +
                        "than the array parameter can hold " +
                        "starting from arrayIndex");
            }
    }
    public IEnumerator<int> GetEnumerator()
    {
        for (int i = 0; i < count; i++)
        {
            while (!this[i]) i++;
            yield return i;
        }
    }
    IEnumerator IEnumerable.GetEnumerator()
        => ((IEnumerable<int>)this).GetEnumerator();
    public void ExceptWith(IEnumerable<int> other)
    {
        foreach (var i in other) if (i >= 0 && i < this.count) this[i] = false;
    }
    public void UnionWith(IEnumerable<int> other)
    {
        foreach (var i in other) if (i >= 0 && i < this.count) this[i] = true;
    }
    public bool SetEquals(IEnumerable<int> other)
    {
        var otherSet = new BitSet(other);
        if (this.count != otherSet.count) return false;
        for (int i = 0; i < this.buffer.Length; i++)
            if (this.buffer[i] != otherSet.buffer[i]) return false;
        return true;
    }
    public bool Overlaps(IEnumerable<int> other)
    {
        foreach (var i in other) if (i >= 0 && i < this.count && this[i]) return true;
        return false;
    }
    public bool IsSupersetOf(IEnumerable<int> other)
    {
        foreach (var i in other) if (!this[i]) return false;
        return true;
    }
    public bool IsSubsetOf(IEnumerable<int> other)
        => new BitSet(other).IsSupersetOf(this);
    public void IntersectWith(IEnumerable<int> other)
    {
        if (other is BitSet that)
            for (int i = 0, count = Math.Min(this.buffer.Length, that.buffer.Length); i < count; i++)
                this.buffer[i] &= that.buffer[i];
        else
            foreach (var i in other)
                if (i >= 0 && i < this.count) this[i] &= true;
    }
    public bool IsProperSubsetOf(IEnumerable<int> other)
      => this.IsSubsetOf(other) &&
           !new BitSet(other).IsSubsetOf(this);
    public bool IsProperSupersetOf(IEnumerable<int> other)
        => this.IsSupersetOf(other) &&
           !new BitSet(other).IsSupersetOf(this);
    public void SymmetricExceptWith(IEnumerable<int> other)
    {
        var copy = new BitSet(this);
        copy.IntersectWith(other);
        this.UnionWith(other);
        this.ExceptWith(copy);
    }
}
