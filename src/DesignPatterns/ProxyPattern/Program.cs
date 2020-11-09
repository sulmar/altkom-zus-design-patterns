using System;

namespace ProxyPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Proxy Pattern!");
            // SaveProductTest();
            SaveProxyProductTest();

        }

        private static void SaveProductTest()
        {
            ProductsDbContext context = new ProductsDbContext();

            Product product = new Product(1, "Design Patterns w C#", 150m);

            context.Add(product);

            product.UnitPrice = 99m;

            context.MarkAsChanged();

            context.SaveChanges();
        }

        private static void SaveProxyProductTest()
        {
            ProductsDbContext context = new ProductsDbContext();

            ProxyProduct product = new ProxyProduct(1, "Design Patterns w C#", 150m);

            context.Add(product);

            product.UnitPrice = 99m;

            if (product.Changed)
            {
                context.SaveChanges();
            }
        }
    }

    #region Models

    public class ProxyProduct : Product
    {
        private bool changed;

        public bool Changed => changed;

        public ProxyProduct(int id, string name, decimal unitPrice) : base(id, name, unitPrice)
        {
        }

        public override decimal UnitPrice
        {
            get
            {
                return base.UnitPrice;
            }

            set
            {
                base.UnitPrice = value;

                changed = true;

            }
        }
    }

    public class Product
    {
        public Product(int id, string name, decimal unitPrice)
        {
            Id = id;
            Name = name;
            UnitPrice = unitPrice;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual decimal UnitPrice { get; set; }
    }

    public class ProductsDbContext
    {
        private Product product;
        private bool changed;

        public void Add(Product product)
        {
            this.product = product;
        }

        public Product Get()
        {
            return product;
        }

        public void SaveChanges()
        {
            if (changed)
            {
                Console.WriteLine($"UPDATE dbo.Products SET UnitPrice = {product.UnitPrice} WHERE ProductId = {product.Id}" );
            }
        }

        public void MarkAsChanged()
        {
            changed = true;
        }
    }

    #endregion
}
