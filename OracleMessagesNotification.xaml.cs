using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;

namespace The_Oracle
{
    /// <summary>
    /// Interaction logic for OracleMessagesNotification.xaml
    /// </summary>
    public partial class OracleMessagesNotification : Window
    {
        MainWindow mw;
        int Mode;
        OracleCustomMessage ocm;

        public OracleMessagesNotification(MainWindow mw, int Mode, OracleCustomMessage ocm = null)
        {
            InitializeComponent();
            this.Hide();

            this.mw = mw;
            this.Mode = Mode;
            this.ocm = ocm;

            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            SwitchMode(Mode);
        }

        private void SwitchMode(int Mode)
        {
            switch (Mode)
            {
                case OracleMessagesNotificationModes.Welcome:
                    MessageTitleTextBlock.Text = "Welcome to The Oracle";
                    InformationTextBlock.Text = "This software solution can be used in the factory, office or home. Designed built by David Paul Jones in 2023 GPLv3";
                    OptionsComboBox.Items.Clear();
                    break;
                case OracleMessagesNotificationModes.OracleSettingsXmlMissing:
                    MessageTitleTextBlock.Text = "OracleSettingsXml.xml file is missing";
                    InformationTextBlock.Text = "Oracle could not find a required xml file, located in the Oracle install folder!";
                    OptionsComboBox.Items.Clear();
                    OptionsComboBox.Items.Add("Browse for Xml file");
                    OptionsComboBox.Items.Add("Create a new Xml file");
                    break;
                case OracleMessagesNotificationModes.OracleDatabaseSettingsXmlMissing:
                    MessageTitleTextBlock.Text = "OracleDatabaseSettingsXml.xml file is missing";
                    InformationTextBlock.Text = "Oracle could not find a required xml file, normally located in the OracleDatabase folder!";
                    OptionsComboBox.Items.Clear();
                    OptionsComboBox.Items.Add("Browse for Xml file");
                    OptionsComboBox.Items.Add("Create a new Xml file");
                    break;
                case OracleMessagesNotificationModes.OracleDatabaseNotFound:
                    MessageTitleTextBlock.Text = "Oracle could not connect to a database";
                    InformationTextBlock.Text = "You have a few options, you can browse for the database, create a new database or close Oracle and seek IT Support.";
                    OptionsComboBox.Items.Clear();
                    OptionsComboBox.Items.Add("Browse for Database");
                    OptionsComboBox.Items.Add("Create a new Database");
                    break;
                case OracleMessagesNotificationModes.OracleDatabaseError:
                    MessageTitleTextBlock.Text = "Oracle had a database error";
                    InformationTextBlock.Text = "The database might be busy or the network could have become slow, Oracle will try the last operation again.";
                    OptionsComboBox.Items.Clear();
                    break;
                case OracleMessagesNotificationModes.Custom:
                    if (ocm != null)
                    {
                        MessageTitleTextBlock.Text = ocm.MessageTitleTextBlock;
                        InformationTextBlock.Text = ocm.InformationTextBlock;
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

            int ButtonID = 255;

            bool success = Int32.TryParse(button.Tag.ToString(), out ButtonID);

            if (button != null && success)
            {

                switch (ButtonID)
                {
                    case 0:
                        switch (Mode)
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
                                            MainWindow.mw.Status.Content = "Selected " + openFileDialog.FileName + " file";
                                            String PathString = System.IO.Path.GetDirectoryName(openFileDialog.FileName);

                                            XMLReaderWriter.ReadXMLNodesOracleSettingsXMLFile(PathString);
                                            Close();
                                        }
                                        break;
                                    case 1:
                                        OracleDatabaseCreate.CreateNew_OracleSettingsXML();
                                        MainWindow.mw.Status.Content = "Created and connected to a new Oracle settings file";
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
                                            MainWindow.mw.Status.Content = "Selected " + openFileDialog.FileName + " file";
                                            String PathString = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
                                            XMLReaderWriter.ReadXMLNodesFromOracleDatabaseXMLFile(PathString);
                                            Close();
                                        }
                                        break;
                                    case 1:
                                        OracleDatabaseCreate.CreateNew_OracleDatabaseSettingsXML();
                                        MainWindow.mw.Status.Content = "Created and connected to a new Oracle database settings file";
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
                                            MainWindow.mw.Status.Content = "Selected " + openFileDialog.FileName + " database";
                                            MainWindowTitle.PathString = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
                                            MainWindowTitle.OracleFileName = System.IO.Path.GetFileName(openFileDialog.FileName);
                                            MainWindowTitle.SetMainWindowTitle();
                                            XMLReaderWriter.ReadXMLNodesFromOracleDatabaseXMLFile(MainWindowTitle.PathString);
                                            Close();
                                        }
                                        break;
                                    case 1:
                                        OracleDatabaseCreate.Create_Oracle();
                                        MainWindow.mw.Status.Content = "Created and connected to a new Oracle database";
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
                                            MainWindow.mw.Status.Content = "Selected " + openFileDialog.FileName + " database";
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
                                        MainWindow.mw.Status.Content = "Created and connected to a new Oracle database";
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
