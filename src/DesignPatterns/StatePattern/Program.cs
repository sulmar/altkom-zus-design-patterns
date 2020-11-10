using Stateless;
using System;
using System.Timers;

namespace StatePattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello State Pattern!");

            // OrderTest();

            Lamp lamp = new Lamp();


            Console.WriteLine(lamp.Graph);

            Console.WriteLine(lamp.State);

            lamp.Push();
            Console.WriteLine(lamp.State);

            //lamp.Push();
            //Console.WriteLine(lamp.State);

            //lamp.Photo();
            //Console.WriteLine(lamp.State);

            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(6));

            Console.WriteLine(lamp.State);


        }

        private static void OrderTest()
        {
            Order order = Order.Create();

            order.Completion();

            if (order.Status == OrderStatus.Completion)
            {
                order.Status = OrderStatus.Sent;
                Console.WriteLine("Your order was sent.");
            }

            order.Cancel();
        }
    }

    #region Models

    public class Order
    {
        public Order(string orderNumber)
        {
            Status = OrderStatus.Created;

            OrderNumber = orderNumber;
            OrderDate = DateTime.Now;
         
        }

        public DateTime OrderDate { get; set; }

        public string OrderNumber { get; set; }

        public OrderStatus Status { get; set; }

        private static int indexer;

        public static Order Create()
        {
            Order order = new Order($"Order #{indexer++}");

            if (order.Status == OrderStatus.Created)
            {
                Console.WriteLine("Thank you for your order");
            }

            return order;
        }

        public void Completion()
        {
            if (Status == OrderStatus.Created)
            {
                this.Status = OrderStatus.Completion;

                Console.WriteLine("Your order is in progress");
            }
        }

        public void Cancel()
        {
            if (this.Status == OrderStatus.Created || this.Status == OrderStatus.Completion)
            {
                this.Status = OrderStatus.Canceled;

                Console.WriteLine("Your order was cancelled.");
            }
        }

    }

    public enum OrderStatus
    {
        Created,
        Completion,
        Sent,
        Canceled,
        Done
    }


    // dotnet add package Stateless

    public class Lamp
    {
        public LampState State => machine.State;

        private readonly StateMachine<LampState, LampTrigger> machine;

        private Timer timer;

        public string Graph => Stateless.Graph.UmlDotGraph.Format(machine.GetInfo());

        public Action OnLampOn { get; set; }

        public Lamp()
        {
            machine = new StateMachine<LampState, LampTrigger>(LampState.Off);

            machine.Configure(LampState.Off)
                .Permit(LampTrigger.Push, LampState.On)
                .PermitIf(LampTrigger.Photo, LampState.On, () => DateTime.Now.TimeOfDay > TimeSpan.Parse("13:00"))
                .Ignore(LampTrigger.Timer);

            machine.Configure(LampState.On)
                .OnEntry(OnLampOn)
                .OnEntry(() => timer.Start(), "Start timer")
                .OnEntry(()=> timer.Start(), "Start timer")
                .OnExit(() => timer.Stop(), "Stop timer")
                .Permit(LampTrigger.Push, LampState.Off)
                .Permit(LampTrigger.Photo, LampState.Off)
                .Permit(LampTrigger.Timer, LampState.Off);

            machine.OnTransitioned(t => Console.WriteLine($"{t.Source} -> {t.Destination}"));

            timer = new Timer(TimeSpan.FromSeconds(5).TotalMilliseconds);

            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            machine.Fire(LampTrigger.Timer);
        }

        public void Push()
        {
            machine.Fire(LampTrigger.Push);
        }

        public void Photo()
        {
            machine.Fire(LampTrigger.Photo);
        }

        


      


    }

    public enum LampState
    {
        On,
        Off
    }

    public enum LampTrigger
    {
        Push,
        Photo,
        Timer
    }

    #endregion

}
