using System;
using System.Collections.Generic;
using Net.C4D.Mongodb.Transactions.Ioc;
using Net.C4D.Mongodb.Transactions.Orders;
using Net.C4D.Mongodb.Transactions.Products;
using Net.C4D.Mongodb.Transactions.Setup;
using Net.C4D.Mongodb.Transactions.Mongo;
using MongoDB.Driver;

namespace Net.C4D.Mongodb.Transactions
{
    class Program
    {
        static void Main(string[] args)
        {
            new ServicesInitializer().InitServices();

            new TestDbInitializer().InitTestDb();

            PerformDemoOrder();
        }

        static void PerformDemoOrder()
        {
            var productsCollection = ServicesContainer.GetService<IMongoCollection<Product>>();
            var ordersService = ServicesContainer.GetService<OrdersService>();

            var productsForOrder = productsCollection.Find(p => p.InStockAmmount > 0).ToList();

            var orderProducts = new List<OrderedProduct>();

            foreach (var product in productsForOrder)
            {
                orderProducts.Add(new OrderedProduct(product, 1));
            }

            ordersService.CreateOrder(Guid.NewGuid(), orderProducts);
        }
    }
}