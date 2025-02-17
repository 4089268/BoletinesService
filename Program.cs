using System;
using BoletinesService;
using Microsoft.Extensions.Configuration;
using Topshelf;

internal class Program
{
    private static void Main(string[] args)
    {
        ConfigureService.Configure();
    }
}