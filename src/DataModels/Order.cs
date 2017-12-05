using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Net.C4D.MongodbProvider;

namespace Net.C4D.Mongodb.Transactions.DataModels {

    [DataContract (Name = "order")]
    public class Order : Entity {
        [DataMember (Name = "id")]
        public Guid Id { get; set; }

        [DataMember (Name = "customer-id")]
        public Guid CustomerId { get; set; }

        [DataMember (Name = "products")]
        public Tuple<Guid, int> Products { get; set; } //<product-id, ammount>

        [DataMember (Name = "transactions")]
        public List<Transaction> Transactions { get; set; }
    }
}