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
        public MainWindow mainWindow;
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
                DataTableManagementRiskAssessment.EventHorizon_RiskAssessment.Clear();

                EventHorizonRamsLINQList = DataTableManagementRiskAssessment.GetRamss();

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
    }
}
