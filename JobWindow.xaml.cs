using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Globalization;
using System.Text.RegularExpressions;
using System.IO;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using Ookii.Dialogs.Wpf;

namespace Event_Horizon
{
    /// <summary>
    /// Interaction logic for JobWindow.xaml
    /// </summary>
    public partial class JobWindow : Window
    {
        MainWindow mainWindow;
        int ramsWindowMode;
        int userID;
        EventHorizonJobLINQ eventHorizonJobLINQ;
        public JobWindow jobWindow;

        DateTime formOpenStartTime;
        TimeSpan eventIsReadAfterTimeSpan = TimeSpan.FromSeconds(3);

        public Int32 ParentEventID;

        public JobWindow(MainWindow mainWindow, int ramsWindowMode, EventHorizonJobLINQ eventHorizonJobLINQ, JobWindow jobWindow = null)
        {
            InitializeComponent();

            this.mainWindow = mainWindow;
            this.ramsWindowMode = ramsWindowMode;
            this.eventHorizonJobLINQ = (EventHorizonJobLINQ)eventHorizonJobLINQ.Clone();
            this.jobWindow = jobWindow;
            this.userID = eventHorizonJobLINQ.UserID;

            AddItemsToRamsProfileTypeComboBox();

            formOpenStartTime = DateTime.Now;

            switch (ramsWindowMode)
            {
                case EventWindowModes.ViewMainEvent:
                    GetJob();

                    if (userID == XMLReaderWriter.UserID || XMLReaderWriter.UserID == 1)
                        ramsWindowMode = EventWindowModes.EditMainEvent;
                    else
                        ramsWindowMode = EventWindowModes.ViewMainEvent;

                    break;
                case EventWindowModes.ViewNote:
                    GetJob();

                    if (userID == XMLReaderWriter.UserID || XMLReaderWriter.UserID == 1)
                        ramsWindowMode = EventWindowModes.EditNote;
                    else
                        ramsWindowMode = EventWindowModes.ViewNote;

                    break;
                case EventWindowModes.ViewReply:
                    GetJob();

                    if (userID == XMLReaderWriter.UserID || XMLReaderWriter.UserID == 1)
                        ramsWindowMode = EventWindowModes.EditReply;
                    else
                        ramsWindowMode = EventWindowModes.ViewReply;

                    break;
                case EventWindowModes.EditMainEvent:
                    GetJob();

                    if (userID == XMLReaderWriter.UserID || XMLReaderWriter.UserID == 1)
                        ramsWindowMode = EventWindowModes.EditMainEvent;
                    else
                        ramsWindowMode = EventWindowModes.ViewMainEvent;

                    break;
                case EventWindowModes.EditNote:
                    GetJob();

                    if (userID == XMLReaderWriter.UserID || XMLReaderWriter.UserID == 1)
                        ramsWindowMode = EventWindowModes.EditNote;
                    else
                        ramsWindowMode = EventWindowModes.ViewNote;

                    break;
                case EventWindowModes.EditReply:
                    GetJob();

                    if (userID == XMLReaderWriter.UserID || XMLReaderWriter.UserID == 1)
                        ramsWindowMode = EventWindowModes.EditReply;
                    else
                        ramsWindowMode = EventWindowModes.ViewReply;

                    break;
                case EventWindowModes.NewEvent:
                case EventWindowModes.NewNote:
                case EventWindowModes.NewReply:
                    eventHorizonJobLINQ.UserID = XMLReaderWriter.UserID;
                    break;
            }

            if (ramsWindowMode == EventWindowModes.ViewMainEvent)
            {
                EventIDLabel.Content = eventHorizonJobLINQ.ID.ToString("D5");
                ParentEventIDLabel.Content = eventHorizonJobLINQ.ID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonJobLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "View Job";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonJobLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonJobLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonJobLINQ.UserID);
                RamsProfileTypeComboBox.IsEnabled = false;
                DescriptionTextBox.IsReadOnly = true;
                AddNoteButton.Visibility = Visibility.Visible;
                ReplyButton.Visibility = Visibility.Visible;
                SaveButton.Visibility = Visibility.Collapsed;
            }
            else if (ramsWindowMode == EventWindowModes.ViewNote)
            {
                EventIDLabel.Content = eventHorizonJobLINQ.ID.ToString("D5");
                ParentEventIDLabel.Content = eventHorizonJobLINQ.ID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonJobLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "View Note";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonJobLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonJobLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonJobLINQ.UserID);
                RamsProfileTypeComboBox.IsEnabled = false;
                DescriptionTextBox.IsReadOnly = true;
                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Collapsed;
            }
            else if (ramsWindowMode == EventWindowModes.ViewReply)
            {
                EventIDLabel.Content = eventHorizonJobLINQ.ID.ToString("D5");
                ParentEventIDLabel.Content = eventHorizonJobLINQ.ID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonJobLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "View Reply";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonJobLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonJobLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonJobLINQ.UserID);
                RamsProfileTypeComboBox.IsEnabled = false;
                DescriptionTextBox.IsReadOnly = true;
                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Collapsed;
            }
            else if (ramsWindowMode == EventWindowModes.EditMainEvent)
            {
                EventIDLabel.Content = eventHorizonJobLINQ.ID.ToString("D5");
                ParentEventIDLabel.Content = eventHorizonJobLINQ.ID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonJobLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "Edit Job";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonJobLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonJobLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonJobLINQ.UserID);
                RamsProfileTypeComboBox.IsEnabled = true;
                DescriptionTextBox.IsReadOnly = false;
                AddNoteButton.Visibility = Visibility.Visible;
                ReplyButton.Visibility = Visibility.Visible;
                SaveButton.Visibility = Visibility.Visible;
            }
            else if (ramsWindowMode == EventWindowModes.EditNote)
            {
                EventIDLabel.Content = eventHorizonJobLINQ.ID.ToString("D5");
                ParentEventIDLabel.Content = eventHorizonJobLINQ.ID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonJobLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "Edit Note";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonJobLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonJobLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonJobLINQ.UserID);
                RamsProfileTypeComboBox.IsEnabled = true;
                DescriptionTextBox.IsReadOnly = false;
                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Visible;
            }
            else if (ramsWindowMode == EventWindowModes.EditReply)
            {
                EventIDLabel.Content = eventHorizonJobLINQ.ID.ToString("D5");
                ParentEventIDLabel.Content = eventHorizonJobLINQ.ID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonJobLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "Edit Reply";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonJobLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonJobLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonJobLINQ.UserID);
                RamsProfileTypeComboBox.IsEnabled = true;
                DescriptionTextBox.IsReadOnly = false;
                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Visible;
            }
            else if (ramsWindowMode == EventWindowModes.NewEvent)
            {
                EventIDLabel.Content = "-1";
                ParentEventIDLabel.Content = eventHorizonJobLINQ.ID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonJobLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "New Job";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[XMLReaderWriter.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, XMLReaderWriter.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, XMLReaderWriter.UserID);
                RamsProfileTypeComboBox.SelectedIndex = eventHorizonJobLINQ.RamsProfileTypeID;
                RamsProfileTypeComboBox.IsEnabled = true;
                DescriptionTextBox.IsReadOnly = false;  
                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Visible;
            }
            else if (ramsWindowMode == EventWindowModes.NewNote)
            {
                EventIDLabel.Content = "-1";
                ParentEventIDLabel.Content = eventHorizonJobLINQ.ID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonJobLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "New Note";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[XMLReaderWriter.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, XMLReaderWriter.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, XMLReaderWriter.UserID);
                RamsProfileTypeComboBox.SelectedIndex = eventHorizonJobLINQ.RamsProfileTypeID;
                RamsProfileTypeComboBox.IsEnabled = false;
                DescriptionTextBox.IsReadOnly = false;
                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Visible;
            }
            else if (ramsWindowMode == EventWindowModes.NewReply)
            {
                EventIDLabel.Content = "-1";
                ParentEventIDLabel.Content = eventHorizonJobLINQ.ID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonJobLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "New Reply";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonJobLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonJobLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonJobLINQ.UserID);
                RamsProfileTypeComboBox.SelectedIndex = eventHorizonJobLINQ.RamsProfileTypeID;
                RamsProfileTypeComboBox.IsEnabled = false;
                DescriptionTextBox.IsReadOnly = false;
                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Visible;
            }

            this.Owner = Application.Current.MainWindow;
        }
        
        private void AddItemsToRamsProfileTypeComboBox()
        {
            foreach (RamsProfileType ramsProfileType in DataTableManagementJob.RamsProfileTypesList)
            {
                RamsProfileTypeComboBox.Items.Add(EventHorizonRamsProfileTypes.GetRamsProfileTypeStackPanel(ramsProfileType));
            }

            if (ramsWindowMode != EventWindowModes.EditMainEvent || ramsWindowMode != EventWindowModes.EditNote || ramsWindowMode != EventWindowModes.EditReply) RamsProfileTypeComboBox.SelectedIndex = 0;
        }
  
        private void GetJob()
        {
            RamsProfileTypeComboBox.SelectedIndex = eventHorizonJobLINQ.RamsProfileTypeID;

            UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonJobLINQ.UserID);

            JobNoTextBox.Text = eventHorizonJobLINQ.JobNo;
            DescriptionTextBox.Text = eventHorizonJobLINQ.Description;
            ClientNameTextBox.Text = eventHorizonJobLINQ.ClientName;
            SiteTextBox.Text = eventHorizonJobLINQ.Site;
            LocationActivityTextBox.Text = eventHorizonJobLINQ.LocationActivity;

            RamsStatusIDComboBox.SelectedIndex = eventHorizonJobLINQ.StatusID;
        }
        
        private void SetJob()
        {
            string jobNoSafeString = JobNoTextBox.Text.Replace("'", "''");
            string descriptionSafeString = DescriptionTextBox.Text.Replace("'", "''");
            string clientNameSafeString = ClientNameTextBox.Text.Replace("'", "''");
            string siteSafeString = SiteTextBox.Text.Replace("'", "''");
            string locationActivitySafeString = LocationActivityTextBox.Text.Replace("'", "''");

            eventHorizonJobLINQ.RamsProfileTypeID = RamsProfileTypeComboBox.SelectedIndex;
            eventHorizonJobLINQ.JobNo = jobNoSafeString;
            eventHorizonJobLINQ.Description = descriptionSafeString;
            eventHorizonJobLINQ.ClientName = clientNameSafeString;
            eventHorizonJobLINQ.Site = siteSafeString;
            eventHorizonJobLINQ.LocationActivity = locationActivitySafeString;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
         
        }

        private void FormCloseButtons_ButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = e.OriginalSource as Button;

            int buttonID = 0;

            bool success = Int32.TryParse(button.Tag.ToString(), out buttonID);

            if (button != null && success)
            {
                switch (buttonID)
                {
                    case EventFormCloseButtons.Cancel:
                        mainWindow.Status.Content = "Last log entry was cancelled.";
                        Close();
                        break;
                    case EventFormCloseButtons.Note:
                        SetJob();
                        JobWindow nevn = new JobWindow(mainWindow, EventWindowModes.NewNote, eventHorizonJobLINQ, this);
                        nevn.Show();
                        nevn.Left += 30;
                        nevn.Top += 30;
                        break;
                    case EventFormCloseButtons.Reply:
                        SetJob();
                        JobWindow nev = new JobWindow(mainWindow, EventWindowModes.NewReply, eventHorizonJobLINQ, this);
                        nev.Show();
                        nev.Left += 30;
                        nev.Top += 30;
                        break;
                    case EventFormCloseButtons.Save:
                        SetJob();
                        switch (ramsWindowMode)
                        {
                            case EventWindowModes.ViewMainEvent:
                                DataTableManagementJob.SaveJob(this, eventHorizonJobLINQ, EventWindowModes.ViewMainEvent);
                                break;
                            case EventWindowModes.ViewNote:
                                DataTableManagementJob.SaveJob(this, eventHorizonJobLINQ, EventWindowModes.ViewNote);
                                break;
                            case EventWindowModes.ViewReply:
                                DataTableManagementJob.SaveJob(this, eventHorizonJobLINQ, EventWindowModes.ViewReply);
                                break;
                            case EventWindowModes.EditMainEvent:
                                DataTableManagementJob.SaveJob(this, eventHorizonJobLINQ, EventWindowModes.EditMainEvent);
                                break;
                            case EventWindowModes.EditNote:
                                DataTableManagementJob.SaveJob(this, eventHorizonJobLINQ, EventWindowModes.EditNote);
                                break;
                            case EventWindowModes.EditReply:
                                DataTableManagementJob.SaveJob(this, eventHorizonJobLINQ, EventWindowModes.EditReply);
                                break;
                            case EventWindowModes.NewEvent:
                                DataTableManagementJob.SaveJob(this, eventHorizonJobLINQ, EventWindowModes.NewEvent);
                                break;
                            case EventWindowModes.NewNote:
                                DataTableManagementJob.SaveJob(this, eventHorizonJobLINQ, EventWindowModes.NewNote);
                                break;
                            case EventWindowModes.NewReply:
                                DataTableManagementJob.SaveJob(this, eventHorizonJobLINQ, EventWindowModes.NewReply);
                                break;
                        }
                        break;
                }
            }
        }

        String RamsProfileTypeName = string.Empty;

        private void RamsProfileTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is StackPanel))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            var selectedTag = ((StackPanel)RamsProfileTypeComboBox.SelectedItem).Tag.ToString();

            RamsProfileTypeName = selectedTag;
           
            Console.Write("** RamsProfileTypeComboBox_SelectedIndex = ");
            Console.WriteLine(RamsProfileTypeComboBox.SelectedIndex);
            Console.Write("** item.Tag EventTypeName = ");
            Console.WriteLine(RamsProfileTypeName);
        }

        public int RamsStatusID = RamsStatus.New;
        private void RamsStatusIDComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RamsStatusID = RamsStatusIDComboBox.SelectedIndex;
        }
    }
}