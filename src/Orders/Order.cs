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

        public List<Tuple<Product, int>> ProductsAndQuantity { get; set; } //<product, ammount>

        public List<Tuple<Guid, DateTime>> Transactions { get; set; } // <transaction id, "touched" date time>

        public OrderStatus Status { get; set; }

        public Order()
        {
            OrderId = Guid.NewGuid();
            Status = OrderStatus.Pending;
            Transactions = new List<Tuple<Guid, DateTime>>();
        }

        public Order(Guid customerId, List<Tuple<Product, int>> products) : this()
        {
            CustomerId = customerId;
            ProductsAndQuantity = products;
        }
    }
}