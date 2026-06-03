using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.IO;
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

namespace Event_Horizon
{
    /// <summary>
    /// Interaction logic for EngineersWindow.xaml
    /// </summary>
    public partial class EngineersWindow : Window
    {
        public MainWindow mainWindow;
        EventHorizonEngineerLINQ eventHorizonEngineerLINQ;
   
        public List<EventHorizonEngineerLINQ> EventHorizonEngineerLINQList;
        public EventHorizonEngineerLINQ eventHorizonEngineerLINQ_SelectedItem;

        public EngineersWindow(MainWindow mainWindow, EventHorizonEngineerLINQ eventHorizonEngineerLINQ)
        {
            InitializeComponent();

            MainWindow.engineersWindow = this;

            this.mainWindow = mainWindow;
            this.eventHorizonEngineerLINQ = (EventHorizonEngineerLINQ)eventHorizonEngineerLINQ.Clone();

            RefreshEngineers();
        }

        public void RefreshEngineers()
        {
            try
            {
                EngineersListView.Items.Clear();
                DataTableManagementEngineer.EventHorizon_Engineer.Clear();

                EventHorizonEngineerLINQList = DataTableManagementEngineer.GetEngineers();

                Console.Write("EventHorizonEngineerLINQList Count = ");
                Console.WriteLine(EventHorizonEngineerLINQList.Count);

                foreach (EventHorizonEngineerLINQ eventHorizonEngineerLINQ in EventHorizonEngineerLINQList)
                {
                    EngineerRow engineerRow = EngineerRow.CreateEngineerRow(eventHorizonEngineerLINQ);

                    EngineersListView.Items.Add(engineerRow); 
                }
                Status.Content = "Engineers " + EngineersListView.Items.Count;
            }
            catch (Exception e)
            {
                Console.WriteLine("----------------------------------------");

                EventHorizonRequesterNotification msg = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "Engineers - " + e.Source, InformationTextBlock = e.Message }, RequesterTypes.OK);
                msg.ShowDialog();
            }
        }

        private void EngineersListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is EngineerRow))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            EngineerRow item = (EngineerRow)dep;

            eventHorizonEngineerLINQ_SelectedItem = (EventHorizonEngineerLINQ)item.Tag;

            Console.WriteLine();
            Console.WriteLine(">S>>EngineersListView_MouseDoubleClick<<<<");
            Console.WriteLine();

            Console.Write("item.Tag eventHorizonLINQ_SelectedItem.ID = ");
            Console.WriteLine(eventHorizonEngineerLINQ_SelectedItem.ID);

            Console.WriteLine();
            Console.WriteLine(">F>>EngineersListView_MouseDoubleClick<<<<");
            Console.WriteLine();

            EngineersListView.SelectedItem = item;

            if (eventHorizonEngineerLINQ_SelectedItem != null)
            {
                //try open event as EditEvent
                EngineerWindow editEngineerWindow = new EngineerWindow(MainWindow.mw, EventWindowModes.ViewMainEvent, eventHorizonEngineerLINQ_SelectedItem, null);
                editEngineerWindow.Show();
            }
        }
        private void DeleteEngineerRow()
        {
            if (eventHorizonEngineerLINQ_SelectedItem != null)
            {
                EventHorizonRequesterNotification orn = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "Delete this engineer, are you sure", InformationTextBlock = "You would not normally do this, it could affect referential integrity." }, RequesterTypes.NoYes);
                var result = orn.ShowDialog();
                if (result == true)
                {
                    if (eventHorizonEngineerLINQ_SelectedItem.ID > 0) DataTableManagementEngineer.DeleteEngineer(eventHorizonEngineerLINQ_SelectedItem.ID);
                }
            }

            RefreshEngineers();
        }
        private void DeleteRightMouseButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteEngineerRow();
        }

        private void EngineersListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is EngineerRow))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            EngineerRow item = (EngineerRow)dep;

            eventHorizonEngineerLINQ_SelectedItem = (EventHorizonEngineerLINQ)item.Tag;

            Console.WriteLine();
            Console.WriteLine(">S>>Engineers EngineersListView_PreviewMouseLeftButtonDown<<<<");
            Console.WriteLine();

            Console.Write("item.Tag eventHorizonEngineerLINQ_SelectedItem.ID = ");
            Console.WriteLine(eventHorizonEngineerLINQ_SelectedItem.ID);

            Console.WriteLine();
            Console.WriteLine(">F>>Engineers EngineersListView_PreviewMouseLeftButtonDown<<<<");
            Console.WriteLine();
        }
    }
}
