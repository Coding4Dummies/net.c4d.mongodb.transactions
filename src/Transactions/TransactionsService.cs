using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using Net.C4D.Mongodb.Transactions.Mongo;
using Net.C4D.Mongodb.Transactions.Commands;
using Net.C4D.Mongodb.Transactions.Ioc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Net.C4D.Mongodb.Transactions.Transactions
{
    public class TransactionsService
    {
        private readonly IMongoCollection<Transaction> _transactionsCollection;

        private readonly List<ICommandProcessor> _commandsProcessors;

        public TransactionsService()
        {
            _transactionsCollection = ServicesContainer.GetService<IMongoCollection<Transaction>>();
            _commandsProcessors = ServicesContainer.GetService<List<ICommandProcessor>>();
        }

        internal Transaction InitTransaction(List<ICommand> transactionCommands)
        {
            if (transactionCommands == null || transactionCommands.Count == 0)
                throw new ArgumentException("Commands can't be null or empty");

            var transaction = new Transaction();

            transaction.Commands = transactionCommands;
            transaction._id = ObjectId.GenerateNewId();

            _transactionsCollection.InsertOne(transaction);

            return transaction;
        }

        public void CommitTransaction(Transaction transaction)
        {
            ProcessTransaction(transaction, TransactionProcessAction.Commit);

            transaction.Status = TransactionStatus.Completed;

            _transactionsCollection.FindOneAndReplace<Transaction>(
                t => t._id == transaction._id,
                transaction
            );
        }

        public void RollBackTransaction(Transaction transaction)
        {
            ProcessTransaction(transaction, TransactionProcessAction.RollBack);

            transaction.Status = TransactionStatus.RolledBack;

            _transactionsCollection.FindOneAndReplace<Transaction>(
                t => t._id == transaction._id,
                transaction
            );
        }

        private void ProcessTransaction(Transaction transaction, TransactionProcessAction action)
        {
            if (transaction._id == null || transaction._id == ObjectId.Empty)
                throw new ArgumentException("Only initialized transactions can be processed");

            foreach (var command in transaction.Commands)
            {
                var commandProcessor = _commandsProcessors.FirstOrDefault(p => p.CanProcess(command));

                if (commandProcessor == null)
                    throw new InvalidOperationException("Corresponding command processor was not initialized");

                switch (action)
                {
                    case TransactionProcessAction.Commit:
                        commandProcessor.Process(command, transaction);
                        break;
                    case TransactionProcessAction.RollBack:
                        commandProcessor.RollBack(command, transaction);
                        break;
                    default:
                        throw new ArgumentException("Argument must be provided", "TransactionProcessAction");
                }
            }
        }
    }

    internal enum TransactionProcessAction
    {
        Commit,
        RollBack
    }
}