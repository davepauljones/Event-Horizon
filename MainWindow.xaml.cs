using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.Data;
using System.IO;
using System.Windows.Threading;
using System.Windows.Media.Effects;

using FontAwesome.WPF;

namespace The_Oracle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int EventTypeID = 0;

        public Int32 LastRecordCount = 0;

        Today t;
        Now n;
        OracleDatabaseHealth odbh;

        public static MainWindow mw;
        public static DateTime OracleDatabaseLastWriteTime = DateTime.Now;

        public static string HSE_LOG_GlobalMDBConnectionString = String.Empty;

        public delegate void OnOracleDatabaseChanged(object source, FileSystemEventArgs e);

        OracleDatabaseFileWatcher fileWatcher;

        bool JustLoaded = false;

        List<EventHorizonLINQ> EventHorizonLINQList;

        public static Dictionary<Int32, DateTime> UsersLastTimeOnlineDictionary = new Dictionary<int, DateTime>();

        private void Init_OracleDatabaseFileWatcher()
        {
            if (File.Exists(XMLReaderWriter.DatabaseLocationString + "\\EventHorizonRemoteDatabase.mdb"))
            {
                fileWatcher = new OracleDatabaseFileWatcher(XMLReaderWriter.DatabaseLocationString, OnChanged);
            }
        }
        
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (fileWatcher != null) fileWatcher.watcher.Dispose(); //Your FileSystemWatcher object
        }
        
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            RunTask();

            // Specify what is done when a file is changed, created, or deleted.
            Console.WriteLine("*************************************************************");
            Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
            Console.WriteLine("*************************************************************");
        }
        
        private async void RunTask()
        {
            System.Threading.Thread.Sleep(XMLReaderWriter.UsersRefreshTimeSpan);

            await Task.Factory.StartNew(() =>
            {
                RunningTask();

            });
        }
        
        private void RunningTask()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                odbh.UpdateLastWriteDateTime(DateTime.Now);

                if (!JustLoaded)
                {
                    if (ReminderListView.SelectedItems.Count == 0)
                    {
                        if (DisplayMode == DisplayModes.Reminders)
                            RefreshLog(ListViews.Reminder);
                        else
                            RefreshLog(ListViews.Log);

                        GetLastEntry(EventHorizonLINQList);
                    }
                }
                else
                    JustLoaded = true;
            }));
        }
        
        private void Init_RefreshTimer()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }
        
        void timer_Tick(object sender, EventArgs e)
        {
            t.SyncDate();
            n.SyncTime();

            //Check Users online every 60 seconds only executes if second is 0
            if (DateTime.Now.Second == XMLReaderWriter.UserID)//use UserID as to offset actual second used to update
            {
                DataTableManagement.InsertOrUpdateLastTimeOnline(XMLReaderWriter.UserID);
                UpdateUsersOnline();
                CheckMyUnreadAndMyReminders();
            }
        }
       
        public MainWindow()
        {
            InitializeComponent();

            mw = this;

            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\OracleBackground.jpg"))
            {
                ImageBrush myBrush = new ImageBrush();
                myBrush.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\OracleBackground.jpg", UriKind.Absolute));

                this.Background = myBrush;
                this.Background.Opacity = 0.3;
            }

            this.WindowState = WindowState.Maximized;

            EventStackPanel.Visibility = Visibility.Visible;

            if (XMLReaderWriter.CheckIf_SettingsXML_FileExists())
            {
                if (XMLReaderWriter.CheckIf_DatabaseSettingsXML_FileExists())
                {
                    String swv = System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString("dd/MM/yyyy");
            
                    HSE_LOG_GlobalMDBConnectionString = String.Empty;

                    MainWindowTitle.SetMainWindowTitle();

                    Loaded += MainWindow_Loaded;
                }
                else
                    Close();
            }
            else
                Close();
        }

        bool MainWindowIs_Loaded = false;
        
        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindowIs_Loaded = true;

            t = new Today();
            TodayGrid.Children.Add(t);

            n = new Now();
            NowGrid.Children.Add(n);

            odbh = new OracleDatabaseHealth();
            OracleDatabaseHealthGrid.Children.Add(odbh);

            DataTableManagement.SetConnectionString(HSE_LOG_GlobalMDBConnectionString);

            if (OracleDatabaseCreate.CheckIfDatabaseExists())
            {
                Init_RefreshTimer();//for clock n calander
                Init_OracleDatabaseFileWatcher();

                RefreshXML();

                CheckMyUnreadAndMyReminders();

                OracleDatabaseCreate.Create_Oracle();

                DataTableManagement.InsertOrUpdateLastTimeOnline(XMLReaderWriter.UserID);
                UpdateUsersOnline();
            }
        }
        
        public void RefreshXML()
        {
            LoadCurrentUserIntoUserStackPanel();
            LoadUsersIntoUsersStackPanel();
            AddItemsToEventTypeComboBox();
        }
        
        private void CheckMyUnreadAndMyReminders()
        {
            Int32 NotificationsAddedThisCycle = 0;
            List<EventHorizonLINQ> oel = DataTableManagement.GetMyUnread();
            Int32 notifications = oel.Count;

            foreach (EventHorizonLINQ oe in oel)
            {
                if (!OracleNotification.Notifications.ContainsKey(oe.ID))
                {
                    OracleNotification on = new OracleNotification(this, oe.ID, notifications, oel.Count, oe);
                    on.Show();

                    notifications--;

                    NotificationsAddedThisCycle++;
                }
            }

            List<EventHorizonLINQ> rmoel = DataTableManagement.GetMyReminders();
            Int32 rmnotifications = rmoel.Count;

            foreach (EventHorizonLINQ oe in rmoel)
            {
                if (!OracleNotification.Notifications.ContainsKey(oe.ID))
                {
                    OracleNotification on = new OracleNotification(this, oe.ID, rmnotifications, rmoel.Count, oe);
                    on.Show();

                    notifications--;

                    NotificationsAddedThisCycle++;
                }
            }

            if (NotificationsAddedThisCycle > 0) MiscFunctions.PlayFile("Notification.mp3");
        }

        private Int32 LastGetLastEntry = 0;

        public void GetLastEntry(List<EventHorizonLINQ> ehll, bool JustLoaded = false)
        {
            var maxValue = ehll.Max(x => x.ID);
            var result = ehll.First(x => x.ID == maxValue);

            if (LastGetLastEntry != maxValue && JustLoaded == true)
            {
                OracleBriefNotification n = new OracleBriefNotification(this, maxValue, 1, 1, result);
                n.Show();
                LastGetLastEntry = maxValue;
            }
        }
        
        public void RefreshLog(int ListViewToPopulate)
        {
            try
            {
                ReminderListView.Items.Clear();
                DataTableManagement.EventHorizon_Event.Clear();

                EventHorizonLINQList = DataTableManagement.GetEvents(ListViewToPopulate, EventTypeID, FilterMode, DisplayMode);

                foreach (EventHorizonLINQ _EventHorizonLINQ in EventHorizonLINQList)
                {
                    List<EventHorizonLINQ> Replies = DataTableManagement.GetReplies(_EventHorizonLINQ.ID);

                    _EventHorizonLINQ.Attributes_Replies = Replies.Count;

                    EventRow _EventRow = CreateEventLogRow(_EventHorizonLINQ);

                    if (_EventHorizonLINQ.EventModeID == EventModes.MainEvent)
                    {
                        ReminderListView.Items.Add(_EventRow);
                    }

                    if (_EventHorizonLINQ.Attributes_Replies > 0)
                    {
                        foreach (EventHorizonLINQ roe in Replies)
                        {
                            _EventRow.RepliesListView.Items.Add(CreateEventLogRow(roe));
                        }
                    }
                }
                Status.Content = "Reminders needing attention " + ReminderListView.Items.Count;
            }
            catch (Exception e)
            {
                Console.WriteLine("----------------------------------------");

                OracleMessagesNotification msg = new OracleMessagesNotification(MainWindow.mw, OracleMessagesNotificationModes.Custom, new OracleCustomMessage { MessageTitleTextBlock = "RefreshLog - " + e.Source, InformationTextBlock = e.Message });
                msg.ShowDialog();
            }
        }

        private EventRow CreateEventLogRow(EventHorizonLINQ _EventHorizonLINQ)
        {
            EventRow _EventRow = new EventRow(_EventHorizonLINQ);

            _EventRow.EventIDTextBlock.Text = _EventHorizonLINQ.ID.ToString("D5");

            if (_EventHorizonLINQ.EventModeID == EventModes.ReplyEvent)
            {
                _EventRow.EventTypeFontAwesomeIconBorder.Background = new SolidColorBrush(Colors.LightSeaGreen);
                _EventRow.EventTypeFontAwesomeIcon.Icon = FontAwesomeIcon.Comment;
                _EventRow.EventTypeTextBlock.Text = "";
                _EventRow.BackgroundGrid.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f7f8f9"));
                _EventRow.SourceIDGrid.Visibility = Visibility.Hidden;
            }
            else
            {
                if (_EventHorizonLINQ.EventTypeID < XMLReaderWriter.EventTypesList.Count)
                {
                    _EventRow.EventTypeFontAwesomeIconBorder.Background = new SolidColorBrush(XMLReaderWriter.EventTypesList[_EventHorizonLINQ.EventTypeID].Color);
                    _EventRow.EventTypeFontAwesomeIcon.Icon = XMLReaderWriter.EventTypesList[_EventHorizonLINQ.EventTypeID].Icon;
                    _EventRow.EventTypeTextBlock.Text = XMLReaderWriter.EventTypesList[_EventHorizonLINQ.EventTypeID].Name;
                    _EventRow.BackgroundGrid.Background = new SolidColorBrush(Colors.White);
                    _EventRow.SourceIDGrid.Visibility = Visibility.Visible;
                }
                else
                {
                    _EventRow.EventTypeFontAwesomeIconBorder.Background = new SolidColorBrush(Colors.White);
                    _EventRow.EventTypeFontAwesomeIcon.Icon = FontAwesomeIcon.Question;
                    _EventRow.EventTypeTextBlock.Text = "Error";
                    _EventRow.BackgroundGrid.Background = new SolidColorBrush(Colors.White);
                    _EventRow.SourceIDGrid.Visibility = Visibility.Visible;
                }
            }

            _EventRow.CreatedDateTimeTextBlock.Text = _EventHorizonLINQ.CreationDate.ToString("dd/MM/y HH:mm");

            if (_EventHorizonLINQ.EventModeID == EventModes.ReplyEvent)
            {
                _EventRow.SourceIDTextBlock.Text = "";
            }
            else
            {
                if (_EventHorizonLINQ.SourceID < XMLReaderWriter.SourceTypesList.Count)
                    _EventRow.SourceIDTextBlock.Text = XMLReaderWriter.SourceTypesList[_EventHorizonLINQ.SourceID].Name;
                else
                    _EventRow.SourceIDTextBlock.Text = "Error";
            }

            _EventRow.DetailsTextBlock.Text = _EventHorizonLINQ.Details;

            if (_EventHorizonLINQ.EventModeID == EventModes.ReplyEvent)
            {

            }
            else
            {
                _EventRow.FrequencyGrid.Children.Add(Frequency.GetFrequency(_EventHorizonLINQ.FrequencyID));
            }

            _EventRow.StatusGrid.Children.Add(StatusIcons.GetStatus(_EventHorizonLINQ.StatusID));

            if (_EventHorizonLINQ.UserID < XMLReaderWriter.UsersList.Count)
            {
                _EventRow.OriginUserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[_EventHorizonLINQ.UserID].Color);
                _EventRow.OriginUserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, _EventHorizonLINQ.UserID);
            }
            else
            {
                _EventRow.OriginUserEllipse.Fill = new SolidColorBrush(Colors.White);
                _EventRow.OriginUserLabel.Content = _EventHorizonLINQ.UserID;
            }

            if (_EventHorizonLINQ.TargetUserID < XMLReaderWriter.UsersList.Count)
            {
                _EventRow.TargetUserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[_EventHorizonLINQ.TargetUserID].Color);
                _EventRow.TargetUserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, _EventHorizonLINQ.TargetUserID);
            }
            else
            {
                _EventRow.TargetUserEllipse.Fill = new SolidColorBrush(Colors.White);
                _EventRow.TargetUserLabel.Content = _EventHorizonLINQ.TargetUserID;
            }

            if (_EventHorizonLINQ.TargetUserID < XMLReaderWriter.UsersList.Count)
            {
                if (_EventHorizonLINQ.TargetUserID > 0)
                    _EventRow.TargetUserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, _EventHorizonLINQ.TargetUserID);
                else
                {
                    _EventRow.TargetUserLabel.Content = "★";
                    _EventRow.TargetUserLabel.Margin = new Thickness(0, -3, 0, 0);
                    _EventRow.TargetUserLabel.FontSize = 14;
                }
            }
            else
            {
                _EventRow.TargetUserEllipse.Fill = new SolidColorBrush(Colors.White);
                _EventRow.TargetUserLabel.Content = _EventHorizonLINQ.TargetUserID;
            }

            String TotalDaysString = string.Empty;

            if (_EventHorizonLINQ.Attributes_TotalDays < 0)
            {
                if (_EventHorizonLINQ.Attributes_TotalDays < -30)
                    TotalDaysString = "30";
                else
                    TotalDaysString = Math.Abs(_EventHorizonLINQ.Attributes_TotalDays).ToString();
            }
            else
            {
                if (_EventHorizonLINQ.Attributes_TotalDays > 30)
                    TotalDaysString = "30";
                else
                    TotalDaysString = _EventHorizonLINQ.Attributes_TotalDays.ToString();
            }

            _EventRow.TotalDaysEllipse.Fill = new SolidColorBrush(_EventHorizonLINQ.Attributes_TotalDaysEllipseColor);
            _EventRow.TotalDaysLabel.Content = TotalDaysString;

            String TargetDateTimeString = _EventHorizonLINQ.TargetDate.ToString("dd/MM/y HH:mm");
            DateTime tdt = DateTime.MinValue;
            if (DateTime.TryParse(TargetDateTimeString, out tdt))
            {
                if (tdt.TimeOfDay == TimeSpan.Zero)
                    TargetDateTimeString = tdt.ToString("dd/MM/y");
                else
                    TargetDateTimeString = tdt.ToString("dd/MM/y HH:mm");
            }
            else
                Console.WriteLine("Unable to parse TargetDateTimeString '{0}'", TargetDateTimeString);

            _EventRow.TargetDateTimeTextBlock.Text = TargetDateTimeString;

            _EventRow.RepliesLabel.Content = _EventHorizonLINQ.Attributes_Replies;

            if (_EventHorizonLINQ.Attributes_Replies == 0)
            {
                _EventRow.RepliesButton.Opacity = 0.1;
                _EventRow.RepliesButton.IsEnabled = false;
            }

            _EventRow.Tag = _EventHorizonLINQ;

            return _EventRow;
        }
        
        public Dictionary<Int32, Grid> UsersOnlineStatus = new Dictionary<int, Grid>();

        private void LoadCurrentUserIntoUserStackPanel()
        {
            try
            {
                UserStackPanel.Children.Clear();

                if (XMLReaderWriter.UsersList[XMLReaderWriter.UserID] != null)
                {
                    User u = XMLReaderWriter.UsersList[XMLReaderWriter.UserID];

                    Grid OriginUserIconEllipseGrid;
                    Ellipse OriginUserIconEllipse;

                    Color IconEllipseColor = Colors.White;

                    IconEllipseColor = XMLReaderWriter.UsersList[u.ID].Color;

                    if (u.ID > 0)
                        OriginUserIconEllipse = new Ellipse { Width = 24, Height = 24, Fill = new SolidColorBrush(IconEllipseColor), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };
                    else
                        OriginUserIconEllipse = new Ellipse { Width = 24, Height = 24, Fill = new SolidColorBrush(IconEllipseColor), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };

                    OriginUserIconEllipseGrid = new Grid { Margin = new Thickness(3, 1, 3, 3), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };

                    OriginUserIconEllipseGrid.Children.Add(OriginUserIconEllipse);

                    Label OriginUserIconEllipseLabel;

                    if (u.ID > 0)
                        OriginUserIconEllipseLabel = new Label { Content = MiscFunctions.GetFirstCharsOfString(u.UserName), Foreground = Brushes.Black, FontSize = 10, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };
                    else
                        OriginUserIconEllipseLabel = new Label { Content = "★", Foreground = Brushes.Black, FontSize = 14, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, -3, 0, 0), MaxHeight = 24, Padding = new Thickness(0) };

                    OriginUserIconEllipseGrid.Children.Add(OriginUserIconEllipseLabel);

                    OriginUserIconEllipseGrid.Opacity = 1;

                    OriginUserIconEllipseGrid.Effect = new DropShadowEffect
                    {
                        Color = new Color { A = 255, R = 0, G = 0, B = 0 },
                        Direction = 320,
                        ShadowDepth = 1,
                        Opacity = 0.6
                    };

                    Label UsersName = new Label { Content = u.UserName, Foreground = Brushes.Black, FontSize = 11, Margin = new Thickness(0, 0, 0, 0), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };

                    UserStackPanel.Children.Add(OriginUserIconEllipseGrid);
                    UserStackPanel.Children.Add(UsersName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("----------------------------------------");

                OracleMessagesNotification msg = new OracleMessagesNotification(MainWindow.mw, OracleMessagesNotificationModes.Custom, new OracleCustomMessage { MessageTitleTextBlock = "LoadCurrentUserIntoUserStackPanel - " + e.Source, InformationTextBlock = e.Message });
                msg.ShowDialog();
            }
        }

        private void LoadUsersIntoUsersStackPanel()
        {
            try
            {
                UsersColumn1StackPanel.Children.Clear();
                UsersColumn2StackPanel.Children.Clear();
                UsersColumn3StackPanel.Children.Clear();
                UsersColumn4StackPanel.Children.Clear();
                UsersColumn5StackPanel.Children.Clear();

                int i = 1;
                foreach (User u in XMLReaderWriter.UsersList)
                {
                    Grid OriginUserIconEllipseGrid;
                    Ellipse OriginUserIconEllipse;

                    Color IconEllipseColor = Colors.White;

                    IconEllipseColor = XMLReaderWriter.UsersList[u.ID].Color;

                    if (u.ID > 0)
                        OriginUserIconEllipse = new Ellipse { Width = 24, Height = 24, Fill = new SolidColorBrush(IconEllipseColor), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };
                    else
                        OriginUserIconEllipse = new Ellipse { Width = 24, Height = 24, Fill = new SolidColorBrush(IconEllipseColor), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };

                    OriginUserIconEllipseGrid = new Grid { Margin = new Thickness(3, 1, 3, 3), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };

                    OriginUserIconEllipseGrid.Children.Add(OriginUserIconEllipse);

                    Label OriginUserIconEllipseLabel;

                    if (u.ID > 0)
                        OriginUserIconEllipseLabel = new Label { Content = MiscFunctions.GetFirstCharsOfString(u.UserName), Foreground = Brushes.Black, FontSize = 10, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };
                    else
                        OriginUserIconEllipseLabel = new Label { Content = "★", Foreground = Brushes.Black, FontSize = 14, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, -3, 0, 0), MaxHeight = 24, Padding = new Thickness(0) };

                    OriginUserIconEllipseGrid.Children.Add(OriginUserIconEllipseLabel);

                    OriginUserIconEllipseGrid.Opacity = 0.3;
                    UsersOnlineStatus.Add(u.ID, OriginUserIconEllipseGrid);

                    OriginUserIconEllipseGrid.Effect = new DropShadowEffect
                    {
                        Color = new Color { A = 255, R = 0, G = 0, B = 0 },
                        Direction = 320,
                        ShadowDepth = 1,
                        Opacity = 0.6
                    };

                    if (i < 4)
                        UsersColumn1StackPanel.Children.Add(OriginUserIconEllipseGrid);
                    else if (i > 3 && i < 7)
                        UsersColumn2StackPanel.Children.Add(OriginUserIconEllipseGrid);
                    else if (i > 6 && i < 10)
                        UsersColumn3StackPanel.Children.Add(OriginUserIconEllipseGrid);
                    else if (i > 9 && i < 13)
                        UsersColumn4StackPanel.Children.Add(OriginUserIconEllipseGrid);
                    else if (i > 12 && i < 16)
                        UsersColumn5StackPanel.Children.Add(OriginUserIconEllipseGrid);

                    i++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("----------------------------------------");

                OracleMessagesNotification msg = new OracleMessagesNotification(MainWindow.mw, OracleMessagesNotificationModes.Custom, new OracleCustomMessage { MessageTitleTextBlock = "LoadUsersIntoUsersStackPanel - " + e.Source, InformationTextBlock = e.Message });
                msg.ShowDialog();
            }
        }

        private void UpdateUsersOnline()
        {
            DataTableManagement.GetUsersLastTimeOnline();

            foreach (User u in XMLReaderWriter.UsersList)
            {
                if (UsersOnlineStatus.ContainsKey(u.ID))
                {
                    //Check if user was still online a minute ago, if so refresh user icon
                    if (UsersLastTimeOnlineDictionary.ContainsKey(u.ID))
                    {
                        if (UsersLastTimeOnlineDictionary[u.ID] > (DateTime.Now - TimeSpan.FromMinutes(2)))
                        {
                            Grid UsersIconGrid = UsersOnlineStatus[u.ID];
                            UsersIconGrid.Opacity = 1;
                        }
                        else
                        {
                            Grid UsersIconGrid = UsersOnlineStatus[u.ID];
                            UsersIconGrid.Opacity = 0.3;
                        }
                    }
                }
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F1:
                    EventWindow nev = new EventWindow(this, new EventHorizonLINQ
                    {
                        Source_ID = -1,
                        Source_Mode = EventWindowModes.NewEvent,
                        Attributes_TotalDays = 0,
                        ID = -1
                    }, null); 
                    nev.Show();
                    break;
                case Key.F2:
                    if (EventLogListViewTagged > 0)
                    {
                        EventWindow eev = new EventWindow(this, new EventHorizonLINQ
                        {
                            Source_ID = EventLogListViewTagged,
                            Source_Mode = EventWindowModes.EditEvent,
                            Attributes_TotalDays = 0,
                            ID = EventLogListViewTagged
                        }, null);
                        eev.Show();
                    }
                    break;
                case Key.F3:
                    if (XMLReaderWriter.EventTypesList.Count > 1)
                    {
                        EventWindow noevF3 = new EventWindow(this, new EventHorizonLINQ
                        {
                            Source_ID = -1,
                            Source_Mode = EventWindowModes.NewEvent,
                            Attributes_TotalDays = 0,
                            ID = -1,
                            EventTypeID = XMLReaderWriter.EventTypesList[1].ID,
                        }, null);
                        noevF3.Show();
                    }
                    break;
                case Key.F4:
                    if (XMLReaderWriter.EventTypesList.Count > 2)
                    {
                        EventWindow noevF4 = new EventWindow(this, new EventHorizonLINQ
                        {
                            Source_ID = -1,
                            Source_Mode = EventWindowModes.NewEvent,
                            Attributes_TotalDays = 0,
                            ID = -1,
                            EventTypeID = XMLReaderWriter.EventTypesList[2].ID,
                        }, null);
                        noevF4.Show();
                    }
                    break;
                case Key.F5:
                    if (XMLReaderWriter.EventTypesList.Count > 3)
                    {
                        EventWindow noevF5 = new EventWindow(this, new EventHorizonLINQ
                        {
                            Source_ID = -1,
                            Source_Mode = EventWindowModes.NewEvent,
                            Attributes_TotalDays = 0,
                            ID = -1,
                            EventTypeID = XMLReaderWriter.EventTypesList[3].ID,
                        }, null);
                        noevF5.Show();
                    }
                    break;
                case Key.F6:
                    if (XMLReaderWriter.EventTypesList.Count > 4)
                    {
                        EventWindow noevF6 = new EventWindow(this, new EventHorizonLINQ
                        {
                            Source_ID = -1,
                            Source_Mode = EventWindowModes.NewEvent,
                            Attributes_TotalDays = 0,
                            ID = -1,
                            EventTypeID = XMLReaderWriter.EventTypesList[4].ID,
                        }, null);
                        noevF6.Show();
                    }
                    break;
                case Key.F7:
                    if (XMLReaderWriter.EventTypesList.Count > 5)
                    {
                        EventWindow noevF7 = new EventWindow(this, new EventHorizonLINQ
                        {
                            Source_ID = -1,
                            Source_Mode = EventWindowModes.NewEvent,
                            Attributes_TotalDays = 0,
                            ID = -1,
                            EventTypeID = XMLReaderWriter.EventTypesList[5].ID,
                        }, null);
                        noevF7.Show();
                    }
                    break;
                case Key.F8:
                    if (XMLReaderWriter.EventTypesList.Count > 6)
                    {
                        EventWindow noevF8 = new EventWindow(this, new EventHorizonLINQ
                        {
                            Source_ID = -1,
                            Source_Mode = EventWindowModes.NewEvent,
                            Attributes_TotalDays = 0,
                            ID = -1,
                            EventTypeID = XMLReaderWriter.EventTypesList[6].ID,
                        }, null);
                        noevF8.Show();
                    }
                    break;
                case Key.F9:
                    if (XMLReaderWriter.EventTypesList.Count > 7)
                    {
                        EventWindow noevF9 = new EventWindow(this, new EventHorizonLINQ
                        {
                            Source_ID = -1,
                            Source_Mode = EventWindowModes.NewEvent,
                            Attributes_TotalDays = 0,
                            ID = -1,
                            EventTypeID = XMLReaderWriter.EventTypesList[7].ID,
                        }, null);
                        noevF9.Show();
                    }
                    break;
                case Key.System:
                    if (e.SystemKey == Key.F10)
                    {
                        if (XMLReaderWriter.EventTypesList.Count > 8)
                        {
                            EventWindow noevF10 = new EventWindow(this, new EventHorizonLINQ
                            {
                                Source_ID = -1,
                                Source_Mode = EventWindowModes.NewEvent,
                                Attributes_TotalDays = 0,
                                ID = -1,
                                EventTypeID = XMLReaderWriter.EventTypesList[8].ID,
                            }, null);
                            noevF10.Show();
                        }
                    }
                    break;
                case Key.F11:
                    if (XMLReaderWriter.EventTypesList.Count > 9)
                    {
                        EventWindow noevF11 = new EventWindow(this, new EventHorizonLINQ
                        {
                            Source_ID = -1,
                            Source_Mode = EventWindowModes.NewEvent,
                            Attributes_TotalDays = 0,
                            ID = -1,
                            EventTypeID = XMLReaderWriter.EventTypesList[9].ID,
                        }, null);
                        noevF11.Show();
                    }
                    break;
                case Key.F12:
                    if (XMLReaderWriter.EventTypesList.Count > 10)
                    {
                        EventWindow noevF12 = new EventWindow(this, new EventHorizonLINQ
                        {
                            Source_ID = -1,
                            Source_Mode = EventWindowModes.NewEvent,
                            Attributes_TotalDays = 0,
                            ID = -1,
                            EventTypeID = XMLReaderWriter.EventTypesList[10].ID,
                        }, null);
                        noevF12.Show();
                    }
                    break;
                case Key.Delete:
                    if (EventLogListViewTagged > 0)
                    {
                        var Result = MessageBox.Show("Are you sure", "Delete this event", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                        if (Result == MessageBoxResult.Yes)
                        {
                            if (EventLogListViewTagged > 0) DataTableManagement.DeleteEvent(EventLogListViewTagged);
                        }
                    }
                    break;
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
                        EventWindow nev = new EventWindow(this, new EventHorizonLINQ
                        {
                            Source_ID = -1,
                            Source_Mode = EventWindowModes.NewEvent,
                            Attributes_TotalDays = 0,
                            ID = -1,
                            EventTypeID = XMLReaderWriter.EventTypesList[0].ID,
                        }, null);
                        nev.Show();
                        break;
                    case 1:
                        if (EventLogListViewTagged > 0)
                        {
                            EventWindow eev = new EventWindow(this, new EventHorizonLINQ
                            {
                                Source_ID = EventLogListViewTagged,
                                Source_Mode = EventWindowModes.EditEvent,
                                Attributes_TotalDays = 0,
                                ID = EventLogListViewTagged,
                            }, null);
                            eev.Show();
                        }
                        break;
                    case 2:
                        if (XMLReaderWriter.EventTypesList.Count > 1)
                        {
                            EventWindow noevF3 = new EventWindow(this, new EventHorizonLINQ
                            {
                                Source_ID = -1,
                                Source_Mode = EventWindowModes.NewEvent,
                                Attributes_TotalDays = 0,
                                ID = -1,
                                EventTypeID = XMLReaderWriter.EventTypesList[1].ID,
                            }, null);
                            noevF3.Show();
                        }
                        break;
                    case 3:
                        if (XMLReaderWriter.EventTypesList.Count > 2)
                        {
                            EventWindow noevF4 = new EventWindow(this, new EventHorizonLINQ
                            {
                                Source_ID = -1,
                                Source_Mode = EventWindowModes.NewEvent,
                                Attributes_TotalDays = 0,
                                ID = -1,
                                EventTypeID = XMLReaderWriter.EventTypesList[2].ID,
                            }, null);
                            noevF4.Show();
                        }
                        break;
                    case 4:
                        if (XMLReaderWriter.EventTypesList.Count > 3)
                        {
                            EventWindow noevF5 = new EventWindow(this, new EventHorizonLINQ
                            {
                                Source_ID = -1,
                                Source_Mode = EventWindowModes.NewEvent,
                                Attributes_TotalDays = 0,
                                ID = -1,
                                EventTypeID = XMLReaderWriter.EventTypesList[3].ID,
                            }, null);
                            noevF5.Show();
                        }
                        break;
                    case 5:
                        if (XMLReaderWriter.EventTypesList.Count > 4)
                        {
                            EventWindow noevF6 = new EventWindow(this, new EventHorizonLINQ
                            {
                                Source_ID = -1,
                                Source_Mode = EventWindowModes.NewEvent,
                                Attributes_TotalDays = 0,
                                ID = -1,
                                EventTypeID = XMLReaderWriter.EventTypesList[4].ID,
                            }, null);
                            noevF6.Show();
                        }
                        break;
                    case 6:
                        if (XMLReaderWriter.EventTypesList.Count > 5)
                        {
                            EventWindow noevF7 = new EventWindow(this, new EventHorizonLINQ
                            {
                                Source_ID = -1,
                                Source_Mode = EventWindowModes.NewEvent,
                                Attributes_TotalDays = 0,
                                ID = -1,
                                EventTypeID = XMLReaderWriter.EventTypesList[5].ID,
                            }, null);
                            noevF7.Show();
                        }
                        break;
                    case 7:
                        if (XMLReaderWriter.EventTypesList.Count > 6)
                        {
                            EventWindow noevF8 = new EventWindow(this, new EventHorizonLINQ
                            {
                                Source_ID = -1,
                                Source_Mode = EventWindowModes.NewEvent,
                                Attributes_TotalDays = 0,
                                ID = -1,
                                EventTypeID = XMLReaderWriter.EventTypesList[6].ID,
                            }, null);
                            noevF8.Show();
                        }
                        break;
                    case 8:
                        if (XMLReaderWriter.EventTypesList.Count > 7)
                        {
                            EventWindow noevF9 = new EventWindow(this, new EventHorizonLINQ
                            {
                                Source_ID = -1,
                                Source_Mode = EventWindowModes.NewEvent,
                                Attributes_TotalDays = 0,
                                ID = -1,
                                EventTypeID = XMLReaderWriter.EventTypesList[7].ID,
                            }, null);
                            noevF9.Show();
                        }
                        break;
                    case 9:
                        if (XMLReaderWriter.EventTypesList.Count > 8)
                        {
                            EventWindow noevF10 = new EventWindow(this, new EventHorizonLINQ
                            {
                                Source_ID = -1,
                                Source_Mode = EventWindowModes.NewEvent,
                                Attributes_TotalDays = 0,
                                ID = -1,
                                EventTypeID = XMLReaderWriter.EventTypesList[8].ID,
                            }, null);
                            noevF10.Show();
                        }
                        break;
                    case 10:
                        if (XMLReaderWriter.EventTypesList.Count > 9)
                        {
                            EventWindow noevF11 = new EventWindow(this, new EventHorizonLINQ
                            {
                                Source_ID = -1,
                                Source_Mode = EventWindowModes.NewEvent,
                                Attributes_TotalDays = 0,
                                ID = -1,
                                EventTypeID = XMLReaderWriter.EventTypesList[9].ID,
                            }, null);
                            noevF11.Show();
                        }
                        break;
                    case 11:
                        if (XMLReaderWriter.EventTypesList.Count > 10)
                        {
                            EventWindow noevF12 = new EventWindow(this, new EventHorizonLINQ
                            {
                                Source_ID = -1,
                                Source_Mode = EventWindowModes.NewEvent,
                                Attributes_TotalDays = 0,
                                ID = -1,
                                EventTypeID = XMLReaderWriter.EventTypesList[10].ID,
                            }, null);
                            noevF12.Show();
                        }
                        //OracleBriefNotification n = new OracleBriefNotification(this, 99, 1, 1 );
                        //n.Show();
                        break;
                }
            }
        }

        public Int32 EventLogListViewTagged = -1;
        
        private void EventLogListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is StackPanel))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            StackPanel item = (StackPanel)dep;

            Console.Write("item = ");
            Console.WriteLine(item.Tag);

            EventLogListViewTagged = Convert.ToInt32(item.Tag);
        }

        private void AddItemsToEventTypeComboBox()
        {
            try
            {
                EventTypeComboBox.ItemsSource = (from c in XMLReaderWriter.EventTypesList select new { c.Name }).Distinct().ToList();
                EventTypeComboBox.DisplayMemberPath = "Name";

                EventTypeComboBox.SelectedIndex = 0;
            }
            catch (Exception e)
            {
                Console.WriteLine("----------------------------------------");

                OracleMessagesNotification msg = new OracleMessagesNotification(MainWindow.mw, OracleMessagesNotificationModes.Custom, new OracleCustomMessage { MessageTitleTextBlock = "AddItemsToEventTypeComboBox - " + e.Source, InformationTextBlock = e.Message });
                msg.ShowDialog();
            }
        }

        public TimeSpan ReminderListTimeSpan = new TimeSpan(1, 0, 0, 0);

        private void TimeSpanRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = e.OriginalSource as RadioButton;

            int ButtonID = 0;

            bool success = Int32.TryParse(radioButton.Tag.ToString(), out ButtonID);

            if (radioButton != null && success)
            {
                switch (ButtonID)
                {
                    case ReminderListTimeSpans.OverDue:
                        ReminderListTimeSpan = new TimeSpan(0, 0, 0, 0);
                        break;
                    case ReminderListTimeSpans.TimeSpan_1to3_Day:
                        ReminderListTimeSpan = new TimeSpan(4, 0, 0, 0);
                        break;
                    case ReminderListTimeSpans.TimeSpan_4to7_Days:
                        ReminderListTimeSpan = new TimeSpan(8, 0, 0, 0);
                        break;
                    case ReminderListTimeSpans.TimeSpan_8to14_Days:
                        ReminderListTimeSpan = new TimeSpan(15, 0, 0, 0);
                        break;
                    case ReminderListTimeSpans.TimeSpan_15to28_Days:
                        ReminderListTimeSpan = new TimeSpan(29, 0, 0, 0);
                        break;
                }
            }

            if (MainWindowIs_Loaded)
            {
                if (DisplayMode == DisplayModes.Reminders)
                    RefreshLog(ListViews.Reminder);
                else
                    RefreshLog(ListViews.Log);

                ReminderListScrollViewer.ScrollToTop();
            }
        }

        private void ReminderListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is EventRow))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            EventRow item = (EventRow)dep;

            EventHorizonLINQ oe = (EventHorizonLINQ)item.Tag;
            EventLogListViewTagged = Convert.ToInt32(oe.ID);

            Console.Write("item.Tag oe.ID = ");
            Console.WriteLine(oe.ID);

            ReminderListView.SelectedItem = item;

            if (EventLogListViewTagged > 0)
            {
                oe.Source_ID = EventLogListViewTagged;
                oe.ID = EventLogListViewTagged;
                oe.Source_Mode = EventWindowModes.EditEvent;

                //try open event as EditEvent
                EventWindow eev = new EventWindow(this, oe, null);
                eev.Show();
            }
        }

        private void EventLogListView_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is StackPanel))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            StackPanel item = (StackPanel)dep;

            Console.Write("item = ");
            Console.WriteLine(item.Tag);

            EventLogListViewTagged = Convert.ToInt32(item.Tag);

            if (EventLogListViewTagged > 0)
            {
                EventWindow eev = new EventWindow(this, new EventHorizonLINQ
                {
                    Source_ID = EventLogListViewTagged,
                    Source_Mode = EventWindowModes.EditEvent,
                    Source_ParentEventID = SelectedParentEventID,
                    Attributes_TotalDays = 0,
                    ID = EventLogListViewTagged,
                }, null);
                eev.Show();
            }
        }

        public int FilterMode = FilterModes.None;
        
        private void FilterModeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = e.OriginalSource as RadioButton;

            int ButtonID = 0;

            bool success = Int32.TryParse(radioButton.Tag.ToString(), out ButtonID);

            if (radioButton != null && success)
            {
                switch (ButtonID)
                {
                    case FilterModes.None:
                        FilterMode = FilterModes.None;
                        break;
                    case FilterModes.OriginIsMe:
                        FilterMode = FilterModes.OriginIsMe;
                        break;
                    case FilterModes.OriginOrTargetIsMe:
                        FilterMode = FilterModes.OriginOrTargetIsMe;
                        break;
                    case FilterModes.OriginAndTargetIsMe:
                        FilterMode = FilterModes.OriginAndTargetIsMe;
                        break;
                }
            }

            Console.Write("FilterMode = ");
            Console.WriteLine(FilterMode);

            if (MainWindowIs_Loaded)
            {
                if (DisplayMode == DisplayModes.Reminders)
                    RefreshLog(ListViews.Reminder);
                else
                    RefreshLog(ListViews.Log);

                ReminderListScrollViewer.ScrollToTop();
            }
        }

        public int DisplayMode = DisplayModes.Reminders;
        
        private void DisplayModeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = e.OriginalSource as RadioButton;

            if (MainWindowIs_Loaded)
            {

                int ButtonID = 0;

                bool success = Int32.TryParse(radioButton.Tag.ToString(), out ButtonID);

                if (radioButton != null && success)
                {
                    switch (ButtonID)
                    {
                        case DisplayModes.Normal:
                            DisplayMode = DisplayModes.Normal;
                            break;
                        case DisplayModes.Reminders:
                            DisplayMode = DisplayModes.Reminders;
                            break;
                    }
                }

                Console.Write("DisplayMode = ");
                Console.WriteLine(DisplayMode);

                if (MainWindowIs_Loaded)
                {
                    if (DisplayMode == DisplayModes.Reminders)
                        RefreshLog(ListViews.Reminder);
                    else
                        RefreshLog(ListViews.Log);
                }
            }
        }

        private void EventTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EventTypeID = EventTypeComboBox.SelectedIndex;

            Console.Write("EventTypeComboBox is ");
            Console.WriteLine(EventTypeID);

            if (MainWindowIs_Loaded)
            {
                if (DisplayMode == DisplayModes.Reminders)
                    RefreshLog(ListViews.Reminder);
                else
                    RefreshLog(ListViews.Log);

                ReminderListScrollViewer.ScrollToTop();
            }
        }

        private void All_ButtonClick(object sender, RoutedEventArgs e)
        {
            EventTypeComboBox.SelectedIndex = 0;
            EventTypeID = EventTypeComboBox.SelectedIndex;

            if (MainWindowIs_Loaded)
            {
                if (DisplayMode == DisplayModes.Reminders)
                    RefreshLog(ListViews.Reminder);
                else
                    RefreshLog(ListViews.Log);

                ReminderListScrollViewer.ScrollToTop();
            }
        }

        public Int32 SelectedParentEventID = 0;
        
        private void ReminderListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is EventRow))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            EventRow item = (EventRow)dep;

            EventHorizonLINQ oe = (EventHorizonLINQ)item.Tag;
            EventLogListViewTagged = Convert.ToInt32(oe.ID);

            SelectedParentEventID = Convert.ToInt32(oe.Source_ParentEventID);

            Console.Write("** ReminderListView.SelectedIndex = ");
            Console.WriteLine(ReminderListView.SelectedIndex);
            Console.Write("** item.Tag oe.ID = ");
            Console.WriteLine(oe.ID);
            Console.Write("** item.Tag oe.Source_ParentEventID = ");
            Console.WriteLine(oe.Source_ParentEventID);
        }
    }
}