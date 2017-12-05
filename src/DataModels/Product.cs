using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Net.C4D.MongodbProvider;

namespace Net.C4D.Mongodb.Transactions.DataModels {

    [DataContract (Name = "product")]
    public class Product : Entity {
        [DataMember (Name = "id")]
        public Guid Id { get; set; }

        [DataMember (Name = "name")]
        public string Name { get; set; }

        [DataMember (Name = "description")]
        public string Description { get; set; }

        [DataMember (Name = "in-stock-amount")]
        public int InStockAmmount { get; set; }

        [DataMember (Name = "transactions")]
        public List<Transaction> Transactions { get; set; }
    }
}