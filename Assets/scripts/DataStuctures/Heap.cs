using System;
using System.Collections.Generic;

public class BinaryHeap<T> where T : IComparable<T>
{
    private List<T> heap = new List<T>();

    public bool IsEmpty()
    {
        return heap.Count == 0;
    }

    public void Add(T item)
    {
        heap.Add(item);
        HeapifyUp(heap.Count - 1);
    }

    public T Remove()
    {
        if (heap.Count == 0)
            throw new InvalidOperationException("Heap is empty");

        T root = heap[0];
        int lastIndex = heap.Count - 1;
        heap[0] = heap[lastIndex];
        heap.RemoveAt(lastIndex);

        if (heap.Count > 1)
            HeapifyDown(0);

        return root;
    }

    private void HeapifyUp(int index)
    {
        while (index > 0)
        {
            int parentIndex = (index - 1) / 2;

            if (heap[index].CompareTo(heap[parentIndex]) >= 0)
                break;

            Swap(index, parentIndex);
            index = parentIndex;
        }
    }

    private void HeapifyDown(int index)
    {
        int leftChild;
        int rightChild;
        int smallestChild;

        while (true)
        {
            leftChild = 2 * index + 1;
            rightChild = 2 * index + 2;
            smallestChild = index;

            if (leftChild < heap.Count && heap[leftChild].CompareTo(heap[smallestChild]) < 0)
                smallestChild = leftChild;

            if (rightChild < heap.Count && heap[rightChild].CompareTo(heap[smallestChild]) < 0)
                smallestChild = rightChild;

            if (smallestChild == index)
                break;

            Swap(index, smallestChild);
            index = smallestChild;
        }
    }

    private void Swap(int i, int j)
    {
        T temp = heap[i];
        heap[i] = heap[j];
        heap[j] = temp;
    }
}
