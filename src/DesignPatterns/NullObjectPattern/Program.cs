using System;

namespace NullObjectPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Null Object Pattern!");

            IProductRepository productRepository = new FakeProductRepository();

            ProductBase product = productRepository.Get(1);

            // Problem: Zawsze musimy sprawdzać czy obiekt nie jest pusty (null).
            product.RateId(3);

        }
    }

    public interface IProductRepository
    {
        ProductBase Get(int id);
    }

    public class FakeProductRepository : IProductRepository
    {
        public ProductBase Get(int id)
        {
            return new NullProduct();
        }
    }

    public abstract class ProductBase
    {
        public abstract void RateId(int rate);
    }

    public class Product : ProductBase
    {
        private int rate;

        public override void RateId(int rate)
        {
            this.rate = rate;
        }
    }

    public class NullProduct : ProductBase
    {
        public override void RateId(int rate)
        {
            // nic nie rób
        }
    }
}
