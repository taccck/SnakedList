using System;
using System.Collections;
using UnityEngine;

public class SnakedList<T>
{
    public Node head;
    public Node tail;

    public int Count { get; private set; }

    public class Node : MonoBehaviour
    {   //has to inherit mono behavior so snake nodes can be components and nodes
        [NonSerialized] public T value;
        [NonSerialized] public Node next;
    }
    
    public IEnumerator GetEnumerator()
    {
        Node curr = head;
        while (curr != null)
        {
            yield return curr;
            curr = curr.next;
        }
    }
    
    public Node Add(Node _newNode)
    {
        Count++;
        if (head == null)
        {
            head = _newNode;
            tail = _newNode;
            return null;
        }

        Node old = tail;
        tail.next = _newNode;
        tail = _newNode;
        return old;
    }
}