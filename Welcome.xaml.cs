using System;
using System.Windows;
using FontAwesome.WPF;
using System.Windows.Media;
using System.IO;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;

namespace The_Oracle
{
    /// <summary>
    /// Interaction logic for Welcome.xaml
    /// </summary>
    public partial class Welcome : Window
    {
        bool PrerequisitesPassed = false;
        TimeSpan AwaitDelayTimeSpan = TimeSpan.FromMilliseconds(100);

        public Welcome()
        {
            InitializeComponent();

            Init();
        }
        private void Init()
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\wormhole1000x600.png"))
            {
                ImageBrush myBrush = new ImageBrush();
                myBrush.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\wormhole1000x600.png", UriKind.Absolute));

                MainBorder.Background = myBrush;
                MainBorder.Background.Opacity = 1;
            }

            CheckPrerequisites();
        }

        private void Welcome_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        public void SetPrerequisites()
        {
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

            InstalledCurrentUserNodesLabel.Content = 1;
            InstalledUsersNodesLabel.Content = XMLReaderWriter.UsersList.Count-1;
            InstalledEventTypesNodesLabel.Content = XMLReaderWriter.EventTypesList.Count-1;
            InstalledSourceTypesNodesLabel.Content = XMLReaderWriter.SourceTypesList.Count-1;
        }

        private void WelcomeButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            int buttonID = 255;

            bool success = Int32.TryParse(button.Tag.ToString(), out buttonID);

            if (button != null && success)
            {

                switch (buttonID)
                {
                    case 0:
                        if (PrerequisitesPassed)
                            DialogResult = true;
                        else
                            DialogResult = false;
                        Close();
                        break;
                    case 1:
                        CheckPrerequisites();
                        break;
                    case 2:
                        MiscFunctions.OpenFileInNotepad(AppDomain.CurrentDomain.BaseDirectory + "EventHorizonLocalSettings.xml");
                        break;
                    case 3:
                        MiscFunctions.OpenFileInNotepad(XMLReaderWriter.DatabaseLocationString + "\\EventHorizonRemoteSettings.xml");
                        break;
                    case 4:
                        
                        break;
                    case 5:
                        MiscFunctions.OpenFileInNotepad(AppDomain.CurrentDomain.BaseDirectory + "EventHorizonLocalSettings.xml");
                        break;
                    case 6:                       
                        MiscFunctions.OpenFileInNotepad(XMLReaderWriter.DatabaseLocationString + "\\EventHorizonRemoteSettings.xml");
                        break;
                    case 7:
                        MiscFunctions.OpenFileInNotepad(XMLReaderWriter.DatabaseLocationString + "\\EventHorizonRemoteSettings.xml");
                        break;
                    case 8:
                        MiscFunctions.OpenFileInNotepad(XMLReaderWriter.DatabaseLocationString + "\\EventHorizonRemoteSettings.xml");
                        break;
                }
            }
        }

        private async void CheckPrerequisites()
        {
            LocalSettingsIcon.Visibility = Visibility.Hidden;
            LocalSettingsLabel.Content = string.Empty;
            RemoteSettingsIcon.Visibility = Visibility.Hidden;
            RemoteSettingsLabel.Content = string.Empty;
            RemoteDatabaseIcon.Visibility = Visibility.Hidden;
            RemoteDatabaseLabel.Content = string.Empty;

            InstalledCurrentUserGrid.Children.Clear();
            InstalledUsersGrid.Children.Clear();
            InstalledEventTypesGrid.Children.Clear();
            InstalledSourceTypesGrid.Children.Clear();

            LocalSettingsModifiedLabel.Content = string.Empty;
            RemoteSettingsModifiedLabel.Content = string.Empty;
            RemoteDatabaseModifiedLabel.Content = string.Empty;

            LocalSettingsSizeLabel.Content = string.Empty;
            RemoteSettingsSizeLabel.Content = string.Empty;
            RemoteDatabaseSizeLabel.Content = string.Empty;

            InstalledCurrentUserNodesLabel.Content = string.Empty;
            InstalledUsersNodesLabel.Content = string.Empty;
            InstalledEventTypesNodesLabel.Content = string.Empty;
            InstalledSourceTypesNodesLabel.Content = string.Empty;

            InstalledCurrentUserIcon.Visibility = Visibility.Hidden;
            InstalledUsersIcon.Visibility = Visibility.Hidden;
            InstalledEventTypesIcon.Visibility = Visibility.Hidden;
            InstalledSourceTypesIcon.Visibility = Visibility.Hidden;

            RetryButton.Visibility = Visibility.Hidden;
            CloseButton.Visibility = Visibility.Hidden;

            int result = 0;
            
            try
            {
                bool partResult = await Execute_Welcome_Async();
                bool part1Result = await Execute_LocalSettings_Async();
                bool part2Result = await Execute_RemoteSettings_Async();
                bool part3Result = await Execute_RemoteDatabase_Async();

                bool part4Result = await Execute_LocalUser_Async();
                bool part5Result = await Execute_RemoteUsers_Async();
                bool part6Result = await Execute_RemoteEventTypes_Async();
                bool part7Result = await Execute_RemoteSourceTypes_Async();

                if (!part1Result)
                    result++;
                else if (!part2Result)
                    result++;
                else if (!part3Result)
                    result++;
                else if (!part4Result)
                    result++;
                else if (!part5Result)
                    result++;
                else if (!part6Result)
                    result++;
                else if (!part7Result)
                    result++;
            }
            catch (Exception ex)
            {
                result++;
                StatusLabel.Content = "Checking Event Horizon Prerequisites Failed!";
                OracleRequesterNotification msg = new OracleRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "CheckPrerequisites - ", InformationTextBlock = $"An error occurred: {ex.Message}" }, RequesterTypes.OK);
                msg.ShowDialog();
            }
            
            if (result == 0)
            {
                PrerequisitesPassed = true;
                StatusLabel.Content = "Checking Event Horizon Prerequisites ......... Passed!";
                SetPrerequisites();
                CloseButton.Visibility = Visibility.Visible;
            }
            else
            {
                PrerequisitesPassed = false;               
                StatusLabel.Content = "Checking Event Horizon Prerequisites ......... Failed!";
                RetryButton.Visibility = Visibility.Visible;
                CloseButton.Visibility = Visibility.Visible;
            }
        }

        private async Task<bool> Execute_Welcome_Async()
        {
            bool result = true;

            StatusLabel.Content = "Checking Event Horizon Prerequisites .";

            await Task.Delay(2000); // Simulated delay
            return result;
        }

        private async Task<bool> Execute_LocalSettings_Async()
        {
            bool result;

            StatusLabel.Content = "Checking Event Horizon Prerequisites ..";

            string EventHorizonLocalSettingsPathFileName = AppDomain.CurrentDomain.BaseDirectory + "EventHorizonLocalSettings.xml";
            Console.Write("Local Folder is ");
            Console.WriteLine(EventHorizonLocalSettingsPathFileName);
            if (File.Exists(EventHorizonLocalSettingsPathFileName))
            {
                result = true;
                LocalSettingsIcon.Icon = FontAwesomeIcon.Check;
                LocalSettingsIcon.Foreground = new SolidColorBrush(Colors.Green);

                if (XMLReaderWriter.TryReadNodesFrom_EventHorizonSettingsXMLFile(EventHorizonLocalSettingsPathFileName))
                {
                    LocalSettingsLabel.Content = EventHorizonLocalSettingsPathFileName;

                    FileModified localSettingsFileModified = MiscFunctions.GetFileModifiedDateTime(EventHorizonLocalSettingsPathFileName);
                    LocalSettingsModifiedLabel.Content = localSettingsFileModified.LastWriteTime;
                    LocalSettingsSizeLabel.Content = localSettingsFileModified.Size;
                }
            }
            else
            {
                result = false;
                LocalSettingsIcon.Icon = FontAwesomeIcon.Times;
                LocalSettingsIcon.Foreground = new SolidColorBrush(Colors.Firebrick);
            }

            LocalSettingsIcon.Visibility = Visibility.Visible;

            await Task.Delay(AwaitDelayTimeSpan); // Simulated delay
            return result;
        }

        private async Task<bool> Execute_RemoteSettings_Async()
        {
            bool result;

            StatusLabel.Content = "Checking Event Horizon Prerequisites ...";

            string EventHorizonRemoteSettingsPathFileName = XMLReaderWriter.DatabaseLocationString + "\\EventHorizonRemoteSettings.xml";
            Console.Write("Remote Folder is ");
            Console.WriteLine(EventHorizonRemoteSettingsPathFileName);

            if (File.Exists(EventHorizonRemoteSettingsPathFileName))
            {
                result = true;
                RemoteSettingsIcon.Icon = FontAwesomeIcon.Check;
                RemoteSettingsIcon.Foreground = new SolidColorBrush(Colors.Green);

                RemoteSettingsLabel.Content = EventHorizonRemoteSettingsPathFileName;

                FileModified RemoteSettingsFileModified = MiscFunctions.GetFileModifiedDateTime(EventHorizonRemoteSettingsPathFileName);
                RemoteSettingsModifiedLabel.Content = RemoteSettingsFileModified.LastWriteTime;
                RemoteSettingsSizeLabel.Content = RemoteSettingsFileModified.Size;
            }
            else
            {
                result = false;
                RemoteSettingsIcon.Icon = FontAwesomeIcon.Times;
                RemoteSettingsIcon.Foreground = new SolidColorBrush(Colors.Firebrick);
            }

            RemoteSettingsIcon.Visibility = Visibility.Visible;

            await Task.Delay(AwaitDelayTimeSpan); // Simulated delay
            return result;
        }

        private async Task<bool> Execute_RemoteDatabase_Async()
        {
            bool result;

            StatusLabel.Content = "Checking Event Horizon Prerequisites ....";

            string EventHorizonRemoteDatabasePathFileName = XMLReaderWriter.DatabaseLocationString + "\\EventHorizonRemoteDatabase.mdb";
            Console.Write("Remote Folder Database is ");
            Console.WriteLine(EventHorizonRemoteDatabasePathFileName);

            if (File.Exists(EventHorizonRemoteDatabasePathFileName))
            {
                result = true;
                RemoteDatabaseIcon.Icon = FontAwesomeIcon.Check;
                RemoteDatabaseIcon.Foreground = new SolidColorBrush(Colors.Green);

                RemoteDatabaseLabel.Content = EventHorizonRemoteDatabasePathFileName;

                FileModified RemoteDatabaseFileModified = MiscFunctions.GetFileModifiedDateTime(EventHorizonRemoteDatabasePathFileName);
                RemoteDatabaseModifiedLabel.Content = RemoteDatabaseFileModified.LastWriteTime;
                RemoteDatabaseSizeLabel.Content = RemoteDatabaseFileModified.Size;
            }
            else
            {
                result = false;
                RemoteDatabaseIcon.Icon = FontAwesomeIcon.Times;
                RemoteDatabaseIcon.Foreground = new SolidColorBrush(Colors.Firebrick);
            }

            RemoteDatabaseIcon.Visibility = Visibility.Visible;

            await Task.Delay(AwaitDelayTimeSpan); // Simulated delay
            return result;
        }

        private async Task<bool> Execute_LocalUser_Async()
        {
            bool result;

            StatusLabel.Content = "Checking Event Horizon Prerequisites .....";

            string EventHorizonRemoteSettingsPathFileName = XMLReaderWriter.DatabaseLocationString + "\\EventHorizonRemoteSettings.xml";
            Console.Write("Remote Folder is ");
            Console.WriteLine(EventHorizonRemoteSettingsPathFileName);

            if (File.Exists(EventHorizonRemoteSettingsPathFileName))
            {
                InstalledCurrentUserIcon.Icon = FontAwesomeIcon.Check;
                InstalledCurrentUserIcon.Foreground = new SolidColorBrush(Colors.Green);

                if (XMLReaderWriter.TryReadNodesFrom_EventHorizonRemoteSettings_Users(EventHorizonRemoteSettingsPathFileName))
                {
                    result = true;

                    MainWindow.mw.LoadCurrentUserIntoGrid(InstalledCurrentUserGrid);
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                result = false;
                InstalledCurrentUserIcon.Icon = FontAwesomeIcon.Times;
                InstalledCurrentUserIcon.Foreground = new SolidColorBrush(Colors.Firebrick);
            }

            InstalledCurrentUserIcon.Visibility = Visibility.Visible;

            await Task.Delay(AwaitDelayTimeSpan); // Simulated delay
            return result;
        }

        private async Task<bool> Execute_RemoteUsers_Async()
        {
            bool result;

            StatusLabel.Content = "Checking Event Horizon Prerequisites ......";

            string EventHorizonRemoteSettingsPathFileName = XMLReaderWriter.DatabaseLocationString + "\\EventHorizonRemoteSettings.xml";
            Console.Write("Remote Folder is ");
            Console.WriteLine(EventHorizonRemoteSettingsPathFileName);

            if (File.Exists(EventHorizonRemoteSettingsPathFileName))
            { 
                InstalledUsersIcon.Icon = FontAwesomeIcon.Check;
                InstalledUsersIcon.Foreground = new SolidColorBrush(Colors.Green);

                if (XMLReaderWriter.TryReadNodesFrom_EventHorizonRemoteSettings_Users(EventHorizonRemoteSettingsPathFileName))
                {
                    result = true;

                    MainWindow.mw.LoadUsersIntoWelcome(InstalledUsersGrid);
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                result = false;
                InstalledUsersIcon.Icon = FontAwesomeIcon.Times;
                InstalledUsersIcon.Foreground = new SolidColorBrush(Colors.Firebrick);
            }

            InstalledUsersIcon.Visibility = Visibility.Visible;

            await Task.Delay(AwaitDelayTimeSpan); // Simulated delay
            return result;
        }

        private async Task<bool> Execute_RemoteEventTypes_Async()
        {
            bool result;

            StatusLabel.Content = "Checking Event Horizon Prerequisites .......";

            string EventHorizonRemoteSettingsPathFileName = XMLReaderWriter.DatabaseLocationString + "\\EventHorizonRemoteSettings.xml";
            Console.Write("Remote Folder is ");
            Console.WriteLine(EventHorizonRemoteSettingsPathFileName);

            if (File.Exists(EventHorizonRemoteSettingsPathFileName))
            {
                InstalledEventTypesIcon.Icon = FontAwesomeIcon.Check;
                InstalledEventTypesIcon.Foreground = new SolidColorBrush(Colors.Green);

                if (XMLReaderWriter.TryReadNodesFrom_EventHorizonRemoteSettings_EventTypes(EventHorizonRemoteSettingsPathFileName))
                {
                    result = true;

                    MainWindow.mw.LoadEventTypesIntoWelcome(InstalledEventTypesGrid);
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                result = false;
                InstalledEventTypesIcon.Icon = FontAwesomeIcon.Times;
                InstalledEventTypesIcon.Foreground = new SolidColorBrush(Colors.Firebrick);
            }

            InstalledEventTypesIcon.Visibility = Visibility.Visible;

            await Task.Delay(AwaitDelayTimeSpan); // Simulated delay
            return result;
        }

        private async Task<bool> Execute_RemoteSourceTypes_Async()
        {
            bool result;

            StatusLabel.Content = "Checking Event Horizon Prerequisites ........";

            string EventHorizonRemoteSettingsPathFileName = XMLReaderWriter.DatabaseLocationString + "\\EventHorizonRemoteSettings.xml";
            Console.Write("Remote Folder is ");
            Console.WriteLine(EventHorizonRemoteSettingsPathFileName);

            if (File.Exists(EventHorizonRemoteSettingsPathFileName))
            {
                result = true;

                InstalledSourceTypesIcon.Icon = FontAwesomeIcon.Check;
                InstalledSourceTypesIcon.Foreground = new SolidColorBrush(Colors.Green);

                if (XMLReaderWriter.TryReadNodesFrom_EventHorizonRemoteSettings_SourceTypes(EventHorizonRemoteSettingsPathFileName))
                {
                    result = true;

                    MainWindow.mw.LoadSourceTypesIntoWelcome(InstalledSourceTypesGrid);
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                result = false;
                InstalledSourceTypesIcon.Icon = FontAwesomeIcon.Times;
                InstalledSourceTypesIcon.Foreground = new SolidColorBrush(Colors.Firebrick);
            }

            InstalledSourceTypesIcon.Visibility = Visibility.Visible;

            await Task.Delay(AwaitDelayTimeSpan); // Simulated delay
            return result;
        }
    }
}