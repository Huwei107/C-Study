







using 单例模式test;

Singleton1 singleton1 = Singleton1.GetInstance();
Singleton1 singleton2 = Singleton1.GetInstance();



Console.WriteLine($"{singleton1}");
Console.WriteLine($"{singleton2}");



