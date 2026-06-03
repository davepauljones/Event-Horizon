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
        EventHorizonRamsLINQ eventHorizonRamsLINQ;
        public JobWindow jobWindow;

        DateTime formOpenStartTime;
        TimeSpan eventIsReadAfterTimeSpan = TimeSpan.FromSeconds(3);

        public Int32 ParentEventID;

        public JobWindow(MainWindow mainWindow, int ramsWindowMode, EventHorizonRamsLINQ eventHorizonRamsLINQ, JobWindow jobWindow = null)
        {
            InitializeComponent();

            this.mainWindow = mainWindow;
            this.ramsWindowMode = ramsWindowMode;
            this.eventHorizonRamsLINQ = (EventHorizonRamsLINQ)eventHorizonRamsLINQ.Clone();
            this.jobWindow = jobWindow;
            this.userID = eventHorizonRamsLINQ.UserID;

            AddItemsToRamsProfileTypeComboBox();

            formOpenStartTime = DateTime.Now;

            switch (ramsWindowMode)
            {
                case EventWindowModes.ViewMainEvent:
                    GetRams();

                    if (userID == XMLReaderWriter.UserID || XMLReaderWriter.UserID == 1)
                        ramsWindowMode = EventWindowModes.EditMainEvent;
                    else
                        ramsWindowMode = EventWindowModes.ViewMainEvent;

                    break;
                case EventWindowModes.ViewNote:
                    GetRams();

                    if (userID == XMLReaderWriter.UserID || XMLReaderWriter.UserID == 1)
                        ramsWindowMode = EventWindowModes.EditNote;
                    else
                        ramsWindowMode = EventWindowModes.ViewNote;

                    break;
                case EventWindowModes.ViewReply:
                    GetRams();

                    if (userID == XMLReaderWriter.UserID || XMLReaderWriter.UserID == 1)
                        ramsWindowMode = EventWindowModes.EditReply;
                    else
                        ramsWindowMode = EventWindowModes.ViewReply;

                    break;
                case EventWindowModes.EditMainEvent:
                    GetRams();

                    if (userID == XMLReaderWriter.UserID || XMLReaderWriter.UserID == 1)
                        ramsWindowMode = EventWindowModes.EditMainEvent;
                    else
                        ramsWindowMode = EventWindowModes.ViewMainEvent;

                    break;
                case EventWindowModes.EditNote:
                    GetRams();

                    if (userID == XMLReaderWriter.UserID || XMLReaderWriter.UserID == 1)
                        ramsWindowMode = EventWindowModes.EditNote;
                    else
                        ramsWindowMode = EventWindowModes.ViewNote;

                    break;
                case EventWindowModes.EditReply:
                    GetRams();

                    if (userID == XMLReaderWriter.UserID || XMLReaderWriter.UserID == 1)
                        ramsWindowMode = EventWindowModes.EditReply;
                    else
                        ramsWindowMode = EventWindowModes.ViewReply;

                    break;
                case EventWindowModes.NewEvent:
                case EventWindowModes.NewNote:
                case EventWindowModes.NewReply:
                    eventHorizonRamsLINQ.UserID = XMLReaderWriter.UserID;
                    break;
            }

            if (ramsWindowMode == EventWindowModes.ViewMainEvent)
            {
                EventIDLabel.Content = eventHorizonRamsLINQ.ID.ToString("D5");
                ParentEventIDLabel.Content = eventHorizonRamsLINQ.ID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonRamsLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "View Job";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonRamsLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonRamsLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonRamsLINQ.UserID);
                RamsProfileTypeComboBox.IsEnabled = false;
                DescriptionTextBox.IsReadOnly = true;
                AddNoteButton.Visibility = Visibility.Visible;
                ReplyButton.Visibility = Visibility.Visible;
                SaveButton.Visibility = Visibility.Collapsed;
            }
            else if (ramsWindowMode == EventWindowModes.ViewNote)
            {
                EventIDLabel.Content = eventHorizonRamsLINQ.ID.ToString("D5");
                ParentEventIDLabel.Content = eventHorizonRamsLINQ.ID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonRamsLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "View Note";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonRamsLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonRamsLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonRamsLINQ.UserID);
                RamsProfileTypeComboBox.IsEnabled = false;
                DescriptionTextBox.IsReadOnly = true;
                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Collapsed;
            }
            else if (ramsWindowMode == EventWindowModes.ViewReply)
            {
                EventIDLabel.Content = eventHorizonRamsLINQ.ID.ToString("D5");
                ParentEventIDLabel.Content = eventHorizonRamsLINQ.ID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonRamsLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "View Reply";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonRamsLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonRamsLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonRamsLINQ.UserID);
                RamsProfileTypeComboBox.IsEnabled = false;
                DescriptionTextBox.IsReadOnly = true;
                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Collapsed;
            }
            else if (ramsWindowMode == EventWindowModes.EditMainEvent)
            {
                EventIDLabel.Content = eventHorizonRamsLINQ.ID.ToString("D5");
                ParentEventIDLabel.Content = eventHorizonRamsLINQ.ID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonRamsLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "Edit Job";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonRamsLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonRamsLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonRamsLINQ.UserID);
                RamsProfileTypeComboBox.IsEnabled = true;
                DescriptionTextBox.IsReadOnly = false;
                AddNoteButton.Visibility = Visibility.Visible;
                ReplyButton.Visibility = Visibility.Visible;
                SaveButton.Visibility = Visibility.Visible;
            }
            else if (ramsWindowMode == EventWindowModes.EditNote)
            {
                EventIDLabel.Content = eventHorizonRamsLINQ.ID.ToString("D5");
                ParentEventIDLabel.Content = eventHorizonRamsLINQ.ID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonRamsLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "Edit Note";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonRamsLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonRamsLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonRamsLINQ.UserID);
                RamsProfileTypeComboBox.IsEnabled = true;
                DescriptionTextBox.IsReadOnly = false;
                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Visible;
            }
            else if (ramsWindowMode == EventWindowModes.EditReply)
            {
                EventIDLabel.Content = eventHorizonRamsLINQ.ID.ToString("D5");
                ParentEventIDLabel.Content = eventHorizonRamsLINQ.ID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonRamsLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "Edit Reply";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonRamsLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonRamsLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonRamsLINQ.UserID);
                RamsProfileTypeComboBox.IsEnabled = true;
                DescriptionTextBox.IsReadOnly = false;
                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Visible;
            }
            else if (ramsWindowMode == EventWindowModes.NewEvent)
            {
                EventIDLabel.Content = "-1";
                ParentEventIDLabel.Content = eventHorizonRamsLINQ.ID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonRamsLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "New Job";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[XMLReaderWriter.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, XMLReaderWriter.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, XMLReaderWriter.UserID);
                RamsProfileTypeComboBox.SelectedIndex = eventHorizonRamsLINQ.RamsProfileTypeID;
                RamsProfileTypeComboBox.IsEnabled = true;
                DescriptionTextBox.IsReadOnly = false;  
                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Visible;
            }
            else if (ramsWindowMode == EventWindowModes.NewNote)
            {
                EventIDLabel.Content = "-1";
                ParentEventIDLabel.Content = eventHorizonRamsLINQ.ID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonRamsLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "New Note";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[XMLReaderWriter.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, XMLReaderWriter.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, XMLReaderWriter.UserID);
                RamsProfileTypeComboBox.SelectedIndex = eventHorizonRamsLINQ.RamsProfileTypeID;
                RamsProfileTypeComboBox.IsEnabled = false;
                DescriptionTextBox.IsReadOnly = false;
                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Visible;
            }
            else if (ramsWindowMode == EventWindowModes.NewReply)
            {
                EventIDLabel.Content = "-1";
                ParentEventIDLabel.Content = eventHorizonRamsLINQ.ID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonRamsLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "New Reply";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonRamsLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonRamsLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonRamsLINQ.UserID);
                RamsProfileTypeComboBox.SelectedIndex = eventHorizonRamsLINQ.RamsProfileTypeID;
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
            foreach (RamsProfileType ramsProfileType in DataTableManagementRiskAssessment.RamsProfileTypesList)
            {
                RamsProfileTypeComboBox.Items.Add(EventHorizonRamsProfileTypes.GetRamsProfileTypeStackPanel(ramsProfileType));
            }

            if (ramsWindowMode != EventWindowModes.EditMainEvent || ramsWindowMode != EventWindowModes.EditNote || ramsWindowMode != EventWindowModes.EditReply) RamsProfileTypeComboBox.SelectedIndex = 0;
        }
  
        private void GetRams()
        {
            RamsProfileTypeComboBox.SelectedIndex = eventHorizonRamsLINQ.RamsProfileTypeID;

            UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonRamsLINQ.UserID);

            JobNoTextBox.Text = eventHorizonRamsLINQ.JobNo;
            DescriptionTextBox.Text = eventHorizonRamsLINQ.Description;
            ClientNameTextBox.Text = eventHorizonRamsLINQ.ClientName;
            SiteTextBox.Text = eventHorizonRamsLINQ.Site;
            LocationActivityTextBox.Text = eventHorizonRamsLINQ.LocationActivity;

            RamsStatusIDComboBox.SelectedIndex = eventHorizonRamsLINQ.StatusID;
        }
        
        private void SetRams()
        {
            string jobNoSafeString = JobNoTextBox.Text.Replace("'", "''");
            string descriptionSafeString = DescriptionTextBox.Text.Replace("'", "''");
            string clientNameSafeString = ClientNameTextBox.Text.Replace("'", "''");
            string siteSafeString = SiteTextBox.Text.Replace("'", "''");
            string locationActivitySafeString = LocationActivityTextBox.Text.Replace("'", "''");

            eventHorizonRamsLINQ.RamsProfileTypeID = RamsProfileTypeComboBox.SelectedIndex;
            eventHorizonRamsLINQ.JobNo = jobNoSafeString;
            eventHorizonRamsLINQ.Description = descriptionSafeString;
            eventHorizonRamsLINQ.ClientName = clientNameSafeString;
            eventHorizonRamsLINQ.Site = siteSafeString;
            eventHorizonRamsLINQ.LocationActivity = locationActivitySafeString;
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
                        SetRams();
                        JobWindow nevn = new JobWindow(mainWindow, EventWindowModes.NewNote, eventHorizonRamsLINQ, this);
                        nevn.Show();
                        nevn.Left += 30;
                        nevn.Top += 30;
                        break;
                    case EventFormCloseButtons.Reply:
                        SetRams();
                        JobWindow nev = new JobWindow(mainWindow, EventWindowModes.NewReply, eventHorizonRamsLINQ, this);
                        nev.Show();
                        nev.Left += 30;
                        nev.Top += 30;
                        break;
                    case EventFormCloseButtons.Save:
                        SetRams();
                        switch (ramsWindowMode)
                        {
                            case EventWindowModes.ViewMainEvent:
                                DataTableManagementJob.SaveJob(this, eventHorizonRamsLINQ, EventWindowModes.ViewMainEvent);
                                break;
                            case EventWindowModes.ViewNote:
                                DataTableManagementJob.SaveJob(this, eventHorizonRamsLINQ, EventWindowModes.ViewNote);
                                break;
                            case EventWindowModes.ViewReply:
                                DataTableManagementJob.SaveJob(this, eventHorizonRamsLINQ, EventWindowModes.ViewReply);
                                break;
                            case EventWindowModes.EditMainEvent:
                                DataTableManagementJob.SaveJob(this, eventHorizonRamsLINQ, EventWindowModes.EditMainEvent);
                                break;
                            case EventWindowModes.EditNote:
                                DataTableManagementJob.SaveJob(this, eventHorizonRamsLINQ, EventWindowModes.EditNote);
                                break;
                            case EventWindowModes.EditReply:
                                DataTableManagementJob.SaveJob(this, eventHorizonRamsLINQ, EventWindowModes.EditReply);
                                break;
                            case EventWindowModes.NewEvent:
                                DataTableManagementJob.SaveJob(this, eventHorizonRamsLINQ, EventWindowModes.NewEvent);
                                break;
                            case EventWindowModes.NewNote:
                                DataTableManagementJob.SaveJob(this, eventHorizonRamsLINQ, EventWindowModes.NewNote);
                                break;
                            case EventWindowModes.NewReply:
                                DataTableManagementJob.SaveJob(this, eventHorizonRamsLINQ, EventWindowModes.NewReply);
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