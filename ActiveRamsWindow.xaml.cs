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
        EventHorizonJobLINQ eventHorizonJobLINQ;
   
        public List<EventHorizonJobLINQ> EventHorizonJobLINQList;
        public EventHorizonJobLINQ eventHorizonJobLINQ_SelectedItem;

        public ActiveRamsWindow(MainWindow mainWindow, EventHorizonJobLINQ eventHorizonJobLINQ)
        {
            InitializeComponent();

            MainWindow.activeRamsWindow = this;

            this.mainWindow = mainWindow;
            this.eventHorizonJobLINQ = (EventHorizonJobLINQ)eventHorizonJobLINQ.Clone();

            RefreshActiveJobs();
        }

        public void RefreshActiveJobs()
        {
            try
            {
                ActiveRamsListView.Items.Clear();
                DataTableManagementJob.EventHorizon_Job.Clear();

                EventHorizonJobLINQList = DataTableManagementJob.GetJobs();

                Console.Write("EventHorizonJobLINQList Count = ");
                Console.WriteLine(EventHorizonJobLINQList.Count);

                foreach (EventHorizonJobLINQ eventHorizonJobLINQ in EventHorizonJobLINQList)
                {
                    RamsRow ramsRow = RamsRow.CreateJobRow(eventHorizonJobLINQ);

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

            MainWindow.activeRamsWindow.eventHorizonJobLINQ_SelectedItem = (EventHorizonJobLINQ)item.Tag;

            MainWindow.activeRamsWindow.ActiveRamsListView.SelectedItem = item;

            if (MainWindow.activeRamsWindow.eventHorizonJobLINQ_SelectedItem != null)
            {
                //try open event as EditRams
                JobWindow editJobWindow = new JobWindow(MainWindow.activeRamsWindow.mainWindow, EventWindowModes.ViewMainEvent, MainWindow.activeRamsWindow.eventHorizonJobLINQ_SelectedItem, null);
                editJobWindow.Owner = MainWindow.activeRamsWindow;
                editJobWindow.Show();
            }
        }

        private void DeleteJobRow()
        {
            if (eventHorizonJobLINQ_SelectedItem != null)
            {             
                EventHorizonRequesterNotification orn = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "Delete this job, are you sure", InformationTextBlock = "This will also delete the associated risk assessment and method statement." }, RequesterTypes.NoYes);
                var result = orn.ShowDialog();
                if (result == true)
                {
                    if (eventHorizonJobLINQ_SelectedItem.ID > 0) DataTableManagementJob.DeleteJob(eventHorizonJobLINQ_SelectedItem.ID);
                }
            }

            RefreshActiveJobs();
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

            eventHorizonJobLINQ_SelectedItem = (EventHorizonJobLINQ)item.Tag;

            Console.WriteLine();
            Console.WriteLine(">S>>Active Jobs ActiveRamsListView_PreviewMouseLeftButtonDown<<<<");
            Console.WriteLine();

            Console.Write("item.Tag eventHorizonJobLINQ_SelectedItem.ID = ");
            Console.WriteLine(eventHorizonJobLINQ_SelectedItem.ID);

            Console.WriteLine();
            Console.WriteLine(">F>>Active Jobs ActiveRamsListView_PreviewMouseLeftButtonDown<<<<");
            Console.WriteLine();
        }
    }
}