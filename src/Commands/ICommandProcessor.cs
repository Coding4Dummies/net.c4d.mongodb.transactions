using System;
using Net.C4D.Mongodb.Transactions.Transactions;

namespace Net.C4D.Mongodb.Transactions.Commands
{
    public interface ICommandProcessor
    {
        bool CanProcess(ICommand command);

        void Process(ICommand command, Transaction transaction);

        void RollBack(ICommand command, Transaction transaction);
    }
}