using System;

class Program
{
    static void Main(string[] args)
    {
        HashMap<string, int> hashMap = new HashMap<string, int>();

        hashMap.Add("Jedan", 1);
        hashMap.Add("Dva", 2);
        hashMap.Add("Tri", 3);
        hashMap.Add("Cetiri", 4);


        foreach(var kvpair in hashMap){
            System.Console.WriteLine($"Key: {kvpair.Key} -- Value: {kvpair.Value}");
        }

        foreach(var k in hashMap.GetKeyCollection()){
            System.Console.WriteLine($"Key: {k}");
        }
        
        foreach(var v in hashMap.GetValueCollection()){
            System.Console.WriteLine($"Value: {v}");
        }

    }
}
