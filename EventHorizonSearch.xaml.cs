using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;

namespace Event_Horizon
{
    /// <summary>
    /// Interaction logic for EventHorizonSearch.xaml
    /// </summary>
    public partial class EventHorizonSearch : UserControl
    {
        public delegate void SearchCallbackDelegate(string value);
        public delegate void EventTypeCallbackDelegate(int value);

        SearchCallbackDelegate Search_Callback;
        EventTypeCallbackDelegate EventType_Callback;

        public EventHorizonSearch(SearchCallbackDelegate Search_Callback, EventTypeCallbackDelegate EventType_Callback)
        {
            InitializeComponent();

            this.Search_Callback = Search_Callback;
            this.EventType_Callback = EventType_Callback;

            Init_RowLimitRowStepControls();
        }

        public void Init_RowLimitRowStepControls()
        {
            Background = new SolidColorBrush(Colors.Transparent);
        }

        public void IsControlEnabled(bool isEnabled)
        {
            if (isEnabled)
            {
                MainWindowWindow.Opacity = 1;
            }
            else
            {
                MainWindowWindow.Opacity = 0.7;
            }
        }

        private void EventTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EventType_Callback(EventTypeComboBox.SelectedIndex);
        }

        private void SearchTextBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Search_Callback(SearchTextBox.Text);
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = e.OriginalSource as Button;

            int buttonID = 0;

            bool success = Int32.TryParse(button.Tag.ToString(), out buttonID);

            if (button != null && success)
            {
                switch (buttonID)
                {
                    case 0:
                        MainWindow.mw.eventHorizonSearch.SearchTextBox.Text = string.Empty;
                        if (MainWindow.mw.MainWindowIs_Loaded)
                        {
                            if (MainWindow.mw.DisplayMode == DisplayModes.Reminders)
                                MainWindow.mw.RefreshLog(ListViews.Reminder);
                            else
                                MainWindow.mw.RefreshLog(ListViews.Log);

                            MainWindow.mw.ReminderListScrollViewer.ScrollToTop();
                        }
                        break;
                    case 1:
                        MainWindow.mw.eventHorizonSearch.EventTypeComboBox.SelectedIndex = 0;
                        MainWindow.mw.EventTypeID = MainWindow.mw.eventHorizonSearch.EventTypeComboBox.SelectedIndex;
                        MainWindow.mw.eventHorizonSearch.SearchTextBox.Text = string.Empty;
                        break;
                }
            }
        }

    }
}
