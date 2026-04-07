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
    /// Interaction logic for ActiveRamsWindow.xaml
    /// </summary>
    public partial class ActiveRamsWindow : Window
    {
        MainWindow mainWindow;
        EventHorizonRamsLINQ eventHorizonRamsLINQ;
   
        public List<EventHorizonRamsLINQ> EventHorizonRamsLINQList;
        public EventHorizonRamsLINQ eventHorizonRamsLINQ_SelectedItem;

        public ActiveRamsWindow(MainWindow mainWindow, EventHorizonRamsLINQ eventHorizonRamsLINQ)
        {
            InitializeComponent();

            MainWindow.activeRamsWindow = this;

            this.mainWindow = mainWindow;
            this.eventHorizonRamsLINQ = (EventHorizonRamsLINQ)eventHorizonRamsLINQ.Clone();

            RefreshActiveRams();
        }

        public void RefreshActiveRams()
        {
            try
            {
                ActiveRamsListView.Items.Clear();
                DataTableManagementRams.EventHorizon_Rams.Clear();

                EventHorizonRamsLINQList = DataTableManagementRams.GetRamss();

                Console.Write("EventHorizonRamsLINQList Count = ");
                Console.WriteLine(EventHorizonRamsLINQList.Count);

                foreach (EventHorizonRamsLINQ eventHorizonRamsLINQ in EventHorizonRamsLINQList)
                {
                    RamsRow ramsRow = RamsRow.CreateRamsRow(eventHorizonRamsLINQ);

                    ActiveRamsListView.Items.Add(ramsRow); 
                }
                Status.Content = "Active rams " + ActiveRamsListView.Items.Count;
            }
            catch (Exception e)
            {
                Console.WriteLine("----------------------------------------");

                EventHorizonRequesterNotification msg = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "Active Rams - " + e.Source, InformationTextBlock = e.Message }, RequesterTypes.OK);
                msg.ShowDialog();
            }
        }

        private void ActiveRamsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is RamsRow))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            RamsRow item = (RamsRow)dep;

            eventHorizonRamsLINQ_SelectedItem = (EventHorizonRamsLINQ)item.Tag;

            Console.WriteLine();
            Console.WriteLine(">S>>ActiveRamsWindow ActiveRamsListView_MouseDoubleClick<<<<");
            Console.WriteLine();

            Console.Write("item.Tag eventHorizonRamsLINQ_SelectedItem.Source_Mode = ");
            Console.WriteLine(eventHorizonRamsLINQ_SelectedItem.Source_Mode);

            Console.Write("item.Tag eventHorizonRamsLINQ_SelectedItem.ID = ");
            Console.WriteLine(eventHorizonRamsLINQ_SelectedItem.ID);

            Console.WriteLine();
            Console.WriteLine(">F>>ActiveRamsWindow ActiveRamsListView_MouseDoubleClick<<<<");
            Console.WriteLine();

            ActiveRamsListView.SelectedItem = item;

            if (eventHorizonRamsLINQ_SelectedItem != null)
            {
                //try open event as EditRams
                RamsWindow editRamsWindow = new RamsWindow(mainWindow, EventWindowModes.ViewMainEvent, eventHorizonRamsLINQ_SelectedItem, null);
                editRamsWindow.Owner = this;
                editRamsWindow.Show();
            }
        }
    }
}
