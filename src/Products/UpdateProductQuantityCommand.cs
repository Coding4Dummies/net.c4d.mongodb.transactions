using System;
using Net.C4D.Mongodb.Transactions.Commands;
using MongoDB.Driver;
using Net.C4D.Mongodb.Transactions.Ioc;
using Net.C4D.Mongodb.Transactions.Mongo;
using MongoDB.Bson;
using System.Collections.Generic;
using Net.C4D.Mongodb.Transactions.Transactions;
using System.Linq;

namespace Net.C4D.Mongodb.Transactions.Products
{
    public class UpdateProductQuantityCommand : ICommand
    {
        public Product Product { get; set; }

        public CommandOperator Operator { get; set; }

        public int Value { get; set; }
    }

    public class UpdateProductQuantityCommandProcessor : ICommandProcessor
    {
        private readonly IMongoCollection<Product> _productsCollection;

        public UpdateProductQuantityCommandProcessor()
        {
            _productsCollection = ServicesContainer.GetService<IMongoCollection<Product>>();
        }

        public bool CanProcess(ICommand command)
        {
            return command is UpdateProductQuantityCommand;
        }

        public void Process(ICommand command, Transaction transaction)
        {
            var processedCommand = ValidateCommand(command);

            var updateDefinitions = new BsonDocument(new Dictionary<string, object>{
                { "$inc", new BsonDocument("InStockAmmount", processedCommand.Value)},
                { "$push", new BsonDocument("Transactions", transaction.TransactionId)}
            });

            var updatedProduct = _productsCollection.FindOneAndUpdate(
                o => o.ProductId == processedCommand.Product.ProductId,
                updateDefinitions);
        }

        public void RollBack(ICommand command, Transaction transaction)
        {
            var processedCommand = ValidateCommand(command);

            var filterObject = new BsonDocument("ProductId", processedCommand.Product.ProductId);

            var savedProduct = _productsCollection.Find(filterObject).FirstOrDefault();

            // If the product is not found in the collection
            if (savedProduct == null || savedProduct.Transactions == null || !savedProduct.Transactions.Any())
                return;

            // If the product wasn't affected by the current transaction
            if (!savedProduct.Transactions.Contains(transaction.TransactionId))
                return;

            var updateDefinitions = new BsonDocument(new Dictionary<string, object>{
                { "$inc", new BsonDocument("InStockAmmount", - processedCommand.Value)},
                { "$pop", new BsonDocument("Transactions", transaction.TransactionId)}
            });

            var updatedProduct = _productsCollection.FindOneAndUpdate(filterObject, updateDefinitions);
        }

        private UpdateProductQuantityCommand ValidateCommand(ICommand command)
        {
            var validatedCommand = command as UpdateProductQuantityCommand;

            // Only update operations are allowed, since "replace" operations can't be rolled back securely
            if (validatedCommand == null || validatedCommand.Operator != CommandOperator.Add)
                throw new InvalidOperationException("Unsupported command or command-operator passed to processor");

            return validatedCommand;
        }
    }
}