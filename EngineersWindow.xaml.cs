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
    }
}
