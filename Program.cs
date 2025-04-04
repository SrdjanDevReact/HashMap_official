using System;

class Program
{
    static void Main(string[] args)
    {
        HashMap<string, int> hashMap = new HashMap<string, int>();

        // Dodavanje elemenata
        hashMap.Add("A", 1);
        hashMap.Add("B", 2);
        hashMap.Add("C", 3);
        hashMap.Add("A123", 4);
        hashMap.Add("B123", 5);
        hashMap.Add("C123", 6);
        hashMap.Add("A223", 43);
        hashMap.Add("B223", 53);
        hashMap.Add("C223", 63);

        // Ispis sadržaja niza unutar HashMap
        for (int i = 0; i < hashMap.GetNodesArray().Length; i++)
        {
            var currentNode = hashMap.GetNodesArray()[i];
            while (currentNode != null)
            {
                Console.WriteLine($"Index: {i}, Key: {currentNode.key}, Value: {currentNode.value}");
                currentNode = currentNode.nextNode;
            }
        }

        // Iteriramo kroz kolekciju i menjamo HashMap tokom iteracije
        try
        {
            foreach (var kvpair in hashMap)
            {
                Console.WriteLine($"Key: {kvpair.Key} -- Value: {kvpair.Value}");
                
                // Dodajemo novi element unutar foreach petlje kako bi izazvali koliziju verzija
                hashMap.Add("NewKey", 100);
            }
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Exception caught: {ex.Message}");
        }

        // Dalji ispis 
        try
        {
            foreach (var k in hashMap.GetKeyCollection())
            {
                Console.WriteLine($"Key: {k}");
            }
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Exception caught during key iteration: {ex.Message}");
        }

        try
        {
            foreach (var v in hashMap.GetValueCollection())
            {
                Console.WriteLine($"Value: {v}");
            }
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Exception caught during value iteration: {ex.Message}");
        }
    }
}



