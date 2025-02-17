using System;
using BoletinesService;
using Topshelf;

internal class Program
{
    private static void Main(string[] args)
    {
        ConfigureService.Configure();
    }
}