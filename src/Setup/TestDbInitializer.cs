

using System;
using Net.C4D.Mongodb.Transactions.Ioc;
using Net.C4D.Mongodb.Transactions.Orders;
using Net.C4D.Mongodb.Transactions.Products;
using Net.C4D.Mongodb.Transactions.Transactions;
using Net.C4D.Mongodb.Transactions.Mongo;

namespace Net.C4D.Mongodb.Transactions.Setup
{
    public class TestDbInitializer
    {
        private readonly MongoRepository<Product> _productsRepository;
        private readonly MongoRepository<Order> _ordersRepository;
        private readonly MongoRepository<Transaction> _transactionsRepository;

        public TestDbInitializer()
        {
            _productsRepository = ServicesContainer.GetService<MongoRepository<Product>>();
            _ordersRepository = ServicesContainer.GetService<MongoRepository<Order>>();
            _transactionsRepository = ServicesContainer.GetService<MongoRepository<Transaction>>();
        }

        public void InitTestDb()
        {
            ResetAllCollections();
            InitDemoProducts();
        }

        private void ResetAllCollections()
        {
            _productsRepository.DeleteBulk(_productsRepository.GetList(p => true));
            _ordersRepository.DeleteBulk(_ordersRepository.GetList(p => true));
            _transactionsRepository.DeleteBulk(_transactionsRepository.GetList(p => true));
        }

        void InitDemoProducts(int numberOfProducts = 5, int initialQuantity = 10)
        {
            var productsRepository = ServicesContainer.GetService<MongoRepository<Product>>();
            var allExistingProducts = productsRepository.GetList(p => true);
            productsRepository.DeleteBulk(allExistingProducts);

            for (int i = 0; i < numberOfProducts; i++)
            {
                var product = new Product
                {
                    Name = string.Format("Sample Product #{0}", i + 1),
                    Description = "Great product - Good value for money",
                    InStockAmmount = initialQuantity
                };

                productsRepository.Insert(product);
            }
        }
    }
}