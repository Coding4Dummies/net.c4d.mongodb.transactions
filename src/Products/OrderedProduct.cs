using System;

namespace Net.C4D.Mongodb.Transactions.Products
{
    public class OrderedProduct
    {
        private readonly Product _product;
        private readonly int _quantity;

        public OrderedProduct(Product product, int quantity)
        {
            _product = product;
            _quantity = quantity;
        }
        public Product Product { get { return _product; } }

        public int Quantity { get { return _quantity; } }
    }
}