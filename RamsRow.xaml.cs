using FontAwesome.WPF;
using System;
using System.Drawing;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Event_Horizon
{
    /// <summary>
    /// Interaction logic for RamsRow.xaml
    /// </summary>
    public partial class RamsRow : UserControl
    {
        EventHorizonJobLINQ eventHorizonJobLINQ;

        public RamsRow(EventHorizonJobLINQ eventHorizonJobLINQ)
        {
            InitializeComponent();

            this.eventHorizonJobLINQ = eventHorizonJobLINQ;
        }

        public static RamsRow CreateJobRow(EventHorizonJobLINQ eventHorizonJobLINQ)
        {
            RamsRow ramsRow = new RamsRow(eventHorizonJobLINQ);

            ramsRow.RamsIDTextBlock.Text = eventHorizonJobLINQ.ID.ToString("D5");

            ramsRow.CreatedDateTimeTextBlock.Text = eventHorizonJobLINQ.CreationDate.ToString("dd/MM/y HH:mm");

            ramsRow.JobNoTextBlock.Text = eventHorizonJobLINQ.JobNo.ToString();

            ramsRow.DescriptionTextBlock.Text = eventHorizonJobLINQ.Description;

            if (eventHorizonJobLINQ.RamsProfileTypeID < DataTableManagementJob.RamsProfileTypesList.Count)
            {
                ramsRow.RamsProfileTypeFontAwesomeIconBorder.Background = new SolidColorBrush(Colors.Beige);
                ramsRow.RamsProfileTypeFontAwesomeIcon.Icon = FontAwesomeIcon.Star;
                ramsRow.RamsProfileTypeTextBlock.Text = DataTableManagementRiskAssessment.RamsProfileTypesList[eventHorizonJobLINQ.RamsProfileTypeID].ProfileName;
                ramsRow.BackgroundGrid.Background = new SolidColorBrush(Colors.Transparent);
            }
            else
            {
                ramsRow.RamsProfileTypeFontAwesomeIconBorder.Background = new SolidColorBrush(Colors.White);
                ramsRow.RamsProfileTypeFontAwesomeIcon.Icon = FontAwesomeIcon.Question;
                ramsRow.RamsProfileTypeTextBlock.Text = "Error";
                ramsRow.BackgroundGrid.Background = new SolidColorBrush(Colors.Transparent);
            }


            if (eventHorizonJobLINQ.UserID < XMLReaderWriter.UsersList.Count)
            {
                ramsRow.UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonJobLINQ.UserID].Color);
                ramsRow.UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonJobLINQ.UserID);
            }
            else
            {
                ramsRow.UserEllipse.Fill = new SolidColorBrush(Colors.White);
                ramsRow.UserLabel.Content = eventHorizonJobLINQ.UserID;
            }

            if (eventHorizonJobLINQ.UserID < XMLReaderWriter.UsersList.Count)
            {
                if (eventHorizonJobLINQ.UserID > 0)
                    ramsRow.UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonJobLINQ.UserID);
                else
                {
                    ramsRow.UserLabel.Content = "★";
                    ramsRow.UserLabel.Margin = new Thickness(0, -3, 0, 0);
                    ramsRow.UserLabel.FontSize = 14;
                }
            }
            else
            {
                ramsRow.UserEllipse.Fill = new SolidColorBrush(Colors.White);
                ramsRow.UserLabel.Content = eventHorizonJobLINQ.UserID;
            }

            ramsRow.ClientNameTextBlock.Text = eventHorizonJobLINQ.ClientName;
            ramsRow.SiteTextBlock.Text = eventHorizonJobLINQ.Site;
            ramsRow.LocationActivityTextBlock.Text = eventHorizonJobLINQ.LocationActivity;

            switch (eventHorizonJobLINQ.StatusID)
            {
                case 0:
                    ramsRow.StatusTextBlock.Text = "New";
                    ramsRow.StatusTextBlock.Background = new SolidColorBrush(Colors.Red);
                    break;
                case 1:
                    ramsRow.StatusTextBlock.Text = "Active";
                    ramsRow.StatusTextBlock.Background = new SolidColorBrush(Colors.Green);
                    break;
                case 2:
                    ramsRow.StatusTextBlock.Text = "Complete";
                    ramsRow.StatusTextBlock.Background = new SolidColorBrush(Colors.Transparent);
                    break;
            }
            
            ramsRow.Tag = eventHorizonJobLINQ;

            return ramsRow;
        }

        private void RAButton_Click(object sender, RoutedEventArgs e)
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
                RiskAssessmentWindow editRamsWindow = new RiskAssessmentWindow(MainWindow.activeRamsWindow.mainWindow, EventWindowModes.ViewMainEvent, MainWindow.activeRamsWindow.eventHorizonJobLINQ_SelectedItem, null);
                editRamsWindow.Owner = MainWindow.activeRamsWindow;
                editRamsWindow.Show();
            }
        }

        private void MSButton_Click(object sender, RoutedEventArgs e)
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
                MethodStatementWindow MethodStatementWindow = new MethodStatementWindow(MainWindow.activeRamsWindow.mainWindow, EventWindowModes.ViewMainEvent, MainWindow.activeRamsWindow.eventHorizonJobLINQ_SelectedItem, null);
                MethodStatementWindow.Owner = MainWindow.activeRamsWindow;
                MethodStatementWindow.Show();
            }
        }
  
    }
}