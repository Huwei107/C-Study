/*
 * 
 * 
1、为什么不能lock值类型
    比如lock(1)呢?lock本质上Monitor.Enter，Monitor.Enter会使值类型装箱，每次lock的是装箱后的对象。
lock其实是类似编译器的语法糖，因此编译器直接限制住不能lock值类型。退一万步说，就算能编译器允许你lock(1)，
但是object.ReferenceEquals(1,1)始终返回false(因为每次装箱后都是不同对象),
也就是说每次都会判断成未申请互斥锁，这样在同一时间，别的线程照样能够访问里面的代码，达不到同步的效果。同理lock((object)1)也不行。

2、Lock字符串
    锁定字符串尤其危险，因为字符串被公共语言运行库 (CLR)“暂留”。 
这意味着整个程序中任何给定字符串都只有一个实例，就是这同一个对象表示了所有运行的应用程序域的所有线程中的该文本。
因此，只要在应用程序进程中的任何位置处具有相同内容的字符串上放置了锁，就将锁定应用程序中该字符串的所有实例。

3、MSDN推荐的Lock对象
    通常，最好避免锁定 public 类型或锁定不受应用程序控制的对象实例。例如，如果该实例可以被公开访问，
则 lock(this) 可能会有问题，因为不受控制的代码也可能会锁定该对象。
这可能导致死锁，即两个或更多个线程等待释放同一对象。出于同样的原因，锁定公共数据类型(相比于对象)也可能导致问题。
而且lock(this)只对当前对象有效，如果多个对象之间就达不到同步的效果。
而自定义类推荐用私有的只读静态对象，比如：private static readonly object obj = new object();
为什么要设置成只读的呢?这时因为如果在lock代码段中改变obj的值，其它线程就畅通无阻了，因为互斥锁的对象变了，object.ReferenceEquals必然返回false。

4、lock(typeof(Class))
    与锁定字符串一样，范围太广了。

 */

/*

C# 字符串池（String Interning）：
    C# 中的字符串池（也叫做字符串常量池或字符串内存池）是为了优化内存使用而设计的。
字符串池的工作原理是，所有在程序中创建的相同内容的字符串会指向同一个内存位置，而不是每次创建一个新的字符串实例。

 字符串池的工作原理：
1、字符串的不可变性：在 C# 中，字符串是不可变的（immutable）。这意味着一旦字符串被创建，它的内容不能更改。这也就使得字符串对象可以被共享，
因为如果一个字符串的内容不会变化，那么就可以确保不同的地方引用该字符串时，所有地方的引用都指向同一内存地址。

2、字符串常量池：C# 自动维护一个字符串常量池（interned string pool）。当你创建一个新的字符串，如果这个字符串的值已经存在于池中，
C# 会返回池中已经存在的字符串实例，而不是创建一个新的实例。如果字符串池中没有该值，则会将该字符串添加到池中。

3、string.Intern() 方法：你可以使用 string.Intern() 方法手动将字符串添加到常量池中或检查是否已经存在于池中。
 
 
 */





class Program
{
    static void Main(string[] args)
    {
        Thread thread1 = new Thread(new ThreadStart(ThreadStart1));
        thread1.Name = "Thread1";
        Thread thread2 = new Thread(new ThreadStart(ThreadStart2));
        thread2.Name = "Thread2";
        Thread thread3 = new Thread(new ThreadStart(ThreadStart3));
        thread3.Name = "Thread3";

        thread1.Start();
        thread2.Start();
        thread3.Start();

        Console.ReadKey();

    }


    static object _object = new object();
    static void Done(int millisecondsTimeout)
    {
        Console.WriteLine(string.Format("{0} -> {1}.Start", DateTime.Now.ToString("HH:mm:ss"), Thread.CurrentThread.Name));

        //下边代码段同一时间只能由一个线程在执行
        lock (_object)
        {
            Console.WriteLine(string.Format("{0} -> {1}进入锁定区域.", DateTime.Now.ToString("HH:mm:ss"), Thread.CurrentThread.Name));
            Thread.Sleep(millisecondsTimeout);
            Console.WriteLine(string.Format("{0} -> {1}退出锁定区域.", DateTime.Now.ToString("HH:mm:ss"), Thread.CurrentThread.Name));
        }
    }
    static void ThreadStart1()
    {
        Done(5000);
    }
    static void ThreadStart2()
    {
        Done(3000);
    }
    static void ThreadStart3()
    {
        Done(1000);
    }
}











