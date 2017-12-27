using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Net.C4D.Mongodb.Transactions.Commands;
using Net.C4D.Mongodb.Transactions.Mongo;

namespace Net.C4D.Mongodb.Transactions.Transactions
{
    public class Transaction : Entity
    {
        public Guid TransactionId { get; private set; }

        public DateTime TimeStamp { get; private set; }

        public TransactionStatus Status { get; set; }

        public List<ICommand> Commands { get; set; }

        public Transaction()
        {
            TransactionId = Guid.NewGuid();
            TimeStamp = DateTime.Now;
            Status = TransactionStatus.Pending;
        }
    }
}