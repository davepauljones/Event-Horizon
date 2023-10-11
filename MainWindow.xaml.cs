using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Globalization;
using Xceed.Wpf.Toolkit;

namespace Event_Horizon
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int EventTypeID = 0;

        internal WidgetDateToday widgetDateToday;
        internal WidgetTimeNow widgetTimeNow;
        internal WidgetDatabaseHealth widgetDatabaseHealth;
        internal WidgetUsersOnline widgetUsersOnline;
        internal WidgetCurrentUser widgetCurrentUser;

        public static MainWindow mw;

        public bool justLoaded = false;

        public List<EventHorizonLINQ> EventHorizonLINQList;

        private static DatabasePoller databasePoller;

        List<SelectionIdString> ListOfReports = new List<SelectionIdString>();
        List<SelectionIdString> ListOfHelp = new List<SelectionIdString>();

        public EventHorizonLINQ eventHorizonLINQ_SelectedItem;

        private void Init_OracleDatabaseFileWatcher()
        {
            databasePoller = new DatabasePoller(XMLReaderWriter.GlobalConnectionString);
            databasePoller.StartPolling();
        }

        public void RunningTask()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                widgetDatabaseHealth.UpdateLastWriteDateTime(DateTime.Now);

                if (DisplayMode == DisplayModes.Reminders)
                    RefreshLog(ListViews.Reminder);
                else
                    RefreshLog(ListViews.Log);
            }));
        }

        public void RunCycle()
        {
            widgetDateToday.SyncDate();
            widgetTimeNow.SyncTime();

            //Check Users online every 60 seconds only executes if second is 0
            if (DateTime.Now.Second == XMLReaderWriter.UserID * 6)//use UserID as to offset actual second used to update
            {
                DataTableManagement.InsertOrUpdateLastTimeOnline(XMLReaderWriter.UserID);
                EventHorizonTokens.LoadUsersIntoOnlineUsersStackPanel(widgetUsersOnline.UsersOnlineStackPanel);
            }
            
            CheckMyUnreadAndMyReminders();
            
            MainWindow.mw.widgetDatabaseHealth.UpdateLastWriteLabel(false);
            
            justLoaded = true;
        }

        private void ReminderListScrollViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //ImageBrush myBrush = new ImageBrush();
            //myBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,/Images/EventHorizonLogoHLNNN.png", UriKind.Absolute));
            //myBrush.Stretch = Stretch.Uniform;
            //myBrush.Transform = new ScaleTransform(0.33, 0.33, 700, ReminderListScrollViewer.ActualHeight);
            //ReminderListScrollViewer.Background = myBrush;
            //ReminderListScrollViewer.Background.Opacity = 1;
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

            widgetDateToday = new WidgetDateToday();
            WidgetDateTodayGrid.Children.Add(widgetDateToday);

            widgetTimeNow = new WidgetTimeNow();
            WidgetTimeNowGrid.Children.Add(widgetTimeNow);

            widgetDatabaseHealth = new WidgetDatabaseHealth();
            WidgetDatabaseHealthGrid.Children.Add(widgetDatabaseHealth);

            widgetUsersOnline = new WidgetUsersOnline();
            WidgetUsersOnlineGrid.Children.Add(widgetUsersOnline);

            widgetCurrentUser = new WidgetCurrentUser();
            WidgetCurrentUserGrid.Children.Add(widgetCurrentUser);

            if (EventHorizonDatabaseCreate.CheckIfDatabaseExists())
            {
                Init_OracleDatabaseFileWatcher();

                RefreshXML();

                CheckMyUnreadAndMyReminders();

                DataTableManagement.InsertOrUpdateLastTimeOnline(XMLReaderWriter.UserID);

                EventHorizonTokens.LoadUsersIntoOnlineUsersStackPanel(widgetUsersOnline.UsersOnlineStackPanel);
            }

            MainWindowIs_Loaded = true;
        }

        public void RefreshXML()
        {
            widgetCurrentUser.CurrentUserStackPanel.Children.Add(EventHorizonTokens.GetUserAsTokenStackPanel(XMLReaderWriter.UsersList[XMLReaderWriter.UserID]));
            EventHorizonTokens.LoadUsersIntoOnlineUsersStackPanel(widgetUsersOnline.UsersOnlineStackPanel);
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

            if (notificationsAddedThisCycle > 0) MiscFunctions.PlayFile(AppDomain.CurrentDomain.BaseDirectory + "\\Audio\\Notification.mp3");
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

                    EventRow eventRow = EventRow.CreateEventLogRow(eventHorizonLINQ);

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
                            EventRow er = EventRow.CreateEventLogRow(eventHorizonLINQRow);

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

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F1:
                    FunctionKeyManager.GetEventTypeFromFunctionKey(1);
                    break;
                case Key.F2:
                    FunctionKeyManager.GetEventTypeFromFunctionKey(2);
                    break;
                case Key.F3:
                    FunctionKeyManager.GetEventTypeFromFunctionKey(3);
                    break;
                case Key.F4:
                    FunctionKeyManager.GetEventTypeFromFunctionKey(4);
                    break;
                case Key.F5:
                    FunctionKeyManager.GetEventTypeFromFunctionKey(5);
                    break;
                case Key.F6:
                    FunctionKeyManager.GetEventTypeFromFunctionKey(6);
                    break;
                case Key.F7:
                    FunctionKeyManager.GetEventTypeFromFunctionKey(7);
                    break;
                case Key.F8:
                    FunctionKeyManager.GetEventTypeFromFunctionKey(8);
                    break;
                case Key.F9:
                    FunctionKeyManager.GetEventTypeFromFunctionKey(9);
                    break;
                case Key.System:
                    if (e.SystemKey == Key.F10)
                    {
                        FunctionKeyManager.GetEventTypeFromFunctionKey(10);
                    }
                    break;
                case Key.F11:
                    FunctionKeyManager.GetEventTypeFromFunctionKey(11);
                    break;
                case Key.F12:
                    FunctionKeyManager.GetEventTypeFromFunctionKey(12);
                    break;
                case Key.Delete:
                    DeleteEventRow();
                    break;
                case Key.Pause:// && (Keyboard.Modifiers & (ModifierKeys.Alt)) == (ModifierKeys.Alt))
                    Console.WriteLine("Key.Pause");
                    FunctionKeyManager.ToggleFunctionKeyBank();
                    break;
            }
        }

        internal void NewEventWindow(int eventType)
        {
            if (XMLReaderWriter.EventTypesList.Count > eventType)
            {
                EventWindow eventWindow = new EventWindow(this, EventWindowModes.NewEvent, new EventHorizonLINQ
                {
                    EventTypeID = XMLReaderWriter.EventTypesList[eventType].ID,
                }, null);
                eventWindow.Show();
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
                        FunctionKeyManager.SetEventTypeFromFunctionKey(1);
                        break;
                    case 1:
                        FunctionKeyManager.SetEventTypeFromFunctionKey(2);
                        break;
                    case 2:
                        FunctionKeyManager.SetEventTypeFromFunctionKey(3);
                        break;
                    case 3:
                        FunctionKeyManager.SetEventTypeFromFunctionKey(4);
                        break;
                    case 4:
                        FunctionKeyManager.SetEventTypeFromFunctionKey(5);
                        break;
                    case 5:
                        FunctionKeyManager.SetEventTypeFromFunctionKey(6);
                        break;
                    case 6:
                        FunctionKeyManager.SetEventTypeFromFunctionKey(7);
                        break;
                    case 7:
                        FunctionKeyManager.SetEventTypeFromFunctionKey(8);
                        break;
                    case 8:
                        FunctionKeyManager.SetEventTypeFromFunctionKey(9);
                        break;
                    case 9:
                        FunctionKeyManager.SetEventTypeFromFunctionKey(10);
                        break;
                    case 10:
                        FunctionKeyManager.SetEventTypeFromFunctionKey(11);
                        break;
                    case 11:
                        FunctionKeyManager.SetEventTypeFromFunctionKey(12);
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
                        FunctionKeyManager.GetEventTypeFromFunctionKey(1);
                        break;
                    case 1:
                        FunctionKeyManager.GetEventTypeFromFunctionKey(2);
                        break;
                    case 2:
                        FunctionKeyManager.GetEventTypeFromFunctionKey(3);
                        break;
                    case 3:
                        FunctionKeyManager.GetEventTypeFromFunctionKey(4);
                        break;
                    case 4:
                        FunctionKeyManager.GetEventTypeFromFunctionKey(5);
                        break;
                    case 5:
                        FunctionKeyManager.GetEventTypeFromFunctionKey(6);
                        break;
                    case 6:
                        FunctionKeyManager.GetEventTypeFromFunctionKey(7);
                        break;
                    case 7:
                        FunctionKeyManager.GetEventTypeFromFunctionKey(8);
                        break;
                    case 8:
                        FunctionKeyManager.GetEventTypeFromFunctionKey(9);
                        break;
                    case 9:
                        FunctionKeyManager.GetEventTypeFromFunctionKey(10);
                        break;
                    case 10:
                        FunctionKeyManager.GetEventTypeFromFunctionKey(11);
                        break;
                    case 11:
                        FunctionKeyManager.GetEventTypeFromFunctionKey(12);
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
                        if (MainWindowIs_Loaded)
                        {
                            if (DisplayMode == DisplayModes.Reminders)
                                RefreshLog(ListViews.Reminder);
                            else
                                RefreshLog(ListViews.Log);

                            ReminderListScrollViewer.ScrollToTop();
                        }
                        break;
                    case 1:
                        SearchTextBox.Text = string.Empty;
                        if (MainWindowIs_Loaded)
                        {
                            if (DisplayMode == DisplayModes.Reminders)
                                RefreshLog(ListViews.Reminder);
                            else
                                RefreshLog(ListViews.Log);

                            ReminderListScrollViewer.ScrollToTop();
                        }
                        break;
                    case 2:
                        if (MainWindowIs_Loaded)
                        {
                            if (DisplayMode == DisplayModes.Reminders)
                                RefreshLog(ListViews.Reminder);
                            else
                                RefreshLog(ListViews.Log);

                            ReminderListScrollViewer.ScrollToTop();
                        }
                        break;
                    case 3:
                        EventWindow newEventWindow = new EventWindow(this, EventWindowModes.NewEvent, new EventHorizonLINQ(), null);
                        newEventWindow.Show();
                        break;
                }
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

            if (MainWindowIs_Loaded)
            {
                if (DisplayMode == DisplayModes.Reminders)
                    RefreshLog(ListViews.Reminder);
                else
                    RefreshLog(ListViews.Log);
            }
        }

        private void OffsetIntegerUpDown_Spinned(object sender, SpinEventArgs e)
        {
            DataTableManagement.RowOffset = (int)OffsetIntegerUpDown.Value;
            Console.Write("DataTableManagement.RowOffset = ");
            Console.WriteLine(DataTableManagement.RowOffset);

            if (MainWindowIs_Loaded)
            {
                if (DisplayMode == DisplayModes.Reminders)
                    RefreshLog(ListViews.Reminder);
                else
                    RefreshLog(ListViews.Log);
            }
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