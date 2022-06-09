namespace CSharpBenchmarking
{
    public class Algo
    {
        public static void QuickSort(IList<int> arr, int left, int right)
        {
            if (left >= right)
                return;

            var pivot = Partition(arr, left, right);

            if (pivot > 1)
            {
                QuickSort(arr, left, pivot - 1);
            }

            if (pivot + 1 < right)
            {
                QuickSort(arr, pivot + 1, right);
            }
        }

        public static void MergeSort(IList<int> arr, int left, int right)
        {
            if (right <= left)
                return;
            
            var mid = (right + left) / 2;
            
            MergeSort(arr, left, mid);
            MergeSort(arr, (mid + 1), right);
            
            Merge(arr, left, (mid + 1), right);
        }

        private static void Merge(IList<int> arr, int left, int mid, int right)
        {
            int[] buf = new int[arr.Count];
            int i;
            var eol = (mid - 1);
            var pos = left;
            var num = (right - left + 1);

            while ((left <= eol) && (mid <= right))
            {
                if (arr[left] <= arr[mid])
                {
                    buf[pos++] = arr[left++];
                }
                else
                {
                    buf[pos++] = arr[mid++];
                }
            }

            while (left <= eol)
                buf[pos++] = arr[left++];
 
            while (mid <= right)
                buf[pos++] = arr[mid++];
            
            for (i = 0; i < num; i++)
            {
                arr[right] = buf[right];
                right--;
            }
        }

        private static int Partition(IList<int> arr, int left, int right)
        {
            var pivot = arr[left];
            while (true)
            {
                while (arr[left] < pivot)
                {
                    left++;
                }

                while (arr[right] > pivot)
                {
                    right--;
                }

                if (left < right)
                {
                    if (arr[left] == arr[right])
                        return right;
                    (arr[right], arr[left]) = (arr[left], arr[right]);
                }
                else
                {
                    return right;
                }
            }
        }
    }
}