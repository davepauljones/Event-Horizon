using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Media.Effects;
using FontAwesome.WPF;
using System.Globalization;
using Xceed.Wpf.Toolkit;

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
        public EventHorizonDatabaseHealth eventHorizonDatabaseHealth;
        private UsersOnline usersOnline;

        public static MainWindow mw;
        public static DateTime OracleDatabaseLastWriteTime = DateTime.Now;

        public delegate void OnOracleDatabaseChanged(object source, FileSystemEventArgs e);

        public bool justLoaded = false;

        public List<EventHorizonLINQ> EventHorizonLINQList;

        public static Dictionary<Int32, DateTime> UsersLastTimeOnlineDictionary = new Dictionary<int, DateTime>();

        private static DatabasePoller databasePoller;

        List<SelectionIdString> ListOfReports = new List<SelectionIdString>();
        List<SelectionIdString> ListOfHelp = new List<SelectionIdString>();

        public EventHorizonLINQ eventHorizonLINQ_SelectedItem;

        public Dictionary<Int32, StackPanel> OnlineUsersStackPanelList = new Dictionary<int, StackPanel>();

        private void Init_OracleDatabaseFileWatcher()
        {
            databasePoller = new DatabasePoller(XMLReaderWriter.GlobalConnectionString);
            databasePoller.StartPolling();
        }

        public void RunningTask()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                eventHorizonDatabaseHealth.UpdateLastWriteDateTime(DateTime.Now);

                if (DisplayMode == DisplayModes.Reminders)
                    RefreshLog(ListViews.Reminder);
                else
                    RefreshLog(ListViews.Log);
            }));
        }

        public void RunCycle()
        {
            today.SyncDate();
            now.SyncTime();

            //Check Users online every 60 seconds only executes if second is 0
            if (DateTime.Now.Second == XMLReaderWriter.UserID * 6)//use UserID as to offset actual second used to update
            {
                DataTableManagement.InsertOrUpdateLastTimeOnline(XMLReaderWriter.UserID);
                LoadUsersIntoOnlineUsersStackPanel(usersOnline.UsersOnlineStackPanel);
            }
            CheckMyUnreadAndMyReminders();
            MainWindow.mw.eventHorizonDatabaseHealth.UpdateLastWriteLabel(false);
            justLoaded = true;
        }

        public MainWindow()
        {
            InitializeComponent();

            mw = this;

            Welcome welcome = new Welcome();

            if (welcome.ShowDialog() == true)
            {
                if (XMLReaderWriter.OverridePassword == false)
                {
                    EventHorizonLogin oli = new EventHorizonLogin(MainWindow.mw);
                    oli.SelectUserComboBox.SelectedIndex = XMLReaderWriter.UserID - 1;

                    if (oli.ShowDialog() == true)
                    {

                        this.WindowState = System.Windows.WindowState.Maximized;

                        EventStackPanel.Visibility = Visibility.Visible;

                        MainWindowTitle.SetMainWindowTitle();

                        Loaded += MainWindow_Loaded;
                    }
                    else
                        Close();
                }
                else
                {
                    this.WindowState = System.Windows.WindowState.Maximized;

                    EventStackPanel.Visibility = Visibility.Visible;

                    MainWindowTitle.SetMainWindowTitle();

                    Loaded += MainWindow_Loaded;
                }
            }
            else
                Close();
        }

        bool MainWindowIs_Loaded = false;

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadReportsVisualTree();
            LoadHelpVisualTree();

            today = new Today();
            TodayGrid.Children.Add(today);

            now = new Now();
            NowGrid.Children.Add(now);

            eventHorizonDatabaseHealth = new EventHorizonDatabaseHealth();
            OracleDatabaseHealthGrid.Children.Add(eventHorizonDatabaseHealth);

            usersOnline = new UsersOnline();
            UsersOnlineGrid.Children.Add(usersOnline);

            if (EventHorizonDatabaseCreate.CheckIfDatabaseExists())
            {
                Init_OracleDatabaseFileWatcher();

                RefreshXML();

                CheckMyUnreadAndMyReminders();

                DataTableManagement.InsertOrUpdateLastTimeOnline(XMLReaderWriter.UserID);

                LoadUsersIntoOnlineUsersStackPanel(usersOnline.UsersOnlineStackPanel);
            }

            MainWindowIs_Loaded = true;
        }

        public void RefreshXML()
        {
            CurrentUserGrid.Children.Add(GetUserAsTokenStackPanel(XMLReaderWriter.UsersList[XMLReaderWriter.UserID]));
            LoadUsersIntoOnlineUsersStackPanel(usersOnline.UsersOnlineStackPanel);
            AddItemsToEventTypeComboBox();
        }

        private void CheckMyUnreadAndMyReminders()
        {
            Int32 notificationsAddedThisCycle = 0;
            List<EventHorizonLINQ> eventHorizonLINQList = DataTableManagement.GetMyUnreadAndMyReminders();
            Int32 notifications = eventHorizonLINQList.Count;

            foreach (EventHorizonLINQ eventHorizonLINQ in eventHorizonLINQList)
            {
                if (!EventHorizonNotification.Notifications.ContainsKey(eventHorizonLINQ.ID))
                {
                    EventHorizonNotification on = new EventHorizonNotification(this, eventHorizonLINQ.ID, notifications, eventHorizonLINQList.Count, eventHorizonLINQ);
                    on.Show();

                    notifications--;

                    notificationsAddedThisCycle++;
                }
            }

            if (notificationsAddedThisCycle > 0) MiscFunctions.PlayFile("Notification.mp3");
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
                        Int32 LineItemNumber = 0;
                        Int32 grandTotalItems = 0;
                        double grandTotalUnitCost = 0;
                        double grandTotalTotal = 0;

                        foreach (EventHorizonLINQ eventHorizonLINQRow in eventHorizonLINQRepliesList)
                        {
                            EventRow er = CreateEventLogRow(eventHorizonLINQRow);

                            eventRow.RepliesListView.Items.Add(er);

                            switch (eventHorizonLINQRow.EventAttributeID)
                            {
                                case EventAttributes.Standard:
                                    break;
                                case EventAttributes.LineItem:
                                    if (eventHorizonLINQRow == eventHorizonLINQRepliesList.First()) // Check if it's the first item
                                    {
                                        er.HeaderGrid.Visibility = Visibility.Visible;;
                                    }

                                    LineItemNumber++;

                                    er.EventTypeTextBlock.Text = "Item " + LineItemNumber;

                                    grandTotalItems += eventHorizonLINQRow.Qty;

                                    grandTotalUnitCost += eventHorizonLINQRow.UnitCost * eventHorizonLINQRow.Qty;

                                    grandTotalTotal += (eventHorizonLINQRow.UnitCost * eventHorizonLINQRow.Qty) - (eventHorizonLINQRow.UnitCost * eventHorizonLINQRow.Qty) * eventHorizonLINQRow.Discount / 100;

                                    if (eventHorizonLINQRow == eventHorizonLINQRepliesList.Last()) // Check if it's the last item
                                    {
                                        er.StatusBarGrid.Visibility = Visibility.Visible;

                                        er.TotalItemsTextBlock.Text = grandTotalItems.ToString();

                                        er.TotalUnitCostTextBlock.Text = grandTotalUnitCost.ToString("C2", CultureInfo.CurrentCulture);

                                        er.GrandTotalTextBlock.Visibility = Visibility.Visible;
                                        er.GrandTotalTextBlock.Text = grandTotalTotal.ToString("C2", CultureInfo.CurrentCulture);
                                    }
                                    break;
                                case EventAttributes.FooBar:
                                    break;
                            }
                        }
                    }
                }
                Status.Content = "Reminders needing attention " + ReminderListView.Items.Count;
            }
            catch (Exception e)
            {
                Console.WriteLine("----------------------------------------");

                EventHorizonRequesterNotification msg = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "RefreshLog - " + e.Source, InformationTextBlock = e.Message }, RequesterTypes.OK);
                msg.ShowDialog();
            }
        }

        public List<EventHorizonLINQ> GetProductItems(EventHorizonLINQ eventHorizonLINQ)
        {
            List<EventHorizonLINQ> Return_EventHorizonLINQ = new List<EventHorizonLINQ>();

            try
            {
                List<EventHorizonLINQ> eventHorizonLINQRepliesList = DataTableManagement.GetReplies(eventHorizonLINQ.ID);

                foreach (EventHorizonLINQ eventHorizonLINQRow in eventHorizonLINQRepliesList)
                {
                    switch (eventHorizonLINQRow.EventAttributeID)
                    {
                        case EventAttributes.Standard:
                            break;
                        case EventAttributes.LineItem:
                            Return_EventHorizonLINQ.Add(eventHorizonLINQRow);
                            break;
                        case EventAttributes.FooBar:
                            break;
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("----------------------------------------");

                EventHorizonRequesterNotification msg = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "GetProductItems - " + e.Source, InformationTextBlock = e.Message }, RequesterTypes.OK);
                msg.ShowDialog();
            }

            return Return_EventHorizonLINQ;
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
                eventRow.SourceTypeFontAwesomeIconBorder.Visibility = Visibility.Hidden;
            }
            else if (eventHorizonLINQ.EventModeID == EventModes.ReplyEvent)
            {
                eventRow.EventTypeFontAwesomeIconBorder.Background = new SolidColorBrush(Colors.LightSlateGray);
                eventRow.EventTypeFontAwesomeIcon.Icon = FontAwesomeIcon.Exchange;
                eventRow.EventTypeTextBlock.Text = "Reply";
                eventRow.BackgroundGrid.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f7f8f9"));
                eventRow.SourceTypeFontAwesomeIconBorder.Visibility = Visibility.Hidden;
            }
            else
            {
                if (eventHorizonLINQ.EventTypeID < XMLReaderWriter.EventTypesList.Count)
                {
                    eventRow.EventTypeFontAwesomeIconBorder.Background = new SolidColorBrush(XMLReaderWriter.EventTypesList[eventHorizonLINQ.EventTypeID].Color);
                    eventRow.EventTypeFontAwesomeIcon.Icon = XMLReaderWriter.EventTypesList[eventHorizonLINQ.EventTypeID].Icon;
                    eventRow.EventTypeTextBlock.Text = XMLReaderWriter.EventTypesList[eventHorizonLINQ.EventTypeID].Name;
                    eventRow.BackgroundGrid.Background = new SolidColorBrush(Colors.Transparent);
                    eventRow.SourceTypeFontAwesomeIconBorder.Visibility = Visibility.Visible;
                }
                else
                {
                    eventRow.EventTypeFontAwesomeIconBorder.Background = new SolidColorBrush(Colors.White);
                    eventRow.EventTypeFontAwesomeIcon.Icon = FontAwesomeIcon.Question;
                    eventRow.EventTypeTextBlock.Text = "Error";
                    eventRow.BackgroundGrid.Background = new SolidColorBrush(Colors.Transparent);
                    eventRow.SourceTypeFontAwesomeIconBorder.Visibility = Visibility.Visible;
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
                    eventRow.SourceTypeFontAwesomeIconBorder.Background = new SolidColorBrush(XMLReaderWriter.SourceTypesList[eventHorizonLINQ.SourceID].Color);
                    eventRow.SourceTypeFontAwesomeIcon.Icon = XMLReaderWriter.SourceTypesList[eventHorizonLINQ.SourceID].Icon;
                    eventRow.SourceIDTextBlock.Text = XMLReaderWriter.SourceTypesList[eventHorizonLINQ.SourceID].Name;
                    eventRow.BackgroundGrid.Background = new SolidColorBrush(Colors.Transparent);
                    eventRow.SourceTypeFontAwesomeIconBorder.Visibility = Visibility.Visible;
                }
                else
                {
                    eventRow.SourceTypeFontAwesomeIconBorder.Background = new SolidColorBrush(Colors.White);
                    eventRow.SourceTypeFontAwesomeIcon.Icon = FontAwesomeIcon.Question;
                    eventRow.SourceIDTextBlock.Text = "Error";
                    eventRow.BackgroundGrid.Background = new SolidColorBrush(Colors.Transparent);
                    eventRow.SourceTypeFontAwesomeIconBorder.Visibility = Visibility.Visible;
                }
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

            if (eventHorizonLINQ.EventAttributeID == EventAttributes.LineItem)
            {
                eventRow.MorphEventRow();
            }

            eventRow.Tag = eventHorizonLINQ;

            return eventRow;
        } 

        internal void LoadUsersIntoOnlineUsersStackPanel(StackPanel parentStackPanel)
        {
            DataTableManagement.GetUsersLastTimeOnline();

            int UsersWhoJustCameOnlineCount = 0;

            int UsersOnlineCount = 0;

            foreach (User user in XMLReaderWriter.UsersList)
            {
                if (user != XMLReaderWriter.UsersList.First())
                {
                    if (!OnlineUsersStackPanelList.ContainsKey(user.ID))
                    {
                        StackPanel childStackPanel = GetUserAsTokenStackPanel(user);
                        
                        if (UsersLastTimeOnlineDictionary.ContainsKey(user.ID))
                        {
                            if (UsersLastTimeOnlineDictionary[user.ID] > (DateTime.Now - TimeSpan.FromMinutes(2)))
                            {
                                childStackPanel.Opacity = 1;

                                UsersOnlineCount++;
                            }
                            else
                            {
                                childStackPanel.Opacity = 0.2;
                            }

                            OnlineUsersStackPanelList.Add(user.ID, childStackPanel);
                            parentStackPanel.Children.Add(childStackPanel);
                        }
                    }
                    else
                    {
                        if (UsersLastTimeOnlineDictionary.ContainsKey(user.ID))
                        {
                            if (UsersLastTimeOnlineDictionary[user.ID] > (DateTime.Now - TimeSpan.FromMinutes(2)))
                            {
                                if (user.ID != XMLReaderWriter.UserID && OnlineUsersStackPanelList[user.ID].Opacity != 1)
                                {
                                    UsersWhoJustCameOnlineCount++;
                                }

                                OnlineUsersStackPanelList[user.ID].Opacity = 1;

                                UsersOnlineCount++;
                            }
                            else
                            {
                                OnlineUsersStackPanelList[user.ID].Opacity = 0.2;
                            }
                        }
                    }
                }

                if (user == XMLReaderWriter.UsersList.Last()) // Check if its the last item
                {
                    usersOnline.NumberOfUsersOnlineLabel.Content = UsersOnlineCount + " of " + (XMLReaderWriter.UsersList.Count - 1);

                    if (UsersWhoJustCameOnlineCount > 0)
                    {
                        MiscFunctions.PlayFile("Notification.mp3");
                    }
                }
            }
        }

        internal StackPanel GetUserAsTokenStackPanel(User user, bool JustUsersToken = false)
        {
            StackPanel stackPanel = new StackPanel { Orientation = Orientation.Horizontal, Height = 30 };

            Grid originUserIconEllipseGrid;
            Ellipse originUserIconEllipse;

            Color iconEllipseColor = XMLReaderWriter.UsersList[user.ID].Color;

            if (user.ID > 0)
                originUserIconEllipse = new Ellipse();
            else
                originUserIconEllipse = new Ellipse();

            originUserIconEllipse.SetResourceReference(Control.StyleProperty, "EllipseToken_EllipseStyle");
            originUserIconEllipse.Fill = new SolidColorBrush(iconEllipseColor);

            originUserIconEllipseGrid = new Grid();

            originUserIconEllipseGrid.SetResourceReference(Control.StyleProperty, "EllipseToken_GridStyle");

            originUserIconEllipseGrid.Children.Add(originUserIconEllipse);

            Label originUserIconEllipseLabel;

            if (user.ID > 0)
            {
                originUserIconEllipseLabel = new Label();
                originUserIconEllipseLabel.Content = MiscFunctions.GetFirstCharsOfString(user.UserName);
                originUserIconEllipseLabel.SetResourceReference(Control.StyleProperty, "EllipseToken_LabelStyle");
            }
            else
            {
                originUserIconEllipseLabel = new Label();
                originUserIconEllipseLabel.Content = "★";
                originUserIconEllipseLabel.SetResourceReference(Control.StyleProperty, "EllipseToken_LabelStyle");
                originUserIconEllipseLabel.FontSize = 14;
                originUserIconEllipseLabel.Margin = new Thickness(0, -3, 0, 0);
                originUserIconEllipseLabel.MaxHeight = 24;
                originUserIconEllipseLabel.Padding = new Thickness(0);
            }

            originUserIconEllipseGrid.Children.Add(originUserIconEllipseLabel);

            TextBlock usersName = new TextBlock();
            usersName.Text = user.UserName;
            usersName.SetResourceReference(Control.StyleProperty, "EventTypeText_TextBlockStyle");

            stackPanel.Children.Add(originUserIconEllipseGrid);

            if (!JustUsersToken)
            {
                stackPanel.Children.Add(usersName);
            }
            else
            {
                stackPanel.Margin = new Thickness(0, 0, -5.5, 0);
            }

            return stackPanel;
        }
 
        internal void LoadUsersIntoWelcome(Grid grid)
        {
            WrapPanel stackPanel = new WrapPanel { Orientation = Orientation.Horizontal };

            int i = 1;
            
            foreach (User user in XMLReaderWriter.UsersList)
            {
                if (i > 1)
                {
                    Grid originUserIconEllipseGrid;

                    originUserIconEllipseGrid = new Grid();

                    originUserIconEllipseGrid.Children.Add(GetUserAsTokenStackPanel(XMLReaderWriter.UsersList[user.ID], true));
 
                    stackPanel.Children.Add(originUserIconEllipseGrid);
                }

                i++;
            }

            grid.Children.Add(stackPanel);
        }

        public void LoadEventTypesIntoWelcome(Grid grid)
        {
            WrapPanel stackPanel = new WrapPanel { Orientation = Orientation.Horizontal };

            int i = 1;
            foreach (EventType eventType in XMLReaderWriter.EventTypesList)
            {
                if (i > 1)
                {
                    Border border = new Border { Width = 28, Height = 28, Background = new SolidColorBrush(eventType.Color), BorderThickness = new Thickness(0), CornerRadius = new CornerRadius(3), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 0, 2.3, 2.3), Padding = new Thickness(0) };

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
            WrapPanel stackPanel = new WrapPanel { Orientation = Orientation.Horizontal };

            int i = 1;
            foreach (SourceType sourceType in XMLReaderWriter.SourceTypesList)
            {
                if (i > 1)
                {
                    Border border = new Border { Width = 28, Height = 28, Background = new SolidColorBrush(sourceType.Color), BorderThickness = new Thickness(0), CornerRadius = new CornerRadius(3), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 0, 2.3, 2.3), Padding = new Thickness(0) };

                    border.Effect = new DropShadowEffect
                    {
                        Color = new Color { A = 255, R = 0, G = 0, B = 0 },
                        Direction = 320,
                        ShadowDepth = 1,
                        Opacity = 0.6
                    };

                    FontAwesome.WPF.FontAwesome fai = new FontAwesome.WPF.FontAwesome { Icon = sourceType.Icon, Width = 28, Height = 28, FontSize = 17, Foreground = new SolidColorBrush(Colors.White), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top, Margin = new Thickness(0, 5, 0, 0) };

                    border.Child = fai;

                    stackPanel.Children.Add(border);
                }

                i++;
            }
            grid.Children.Add(stackPanel);
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
                    if (eventHorizonLINQ_SelectedItem != null)
                    {
                        EventWindow editEventWindow = new EventWindow(this, EventWindowModes.NewNote, eventHorizonLINQ_SelectedItem, null);
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
                    DeleteEventRow();
                    break;
            }
        }

        private void DeleteEventRow()
        {
            if (eventHorizonLINQ_SelectedItem != null)
            {
                if (eventHorizonLINQ_SelectedItem.Attributes_Replies > 0)
                {
                    EventHorizonRequesterNotification rorn = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "Error, this event has notes or replies", InformationTextBlock = "You won't be able to delete an event if it has notes or replies.\nYou must delete them first." }, RequesterTypes.OK);
                    rorn.ShowDialog();
                }
                else
                {
                    EventHorizonRequesterNotification orn = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "Delete this event, are you sure", InformationTextBlock = "Consider changing the events status to archived instead.\nThat way you use the event as a log." }, RequesterTypes.NoYes);
                    var result = orn.ShowDialog();
                    if (result == true)
                    {
                        if (eventHorizonLINQ_SelectedItem.ID > 0) DataTableManagement.DeleteEvent(eventHorizonLINQ_SelectedItem.ID);
                    }
                }
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
                        if (eventHorizonLINQ_SelectedItem != null)
                        {
                            EventWindow editEventWindow = new EventWindow(this, EventWindowModes.ViewMainEvent, eventHorizonLINQ_SelectedItem, null);
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

        private void AddItemsToEventTypeComboBox()
        {
            foreach (EventType eventType in XMLReaderWriter.EventTypesList)
            {
                EventTypeComboBox.Items.Add(EventHorizonEventTypes.GetEventTypeStackPanel(eventType));
            }

            EventTypeComboBox.SelectedIndex = 0;
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
                    case ReminderListTimeSpans.TimeSpan_29to60_Days:
                        ReminderListTimeSpan = new TimeSpan(60, 0, 0, 0);
                        break;
                    case ReminderListTimeSpans.TimeSpan_61to90_Days:
                        ReminderListTimeSpan = new TimeSpan(90, 0, 0, 0);
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
                    case 2:
                        //Just Refresh
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

            eventHorizonLINQ_SelectedItem = (EventHorizonLINQ)item.Tag;

            Console.WriteLine();
            Console.WriteLine(">S>>MainWindow ReminderListView_PreviewMouseLeftButtonDown<<<<");
            Console.WriteLine();

            Console.Write("item.Tag eventHorizonLINQ_SelectedItem.Source_Mode = ");
            Console.WriteLine(eventHorizonLINQ_SelectedItem.Source_Mode);

            Console.Write("item.Tag eventHorizonLINQ_SelectedItem.ID = ");
            Console.WriteLine(eventHorizonLINQ_SelectedItem.ID);

            Console.Write("item.Tag eventHorizonLINQ_SelectedItem.Source_ParentEventID = ");
            Console.WriteLine(eventHorizonLINQ_SelectedItem.Source_ParentEventID);

            Console.Write("item.Tag eventHorizonLINQ_SelectedItem.Attributes_Replies = ");
            Console.WriteLine(eventHorizonLINQ_SelectedItem.Attributes_Replies);

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

            eventHorizonLINQ_SelectedItem = (EventHorizonLINQ)item.Tag;

            Console.WriteLine();
            Console.WriteLine(">S>>MainWindow ReminderListView_MouseDoubleClick<<<<");
            Console.WriteLine();

            Console.Write("item.Tag eventHorizonLINQ_SelectedItem.Source_Mode = ");
            Console.WriteLine(eventHorizonLINQ_SelectedItem.Source_Mode);

            Console.Write("item.Tag eventHorizonLINQ_SelectedItem.ID = ");
            Console.WriteLine(eventHorizonLINQ_SelectedItem.ID);

            Console.Write("item.Tag eventHorizonLINQ_SelectedItem.Source_ParentEventID = ");
            Console.WriteLine(eventHorizonLINQ_SelectedItem.Source_ParentEventID);

            Console.Write("item.Tag eventHorizonLINQ_SelectedItem.Attributes_Replies = ");
            Console.WriteLine(eventHorizonLINQ_SelectedItem.Attributes_Replies);

            Console.WriteLine();
            Console.WriteLine(">F>>MainWindow ReminderListView_MouseDoubleClick<<<<");
            Console.WriteLine();

            ReminderListView.SelectedItem = item;

            if (eventHorizonLINQ_SelectedItem != null)
            {
                //try open event as EditEvent
                EventWindow editEventWindow = new EventWindow(this, EventWindowModes.ViewMainEvent, eventHorizonLINQ_SelectedItem, null);
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

        private void TreeViewButtons_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var code = button.Tag.ToString();
            uint switchValue = uint.Parse(code);

            switch (switchValue)
            {
                case TreeViews.Reports:
                    if (ReportsStackPanel.Visibility == System.Windows.Visibility.Collapsed)
                    {
                        ReportsStackPanel.Visibility = System.Windows.Visibility.Visible;
                        ReportsVisualTreeListView.Visibility = System.Windows.Visibility.Visible;
                    }
                    else
                    {
                        ReportsStackPanel.Visibility = System.Windows.Visibility.Collapsed;
                        ReportsVisualTreeListView.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    break;
                case TreeViews.Help:
                    if (HelpStackPanel.Visibility == System.Windows.Visibility.Collapsed)
                    {
                        HelpStackPanel.Visibility = System.Windows.Visibility.Visible;
                        HelpVisualTreeListView.Visibility = System.Windows.Visibility.Visible;
                    }
                    else
                    {
                        HelpStackPanel.Visibility = System.Windows.Visibility.Collapsed;
                        HelpVisualTreeListView.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    break;
            }
        }
        private void LoadReportsVisualTree()
        {
            ListOfReports.Clear();
            ReportsVisualTreeListView.Items.Clear();

            ListOfReports.Add(new SelectionIdString { Id = 0, Name = "Product Items" });
            ListOfReports.Add(new SelectionIdString { Id = 1, Name = "Spare" });
            ListOfReports.Add(new SelectionIdString { Id = 2, Name = "Spare" });
            ListOfReports.Add(new SelectionIdString { Id = 3, Name = "Help" });

            NumberOfReportsTextBlock.Text = ListOfReports.Count.ToString();

            foreach (SelectionIdString ss in ListOfReports)
            {
                ReportsVisualTreeListView.Items.Add(new TextBlock { Tag = ss.Id, Text = " " + ss.Name + " ", Style = (Style)FindResource("TreeViewItemTextBlock") });
            }
        }
        private void LoadHelpVisualTree()
        {
            ListOfHelp.Clear();
            HelpVisualTreeListView.Items.Clear();

            ListOfHelp.Add(new SelectionIdString { Id = 0, Name = "Event Status" });
            ListOfHelp.Add(new SelectionIdString { Id = 1, Name = "Foo" });
            ListOfHelp.Add(new SelectionIdString { Id = 2, Name = "FooBar" });

            NumberOfHelpTextBlock.Text = ListOfHelp.Count.ToString();

            foreach (SelectionIdString ss in ListOfHelp)
            {
                HelpVisualTreeListView.Items.Add(new TextBlock { Tag = ss.Id, Text = " " + ss.Name + " ", Style = (Style)FindResource("TreeViewItemTextBlock") });
            }
        }

        private void ReportsVisualTreeListView_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is ListViewItem))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            ListViewItem item = (ListViewItem)dep;

            item.IsSelected = true;
            e.Handled = true;

            try
            {
                if (ReportsVisualTreeListView.SelectedItem == null)
                {
                    return;
                }

                if (ReportsVisualTreeListView.SelectedItems.Count == 1)
                {
                    ReportsWindow REPORTS;

                    switch (ReportsVisualTreeListView.SelectedIndex)
                    {
                        case Reports.Product:
                            if (eventHorizonLINQ_SelectedItem != null)
                            {
                                REPORTS = new ReportsWindow(eventHorizonLINQ_SelectedItem, GetProductItems(eventHorizonLINQ_SelectedItem), Helps.None);
                                REPORTS.Show();
                            }
                            break;
                        case Reports.Foo:
                            
                            break;
                        case Reports.FooBar:

                            break;
                    }

                    item.IsSelected = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void HelpVisualTreeListView_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is ListViewItem))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            ListViewItem item = (ListViewItem)dep;

            item.IsSelected = true;
            e.Handled = true;

            try
            {
                if (HelpVisualTreeListView.SelectedItem == null)
                {
                    return;
                }

                if (HelpVisualTreeListView.SelectedItems.Count == 1)
                {
                    ReportsWindow REPORTS;

                    switch (HelpVisualTreeListView.SelectedIndex)
                    {
                        case Helps.EventStatus:
                            REPORTS = new ReportsWindow(null, null, Helps.EventStatus);
                            REPORTS.Show();
                            break;
                        case Helps.Foo:
                            REPORTS = new ReportsWindow(null, null, Helps.Foo);
                            REPORTS.Show();
                            break;
                        case Helps.FooBar:
                            REPORTS = new ReportsWindow(null, null, Helps.FooBar);
                            REPORTS.Show();
                            break;
                    }

                    item.IsSelected = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void RecordsToViewRadioButton_Checked(object sender, RoutedEventArgs e)
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
                        case RowLimitModes.NoLimit:
                            DataTableManagement.RowLimitMode = RowLimitModes.NoLimit;
                            RecordsIntegerUpDown.IsEnabled = false;
                            OffsetIntegerUpDown.IsEnabled = false;
                            break;
                        case RowLimitModes.LimitOnly:
                            DataTableManagement.RowLimitMode = RowLimitModes.LimitOnly;
                            RecordsIntegerUpDown.IsEnabled = true;
                            OffsetIntegerUpDown.IsEnabled = false;
                            break;
                        case RowLimitModes.LimitWithOffset:
                            DataTableManagement.RowLimitMode = RowLimitModes.LimitWithOffset;
                            RecordsIntegerUpDown.IsEnabled = true;
                            OffsetIntegerUpDown.IsEnabled = true;
                            break;
                    }
                }

                Console.Write("DataTableManagement.RowLimitMode = ");
                Console.WriteLine(DataTableManagement.RowLimitMode);

                if (MainWindowIs_Loaded)
                {
                    if (DisplayMode == DisplayModes.Reminders)
                        RefreshLog(ListViews.Reminder);
                    else
                        RefreshLog(ListViews.Log);
                }
            }
        }

        private void RecordsIntegerUpDown_Spinned(object sender, SpinEventArgs e)
        {
            DataTableManagement.RowLimit = (int)RecordsIntegerUpDown.Value;
            Console.Write("DataTableManagement.RowLimit = ");
            Console.WriteLine(DataTableManagement.RowLimit);
        }

        private void OffsetIntegerUpDown_Spinned(object sender, SpinEventArgs e)
        {
            DataTableManagement.RowOffset = (int)OffsetIntegerUpDown.Value;
            Console.Write("DataTableManagement.RowOffset = ");
            Console.WriteLine(DataTableManagement.RowOffset);
        }

        private void RightMouseButton_Click(object sender, RoutedEventArgs e)
        {
            MenuItem button = e.OriginalSource as MenuItem;

            int buttonID = 0;

            bool success = Int32.TryParse(button.Tag.ToString(), out buttonID);

            if (button != null && success)
            {
                switch (buttonID)
                {
                    case EventRowContextMenu.ViewAsProduct:
                        Console.WriteLine("View as Product");
                        try
                        {
                            ReportsWindow REPORTS;

                            if (eventHorizonLINQ_SelectedItem != null)
                            {
                                REPORTS = new ReportsWindow(eventHorizonLINQ_SelectedItem, GetProductItems(eventHorizonLINQ_SelectedItem), Helps.None);
                                REPORTS.Show();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    case EventRowContextMenu.Spare:
                        Console.WriteLine("Spare");
                        break;
                    case EventRowContextMenu.Delete:
                        Console.WriteLine("Delete");
                        DeleteEventRow();
                        break;
                    case EventRowContextMenu.Help:
                        Console.WriteLine("Help");
                        try
                        {
                            ReportsWindow REPORTS;

                            REPORTS = new ReportsWindow(null, null, Helps.EventStatus);
                            REPORTS.Show();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                }
            }
        }

        
    }
}