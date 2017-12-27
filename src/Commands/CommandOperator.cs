using System;
using System.Runtime.Serialization;
using Net.C4D.Mongodb.Transactions.Mongo;

namespace Net.C4D.Mongodb.Transactions.Commands
{
    [Flags]
    public enum CommandOperator
    {
        Add = 1,
        SetValue = 2,
        Delete = 4,
        CreateNew = 8
    }
}