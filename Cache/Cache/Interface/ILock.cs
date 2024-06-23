namespace Cache.Interface
{
    public interface ILock
    {
        public void EnterReadLock();
        public void ExitReadLock();
        public void EnterWriteLock();
        public void ExitWriteLock();
    }
}
