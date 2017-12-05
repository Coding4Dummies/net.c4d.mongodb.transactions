using System;
using System.Runtime.Serialization;
using Net.C4D.Mongodb.Transactions.Enums;
using Net.C4D.MongodbProvider;

namespace Net.C4D.Mongodb.Transactions.DataModels {
    public class ProductAction : Entity {
        [DataMember (Name = "action")]
        public ActionType Action { get; set; }

        [DataMember (Name = "target-document")]
        public string TargetDocument { get; set; }

        [DataMember (Name = "arguments")]
        public object[] Arguments { get; set; }
    }
}