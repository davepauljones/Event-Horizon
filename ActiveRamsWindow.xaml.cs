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

            MainWindow.activeRamsWindow.eventHorizonRamsLINQ_SelectedItem = (EventHorizonRamsLINQ)item.Tag;

            MainWindow.activeRamsWindow.ActiveRamsListView.SelectedItem = item;

            if (MainWindow.activeRamsWindow.eventHorizonRamsLINQ_SelectedItem != null)
            {
                //try open event as EditRams
                JobWindow editJobWindow = new JobWindow(MainWindow.activeRamsWindow.mainWindow, EventWindowModes.ViewMainEvent, MainWindow.activeRamsWindow.eventHorizonRamsLINQ_SelectedItem, null);
                editJobWindow.Owner = MainWindow.activeRamsWindow;
                editJobWindow.Show();
            }
        }

        private void DeleteJobRow()
        {
            if (eventHorizonRamsLINQ_SelectedItem != null)
            {             
                EventHorizonRequesterNotification orn = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "Delete this job, are you sure", InformationTextBlock = "This will also delete the associated risk assessment and method statement." }, RequesterTypes.NoYes);
                var result = orn.ShowDialog();
                if (result == true)
                {
                    if (eventHorizonRamsLINQ_SelectedItem.ID > 0) DataTableManagementJob.DeleteJob(eventHorizonRamsLINQ_SelectedItem.ID);
                }
            }

            RefreshActiveRams();
        }
        private void DeleteRightMouseButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteJobRow();
        }

        private void ActiveRamsListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
            Console.WriteLine(">S>>Active Jobs ActiveRamsListView_PreviewMouseLeftButtonDown<<<<");
            Console.WriteLine();

            Console.Write("item.Tag eventHorizonRamsLINQ_SelectedItem.ID = ");
            Console.WriteLine(eventHorizonRamsLINQ_SelectedItem.ID);

            Console.WriteLine();
            Console.WriteLine(">F>>Active Jobs ActiveRamsListView_PreviewMouseLeftButtonDown<<<<");
            Console.WriteLine();
        }
    }
}
