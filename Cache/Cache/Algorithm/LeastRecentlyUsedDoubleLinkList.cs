using Cache.Entity;
using Cache.Interface;

namespace Cache.Algorithm
{
    public class LeastRecentlyUsedDoubleLinkList<K, V> : ILeastUsedStructure<K, V>
    {
        public DoubleLinkedListItem<K, V> header { get; set; }
        public DoubleLinkedListItem<K, V> tail { get; set; }

        public LeastRecentlyUsedDoubleLinkList()
        {
            Clear();
        }

        public void AddItemToTheTop(DoubleLinkedListItem<K, V> i)
        {
            if (i != null)
            {
                i.previousItem = header;
                i.nextItem = header.nextItem;
                header.nextItem.previousItem = i;
                header.nextItem = i;
            }
        }

        public void RemoveItem(DoubleLinkedListItem<K, V> i)
        {
            if (i != header && i != tail)
            {
                i.nextItem.previousItem = i.previousItem;
                i.previousItem.nextItem = i.nextItem;
                i.previousItem = null;
                i.nextItem = null;
            }
        }

        public DoubleLinkedListItem<K, V> RemoveLeastRecentlyUsedItem()
        {
            DoubleLinkedListItem<K, V> leastRecentlyUsedItem = null;
            if (tail != null)
            {
                leastRecentlyUsedItem = tail.previousItem;
                if (leastRecentlyUsedItem != null)
                {
                    RemoveItem(leastRecentlyUsedItem);
                }
            }

            return leastRecentlyUsedItem;
        }

        public void Clear()
        {
            header = new DoubleLinkedListItem<K, V>();
            tail = new DoubleLinkedListItem<K, V>();
            header.nextItem = tail;
            tail.previousItem = header;
        }
    }
}
