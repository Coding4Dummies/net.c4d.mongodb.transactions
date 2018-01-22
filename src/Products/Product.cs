using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Net.C4D.Mongodb.Transactions.Mongo;
using Newtonsoft.Json;
using Net.C4D.Mongodb.Transactions.Transactions;

namespace Net.C4D.Mongodb.Transactions.Products
{
    public class Product : Entity
    {
        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int InStockAmmount { get; set; }

        public List<Guid> Transactions { get; set; }

        public Product()
        {
            ProductId = Guid.NewGuid();
            Transactions = new List<Guid>();
        }
    }
}