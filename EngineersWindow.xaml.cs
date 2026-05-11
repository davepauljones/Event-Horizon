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
        EventHorizonEngineersLINQ eventHorizonEngineersLINQ;
   
        public List<EventHorizonEngineersLINQ> EventHorizonEngineersLINQList;
        public EventHorizonEngineersLINQ eventHorizonEngineersLINQ_SelectedItem;

        public EngineersWindow(MainWindow mainWindow, EventHorizonEngineersLINQ eventHorizonEngineersLINQ)
        {
            InitializeComponent();

            MainWindow.engineersWindow = this;

            this.mainWindow = mainWindow;
            this.eventHorizonEngineersLINQ = (EventHorizonEngineersLINQ)eventHorizonEngineersLINQ.Clone();

            RefreshEngineers();
        }

        public void RefreshEngineers()
        {
            try
            {
                EngineersListView.Items.Clear();
                DataTableManagementEngineer.EventHorizon_Engineer.Clear();

                EventHorizonEngineersLINQList = DataTableManagementEngineer.GetEngineers();

                Console.Write("EventHorizonEngineersLINQList Count = ");
                Console.WriteLine(EventHorizonEngineersLINQList.Count);

                foreach (EventHorizonEngineersLINQ eventHorizonRamsLINQ in EventHorizonEngineersLINQList)
                {
                    EngineerRow engineerRow = EngineerRow.CreateEngineerRow(eventHorizonEngineersLINQ);

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
