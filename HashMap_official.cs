using System;
using System.Collections;
using System.Collections.Generic;

public class HashMap<K, V> : IEnumerable<KeyValuePair<K, V>>
{
    public class Node
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

    public class KeyCollection : IEnumerable<K>
    {
        class KeyEnumerator : IEnumerator<K>
        {
            private Node[] nodes;
            private int index;
            private Node currentNode;
            private HashMap<K, V> map;
            private int version;

            public KeyEnumerator(Node[] nodes, HashMap<K, V> map)
            {
                this.nodes = nodes;
                this.map = map;
                this.version = map.version;
                Reset();
            }

            public K Current => currentNode.key;

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                nodes = null;
                currentNode = null;
                map = null; 
            }

            public bool MoveNext()
            {
                if (map.version != version)
                {
                    throw new InvalidOperationException("Collection was modified during iteration.");
                }

                if (currentNode != null && currentNode.nextNode != null)
                {
                    currentNode = currentNode.nextNode;
                    return true;
                }

                while (index < nodes.Length - 1)
                {
                    index++;
                    if (nodes[index] != null)
                    {
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

        private Node[] nodes;
        private HashMap<K, V> map;

        public KeyCollection(Node[] nodes, HashMap<K, V> map)
        {
            this.nodes = nodes;
            this.map = map;
        }

        public IEnumerator<K> GetEnumerator()
        {
            return new KeyEnumerator(nodes, map);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class ValueCollection : IEnumerable<V>
    {
        class ValueEnumerator : IEnumerator<V>
        {
            private Node[] nodes;
            private int index;
            private Node currentNode;
            private HashMap<K, V> map;
            private int version;

            public ValueEnumerator(Node[] nodes, HashMap<K, V> map)
            {
                this.nodes = nodes;
                this.map = map;
                this.version = map.version;
                Reset();
            }

            public V Current => currentNode.value;

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                nodes = null;
                currentNode = null;
                map = null;
            }

            public bool MoveNext()
            {
                if (map.version != version)
                {
                    throw new InvalidOperationException("Collection was modified during iteration.");
                }

                if (currentNode != null && currentNode.nextNode != null)
                {
                    currentNode = currentNode.nextNode;
                    return true;
                }

                while (index < nodes.Length - 1)
                {
                    index++;
                    if (nodes[index] != null)
                    {
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

        private Node[] nodes;
        private HashMap<K, V> map;

        public ValueCollection(Node[] nodes, HashMap<K, V> map)
        {
            this.nodes = nodes;
            this.map = map;
        }

        public IEnumerator<V> GetEnumerator()
        {
            return new ValueEnumerator(nodes, map);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    class KeyValueEnumerator : IEnumerator<KeyValuePair<K, V>>
    {
        private Node[] nodes;
        private int index;
        private Node currentNode;
        private HashMap<K, V> map;
        private int initialVersion;

        public KeyValueEnumerator(Node[] nodes, HashMap<K, V> map)
        {
            this.nodes = nodes;
            this.map = map;
            initialVersion = map.version;
            Reset();
        }

        public KeyValuePair<K, V> Current => new KeyValuePair<K, V>(currentNode.key, currentNode.value);

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            nodes = null;
            currentNode = null;
            map = null;  
        }

        public bool MoveNext()
        {
            if (map.version != initialVersion)
            {
                throw new InvalidOperationException("Collection was modified during iteration.");
            }

            if (currentNode != null && currentNode.nextNode != null)
            {
                currentNode = currentNode.nextNode;
                return true;
            }

            while (index < nodes.Length - 1)
            {
                index++;
                if (nodes[index] != null)
                {
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
    private int version;

    public HashMap()
    {
        arrayOfNodes = new Node[16];
        count = 0;
        version = 0;
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
        version++;
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
        version++;
    }

    public Node[] GetNodesArray()
    {
        return arrayOfNodes;
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
                version++;
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
        version++;
    }

    public KeyCollection GetKeyCollection()
    {
        return new KeyCollection(arrayOfNodes, this);
    }

    public ValueCollection GetValueCollection()
    {
        return new ValueCollection(arrayOfNodes, this);
    }

    public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
    {
        return new KeyValueEnumerator(arrayOfNodes, this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
