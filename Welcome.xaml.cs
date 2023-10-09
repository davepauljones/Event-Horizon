using System;
using System.Windows;
using FontAwesome.WPF;
using System.Windows.Media;
using System.IO;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;

namespace Event_Horizon
{
    /// <summary>
    /// Interaction logic for Welcome.xaml
    /// </summary>
    public partial class Welcome : Window
    {
        public struct WelcomeButtons
        {
            public const int Logon = 0;
            public const int Retry = 1;
            public const int New_LocalSettingsXML = 2;
            public const int Edit_LocalSettingsXML = 3;
            public const int New_RemoteSettingsXML = 4;
            public const int Edit_RemoteSettingsXML = 5;
            public const int New_RemoteDatabase = 6;
            public const int Select_RemoteDatabase = 7;
            public const int New_User_RemoteSettings = 8;
            public const int Edit_Users_RemoteSettings = 9;
            public const int Select_CurrentUser_RemoteSettings = 10;
            public const int Edit_CurrentUser_LocalSettings = 11;
            public const int New_EventType_RemoteSettings = 12;
            public const int Edit_EventTypes_RemoteSettings = 13;
            public const int New_SourceType_RemoteSettings = 14;
            public const int Edit_SourceTypes_RemoteSettings = 15;
        }

        bool PrerequisitesPassed = false;
        TimeSpan AwaitDelayTimeSpan = TimeSpan.FromMilliseconds(10);

        public Welcome()
        {
            InitializeComponent();

            Init();
        }
        private void Init()
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\EventHorizonLogoHLNN.png"))
            {
                ImageBrush myBrush = new ImageBrush();
                myBrush.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\EventHorizonLogoHLNN.png", UriKind.Absolute));
                myBrush.Stretch = Stretch.Uniform;
                myBrush.Transform = new ScaleTransform(1, 1,45,10);
                HeaderGrid.Background = myBrush;
                HeaderGrid.Background.Opacity = 1;
            }

            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\wormhole1020x1000.png"))
            {
                ImageBrush myBrush = new ImageBrush();
                myBrush.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\wormhole1020x1000.png", UriKind.Absolute));

                InnerMainBorder.Background = myBrush;
                InnerMainBorder.Background.Opacity = 1;
            }

            CheckPrerequisites();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            //Calculate half of the offset to move the form

            if (sizeInfo.HeightChanged)
                this.Top += (sizeInfo.PreviousSize.Height - sizeInfo.NewSize.Height) / 2;

            if (sizeInfo.WidthChanged)
                this.Left += (sizeInfo.PreviousSize.Width - sizeInfo.NewSize.Width) / 2;
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
                    case WelcomeButtons.Logon:
                        if (PrerequisitesPassed)
                            DialogResult = true;
                        else
                            DialogResult = false;
                        Close();
                        break;
                    case WelcomeButtons.Retry:
                        CheckPrerequisites();
                        break;
                    case WelcomeButtons.New_LocalSettingsXML:
                        EventHorizonRequesterNotification norn = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "Create a New Local Settings File", InformationTextBlock = ReusableMessages.WithGreatPowerComesGreatResponsibility + "\n\nAre you sure ?" }, RequesterTypes.NoYes);
                        if (norn.ShowDialog() == true)
                        {

                        }
                        break;
                    case WelcomeButtons.Edit_LocalSettingsXML:
                        EventHorizonRequesterNotification eorn = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "Edit an existing Local Settings File", InformationTextBlock = ReusableMessages.WithGreatPowerComesGreatResponsibility + "\n\nAre you sure ?" }, RequesterTypes.NoYes);
                        if (eorn.ShowDialog() == true)
                        {
                            MiscFunctions.OpenFileInNotepad(AppDomain.CurrentDomain.BaseDirectory + "EventHorizonLocalSettings.xml");
                        }
                        break;
                    case WelcomeButtons.New_RemoteSettingsXML:
                        EventHorizonRequesterNotification nrorn = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "Create a New Remote Settings File", InformationTextBlock = ReusableMessages.WithGreatPowerComesGreatResponsibility + "\n\nAre you sure ?" }, RequesterTypes.NoYes);
                        if (nrorn.ShowDialog() == true)
                        {
                            EventHorizonDatabaseCreate.AddFieldsToExistingTable();
                        }
                        break;
                    case WelcomeButtons.Edit_RemoteSettingsXML:
                        EventHorizonRequesterNotification srsborn = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "Select an existing Remote Settings File", InformationTextBlock = ReusableMessages.WithGreatPowerComesGreatResponsibility + "\n\nAre you sure ?" }, RequesterTypes.NoYes);
                        if (srsborn.ShowDialog() == true)
                        {
                            string pathFileName = EventHorizonDatabaseCreate.OpenRemoteSettingsXMLFile();

                            Console.Write("Selected Remote Settings File = ");
                            Console.WriteLine(pathFileName);

                            Console.Write("Path.GetFileNameWithoutExtension(pathFileName) = ");
                            Console.WriteLine(Path.GetFileNameWithoutExtension(pathFileName));

                            RemoteSettingsLabel.Content = pathFileName;

                            CheckPrerequisites();
                        }
                        break;
                    case WelcomeButtons.New_RemoteDatabase:
                        EventHorizonRequesterNotification nrdborn = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "Create a New Remote Database File", InformationTextBlock = ReusableMessages.WithGreatPowerComesGreatResponsibility + "\n\nAre you sure ?" }, RequesterTypes.NoYes);
                        if (nrdborn.ShowDialog() == true)
                        {
                            EventHorizonDatabaseCreate.CreateSQLiteDatabaseFile();
                        }   
                        break;
                    case WelcomeButtons.Select_RemoteDatabase:
                        EventHorizonRequesterNotification srdborn = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "Select an existing Remote Database File", InformationTextBlock = ReusableMessages.WithGreatPowerComesGreatResponsibility + "\n\nAre you sure ?" }, RequesterTypes.NoYes);
                        if (srdborn.ShowDialog() == true)
                        {
                            string pathFileName = EventHorizonDatabaseCreate.OpenSQLiteDatabaseFile();

                            Console.Write("Selected Database = ");
                            Console.WriteLine(pathFileName);

                            Console.Write("Path.GetFileNameWithoutExtension(pathFileName) = ");
                            Console.WriteLine(Path.GetFileNameWithoutExtension(pathFileName));

                            RemoteDatabaseLabel.Content = pathFileName;

                            CheckPrerequisites();
                        }
                        break;
                    case WelcomeButtons.New_User_RemoteSettings:
                        EventHorizonRequesterNotification nuorn = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "Add a New User to Remote Settings File", InformationTextBlock = ReusableMessages.WithGreatPowerComesGreatResponsibility + "\n\nAre you sure ?" }, RequesterTypes.NoYes);
                        if (nuorn.ShowDialog() == true)
                        {
                            
                        }
                        break;
                    case WelcomeButtons.Edit_Users_RemoteSettings:
                        EventHorizonRequesterNotification eurorn = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "Edit existing Users in Remote Settings File", InformationTextBlock = ReusableMessages.WithGreatPowerComesGreatResponsibility + "\n\nAre you sure ?" }, RequesterTypes.NoYes);
                        if (eurorn.ShowDialog() == true)
                        {
                            MiscFunctions.OpenFileInNotepad(XMLReaderWriter.DatabaseLocationString + "\\" + XMLReaderWriter.DefaultRemoteSettingsFileName);
                        }
                        break;
                    case WelcomeButtons.Select_CurrentUser_RemoteSettings:
                        EventHorizonRequesterNotification scuorn = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "Select the Current User", InformationTextBlock = ReusableMessages.WithGreatPowerComesGreatResponsibility + "\n\nAre you sure ?" }, RequesterTypes.NoYes);
                        if (scuorn.ShowDialog() == true)
                        {

                        }
                        break;
                    case WelcomeButtons.Edit_CurrentUser_LocalSettings:
                        EventHorizonRequesterNotification ecurorn = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "Edit Current User in Local Settings File", InformationTextBlock = ReusableMessages.WithGreatPowerComesGreatResponsibility + "\n\nAre you sure ?" }, RequesterTypes.NoYes);
                        if (ecurorn.ShowDialog() == true)
                        {
                            MiscFunctions.OpenFileInNotepad(AppDomain.CurrentDomain.BaseDirectory + XMLReaderWriter.DefaultLocalSettingsFileName);
                        }
                        break;
                    case WelcomeButtons.New_EventType_RemoteSettings:
                        EventHorizonRequesterNotification netorn = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "Add a New Event Type to Remote Settings File", InformationTextBlock = ReusableMessages.WithGreatPowerComesGreatResponsibility + "\n\nAre you sure ?" }, RequesterTypes.NoYes);
                        if (netorn.ShowDialog() == true)
                        {

                        }
                        break;
                    case WelcomeButtons.Edit_EventTypes_RemoteSettings:
                        EventHorizonRequesterNotification eetrorn = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "Edit Event Types in Remote Settings File", InformationTextBlock = ReusableMessages.WithGreatPowerComesGreatResponsibility + "\n\nAre you sure ?" }, RequesterTypes.NoYes);
                        if (eetrorn.ShowDialog() == true)
                        {
                            MiscFunctions.OpenFileInNotepad(XMLReaderWriter.DatabaseLocationString + "\\" + XMLReaderWriter.DefaultRemoteSettingsFileName);
                        }
                        break;
                    case WelcomeButtons.New_SourceType_RemoteSettings:
                        EventHorizonRequesterNotification nstorn = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "Add a New Source Type to Remote Settings File", InformationTextBlock = ReusableMessages.WithGreatPowerComesGreatResponsibility + "\n\nAre you sure ?" }, RequesterTypes.NoYes);
                        if (nstorn.ShowDialog() == true)
                        {
                            MiscFunctions.OpenFileInNotepad(XMLReaderWriter.DatabaseLocationString + "\\" + XMLReaderWriter.DefaultRemoteSettingsFileName);
                        }
                        break;
                    case WelcomeButtons.Edit_SourceTypes_RemoteSettings:
                        EventHorizonRequesterNotification estrorn = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "Edit Source Types in Remote Settings File", InformationTextBlock = ReusableMessages.WithGreatPowerComesGreatResponsibility + "\n\nAre you sure ?" }, RequesterTypes.NoYes);
                        if (estrorn.ShowDialog() == true)
                        {
                            MiscFunctions.OpenFileInNotepad(XMLReaderWriter.DatabaseLocationString + "\\" + XMLReaderWriter.DefaultRemoteSettingsFileName);
                        }
                        break;
                }
            }
        }

        private async void CheckPrerequisites()
        {
            StatusLabel.Foreground = new SolidColorBrush(Colors.Firebrick);
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
            LoginButton.Visibility = Visibility.Hidden;

            int result = 0;
            
            try
            {
                bool partResult = await Execute_Welcome_Async();
                
                bool part1Result = await Execute_LocalSettings_Async();
                
                bool part2Result = await Execute_RemoteSettings_Async();
                bool part3Result = await Execute_RemoteDatabase_Async();

                
                bool part4Result = await Execute_RemoteUsers_Async();
                bool part5Result = await Execute_LocalUser_Async();
                bool part6Result = await Execute_RemoteEventTypes_Async();
                
                XMLReaderWriter.CreateEventAttributes();

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
                StatusLabel.Foreground = new SolidColorBrush(Colors.Firebrick);
                StatusLabel.Content = "Checking Event Horizon Prerequisites Failed!";
                EventHorizonRequesterNotification msg = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "CheckPrerequisites - ", InformationTextBlock = $"An error occurred: {ex.Message}" }, RequesterTypes.OK);
                msg.ShowDialog();
            }
            
            if (result == 0)
            {
                PrerequisitesPassed = true;
                StatusLabel.Foreground = new SolidColorBrush(Colors.Green);
                StatusLabel.Content = "Checking Event Horizon Prerequisites ......... Passed!";
                SetPrerequisites();
                RetryButton.Visibility = Visibility.Hidden;

                if (XMLReaderWriter.OverridePassword)
                    LoginButton.Content = "Open";
                else
                    LoginButton.Content = "Login";

                LoginButton.Visibility = Visibility.Visible;
            }
            else
            {
                PrerequisitesPassed = false;
                StatusLabel.Foreground = new SolidColorBrush(Colors.Firebrick);
                StatusLabel.Content = "Checking Event Horizon Prerequisites ......... Failed!";
                RetryButton.Visibility = Visibility.Visible;
                LoginButton.Content = "Close";
                LoginButton.Visibility = Visibility.Visible;
            }
        }

        private async Task<bool> Execute_Welcome_Async()
        {
            bool result = true;

            StatusLabel.Content = "Checking Event Horizon Prerequisites .";

            await Task.Delay(1200); // Simulated delay
            return result;
        }

        private async Task<bool> Execute_LocalSettings_Async()
        {
            bool result;

            StatusLabel.Content = "Checking Event Horizon Prerequisites ..";

            string EventHorizonLocalSettingsPathFileName = AppDomain.CurrentDomain.BaseDirectory + "EventHorizonLocalSettings.xml";
            Console.Write("Execute_LocalSettings_Async - Local Folder is ");
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

                    XMLReaderWriter.SetDatabaseConnectionString();
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

            string EventHorizonRemoteSettingsPathFileName = XMLReaderWriter.DatabaseLocationString + "\\" + XMLReaderWriter.DefaultRemoteSettingsFileName;
            Console.Write("Execute_RemoteSettings_Async - Remote Folder is ");
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

            string EventHorizonRemoteDatabasePathFileName = XMLReaderWriter.DatabaseLocationString + "\\" + XMLReaderWriter.GlobalDatabaseString + XMLReaderWriter.GlobalDatabaseFileExtensionString;
            
            Console.Write("Execute_RemoteDatabase_Async - Remote Folder Database is ");
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

        private async Task<bool> Execute_RemoteUsers_Async()
        {
            bool result;

            StatusLabel.Content = "Checking Event Horizon Prerequisites .....";

            string EventHorizonRemoteSettingsPathFileName = XMLReaderWriter.DatabaseLocationString + "\\" + XMLReaderWriter.DefaultRemoteSettingsFileName;
            Console.Write("Execute_RemoteUsers_Async - Remote Folder is ");
            Console.WriteLine(EventHorizonRemoteSettingsPathFileName);

            if (File.Exists(EventHorizonRemoteSettingsPathFileName))
            { 
                InstalledUsersIcon.Icon = FontAwesomeIcon.Check;
                InstalledUsersIcon.Foreground = new SolidColorBrush(Colors.Green);

                if (XMLReaderWriter.TryReadNodesFrom_EventHorizonRemoteSettings_Users(EventHorizonRemoteSettingsPathFileName))
                {
                    result = true;

                    EventHorizonTokens.LoadUsersIntoWelcome(InstalledUsersGrid);
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

        private async Task<bool> Execute_LocalUser_Async()
        {
            bool result;

            StatusLabel.Content = "Checking Event Horizon Prerequisites ......";

            string EventHorizonRemoteSettingsPathFileName = XMLReaderWriter.DatabaseLocationString + "\\" + XMLReaderWriter.DefaultRemoteSettingsFileName;
            Console.Write("Execute_LocalUser_Async - Remote Folder is ");
            Console.WriteLine(EventHorizonRemoteSettingsPathFileName);

            if (File.Exists(EventHorizonRemoteSettingsPathFileName))
            {
                InstalledCurrentUserIcon.Icon = FontAwesomeIcon.Check;
                InstalledCurrentUserIcon.Foreground = new SolidColorBrush(Colors.Green);

                if (XMLReaderWriter.TryReadNodesFrom_EventHorizonRemoteSettings_Users(EventHorizonRemoteSettingsPathFileName))
                {
                    result = true;

                    InstalledCurrentUserGrid.Children.Add(EventHorizonTokens.GetUserAsTokenStackPanel(XMLReaderWriter.UsersList[XMLReaderWriter.UserID]));
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

        private async Task<bool> Execute_RemoteEventTypes_Async()
        {
            bool result;

            StatusLabel.Content = "Checking Event Horizon Prerequisites .......";

            string EventHorizonRemoteSettingsPathFileName = XMLReaderWriter.DatabaseLocationString + "\\" + XMLReaderWriter.DefaultRemoteSettingsFileName;
            Console.Write("Remote Folder is ");
            Console.WriteLine(EventHorizonRemoteSettingsPathFileName);

            if (File.Exists(EventHorizonRemoteSettingsPathFileName))
            {
                InstalledEventTypesIcon.Icon = FontAwesomeIcon.Check;
                InstalledEventTypesIcon.Foreground = new SolidColorBrush(Colors.Green);

                if (XMLReaderWriter.TryReadNodesFrom_EventHorizonRemoteSettings_EventTypes(EventHorizonRemoteSettingsPathFileName))
                {
                    result = true;

                    EventHorizonTokens.LoadEventTypesIntoWelcome(InstalledEventTypesGrid);
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

            string EventHorizonRemoteSettingsPathFileName = XMLReaderWriter.DatabaseLocationString + "\\" + XMLReaderWriter.DefaultRemoteSettingsFileName;
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

                    EventHorizonTokens.LoadSourceTypesIntoWelcome(InstalledSourceTypesGrid);
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