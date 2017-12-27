using System;
using Net.C4D.Mongodb.Transactions.Commands;
using MongoDB.Driver;
using Net.C4D.Mongodb.Transactions.Ioc;
using Net.C4D.Mongodb.Transactions.Mongo;
using MongoDB.Bson;
using System.Collections.Generic;
using Net.C4D.Mongodb.Transactions.Transactions;

namespace Net.C4D.Mongodb.Transactions.Products
{
    public class UpdateProductQuantityCommand : ICommand
    {
        public Product Product { get; set; }

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
            var validCommandOperators = CommandOperator.Add | CommandOperator.SetValue;

            if (processedCommand == null || !validCommandOperators.HasFlag(processedCommand.Operator))
                throw new InvalidOperationException("Unsupported command or command-operator passed to processor");

            var collection = _productsRepository.MongoCollection;

            var filterObject = new BsonDocument("ProductId", processedCommand.Product.ProductId);

            var commandUpdateOperator = processedCommand.Operator == CommandOperator.Add ? "$inc" : "$set";

            var updateDefinitions = new BsonDocument(new Dictionary<string, object>{
                { commandUpdateOperator, new BsonDocument("InStockAmmount", processedCommand.Value)},
                {"$push", new BsonDocument(
                    new Dictionary<string,object>{
                        {"Transactions", new ExecutedTransaction(processedCommand.TransactionId, DateTime.Now).ToBsonDocument()}
                    })
                }
            });

            var updateObject = new BsonDocument(updateDefinitions);

            collection.FindOneAndUpdate(filterObject, updateObject);
        }
    }
}