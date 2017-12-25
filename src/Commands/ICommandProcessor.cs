using System;

namespace Net.C4D.Mongodb.Transactions.Commands
{
    public interface ICommandProcessor
    {
        bool CanProcess(ICommand command);

        void Process(ICommand command);
    }
}