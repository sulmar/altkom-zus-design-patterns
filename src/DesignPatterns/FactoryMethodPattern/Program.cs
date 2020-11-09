using System;

namespace FactoryMethodPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Factory Method Pattern!");

            VisitCalculateAmountTest();

        }

        private static void VisitCalculateAmountTest()
        {
            Console.WriteLine("Podaj rodzaj wizyty: (N)FZ (P)rywatna (F)irma");

            string input = Console.ReadLine();

            VisitFactory visitFactory = new VisitFactory();

            Visit visit = visitFactory.Create(input);

            decimal totalAmount = visit.CalculateAmount();

            Console.BackgroundColor = ColorFactory.Create(totalAmount);

            Console.WriteLine($"Total amount {totalAmount:C2}");

            Console.ResetColor();
        }
    }


    public class ColorFactory
    {
        public static ConsoleColor Create(decimal amount)
        {
            if (amount < 100)
                return ConsoleColor.Red;
            else
                if (amount >= 100 && amount < 500)
                return ConsoleColor.Yellow;
            else
                return ConsoleColor.Green;
                
        }
    }

    public class Factory        
    {
        public static T Create<T>()
            where T : new()
        {
            return new T();
        }
    }

    public class VisitFactory
    {
        public Visit Create(string input)
        {
            switch (input)
            {
                case "N": return new NFZVisit(); 
                case "P": return new PrivateVisit(); 
                case "F": return new PackageVisit();

                default: throw new NotSupportedException(input);
            }
        }
    }

    #region Models

    public class NFZVisit : Visit
    {
        public override decimal CalculateAmount()
        {
            return 0;
        }
    }

    public class PrivateVisit : Visit
    {

    }

    public class PackageVisit : Visit
    {
        public override decimal CalculateAmount()
        {
            return base.CalculateAmount() * 0.9m;
        }
    }

    public abstract class Visit
    {
        public DateTime VisitDate { get; set; }
        public TimeSpan Duration { get; set; }
        public decimal Amount { get; set; }

        public Visit()
        {
            Amount = 100;
            Duration = TimeSpan.FromMinutes(15);
        }

        public virtual decimal CalculateAmount()
        {
            return (decimal)Duration.TotalHours * Amount;
        }
    }

    #endregion
}
