using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Net.C4D.Mongodb.Transactions.Commands;
using Net.C4D.Mongodb.Transactions.Ioc;
using Net.C4D.Mongodb.Transactions.Orders;
using Net.C4D.Mongodb.Transactions.Products;
using Net.C4D.Mongodb.Transactions.Transactions;
using Net.C4D.Mongodb.Transactions.Mongo;
using MongoDB.Driver;

namespace Net.C4D.Mongodb.Transactions.Setup
{
    public class ServicesInitializer
    {
        public void InitServices()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true);

            var config = builder.Build();

            var mongodbProvider = new MongoDatabaseProvider(config["ConnectionStrings:DefaultConnection"]);

            ServicesContainer.RegisterService<IMongoCollection<Order>>(mongodbProvider.GetMongoCollection<Order>());
            ServicesContainer.RegisterService<IMongoCollection<Product>>(mongodbProvider.GetMongoCollection<Product>());
            ServicesContainer.RegisterService<IMongoCollection<Transaction>>(mongodbProvider.GetMongoCollection<Transaction>());

            var commandsProcessors = new List<ICommandProcessor>();
            commandsProcessors.Add(new CreateOrderCommandProcessor());
            commandsProcessors.Add(new UpdateProductQuantityCommandProcessor());

            ServicesContainer.RegisterService<List<ICommandProcessor>>(commandsProcessors);

            ServicesContainer.RegisterService<TransactionsService>(new TransactionsService());
            ServicesContainer.RegisterService<OrdersService>(new OrdersService());
        }
    }
}