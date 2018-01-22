namespace Net.C4D.Mongodb.Transactions.Transactions
{
    public enum TransactionStatus
    {
        Pending,
        Completed,
        RolledBack,
        Error
    }
}