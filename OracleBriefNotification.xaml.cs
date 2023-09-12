using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace The_Oracle
{
    /// <summary>
    /// Interaction logic for OracleBriefNotification.xaml
    /// </summary>
    public partial class OracleBriefNotification : Window
    {
        MainWindow mw;
        Int32 eventID;
        Int32 notificationNumber;
        Int32 totalNotifications;
        EventHorizonLINQ eventHorizonLINQ;

        public static Dictionary<Int32, OracleBriefNotification> BriefNotifications = new Dictionary<int, OracleBriefNotification>();
        Int32 remindMeID = RemindMeDateTimes.OneHour;

        public OracleBriefNotification(MainWindow mw, Int32 eventID, Int32 notificationNumber, Int32 totalNotifications, EventHorizonLINQ eventHorizonLINQ = null)
        {
            InitializeComponent();
            this.Hide();

            this.mw = mw;
            this.eventID = eventID;
            this.notificationNumber = notificationNumber;
            this.totalNotifications = totalNotifications;
            this.eventHorizonLINQ = eventHorizonLINQ;

            Init();
        }

        private void Init()
        {
            if (eventHorizonLINQ != null)
            {
                NotificationsLabel.Content = notificationNumber + " of " + totalNotifications;
                UserNameTextBlock.Text = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonLINQ.UserID);
                TargetUserNameTextBlock.Text = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonLINQ.TargetUserID);
                DetailsTextBlock.Text = eventHorizonLINQ.Details;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Left + 10;
            this.Top = desktopWorkingArea.Bottom - 10 - this.Height;
            this.Show();

            if (!BriefNotifications.ContainsKey(eventID))
            {
                BriefNotifications.Add(eventID, this);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Closing -= Window_Closing;
            e.Cancel = true;
            var anim = new DoubleAnimation(0, (Duration)TimeSpan.FromSeconds(1));
            anim.Completed += (s, _) => this.Close();
            this.BeginAnimation(UIElement.OpacityProperty, anim);
        }

        private void TreeView_ButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            int buttonID = 255;

            bool success = Int32.TryParse(button.Tag.ToString(), out buttonID);

            if (button != null && success)
            {

                switch (buttonID)
                {
                    case 0:
                        if (BriefNotifications.ContainsKey(eventID)) BriefNotifications.Remove(eventID);

                        DataTableManagement.UpdateMyReminder(eventID, RemindMeModes.No, DateTime.Now, NotificationAcknowlegedModes.Yes);

                        if (eventHorizonLINQ.TargetUserID == XMLReaderWriter.UserID) DataTableManagement.UpdateStatusID(eventID, Statuses.ActiveNotified);

                        Close();

                        EventWindow veiwEventWindow = new EventWindow(MainWindow.mw, EventWindowModes.ViewMainEvent, eventHorizonLINQ, null);
                        veiwEventWindow.Left = veiwEventWindow.Left - 100;
                        veiwEventWindow.Top = veiwEventWindow.Top - 100;
                        veiwEventWindow.Show();
                        break;
                    case 1:
                        DateTime remindMeDateTime = DateTime.Now;
                        
                        if (RemindMeComboBox.SelectedIndex > -1)
                        {
                            switch (remindMeID)
                            {
                                case RemindMeDateTimes.FiveMinutes:
                                    remindMeDateTime = DateTime.Now + TimeSpan.FromMinutes(5);
                                    break;
                                case RemindMeDateTimes.OneHour:
                                    remindMeDateTime = DateTime.Now + TimeSpan.FromHours(1);
                                    break;
                                case RemindMeDateTimes.OneDay:
                                    remindMeDateTime = DateTime.Now + TimeSpan.FromDays(1);
                                    break;
                                case RemindMeDateTimes.TwoDays:
                                    remindMeDateTime = DateTime.Now + TimeSpan.FromDays(2);
                                    break;
                                case RemindMeDateTimes.NextWeek:
                                    remindMeDateTime = DateTime.Now + TimeSpan.FromDays(7);
                                    break;
                                case RemindMeDateTimes.NextMonth:
                                    remindMeDateTime = DateTime.Now + TimeSpan.FromDays(28);
                                    break;
                            }

                            DataTableManagement.UpdateMyReminder(eventID, RemindMeModes.Yes, remindMeDateTime, NotificationAcknowlegedModes.No);
                            if (eventHorizonLINQ.TargetUserID == XMLReaderWriter.UserID) DataTableManagement.UpdateStatusID(eventID, Statuses.ActiveNotified);
                        }
                        else
                        {
                            DataTableManagement.UpdateMyReminder(eventID, RemindMeModes.No, remindMeDateTime, NotificationAcknowlegedModes.Yes);
                            if (eventHorizonLINQ.TargetUserID == XMLReaderWriter.UserID) DataTableManagement.UpdateStatusID(eventID, Statuses.ActiveNotified);
                        }

                        if (BriefNotifications.ContainsKey(eventID)) BriefNotifications.Remove(eventID);
                        
                        Close();
                        break;
                }
            }
        }

        private void RemindMeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            remindMeID = RemindMeComboBox.SelectedIndex;

            Console.Write("remindMeID = ");
            Console.WriteLine(remindMeID);
        }

        private void Storyboard_Completed(object sender, EventArgs e)
        {
            var anim = new DoubleAnimation(1, (Duration)TimeSpan.FromSeconds(6));
            anim.Completed += (s, _) => this.Close();
            this.BeginAnimation(UIElement.OpacityProperty, anim);
        }
    }
}