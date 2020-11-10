using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnitOfWork;
using System.Linq;

namespace UnitOfWork
{
    public class AbcUnitOfWork
    {
        public Guid SelectedCustomerId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Content { get; set; }
        public ICollection<Customer> Customers { get; set; }
    }

    public class Customer
    {
        public Guid Id { get; set; }
        public string Number { get; set; }

    }
}


namespace CommandPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Command Pattern!");

            Message message = new Message("555000123", "555888000", "Hello World!");

            if (message.CanPrint())
            {
                message.Print();
            }

            if (message.CanSend())
            {
                message.Send();
            }

            AbcUnitOfWork abcUnitOfWork = new AbcUnitOfWork();

            abcUnitOfWork.From = "555000123";
            abcUnitOfWork.To = "555888000";
            abcUnitOfWork.Content = "Hello World!";

            CommandExecutor commandExecutor = new CommandExecutor();

            // transakcja

            commandExecutor.Add(new GetCustomerCommand(abcUnitOfWork));
            commandExecutor.Add(new SendCommand(abcUnitOfWork));
            commandExecutor.Add(new PrintCommand(abcUnitOfWork));

            commandExecutor.Process();
           
        }
    }

    public class CommandExecutor
    {
        private readonly ICollection<ICommand> commands = new Collection<ICommand>();

        public void Add(ICommand command)
        {
            commands.Add(command);
        }

        public void Process()
        {
            foreach (ICommand command in commands)
            {
                if (command.CanExecute())
                {
                    command.Execute();
                }
            }
        }
    }

    public interface ICommand
    {
        void Execute();
        bool CanExecute();
    }

    public class GetCustomerCommand : CommandBase<AbcUnitOfWork>
    {
        public GetCustomerCommand(AbcUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override bool CanExecute()
        {
            return true;
        }

        public override void Execute()
        {
            Customer customer = unitOfWork.Customers.SingleOrDefault(c => c.Id == unitOfWork.SelectedCustomerId);

            unitOfWork.To = customer.Number;
        }
    }

    public class PrintCommand : CommandBase<AbcUnitOfWork>
    {
        public PrintCommand(AbcUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override bool CanExecute()
        {
            return !string.IsNullOrEmpty(unitOfWork.Content);
        }

        public override void Execute()
        {
            Console.WriteLine($"Print message from <{unitOfWork.From}> to <{unitOfWork.To}> {unitOfWork.Content}");
        }
    }



    public abstract class CommandBase<TUnitOfWork> : ICommand
    {
        protected readonly TUnitOfWork unitOfWork;

        protected CommandBase(TUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public abstract bool CanExecute();

        public abstract void Execute();
    }

    public class SendCommand : CommandBase<AbcUnitOfWork>, ICommand
    {
        public SendCommand(AbcUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override bool CanExecute()
        {
            return !(string.IsNullOrEmpty(unitOfWork.From) || string.IsNullOrEmpty(unitOfWork.To) || string.IsNullOrEmpty(unitOfWork.Content));
        }

        public override void Execute()
        {
            Console.WriteLine($"Send message from <{unitOfWork.From}> to <{unitOfWork.To}> {unitOfWork.Content}");
        }

    }



    #region Models

    public class Message
    {
        public Message(string from, string to, string content)
        {
            From = from;
            To = to;
            Content = content;
        }

        public string From { get; set; }
        public string To { get; set; }
        public string Content { get; set; }

     
        public void Send()
        {
            Console.WriteLine($"Send message from <{From}> to <{To}> {Content}");
        }

        public bool CanSend()
        {
            return !(string.IsNullOrEmpty(From) || string.IsNullOrEmpty(To) || string.IsNullOrEmpty(Content));
        }

        public void Print()
        {
            Console.WriteLine($"Print message from <{From}> to <{To}> {Content}");
        }

        public bool CanPrint()
        {
            return string.IsNullOrEmpty(Content);
        }



    }



    

    #endregion
}
