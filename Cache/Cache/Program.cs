// See https://aka.ms/new-console-template for more information
using Cache;
using Cache.Algorithm;
using Cache.Infrastructure.Loggers;

ConsoleLogger logger = new ConsoleLogger();
LeastRecentlyUsedDoubleLinkList<String, object> leastRecentlyUsed = new LeastRecentlyUsedDoubleLinkList<string, object>();

var cacheLeastRecentlyUsed = CacheLeastRecentlyUsed<string, object>.SingletonInstance(logger, leastRecentlyUsed);

//Get Item from cache without set up
var result = cacheLeastRecentlyUsed.Get("Key1");
Console.WriteLine("Try to get value of Key1 without set up on Cache, Key1 : " + result);

//Set item but size of the Cache is Zero
cacheLeastRecentlyUsed.Set("Key1", 1);
result = cacheLeastRecentlyUsed.Get("Key1");
Console.WriteLine("Try to get value of Key1 but size of Cache is zero : " + result);

//Set size to be 1, set key, value and get value from key
cacheLeastRecentlyUsed.ConfigureSize(1);
cacheLeastRecentlyUsed.Set("Key1", 1);
result = cacheLeastRecentlyUsed.Get("Key1");
Console.WriteLine("After set up, value of Key1 is : " + result);

//Get item from cache and its key is not set up
result = cacheLeastRecentlyUsed.Get("Key2");
Console.WriteLine("Without set up, value of Key2 is : " + result);

//Set up an new item to the cache
cacheLeastRecentlyUsed.Set("Key2", 2);
result = cacheLeastRecentlyUsed.Get("Key2");
Console.WriteLine("Get value of Key2 after set up : " + result);

//Key1 is removed from cache as cache is full and it is the least used one
result = cacheLeastRecentlyUsed.Get("Key1");
Console.WriteLine("Key1 item is removed as cache is full and it is the least used one. Key1 : " + result);

//Remove non-existed key from cache
cacheLeastRecentlyUsed.Remove("Key3");
result = cacheLeastRecentlyUsed.Get("Key2");
Console.WriteLine("Key2 still exists after removing non-existed Key3. Key2 : " + result);

//Remove existed key
cacheLeastRecentlyUsed.Remove("Key2");
result = cacheLeastRecentlyUsed.Get("Key2");
Console.WriteLine("Key2 is gone after remove. Key2 : " + result);

//Get first item from double linked list
cacheLeastRecentlyUsed.ConfigureSize(3);
cacheLeastRecentlyUsed.Set("Key1", 1);
cacheLeastRecentlyUsed.Set("Key2", 2);
cacheLeastRecentlyUsed.Set("Key3", 3);
var firstItem = cacheLeastRecentlyUsed.leastUsedStructure.header.nextItem.key;
var leastUsedItem = cacheLeastRecentlyUsed.leastUsedStructure.tail.previousItem.key;
Console.WriteLine("Add Key1, Key2, Key3, the first value in double linked list is " + firstItem + ", the least used one is " + leastUsedItem);

//Set existed item will change the least used one
cacheLeastRecentlyUsed.Set("Key1", 4);
firstItem = cacheLeastRecentlyUsed.leastUsedStructure.header.nextItem.key;
leastUsedItem = cacheLeastRecentlyUsed.leastUsedStructure.tail.previousItem.key;
Console.WriteLine("Set Key1 will move it to the first one. First One " + firstItem + ", and the least used one is changed to " + leastUsedItem);

//Get existed item will change the least used one
cacheLeastRecentlyUsed.Get("Key2");
firstItem = cacheLeastRecentlyUsed.leastUsedStructure.header.nextItem.key;
leastUsedItem = cacheLeastRecentlyUsed.leastUsedStructure.tail.previousItem.key;
Console.WriteLine("Get Key2 will move it to the first one. First One " + firstItem + ", and the least used one is changed to " + leastUsedItem);


cacheLeastRecentlyUsed.Clear();
var numberOfItemsInDoubleLinkedList = 1;
var header = cacheLeastRecentlyUsed.leastUsedStructure.header;
var tail = cacheLeastRecentlyUsed.leastUsedStructure.tail;
while (header != tail)
{
    numberOfItemsInDoubleLinkedList++;
    header = header.nextItem;
}

Console.WriteLine("After clear, no item exists on cacheItems. Cache items : " + cacheLeastRecentlyUsed.cacheItems.Count() + ". Double linked list only contains header and tail, size of it : " + numberOfItemsInDoubleLinkedList);

Console.ReadLine();

