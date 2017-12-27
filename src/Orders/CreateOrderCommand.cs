using System;
using System.Collections.Generic;
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
        private readonly MongoRepository<Order> _ordersRepository;

        public CreateOrderCommandProcessor()
        {
            _ordersRepository = ServicesContainer.GetService<MongoRepository<Order>>();
        }

        public bool CanProcess(ICommand command)
        {
            return command is CreateOrderCommand;
        }

        public void Process(ICommand command)
        {
            var processedCommand = command as CreateOrderCommand;

            if (processedCommand == null)
                throw new InvalidOperationException("Unsupported command passed to processor");

            var order = new Order(processedCommand.CustomerId, processedCommand.Products);
            order.Transactions.Add(new ExecutedTransaction(processedCommand.TransactionId, DateTime.Now));

            _ordersRepository.Insert(order);
        }
    }
}