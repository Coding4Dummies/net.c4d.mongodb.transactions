using System;
using Net.C4D.Mongodb.Transactions.Commands;
using MongoDB.Driver;
using Net.C4D.Mongodb.Transactions.Ioc;
using Net.C4D.Mongodb.Transactions.Mongo;

namespace Net.C4D.Mongodb.Transactions.Products
{
    public class UpdateProductQuantityCommand : ICommand
    {
        public Guid ProductId { get; set; }

        public CommandOperator Operator { get; set; }

        public int Value { get; set; }

        public Guid TransactionId { get; set; }
    }

    public class UpdateProductQuantityCommandProcessor : ICommandProcessor
    {
        private readonly MongoRepository<Product> _productsRepository;

        public UpdateProductQuantityCommandProcessor()
        {
            _productsRepository = ServicesContainer.GetService<MongoRepository<Product>>();
        }

        public bool CanProcess(ICommand command)
        {
            return command is UpdateProductQuantityCommand;
        }

        public void Process(ICommand command)
        {
            var processedCommand = command as UpdateProductQuantityCommand;

            if (processedCommand == null)
                throw new InvalidOperationException("Unsupported command passed to processor");

            var collection = _productsRepository.MongoCollection;
            var filterDefinition = Builders<Product>.Filter.Eq("ProductId", processedCommand.ProductId);

            UpdateDefinition<Product> updateDefinition;

            switch (processedCommand.Operator)
            {
                case CommandOperator.Add:
                    updateDefinition = Builders<Product>.Update.Inc("InStockAmmount", processedCommand.Value);
                    break;
                case CommandOperator.Substract:
                    updateDefinition = Builders<Product>.Update.Inc("InStockAmmount", -processedCommand.Value);
                    break;
                default:
                    throw new InvalidOperationException("Unsupported operator passed to the command");
            }

            updateDefinition.Push("Transactions", processedCommand.TransactionId);

            collection.FindOneAndUpdate(filterDefinition, updateDefinition);
        }
    }
}