using System;
using System.Collections.Generic;
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

namespace The_Oracle
{
    /// <summary>
    /// Interaction logic for OracleNotification.xaml
    /// </summary>
    public partial class OracleNotification : Window
    {
        MainWindow mw;
        Int32 EventID;
        Int32 NotificationNumber;
        Int32 TotalNotifications;
        EventHorizonLINQ oe;

        public static Dictionary<Int32, OracleNotification> Notifications = new Dictionary<int, OracleNotification>();

        public OracleNotification(MainWindow mw, Int32 EventID, Int32 NotificationNumber, Int32 TotalNotifications, EventHorizonLINQ oe = null)
        {
            InitializeComponent();
            this.Hide();

            this.mw = mw;
            this.EventID = EventID;
            this.NotificationNumber = NotificationNumber;
            this.TotalNotifications = TotalNotifications;
            this.oe = oe;

            Init();
        }

        private void Init()
        {
            if (oe != null)
            {
                NotificationsLabel.Content = NotificationNumber + " of " + TotalNotifications;
                UserNameTextBlock.Text = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, oe.UserID);
                //UserNameTextBlock.Text = oe.TargetUserID.ToString();
                DetailsTextBlock.Text = oe.Details;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right-10 - this.Width;
            this.Top = desktopWorkingArea.Bottom-10 - this.Height;
            this.Show();

            if (!Notifications.ContainsKey(EventID))
            {
                Notifications.Add(EventID, this);
            }
        }

        private void TreeView_ButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            int ButtonID = 255;

            bool success = Int32.TryParse(button.Tag.ToString(), out ButtonID);

            if (button != null && success)
            {

                switch (ButtonID)
                {
                    case 0:
                        if (Notifications.ContainsKey(EventID)) Notifications.Remove(EventID);

                        DataTableManagement.UpdateMyReminder(EventID, RemindMeModes.No, DateTime.Now, NotificationAcknowlegedModes.Yes);

                        if (oe.TargetUserID == XMLReaderWriter.UserID) DataTableManagement.UpdateStatusID(EventID, Statuses.ActiveNotified);

                        Close();

                        EventWindow ve = new EventWindow(MainWindow.mw, oe, null);
                        ve.Left = ve.Left - 100;
                        ve.Top = ve.Top - 100;
                        ve.Show();
                        break;
                    case 1:
                        DateTime RemindMeDateTime = DateTime.Now;
                        
                        if (RemindMeComboBox.SelectedIndex > -1)
                        {
                            switch (RemindMeID)
                            {
                                case RemindMeDateTimes.FiveMinutes:
                                    RemindMeDateTime = DateTime.Now + TimeSpan.FromMinutes(5);
                                    break;
                                case RemindMeDateTimes.OneHour:
                                    RemindMeDateTime = DateTime.Now + TimeSpan.FromHours(1);
                                    break;
                                case RemindMeDateTimes.OneDay:
                                    RemindMeDateTime = DateTime.Now + TimeSpan.FromDays(1);
                                    break;
                                case RemindMeDateTimes.TwoDays:
                                    RemindMeDateTime = DateTime.Now + TimeSpan.FromDays(2);
                                    break;
                                case RemindMeDateTimes.NextWeek:
                                    RemindMeDateTime = DateTime.Now + TimeSpan.FromDays(7);
                                    break;
                                case RemindMeDateTimes.NextMonth:
                                    RemindMeDateTime = DateTime.Now + TimeSpan.FromDays(28);
                                    break;
                            }

                            DataTableManagement.UpdateMyReminder(EventID, RemindMeModes.Yes, RemindMeDateTime, NotificationAcknowlegedModes.No);
                            if (oe.TargetUserID == XMLReaderWriter.UserID) DataTableManagement.UpdateStatusID(EventID, Statuses.ActiveNotified);
                        }
                        else
                        {
                            DataTableManagement.UpdateMyReminder(EventID, RemindMeModes.No, RemindMeDateTime, NotificationAcknowlegedModes.Yes);
                            if (oe.TargetUserID == XMLReaderWriter.UserID) DataTableManagement.UpdateStatusID(EventID, Statuses.ActiveNotified);
                        }

                        if (Notifications.ContainsKey(EventID)) Notifications.Remove(EventID);
                        
                        Close();
                        break;
                }
            }
        }

        Int32 RemindMeID = RemindMeDateTimes.OneHour;
        private void RemindMeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RemindMeID = RemindMeComboBox.SelectedIndex;

            Console.Write("RemindMeID = ");
            Console.WriteLine(RemindMeID);
        }
    }
}
