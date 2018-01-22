using System;
using System.Collections.Generic;
using System.Linq;
using Net.C4D.Mongodb.Transactions.Commands;
using Net.C4D.Mongodb.Transactions.Ioc;
using Net.C4D.Mongodb.Transactions.Products;
using Net.C4D.Mongodb.Transactions.Transactions;

namespace Net.C4D.Mongodb.Transactions.Orders
{
    public class OrdersService
    {
        private readonly TransactionsService _transactionsService;

        public OrdersService()
        {
            _transactionsService = ServicesContainer.GetService<TransactionsService>();
        }

        public void CreateOrder(Guid customerId, List<OrderedProduct> productsAndAmounts)
        {
            var createOrderTransactionCommands = new List<ICommand>();

            createOrderTransactionCommands.Add(
                new CreateOrderCommand
                {
                    CustomerId = customerId,
                    Products = productsAndAmounts
                });

            createOrderTransactionCommands.AddRange(
                productsAndAmounts.Select(t => new UpdateProductQuantityCommand
                {
                    Product = t.Product,
                    Operator = CommandOperator.Add,
                    Value = -t.Quantity
                }));

            var createOrderTransaction = _transactionsService.InitTransaction(createOrderTransactionCommands);

            try
            {
                _transactionsService.CommitTransaction(createOrderTransaction);

                //_transactionsService.RollBackTransaction(createOrderTransaction);
            }
            catch
            {
                _transactionsService.RollBackTransaction(createOrderTransaction);
            }
        }
    }
}