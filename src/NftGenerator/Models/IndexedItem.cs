class IndexedItem<T>
{
    public IndexedItem(T item, int index)
    {
        this.Item = item;
        this.Index = index;
    }

    public T Item { get; private set; }
    public int Index { get; set; }
}
