using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Net.C4D.Mongodb.Transactions.Enums;
using Net.C4D.MongodbProvider;

namespace Net.C4D.Mongodb.Transactions.DataModels {

    [DataContract (Name = "transaction")]
    public class Transaction : Entity {
        [DataMember (Name = "id")]
        public Guid Id { get; set; }

        [DataMember (Name = "time-stamp")]
        public DateTime TimeStamp { get; set; }

        [DataMember (Name = "product-actions")]
        public List<ProductAction> ProductActions { get; set; }
            [DataMember (Name = "status")]
        public TransactionStatus Status { get; set; }
    }
}