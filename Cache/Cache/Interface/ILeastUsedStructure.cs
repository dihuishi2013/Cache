using Cache.Entity;

namespace Cache.Interface
{
    public interface ILeastUsedStructure<K,V>
    {
        public DoubleLinkedListItem<K, V> header { get; set; }
        public DoubleLinkedListItem<K, V> tail { get; set; }
        public void AddItemToTheTop(DoubleLinkedListItem<K, V> i);
        public void RemoveItem(DoubleLinkedListItem<K, V> i);
        public DoubleLinkedListItem<K, V> RemoveLeastRecentlyUsedItem();
        public void Clear();
    }
}
