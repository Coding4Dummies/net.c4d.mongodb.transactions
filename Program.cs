using System;
using Microsoft.Extensions.Configuration;

namespace net.c4d.mongodb.transactions
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration config =  new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            
        }
    }
}
