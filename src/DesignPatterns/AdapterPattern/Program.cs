using CrystalDecisions.CrystalReports;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace AdapterPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Adapter Pattern!");

            //MotorolaRadioTest();

            //HyteriaRadioTest();

            IRadioAdapter radioAdapter = new HyteraRadioAdapter2();

            radioAdapter.Send("Hello World!", 10);

            radioAdapter = new MotorolaRadioAdapter2("1234");

            radioAdapter.Send("Hello .NET", 25);

            IReportService reportService = new ReporingServicesService();

            reportService.Generate("raport1.rdlc", "raport1.pdf");


            reportService = new CrystalReportsService("user", "pass");
            reportService.Generate("raport1.rpt", "raport1.pdf");


            CrystalReportTest();


        }

        private static void CrystalReportTest()
        {
            ReportDocument rpt = new ReportDocument();
            rpt.Load("report1.rpt");

            ConnectionInfo connectInfo = new ConnectionInfo()
            {
                ServerName = "MyServer",
                DatabaseName = "MyDb",
                UserID = "myuser",
                Password = "mypassword"
            };

            foreach (Table table in rpt.Database.Tables)
            {
                table.LogOnInfo.ConnectionInfo = connectInfo;
                table.ApplyLogOnInfo(table.LogOnInfo);
            }

            rpt.ExportToDisk(ReportDocument.ExportFormatType.PortableDocFormat, "report1.pdf");
        }

        private static void MotorolaRadioTest()
        {
            MotorolaRadio radio = new MotorolaRadio();
            radio.PowerOn("1234");
            radio.SelectChannel(10);
            radio.Send("Hello World!");
            radio.PowerOff();
        }

        private static void HyteriaRadioTest()
        {
            HyteraRadio radio = new HyteraRadio();
            radio.Init();
            radio.SendMessage(10, "Hello World!");
            radio.Release();
        }
    }

    public interface IReportService
    {
        void Generate(string templateFilename, string outputFilename);
    }

    public class CrystalReportsService : IReportService
    {
        private readonly ReportDocument report;
        private readonly string username;
        private readonly string password;

        public CrystalReportsService(string username, string password)
        {
            report = new ReportDocument();
        }

        public void Generate(string templateFilename, string outputFilename)
        {
            report.Load(templateFilename);
            report.SetDatabaseLogon(username, password);
            report.ExportToDisk(ReportDocument.ExportFormatType.PortableDocFormat, outputFilename);
        }
    }

    public class ReporingServicesService : IReportService
    {
        private RdlcReportService reportService;

        public ReporingServicesService()
        {
            reportService = new RdlcReportService();
        }
        public void Generate(string templateFilename, string outputFilename)
        {
            reportService.Export(templateFilename, outputFilename);
        }
    }

    public class RdlcReportService 
    {
        public void Export(string template, string output)
        {
            Console.WriteLine($"Exporting {template} to {output}...");
        }
    }


    // Abstract adapter
    public interface IRadioAdapter
    {
        void Send(string message, byte channel);
    }

    // Wariant klasowy
    public class HyteraRadioAdapter2 : HyteraRadio, IRadioAdapter
    {
        public void Send(string message, byte channel)
        {
            base.Init();
            base.SendMessage(channel, message);
            base.Release();
        }
    }

    public class MotorolaRadioAdapter2 : MotorolaRadio, IRadioAdapter
    {
        private readonly string pincode;

        public MotorolaRadioAdapter2(string pincode)
            : base()
        {
            this.pincode = pincode;
        }

        public void Send(string message, byte channel)
        {
            base.PowerOn(pincode);
            base.SelectChannel(channel);
            base.Send(message);
            base.PowerOff();
        }
    }


    // Wariant obiektowy

    // Concrete adapter
    public class HyteraRadioAdapter : IRadioAdapter
    {
        // Adaptee
        private HyteraRadio radio;

        public HyteraRadioAdapter()
        {
            radio = new HyteraRadio();
        }

        public void Send(string message, byte channel)
        {
            radio.Init();
            radio.SendMessage(channel, message);
            radio.Release();
        }
    }

    // Concrete adapter
    public class MotorolaRadioAdapter : IRadioAdapter
    {
        // Adaptee
        private MotorolaRadio radio;

        private readonly string pincode;

        public MotorolaRadioAdapter(string pincode)
        {
            this.pincode = pincode;

            radio = new MotorolaRadio();
        }

        public void Send(string message, byte channel)
        {
            radio.PowerOn(pincode);
            radio.SelectChannel(channel);
            radio.Send(message);
            radio.PowerOff();
        }
    }
}
