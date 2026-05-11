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
    /// Interaction logic for EngineerWindow.xaml
    /// </summary>
    public partial class EngineerWindow : Window
    {
        MainWindow mainWindow;
        int ramsWindowMode;
        int userID;
        EventHorizonEngineersLINQ eventHorizonEngineersLINQ;
        public EngineerWindow engineerWindow;

        DateTime formOpenStartTime;
        TimeSpan eventIsReadAfterTimeSpan = TimeSpan.FromSeconds(3);

        public Int32 ParentEventID;

        public EngineerWindow(MainWindow mainWindow, int ramsWindowMode, EventHorizonEngineersLINQ eventHorizonEngineersLINQ, EngineerWindow engineerWindow = null)
        {
            InitializeComponent();

            this.mainWindow = mainWindow;
            this.ramsWindowMode = ramsWindowMode;
            this.eventHorizonEngineersLINQ = (EventHorizonEngineersLINQ)eventHorizonEngineersLINQ.Clone();
            this.engineerWindow = engineerWindow;
            this.userID = eventHorizonRamsLINQ.UserID;

            formOpenStartTime = DateTime.Now;

            switch (ramsWindowMode)
            {
                case EventWindowModes.ViewMainEvent:
                    GetEngineer();

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
                CreatedDateTimeLabel.Content = eventHorizonRamsLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "View Main Method Statement";
                
                NameTextBox.IsReadOnly = true;
                RoleTextBox.IsReadOnly = true;
                CompetenceDetailsTextBox.IsReadOnly = true;

                AddNoteButton.Visibility = Visibility.Visible;
                ReplyButton.Visibility = Visibility.Visible;
                SaveButton.Visibility = Visibility.Collapsed;
            }
            else if (ramsWindowMode == EventWindowModes.ViewNote)
            {
                EventIDLabel.Content = eventHorizonRamsLINQ.ID.ToString("D5");  
                CreatedDateTimeLabel.Content = eventHorizonRamsLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "View Note";
                
                NameTextBox.IsReadOnly = true;
                RoleTextBox.IsReadOnly = true;
                CompetenceDetailsTextBox.IsReadOnly = true;
                
                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Collapsed;
            }
            else if (ramsWindowMode == EventWindowModes.ViewReply)
            {
                EventIDLabel.Content = eventHorizonRamsLINQ.ID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonRamsLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "View Reply";

                NameTextBox.IsReadOnly = true;
                RoleTextBox.IsReadOnly = true;
                CompetenceDetailsTextBox.IsReadOnly = true;

                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Collapsed;
            }
            else if (ramsWindowMode == EventWindowModes.EditMainEvent)
            {
                EventIDLabel.Content = eventHorizonRamsLINQ.ID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonRamsLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "Edit Main Method Statement";

                NameTextBox.IsReadOnly = false;
                RoleTextBox.IsReadOnly = false;
                CompetenceDetailsTextBox.IsReadOnly = false;

                AddNoteButton.Visibility = Visibility.Visible;
                ReplyButton.Visibility = Visibility.Visible;
                SaveButton.Visibility = Visibility.Visible;
            }
            else if (ramsWindowMode == EventWindowModes.EditNote)
            {
                EventIDLabel.Content = eventHorizonRamsLINQ.ID.ToString("D5"); 
                CreatedDateTimeLabel.Content = eventHorizonRamsLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "Edit Note";
                
                NameTextBox.IsReadOnly = false;
                RoleTextBox.IsReadOnly = false;
                CompetenceDetailsTextBox.IsReadOnly = false;
                
                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Visible;
            }
            else if (ramsWindowMode == EventWindowModes.EditReply)
            {
                EventIDLabel.Content = eventHorizonRamsLINQ.ID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonRamsLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "Edit Reply";
                
                NameTextBox.IsReadOnly = false;
                RoleTextBox.IsReadOnly = false;
                CompetenceDetailsTextBox.IsReadOnly = false;
                
                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Visible;
            }
            else if (ramsWindowMode == EventWindowModes.NewEvent)
            {
                EventIDLabel.Content = "-1";
                CreatedDateTimeLabel.Content = eventHorizonRamsLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "New Method Statement";
                
                NameTextBox.IsReadOnly = false;
                RoleTextBox.IsReadOnly = false;
                CompetenceDetailsTextBox.IsReadOnly = false;
                
                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Visible;
            }
            else if (ramsWindowMode == EventWindowModes.NewNote)
            {
                EventIDLabel.Content = "-1";
                CreatedDateTimeLabel.Content = eventHorizonRamsLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "New Note";
                
                NameTextBox.IsReadOnly = false;
                RoleTextBox.IsReadOnly = false;
                CompetenceDetailsTextBox.IsReadOnly = false;
                
                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Visible;
            }
            else if (ramsWindowMode == EventWindowModes.NewReply)
            {
                EventIDLabel.Content = "-1";
                CreatedDateTimeLabel.Content = eventHorizonRamsLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "New Reply";
                
                NameTextBox.IsReadOnly = false;
                RoleTextBox.IsReadOnly = false;
                CompetenceDetailsTextBox.IsReadOnly = false;
                
                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Visible;
            }

            this.Owner = Application.Current.MainWindow;
        }

        private void GetEngineer()
        { 
            NameTextBox.Text = eventHorizonEngineersLINQ.Name;
            DescriptionTextBox.Text = eventHorizonRamsLINQ.Description;
            ClientNameTextBox.Text = eventHorizonRamsLINQ.ClientName;
            SiteTextBox.Text = eventHorizonRamsLINQ.Site;
            LocationActivityTextBox.Text = eventHorizonRamsLINQ.LocationActivity;
            ElementReviewedTextBox.Text =  eventHorizonRamsLINQ.ElementReviewed;
            MSContractTitleTextBox.Text = eventHorizonRamsLINQ.MSContractTitle;

            MSRevisionNoComboBox.SelectedIndex = eventHorizonRamsLINQ.MSRevisionNo;

            MSContractorTextBox.Text = eventHorizonRamsLINQ.MSContractor;

            RamsStatusIDComboBox.SelectedIndex = eventHorizonRamsLINQ.StatusID;
        }
        
        private void SetRams()
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

        public int MSRevisionNoID = 1;

        private void MSRevisionNoComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MSRevisionNoID = RevisionNoComboBox.SelectedIndex;
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
                        EngineerWindow nevn = new EngineerWindow(mainWindow, EventWindowModes.NewNote, eventHorizonRamsLINQ, this);
                        nevn.Show();
                        nevn.Left += 30;
                        nevn.Top += 30;
                        break;
                    case EventFormCloseButtons.Reply:
                        SetRams();
                        EngineerWindow nev = new EngineerWindow(mainWindow, EventWindowModes.NewReply, eventHorizonRamsLINQ, this);
                        nev.Show();
                        nev.Left += 30;
                        nev.Top += 30;
                        break;
                    case EventFormCloseButtons.Save:
                        SetRams();
                        switch (ramsWindowMode)
                        {
                            case EventWindowModes.ViewMainEvent:
                                DataTableManagementEngineer.SaveEngineer(this, eventHorizonRamsLINQ, EventWindowModes.ViewMainEvent);
                                break;
                            case EventWindowModes.ViewNote:
                                DataTableManagementEngineer.SaveEngineer(this, eventHorizonRamsLINQ, EventWindowModes.ViewNote);
                                break;
                            case EventWindowModes.ViewReply:
                                DataTableManagementEngineer.SaveEngineer(this, eventHorizonRamsLINQ, EventWindowModes.ViewReply);
                                break;
                            case EventWindowModes.EditMainEvent:
                                DataTableManagementEngineer.SaveEngineer(this, eventHorizonRamsLINQ, EventWindowModes.EditMainEvent);
                                break;
                            case EventWindowModes.EditNote:
                                DataTableManagementEngineer.SaveEngineer(this, eventHorizonRamsLINQ, EventWindowModes.EditNote);
                                break;
                            case EventWindowModes.EditReply:
                                DataTableManagementEngineer.SaveEngineer(this, eventHorizonRamsLINQ, EventWindowModes.EditReply);
                                break;
                            case EventWindowModes.NewEvent:
                                DataTableManagementEngineer.SaveEngineer(this, eventHorizonRamsLINQ, EventWindowModes.NewEvent);
                                break;
                            case EventWindowModes.NewNote:
                                DataTableManagementEngineer.SaveEngineer(this, eventHorizonRamsLINQ, EventWindowModes.NewNote);
                                break;
                            case EventWindowModes.NewReply:
                                DataTableManagementEngineer.SaveEngineer(this, eventHorizonRamsLINQ, EventWindowModes.NewReply);
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