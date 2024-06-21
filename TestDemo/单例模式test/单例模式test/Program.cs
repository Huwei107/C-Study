﻿
using 单例模式test;

Singleton1 singleton1 = Singleton1.GetInstance();
Singleton1 singleton2 = Singleton1.GetInstance();


Console.WriteLine($"{singleton1}");
Console.WriteLine($"{singleton2}");

Console.WriteLine($"===================泛型单例======================");









Console.WriteLine($"===================差集======================");

var list1 = new List<int>() { 1, 2, 3 };
var list2 = new List<int>() { 1, 2, 4 };

var res1 = list1.Except(list2).ToList();
res1.ForEach(x =>
{
    Console.WriteLine($"{x}");
});
Console.WriteLine($"====================测试=====================");

var num1 = 3.0;
string result = num1 % 1 == 0 ? "整数" : "小数";
Console.WriteLine($"{result}");

Console.WriteLine($"====================位运算=====================");

int a = 13;
int b = 20;
//a ^= b;
//b ^= a;
//a ^= b;
a = ~a;
Console.WriteLine($"a:{a}   b:{b}");





