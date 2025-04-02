using System;
using System.Collections;
using System.Collections.Generic;

public class HashMap<K, V> : IEnumerable<KeyValuePair<K,V>>
{
    class Node
    {
        public K key;
        public V value;
        public Node nextNode;

        public Node(K key, V value)
        {
            this.key = key;
            this.value = value;
        }
    }

    class KeyValueEnumerator : IEnumerator<KeyValuePair<K, V>>
    {

        private Node[] nodes;
        private int index;
        private Node currentNode;

        public KeyValueEnumerator(Node[] nodes)
        {
            this.nodes = nodes;
            Reset();

        }
        public KeyValuePair<K, V> Current => new KeyValuePair<K, V>(currentNode.key,currentNode.value);

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            System.Console.WriteLine("Zavrseno!");
        }

        public bool MoveNext()
        {
            if(currentNode != null && currentNode.nextNode != null) {
                currentNode = currentNode.nextNode;
                return true;
            }
            while(index < nodes.Length - 1 ){
                index++;
                if(nodes[index]!= null){
                    currentNode = nodes[index];
                    return true;
                }
            }
            return false;
            
        }

        public void Reset()
        {
            index = -1;
            currentNode = null;
        }
    }

    private Node[] arrayOfNodes;
    private int count;
    private const double LoadFactor = 0.75;

    public HashMap()
    {
        arrayOfNodes = new Node[16];
        count = 0;
    }

    public int Count => count;

    public void Add(K key, V value)
    {
        if ((double)count / arrayOfNodes.Length > LoadFactor)
        {
            Expand();
        }

        int bucketIndex = GetBucketValue(key);
        if (arrayOfNodes[bucketIndex] == null)
        {
            arrayOfNodes[bucketIndex] = new Node(key, value);
        }
        else
        {
            Node currentNode = arrayOfNodes[bucketIndex];
            while (currentNode != null)
            {
                if (currentNode.key.Equals(key))
                {
                    currentNode.value = value;
                    return;
                }
                currentNode = currentNode.nextNode;
            }
            Node newNode = new Node(key, value);
            newNode.nextNode = arrayOfNodes[bucketIndex];
            arrayOfNodes[bucketIndex] = newNode;
        }
        count++;
    }

    private void Expand()
    {
        int newSize = arrayOfNodes.Length * 2;
        Node[] newNiz = new Node[newSize];

        foreach (var oldNode in arrayOfNodes)
        {
            Node currentNode = oldNode;
            while (currentNode != null)
            {
                int newBucketIndex = GetBucketValue(currentNode.key, newSize);

                Node next = currentNode.nextNode;
                currentNode.nextNode = newNiz[newBucketIndex];
                newNiz[newBucketIndex] = currentNode;

                currentNode = next;
            }
        }
        arrayOfNodes = newNiz;
    }

    public int GetBucketValue(K key)
    {
        return GetBucketValue(key, arrayOfNodes.Length);
    }
    public int GetBucketValue(K key, int lengthOfArray)
    {
        return (key.GetHashCode() & 0x7FFFFFFF) % lengthOfArray;
    }

    public bool TryGetValue(K key, out V value)
    {
        int bucketIndex = GetBucketValue(key);
        Node currentNode = arrayOfNodes[bucketIndex];
        while (currentNode != null)
        {
            if (key.Equals(currentNode.key))
            {
                value = currentNode.value;
                return true;
            }
            currentNode = currentNode.nextNode;
        }

        value = default(V);
        return false;
    }

    public V this[K key]
    {
        get
        {
            if (TryGetValue(key, out V value))
            {
                return value;
            }
            throw new KeyNotFoundException();
        }
        set
        {
            Add(key, value);
        }
    }

    public bool ContainsKey(K key)
    {
        return TryGetValue(key, out _);
    }

    public bool Remove(K key)
    {
        int bucketIndex = GetBucketValue(key);
        ref Node currentNode = ref arrayOfNodes[bucketIndex];

        while (currentNode != null)
        {
            if (key.Equals(currentNode.key))
            {
                currentNode = currentNode.nextNode;
                count--;
                return true;
            }
            currentNode = ref currentNode.nextNode;
        }
        return false;
    }

    public void Clear()
    {
        arrayOfNodes = new Node[16];
        count = 0;
    }

    

    public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
    {
        return new KeyValueEnumerator(arrayOfNodes);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
