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
        EventHorizonEngineerLINQ eventHorizonEngineerLINQ;
        public EngineerWindow engineerWindow;

        DateTime formOpenStartTime;
        TimeSpan eventIsReadAfterTimeSpan = TimeSpan.FromSeconds(3);

        public Int32 ParentEventID;

        public EngineerWindow(MainWindow mainWindow, int ramsWindowMode, EventHorizonEngineerLINQ eventHorizonEngineerLINQ, EngineerWindow engineerWindow = null)
        {
            InitializeComponent();

            this.mainWindow = mainWindow;
            this.ramsWindowMode = ramsWindowMode;
            this.eventHorizonEngineerLINQ = (EventHorizonEngineerLINQ)eventHorizonEngineerLINQ.Clone();
            this.engineerWindow = engineerWindow;
            this.userID = eventHorizonEngineerLINQ.UserID;

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
                    GetEngineer();

                    if (userID == XMLReaderWriter.UserID || XMLReaderWriter.UserID == 1)
                        ramsWindowMode = EventWindowModes.EditNote;
                    else
                        ramsWindowMode = EventWindowModes.ViewNote;

                    break;
                case EventWindowModes.ViewReply:
                    GetEngineer();

                    if (userID == XMLReaderWriter.UserID || XMLReaderWriter.UserID == 1)
                        ramsWindowMode = EventWindowModes.EditReply;
                    else
                        ramsWindowMode = EventWindowModes.ViewReply;

                    break;
                case EventWindowModes.EditMainEvent:
                    GetEngineer();

                    if (userID == XMLReaderWriter.UserID || XMLReaderWriter.UserID == 1)
                        ramsWindowMode = EventWindowModes.EditMainEvent;
                    else
                        ramsWindowMode = EventWindowModes.ViewMainEvent;

                    break;
                case EventWindowModes.EditNote:
                    GetEngineer();

                    if (userID == XMLReaderWriter.UserID || XMLReaderWriter.UserID == 1)
                        ramsWindowMode = EventWindowModes.EditNote;
                    else
                        ramsWindowMode = EventWindowModes.ViewNote;

                    break;
                case EventWindowModes.EditReply:
                    GetEngineer();

                    if (userID == XMLReaderWriter.UserID || XMLReaderWriter.UserID == 1)
                        ramsWindowMode = EventWindowModes.EditReply;
                    else
                        ramsWindowMode = EventWindowModes.ViewReply;

                    break;
                case EventWindowModes.NewEvent:
                case EventWindowModes.NewNote:
                case EventWindowModes.NewReply:
                    eventHorizonEngineerLINQ.UserID = XMLReaderWriter.UserID;
                    break;
            }

            if (ramsWindowMode == EventWindowModes.ViewMainEvent)
            {
                EventIDLabel.Content = eventHorizonEngineerLINQ.ID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonEngineerLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "View Main Engineer";

                UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonEngineerLINQ.UserID].Color);
                UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonEngineerLINQ.UserID);
                UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonEngineerLINQ.UserID);

                NameTextBox.IsReadOnly = true;
                RoleTextBox.IsReadOnly = true;
                CompetenceDetailsTextBox.IsReadOnly = true;

                AddNoteButton.Visibility = Visibility.Visible;
                ReplyButton.Visibility = Visibility.Visible;
                SaveButton.Visibility = Visibility.Collapsed;
            }
            else if (ramsWindowMode == EventWindowModes.ViewNote)
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
            else if (ramsWindowMode == EventWindowModes.ViewReply)
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
            else if (ramsWindowMode == EventWindowModes.EditMainEvent)
            {
                EventIDLabel.Content = eventHorizonEngineerLINQ.ID.ToString("D5");
                CreatedDateTimeLabel.Content = eventHorizonEngineerLINQ.CreationDate.ToString("dd/MM/yy HH:mm");
                EventTitleLabel.Content = "Edit Main Engineer";

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
            else if (ramsWindowMode == EventWindowModes.EditNote)
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
            else if (ramsWindowMode == EventWindowModes.EditReply)
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
            else if (ramsWindowMode == EventWindowModes.NewEvent)
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
            else if (ramsWindowMode == EventWindowModes.NewNote)
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
            else if (ramsWindowMode == EventWindowModes.NewReply)
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

            UserNameLabel.Content = MiscFunctions.GetUserNameFromUserID(XMLReaderWriter.UsersList, eventHorizonEngineerLINQ.UserID);
        }
        
        private void SetEngineer()
        {
            string NameSafeString = NameTextBox.Text.Replace("'", "''");
            string RoleSafeString = RoleTextBox.Text.Replace("'", "''");
            string CompetenceDetailsSafeString = CompetenceDetailsTextBox.Text.Replace("'", "''");

            eventHorizonEngineerLINQ.Name = NameSafeString;
            eventHorizonEngineerLINQ.Role = RoleSafeString;
            eventHorizonEngineerLINQ.CompetenceDetails = CompetenceDetailsSafeString;
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
                        switch (ramsWindowMode)
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