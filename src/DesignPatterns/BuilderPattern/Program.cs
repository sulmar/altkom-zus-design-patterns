using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace BuilderPattern
{
    public interface ISalesReportBuilder
    {
        void AddHeader(string title);
        void AddSectionByGender();
        void AddSectionByProduct();
        SalesReport Build();
    }

    public class MySalesReportBuilder : ISalesReportBuilder
    {
        private string title;
        private bool hasSectionByGender;
        private bool hasSectionByProduct;

        private IEnumerable<Order> orders;

        public MySalesReportBuilder(IEnumerable<Order> orders)
        {
            this.orders = orders;
        }

        public void AddHeader(string title)
        {
            this.title = title;
        }

        public void AddSectionByGender()
        {
            hasSectionByGender = true;
        }

        public void AddSectionByProduct()
        {
            hasSectionByProduct = true;
        }

        public SalesReport Build()
        {
            SalesReport salesReport = new SalesReport();

            // Header
            if (!string.IsNullOrEmpty(title))
            {
                salesReport.Title = title;
                salesReport.CreateDate = DateTime.Now;
                salesReport.TotalSalesAmount = orders.Sum(s => s.Amount);
            }

            // Section By Gender
            if (hasSectionByGender)
            {
                salesReport.GenderDetails = orders
                 .GroupBy(o => o.Customer.Gender)
                 .Select(g => new GenderReportDetail(
                             g.Key,
                             g.Sum(x => x.Details.Sum(d => d.Quantity)),
                             g.Sum(x => x.Details.Sum(d => d.LineTotal))));
            }

            // Section By Product
            if (hasSectionByProduct)
            {
                salesReport.ProductDetails = orders
               .SelectMany(o => o.Details)
               .GroupBy(o => o.Product)
               .Select(g => new ProductReportDetail(g.Key, g.Sum(p => p.Quantity), g.Sum(p => p.LineTotal)));
            }

            return salesReport;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Builder Pattern!");

            //PhoneTest();

            // SalesReportTest();
            FluentPhone
                .Hangup()
                .From("555444333")                
                .To("555888111")
                .To("555999999")
                .WithSubject("Wzorce projektowe w C#")
                .Call();                   
        }

        private static void SalesReportTest()
        {
            FakeOrdersService ordersService = new FakeOrdersService();
            IEnumerable<Order> orders = ordersService.Get();

            ISalesReportBuilder salesReportBuilder = new MySalesReportBuilder(orders);
            salesReportBuilder.AddHeader("Raport sprzedaży");
            salesReportBuilder.AddSectionByGender();
            salesReportBuilder.AddSectionByProduct();

            SalesReport salesReport = salesReportBuilder.Build();

            Console.WriteLine(salesReport);

        }

        // Fluent API (deklaratywne)
        //private static void FluentPhoneTest()
        //{
        //    FluentPhone
        //        .Hangup()
        //        .HighPriorytet
        //        .From("555999123")
        //        .To("555000321")
        //        .WithSubject(".NET Design Patterns")
        //        .Call();    // Build
        //}


        // imperatywny
        private static void PhoneTest()
        {
            Phone phone = new Phone();
            phone.Call("555999123", "555000321", ".NET Design Patterns");
        }

       
    }

    public class FakeOrdersService
    {
        private readonly IList<Product> products;
        private readonly IList<Customer> customers;

        public FakeOrdersService()
            : this(CreateProducts(), CreateCustomers())
        {

        }

        public FakeOrdersService(IList<Product> products, IList<Customer> customers)
        {
            this.products = products;
            this.customers = customers;
        }

        private static IList<Customer> CreateCustomers()
        {
            return new List<Customer>
            {
                 new Customer("Anna", "Kowalska"),
                 new Customer("Jan", "Nowak"),
                 new Customer("John", "Smith"),
            };

        }

        private static IList<Product> CreateProducts()
        {
            return new List<Product>
            {
                new Product(1, "Książka C#", unitPrice: 100m),
                new Product(2, "Książka Praktyczne Wzorce projektowe w C#", unitPrice: 150m),
                new Product(3, "Zakładka do książki", unitPrice: 10m),
            };
        }

        public IEnumerable<Order> Get()
        {
            Order order1 = new Order(DateTime.Parse("2020-06-12 14:59"), customers[0]);
            order1.AddDetail(products[0], 2);
            order1.AddDetail(products[1], 2);
            order1.AddDetail(products[2], 3);

            yield return order1;

            Order order2 = new Order(DateTime.Parse("2020-06-12 14:59"), customers[1]);
            order2.AddDetail(products[0], 2);
            order2.AddDetail(products[1], 4);

            yield return order2;

            Order order3 = new Order(DateTime.Parse("2020-06-12 14:59"), customers[2]);
            order2.AddDetail(products[0], 2);
            order2.AddDetail(products[2], 5);

            yield return order3;


        }
    }

    #region Models

    // Fluent API (deklaratywne)
    //private static void FluentPhoneTest()
    //{
    //    FluentPhone
    //        .Hangup()
    //        .HighPriorytet
    //        .From("555999123")
    //        .To("555000321")
    //        .WithSubject(".NET Design Patterns")
    //        .Call();    // Build
    //}

    public class FluentPhone
    {
        private string from;
        private ICollection<string> tos;
        private string subject;

        protected FluentPhone()
        {
            tos = new Collection<string>();
        }

        public static FluentPhone Hangup()
        {
            return new FluentPhone();
        }

        public void Hangdown() 
        { 
        }

        public FluentPhone From(string number)
        {
            this.from = number;
            return this;
        }

        public FluentPhone To(string number)
        {
            this.tos.Add(number);
            return this;
        }

        public FluentPhone WithSubject(string subject)
        {
            this.subject = subject;
            return this;
        }

        public void Call()
        {
            Call(from, tos, subject);           
        }

        private void Call(string from, string to, string subject)
        {
            Console.WriteLine($"Calling from {from} to {to} with subject {subject}");
        }

        private void Call(string from, string to)
        {
            Console.WriteLine($"Calling from {from} to {to}");
        }

        private void Call(string from, IEnumerable<string> tos, string subject)
        {
            foreach (var to in tos)
            {
                if (!string.IsNullOrEmpty(subject))
                {
                    Call(from, to, subject);
                }
                else
                {
                    Call(from, to);
                }
            }
        }
    }

    public class Phone
    {
        public void Call(string from, string to, string subject)
        {
            Console.WriteLine($"Calling from {from} to {to} with subject {subject}");
        }

        public void Call(string from, string to)
        {
            Console.WriteLine($"Calling from {from} to {to}");
        }

        public void Call(string from, IEnumerable<string> tos, string subject)
        {
            foreach (var to in tos)
            {
                Call(from, to, subject);
            }
        }
    }

    #endregion

    public class SalesReport
    {
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }
        public decimal TotalSalesAmount { get; set; }

        public IEnumerable<ProductReportDetail> ProductDetails { get; set; }
        public IEnumerable<GenderReportDetail> GenderDetails { get; set; }

        

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("------------------------------");

            stringBuilder.AppendLine($"{Title} {CreateDate}");

            stringBuilder.AppendLine($"Total Sales Amount: {TotalSalesAmount:c2}");

            stringBuilder.AppendLine("------------------------------");

            stringBuilder.AppendLine("Total By Products:");
            
            foreach (var detail in ProductDetails)
            {
                stringBuilder.AppendLine( $"- {detail.Product.Name} {detail.Quantity} {detail.TotalAmount:c2}");
            }

            stringBuilder.AppendLine("Total By Gender:");

            foreach (var detail in GenderDetails)
            {
                stringBuilder.AppendLine($"- {detail.Gender} {detail.Quantity} {detail.TotalAmount:c2}");
            }

            return stringBuilder.ToString();
        }
    }

    public class ProductReportDetail
    {
        public ProductReportDetail(Product product, int quantity, decimal totalAmount)
        {
            Product = product;
            Quantity = quantity;
            TotalAmount = totalAmount;
        }

        public Product Product { get; set; }
        public decimal TotalAmount { get; set; }
        public int Quantity { get; set; }
    }

    public class GenderReportDetail
    {
        public GenderReportDetail(Gender gender, int quantity, decimal totalAmount)
        {
            Gender = gender;
            Quantity = quantity;
            TotalAmount = totalAmount;
        }

        public Gender Gender { get; set; }
        public decimal TotalAmount { get; set; }
        public int Quantity { get; set; }
    }


    public class Order
    {
        public DateTime OrderDate { get; set; }
        public Customer Customer { get; set; }
        public decimal Amount => Details.Sum(p => p.LineTotal);

        public ICollection<OrderDetail> Details = new Collection<OrderDetail>();

        public void AddDetail(Product product, int quantity = 1)
        {
            OrderDetail detail = new OrderDetail(product, quantity);

            this.Details.Add(detail);
        }

        public Order(DateTime orderDate, Customer customer)
        {
            OrderDate = orderDate;
            Customer = customer;
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
        public decimal UnitPrice { get; set; }
    }

    public class OrderDetail
    {
        public OrderDetail(Product product, int quantity = 1)
        {
            Product = product;
            Quantity = quantity;

            UnitPrice = product.UnitPrice;
        }

        public int Id { get; set; }
        public Product Product { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal LineTotal => UnitPrice * Quantity;
    }

    public class Customer
    {
        public Customer(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;

            if (firstName.EndsWith("a"))
            {
                Gender = Gender.Female;
            }
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }

    }

    public enum Gender
    {
        Male,
        Female
    }

}