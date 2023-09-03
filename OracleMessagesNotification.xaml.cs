using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;

namespace The_Oracle
{
    /// <summary>
    /// Interaction logic for OracleMessagesNotification.xaml
    /// </summary>
    public partial class OracleMessagesNotification : Window
    {
        MainWindow mw;
        int mode;
        OracleCustomMessage oracleCustomMessage;

        public OracleMessagesNotification(MainWindow mw, int mode, OracleCustomMessage oracleCustomMessage = null)
        {
            InitializeComponent();
            this.Hide();

            this.mw = mw;
            this.mode = mode;
            this.oracleCustomMessage = oracleCustomMessage;

            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            SwitchMode(mode);
        }

        private void SwitchMode(int mode)
        {
            switch (mode)
            {
                case OracleMessagesNotificationModes.Welcome:
                    MessageTitleTextBlock.Text = "Welcome to Event Horizon";
                    InformationTextBlock.Text = "This software solution can be used in the factory, office or home. Designed built by David Paul Jones in 2023 GPLv3";
                    OptionsComboBox.Items.Clear();
                    break;
                case OracleMessagesNotificationModes.OracleSettingsXmlMissing:
                    MessageTitleTextBlock.Text = "EventHorizonLocalSettings.xml file is missing";
                    InformationTextBlock.Text = "Event Horizon could not find a required xml file, located in the Event Horizon install folder!";
                    OptionsComboBox.Items.Clear();
                    OptionsComboBox.Items.Add("Browse for Xml file");
                    OptionsComboBox.Items.Add("Create a new Xml file");
                    break;
                case OracleMessagesNotificationModes.OracleDatabaseSettingsXmlMissing:
                    MessageTitleTextBlock.Text = "EventHorizonRemoteSettings.xml file is missing";
                    InformationTextBlock.Text = "Event Horizon could not find a required xml file, normally located in the EventHorizonRemoteDatabase folder!";
                    OptionsComboBox.Items.Clear();
                    OptionsComboBox.Items.Add("Browse for Xml file");
                    OptionsComboBox.Items.Add("Create a new Xml file");
                    break;
                case OracleMessagesNotificationModes.OracleDatabaseNotFound:
                    MessageTitleTextBlock.Text = "Event Horizon could not connect to a database";
                    InformationTextBlock.Text = "You have a few options, you can browse for the database, create a new database or close Event Horizon and seek IT Support.";
                    OptionsComboBox.Items.Clear();
                    OptionsComboBox.Items.Add("Browse for Database");
                    OptionsComboBox.Items.Add("Create a new Database");
                    break;
                case OracleMessagesNotificationModes.OracleDatabaseError:
                    MessageTitleTextBlock.Text = "Event Horizon had a database error";
                    InformationTextBlock.Text = "The database might be busy or the network could have become slow, Event Horizon will try the last operation again.";
                    OptionsComboBox.Items.Clear();
                    break;
                case OracleMessagesNotificationModes.Custom:
                    if (oracleCustomMessage != null)
                    {
                        MessageTitleTextBlock.Text = oracleCustomMessage.MessageTitleTextBlock;
                        InformationTextBlock.Text = oracleCustomMessage.InformationTextBlock;
                    }
                    OptionsComboBox.Items.Clear();
                    OptionsComboBox.Items.Add("Browse for Database");
                    OptionsComboBox.Items.Add("Create a new Database");
                    break;
            }
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
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
                        switch (mode)
                        {
                            case OracleMessagesNotificationModes.Welcome:
                                switch (OptionsComboBox.SelectedIndex)
                                {
                                    case 0:
                                        break;
                                    case 1:
                                        break;
                                }
                                break;
                            case OracleMessagesNotificationModes.OracleSettingsXmlMissing:
                                switch (OptionsComboBox.SelectedIndex)
                                {
                                    case 0:
                                        OpenFileDialog openFileDialog = new OpenFileDialog();
                                        openFileDialog.Multiselect = true;
                                        openFileDialog.Filter = "Xml files (*.xml)|*.xml|All files (*.*)|*.*";
                                        openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
                                        if (openFileDialog.ShowDialog() == true)
                                        {
                                            mw.Status.Content = "Selected " + openFileDialog.FileName + " file";
                                            string PathString = System.IO.Path.GetDirectoryName(openFileDialog.FileName);

                                            XMLReaderWriter.ReadXMLNodesOracleSettingsXMLFile(PathString);
                                            Close();
                                        }
                                        break;
                                    case 1:
                                        OracleDatabaseCreate.CreateNew_EventHorizonLocalSettings();
                                        mw.Status.Content = "Created and connected to a new Event Horizon settings file";
                                        break;
                                }
                                break;
                            case OracleMessagesNotificationModes.OracleDatabaseSettingsXmlMissing:
                                switch (OptionsComboBox.SelectedIndex)
                                {
                                    case 0:
                                        OpenFileDialog openFileDialog = new OpenFileDialog();
                                        openFileDialog.Multiselect = true;
                                        openFileDialog.Filter = "Xml files (*.xml)|*.xml|All files (*.*)|*.*";
                                        openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
                                        if (openFileDialog.ShowDialog() == true)
                                        {
                                            mw.Status.Content = "Selected " + openFileDialog.FileName + " file";
                                            string PathString = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
                                            XMLReaderWriter.ReadXMLNodesFromOracleDatabaseXMLFile(PathString);
                                            Close();
                                        }
                                        break;
                                    case 1:
                                        OracleDatabaseCreate.CreateNew_EventHorizonRemoteSettings();
                                        mw.Status.Content = "Created and connected to a new Event Horizon database settings file";
                                        break;
                                }
                                break;
                            case OracleMessagesNotificationModes.OracleDatabaseNotFound:
                                switch (OptionsComboBox.SelectedIndex)
                                {
                                    case 0:
                                        OpenFileDialog openFileDialog = new OpenFileDialog();
                                        openFileDialog.Multiselect = true;
                                        openFileDialog.Filter = "Database files (*.mdb)|*.mdb|All files (*.*)|*.*";
                                        openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
                                        if (openFileDialog.ShowDialog() == true)
                                        {
                                            MainWindow.HSE_LOG_GlobalMDBConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source = " + openFileDialog.FileName;
                                            mw.Status.Content = "Selected " + openFileDialog.FileName + " database";
                                            MainWindowTitle.PathString = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
                                            MainWindowTitle.OracleFileName = System.IO.Path.GetFileName(openFileDialog.FileName);
                                            MainWindowTitle.SetMainWindowTitle();
                                            XMLReaderWriter.ReadXMLNodesFromOracleDatabaseXMLFile(MainWindowTitle.PathString);
                                            Close();
                                        }
                                        break;
                                    case 1:
                                        OracleDatabaseCreate.Create_Oracle();
                                        mw.Status.Content = "Created and connected to a new Event Horizon database";
                                        break;
                                }
                                break;
                            case OracleMessagesNotificationModes.OracleDatabaseError:
                                switch (OptionsComboBox.SelectedIndex)
                                {
                                    case 0:
                                        break;
                                    case 1:
                                        break;
                                }
                                break;
                            case OracleMessagesNotificationModes.Custom:
                                switch (OptionsComboBox.SelectedIndex)
                                {
                                    case 0:
                                        OpenFileDialog openFileDialog = new OpenFileDialog();
                                        openFileDialog.Multiselect = true;
                                        openFileDialog.Filter = "Database files (*.mdb)|*.mdb|All files (*.*)|*.*";
                                        openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
                                        if (openFileDialog.ShowDialog() == true)
                                        {
                                            MainWindow.HSE_LOG_GlobalMDBConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source = " + openFileDialog.FileName;
                                            mw.Status.Content = "Selected " + openFileDialog.FileName + " database";
                                            MainWindowTitle.PathString = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
                                            MainWindowTitle.OracleFileName = System.IO.Path.GetFileName(openFileDialog.FileName);
                                            MainWindowTitle.SetMainWindowTitle();
                                            XMLReaderWriter.ReadXMLNodesFromOracleDatabaseXMLFile(MainWindowTitle.PathString);
                                            XMLReaderWriter.WriteSettingsXmlFile(new OracleSettings { UserID = 15, UserName = "David Jones", DatabaseLocation = System.IO.Path.GetDirectoryName(openFileDialog.FileName) });
                                            Close();
                                        }
                                        break;
                                    case 1:
                                        OracleDatabaseCreate.Create_Oracle();
                                        mw.Status.Content = "Created and connected to a new Event Horizon database";
                                        break;
                                }
                                break;
                        }
                        break;
                    case 1:
                        Close();
                        break;
                }
            }
        }
    }
}
