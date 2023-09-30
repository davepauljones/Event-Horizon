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

            GenerateReport2();
        }

        private void GenerateReport2()
        {
            FlowDocument flowDoc;
            // Create the parent FlowDocument...
            flowDoc = new FlowDocument();

            flowDoc.ColumnWidth = 10000;

            Image i = new Image();
            i.Width = 138;
            i.Height = 38;
            i.Stretch = Stretch.Fill;
            i.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            i.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            i.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(new Uri("pack://application:,,/EventHorizonLogoNewSmall.png"));

            flowDoc.Blocks.Add(new BlockUIContainer(i));

            string TitleRun;
            string DescriptionRun;

            TitleRun = "Event Horizon - List for ID: " + eventHorizonLINQ_MainEvent.ID;
            DescriptionRun = eventHorizonLINQ_MainEvent.Details;

            Paragraph titlerun = new Paragraph(new Bold(new Run(TitleRun + " Report")));
            titlerun.FontSize = 30;
            flowDoc.Blocks.Add(titlerun);

            Paragraph descriptionrun = new Paragraph(new Run(DescriptionRun));
            flowDoc.Blocks.Add(descriptionrun);

            Paragraph dt = new Paragraph(new Run(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString()));
            flowDoc.Blocks.Add(dt);

            this.Title += " - " + TitleRun;

            Int32 ItemNumber = 0;

            foreach (EventHorizonLINQ eventHorizonLINQ in eventHorizonLINQ_LineItemsList)
            {
                ItemNumber++;

                Table table1;
            // Create the Table...
            table1 = new Table();
            // ...and add it to the FlowDocument Blocks collection.
            flowDoc.Blocks.Add(table1);

            // Set some global formatting properties for the table.
            table1.CellSpacing = 10;
            table1.Background = Brushes.White;

                // Create 6 columns and add them to the table's Columns collection.
                int numberOfColumns = 6;
                for (int x = 0; x < numberOfColumns; x++)
                {
                    table1.Columns.Add(new TableColumn());

                    // Set alternating background colors for the middle colums.
                    if (x % 2 == 0)
                        table1.Columns[x].Background = Brushes.Beige;
                    else
                        table1.Columns[x].Background = Brushes.LightSteelBlue;
                }

                // Create and add an empty TableRowGroup to hold the table's Rows.
                table1.RowGroups.Add(new TableRowGroup());

                // Add the first (title) row.
                table1.RowGroups[0].Rows.Add(new TableRow());

                // Alias the current working row for easy reference.
                TableRow currentRow = table1.RowGroups[0].Rows[0];

                // Global formatting for the title row.
                currentRow.Background = Brushes.Silver;
                currentRow.FontSize = 40;
                currentRow.FontWeight = System.Windows.FontWeights.Bold;

                //// Add the header row with content,
                //currentRow.Cells.Add(new TableCell(new Paragraph(new Run("2004 Sales Project"))));
                //// and set the row to span all 6 columns.
                //currentRow.Cells[0].ColumnSpan = 6;

            
                // Add the second (header) row.
                table1.RowGroups[0].Rows.Add(new TableRow());
                currentRow = table1.RowGroups[0].Rows[1];

                // Global formatting for the header row.
                currentRow.FontSize = 18;
                currentRow.FontWeight = FontWeights.Bold;

                // Add cells with content to the second row.
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Item No."))));
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Image"))));
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Quarter 2"))));
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Quarter 3"))));
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Quarter 4"))));
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run("TOTAL"))));

                // Create a paragraph and add it to the FlowDocument
                Paragraph paragraph = new Paragraph();
                flowDoc.Blocks.Add(paragraph);

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

                // Add the third row.
                table1.RowGroups[0].Rows.Add(new TableRow());
                currentRow = table1.RowGroups[0].Rows[2];

                // Global formatting for the row.
                currentRow.FontSize = 12;
                currentRow.FontWeight = FontWeights.Normal;

                // Add cells with content to the third row.
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(ItemNumber.ToString()))));
                currentRow.Cells.Add(new TableCell(paragraph));
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run("$55,000"))));
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run("$60,000"))));
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run("$65,000"))));
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run("$230,000"))));

                // Bold the first cell.
                currentRow.Cells[0].FontWeight = FontWeights.Bold;

                table1.RowGroups[0].Rows.Add(new TableRow());
                currentRow = table1.RowGroups[0].Rows[3];

                // Global formatting for the footer row.
                currentRow.Background = Brushes.LightGray;
                currentRow.FontSize = 18;
                currentRow.FontWeight = System.Windows.FontWeights.Normal;

                //// Add the header row with content,
                //currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Projected 2004 Revenue: $810,000"))));
                //// and set the row to span all 6 columns.
                //currentRow.Cells[0].ColumnSpan = 6;

            }

            var package = Package.Open(new MemoryStream(), FileMode.Create, FileAccess.ReadWrite);
            var packUri = new Uri("pack://temp.xps");
            PackageStore.RemovePackage(packUri);
            PackageStore.AddPackage(packUri, package);

            xpsDocument = new XpsDocument(package, CompressionOption.SuperFast, packUri.ToString());

            XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(xpsDocument);

            writer.Write(((IDocumentPaginatorSource)flowDoc).DocumentPaginator);

            Document = xpsDocument.GetFixedDocumentSequence();

            xpsDocument.Close();

            PreviewD.Document = Document;
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

            Int32 ItemNumber = 0;

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

                //doc.Blocks.Add(paragraph);

                Table table = new Table();
                for (int iii = 0; iii < 7; iii++)
                {
                    table.Columns.Add(new TableColumn());
                }

                TableRow tableRow = new TableRow();
                //tableRow.Background = Brushes.Pink;
                tableRow.FontSize = 40;
                tableRow.FontWeight = FontWeights.Bold;

                tableRow.Cells.Add(new TableCell(new Paragraph(new Run(ItemNumber.ToString()))));
                //tableRow.Cells.Add(new TableCell(new Paragraph(new Run("Blah"))));
                tableRow.Cells.Add(new TableCell(paragraph));
                tableRow.Cells.Add(new TableCell(new Paragraph(new Run(eventHorizonLINQ.Details))));
                tableRow.Cells.Add(new TableCell(new Paragraph(new Run(eventHorizonLINQ.UnitCost.ToString()))));
                //tableRow.Cells[0].BorderThickness = new Thickness(3);
                //tableRow.Cells[0].ColumnSpan = 2;
                //
                //tableRow.Background = System.Windows.Media.Brushes.Navy;

                tableRow.Foreground = System.Windows.Media.Brushes.White;

                var tableRowGroup = new TableRowGroup();
                tableRowGroup.Rows.Add(tableRow);
                table.RowGroups.Add(tableRowGroup);
                doc.Blocks.Add(table);

                // Add more text if needed
                //Run moreTextRun = new Run(eventHorizonLINQ.Details);
                //moreTextRun.BaselineAlignment = BaselineAlignment.Center;
                //paragraph.Inlines.Add(moreTextRun);

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