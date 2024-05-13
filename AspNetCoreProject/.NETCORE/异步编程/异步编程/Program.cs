
using System.Text;




class Program
{
    private static async Task Main(string[] args)
    {
        #region 线程切换
        Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < 10000; i++)
        {
            sb.Append("1111111111111");
        }
        await File.WriteAllTextAsync(@"d:\1.txt", sb.ToString());
        Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
        Console.WriteLine("============================================================================================================");
        //如果写入内容过少，线程id不变
        //await调用的等待期间，.NET会把当前的线程返回给线程池，等异步方法执行完毕后，框架会从线程池再取出来一个线程执行后续的代码
        #endregion

        #region 异步方法不等于多线程
        Console.WriteLine("之前：" + Thread.CurrentThread.ManagedThreadId);
        var r = await CalcAsync(5000);
        Console.WriteLine($"r={r}");
        Console.WriteLine("之后：" + Thread.CurrentThread.ManagedThreadId);
        #endregion
    }



    public static async Task<double> CalcAsync(int n)
    {
        //Console.WriteLine("CalcAsync," + Thread.CurrentThread.ManagedThreadId);
        //double result = 0;
        //Random rnd = new Random();
        //for (int i = 0; i < n * n; i++)
        //{
        //    result += rnd.NextDouble();
        //}
        //return result;

        return await Task.Run(() =>
        {
            Console.WriteLine("CalcAsync," + Thread.CurrentThread.ManagedThreadId);
            double result = 0;
            Random rnd = new Random();
            for (int i = 0; i < n * n; i++)
            {
                result += rnd.NextDouble();
            }
            return result;
        });
    }

}









