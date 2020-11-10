using System;
using System.Runtime.InteropServices.ComTypes;

namespace StrategyPattern
{
    // Happy Hours - 10% upustu w godzinach od 9 do 15
    public class HappyHoursOrderCalculator
    {
        public decimal CalculateDiscount(Order order)
        {
            if (order.OrderDate.Hour >= 9 && order.OrderDate.Hour <= 15)
            {
                return order.Amount * 0.1m;
            }
            else
                return 0;
        }
    }


    public interface IDiscountStrategy
    {
        bool CanDiscount(Order order);
        decimal Discount(Order order);
    }

    public interface ICanDiscountStrategy
    {
        bool CanDiscount(Order order);
    }

    public interface ICalculateDiscountStrategy
    {
        decimal Discount(Order order);
    }


    public class HappyHoursCanDiscountStrategy : ICanDiscountStrategy
    {
        private readonly TimeSpan from;
        private readonly TimeSpan to;

        public HappyHoursCanDiscountStrategy(TimeSpan from, TimeSpan to)
        {
            this.from = from;
            this.to = to;
        }

        public bool CanDiscount(Order order)
        {
            return order.OrderDate.TimeOfDay >= from && order.OrderDate.TimeOfDay <= to;
        }
    }

    public class GenderCanDiscountStrategy : ICanDiscountStrategy
    {
        private readonly Gender gender;

        public GenderCanDiscountStrategy(Gender gender)
        {
            this.gender = gender;
        }

        public bool CanDiscount(Order order)
        {
            return order.Customer.Gender == gender;
        }
    }

    public class FixedDiscountStrategy : ICalculateDiscountStrategy
    {
        private readonly decimal fixedAmount;

        public FixedDiscountStrategy(decimal fixedAmount)
        {
            this.fixedAmount = fixedAmount;
        }

        public decimal Discount(Order order)
        {
            return fixedAmount;
        }
    }

    public class PercentageDiscountStrategy : ICalculateDiscountStrategy
    {
        private readonly decimal percentage;

        public PercentageDiscountStrategy(decimal percentage)
        {
            this.percentage = percentage;
        }

        public decimal Discount(Order order)
        {
            return order.Amount * percentage;
        }
    }

    public class HappyHoursFixedDiscountStrategy : IDiscountStrategy
    {
        private readonly TimeSpan from;
        private readonly TimeSpan to;
        private readonly decimal fixedAmount;

        public bool CanDiscount(Order order)
        {
            return order.OrderDate.TimeOfDay >= from && order.OrderDate.TimeOfDay <= to;
        }

        public decimal Discount(Order order)
        {
            return fixedAmount;
        }
    }

    public class HappyHoursPercentageDiscountStrategy : IDiscountStrategy
    {
        private readonly TimeSpan from;
        private readonly TimeSpan to;
        private readonly decimal percentage;

        public HappyHoursPercentageDiscountStrategy(TimeSpan from, TimeSpan to, decimal percentage)
        {
            this.from = from;
            this.to = to;
            this.percentage = percentage;
        }

        public bool CanDiscount(Order order)
        {
            return order.OrderDate.TimeOfDay >= from && order.OrderDate.TimeOfDay <= to;
        }

        public decimal Discount(Order order)
        {
            return order.Amount * percentage;
        }
    }


    public class GenderFixedDiscountStrategy : IDiscountStrategy
    {
        private readonly Gender gender;
        private readonly decimal fixedAmount;

        public GenderFixedDiscountStrategy(Gender gender, decimal fixedAmount)
        {
            this.gender = gender;
            this.fixedAmount = fixedAmount;
        }

        public bool CanDiscount(Order order)
        {
            return order.Customer.Gender == gender;
        }

        public decimal Discount(Order order)
        {
            return fixedAmount;
        }
    }


    public class GenderPercentageDiscountStrategy : IDiscountStrategy
    {
        private readonly Gender gender;
        private readonly decimal percentage;

        public GenderPercentageDiscountStrategy(Gender gender, decimal percentage)
        {
            this.gender = gender;
            this.percentage = percentage;
        }

        public bool CanDiscount(Order order)
        {
            return order.Customer.Gender == gender;
        }

        public decimal Discount(Order order)
        {
            return order.Amount * percentage;
        }
    }

    public class DiscountOrderCalculator
    {
        private readonly IDiscountStrategy discountStrategy;

        public DiscountOrderCalculator(IDiscountStrategy discountStrategy)
        {
            this.discountStrategy = discountStrategy;
        }

        public decimal CalculateDiscount(Order order)
        {
            // 1. Warunek
            if (discountStrategy.CanDiscount(order))
            {
                // 2. Zniżka
                return discountStrategy.Discount(order);
            }
            else
                return 0;
        }
    }

    public class DiscountOrderCalculator2
    {
        private readonly ICanDiscountStrategy canDiscountStrategy;
        private readonly ICalculateDiscountStrategy discountStrategy;

        public DiscountOrderCalculator2(ICanDiscountStrategy canDiscountStrategy, ICalculateDiscountStrategy discountStrategy)
        {
            this.canDiscountStrategy = canDiscountStrategy;
            this.discountStrategy = discountStrategy;
        }

        public decimal CalculateDiscount(Order order)
        {
            // 1. Warunek
            if (canDiscountStrategy.CanDiscount(order))
            {
                // 2. Zniżka
                return discountStrategy.Discount(order);
            }
            else
                return 0;
        }
    }
}
