using Cache;
using Cache.Algorithm;
using Cache.Entity;
using Cache.Infrastructure.Loggers;
using Newtonsoft.Json;

namespace TestProject
{
    public class UnitTestCache
    {
        CacheLeastRecentlyUsed<String, object> cacheLeastRecentlyUsed;

        public UnitTestCache()
        {
            ConsoleLogger consoleLogger = new ConsoleLogger();
            LeastRecentlyUsedDoubleLinkList<String, object> leastRecentlyUsed = new LeastRecentlyUsedDoubleLinkList<string, object>();

            cacheLeastRecentlyUsed = CacheLeastRecentlyUsed<string, object>.SingletonInstance(consoleLogger, leastRecentlyUsed);
        }


        [Fact]
        public void GetItemFromEmptyCache()
        {
            cacheLeastRecentlyUsed.Clear();
            var value = cacheLeastRecentlyUsed.Get("Key1");
            Assert.Equal(null, value);
        }

        [Fact]
        public void GetItemAfterSet()
        {
            cacheLeastRecentlyUsed.Clear();
            cacheLeastRecentlyUsed.ConfigureSize(3);
            cacheLeastRecentlyUsed.Set("Key1", 2);
            var value = cacheLeastRecentlyUsed.Get("Key1");
            Assert.Equal(2, value);
        }

        [Fact]
        public void SetCacheUsingSameKeyMultipleTimes()
        {
            cacheLeastRecentlyUsed.Clear();
            cacheLeastRecentlyUsed.ConfigureSize(3);
            cacheLeastRecentlyUsed.Set("Key1", 2);
            cacheLeastRecentlyUsed.Set("Key1", "value1");

            var value = cacheLeastRecentlyUsed.Get("Key1");
            Assert.Equal("value1", value);
        }


        [Fact]
        public void GetFirstItemFromDoubleLinkedList()
        {
            cacheLeastRecentlyUsed.Clear();
            cacheLeastRecentlyUsed.ConfigureSize(3);
            cacheLeastRecentlyUsed.Set("Key1", 1);
            cacheLeastRecentlyUsed.Set("Key2", 2);
            cacheLeastRecentlyUsed.Set("Key3", 3);

            var topItem = cacheLeastRecentlyUsed.leastUsedStructure.header.nextItem;
            var result = new DoubleLinkedListItem<string, object>(topItem.key, topItem.value);
            var expected = new DoubleLinkedListItem<string, object>("Key3", 3);

            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void GetLastItemFromDoubleLinkedList()
        {
            cacheLeastRecentlyUsed.Clear();
            cacheLeastRecentlyUsed.ConfigureSize(3);
            cacheLeastRecentlyUsed.Set("Key1", 1);
            cacheLeastRecentlyUsed.Set("Key2", 2);
            cacheLeastRecentlyUsed.Set("Key3", 3);

            var lastItem = cacheLeastRecentlyUsed.leastUsedStructure.tail.previousItem;
            var result = new DoubleLinkedListItem<string, object>(lastItem.key, lastItem.value);
            var expected = new DoubleLinkedListItem<string, object>("Key1", 1);

            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void GetItemAfterRemove()
        {
            cacheLeastRecentlyUsed.Clear();
            cacheLeastRecentlyUsed.ConfigureSize(3);
            cacheLeastRecentlyUsed.Set("Key1", 1);
            cacheLeastRecentlyUsed.Set("Key1", 2);
            cacheLeastRecentlyUsed.Remove("Key1");
            var result = cacheLeastRecentlyUsed.Get("Key1");

            Assert.Equal(null, result);
        }

        [Fact]
        public void GetLeastUsedItemWhenMoreThanSizeIsAdded()
        {
            cacheLeastRecentlyUsed.Clear();
            cacheLeastRecentlyUsed.ConfigureSize(1);
            cacheLeastRecentlyUsed.Set("Key1", 1);
            cacheLeastRecentlyUsed.Set("Key2", 2);
            var result = cacheLeastRecentlyUsed.Get("Key1");
            Assert.Equal(null, result);
        }

        [Fact]
        public void GetItemAfterExceedSizeAndRemove()
        {
            cacheLeastRecentlyUsed.Clear();
            cacheLeastRecentlyUsed.ConfigureSize(1);
            cacheLeastRecentlyUsed.Set("Key1", 1);
            cacheLeastRecentlyUsed.Set("Key2", 2);
            cacheLeastRecentlyUsed.Remove("Key2");
            cacheLeastRecentlyUsed.Set("Key1", 3);
            var result = cacheLeastRecentlyUsed.Get("Key1");
            Assert.Equal(3, result);
        }

        [Fact]
        public void GetItemAfterThousandsOfOperationsWithoutExceedSize()
        {
            cacheLeastRecentlyUsed.Clear();
            cacheLeastRecentlyUsed.ConfigureSize(5000);
            for (int i = 0; i < 2000; i++)
            {
                cacheLeastRecentlyUsed.Set(i.ToString(), i);
            }

            cacheLeastRecentlyUsed.Get("200");
            var top = cacheLeastRecentlyUsed.leastUsedStructure.header.nextItem;

            Assert.Equal(200, top.value);
        }

        [Fact]
        public void GetFirstItemAfterThousandsOfOperationsExceedSize()
        {
            cacheLeastRecentlyUsed.Clear();
            cacheLeastRecentlyUsed.ConfigureSize(5000);
            for (int i = 0; i < 2000; i++)
            {
                cacheLeastRecentlyUsed.Set(i.ToString(), i);
            }

            var result = cacheLeastRecentlyUsed.Get("200");
            Assert.Equal(200, result);
        }

        [Fact]
        public void ExpandCacheSize()
        {
            cacheLeastRecentlyUsed.Clear();
            cacheLeastRecentlyUsed.ConfigureSize(2);
            cacheLeastRecentlyUsed.Set("Key1", 1);
            cacheLeastRecentlyUsed.Set("Key2", 2);
            cacheLeastRecentlyUsed.Set("Key3", 3);
            cacheLeastRecentlyUsed.ConfigureSize(3);
            cacheLeastRecentlyUsed.Set("Key1", 1);

            var result = cacheLeastRecentlyUsed.Get("Key1");
            Assert.Equal(1, result);
        }

        [Fact]
        public void ShrinkCacheSize()
        {
            cacheLeastRecentlyUsed.Clear();
            cacheLeastRecentlyUsed.ConfigureSize(3);
            cacheLeastRecentlyUsed.Set("Key1", 1);
            cacheLeastRecentlyUsed.Set("Key2", 2);
            cacheLeastRecentlyUsed.Set("Key3", 3);

            cacheLeastRecentlyUsed.ConfigureSize(2);
            var result = cacheLeastRecentlyUsed.Get("Key1");
            Assert.Equal(null, result);
        }
    }
}