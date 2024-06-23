namespace Cache.Interface
{
    public interface ICache<K, V>
    {
        public V Get(K key);
        public void Set(K key, V value);
        public void Clear();
    }
}
