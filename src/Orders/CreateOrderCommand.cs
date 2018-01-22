using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using Net.C4D.Mongodb.Transactions.Commands;
using Net.C4D.Mongodb.Transactions.Ioc;
using Net.C4D.Mongodb.Transactions.Mongo;
using Net.C4D.Mongodb.Transactions.Products;
using Net.C4D.Mongodb.Transactions.Transactions;

namespace Net.C4D.Mongodb.Transactions.Orders
{
    public class CreateOrderCommand : ICommand
    {
        public Guid CustomerId { get; set; }

        public List<OrderedProduct> Products { get; set; }

        public Guid TransactionId { get; set; }
    }

    public class CreateOrderCommandProcessor : ICommandProcessor
    {
        private readonly IMongoCollection<Order> _ordersCollection;

        public CreateOrderCommandProcessor()
        {
            _ordersCollection = ServicesContainer.GetService<IMongoCollection<Order>>();
        }

        public bool CanProcess(ICommand command)
        {
            return command is CreateOrderCommand;
        }

        public void Process(ICommand command, Transaction transaction)
        {
            var processedCommand = command as CreateOrderCommand;

            if (processedCommand == null)
                throw new InvalidOperationException("Unsupported or empty command passed to processor");

            var order = new Order(processedCommand.CustomerId, processedCommand.Products);
            order.Transactions.Add(transaction.TransactionId);

            _ordersCollection.InsertOne(order);
        }

        public void RollBack(ICommand command, Transaction transaction)
        {
            var processedCommand = command as CreateOrderCommand;

            if (processedCommand == null)
                throw new InvalidOperationException("Unsupported or empty command passed to processor");

            var insertedOrder = _ordersCollection.Find(order =>
                order.Transactions.Contains(transaction.TransactionId) &&
                order.ProductsAndQuantity == processedCommand.Products &&
                order.CustomerId == processedCommand.CustomerId).FirstOrDefault();

            if (insertedOrder != null)
            {
                _ordersCollection.DeleteOne(order => order._id == insertedOrder._id);
            }
        }
    }
}