namespace Cache.Entity
{
    public class DoubleLinkedListItem<K, V>
    {
        public K key { get; set; }
        public V value { get; set; }
        public DoubleLinkedListItem<K, V> previousItem { get; set; }
        public DoubleLinkedListItem<K, V> nextItem { get; set; }

        public DoubleLinkedListItem()
        {
        }

        public DoubleLinkedListItem(K key, V value)
        {
            this.key = key;
            this.value = value;
        }
    }
}
