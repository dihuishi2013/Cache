using Cache.Entity;
using Cache.Interface;

namespace Cache
{
    public class CacheLeastRecentlyUsed<K, V> : ICache<K, V>
    {

        private int count = 0;
        private int size = 0;

        private ILogger logger { get; set; }
        private ReaderWriterLockSlim cacheLock { get; set; }
        public ILeastUsedStructure<K, V> leastUsedStructure { get; set; }
        private static readonly object lockSingleton = new object();

        public Dictionary<K, DoubleLinkedListItem<K, V>> cacheItems { get; private set; }

        private CacheLeastRecentlyUsed(ILogger logger, ILeastUsedStructure<K, V> iLeastUsedStructure)
        {
            cacheItems = new Dictionary<K, DoubleLinkedListItem<K, V>>();
            this.cacheLock = new ReaderWriterLockSlim();

            this.logger = logger;
            this.leastUsedStructure = iLeastUsedStructure;
        }

        private static CacheLeastRecentlyUsed<K, V> cacheLeastRecentlyUsedInstance = null;

        public static CacheLeastRecentlyUsed<K, V> SingletonInstance(ILogger logger, ILeastUsedStructure<K, V> leastUsedStructure)
        {
            if (cacheLeastRecentlyUsedInstance == null)
            {
                lock (lockSingleton)
                {
                    if (cacheLeastRecentlyUsedInstance == null)
                    {
                        cacheLeastRecentlyUsedInstance = new CacheLeastRecentlyUsed<K, V>(logger, leastUsedStructure);
                    }
                }
            }
            return cacheLeastRecentlyUsedInstance;

        }

        public void Set(K key, V value)
        {
            cacheLock.EnterWriteLock();

            try
            {
                if (size > 0)
                {
                    if (!cacheItems.ContainsKey(key))
                    {

                        DoubleLinkedListItem<K, V> item = new DoubleLinkedListItem<K, V>(key, value);
                        cacheItems.Add(key, item);
                        leastUsedStructure.AddItemToTheTop(item);

                        if (count == size)
                        {
                            var leastRecentlyUsedItem = leastUsedStructure.RemoveLeastRecentlyUsedItem();
                            cacheItems.Remove(leastRecentlyUsedItem.key);
                            logger.Display("Item: " + leastRecentlyUsedItem.key.ToString() + " is removed with value: " + leastRecentlyUsedItem.value.ToString());
                        }
                        else
                        {
                            count += 1;
                        }
                    }
                    else
                    {
                        var item = cacheItems[key];
                        leastUsedStructure.RemoveItem(item);
                        item.value = value;
                        leastUsedStructure.AddItemToTheTop(item);
                        cacheItems[key] = item;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Display(ex.Message);
            }
            finally
            {
                cacheLock.ExitWriteLock();
            }
        }

        public V Get(K key)
        {
            V result = default(V);
            cacheLock.EnterReadLock();
            try
            {
                if (cacheItems.ContainsKey(key))
                {
                    var item = cacheItems[key];
                    leastUsedStructure.RemoveItem(item);
                    leastUsedStructure.AddItemToTheTop(item);
                    result = cacheItems[key].value;
                }

                return result;
            }
            catch (Exception ex)
            {
                logger.Display(ex.Message);
            }
            finally
            {
                cacheLock.ExitReadLock();
            }

            return result;
        }

        public void Remove(K key)
        {
            cacheLock.EnterWriteLock();
            try
            {
                if (cacheItems.ContainsKey(key))
                {
                    var item = cacheItems[key];
                    leastUsedStructure.RemoveItem(item);
                    cacheItems.Remove(key);
                    count = -1;
                }

            }
            catch (Exception ex)
            {
                logger.Display(ex.Message);
            }
            finally
            {
                cacheLock.ExitWriteLock();
            }
        }

        public void Clear()
        {
            cacheLock.EnterWriteLock();
            try
            {
                leastUsedStructure.Clear();
                cacheItems.Clear();
                count = 0;
                size = 0;
            }
            catch (Exception ex)
            {
                logger.Display(ex.Message);
            }
            finally { cacheLock.ExitWriteLock(); }
        }

        public void ConfigureSize(int size)
        {
            int i = 0;
            //When cache shrinks
            if (this.size > size)
            {
                var item = leastUsedStructure.header;
                var tail = leastUsedStructure.tail;
                while(i < size)
                {
                    item = item.nextItem;
                    i++;
                }

                while(item.nextItem !=  tail)
                {
                    var nextItem = item.nextItem;
                    leastUsedStructure.RemoveItem(nextItem);
                    cacheItems.Remove(item.key);
                }
                

            }
            this.size = size;
        }
    }
}
