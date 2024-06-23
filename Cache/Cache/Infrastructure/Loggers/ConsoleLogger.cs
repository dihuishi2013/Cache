using Cache.Interface;

namespace Cache.Infrastructure.Loggers
{
    public class ConsoleLogger : ILogger
    {
        public void Display(string message)
        {
            Console.WriteLine(message);
        }
    }
}
