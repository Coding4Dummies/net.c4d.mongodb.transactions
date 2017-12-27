using System;

namespace Net.C4D.Mongodb.Transactions.Transactions
{
    public class ExecutedTransaction
    {
        private readonly Guid _transactionId;
        private readonly DateTime _executionDateTime;

        public ExecutedTransaction(Guid transactionId, DateTime executionDateTime)
        {
            _transactionId = transactionId;
            _executionDateTime = executionDateTime;
        }
        public Guid TransactionId { get { return _transactionId; } }

        public DateTime ExecutionDateTime { get { return _executionDateTime; } }
    }
}