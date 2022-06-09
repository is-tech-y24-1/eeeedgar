package eeee.dgar;

public class Algo {
    public static void quickSort(int[] arr, int left, int right) {
        if (left >= right) return;
        int pivot = Partition(arr, left, right);
        if (pivot > 1) {
            quickSort(arr, left, pivot - 1);
        }
        if (pivot + 1 < right) {
            quickSort(arr, pivot + 1, right);
        }
    }

    public static void mergeSort(int[] arr, int left, int right) {
        if (right <= left) return;
        int mid = (right + left) / 2;
        mergeSort(arr, left, mid);
        mergeSort(arr, (mid + 1), right);
        merge(arr, left, (mid + 1), right);
    }

    private static void merge(int[] arr, int left, int mid, int right) {
        int[] temp = new int[arr.length];
        int i;
        int eol = (mid - 1);
        int pos = left;
        int num = (right - left + 1);

        while ((left <= eol) && (mid <= right)) {
            if (arr[left] <= arr[mid]) {
                temp[pos++] = arr[left++];
            } else {
                temp[pos++] = arr[mid++];
            }
        }
        while (left <= eol)
            temp[pos++] = arr[left++];
        while (mid <= right)
            temp[pos++] = arr[mid++];
        for (i = 0; i < num; i++) {
            arr[right] = temp[right];
            right--;
        }
    }

    private static int Partition(int[] arr, int left, int right) {
        int pivot = arr[left];
        while (true) {
            while (arr[left] < pivot) {
                left++;
            }
            while (arr[right] > pivot) {
                right--;
            }
            if (left < right) {
                if (arr[left] == arr[right]) return right;
                int temp = arr[left];
                arr[left] = arr[right];
                arr[right] = temp;
            } else {
                return right;
            }
        }
    }
}
