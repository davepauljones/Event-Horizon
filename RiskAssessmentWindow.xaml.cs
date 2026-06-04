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
    /// Interaction logic for RiskAssessmentWindow.xaml
    /// </summary>
    public partial class RiskAssessmentWindow : Window
    {
        MainWindow mainWindow;
        int ramsWindowMode;
        int userID;
        EventHorizonJobLINQ eventHorizonJobLINQ;
        public RiskAssessmentWindow riskAssessmentWindow;

        DateTime formOpenStartTime;
        TimeSpan eventIsReadAfterTimeSpan = TimeSpan.FromSeconds(3);

        public Int32 ParentEventID;

        public RiskAssessmentWindow(MainWindow mainWindow, int ramsWindowMode, EventHorizonJobLINQ eventHorizonJobLINQ, RiskAssessmentWindow riskAssessmentWindow = null)
        {
            InitializeComponent();

            this.mainWindow = mainWindow;
            this.ramsWindowMode = ramsWindowMode;
            this.eventHorizonJobLINQ = (EventHorizonJobLINQ)eventHorizonJobLINQ.Clone();
            this.riskAssessmentWindow = riskAssessmentWindow;
            this.userID = eventHorizonJobLINQ.UserID;

            formOpenStartTime = DateTime.Now;

            switch (ramsWindowMode)
            {
                case EventWindowModes.ViewMainEvent:
                    GetRiskAssessment();

                    if (userID == XMLReaderWriter.UserID || XMLReaderWriter.UserID == 1)
                        ramsWindowMode = EventWindowModes.EditMainEvent;
                    else
                        ramsWindowMode = EventWindowModes.ViewMainEvent;

                    break;
                case EventWindowModes.ViewNote:
                    GetRiskAssessment();

                    if (userID == XMLReaderWriter.UserID || XMLReaderWriter.UserID == 1)
                        ramsWindowMode = EventWindowModes.EditNote;
                    else
                        ramsWindowMode = EventWindowModes.ViewNote;

                    break;
                case EventWindowModes.ViewReply:
                    GetRiskAssessment();

                    if (userID == XMLReaderWriter.UserID || XMLReaderWriter.UserID == 1)
                        ramsWindowMode = EventWindowModes.EditReply;
                    else
                        ramsWindowMode = EventWindowModes.ViewReply;

                    break;
                case EventWindowModes.EditMainEvent:
                    GetRiskAssessment();

                    if (userID == XMLReaderWriter.UserID || XMLReaderWriter.UserID == 1)
                        ramsWindowMode = EventWindowModes.EditMainEvent;
                    else
                        ramsWindowMode = EventWindowModes.ViewMainEvent;

                    break;
                case EventWindowModes.EditNote:
                    GetRiskAssessment();

                    if (userID == XMLReaderWriter.UserID || XMLReaderWriter.UserID == 1)
                        ramsWindowMode = EventWindowModes.EditNote;
                    else
                        ramsWindowMode = EventWindowModes.ViewNote;

                    break;
                case EventWindowModes.EditReply:
                    GetRiskAssessment();

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
                EventTitleLabel.Content = "View Risk Assessment";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonJobLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonJobLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonJobLINQ.UserID);
                RamsProfileTypeComboBox.IsEnabled = false;
                DescriptionTextBox.IsReadOnly = true;
                RevisionNoComboBox.IsEnabled = false;
                ReviewedDatePicker.IsEnabled = false;
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
                RevisionNoComboBox.IsEnabled = false;
                ReviewedDatePicker.IsEnabled = false;
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
                RevisionNoComboBox.IsEnabled = false;
                ReviewedDatePicker.IsEnabled = false;
                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Collapsed;
            }
            else if (ramsWindowMode == EventWindowModes.EditMainEvent)
            {
                EventIDLabel.Content = eventHorizonJobLINQ.ID.ToString("D5");
                ParentEventIDLabel.Content = eventHorizonJobLINQ.ID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonJobLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "Edit Risk Assessment";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonJobLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonJobLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonJobLINQ.UserID);
                RamsProfileTypeComboBox.IsEnabled = true;
                DescriptionTextBox.IsReadOnly = false;
                RevisionNoComboBox.IsEnabled = true;
                ReviewedDatePicker.IsEnabled = true;
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
                RevisionNoComboBox.IsEnabled = true;
                ReviewedDatePicker.IsEnabled = true;
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
                RevisionNoComboBox.IsEnabled = true;
                ReviewedDatePicker.IsEnabled = true;
                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Visible;
            }
            else if (ramsWindowMode == EventWindowModes.NewEvent)
            {
                EventIDLabel.Content = "-1";
                ParentEventIDLabel.Content = eventHorizonJobLINQ.ID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonJobLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "New Risk Assessment";
                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[XMLReaderWriter.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, XMLReaderWriter.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, XMLReaderWriter.UserID);
                RamsProfileTypeComboBox.SelectedIndex = eventHorizonJobLINQ.RamsProfileTypeID;
                RamsProfileTypeComboBox.IsEnabled = true;
                DescriptionTextBox.IsReadOnly = false;
                RevisionNoComboBox.SelectedIndex = 0;
                RevisionNoComboBox.IsEnabled = true;
                ReviewedDatePicker.IsEnabled = true;
                ReviewedDatePicker.SelectedDate = DateTime.Now;
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
                RevisionNoComboBox.SelectedIndex = 0;
                RevisionNoComboBox.IsEnabled = true;
                ReviewedDatePicker.IsEnabled = true;
                ReviewedDatePicker.SelectedDate = DateTime.Now;
                ReviewedDatePicker.IsEnabled = true;
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
                RevisionNoComboBox.SelectedIndex = 0;
                RevisionNoComboBox.IsEnabled = true;
                ReviewedDatePicker.IsEnabled = true;
                ReviewedDatePicker.SelectedDate = DateTime.Now;
                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Visible;
            }

            this.Owner = Application.Current.MainWindow;
        }
  
        private void GetRiskAssessment()
        {
            RamsProfileTypeComboBox.SelectedIndex = eventHorizonJobLINQ.RamsProfileTypeID;
            RevisionNoComboBox.SelectedIndex = eventHorizonRiskAssessmentLINQ.RevisionNo;

            DateTime reviewedDateTimet = eventHorizonRamsLINQ.ReviewedDateTime;
            ReviewedDatePicker.SelectedDate = reviewedDateTimet;

            UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonRamsLINQ.UserID);

            JobNoTextBox.Text = eventHorizonRamsLINQ.JobNo;
            DescriptionTextBox.Text = eventHorizonRamsLINQ.Description;
            ClientNameTextBox.Text = eventHorizonRamsLINQ.ClientName;
            SiteTextBox.Text = eventHorizonRamsLINQ.Site;
            LocationActivityTextBox.Text = eventHorizonRamsLINQ.LocationActivity;
            ElementReviewedTextBox.Text =  eventHorizonRamsLINQ.ElementReviewed;

            RamsStatusIDComboBox.SelectedIndex = eventHorizonRamsLINQ.StatusID;
        }
        
        private void SetRiskAssessment()
        {
            string descriptionSafeString = DescriptionTextBox.Text.Replace("'", "''");

            DateTime reviewedDateTime = DateTime.Now;
            reviewedDateTime = new DateTime(ReviewedDatePicker.SelectedDate.Value.Year, ReviewedDatePicker.SelectedDate.Value.Month, ReviewedDatePicker.SelectedDate.Value.Day, reviewedDateTime.Hour, reviewedDateTime.Minute, reviewedDateTime.Second);

            eventHorizonRamsLINQ.RamsProfileTypeID = RamsProfileTypeComboBox.SelectedIndex;
            eventHorizonRamsLINQ.Description = descriptionSafeString;
            eventHorizonRamsLINQ.ReviewedDateTime = reviewedDateTime;
        }
        
        public int RevisionNoID = 1;
        
        private void RevisionNoComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RevisionNoID = RevisionNoComboBox.SelectedIndex;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9:]+");
            e.Handled = regex.IsMatch(e.Text);
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
                        RiskAssessmentWindow nevn = new RiskAssessmentWindow(mainWindow, EventWindowModes.NewNote, eventHorizonRamsLINQ, this);
                        nevn.Show();
                        nevn.Left += 30;
                        nevn.Top += 30;
                        break;
                    case EventFormCloseButtons.Reply:
                        SetRams();
                        RiskAssessmentWindow nev = new RiskAssessmentWindow(mainWindow, EventWindowModes.NewReply, eventHorizonRamsLINQ, this);
                        nev.Show();
                        nev.Left += 30;
                        nev.Top += 30;
                        break;
                    case EventFormCloseButtons.Save:
                        SetRams();
                        switch (ramsWindowMode)
                        {
                            case EventWindowModes.ViewMainEvent:
                                DataTableManagementRiskAssessment.SaveRiskAssessment(this, eventHorizonRamsLINQ, EventWindowModes.ViewMainEvent);
                                break;
                            case EventWindowModes.ViewNote:
                                DataTableManagementRiskAssessment.SaveRiskAssessment(this, eventHorizonRamsLINQ, EventWindowModes.ViewNote);
                                break;
                            case EventWindowModes.ViewReply:
                                DataTableManagementRiskAssessment.SaveRiskAssessment(this, eventHorizonRamsLINQ, EventWindowModes.ViewReply);
                                break;
                            case EventWindowModes.EditMainEvent:
                                DataTableManagementRiskAssessment.SaveRiskAssessment(this, eventHorizonRamsLINQ, EventWindowModes.EditMainEvent);
                                break;
                            case EventWindowModes.EditNote:
                                DataTableManagementRiskAssessment.SaveRiskAssessment(this, eventHorizonRamsLINQ, EventWindowModes.EditNote);
                                break;
                            case EventWindowModes.EditReply:
                                DataTableManagementRiskAssessment.SaveRiskAssessment(this, eventHorizonRamsLINQ, EventWindowModes.EditReply);
                                break;
                            case EventWindowModes.NewEvent:
                                DataTableManagementRiskAssessment.SaveRiskAssessment(this, eventHorizonRamsLINQ, EventWindowModes.NewEvent);
                                break;
                            case EventWindowModes.NewNote:
                                DataTableManagementRiskAssessment.SaveRiskAssessment(this, eventHorizonRamsLINQ, EventWindowModes.NewNote);
                                break;
                            case EventWindowModes.NewReply:
                                DataTableManagementRiskAssessment.SaveRiskAssessment(this, eventHorizonRamsLINQ, EventWindowModes.NewReply);
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