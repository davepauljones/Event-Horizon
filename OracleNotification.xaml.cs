using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace The_Oracle
{
    /// <summary>
    /// Interaction logic for OracleNotification.xaml
    /// </summary>
    public partial class OracleNotification : Window
    {
        MainWindow mw;
        Int32 eventID;
        Int32 notificationNumber;
        Int32 totalNotifications;
        EventHorizonLINQ eventHorizonLINQ;

        public static Dictionary<Int32, OracleNotification> Notifications = new Dictionary<int, OracleNotification>();
        Int32 remindMeID = RemindMeDateTimes.OneHour;

        public OracleNotification(MainWindow mw, Int32 eventID, Int32 notificationNumber, Int32 totalNotifications, EventHorizonLINQ eventHorizonLINQ = null)
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
                DetailsTextBlock.Text = eventHorizonLINQ.Details;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right-10 - this.Width;
            this.Top = desktopWorkingArea.Bottom-10 - this.Height;
            this.Show();

            if (!Notifications.ContainsKey(eventID))
            {
                Notifications.Add(eventID, this);
            }
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
                        if (Notifications.ContainsKey(eventID)) Notifications.Remove(eventID);

                        DataTableManagement.UpdateMyReminder(eventID, RemindMeModes.No, DateTime.Now, NotificationAcknowlegedModes.Yes);

                        if (eventHorizonLINQ.TargetUserID == XMLReaderWriter.UserID)
                        {
                            DataTableManagement.UpdateStatusID(eventID, Statuses.ActiveNotified);
                            eventHorizonLINQ.StatusID = Statuses.ActiveNotified;
                        }

                        Close();

                        if (eventHorizonLINQ.EventModeID == EventModes.MainEvent)
                        {
                            EventWindow veiwEventWindow = new EventWindow(MainWindow.mw, EventWindowModes.ViewMainEvent, eventHorizonLINQ, null);
                            veiwEventWindow.Left = veiwEventWindow.Left - 100;
                            veiwEventWindow.Top = veiwEventWindow.Top - 100;
                            veiwEventWindow.Show();
                        }
                        else if (eventHorizonLINQ.EventModeID == EventModes.NoteEvent)
                        {
                            EventWindow veiwEventWindow = new EventWindow(MainWindow.mw, EventWindowModes.ViewNote, eventHorizonLINQ, null);
                            veiwEventWindow.Left = veiwEventWindow.Left - 100;
                            veiwEventWindow.Top = veiwEventWindow.Top - 100;
                            veiwEventWindow.Show();
                        }
                        else if (eventHorizonLINQ.EventModeID == EventModes.ReplyEvent)
                        {
                            EventWindow veiwEventWindow = new EventWindow(MainWindow.mw, EventWindowModes.ViewReply, eventHorizonLINQ, null);
                            veiwEventWindow.Left = veiwEventWindow.Left - 100;
                            veiwEventWindow.Top = veiwEventWindow.Top - 100;
                            veiwEventWindow.Show();
                        }
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

                        if (Notifications.ContainsKey(eventID)) Notifications.Remove(eventID);
                        
                        Close();
                        break;
                }
            }
        }

        private void RemindMeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            remindMeID = RemindMeComboBox.SelectedIndex;

            Console.Write("RemindMeID = ");
            Console.WriteLine(remindMeID);
        }
    }
}