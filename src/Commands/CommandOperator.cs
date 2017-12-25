using System.Runtime.Serialization;
using Net.C4D.Mongodb.Transactions.Mongo;

namespace Net.C4D.Mongodb.Transactions.Commands
{
    public enum CommandOperator
    {
        Add,
        Substract,
        SetValue,
        Delete,
        CreateNew
    }
}