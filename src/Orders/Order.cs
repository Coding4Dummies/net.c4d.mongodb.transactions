using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Net.C4D.Mongodb.Transactions.Products;
using Net.C4D.Mongodb.Transactions.Transactions;
using Net.C4D.Mongodb.Transactions.Mongo;
using Newtonsoft.Json;

namespace Net.C4D.Mongodb.Transactions.Orders
{
    public class Order : Entity
    {
        public Guid OrderId { get; private set; }

        public Guid CustomerId { get; set; }

        public List<OrderedProduct> ProductsAndQuantity { get; set; }

        public List<ExecutedTransaction> Transactions { get; set; }

        public OrderStatus Status { get; set; }

        public Order()
        {
            OrderId = Guid.NewGuid();
            Status = OrderStatus.Pending;
            Transactions = new List<ExecutedTransaction>();
        }

        public Order(Guid customerId, List<OrderedProduct> products) : this()
        {
            CustomerId = customerId;
            ProductsAndQuantity = products;
        }
    }
}