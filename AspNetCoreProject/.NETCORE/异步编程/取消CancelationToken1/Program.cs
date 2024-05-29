



using System.Diagnostics;

class Program
{
    private static async Task Main(string[] args)
    {
        try
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            //1秒后不结束就直接结束
            cts.CancelAfter(1000);
            await DownloadAsync3("https://www.baidu.com/", 100, cts.Token);
        }
        catch (TaskCanceledException ex)
        {
            Console.WriteLine($"{DateTime.Now}：{ex.Message}请求被取消！");
        }
    }

    static async Task DownloadAsync1(string url, int n)
    {
        using (HttpClient clent = new HttpClient())
        {
            for (int i = 0; i < n; i++)
            {
                var html = await clent.GetStringAsync(url);
                Console.WriteLine($"{DateTime.Now}：{html}");
            }
        }
    }

    static async Task DownloadAsync2(string url, int n, CancellationToken cancellationToken)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        stopwatch.Start();
        using (HttpClient clent = new HttpClient())
        {

            for (int i = 0; i < n; i++)
            {
                var html = await clent.GetStringAsync(url);
                if (cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine($"{DateTime.Now}：请求被取消！");
                    break;
                }
                Console.WriteLine($"{DateTime.Now}：{html}");
            }
        }
        stopwatch.Stop();
        Console.WriteLine($"共耗时：{stopwatch.ElapsedMilliseconds}");
    }

    /// <summary>
    /// 这种方法更好，主不过要自己捕获异常来处理
    /// </summary>
    /// <param name="url"></param>
    /// <param name="n"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    static async Task DownloadAsync3(string url, int n, CancellationToken cancellationToken)
    {
        using (HttpClient clent = new HttpClient())
        {
            for (int i = 0; i < n; i++)
            {
                var responseMessage = await clent.GetAsync(url, cancellationToken);//抛出异常来终止
                var html = responseMessage.Content.ReadAsStringAsync();
                Console.WriteLine($"{DateTime.Now}：{html}");
            }
        }
    }
}


