using System;

public class demo { 
    static public void Main(string[] args) { 
        Console.WriteLine("Hello from .NET!");
        //
        // create the proxy class 
        HelloWorld foo = new HelloWorld();
        Console.WriteLine(foo.speak());
} }

