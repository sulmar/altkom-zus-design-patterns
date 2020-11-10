using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace VisitorPattern
{
    public class VisitorFactory
    {
        public static IVisitor Create(string name)
        {
            switch(name)
            {
                case "markdown": return new MarkdownVisitor();
                case "html": return new HtmlVisitor("Hello");

                default: throw new NotImplementedException();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Visitor Pattern!");

            Form form = Get();

            IVisitor visitor = VisitorFactory.Create("html");

            form.Accept(visitor);

            string html = visitor.Output;

            System.IO.File.WriteAllText("index.html", html);
        }

        public static Form Get()
        {
            Form form = new Form
            {
                Name = "/forms/customers",
                Title = "Design Patterns",

                Body = new Collection<ControlBase>
                {
                    new LabelControl { Caption = "Person", Name = "lblName" },
                    new TextBoxControl { Caption = "FirstName", Name = "txtFirstName", Value = "John"},
                    new CheckBoxControl { Caption = "IsAdult", Name = "chkIsAdult", Value = true },
                    new ButtonControl {  Caption = "Submit", Name = "btnSubmit", ImageSource = "save.png" },
                }

            };

            return form;
        }
    }

    #region Models

    //public class FormVisitor
    //{
    //    public string Process(Form form, IVisitor visitor)
    //    {
    //        foreach (var control in form.Body)
    //        {
    //            control.Accept(visitor);
    //        }

    //        return visitor.Output;
    //    }
    //}

    public class Form
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public ICollection<ControlBase> Body { get; set; }

        public void Accept(IVisitor visitor)
        {
            foreach (var control in Body)
            {
                control.Accept(visitor);
            }
        }
    }

    public class Control
    {
        public string Name { get; set; }
        public string Caption { get; set; }
        public ControlType Type { get; set; }
        public string Value { get; set; }
        public string ImageSource { get; set; }
    }

    public enum ControlType
    {
        Label,
        TextBox,
        Checkbox,
        Button
    }

    public abstract class ControlBase
    {
        public string Name { get; set; }
        public string Caption { get; set; }
        public abstract void Accept(IVisitor visitor);
    }

    public class LabelControl : ControlBase
    {
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class TextBoxControl : ControlBase
    {
        public string Value { get; set; }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class CheckBoxControl : ControlBase
    {
        public bool Value { get; set; }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class ButtonControl : ControlBase
    {
        public string ImageSource { get; set; }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

  

    #endregion

    // Abstract visitor
    public interface IVisitor
    {
        void Visit(LabelControl control);
        void Visit(TextBoxControl control);
        void Visit(CheckBoxControl control);
        void Visit(ButtonControl control);

        string Output { get; }
    }

    public class MarkdownVisitor : IVisitor
    {
        private readonly StringBuilder builder = new StringBuilder();

        public string Output => builder.ToString();

        public void Visit(LabelControl control)
        {
            builder.AppendLine($"**{control.Caption}**");
        }

        public void Visit(TextBoxControl control)
        {
            builder.AppendLine($"**{control.Caption}** _{control.Value}_");
        }

        public void Visit(CheckBoxControl control)
        {
            builder.AppendLine($"**{control.Caption}** {control.Value}");
        }

        public void Visit(ButtonControl control)
        {
            builder.AppendLine($"_{control.ImageSource}_");
        }
    }

    // Concrete visitor
    public class HtmlVisitor : IVisitor
    {
        private readonly StringBuilder builder = new StringBuilder();

        public HtmlVisitor(string title)
        {
            Start(title);
        }

        private void Start(string title)
        {
            builder.AppendLine("<html>");

            builder.AppendLine($"<title>{title}</title>");

            builder.AppendLine("<body>");
        }

        public string Output
        {
            get
            {
                builder.AppendLine("</body>");
                builder.AppendLine("</html>");

                return builder.ToString();
            }
        }

        public void Visit(LabelControl control)
        {
            builder.AppendLine($"<span>{control.Caption}</span>");
        }

        public void Visit(TextBoxControl control)
        {
            builder.AppendLine($"<span>{control.Caption}</span><input type='text' value='{control.Value}'></input>");
        }

        public void Visit(CheckBoxControl control)
        {
            builder.AppendLine($"<span>{control.Caption}</span><input type='checkbox' value='{control.Value}'></input>");
        }

        public void Visit(ButtonControl control)
        {
            builder.AppendLine($"<button><img src='{control.ImageSource}'/>{control.Caption}</button>");
        }
    }
}
