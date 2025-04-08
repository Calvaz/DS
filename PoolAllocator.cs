using System;

/// <summary>
/// An array-backed linked list designed to manage a fixed number of items.
/// This data structure uses a fixed-size pool backed by a freelist to track unused slots.
/// Allows insertion, removal and updates without reallocating, shifting or copying data, maintaining insertion order.
/// </summary>
/// <typeparam name="T"></typeparam>
public class PoolAllocator<T> {
    private readonly Node[] _items;
    private int _head;
    private int _freeHead;
    public PoolAllocator(int capacity) {
        _items = new Node[capacity];
        _head = -1;
        _freeHead = 0;
        for (int i = 0; i < capacity - 1; i++) {
            _items[i].Next = i + 1;
        }
        _items[capacity - 1].Next = -1;
    }

    private struct Node {
        public T Value;
        public int Next;
    }
  
    public IEnumerable<T> GetItems()
    {
        foreach (var i in _items)
        {
            yield return i.Value;
        }
    }
  

    public int Add(T item) {
        if (_freeHead == -1)
            throw new InvalidOperationException("No free slots available.");

        int newIndex = _freeHead;
        _freeHead = _items[newIndex].Next;

        _items[newIndex].Value = item;
        _items[newIndex].Next = _head;
        _head = newIndex;
        return newIndex;
    }

    public void Remove(int index) {
        if (index < 0 || index >= _items.Length) return;
        _items[index].Next = _freeHead;
        _freeHead = index;
    }

    public void Update(int index, T item) {
        if (index < 0 || index >= _items.Length) return;
        _items[index].Value = item;
    }
}

