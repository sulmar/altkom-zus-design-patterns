using System;

namespace TemplateMethodPattern
{
    // Happy Hours - 10% upustu w godzinach od 9 do 15
    public class HappyHoursOrderCalculator
    {
        private readonly TimeSpan from;
        private readonly TimeSpan to;
        private readonly decimal percentage;

        public HappyHoursOrderCalculator(TimeSpan from, TimeSpan to, decimal percentage)
        {
            this.from = from;
            this.to = to;
            this.percentage = percentage;
        }

        public decimal CalculateDiscount(Order order)
        {
            if (order.OrderDate.TimeOfDay >= from && order.OrderDate.TimeOfDay <= to)
            {
                return order.Amount * percentage;
            }
            else
                return 0;
        }
    }

    // Promocja - znizka dla kobiet 20%
    public class GenderOrderCalculator
    {
        private readonly Gender gender;
        private readonly decimal percentage;

        public GenderOrderCalculator(Gender gender, decimal percentage)
        {
            this.gender = gender;
            this.percentage = percentage;
        }

        public decimal CalculateDiscount(Order order)
        {
            if (order.Customer.Gender == gender)
            {
                return order.Amount * percentage;
            }
            else
                return 0;
        }
    }


    public abstract class DiscountOrderCalculator
    {
        public abstract bool CanDiscount(Order order);
        public abstract decimal Discount(Order order);

        public decimal CalculateDiscount(Order order)           // Template Method
        {
            // 1. Warunek (predykat)
            if (CanDiscount(order))
            {
                // 2. Zniżka
                return Discount(order);
            }
            else
                return 0;
        }
    }

    public class HappyHoursDiscountOrderCalculator : DiscountOrderCalculator
    {
        private readonly TimeSpan from;
        private readonly TimeSpan to;
        private readonly decimal percentage;

        public HappyHoursDiscountOrderCalculator(TimeSpan from, TimeSpan to, decimal percentage)
        {
            this.from = from;
            this.to = to;
            this.percentage = percentage;
        }

        public override bool CanDiscount(Order order)
        {
            return order.OrderDate.TimeOfDay >= from && order.OrderDate.TimeOfDay <= to;
        }

        public override decimal Discount(Order order)
        {
            return order.Amount * percentage;
        }
    }

    public class HappyHoursFixedDiscountOrderCalculator : DiscountOrderCalculator
    {
        private readonly TimeSpan from;
        private readonly TimeSpan to;
        private readonly decimal fixedAmount;

        public HappyHoursFixedDiscountOrderCalculator(TimeSpan from, TimeSpan to, decimal fixedAmount)
        {
            this.from = from;
            this.to = to;
            this.fixedAmount = fixedAmount;
        }

        public override bool CanDiscount(Order order)
        {
            return order.OrderDate.TimeOfDay >= from && order.OrderDate.TimeOfDay <= to;
        }

        public override decimal Discount(Order order)
        {
            return fixedAmount;
        }
    }

    public class GenderDiscountOrderCalculator : DiscountOrderCalculator
    {
        private readonly Gender gender;
        private readonly decimal percentage;

        public GenderDiscountOrderCalculator(Gender gender, decimal percentage)
        {
            this.gender = gender;
            this.percentage = percentage;
        }

        public override bool CanDiscount(Order order)
        {
            return order.Customer.Gender == gender;
        }

        public override decimal Discount(Order order)
        {
            return order.Amount * percentage;
        }
    }
}
