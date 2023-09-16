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

        private Today today;
        private Now now;
        private OracleDatabaseHealth oracleDatabaseHealth;

        public static MainWindow mw;
        public static DateTime OracleDatabaseLastWriteTime = DateTime.Now;

        public delegate void OnOracleDatabaseChanged(object source, FileSystemEventArgs e);

        private OracleDatabaseFileWatcher fileWatcher;

        private bool justLoaded = false;

        private List<EventHorizonLINQ> EventHorizonLINQList;

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
                oracleDatabaseHealth.UpdateLastWriteDateTime(DateTime.Now);

                 if (ReminderListView.SelectedItems.Count == 0)
                 {
                    if (DisplayMode == DisplayModes.Reminders)
                        RefreshLog(ListViews.Reminder);
                    else
                        RefreshLog(ListViews.Log);

                    GetLastEntry(EventHorizonLINQList, justLoaded);
                 }

                 justLoaded = true;
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
            today.SyncDate();
            now.SyncTime();

            //Check Users online every 60 seconds only executes if second is 0
            if (DateTime.Now.Second == XMLReaderWriter.UserID * 6)//use UserID as to offset actual second used to update
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

            Welcome welcome = new Welcome();
            
            if (welcome.ShowDialog() == true)
            {
                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\OracleBackground.jpg"))
                {
                    ImageBrush myBrush = new ImageBrush();
                    myBrush.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\OracleBackground.jpg", UriKind.Absolute));

                    this.Background = myBrush;
                    this.Background.Opacity = 0.3;
                }

                this.WindowState = WindowState.Maximized;

                EventStackPanel.Visibility = Visibility.Visible;

                XMLReaderWriter.GlobalConnectionString = string.Empty;

                MainWindowTitle.SetMainWindowTitle();

                Loaded += MainWindow_Loaded;
            }
            else
                Close();
        }

        bool MainWindowIs_Loaded = false;
        
        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            today = new Today();
            TodayGrid.Children.Add(today);

            now = new Now();
            NowGrid.Children.Add(now);

            oracleDatabaseHealth = new OracleDatabaseHealth();
            OracleDatabaseHealthGrid.Children.Add(oracleDatabaseHealth);

            XMLReaderWriter.SetDatabaseConnectionString();

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

            MainWindowIs_Loaded = true;
        }
        
        public void RefreshXML()
        {
            LoadCurrentUserIntoGrid(CurrentUserGrid);
            LoadUsersIntoUsersStackPanel();
            AddItemsToEventTypeComboBox();
        }
        
        private void CheckMyUnreadAndMyReminders()
        {
            Int32 notificationsAddedThisCycle = 0;
            List<EventHorizonLINQ> eventHorizonLINQList = DataTableManagement.GetMyUnread();
            Int32 notifications = eventHorizonLINQList.Count;

            foreach (EventHorizonLINQ eventHorizonLINQ in eventHorizonLINQList)
            {
                if (!OracleNotification.Notifications.ContainsKey(eventHorizonLINQ.ID))
                {
                    OracleNotification on = new OracleNotification(this, eventHorizonLINQ.ID, notifications, eventHorizonLINQList.Count, eventHorizonLINQ);
                    on.Show();

                    notifications--;

                    notificationsAddedThisCycle++;
                }
            }

            List<EventHorizonLINQ> EventHorizonLINQreminders = DataTableManagement.GetMyReminders();
            Int32 rmnotifications = EventHorizonLINQreminders.Count;

            foreach (EventHorizonLINQ eventHorizonLINQ in EventHorizonLINQreminders)
            {
                if (!OracleNotification.Notifications.ContainsKey(eventHorizonLINQ.ID))
                {
                    OracleNotification on = new OracleNotification(this, eventHorizonLINQ.ID, rmnotifications, EventHorizonLINQreminders.Count, eventHorizonLINQ);
                    on.Show();

                    notifications--;

                    notificationsAddedThisCycle++;
                }
            }

            if (notificationsAddedThisCycle > 0) MiscFunctions.PlayFile("Notification.mp3");
        }

        private Int32 LastGetLastEntry = 0;

        public void GetLastEntry(List<EventHorizonLINQ> eventHorizonLINQList, bool justLoaded)
        {
            try
            {
                var maxValue = eventHorizonLINQList.Max(x => x.ID);
                var result = eventHorizonLINQList.First(x => x.ID == maxValue);

                if (LastGetLastEntry < maxValue)
                {
                    if (justLoaded == true && result.UserID != XMLReaderWriter.UserID)
                    {
                        OracleBriefNotification oracleBriefNotification = new OracleBriefNotification(this, maxValue, 1, 1, result);
                        oracleBriefNotification.Show();
                    }

                    LastGetLastEntry = maxValue;
                }
            }
            catch (Exception e)
            {
                //necessary if searching a blank database
                Console.WriteLine("***** GeLastEntry *****");
                Console.WriteLine(e);
            }
        }
        
        public void RefreshLog(int listViewToPopulate)
        {
            try
            {
                ReminderListView.Items.Clear();
                DataTableManagement.EventHorizon_Event.Clear();

                EventHorizonLINQList = DataTableManagement.GetEvents(listViewToPopulate, EventTypeID, FilterMode, DisplayMode, SearchTextBox.Text);

                foreach (EventHorizonLINQ eventHorizonLINQ in EventHorizonLINQList)
                {
                    List<EventHorizonLINQ> eventHorizonLINQRepliesList = DataTableManagement.GetReplies(eventHorizonLINQ.ID);

                    eventHorizonLINQ.Attributes_Replies = eventHorizonLINQRepliesList.Count;

                    EventRow eventRow = CreateEventLogRow(eventHorizonLINQ);

                    if (eventHorizonLINQ.EventModeID == EventModes.MainEvent)
                    {
                        ReminderListView.Items.Add(eventRow);
                    }

                    if (eventHorizonLINQ.Attributes_Replies > 0)
                    {
                        foreach (EventHorizonLINQ eventHorizonLINQRow in eventHorizonLINQRepliesList)
                        {
                            eventRow.RepliesListView.Items.Add(CreateEventLogRow(eventHorizonLINQRow));
                        }
                    }
                }
                Status.Content = "Reminders needing attention " + ReminderListView.Items.Count;
            }
            catch (Exception e)
            {
                Console.WriteLine("----------------------------------------");

                OracleRequesterNotification msg = new OracleRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "RefreshLog - " + e.Source, InformationTextBlock = e.Message }, RequesterTypes.OK);
                msg.ShowDialog();
            }
        }

        private EventRow CreateEventLogRow(EventHorizonLINQ eventHorizonLINQ)
        {
            EventRow eventRow = new EventRow(eventHorizonLINQ);

            eventRow.EventIDTextBlock.Text = eventHorizonLINQ.ID.ToString("D5");

            if (eventHorizonLINQ.EventModeID == EventModes.NoteEvent)
            {
                eventRow.EventTypeFontAwesomeIconBorder.Background = new SolidColorBrush(Colors.LightSlateGray);
                eventRow.EventTypeFontAwesomeIcon.Icon = FontAwesomeIcon.StickyNote;
                eventRow.EventTypeTextBlock.Text = "Note";
                eventRow.BackgroundGrid.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f7f8f9"));
                eventRow.SourceIDGrid.Visibility = Visibility.Hidden;
            }
            else if (eventHorizonLINQ.EventModeID == EventModes.ReplyEvent)
            {
                eventRow.EventTypeFontAwesomeIconBorder.Background = new SolidColorBrush(Colors.LightSlateGray);
                eventRow.EventTypeFontAwesomeIcon.Icon = FontAwesomeIcon.Exchange;
                eventRow.EventTypeTextBlock.Text = "Reply";
                eventRow.BackgroundGrid.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f7f8f9"));
                eventRow.SourceIDGrid.Visibility = Visibility.Hidden;
            }
            else
            {
                if (eventHorizonLINQ.EventTypeID < XMLReaderWriter.EventTypesList.Count)
                {
                    eventRow.EventTypeFontAwesomeIconBorder.Background = new SolidColorBrush(XMLReaderWriter.EventTypesList[eventHorizonLINQ.EventTypeID].Color);
                    eventRow.EventTypeFontAwesomeIcon.Icon = XMLReaderWriter.EventTypesList[eventHorizonLINQ.EventTypeID].Icon;
                    eventRow.EventTypeTextBlock.Text = XMLReaderWriter.EventTypesList[eventHorizonLINQ.EventTypeID].Name;
                    eventRow.BackgroundGrid.Background = new SolidColorBrush(Colors.White);
                    eventRow.SourceIDGrid.Visibility = Visibility.Visible;
                }
                else
                {
                    eventRow.EventTypeFontAwesomeIconBorder.Background = new SolidColorBrush(Colors.White);
                    eventRow.EventTypeFontAwesomeIcon.Icon = FontAwesomeIcon.Question;
                    eventRow.EventTypeTextBlock.Text = "Error";
                    eventRow.BackgroundGrid.Background = new SolidColorBrush(Colors.White);
                    eventRow.SourceIDGrid.Visibility = Visibility.Visible;
                }
            }

            eventRow.CreatedDateTimeTextBlock.Text = eventHorizonLINQ.CreationDate.ToString("dd/MM/y HH:mm");

            if (eventHorizonLINQ.EventModeID == EventModes.NoteEvent || eventHorizonLINQ.EventModeID == EventModes.ReplyEvent)
            {
                eventRow.SourceIDTextBlock.Text = "";
            }
            else
            {
                if (eventHorizonLINQ.SourceID < XMLReaderWriter.SourceTypesList.Count)
                {
                    eventRow.SourceIDEllipse.Fill = new SolidColorBrush(XMLReaderWriter.SourceTypesList[eventHorizonLINQ.SourceID].Color);
                    eventRow.SourceIDLabel.Content = MiscFunctions.GetFirstCharsOfString(XMLReaderWriter.SourceTypesList[eventHorizonLINQ.SourceID].Name);
                    eventRow.SourceIDTextBlock.Text = XMLReaderWriter.SourceTypesList[eventHorizonLINQ.SourceID].Name;
                }
                else
                {
                    eventRow.SourceIDEllipse.Fill = new SolidColorBrush(Colors.White);
                    eventRow.SourceIDLabel.Content = "EE";
                    eventRow.SourceIDTextBlock.Text = "Error";
                }
            }
           
            if (eventHorizonLINQ.SourceID < XMLReaderWriter.SourceTypesList.Count)
            {
                if (eventHorizonLINQ.SourceID > 0)
                    eventRow.SourceIDLabel.Content = MiscFunctions.GetFirstCharsOfString(XMLReaderWriter.SourceTypesList[eventHorizonLINQ.SourceID].Name);
                else
                {
                    eventRow.SourceIDLabel.Content = "★";
                    eventRow.SourceIDLabel.Margin = new Thickness(0, -3, 0, 0);
                    eventRow.SourceIDLabel.FontSize = 14;
                }
            }
            else
            {
                eventRow.SourceIDEllipse.Fill = new SolidColorBrush(Colors.White);
                eventRow.SourceIDLabel.Content = MiscFunctions.GetFirstCharsOfString(XMLReaderWriter.SourceTypesList[eventHorizonLINQ.SourceID].Name);
            }

            eventRow.DetailsTextBlock.Text = eventHorizonLINQ.Details;

            if (eventHorizonLINQ.EventModeID != EventModes.NoteEvent && eventHorizonLINQ.EventModeID != EventModes.ReplyEvent) eventRow.FrequencyGrid.Children.Add(Frequency.GetFrequency(eventHorizonLINQ.FrequencyID));

            eventRow.StatusGrid.Children.Add(StatusIcons.GetStatus(eventHorizonLINQ.StatusID));

            if (eventHorizonLINQ.UserID < XMLReaderWriter.UsersList.Count)
            {
                eventRow.OriginUserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonLINQ.UserID].Color);
                eventRow.OriginUserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonLINQ.UserID);
            }
            else
            {
                eventRow.OriginUserEllipse.Fill = new SolidColorBrush(Colors.White);
                eventRow.OriginUserLabel.Content = eventHorizonLINQ.UserID;
            }

            if (eventHorizonLINQ.TargetUserID < XMLReaderWriter.UsersList.Count)
            {
                eventRow.TargetUserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonLINQ.TargetUserID].Color);
                eventRow.TargetUserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonLINQ.TargetUserID);
            }
            else
            {
                eventRow.TargetUserEllipse.Fill = new SolidColorBrush(Colors.White);
                eventRow.TargetUserLabel.Content = eventHorizonLINQ.TargetUserID;
            }

            if (eventHorizonLINQ.TargetUserID < XMLReaderWriter.UsersList.Count)
            {
                if (eventHorizonLINQ.TargetUserID > 0)
                    eventRow.TargetUserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonLINQ.TargetUserID);
                else
                {
                    eventRow.TargetUserLabel.Content = "★";
                    eventRow.TargetUserLabel.Margin = new Thickness(0, -3, 0, 0);
                    eventRow.TargetUserLabel.FontSize = 14;
                }
            }
            else
            {
                eventRow.TargetUserEllipse.Fill = new SolidColorBrush(Colors.White);
                eventRow.TargetUserLabel.Content = eventHorizonLINQ.TargetUserID;
            }

            string totalDaysString;

            if (eventHorizonLINQ.Attributes_TotalDays < 0)
            {
                if (eventHorizonLINQ.Attributes_TotalDays < -30)
                    totalDaysString = "30";
                else
                    totalDaysString = Math.Abs(eventHorizonLINQ.Attributes_TotalDays).ToString();
            }
            else
            {
                if (eventHorizonLINQ.Attributes_TotalDays > 30)
                    totalDaysString = "30";
                else
                    totalDaysString = eventHorizonLINQ.Attributes_TotalDays.ToString();
            }

            eventRow.TotalDaysEllipse.Fill = new SolidColorBrush(eventHorizonLINQ.Attributes_TotalDaysEllipseColor);
            eventRow.TotalDaysLabel.Content = totalDaysString;

            string targetDateTimeString = eventHorizonLINQ.TargetDate.ToString("dd/MM/y HH:mm");
            DateTime targetDateTime = DateTime.MinValue;
            if (DateTime.TryParse(targetDateTimeString, out targetDateTime))
            {
                if (targetDateTime.TimeOfDay == TimeSpan.Zero)
                    targetDateTimeString = targetDateTime.ToString("dd/MM/y");
                else
                    targetDateTimeString = targetDateTime.ToString("dd/MM/y HH:mm");
            }
            else
                Console.WriteLine("Unable to parse TargetDateTimeString '{0}'", targetDateTimeString);

            eventRow.TargetDateTimeTextBlock.Text = targetDateTimeString;

            eventRow.RepliesLabel.Content = eventHorizonLINQ.Attributes_Replies;

            if (eventHorizonLINQ.Attributes_Replies == 0)
            {
                eventRow.RepliesButton.Opacity = 0.1;
                eventRow.RepliesButton.IsEnabled = false;
            }

            eventRow.Tag = eventHorizonLINQ;

            return eventRow;
        }
        
        public Dictionary<Int32, Grid> UsersOnlineStatus = new Dictionary<int, Grid>();

        public void LoadCurrentUserIntoGrid(Grid grid)
        {
            try
            {
                StackPanel stackPanel = new StackPanel { Orientation = Orientation.Horizontal };

                if (XMLReaderWriter.UsersList[XMLReaderWriter.UserID] != null)
                {
                    User user = XMLReaderWriter.UsersList[XMLReaderWriter.UserID];

                    Grid originUserIconEllipseGrid;
                    Ellipse originUserIconEllipse;

                    Color iconEllipseColor = Colors.White;

                    iconEllipseColor = XMLReaderWriter.UsersList[user.ID].Color;

                    if (user.ID > 0)
                        originUserIconEllipse = new Ellipse { Width = 24, Height = 24, Fill = new SolidColorBrush(iconEllipseColor), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };
                    else
                        originUserIconEllipse = new Ellipse { Width = 24, Height = 24, Fill = new SolidColorBrush(iconEllipseColor), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };

                    originUserIconEllipseGrid = new Grid { Margin = new Thickness(3, 1, 3, 3), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };

                    originUserIconEllipseGrid.Children.Add(originUserIconEllipse);

                    Label originUserIconEllipseLabel;

                    if (user.ID > 0)
                        originUserIconEllipseLabel = new Label { Content = MiscFunctions.GetFirstCharsOfString(user.UserName), Foreground = Brushes.Black, FontSize = 10, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };
                    else
                        originUserIconEllipseLabel = new Label { Content = "★", Foreground = Brushes.Black, FontSize = 14, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, -3, 0, 0), MaxHeight = 24, Padding = new Thickness(0) };

                    originUserIconEllipseGrid.Children.Add(originUserIconEllipseLabel);

                    originUserIconEllipseGrid.Opacity = 1;

                    originUserIconEllipseGrid.Effect = new DropShadowEffect
                    {
                        Color = new Color { A = 255, R = 0, G = 0, B = 0 },
                        Direction = 320,
                        ShadowDepth = 1,
                        Opacity = 0.6
                    };

                    TextBlock usersName = new TextBlock { Text = user.UserName, Foreground = Brushes.Black, FontSize = 11, MaxWidth = 120, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(4, 5, 0, 0), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top, Padding = new Thickness(0) };

                    stackPanel.Children.Add(originUserIconEllipseGrid);
                    stackPanel.Children.Add(usersName);
                }
                grid.Children.Add(stackPanel);
            }
            catch (Exception e)
            {
                Console.WriteLine("----------------------------------------");

                OracleRequesterNotification msg = new OracleRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "LoadCurrentUserIntoUserStackPanel - " + e.Source, InformationTextBlock = e.Message }, RequesterTypes.OK);
                msg.ShowDialog();
            }
        }
        public void LoadUsersIntoWelcome(Grid grid)
        {
            StackPanel stackPanel = new StackPanel { Orientation = Orientation.Horizontal };

            int i = 1;
            foreach (User user in XMLReaderWriter.UsersList)
            {
                if (i > 1)
                {
                    Grid originUserIconEllipseGrid;
                    Ellipse originUserIconEllipse;

                    Color iconEllipseColor = Colors.White;

                    iconEllipseColor = XMLReaderWriter.UsersList[user.ID].Color;

                    if (user.ID > 0)
                        originUserIconEllipse = new Ellipse { Width = 24, Height = 24, Fill = new SolidColorBrush(iconEllipseColor), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };
                    else
                        originUserIconEllipse = new Ellipse { Width = 24, Height = 24, Fill = new SolidColorBrush(iconEllipseColor), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };

                    originUserIconEllipseGrid = new Grid { Margin = new Thickness(3, 1, 3, 3), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };

                    originUserIconEllipseGrid.Children.Add(originUserIconEllipse);

                    Label originUserIconEllipseLabel;

                    if (user.ID > 0)
                        originUserIconEllipseLabel = new Label { Content = MiscFunctions.GetFirstCharsOfString(user.UserName), Foreground = Brushes.Black, FontSize = 10, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };
                    else
                        originUserIconEllipseLabel = new Label { Content = "★", Foreground = Brushes.Black, FontSize = 14, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, -3, 0, 0), MaxHeight = 24, Padding = new Thickness(0) };

                    originUserIconEllipseGrid.Children.Add(originUserIconEllipseLabel);

                    originUserIconEllipseGrid.Opacity = 1;

                    originUserIconEllipseGrid.Effect = new DropShadowEffect
                    {
                        Color = new Color { A = 255, R = 0, G = 0, B = 0 },
                        Direction = 320,
                        ShadowDepth = 1,
                        Opacity = 0.6
                    };

                    stackPanel.Children.Add(originUserIconEllipseGrid);
                }

                i++;
            }
            grid.Children.Add(stackPanel);
        }
        public void LoadEventTypesIntoWelcome(Grid grid)
        {
            StackPanel stackPanel = new StackPanel { Orientation = Orientation.Horizontal };

            int i = 1;
            foreach (EventType eventType in XMLReaderWriter.EventTypesList)
            {
                if (i > 1)
                {
                    Border border = new Border { Width = 28, Height = 28, Background = new SolidColorBrush(eventType.Color), BorderThickness = new Thickness(0), CornerRadius = new CornerRadius(3), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 0, 2.3, 0), Padding = new Thickness(0) };

                    border.Effect = new DropShadowEffect
                    {
                        Color = new Color { A = 255, R = 0, G = 0, B = 0 },
                        Direction = 320,
                        ShadowDepth = 1,
                        Opacity = 0.6
                    };

                    FontAwesome.WPF.FontAwesome fai = new FontAwesome.WPF.FontAwesome { Icon = eventType.Icon, Width = 28, Height = 28, FontSize = 17, Foreground = new SolidColorBrush(Colors.White), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top, Margin = new Thickness(0, 5, 0, 0) };

                    border.Child = fai;

                    stackPanel.Children.Add(border);
                }
                
                i++;
            }
            grid.Children.Add(stackPanel);
        }
        public void LoadSourceTypesIntoWelcome(Grid grid)
        {
            StackPanel stackPanel = new StackPanel { Orientation = Orientation.Horizontal };

            int i = 1;
            foreach (SourceType sourceType in XMLReaderWriter.SourceTypesList)
            {
                if (i > 1)
                {
                    Grid originUserIconEllipseGrid;
                    Ellipse originUserIconEllipse;

                    Color iconEllipseColor = Colors.White;

                    iconEllipseColor = sourceType.Color;

                    if (sourceType.ID > 0)
                        originUserIconEllipse = new Ellipse { Width = 24, Height = 24, Fill = new SolidColorBrush(iconEllipseColor), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };
                    else
                        originUserIconEllipse = new Ellipse { Width = 24, Height = 24, Fill = new SolidColorBrush(iconEllipseColor), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };

                    originUserIconEllipseGrid = new Grid { Margin = new Thickness(3, 1, 3, 3), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };

                    originUserIconEllipseGrid.Children.Add(originUserIconEllipse);

                    Label originUserIconEllipseLabel;

                    if (sourceType.ID > 0)
                        originUserIconEllipseLabel = new Label { Content = MiscFunctions.GetFirstCharsOfString(sourceType.Name), Foreground = Brushes.Black, FontSize = 10, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };
                    else
                        originUserIconEllipseLabel = new Label { Content = "★", Foreground = Brushes.Black, FontSize = 14, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, -3, 0, 0), MaxHeight = 24, Padding = new Thickness(0) };

                    originUserIconEllipseGrid.Children.Add(originUserIconEllipseLabel);

                    originUserIconEllipseGrid.Opacity = 1;

                    originUserIconEllipseGrid.Effect = new DropShadowEffect
                    {
                        Color = new Color { A = 255, R = 0, G = 0, B = 0 },
                        Direction = 320,
                        ShadowDepth = 1,
                        Opacity = 0.6
                    };

                    stackPanel.Children.Add(originUserIconEllipseGrid);
                }

                i++;
            }
            grid.Children.Add(stackPanel);
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
                foreach (User user in XMLReaderWriter.UsersList)
                {
                    Grid originUserIconEllipseGrid;
                    Ellipse originUserIconEllipse;

                    Color iconEllipseColor = Colors.White;

                    iconEllipseColor = XMLReaderWriter.UsersList[user.ID].Color;

                    if (user.ID > 0)
                        originUserIconEllipse = new Ellipse { Width = 24, Height = 24, Fill = new SolidColorBrush(iconEllipseColor), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };
                    else
                        originUserIconEllipse = new Ellipse { Width = 24, Height = 24, Fill = new SolidColorBrush(iconEllipseColor), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };

                    originUserIconEllipseGrid = new Grid { Margin = new Thickness(3, 1, 3, 3), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };

                    originUserIconEllipseGrid.Children.Add(originUserIconEllipse);

                    Label originUserIconEllipseLabel;

                    if (user.ID > 0)
                        originUserIconEllipseLabel = new Label { Content = MiscFunctions.GetFirstCharsOfString(user.UserName), Foreground = Brushes.Black, FontSize = 10, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };
                    else
                        originUserIconEllipseLabel = new Label { Content = "★", Foreground = Brushes.Black, FontSize = 14, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, -3, 0, 0), MaxHeight = 24, Padding = new Thickness(0) };

                    originUserIconEllipseGrid.Children.Add(originUserIconEllipseLabel);

                    originUserIconEllipseGrid.Opacity = 0.3;
                    UsersOnlineStatus.Add(user.ID, originUserIconEllipseGrid);

                    originUserIconEllipseGrid.Effect = new DropShadowEffect
                    {
                        Color = new Color { A = 255, R = 0, G = 0, B = 0 },
                        Direction = 320,
                        ShadowDepth = 1,
                        Opacity = 0.6
                    };

                    if (i < 4)
                        UsersColumn1StackPanel.Children.Add(originUserIconEllipseGrid);
                    else if (i > 3 && i < 7)
                        UsersColumn2StackPanel.Children.Add(originUserIconEllipseGrid);
                    else if (i > 6 && i < 10)
                        UsersColumn3StackPanel.Children.Add(originUserIconEllipseGrid);
                    else if (i > 9 && i < 13)
                        UsersColumn4StackPanel.Children.Add(originUserIconEllipseGrid);
                    else if (i > 12 && i < 16)
                        UsersColumn5StackPanel.Children.Add(originUserIconEllipseGrid);

                    i++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("----------------------------------------");

                OracleRequesterNotification msg = new OracleRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "LoadUsersIntoUsersStackPanel - " + e.Source, InformationTextBlock = e.Message }, RequesterTypes.OK);
                msg.ShowDialog();
            }
        }

        private void UpdateUsersOnline()
        {
            DataTableManagement.GetUsersLastTimeOnline();

            foreach (User user in XMLReaderWriter.UsersList)
            {
                if (UsersOnlineStatus.ContainsKey(user.ID))
                {
                    //Check if user was still online a minute ago, if so refresh user icon
                    if (UsersLastTimeOnlineDictionary.ContainsKey(user.ID))
                    {
                        if (UsersLastTimeOnlineDictionary[user.ID] > (DateTime.Now - TimeSpan.FromMinutes(2)))
                        {
                            Grid usersIconGrid = UsersOnlineStatus[user.ID];
                            usersIconGrid.Opacity = 1;
                        }
                        else
                        {
                            Grid usersIconGrid = UsersOnlineStatus[user.ID];
                            usersIconGrid.Opacity = 0.3;
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
                    EventWindow newEventWindow = new EventWindow(this, EventWindowModes.NewEvent, new EventHorizonLINQ(), null);
                    newEventWindow.Show();
                    break;
                case Key.F2:
                    if (eventHorizonLINQ != null)
                    {
                        EventWindow editEventWindow = new EventWindow(this, EventWindowModes.NewNote, eventHorizonLINQ, null);
                        editEventWindow.Show();
                    }
                    break;
                case Key.F3:
                    if (XMLReaderWriter.EventTypesList.Count > 1)
                    {
                        EventWindow F3NewEventWindow = new EventWindow(this, EventWindowModes.NewEvent, new EventHorizonLINQ
                        {
                            EventTypeID = XMLReaderWriter.EventTypesList[1].ID,
                        }, null);
                        F3NewEventWindow.Show();
                    }
                    break;
                case Key.F4:
                    if (XMLReaderWriter.EventTypesList.Count > 2)
                    {
                        EventWindow F4NewEventWindow = new EventWindow(this, EventWindowModes.NewEvent, new EventHorizonLINQ
                        {
                            EventTypeID = XMLReaderWriter.EventTypesList[2].ID,
                        }, null);
                        F4NewEventWindow.Show();
                    }
                    break;
                case Key.F5:
                    if (XMLReaderWriter.EventTypesList.Count > 3)
                    {
                        EventWindow F5NewEventWindow = new EventWindow(this, EventWindowModes.NewEvent, new EventHorizonLINQ
                        {
                            EventTypeID = XMLReaderWriter.EventTypesList[3].ID,
                        }, null);
                        F5NewEventWindow.Show();
                    }
                    break;
                case Key.F6:
                    if (XMLReaderWriter.EventTypesList.Count > 4)
                    {
                        EventWindow F6NewEventWindow = new EventWindow(this, EventWindowModes.NewEvent, new EventHorizonLINQ
                        {
                            EventTypeID = XMLReaderWriter.EventTypesList[4].ID,
                        }, null);
                        F6NewEventWindow.Show();
                    }
                    break;
                case Key.F7:
                    if (XMLReaderWriter.EventTypesList.Count > 5)
                    {
                        EventWindow F7NewEventWindow = new EventWindow(this, EventWindowModes.NewEvent, new EventHorizonLINQ
                        {
                            EventTypeID = XMLReaderWriter.EventTypesList[5].ID,
                        }, null);
                        F7NewEventWindow.Show();
                    }
                    break;
                case Key.F8:
                    if (XMLReaderWriter.EventTypesList.Count > 6)
                    {
                        EventWindow F8NewEventWindow = new EventWindow(this, EventWindowModes.NewEvent, new EventHorizonLINQ
                        {
                            EventTypeID = XMLReaderWriter.EventTypesList[6].ID,
                        }, null);
                        F8NewEventWindow.Show();
                    }
                    break;
                case Key.F9:
                    if (XMLReaderWriter.EventTypesList.Count > 7)
                    {
                        EventWindow F9NewEventWindow = new EventWindow(this, EventWindowModes.NewEvent, new EventHorizonLINQ
                        {
                            EventTypeID = XMLReaderWriter.EventTypesList[7].ID,
                        }, null);
                        F9NewEventWindow.Show();
                    }
                    break;
                case Key.System:
                    if (e.SystemKey == Key.F10)
                    {
                        if (XMLReaderWriter.EventTypesList.Count > 8)
                        {
                            EventWindow F10NewEventWindow = new EventWindow(this, EventWindowModes.NewEvent, new EventHorizonLINQ
                            {
                                EventTypeID = XMLReaderWriter.EventTypesList[8].ID,
                            }, null);
                            F10NewEventWindow.Show();
                        }
                    }
                    break;
                case Key.F11:
                    if (XMLReaderWriter.EventTypesList.Count > 9)
                    {
                        EventWindow F11NewEventWindow = new EventWindow(this, EventWindowModes.NewEvent, new EventHorizonLINQ
                        {
                            EventTypeID = XMLReaderWriter.EventTypesList[9].ID,
                        }, null);
                        F11NewEventWindow.Show();
                    }
                    break;
                case Key.F12:
                    if (XMLReaderWriter.EventTypesList.Count > 10)
                    {
                        EventWindow F12NewEventWindow = new EventWindow(this, EventWindowModes.NewEvent, new EventHorizonLINQ
                        {
                            EventTypeID = XMLReaderWriter.EventTypesList[10].ID,
                        }, null);
                        F12NewEventWindow.Show();
                    }
                    break;
                case Key.Delete:
                    if (eventHorizonLINQ != null)
                    {
                        if (SelectedReplies > 0)
                        {
                            OracleRequesterNotification rorn = new OracleRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "Error, this event has notes or replies", InformationTextBlock = "You won't be able to delete an event if it has notes or replies.\nYou must delete them first." }, RequesterTypes.OK);
                            rorn.ShowDialog();
                        }
                        else
                        {
                            OracleRequesterNotification orn = new OracleRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "Delete this event, are you sure", InformationTextBlock = "Consider changing the events status to archived instead.\nThat way you use the event as a log." }, RequesterTypes.NoYes);
                            var result = orn.ShowDialog();
                            if (result == true)
                            {
                                if (eventHorizonLINQ.ID > 0) DataTableManagement.DeleteEvent(eventHorizonLINQ.ID);
                            }
                        }
                    }
                    break;
            }
        }

        private void TreeView_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Button button = sender as Button;

            int buttonID = 255;

            bool success = Int32.TryParse(button.Tag.ToString(), out buttonID);

            if (button != null && success)
            {
                switch (buttonID)
                {
                    case 0:
                        EventTypeComboBox.SelectedIndex = 0;                     
                        break;
                    case 1:
                        EventTypeComboBox.SelectedIndex = 0;
                        break;
                    case 2:
                        EventTypeComboBox.SelectedIndex = 1;
                        break;
                    case 3:
                        EventTypeComboBox.SelectedIndex = 2;
                        break;
                    case 4:
                        EventTypeComboBox.SelectedIndex = 3;
                        break;
                    case 5:
                        EventTypeComboBox.SelectedIndex = 4;
                        break;
                    case 6:
                        EventTypeComboBox.SelectedIndex = 5;
                        break;
                    case 7:
                        EventTypeComboBox.SelectedIndex = 6;
                        break;
                    case 8:
                        EventTypeComboBox.SelectedIndex = 7;
                        break;
                    case 9:
                        EventTypeComboBox.SelectedIndex = 8;
                        break;
                    case 10:
                        EventTypeComboBox.SelectedIndex = 9;
                        break;
                    case 11:
                        EventTypeComboBox.SelectedIndex = 10;
                        break;
                }
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
                        EventWindow newEventWindow = new EventWindow(this, EventWindowModes.NewEvent, new EventHorizonLINQ
                        {
                            EventTypeID = XMLReaderWriter.EventTypesList[0].ID,
                        }, null);
                        newEventWindow.Show();
                        break;
                    case 1:
                        if (eventHorizonLINQ != null)
                        {
                            EventWindow editEventWindow = new EventWindow(this, EventWindowModes.ViewMainEvent, eventHorizonLINQ, null);
                            editEventWindow.Show();
                        }
                        break;
                    case 2:
                        if (XMLReaderWriter.EventTypesList.Count > 1)
                        {
                            EventWindow F3NewEventWindow = new EventWindow(this, EventWindowModes.NewEvent, new EventHorizonLINQ
                            {
                                EventTypeID = XMLReaderWriter.EventTypesList[1].ID,
                            }, null);
                            F3NewEventWindow.Show();
                        }
                        break;
                    case 3:
                        if (XMLReaderWriter.EventTypesList.Count > 2)
                        {
                            EventWindow F4NewEventWindow = new EventWindow(this, EventWindowModes.NewEvent, new EventHorizonLINQ
                            {
                                EventTypeID = XMLReaderWriter.EventTypesList[2].ID,
                            }, null);
                            F4NewEventWindow.Show();
                        }
                        break;
                    case 4:
                        if (XMLReaderWriter.EventTypesList.Count > 3)
                        {
                            EventWindow F5NewEventWindow = new EventWindow(this, EventWindowModes.NewEvent, new EventHorizonLINQ
                            {
                                EventTypeID = XMLReaderWriter.EventTypesList[3].ID,
                            }, null);
                            F5NewEventWindow.Show();
                        }
                        break;
                    case 5:
                        if (XMLReaderWriter.EventTypesList.Count > 4)
                        {
                            EventWindow F6NewEventWindow = new EventWindow(this, EventWindowModes.NewEvent, new EventHorizonLINQ
                            {
                                EventTypeID = XMLReaderWriter.EventTypesList[4].ID,
                            }, null);
                            F6NewEventWindow.Show();
                        }
                        break;
                    case 6:
                        if (XMLReaderWriter.EventTypesList.Count > 5)
                        {
                            EventWindow F7NewEventWindow = new EventWindow(this, EventWindowModes.NewEvent, new EventHorizonLINQ
                            {
                                EventTypeID = XMLReaderWriter.EventTypesList[5].ID,
                            }, null);
                            F7NewEventWindow.Show();
                        }
                        break;
                    case 7:
                        if (XMLReaderWriter.EventTypesList.Count > 6)
                        {
                            EventWindow F8NewEventWindow = new EventWindow(this, EventWindowModes.NewEvent, new EventHorizonLINQ
                            {
                                EventTypeID = XMLReaderWriter.EventTypesList[6].ID,
                            }, null);
                            F8NewEventWindow.Show();
                        }
                        break;
                    case 8:
                        if (XMLReaderWriter.EventTypesList.Count > 7)
                        {
                            EventWindow F9NewEventWindow = new EventWindow(this, EventWindowModes.NewEvent, new EventHorizonLINQ
                            {
                                EventTypeID = XMLReaderWriter.EventTypesList[7].ID,
                            }, null);
                            F9NewEventWindow.Show();
                        }
                        break;
                    case 9:
                        if (XMLReaderWriter.EventTypesList.Count > 8)
                        {
                            EventWindow F10NewEventWindow = new EventWindow(this, EventWindowModes.NewEvent, new EventHorizonLINQ
                            {
                                EventTypeID = XMLReaderWriter.EventTypesList[8].ID,
                            }, null);
                            F10NewEventWindow.Show();
                        }
                        break;
                    case 10:
                        if (XMLReaderWriter.EventTypesList.Count > 9)
                        {
                            EventWindow F11NewEventWindow = new EventWindow(this, EventWindowModes.NewEvent, new EventHorizonLINQ
                            {
                                EventTypeID = XMLReaderWriter.EventTypesList[9].ID,
                            }, null);
                            F11NewEventWindow.Show();
                        }
                        break;
                    case 11:
                        if (XMLReaderWriter.EventTypesList.Count > 10)
                        {
                            EventWindow F12NewEventWindow = new EventWindow(this, EventWindowModes.NewEvent, new EventHorizonLINQ
                            {
                                EventTypeID = XMLReaderWriter.EventTypesList[10].ID,
                            }, null);
                            F12NewEventWindow.Show();
                        }
                        break;
                }
            }
        }

        public EventHorizonLINQ eventHorizonLINQ;
        
        //private void EventLogListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    DependencyObject dep = (DependencyObject)e.OriginalSource;

        //    while ((dep != null) && !(dep is StackPanel))
        //    {
        //        dep = VisualTreeHelper.GetParent(dep);
        //    }

        //    if (dep == null)
        //        return;

        //    StackPanel item = (StackPanel)dep;

        //    Console.Write("item = ");
        //    Console.WriteLine(item.Tag);

        //    EventLogListViewTagged = Convert.ToInt32(item.Tag);
        //}

        private void AddItemsToEventTypeComboBox()
        {
            foreach (EventType eventType in XMLReaderWriter.EventTypesList)
            {
                EventTypeComboBox.Items.Add(EventHorizonEventTypes.GetEventTypeStackPanel(eventType));
            }

            EventTypeComboBox.SelectedIndex = 0;

            //EventTypeComboBox.ItemsSource = (from c in XMLReaderWriter.EventTypesList select new { c.Name }).Distinct().ToList();
            //EventTypeComboBox.DisplayMemberPath = "Name";

            //EventTypeComboBox.SelectedIndex = 0;
        }

        public TimeSpan ReminderListTimeSpan = new TimeSpan(1, 0, 0, 0);

        private void TimeSpanRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = e.OriginalSource as RadioButton;

            int buttonID = 0;

            bool success = Int32.TryParse(radioButton.Tag.ToString(), out buttonID);

            if (radioButton != null && success)
            {
                switch (buttonID)
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

        //private void EventLogListView_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    DependencyObject dep = (DependencyObject)e.OriginalSource;

        //    while ((dep != null) && !(dep is StackPanel))
        //    {
        //        dep = VisualTreeHelper.GetParent(dep);
        //    }

        //    if (dep == null)
        //        return;

        //    StackPanel item = (StackPanel)dep;

        //    Console.Write("item = ");
        //    Console.WriteLine(item.Tag);

        //    EventLogListViewTagged = Convert.ToInt32(item.Tag);

        //    if (EventLogListViewTagged > 0)
        //    {
        //        EventWindow editEventWindow = new EventWindow(this, new EventHorizonLINQ
        //        {
        //            Source_ID = EventLogListViewTagged,
        //            Source_Mode = EventWindowModes.EditMainEvent,
        //            Source_ParentEventID = SelectedParentEventID,
        //            Attributes_TotalDays = 0,
        //            ID = EventLogListViewTagged,
        //        }, null);
        //        editEventWindow.Show();
        //    }
        //}

        public int FilterMode = FilterModes.None;
        
        private void FilterModeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = e.OriginalSource as RadioButton;

            int buttonID = 0;

            bool success = Int32.TryParse(radioButton.Tag.ToString(), out buttonID);

            if (radioButton != null && success)
            {
                switch (buttonID)
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
                int buttonID = 0;

                bool success = Int32.TryParse(radioButton.Tag.ToString(), out buttonID);

                if (radioButton != null && success)
                {
                    switch (buttonID)
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
            Button button = e.OriginalSource as Button;

            int buttonID = 0;

            bool success = Int32.TryParse(button.Tag.ToString(), out buttonID);

            if (button != null && success)
            {
                switch (buttonID)
                {
                    case 0:
                        EventTypeComboBox.SelectedIndex = 0;
                        EventTypeID = EventTypeComboBox.SelectedIndex;
                        SearchTextBox.Text = string.Empty;
                        break;
                    case 1:
                        SearchTextBox.Text = string.Empty;
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

        public Int32 SelectedParentEventID = 0;
        public Int32 SelectedReplies = 0;

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

            eventHorizonLINQ = (EventHorizonLINQ)item.Tag;
            
            //EventLogListViewTagged = Convert.ToInt32(eventHorizonLINQ.ID);

            //SelectedParentEventID = Convert.ToInt32(eventHorizonLINQ.Source_ParentEventID);
            
            //SelectedReplies = Convert.ToInt32(eventHorizonLINQ.Attributes_Replies);

            Console.WriteLine();
            Console.WriteLine(">S>>MainWindow ReminderListView_PreviewMouseLeftButtonDown<<<<");
            Console.WriteLine();

            Console.Write("item.Tag eventHorizonLINQ.Source_Mode = ");
            Console.WriteLine(eventHorizonLINQ.Source_Mode);
            
            Console.Write("item.Tag eventHorizonLINQ.ID = ");
            Console.WriteLine(eventHorizonLINQ.ID);
            
            Console.Write("item.Tag eventHorizonLINQ.Source_ParentEventID = ");
            Console.WriteLine(eventHorizonLINQ.Source_ParentEventID);
            
            Console.Write("item.Tag eventHorizonLINQ.Attributes_Replies = ");
            Console.WriteLine(eventHorizonLINQ.Attributes_Replies);
            
            Console.WriteLine();
            Console.WriteLine(">F>>MainWindow ReminderListView_PreviewMouseLeftButtonDown<<<<");
            Console.WriteLine();
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

            eventHorizonLINQ = (EventHorizonLINQ)item.Tag;
            //EventLogListViewTagged = Convert.ToInt32(eventHorizonLINQ.ID);

            Console.WriteLine();
            Console.WriteLine(">S>>MainWindow ReminderListView_MouseDoubleClick<<<<");
            Console.WriteLine();

            Console.Write("item.Tag eventHorizonLINQ.Source_Mode = ");
            Console.WriteLine(eventHorizonLINQ.Source_Mode);

            Console.Write("item.Tag eventHorizonLINQ.ID = ");
            Console.WriteLine(eventHorizonLINQ.ID);

            Console.Write("item.Tag eventHorizonLINQ.Source_ParentEventID = ");
            Console.WriteLine(eventHorizonLINQ.Source_ParentEventID);

            Console.Write("item.Tag eventHorizonLINQ.Attributes_Replies = ");
            Console.WriteLine(eventHorizonLINQ.Attributes_Replies);

            Console.WriteLine();
            Console.WriteLine(">F>>MainWindow ReminderListView_MouseDoubleClick<<<<");
            Console.WriteLine();
            
            ReminderListView.SelectedItem = item;

            if (eventHorizonLINQ != null)
            {
                //eventHorizonLINQ.Source_ID = EventLogListViewTagged;
                //eventHorizonLINQ.ID = EventLogListViewTagged;
                //eventHorizonLINQ.Source_Mode = EventWindowModes.EditMainEvent;

                //try open event as EditEvent
                EventWindow editEventWindow = new EventWindow(this, EventWindowModes.ViewMainEvent, eventHorizonLINQ, null);
                editEventWindow.Show();
            }
        }

        private void SearchTextBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (DisplayMode == DisplayModes.Reminders)
                    RefreshLog(ListViews.Reminder);
                else
                    RefreshLog(ListViews.Log);
            }
        }
    }
}