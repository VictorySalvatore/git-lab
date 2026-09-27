using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehavPatterns
{
    public interface SortingStrategy
    {
        void Sort(int[] array);
    }
    public class BubbleSortStrategy : SortingStrategy
    {
        public void Sort(int[] array)
        {
            Console.WriteLine("Sorting using Bubble Sort");
            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = 0; j < array.Length - i - 1; j++)
                {
                    if (array[j] > array[j + 1])
                    {
                        int temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                }
            }
        }
    }
    public class QuickSortStrategy : SortingStrategy
    {
        public void Sort(int[] array)
        {
            Console.WriteLine("Sorting using Quick Sort");
            QuickSort(array, 0, array.Length - 1);
        }

        private void QuickSort(int[] array, int low, int high)
        {
            if (low < high)
            {
                int pivot = Partition(array, low, high);
                QuickSort(array, low, pivot - 1);
                QuickSort(array, pivot + 1, high);
            }
        }

        private int Partition(int[] array, int low, int high)
        {
            int pivot = array[high];
            int i = low - 1;
            for (int j = low; j < high; j++)
            {
                if (array[j] < pivot)
                {
                    i++;
                    int temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
                }
            }
            int temp2 = array[i + 1];
            array[i + 1] = array[high];
            array[high] = temp2;
            return i + 1;
        }
    }
    public class Sorter /// Контекст, использует стратегию сортировки
    {
        private SortingStrategy strat;

        public void SetStrategy(SortingStrategy strategy)
        {
            strat = strategy;
        }

        public void SortArray(int[] array)
        {
            strat.Sort(array);
        }
    }
}