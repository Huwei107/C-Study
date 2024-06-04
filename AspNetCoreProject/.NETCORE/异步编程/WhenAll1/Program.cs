

class Progam
{
    private static async Task Main(string[] args)
    {
        var t1 = File.ReadAllTextAsync(@"D:\1.txt");
        var t2 = File.ReadAllTextAsync(@"D:\2.txt");
        var t3 = File.ReadAllTextAsync(@"D:\3.txt");

        var strs = await Task.WhenAll(t1, t2, t3);
        var res1 = strs[0];
        var res2 = strs[1];
        var res3 = strs[2];
        Console.WriteLine(res1);    
        Console.WriteLine(res2);    
        Console.WriteLine(res3);    
    }
}
