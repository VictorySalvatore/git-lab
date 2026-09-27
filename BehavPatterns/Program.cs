using System.Reflection.Metadata;

namespace BehavPatterns
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("STRATEGY");

            Sorter sorter = new Sorter();
            int[] array1 = { 5, 3, 8, 4, 2 };
            Console.WriteLine("Исходный массив: " + string.Join(", ", array1));
            sorter.SetStrategy(new BubbleSortStrategy());
            sorter.SortArray(array1);
            Console.WriteLine("Результат: " + string.Join(", ", array1));
            int[] array2 = { 5, 3, 8, 4, 2 };
            sorter.SetStrategy(new QuickSortStrategy());
            sorter.SortArray(array2);
            Console.WriteLine("Результат: " + string.Join(", ", array2));
            Console.WriteLine();

            Console.WriteLine("CHAIN OF RESPONSIBILITY");

            Handler handlerA = new ConcreteHandlerA();
            Handler handlerB = new ConcreteHandlerB();
            Handler handlerC = new ConcreteHandlerC();
            handlerA.SetNextHandler(handlerB);
            handlerB.SetNextHandler(handlerC);
            handlerA.HandleRequest(new Request(RequestType.TypeA));
            handlerA.HandleRequest(new Request(RequestType.TypeB));
            handlerA.HandleRequest(new Request(RequestType.TypeC));
            Console.WriteLine();

            Console.WriteLine("ITERATOR");

            int[] numbers = { 1, 2, 3, 4, 5 };
            ArrayCollection<int> collection = new ArrayCollection<int>(numbers);
            Iterator<int> iterator = collection.CreateIterator();
            while (iterator.HasNext())
            {
                Console.WriteLine(iterator.Next());
            }
            Console.WriteLine();
        }
    }
}
