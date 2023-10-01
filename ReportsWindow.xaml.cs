using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;

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
        List<EventHorizonLINQ> eventHorizonLINQ_LineItemsList;

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
            FlowDocument flowDoc;
            // Create the parent FlowDocument...
            flowDoc = new FlowDocument();

            flowDoc.ColumnWidth = 10000;

            Image i = new Image();
            i.Width = 189;
            i.Height = 34;
            i.Stretch = Stretch.Uniform;
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

            Table tableHeader = new Table();
            // ...and add it to the FlowDocument Blocks collection.
            flowDoc.Blocks.Add(tableHeader);

            // Set some global formatting properties for the table.
            tableHeader.CellSpacing = 10;
            tableHeader.Background = Brushes.White;

            List<GridLength> gridLengths = new List<GridLength> {
                new GridLength(60),
                new GridLength(60),
                new GridLength(50),
                new GridLength(260),
                new GridLength(60),
                new GridLength(60),
                new GridLength(60),
                new GridLength(80)
            };

            int numberOfColumns = 8;
            for (int x = 0; x < numberOfColumns; x++)
            {
                tableHeader.Columns.Add(new TableColumn());

                tableHeader.Columns[x].Width = gridLengths[x];
            }

            // Create and add an empty TableRowGroup to hold the table's Rows.
            tableHeader.RowGroups.Add(new TableRowGroup());

            // Add the first (title) row.
            tableHeader.RowGroups[0].Rows.Add(new TableRow());

            // Alias the current working row for easy reference.
            TableRow currentRowHeader = tableHeader.RowGroups[0].Rows[0];

            // Global formatting for the title row.
            currentRowHeader.Background = Brushes.Silver;
            currentRowHeader.FontSize = 1;
            currentRowHeader.FontWeight = System.Windows.FontWeights.Bold;

            // Add the second (header) row.
            tableHeader.RowGroups[0].Rows.Add(new TableRow());
            currentRowHeader = tableHeader.RowGroups[0].Rows[1];

            // Global formatting for the header row.
            currentRowHeader.FontSize = 12;
            currentRowHeader.FontWeight = FontWeights.Bold;
            tableHeader.Columns.Add(new TableColumn() { Width = new GridLength(100) }); // First column width

            // Add cells with content to the second row.
            Paragraph paragraphItemNumberHeader = new Paragraph(new Run("Item No."));
            paragraphItemNumberHeader.TextAlignment = TextAlignment.Center;
            currentRowHeader.Cells.Add(new TableCell(paragraphItemNumberHeader));

            Paragraph paragraphImageHeader = new Paragraph(new Run("Image"));
            paragraphImageHeader.TextAlignment = TextAlignment.Center;
            currentRowHeader.Cells.Add(new TableCell(paragraphImageHeader));

            Paragraph paragraphQtyHeader = new Paragraph(new Run("Qty"));
            paragraphQtyHeader.TextAlignment = TextAlignment.Center;
            currentRowHeader.Cells.Add(new TableCell(paragraphQtyHeader));

            Paragraph paragraphDescriptionHeader = new Paragraph(new Run("Description"));
            paragraphDescriptionHeader.TextAlignment = TextAlignment.Left;
            currentRowHeader.Cells.Add(new TableCell(paragraphDescriptionHeader));

            Paragraph paragraphUnitCostHeader = new Paragraph(new Run("Unit Cost"));
            paragraphUnitCostHeader.TextAlignment = TextAlignment.Right;
            currentRowHeader.Cells.Add(new TableCell(paragraphUnitCostHeader));

            Paragraph paragraphSubTotalHeader = new Paragraph(new Run("Sub Total"));
            paragraphSubTotalHeader.TextAlignment = TextAlignment.Right;
            currentRowHeader.Cells.Add(new TableCell(paragraphSubTotalHeader));

            Paragraph paragraphDiscountHeader = new Paragraph(new Run("Discount"));
            paragraphDiscountHeader.TextAlignment = TextAlignment.Right;
            currentRowHeader.Cells.Add(new TableCell(paragraphDiscountHeader));

            Paragraph paragraphTotalHeader = new Paragraph(new Run("Total"));
            paragraphTotalHeader.TextAlignment = TextAlignment.Right;
            currentRowHeader.Cells.Add(new TableCell(paragraphTotalHeader));

            // Add the second (header) row.
            tableHeader.RowGroups[0].Rows.Add(new TableRow());
            currentRowHeader = tableHeader.RowGroups[0].Rows[2];

            // Global formatting for the title row.
            currentRowHeader.Background = Brushes.Silver;
            currentRowHeader.FontSize = 1;
            currentRowHeader.FontWeight = System.Windows.FontWeights.Bold;

            Int32 ItemNumber = 0;
            Int32 grandTotalItems = 0;
            double grandTotalUnitCost = 0;
            double grandTotalTotal = 0;

            foreach (EventHorizonLINQ eventHorizonLINQ in eventHorizonLINQ_LineItemsList)
            {
                ItemNumber++;

                grandTotalItems += eventHorizonLINQ.Qty;

                grandTotalUnitCost += eventHorizonLINQ.UnitCost * eventHorizonLINQ.Qty;

                grandTotalTotal += (eventHorizonLINQ.UnitCost * eventHorizonLINQ.Qty) - (eventHorizonLINQ.UnitCost * eventHorizonLINQ.Qty) * eventHorizonLINQ.Discount / 100;


                Table table1 = new Table();

                flowDoc.Blocks.Add(table1);

                // Set some global formatting properties for the table.
                table1.CellSpacing = 10;
                table1.Background = Brushes.White;

                for (int x = 0; x < numberOfColumns; x++)
                {
                    table1.Columns.Add(new TableColumn());

                    table1.Columns[x].Width = gridLengths[x];
                }

                // Create and add an empty TableRowGroup to hold the table's Rows.
                table1.RowGroups.Add(new TableRowGroup());

                // Add the first (title) row.
                table1.RowGroups[0].Rows.Add(new TableRow());

                // Alias the current working row for easy reference.
                TableRow currentRow = table1.RowGroups[0].Rows[0];

                // Global formatting for the title row.
                //currentRow.Background = Brushes.Silver;
                //currentRow.FontSize = 18;
                //currentRow.FontWeight = System.Windows.FontWeights.Medium;

                // Add the second (header) row.
                table1.RowGroups[0].Rows.Add(new TableRow());
                currentRow = table1.RowGroups[0].Rows[1];

                // Global formatting for the header row.
                currentRow.FontSize = 12;
                currentRow.FontWeight = FontWeights.Bold;
                table1.Columns.Add(new TableColumn() { Width = new GridLength(100) }); // First column width

                // Create a paragraph and add it to the FlowDocument
                Paragraph paragraphImage = new Paragraph();
                flowDoc.Blocks.Add(paragraphImage);

                if (File.Exists(eventHorizonLINQ.PathFileName))
                {
                    // Create an InlineUIContainer to host an image
                    InlineUIContainer imageContainer = new InlineUIContainer();

                    Image image = new Image();
                    image.MaxWidth = 200;
                    image.MaxHeight = 100;
                    image.Stretch = Stretch.Uniform;
                    image.StretchDirection = StretchDirection.DownOnly;
                    image.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    image.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(new Uri(eventHorizonLINQ.PathFileName));

                    // Add the Image to the InlineUIContainer
                    imageContainer.Child = image;

                    // Add the InlineUIContainer to the paragraph
                    paragraphImage.Inlines.Add(imageContainer);
                    paragraphImage.TextAlignment = TextAlignment.Center;
                }

                // Add the third row.
                table1.RowGroups[0].Rows.Add(new TableRow());
                currentRow = table1.RowGroups[0].Rows[2];

                // Global formatting for the row.
                currentRow.FontSize = 12;
                currentRow.FontWeight = FontWeights.Normal;

                Paragraph paragraphItemNumber = new Paragraph(new Run(ItemNumber.ToString()));
                paragraphItemNumber.TextAlignment = TextAlignment.Center;
                currentRow.Cells.Add(new TableCell(paragraphItemNumber));

                currentRow.Cells.Add(new TableCell(paragraphImage));
                
                Paragraph paragraphQty = new Paragraph( new Run(eventHorizonLINQ.Qty.ToString()));
                paragraphQty.TextAlignment = TextAlignment.Center;
                currentRow.Cells.Add(new TableCell(paragraphQty));

                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(eventHorizonLINQ.Details.ToString()))));

                Paragraph paragraphUnitCost = new Paragraph(new Run(eventHorizonLINQ.UnitCost.ToString("C2", CultureInfo.CurrentCulture)));
                paragraphUnitCost.TextAlignment = TextAlignment.Right;
                currentRow.Cells.Add(new TableCell(paragraphUnitCost));

                double lineTotal = eventHorizonLINQ.UnitCost * eventHorizonLINQ.Qty;
                Paragraph paragraphLineTotal = new Paragraph(new Run(lineTotal.ToString("C2", CultureInfo.CurrentCulture)));
                paragraphLineTotal.TextAlignment = TextAlignment.Right;
                currentRow.Cells.Add(new TableCell(paragraphLineTotal));

                double discount = eventHorizonLINQ.Discount / 100;
                Paragraph paragraphDiscount = new Paragraph(new Run(discount.ToString("P", CultureInfo.InvariantCulture)));
                paragraphDiscount.TextAlignment = TextAlignment.Right;
                currentRow.Cells.Add(new TableCell(paragraphDiscount));

                double total = (eventHorizonLINQ.UnitCost * eventHorizonLINQ.Qty) - (eventHorizonLINQ.UnitCost * eventHorizonLINQ.Qty) * eventHorizonLINQ.Discount / 100;
                Paragraph paragraphTotal = new Paragraph(new Run(total.ToString("C2", CultureInfo.CurrentCulture)));
                paragraphTotal.TextAlignment = TextAlignment.Right;
                currentRow.Cells.Add(new TableCell(paragraphTotal));

                // Bold the first cell.
                currentRow.Cells[0].FontWeight = FontWeights.Bold;

                table1.RowGroups[0].Rows.Add(new TableRow());
                currentRow = table1.RowGroups[0].Rows[3];
            }

            Table tableFooter = new Table();
            // ...and add it to the FlowDocument Blocks collection.
            flowDoc.Blocks.Add(tableFooter);

            // Set some global formatting properties for the table.
            tableFooter.CellSpacing = 10;
            tableFooter.Background = Brushes.White;

            for (int x = 0; x < numberOfColumns; x++)
            {
                tableFooter.Columns.Add(new TableColumn());

                tableFooter.Columns[x].Width = gridLengths[x];
            }

            // Create and add an empty TableRowGroup to hold the table's Rows.
            tableFooter.RowGroups.Add(new TableRowGroup());

            // Add the first (title) row.
            tableFooter.RowGroups[0].Rows.Add(new TableRow());

            // Alias the current working row for easy reference.
            TableRow currentRowFooter = tableFooter.RowGroups[0].Rows[0];

            // Global formatting for the title row.
            currentRowFooter.Background = Brushes.Silver;
            currentRowFooter.FontSize = 1;
            currentRowFooter.FontWeight = System.Windows.FontWeights.Bold;

            // Add the second (header) row.
            tableFooter.RowGroups[0].Rows.Add(new TableRow());
            currentRowFooter = tableFooter.RowGroups[0].Rows[1];

            // Global formatting for the header row.
            currentRowFooter.FontSize = 12;
            currentRowFooter.FontWeight = FontWeights.Bold;
            //tableFooter.Columns.Add(new TableColumn() { Width = new GridLength(100) }); // First column width

            // Add cells with content to the second row.
            Paragraph paragraphItemNumbeFooter = new Paragraph(new Run("Lines"));
            paragraphItemNumbeFooter.TextAlignment = TextAlignment.Center;
            currentRowFooter.Cells.Add(new TableCell(paragraphItemNumbeFooter));

            Paragraph paragraphImageFooter = new Paragraph(new Run(""));
            paragraphImageFooter.TextAlignment = TextAlignment.Center;
            currentRowFooter.Cells.Add(new TableCell(paragraphImageFooter));

            Paragraph paragraphQtyFooter = new Paragraph(new Run("Qty"));
            paragraphQtyFooter.TextAlignment = TextAlignment.Center;
            currentRowFooter.Cells.Add(new TableCell(paragraphQtyFooter));

            Paragraph paragraphDescriptionFooter = new Paragraph(new Run("Description"));
            paragraphDescriptionFooter.TextAlignment = TextAlignment.Left;
            currentRowFooter.Cells.Add(new TableCell(paragraphDescriptionFooter));

            Paragraph paragraphUnitCostFooter = new Paragraph(new Run(""));
            paragraphUnitCostFooter.TextAlignment = TextAlignment.Right;
            currentRowFooter.Cells.Add(new TableCell(paragraphUnitCostFooter));

            Paragraph paragraphSubTotalFooter = new Paragraph(new Run("Sub Total"));
            paragraphSubTotalFooter.TextAlignment = TextAlignment.Right;
            currentRowFooter.Cells.Add(new TableCell(paragraphSubTotalFooter));

            Paragraph paragraphDiscountFooter = new Paragraph(new Run("Discount"));
            paragraphDiscountFooter.TextAlignment = TextAlignment.Right;
            currentRowFooter.Cells.Add(new TableCell(paragraphDiscountFooter));

            Paragraph paragraphTotalFooter = new Paragraph(new Run("Grand Total"));
            paragraphTotalFooter.TextAlignment = TextAlignment.Right;
            currentRowFooter.Cells.Add(new TableCell(paragraphTotalFooter));


            tableFooter.RowGroups[0].Rows.Add(new TableRow());
            currentRowFooter = tableFooter.RowGroups[0].Rows[2];

            // Global formatting for the title row.
            currentRowFooter.Background = Brushes.White;
            currentRowFooter.FontSize = 12;
            //currentRowFooter.FontWeight = System.Windows.FontWeights.Bold;

            Paragraph paragraphGrandItemNumber = new Paragraph(new Run(ItemNumber.ToString()));
            paragraphGrandItemNumber.TextAlignment = TextAlignment.Center;
            currentRowFooter.Cells.Add(new TableCell(paragraphGrandItemNumber));

            ////// Create a paragraph and add it to the FlowDocument
            //Paragraph paragraphGrandImageFooter = new Paragraph();
            //flowDoc.Blocks.Add(paragraphGrandImageFooter);

            //// Create an InlineUIContainer to host an image
            //InlineUIContainer imageContainerFooter = new InlineUIContainer();

            //Image imageFooter = new Image();
            //imageFooter.MaxWidth = 200;
            //imageFooter.MaxHeight = 100;
            //imageFooter.Stretch = Stretch.Uniform;
            //imageFooter.StretchDirection = StretchDirection.DownOnly;
            //imageFooter.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            //imageFooter.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            //imageFooter.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(new Uri(eventHorizonLINQ_MainEvent.PathFileName));

            //// Add the Image to the InlineUIContainer
            //imageContainerFooter.Child = imageFooter;

            //// Add the InlineUIContainer to the paragraph
            //paragraphGrandImageFooter.Inlines.Add(imageContainerFooter);
            //paragraphGrandImageFooter.TextAlignment = TextAlignment.Center;

            //currentRowFooter.Cells.Add(new TableCell(paragraphGrandImageFooter));

            Paragraph paragraphImageSpaceHolder = new Paragraph(new Run(""));
            paragraphImageSpaceHolder.TextAlignment = TextAlignment.Right;
            currentRowFooter.Cells.Add(new TableCell(paragraphImageSpaceHolder));

            Paragraph paragraphGrandQty = new Paragraph(new Run(grandTotalItems.ToString()));
            paragraphGrandQty.TextAlignment = TextAlignment.Center;
            currentRowFooter.Cells.Add(new TableCell(paragraphGrandQty));

            currentRowFooter.Cells.Add(new TableCell(new Paragraph(new Run(eventHorizonLINQ_MainEvent.Details))));

            Paragraph paragraphGrandUnitCost = new Paragraph(new Run(""));
            paragraphGrandUnitCost.TextAlignment = TextAlignment.Right;
            currentRowFooter.Cells.Add(new TableCell(paragraphGrandUnitCost));

            Paragraph paragraphGrandLineTotal = new Paragraph(new Run(grandTotalUnitCost.ToString("C2", CultureInfo.CurrentCulture)));
            paragraphGrandLineTotal.TextAlignment = TextAlignment.Right;
            currentRowFooter.Cells.Add(new TableCell(paragraphGrandLineTotal));

            double grandTotalDiscount = grandTotalUnitCost - grandTotalTotal;

            Paragraph paragraphGrandDiscount = new Paragraph(new Run(grandTotalDiscount.ToString("C2", CultureInfo.CurrentCulture)));
            paragraphGrandDiscount.TextAlignment = TextAlignment.Right;
            currentRowFooter.Cells.Add(new TableCell(paragraphGrandDiscount));

            Paragraph paragraphGrandTotal = new Paragraph(new Run(grandTotalTotal.ToString("C2", CultureInfo.CurrentCulture)));
            paragraphGrandTotal.TextAlignment = TextAlignment.Right;
            currentRowFooter.Cells.Add(new TableCell(paragraphGrandTotal));


            // Add the second (header) row.
            tableFooter.RowGroups[0].Rows.Add(new TableRow());
            currentRowFooter = tableFooter.RowGroups[0].Rows[3];

            // Global formatting for the title row.
            currentRowFooter.Background = Brushes.Silver;
            currentRowFooter.FontSize = 1;
            currentRowFooter.FontWeight = System.Windows.FontWeights.Bold;

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

        private void Reports_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (WindowState == WindowState.Maximized) WindowState = WindowState.Normal;
        }

        private void Print_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }
    }
}