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
using System.Windows.Shapes;

namespace Event_Horizon
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
        int helpReportToView;

        public ReportsWindow(EventHorizonLINQ eventHorizonLINQ_MainEvent, List<EventHorizonLINQ> eventHorizonLINQ_LineItemsList, int helpReportToView = Helps.None)
        {
            InitializeComponent();

            this.eventHorizonLINQ_MainEvent = eventHorizonLINQ_MainEvent;
            this.eventHorizonLINQ_LineItemsList = eventHorizonLINQ_LineItemsList;
            this.helpReportToView = helpReportToView;

            Init();
        }

        private void Init()
        {
            this.Owner = Application.Current.MainWindow;

            switch (helpReportToView)
            {
                case Helps.None:
                    if (eventHorizonLINQ_MainEvent != null && eventHorizonLINQ_LineItemsList != null)
                        GenerateReport();
                    break;
                case Helps.EventStatus:
                    GenerateHelpReport_EventStatus();
                    break;
                case Helps.EventFunctionKeys:
                    GenerateHelpReport_EventFunctionKeys();
                    break;
                case Helps.FooBar:
                    GenerateHelpReport_EventStatus();
                    break;
            }
        }

        private void GenerateHelpReport_EventFunctionKeys()
        {
            FlowDocument flowDoc;
            // Create the parent FlowDocument...
            flowDoc = new FlowDocument();

            flowDoc.ColumnWidth = 10000;

            Image i = new Image();
            i.Width = 425;
            i.Height = 51;
            i.Stretch = Stretch.Uniform;
            i.Margin = new Thickness(50, 0, 0, 0);
            i.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            i.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            i.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(new Uri("pack://application:,,/Images/EventHorizonLogoHLNNN.png"));

            flowDoc.Blocks.Add(new BlockUIContainer(i));

            BlockUIContainer buic = new BlockUIContainer();
            Grid g1 = new Grid { Margin = new Thickness(5) };
            Grid g2 = new Grid { Margin = new Thickness(5) };
            Grid g3 = new Grid { Margin = new Thickness(5) };
            Grid g4 = new Grid { Margin = new Thickness(5) };
            Grid g5 = new Grid { Margin = new Thickness(5) };

            StackPanel sp = new StackPanel { Margin = new Thickness(50, 0, 0, 0) };

            Label helpHeadingLabel = new Label { Content = "Keyboard", Margin = new Thickness(5, 10, 0, 0), FontWeight = FontWeights.Bold, FontSize = 22, HorizontalContentAlignment = HorizontalAlignment.Left };
            sp.Children.Add(helpHeadingLabel);

            Label statusHeadingLabel = new Label { Content = "Example Function keys", Margin = new Thickness(5, 3, 0, 0), FontWeight = FontWeights.Normal };
            sp.Children.Add(statusHeadingLabel);

            StackPanel sp1 = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 20, 0, 0) };
            
            g1.Children.Add(FunctionKeyManager.CreateHelpFunctionKey("F1", FontAwesome.WPF.FontAwesomeIcon.Pencil, "Memo", Colors.Crimson));
            sp1.Children.Add(g1);

            g2.Children.Add(FunctionKeyManager.CreateHelpFunctionKey("F2", FontAwesome.WPF.FontAwesomeIcon.Road, "Road Map", Colors.LightSteelBlue));
            sp1.Children.Add(g2);

            g3.Children.Add(FunctionKeyManager.CreateHelpFunctionKey("F3", FontAwesome.WPF.FontAwesomeIcon.Bug, "Bug Fix", Colors.Firebrick));
            sp1.Children.Add(g3);

            Label spacerLabel = new Label { Content = "....... ", Margin = new Thickness(5, 3, 0, 0), FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Bottom };
            sp1.Children.Add(spacerLabel);

            g4.Children.Add(FunctionKeyManager.CreateHelpFunctionKey("F11", FontAwesome.WPF.FontAwesomeIcon.Gift, "Product", Colors.DodgerBlue));
            sp1.Children.Add(g4);

            g5.Children.Add(FunctionKeyManager.CreateHelpFunctionKey("F12", FontAwesome.WPF.FontAwesomeIcon.Dollar, "Marketing", Colors.Green));
            sp1.Children.Add(g5);

            sp.Children.Add(sp1);

            TextBlock introductionTextBlock = new TextBlock { Text = "Event Horizon uses the function keys and other keys to facilitate in creating, searching and filtering events.", Margin = new Thickness(10, 20, 50, 10), TextWrapping = TextWrapping.Wrap };
            sp.Children.Add(introductionTextBlock);

            Grid g6 = new Grid { Margin = new Thickness(5,20,0,0), HorizontalAlignment = HorizontalAlignment.Left };
            g6.Children.Add(FunctionKeyManager.CreateHelpFunctionKey("F11", FontAwesome.WPF.FontAwesomeIcon.Gift, "Product", Colors.DodgerBlue));
            sp.Children.Add(g6);

            TextBlock paragraph1TextBlock = new TextBlock { Text = "To create a new Product event [ LEFT-CLICK ] or press the equivalent function key.", Margin = new Thickness(10, 10, 50, 10), TextWrapping = TextWrapping.Wrap };
            sp.Children.Add(paragraph1TextBlock);

            Grid g7 = new Grid { Margin = new Thickness(5, 20, 0, 0), HorizontalAlignment = HorizontalAlignment.Left };
            g7.Children.Add(FunctionKeyManager.CreateHelpFunctionKey("F3", FontAwesome.WPF.FontAwesomeIcon.Bug, "Bug Fix", Colors.Firebrick));
            sp.Children.Add(g7);

            TextBlock paragraph2TextBlock = new TextBlock { Text = "To filter to a Bug Fix event [ RIGHT-CLICK ] or select event type Bug Fix from the drop down combo box.", Margin = new Thickness(10, 10, 50, 10), TextWrapping = TextWrapping.Wrap };
            sp.Children.Add(paragraph2TextBlock);

            StackPanel comboStackPanel = new StackPanel { Margin = new Thickness(5, 0, 0, 0) };
            comboStackPanel.Children.Add(FunctionKeyManager.CreateEventTypeComboBox("Event Type", new EventType { ID = 0, Icon = FontAwesome.WPF.FontAwesomeIcon.Bug, Color = Colors.Firebrick, Name = "Bug Fix" } ));
            sp.Children.Add(comboStackPanel);

            TextBlock paragraph3TextBlock = new TextBlock { Text = "To clear the filter, click the [ ALL BUTTON ]", Margin = new Thickness(10, 20, 50, 0), TextWrapping = TextWrapping.Wrap };
            sp.Children.Add(paragraph3TextBlock);

            StackPanel allButtonStackPanel = new StackPanel { Margin = new Thickness(0, 0, 0, 0), HorizontalAlignment = HorizontalAlignment.Left };
            allButtonStackPanel.Children.Add(FunctionKeyManager.CreateButton("ALL"));
            sp.Children.Add(allButtonStackPanel);

            TextBlock paragraph5TextBlock = new TextBlock { Text = "Event Horizon has a search input box, that is case sensitive, [ CLICK INTO ] the search box, start typing a few characters and press return.\n\nThe event list will filter on any matching events that have the entered text within there respective details fields.", Margin = new Thickness(10, 20, 50, 0), TextWrapping = TextWrapping.Wrap };
            sp.Children.Add(paragraph5TextBlock);

            StackPanel searchTextBoxStackPanel = new StackPanel { Margin = new Thickness(0, 0, 0, 0), HorizontalAlignment = HorizontalAlignment.Left };
            searchTextBoxStackPanel.Children.Add(FunctionKeyManager.CreateSearchTextBox("Search Details", "Bug"));
            sp.Children.Add(searchTextBoxStackPanel);

            TextBlock paragraph4TextBlock = new TextBlock { Text = "To clear the search filter, click the [ CLEAR BUTTON ]", Margin = new Thickness(10, 20, 50, 0), TextWrapping = TextWrapping.Wrap };
            sp.Children.Add(paragraph4TextBlock);

            StackPanel clearButtonStackPanel = new StackPanel { Margin = new Thickness(0, 0, 0, 0), HorizontalAlignment = HorizontalAlignment.Left };
            clearButtonStackPanel.Children.Add(FunctionKeyManager.CreateButton("CLEAR"));
            sp.Children.Add(clearButtonStackPanel);

            buic.Child = sp;
            flowDoc.Blocks.Add(buic);

            
            BlockUIContainer buicPage2 = new BlockUIContainer();
            StackPanel spPage2 = new StackPanel { Margin = new Thickness(50, 0, 0, 0) };

            TextBlock paragraph6TextBlock = new TextBlock { Text = "The delete key is used to delete an event, in order to delete an event, you must be the originator of the event and the event must not contain any notes or replies.\n\nThe prefered way is not to delete an event, but to set its status to archeived instead, see status help for more info.", Margin = new Thickness(10, 20, 50, 0), TextWrapping = TextWrapping.Wrap };
            spPage2.Children.Add(paragraph6TextBlock);

            StackPanel deleteKeyStackPanel = new StackPanel { Margin = new Thickness(0, 10, 0, 0), HorizontalAlignment = HorizontalAlignment.Left };
            deleteKeyStackPanel.Children.Add(FunctionKeyManager.CreateButton("Delete", 70, 60));
            spPage2.Children.Add(deleteKeyStackPanel);

            TextBlock paragraph7TextBlock = new TextBlock { Text = "To delete an event, highlight an event and press [ DELETE KEY ]", Margin = new Thickness(10, 20, 50, 0), TextWrapping = TextWrapping.Wrap };
            spPage2.Children.Add(paragraph7TextBlock);

            buicPage2.Child = spPage2;
            flowDoc.Blocks.Add(buicPage2);


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

        private void GenerateHelpReport_EventStatus()
        {
            FlowDocument flowDoc;
            // Create the parent FlowDocument...
            flowDoc = new FlowDocument();

            flowDoc.ColumnWidth = 10000;

            Image i = new Image();
            i.Width = 425;
            i.Height = 51;
            i.Stretch = Stretch.Uniform;
            i.Margin = new Thickness(25, 0, 0, 0);
            i.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            i.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            i.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(new Uri("pack://application:,,/Images/EventHorizonLogoHLNNN.png"));

            flowDoc.Blocks.Add(new BlockUIContainer(i));

            BlockUIContainer buic = new BlockUIContainer();
            Grid g1 = new Grid { Margin = new Thickness(5) };
            Grid g2 = new Grid { Margin = new Thickness(5) };
            Grid g3 = new Grid { Margin = new Thickness(5) };
            Grid g4 = new Grid { Margin = new Thickness(5) };
            Grid g5 = new Grid { Margin = new Thickness(5) };

            StackPanel sp = new StackPanel { Margin = new Thickness(20, 0, 0, 0) };

            Label helpHeadingLabel = new Label { Content = "Event Status Help", Margin = new Thickness(5, 20, 0, 0), FontWeight = FontWeights.Bold, FontSize = 22, HorizontalContentAlignment = HorizontalAlignment.Left };
            sp.Children.Add(helpHeadingLabel);

            TextBlock statusIntroductionTextBlock = new TextBlock { Text = "Event Horizon keeps track of events by assigning a Status to the event during the various stages of the events life span.\nThis acts a visual aid for all users, but especially useful as feedback for the origin user, the origin user can quickly determine whether the target user has responded to the notification and or read the event.\nStatus 'Active, Notified & Read' are automatic, Status 'Active, Notified, Read & Archived' is manual.\nThe origin user who created the event, can at any time, override the event's Status by manually setting the events Status, ie. When a target user has not given a response to the event, the origin user can change the event's Status back to 'Active', this will result in the target user getting a new notification, as though the event was just created and the cycle starts over.", Margin = new Thickness(10, 10, 50, 10), TextWrapping = TextWrapping.Wrap };
            sp.Children.Add(statusIntroductionTextBlock);

            Label statusHeadingLabel = new Label { Content = "Status Legend", Margin = new Thickness(5, 3, 0, 0), FontWeight = FontWeights.Bold };
            sp.Children.Add(statusHeadingLabel);

            StackPanel sp1 = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(40, 20, 0, 0) };
            g1.Children.Add(StatusIcons.GetStatus(Statuses.Inactive));
            sp1.Children.Add(g1);
            Label l1 = new Label { Content = "Inactive", Margin = new Thickness(10,3,0,0) };
            sp1.Children.Add(l1);
            sp.Children.Add(sp1);

            StackPanel sp2 = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(40, 0, 0, 0) };
            g2.Children.Add(StatusIcons.GetStatus(Statuses.Active));
            sp2.Children.Add(g2);
            Label l2 = new Label { Content = "Active", Margin = new Thickness(10, 3, 0, 0) };
            sp2.Children.Add(l2);
            sp.Children.Add(sp2);

            StackPanel sp3 = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(40, 0, 0, 0) };
            g3.Children.Add(StatusIcons.GetStatus(Statuses.ActiveNotified));
            sp3.Children.Add(g3);
            Label l3 = new Label { Content = "Active & Notified", Margin = new Thickness(10, 3, 0, 0) };
            sp3.Children.Add(l3);
            sp.Children.Add(sp3);

            StackPanel sp4 = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(40, 0, 0, 0) };
            g4.Children.Add(StatusIcons.GetStatus(Statuses.ActiveNotifiedRead));
            sp4.Children.Add(g4);
            Label l4 = new Label { Content = "Active, Notified & Read", Margin = new Thickness(10, 3, 0, 0) };
            sp4.Children.Add(l4);
            sp.Children.Add(sp4);

            StackPanel sp5 = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(40, 0, 0, 0) };
            g5.Children.Add(StatusIcons.GetStatus(Statuses.ActiveNotifiedReadArchived));
            sp5.Children.Add(g5);
            Label l5 = new Label { Content = "Active, Notified, Read & Archived", Margin = new Thickness(10, 3, 0, 0) };
            sp5.Children.Add(l5);
            sp.Children.Add(sp5);

            TextBlock activeTextBlock = new TextBlock { Text = "When a new event is created, the event's Status is set to Active.", Margin = new Thickness(50, 20, 50, 10), TextWrapping = TextWrapping.Wrap };
            sp.Children.Add(activeTextBlock);

            Grid g6 = new Grid { Margin = new Thickness(5) };
            StackPanel sp6 = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(40, 0, 0, 0) };
            g6.Children.Add(StatusIcons.GetStatus(Statuses.Active));
            sp6.Children.Add(g6);
            Label l6 = new Label { Content = "Active", Margin = new Thickness(10, 3, 0, 0) };
            sp6.Children.Add(l6);
            sp.Children.Add(sp6);

            TextBlock activeNotifiedTextBlock = new TextBlock { Text = "When a new event is created & the target user has acknowleged the notification, the event's Status is set to Active & Notified.", Margin = new Thickness(50, 20, 50, 10), TextWrapping = TextWrapping.Wrap };
            sp.Children.Add(activeNotifiedTextBlock);

            Grid g7 = new Grid { Margin = new Thickness(5) };
            StackPanel sp7 = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(40, 0, 0, 0) };
            g7.Children.Add(StatusIcons.GetStatus(Statuses.ActiveNotified));
            sp7.Children.Add(g7);
            Label l7 = new Label { Content = "Active & Notified", Margin = new Thickness(10, 3, 0, 0) };
            sp7.Children.Add(l7);
            sp.Children.Add(sp7);

            TextBlock activeNotifiedReadTextBlock = new TextBlock { Text = "When a new event is created & the target user has acknowleged the notification & the target user has viewed the event for longer than 3 seconds, then closed, the event's Status is set to Active, Notified & Read.", Margin = new Thickness(50, 20, 50, 10), TextWrapping = TextWrapping.Wrap };
            sp.Children.Add(activeNotifiedReadTextBlock);

            Grid g8 = new Grid { Margin = new Thickness(5) };
            StackPanel sp8 = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(40, 0, 0, 0) };
            g8.Children.Add(StatusIcons.GetStatus(Statuses.ActiveNotifiedRead));
            sp8.Children.Add(g8);
            Label l8 = new Label { Content = "Active, Notified & Read", Margin = new Thickness(10, 3, 0, 0) };
            sp8.Children.Add(l8);
            sp.Children.Add(sp8);

            TextBlock activeNotifiedReadArchivedTextBlock = new TextBlock { Text = "When an event is finished with, you can set its Status to Active, Notified, Read & Archived, this will prevent the event from showing up in the 'Reminders' display mode, but is available to view as a log, 'By New' display mode.", Margin = new Thickness(50, 20, 50, 10), TextWrapping = TextWrapping.Wrap };
            sp.Children.Add(activeNotifiedReadArchivedTextBlock);

            Grid g9 = new Grid { Margin = new Thickness(5) };
            StackPanel sp9 = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(40, 0, 0, 0) };
            g9.Children.Add(StatusIcons.GetStatus(Statuses.ActiveNotifiedReadArchived));
            sp9.Children.Add(g9);
            Label l9 = new Label { Content = "Active, Notified, Read & Archived", Margin = new Thickness(10, 3, 0, 0) };
            sp9.Children.Add(l9);
            sp.Children.Add(sp9);

            buic.Child = sp;
            flowDoc.Blocks.Add(buic);

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
            i.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(new Uri("pack://application:,,/Images/EventHorizonLogoNewSmall.png"));

            flowDoc.Blocks.Add(new BlockUIContainer(i));

            Paragraph dt = new Paragraph(new Run(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString()));
            flowDoc.Blocks.Add(dt);

            string TitleRun;
            string DescriptionRun;

            TitleRun = "Ref. " + eventHorizonLINQ_MainEvent.ID.ToString("D5");
            DescriptionRun = eventHorizonLINQ_MainEvent.Details;

            Paragraph titlerun = new Paragraph(new Run(TitleRun));
            titlerun.FontSize = 14;
            flowDoc.Blocks.Add(titlerun);

            Paragraph descriptionrun = new Paragraph(new Bold(new Run(DescriptionRun)));
            descriptionrun.FontSize = 18;
            flowDoc.Blocks.Add(descriptionrun);

            this.Title += " - " + TitleRun;

            if (File.Exists(eventHorizonLINQ_MainEvent.PathFileName))
            {
                // Create an InlineUIContainer to host an image
                InlineUIContainer imageContainer = new InlineUIContainer();
                
                Image productImage = new Image();
                productImage.Width = 763;
                productImage.Height = 407;
                productImage.Stretch = Stretch.Uniform;
                productImage.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                productImage.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                productImage.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(new Uri(eventHorizonLINQ_MainEvent.PathFileName));

                flowDoc.Blocks.Add(new BlockUIContainer(productImage));
            }

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