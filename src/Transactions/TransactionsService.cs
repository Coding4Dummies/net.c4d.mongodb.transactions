using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using Net.C4D.Mongodb.Transactions.Mongo;
using Net.C4D.Mongodb.Transactions.Commands;
using Net.C4D.Mongodb.Transactions.Ioc;

namespace Net.C4D.Mongodb.Transactions.Transactions
{
    public class TransactionsService
    {
        private readonly MongoRepository<Transaction> _transactionsRepository;

        private readonly List<ICommandProcessor> _commandsProcessors;

        public TransactionsService()
        {
            _transactionsRepository = ServicesContainer.GetService<MongoRepository<Transaction>>();
            _commandsProcessors = ServicesContainer.GetService<List<ICommandProcessor>>();
        }

        public void CreateTransaction(Transaction transaction)
        {
            _transactionsRepository.Insert(transaction);

            ProcessTransaction(transaction);
        }

        public void UpdateTransaction(Transaction transaction)
        {
            _transactionsRepository.Save(transaction);
        }

        public void ProcessTransaction(Transaction transaction)
        {
            try
            {
                foreach (var command in transaction.Commands)
                {
                    var commandProcessor = _commandsProcessors.FirstOrDefault(p => p.CanProcess(command));

                    if (commandProcessor == null)
                        throw new InvalidOperationException("Corresponding processor was not found");

                    commandProcessor.Process(command);
                }
                transaction.Status = TransactionStatus.Completed;
                UpdateTransaction(transaction);
            }
            catch
            {
                transaction.Status = TransactionStatus.Error;
                UpdateTransaction(transaction);
            }
        }
    }
}