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
        int engineerWindowMode;
        int userID;
        EventHorizonEngineerLINQ eventHorizonEngineerLINQ;
        public EngineerWindow engineerWindow;

        DateTime formOpenStartTime;
        TimeSpan eventIsReadAfterTimeSpan = TimeSpan.FromSeconds(3);

        public Int32 ParentEventID;

        public EngineerWindow(MainWindow mainWindow, int engineerWindowMode, EventHorizonEngineerLINQ eventHorizonEngineersLINQ, EngineerWindow engineerWindow = null)
        {
            InitializeComponent();

            this.mainWindow = mainWindow;
            this.engineerWindowMode = engineerWindowMode;
            this.eventHorizonEngineerLINQ = (EventHorizonEngineerLINQ)eventHorizonEngineersLINQ.Clone();
            this.engineerWindow = engineerWindow;
            this.userID = eventHorizonEngineerLINQ.UserID;

            formOpenStartTime = DateTime.Now;

            switch (engineerWindowMode)
            {
                case EventWindowModes.ViewMainEvent:
                    GetEngineer();

                    if (userID == XMLReaderWriter.UserID || XMLReaderWriter.UserID == 1)
                        engineerWindowMode = EventWindowModes.EditMainEvent;
                    else
                        engineerWindowMode = EventWindowModes.ViewMainEvent;

                    break;
                case EventWindowModes.ViewNote:
                    GetEngineer();

                    if (userID == XMLReaderWriter.UserID || XMLReaderWriter.UserID == 1)
                        engineerWindowMode = EventWindowModes.EditNote;
                    else
                        engineerWindowMode = EventWindowModes.ViewNote;

                    break;
                case EventWindowModes.ViewReply:
                    GetEngineer();

                    if (userID == XMLReaderWriter.UserID || XMLReaderWriter.UserID == 1)
                        engineerWindowMode = EventWindowModes.EditReply;
                    else
                        engineerWindowMode = EventWindowModes.ViewReply;

                    break;
                case EventWindowModes.EditMainEvent:
                    GetEngineer();

                    if (userID == XMLReaderWriter.UserID || XMLReaderWriter.UserID == 1)
                        engineerWindowMode = EventWindowModes.EditMainEvent;
                    else
                        engineerWindowMode = EventWindowModes.ViewMainEvent;

                    break;
                case EventWindowModes.EditNote:
                    GetEngineer();

                    if (userID == XMLReaderWriter.UserID || XMLReaderWriter.UserID == 1)
                        engineerWindowMode = EventWindowModes.EditNote;
                    else
                        engineerWindowMode = EventWindowModes.ViewNote;

                    break;
                case EventWindowModes.EditReply:
                    GetEngineer();

                    if (userID == XMLReaderWriter.UserID || XMLReaderWriter.UserID == 1)
                        engineerWindowMode = EventWindowModes.EditReply;
                    else
                        engineerWindowMode = EventWindowModes.ViewReply;

                    break;
                case EventWindowModes.NewEvent:
                case EventWindowModes.NewNote:
                case EventWindowModes.NewReply:
                    eventHorizonEngineerLINQ.UserID = XMLReaderWriter.UserID;
                    break;
            }

            if (engineerWindowMode == EventWindowModes.ViewMainEvent)
            {
                EventIDLabel.Content = eventHorizonEngineerLINQ.ID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonEngineerLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "View Engineer";
                
                NameTextBox.IsReadOnly = true;
                RoleTextBox.IsReadOnly = true;
                CompetenceDetailsTextBox.IsReadOnly = true;

                AddNoteButton.Visibility = Visibility.Visible;
                ReplyButton.Visibility = Visibility.Visible;
                SaveButton.Visibility = Visibility.Collapsed;
            }
            else if (engineerWindowMode == EventWindowModes.ViewNote)
            {
                EventIDLabel.Content = eventHorizonEngineerLINQ.ID.ToString("D5");  
                CreatedDateTimeLabel.Content = eventHorizonEngineerLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "View Note";

                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonEngineerLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonEngineerLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonEngineerLINQ.UserID);

                NameTextBox.IsReadOnly = true;
                RoleTextBox.IsReadOnly = true;
                CompetenceDetailsTextBox.IsReadOnly = true;
                
                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Collapsed;
            }
            else if (engineerWindowMode == EventWindowModes.ViewReply)
            {
                EventIDLabel.Content = eventHorizonEngineerLINQ.ID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonEngineerLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "View Reply";

                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonEngineerLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonEngineerLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonEngineerLINQ.UserID);

                NameTextBox.IsReadOnly = true;
                RoleTextBox.IsReadOnly = true;
                CompetenceDetailsTextBox.IsReadOnly = true;

                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Collapsed;
            }
            else if (engineerWindowMode == EventWindowModes.EditMainEvent)
            {
                EventIDLabel.Content = eventHorizonEngineerLINQ.ID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonEngineerLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "Edit Engineer";

                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonEngineerLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonEngineerLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonEngineerLINQ.UserID);

                NameTextBox.IsReadOnly = false;
                RoleTextBox.IsReadOnly = false;
                CompetenceDetailsTextBox.IsReadOnly = false;

                AddNoteButton.Visibility = Visibility.Visible;
                ReplyButton.Visibility = Visibility.Visible;
                SaveButton.Visibility = Visibility.Visible;
            }
            else if (engineerWindowMode == EventWindowModes.EditNote)
            {
                EventIDLabel.Content = eventHorizonEngineerLINQ.ID.ToString("D5"); 
                CreatedDateTimeLabel.Content = eventHorizonEngineerLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "Edit Note";

                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonEngineerLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonEngineerLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonEngineerLINQ.UserID);

                NameTextBox.IsReadOnly = false;
                RoleTextBox.IsReadOnly = false;
                CompetenceDetailsTextBox.IsReadOnly = false;
                
                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Visible;
            }
            else if (engineerWindowMode == EventWindowModes.EditReply)
            {
                EventIDLabel.Content = eventHorizonEngineerLINQ.ID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonEngineerLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "Edit Reply";

                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonEngineerLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonEngineerLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonEngineerLINQ.UserID);

                NameTextBox.IsReadOnly = false;
                RoleTextBox.IsReadOnly = false;
                CompetenceDetailsTextBox.IsReadOnly = false;
                
                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Visible;
            }
            else if (engineerWindowMode == EventWindowModes.NewEvent)
            {
                EventIDLabel.Content = "-1";
                CreatedDateTimeLabel.Content = eventHorizonEngineerLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "New Engineer";

                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonEngineerLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonEngineerLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonEngineerLINQ.UserID);

                NameTextBox.IsReadOnly = false;
                RoleTextBox.IsReadOnly = false;
                CompetenceDetailsTextBox.IsReadOnly = false;
                
                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Visible;
            }
            else if (engineerWindowMode == EventWindowModes.NewNote)
            {
                EventIDLabel.Content = "-1";
                CreatedDateTimeLabel.Content = eventHorizonEngineerLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "New Note";

                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonEngineerLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonEngineerLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonEngineerLINQ.UserID);

                NameTextBox.IsReadOnly = false;
                RoleTextBox.IsReadOnly = false;
                CompetenceDetailsTextBox.IsReadOnly = false;
                
                AddNoteButton.Visibility = Visibility.Collapsed;
                ReplyButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Visible;
            }
            else if (engineerWindowMode == EventWindowModes.NewReply)
            {
                EventIDLabel.Content = "-1";
                CreatedDateTimeLabel.Content = eventHorizonEngineerLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "New Reply";

                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonEngineerLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonEngineerLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonEngineerLINQ.UserID);

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
            NameTextBox.Text = eventHorizonEngineerLINQ.Name;
            RoleTextBox.Text = eventHorizonEngineerLINQ.Role;
            CompetenceDetailsTextBox.Text = eventHorizonEngineerLINQ.CompetenceDetails;
        }
        
        private void SetEngineer()
        {
            string nameSafeString = NameTextBox.Text.Replace("'", "''");
            string roleSafeString = RoleTextBox.Text.Replace("'", "''");
            string competenceDetailsSafeString = CompetenceDetailsTextBox.Text.Replace("'", "''");
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
                        SetEngineer();
                        EngineerWindow nevn = new EngineerWindow(mainWindow, EventWindowModes.NewNote, eventHorizonEngineerLINQ, this);
                        nevn.Show();
                        nevn.Left += 30;
                        nevn.Top += 30;
                        break;
                    case EventFormCloseButtons.Reply:
                        SetEngineer();
                        EngineerWindow nev = new EngineerWindow(mainWindow, EventWindowModes.NewReply, eventHorizonEngineerLINQ, this);
                        nev.Show();
                        nev.Left += 30;
                        nev.Top += 30;
                        break;
                    case EventFormCloseButtons.Save:
                        SetEngineer();
                        switch (engineerWindowMode)
                        {
                            case EventWindowModes.ViewMainEvent:
                                DataTableManagementEngineer.SaveEngineer(this, eventHorizonEngineerLINQ, EventWindowModes.ViewMainEvent);
                                break;
                            case EventWindowModes.ViewNote:
                                DataTableManagementEngineer.SaveEngineer(this, eventHorizonEngineerLINQ, EventWindowModes.ViewNote);
                                break;
                            case EventWindowModes.ViewReply:
                                DataTableManagementEngineer.SaveEngineer(this, eventHorizonEngineerLINQ, EventWindowModes.ViewReply);
                                break;
                            case EventWindowModes.EditMainEvent:
                                DataTableManagementEngineer.SaveEngineer(this, eventHorizonEngineerLINQ, EventWindowModes.EditMainEvent);
                                break;
                            case EventWindowModes.EditNote:
                                DataTableManagementEngineer.SaveEngineer(this, eventHorizonEngineerLINQ, EventWindowModes.EditNote);
                                break;
                            case EventWindowModes.EditReply:
                                DataTableManagementEngineer.SaveEngineer(this, eventHorizonEngineerLINQ, EventWindowModes.EditReply);
                                break;
                            case EventWindowModes.NewEvent:
                                DataTableManagementEngineer.SaveEngineer(this, eventHorizonEngineerLINQ, EventWindowModes.NewEvent);
                                break;
                            case EventWindowModes.NewNote:
                                DataTableManagementEngineer.SaveEngineer(this, eventHorizonEngineerLINQ, EventWindowModes.NewNote);
                                break;
                            case EventWindowModes.NewReply:
                                DataTableManagementEngineer.SaveEngineer(this, eventHorizonEngineerLINQ, EventWindowModes.NewReply);
                                break;
                        }
                        break;
                }
            }
        }
    }
}