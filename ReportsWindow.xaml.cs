using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using System.Printing;
using System.IO;
using System.IO.Packaging;

namespace The_Oracle
{
    /// <summary>
    /// Interaction logic for ReportsWindow.xaml
    /// </summary>
    public partial class ReportsWindow : Window
    {
        XpsDocument xpsDocument;
        FixedDocumentSequence Document { get; set; }

        EventHorizonLINQ eventHorizonLINQ_MainEvent;
        List<EventHorizonLINQ> eventHorizonLINQ_LineItemsList = new List<EventHorizonLINQ>();

        public ReportsWindow(EventHorizonLINQ eventHorizonLINQ_MainEvent, List<EventHorizonLINQ> eventHorizonLINQ_LineItemsList)
        {
            InitializeComponent();

            this.eventHorizonLINQ_MainEvent = eventHorizonLINQ_MainEvent;
            this.eventHorizonLINQ_LineItemsList = eventHorizonLINQ_LineItemsList;

            Init();
        }

        private void Init()
        {
            this.Owner = Application.Current.MainWindow;

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
            i.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(new Uri("pack://application:,,/EventHorizonLogoNewSmall.png"));

            doc.Blocks.Add(new BlockUIContainer(i));

            string TitleRun;
            string DescriptionRun;

            TitleRun = "Event Horizon - List for ID: " + eventHorizonLINQ_MainEvent.ID;
            DescriptionRun = eventHorizonLINQ_MainEvent.Details;

            Paragraph titlerun = new Paragraph(new Bold(new Run(TitleRun + " Report")));
            titlerun.FontSize = 30;
            doc.Blocks.Add(titlerun);

            Paragraph descriptionrun = new Paragraph(new Run(DescriptionRun));
            doc.Blocks.Add(descriptionrun);

            Paragraph dt = new Paragraph(new Run(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString()));
            doc.Blocks.Add(dt);

            this.Title += " - " + TitleRun;

            foreach (EventHorizonLINQ eventHorizonLINQ in eventHorizonLINQ_LineItemsList)
            {
                // Create a paragraph and add it to the FlowDocument
                Paragraph paragraph = new Paragraph();
                doc.Blocks.Add(paragraph);

                // Create an InlineUIContainer to host an image
                InlineUIContainer imageContainer = new InlineUIContainer();

                Image image = new Image();
                image.MaxWidth = 200;
                image.MaxHeight = 100;
                image.Stretch = Stretch.Uniform;
                image.StretchDirection = StretchDirection.DownOnly;
                image.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                image.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(new Uri(eventHorizonLINQ.PathFileName));

                // Add the Image to the InlineUIContainer
                imageContainer.Child = image;
                
                // Add the InlineUIContainer to the paragraph
                paragraph.Inlines.Add(imageContainer);

                doc.Blocks.Add(paragraph);

                // Add more text if needed
                Run moreTextRun = new Run(eventHorizonLINQ.Details);
                paragraph.Inlines.Add(moreTextRun);

                //doc.Blocks.Add(new Paragraph(new Run(eventHorizonLINQ.Details)));

                //doc.Blocks.Add(new InlineUIContainer(ii));
            }

            var package = Package.Open(new MemoryStream(), FileMode.Create, FileAccess.ReadWrite);
            var packUri = new Uri("pack://temp.xps");
            PackageStore.RemovePackage(packUri);
            PackageStore.AddPackage(packUri, package);

            xpsDocument = new XpsDocument(package, CompressionOption.SuperFast, packUri.ToString());

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