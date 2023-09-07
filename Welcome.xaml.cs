using System;
using System.Windows;
using FontAwesome.WPF;
using System.Windows.Media;
using System.IO;
using System.Windows.Input;

namespace The_Oracle
{
    /// <summary>
    /// Interaction logic for Welcome.xaml
    /// </summary>
    public partial class Welcome : Window
    {
        public Welcome()
        {
            InitializeComponent();

            Init();
        }
        private void Init()
        {
            SetPrerequisites();
        }

        private void Welcome_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        public void SetPrerequisites()
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "EventHorizonLocalSettings.xml"))
            {
                LocalSettingsIcon.Icon = FontAwesomeIcon.Check;
                LocalSettingsIcon.Foreground = new SolidColorBrush(Colors.Green);
            }
            else
            {
                LocalSettingsIcon.Icon = FontAwesomeIcon.Times;
                LocalSettingsIcon.Foreground = new SolidColorBrush(Colors.Firebrick);
            }

            if (File.Exists(XMLReaderWriter.DatabaseLocationString + "\\EventHorizonRemoteSettings.xml"))
            {
                RemoteSettingsIcon.Icon = FontAwesomeIcon.Check;
                RemoteSettingsIcon.Foreground = new SolidColorBrush(Colors.Green);
            }
            else
            {
                RemoteSettingsIcon.Icon = FontAwesomeIcon.Times;
                RemoteSettingsIcon.Foreground = new SolidColorBrush(Colors.Firebrick);
            }

            if (File.Exists(XMLReaderWriter.DatabaseLocationString + "\\EventHorizonRemoteDatabase.mdb"))
            {
                RemoteDatabaseIcon.Icon = FontAwesomeIcon.Check;
                RemoteDatabaseIcon.Foreground = new SolidColorBrush(Colors.Green);
            }
            else
            {
                RemoteDatabaseIcon.Icon = FontAwesomeIcon.Times;
                RemoteDatabaseIcon.Foreground = new SolidColorBrush(Colors.Firebrick);
            }

            if (XMLReaderWriter.UsersList.Count > 0)
            {
                InstalledUsersIcon.Icon = FontAwesomeIcon.Check;
                InstalledUsersIcon.Foreground = new SolidColorBrush(Colors.Green);
            }
            else
            {
                InstalledUsersIcon.Icon = FontAwesomeIcon.Times;
                InstalledUsersIcon.Foreground = new SolidColorBrush(Colors.Firebrick);
            }

            if (XMLReaderWriter.EventTypesList.Count > 0)
            {
                InstalledEventTypesIcon.Icon = FontAwesomeIcon.Check;
                InstalledEventTypesIcon.Foreground = new SolidColorBrush(Colors.Green);
            }
            else
            {
                InstalledEventTypesIcon.Icon = FontAwesomeIcon.Times;
                InstalledEventTypesIcon.Foreground = new SolidColorBrush(Colors.Firebrick);
            }

            if (XMLReaderWriter.SourceTypesList.Count > 0)
            {
                InstalledSourceTypesIcon.Icon = FontAwesomeIcon.Check;
                InstalledSourceTypesIcon.Foreground = new SolidColorBrush(Colors.Green);
            }
            else
            {
                InstalledSourceTypesIcon.Icon = FontAwesomeIcon.Times;
                InstalledSourceTypesIcon.Foreground = new SolidColorBrush(Colors.Firebrick);
            }

            LocalSettingsLabel.Content = AppDomain.CurrentDomain.BaseDirectory + "EventHorizonLocalSettings.xml";
            RemoteSettingsLabel.Content = XMLReaderWriter.DatabaseLocationString + "\\EventHorizonRemoteSettings.xml";
            RemoteDatabaseLabel.Content = XMLReaderWriter.DatabaseLocationString + "\\EventHorizonRemoteDatabase.mdb";

            MainWindow.mw.LoadUsersIntoWelcome(InstalledUsersGrid);
            MainWindow.mw.LoadEventTypesIntoWelcome(InstalledEventTypesGrid);
            MainWindow.mw.LoadSourceTypesIntoWelcome(InstalledSourceTypesGrid);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}