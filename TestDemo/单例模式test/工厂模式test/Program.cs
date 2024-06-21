using 工厂模式test;

/*
 工厂模式（Factory Pattern）是一种创建型设计模式，它提供了一种创建对象的最佳方式。在C#中，工厂模式通过定义一个公共接口或抽象类来创建对象，而具体的对象创建则由工厂类来实现。 工厂模式主要包含三个角色：

1. 抽象产品（Abstract Product）：定义了产品的接口，具体产品需要实现这个接口。

2. 具体产品（Concrete Product）：实现了抽象产品接口的具体类。

3. 工厂（Factory）：负责创建具体产品的工厂类，通常包含一个创建产品的方法。
 
 */

Factory factory = new();    //Factory factory = new Factory();

// 创建具体产品A
IProduct productA = factory.CreateProduct("A");
productA.Operation();  // 输出：具体产品A的操作

// 创建具体产品B
IProduct productB = factory.CreateProduct("B");
productB.Operation();  // 输出：具体产品B的操作


Console.ReadLine();

