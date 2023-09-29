using FontAwesome.WPF;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using System.Printing;

namespace The_Oracle
{
    /// <summary>
    /// Interaction logic for Reports.xaml
    /// </summary>
    public partial class Reports : Window
    {
        byte ReportType = ReportTypes.RollCall;
        byte Destination = 0;

        public struct ReportTypes
        {
            public const byte RollCall = 0;
            public const byte AccessGroups = 1;
            public const byte TimeZones = 2;
            public const byte TimeSheets = 3;
        }
        public struct ReportTypeStrings
        {
            public const string RollCall = "Roll Call";
            public const string AccessGroups = "Access Groups";
            public const string TimeZones = "Time Zones";
            public const string TimeSheets = "Time Sheets";
        }

        XpsDocument xpsDocument;
        FixedDocumentSequence Document { get; set; }

        List<SelectionIdString> ListOfReport = new List<SelectionIdString>();
        public Reports(byte Destination, byte ReportType = ReportTypes.RollCall)
        {
            InitializeComponent();

            this.Destination = Destination;
            this.ReportType = ReportType;

            Init();
        }

        private void Init()
        {
            this.Owner = Application.Current.MainWindow;

            string DestinationString = string.Empty;

            switch (Destination)
            {
                
            }

            string ReportTypeString = string.Empty;

            switch (ReportType)
            {
                case Reports.ReportTypes.RollCall:
                    ReportTypeString = Reports.ReportTypeStrings.RollCall;
                    break;
                case Reports.ReportTypes.AccessGroups:
                    ReportTypeString = Reports.ReportTypeStrings.AccessGroups;
                    break;
                case Reports.ReportTypes.TimeZones:
                    ReportTypeString = Reports.ReportTypeStrings.TimeZones;
                    break;
                case Reports.ReportTypes.TimeSheets:
                    ReportTypeString = Reports.ReportTypeStrings.TimeSheets;
                    break;
            }

            GenerateReport();
        }

        private void GenerateReport()
        {
            FlowDocument doc = new FlowDocument();

            Image i = new Image();
            i.Width = 138;
            i.Height = 38;
            i.Stretch = Stretch.Fill;
            i.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            i.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            i.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(new Uri("pack://application:,,,/Images/upastextonlylogo.png"));

            doc.Blocks.Add(new BlockUIContainer(i));

            string TitleRun = string.Empty;
            string DescriptionRun = string.Empty;

            switch (ReportType)
            {
                case ReportTypes.RollCall:
                    TitleRun = "Roll Call";
                    DescriptionRun = "Roll Call is a list of users that have used UPAS today.";
                    //ArduDatabaseModule.UsersToList(ListOfReport);
                    break;
            }

            Paragraph titlerun = new Paragraph(new Bold(new Run(TitleRun + " Report")));
            titlerun.FontSize = 30;
            doc.Blocks.Add(titlerun);

            Paragraph descriptionrun = new Paragraph(new Run(DescriptionRun));
            doc.Blocks.Add(descriptionrun);

            Paragraph dt = new Paragraph(new Run(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString()));
            doc.Blocks.Add(dt);

            this.Title += " - " + TitleRun;

            List fdList = new List();

            foreach (SelectionIdString ss in ListOfReport)
            {
                fdList.ListItems.Add(new ListItem(new Paragraph(new Run(ss.Name))));
            }

            doc.Blocks.Add(fdList);

            var package = Package.Open( new MemoryStream(), FileMode.Create, FileAccess.ReadWrite );
            var packUri = new Uri( "pack://temp.xps" );
            PackageStore.RemovePackage( packUri );
            PackageStore.AddPackage( packUri, package );
 
            xpsDocument = new XpsDocument( package, CompressionOption.SuperFast, packUri.ToString() );

            XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(xpsDocument);

            writer.Write(((IDocumentPaginatorSource)doc).DocumentPaginator);

            Document = xpsDocument.GetFixedDocumentSequence();

            xpsDocument.Close();

            PreviewD.Document = Document;
        }
        private void Reports_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (WindowState == WindowState.Maximized) WindowState = WindowState.Normal;
        }

        private void Print_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }       
    }
}

