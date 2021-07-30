using System;
using System.Collections.Generic;
using System.Linq;

public class CyclingList<T>
{
    private List<T> _elements;
    private int _index;

    public CyclingList()
    {
        _elements = new List<T>();
    }

    public CyclingList(IEnumerable<T> elements) : this()
    {
        Add(elements.ToList());
    }

    public CyclingList(List<T> elements) : this()
    {
        Add(elements);
    }

    public void Add(T element) => _elements.Add(element);

    public void Add(List<T> elements) => elements.ForEach(val => _elements.Add(val));

    public void Add(T[] elements)
    {
        foreach (var element in elements)
        {
            _elements.Add(element);
        }
    }

    public bool Remove(T element) => _elements.Remove(element);

    public void SetIndex(int newIndex)
    {
        if (newIndex < 0) throw new Exception("Index can't be lower than zero");
        _index = newIndex;
    }

    public T GetCurrentElement()
    {
        var retrieveIndex = _index % _elements.Count;
        return _elements[retrieveIndex];
    }

    public T GetNextElement()
    {
        var retrieveIndex = ++_index % _elements.Count;
        return _elements[retrieveIndex];
    }

    public void IncrementIndex()
    {
        _index++;
    }

    public T GetElement(int index)
    {
        var retrieveIndex = index % _elements.Count;
        return _elements[retrieveIndex];
    }
}