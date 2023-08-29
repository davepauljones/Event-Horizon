using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;
using System.Data;
using System.Data.OleDb;
using System.Xml.Linq;
using System.IO;
using System.Xml.Serialization;

using FontAwesome.WPF;
using System.Windows.Media;
using System.Windows;
using System.Collections;

namespace The_Oracle
{
    public class XMLReaderWriter
    {
        public static Int32 UserID = 0;
        public static String UserNameString = string.Empty;
        public static String DatabaseLocationString = AppDomain.CurrentDomain.BaseDirectory;
        public static List<User> UsersList = new List<User>();
        public static List<EventType> EventTypesList = new List<EventType>();
        public static List<FunctionKeyEventType> FunctionKeyEventTypesList = new List<FunctionKeyEventType>();
        public static List<SourceType> SourceTypesList = new List<SourceType>();
        public static TimeSpan UsersRefreshTimeSpan = TimeSpan.FromMilliseconds(100); 

        public static void ReadXMLNodesOracleSettingsXMLFile(String PathName)
        {
            try
            {
                if (File.Exists(PathName + "\\OracleSettingsXML.xml"))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(PathName + "\\OracleSettingsXML.xml");

                    XmlNodeList nodes = doc.DocumentElement.SelectNodes("/Oracle/Settings");

                    Console.WriteLine("User");
                    foreach (XmlNode node in nodes)
                    {
                        UserID = Convert.ToInt32(node.SelectSingleNode("UserID").InnerText);
                        UserNameString = node.SelectSingleNode("UserName").InnerText;
                        DatabaseLocationString = node.SelectSingleNode("DatabaseLocation").InnerText;

                        UsersRefreshTimeSpan = TimeSpan.FromMilliseconds(UserID * 250);

                        Console.Write("UserID = ");
                        Console.Write(UserID);
                        Console.Write(" UserNameString = ");
                        Console.Write(UserNameString);
                        Console.Write(" DatabaseLocationString = ");
                        Console.Write(DatabaseLocationString);
                    }
                }
                else
                {
                    //MessageBox.Show("Could not connect to Oracle Local Settings File at " + AppDomain.CurrentDomain.BaseDirectory + "\\OracleSettingsXML.xml", "Oracle Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    OracleMessagesNotification msg = new OracleMessagesNotification(MainWindow.mw, OracleMessagesNotificationModes.OracleSettingsXmlMissing);
                    msg.ShowDialog();
                }
            }
            catch (XmlException e)
            {
                Console.WriteLine("----------------------------------------");

                Console.WriteLine("An exception was thrown.");
                Console.WriteLine(e.Message);
                if (e.Data.Count > 0)
                {
                    Console.WriteLine("  Extra details:");
                    foreach (DictionaryEntry de in e.Data)
                        Console.WriteLine("    Key: {0,-20}      Value: {1}", "'" + de.Key.ToString() + "'", de.Value);
                }
            }
        }

        public static void ReadXMLNodesFromOracleDatabaseXMLFile(String PathName)
        {
            try
            {
                if (File.Exists(PathName + "\\OracleDatabaseSettingsXML.xml"))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(PathName + "\\OracleDatabaseSettingsXML.xml");

                    XmlNodeList UsersNodes = doc.DocumentElement.SelectNodes("/OracleDatabase/Users");

                    UsersList.Clear();
                    
                    UsersList.Add(new User { ID = 0, UserName = "None", Color = Colors.White });

                    int id = 1;

                    foreach (XmlNode node in UsersNodes)
                    {
                        UsersList.Add(new User { ID = id, UserName = node["UserName"].InnerText, Color = (Color)ColorConverter.ConvertFromString(node["Color"].InnerText) });
                        id++;
                    }

                    Console.WriteLine("Users");
                    foreach (User u in UsersList)
                    {
                        Console.Write("ID = ");
                        Console.Write(u.ID);
                        Console.Write(" UserName = ");
                        Console.Write(u.UserName);
                        Console.Write(" Color = ");
                        Console.WriteLine(u.Color);
                    }

                    //XmlNodeList FunctionKeyEventTypesNodes = doc.DocumentElement.SelectNodes("/OracleDatabase/FunctionKeys");

                    //FunctionKeyEventTypesList.Clear();

                    //foreach (XmlNode node in FunctionKeyEventTypesNodes)
                    //{
                    //    FunctionKeyEventTypesList.Add(new FunctionKeyEventType { EventTypeID = Convert.ToInt32(node["EventTypeID"].InnerText), ShortName = node["ShortName"].InnerText });
                    //}

                    //Console.WriteLine("FunctionKeyEventTypesList");
                    //id = 0;
                    //foreach (FunctionKeyEventType fket in FunctionKeyEventTypesList)
                    //{
                    //    switch (id)
                    //    {
                    //        case 0:
                    //            MainWindow.mw.F3ButtonLabel.Content = fket.ShortName;
                    //            break;
                    //        case 1:
                    //            MainWindow.mw.F4ButtonLabel.Content = fket.ShortName;
                    //            break;
                    //        case 2:
                    //            MainWindow.mw.F5ButtonLabel.Content = fket.ShortName;
                    //            break;
                    //        case 3:
                    //            MainWindow.mw.F6ButtonLabel.Content = fket.ShortName;
                    //            break;
                    //        case 4:
                    //            MainWindow.mw.F7ButtonLabel.Content = fket.ShortName;
                    //            break;
                    //        case 5:
                    //            MainWindow.mw.F8ButtonLabel.Content = fket.ShortName;
                    //            break;
                    //        case 6:
                    //            MainWindow.mw.F9ButtonLabel.Content = fket.ShortName;
                    //            break;
                    //        case 7:
                    //            MainWindow.mw.F10ButtonLabel.Content = fket.ShortName;
                    //            break;
                    //        case 8:
                    //            MainWindow.mw.F11ButtonLabel.Content = fket.ShortName;
                    //            break;
                    //        case 9:
                    //            MainWindow.mw.F12ButtonLabel.Content = fket.ShortName;
                    //            break;
                    //    }

                    //    Console.Write("EventTypeID = ");
                    //    Console.Write(fket.EventTypeID);
                    //    Console.Write(" ShortName = ");
                    //    Console.WriteLine(fket.ShortName);
                    //    id++;
                    //}

                    XmlNodeList EventTypesNodes = doc.DocumentElement.SelectNodes("/OracleDatabase/EventTypes");

                    EventTypesList.Clear();

                    EventTypesList.Add(new EventType { ID = 0, Name = "All Events", Icon = FontAwesomeIcon.File, Color = (Color)ColorConverter.ConvertFromString("#FFAAAAAA") });

                    id = 1;
                    foreach (XmlNode node in EventTypesNodes)
                    {
                        EventTypesList.Add(new EventType { ID = id, Name = node["Name"].InnerText, Icon = GetUIFontAwesome(node["Icon"].InnerText), Color = (Color)ColorConverter.ConvertFromString(node["Color"].InnerText) });
                        id++;
                    }

                    Console.WriteLine("EventTypes");
                    id = 0;
                    foreach (EventType et in EventTypesList)
                    {
                        Console.WriteLine("FunctionKeyEventTypesList");        
                        switch (id)
                        {
                            case 1:
                                MainWindow.mw.F3ButtonLabel.Content = et.Name;
                                MainWindow.mw.F3FontAwesomeIcon.Icon = et.Icon;
                                MainWindow.mw.F3FontAwesomeIconBorder.Background = new SolidColorBrush(et.Color);
                                break;
                            case 2:
                                MainWindow.mw.F4ButtonLabel.Content = et.Name;
                                MainWindow.mw.F4FontAwesomeIcon.Icon = et.Icon;
                                MainWindow.mw.F4FontAwesomeIconBorder.Background = new SolidColorBrush(et.Color);
                                break;
                            case 3:
                                MainWindow.mw.F5ButtonLabel.Content = et.Name;
                                MainWindow.mw.F5FontAwesomeIcon.Icon = et.Icon;
                                MainWindow.mw.F5FontAwesomeIconBorder.Background = new SolidColorBrush(et.Color);
                                break;
                            case 4:
                                MainWindow.mw.F6ButtonLabel.Content = et.Name;
                                MainWindow.mw.F6FontAwesomeIcon.Icon = et.Icon;
                                MainWindow.mw.F6FontAwesomeIconBorder.Background = new SolidColorBrush(et.Color);
                                break;
                            case 5:
                                MainWindow.mw.F7ButtonLabel.Content = et.Name;
                                MainWindow.mw.F7FontAwesomeIcon.Icon = et.Icon;
                                MainWindow.mw.F7FontAwesomeIconBorder.Background = new SolidColorBrush(et.Color);
                                break;
                            case 6:
                                MainWindow.mw.F8ButtonLabel.Content = et.Name;
                                MainWindow.mw.F8FontAwesomeIcon.Icon = et.Icon;
                                MainWindow.mw.F8FontAwesomeIconBorder.Background = new SolidColorBrush(et.Color);
                                break;
                            case 7:
                                MainWindow.mw.F9ButtonLabel.Content = et.Name;
                                MainWindow.mw.F9FontAwesomeIcon.Icon = et.Icon;
                                MainWindow.mw.F9FontAwesomeIconBorder.Background = new SolidColorBrush(et.Color);
                                break;
                            case 8:
                                MainWindow.mw.F10ButtonLabel.Content = et.Name;
                                MainWindow.mw.F10FontAwesomeIcon.Icon = et.Icon;
                                MainWindow.mw.F10FontAwesomeIconBorder.Background = new SolidColorBrush(et.Color);
                                break;
                            case 9:
                                MainWindow.mw.F11ButtonLabel.Content = et.Name;
                                MainWindow.mw.F11FontAwesomeIcon.Icon = et.Icon;
                                MainWindow.mw.F11FontAwesomeIconBorder.Background = new SolidColorBrush(et.Color);
                                break;
                            case 10:
                                MainWindow.mw.F12ButtonLabel.Content = et.Name;
                                MainWindow.mw.F12FontAwesomeIcon.Icon = et.Icon;
                                MainWindow.mw.F12FontAwesomeIconBorder.Background = new SolidColorBrush(et.Color);
                                break;
                        }

                        id++;

                        Console.Write("ID = ");
                        Console.Write(et.ID);
                        Console.Write(" Name = ");
                        Console.Write(et.Name);
                        Console.Write(" Icon = ");
                        Console.Write(et.Icon);
                        Console.Write(" Color = ");
                        Console.WriteLine(et.Color);
                    }

                    XmlNodeList SourceTypesNodes = doc.DocumentElement.SelectNodes("/OracleDatabase/SourceTypes");
                    
                    SourceTypesList.Clear();

                    id = 1;
                    foreach (XmlNode node in SourceTypesNodes)
                    {
                        SourceTypesList.Add(new SourceType { ID = id, Name = node["Name"].InnerText, Icon = GetUIFontAwesome(node["Icon"].InnerText), Color = (Color)ColorConverter.ConvertFromString(node["Color"].InnerText) });
                        id++;
                    }

                    Console.WriteLine("SourceTypes");
                    foreach (SourceType et in SourceTypesList)
                    {
                        Console.Write("ID = ");
                        Console.Write(et.ID);
                        Console.Write(" Name = ");
                        Console.Write(et.Name);
                        Console.Write(" Icon = ");
                        Console.Write(et.Icon);
                        Console.Write(" Color = ");
                        Console.WriteLine(et.Color);
                    }
                }
                else
                {
                    //MessageBox.Show("Could not connect to Oracle Global Settings File at " + XMLReaderWriter.DatabaseLocationString + "\\OracleDatabaseSettingsXML.xml", "Oracle Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    OracleMessagesNotification msg = new OracleMessagesNotification(MainWindow.mw, OracleMessagesNotificationModes.OracleDatabaseSettingsXmlMissing);
                    msg.ShowDialog();
                }
            }
            catch (XmlException e)
            {
                Console.WriteLine("----------------------------------------");

                Console.WriteLine("An exception was thrown.");
                Console.WriteLine(e.Message);
                if (e.Data.Count > 0)
                {
                    Console.WriteLine("  Extra details:");
                    foreach (DictionaryEntry de in e.Data)
                        Console.WriteLine("    Key: {0,-20}      Value: {1}", "'" + de.Key.ToString() + "'", de.Value);
                }
            }
        }

        private static FontAwesomeIcon GetUIFontAwesome(string strIcon)
        {
            FontAwesome.WPF.FontAwesomeIcon item;
            if (Enum.TryParse(strIcon, out item))
                return item;
            else
                return FontAwesome.WPF.FontAwesomeIcon.None;
        }

        public static void WriteSettingsXmlFile(OracleSettings os)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            XmlWriter writer = XmlWriter.Create(os.DatabaseLocation + @"\" + @"OracleSettingsXML.xml", settings);

            Console.Write("os.DatabaseLocation = ");
            Console.WriteLine(os.DatabaseLocation + @"\OracleSettingsXML.xml");

            writer.WriteStartDocument();

            writer.WriteComment("Oracle solution settings file generated by the program.");

            writer.WriteStartElement("Oracle");
            writer.WriteStartElement("Settings");
            writer.WriteElementString("UserID", os.UserID.ToString());
            writer.WriteElementString("UserName", os.UserName);
            writer.WriteElementString("DatabaseLocation", os.DatabaseLocation);
            writer.WriteElementString("HoverDatabaseLocation", "N:\\HoverData");

            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
        }

        public static bool CheckIf_DatabaseSettingsXML_FileExists()
        {
            bool Result = false;

            if (File.Exists(XMLReaderWriter.DatabaseLocationString + "\\OracleDatabaseSettingsXML.xml"))
            {
                XMLReaderWriter.ReadXMLNodesFromOracleDatabaseXMLFile(XMLReaderWriter.DatabaseLocationString);
                Result = true;
            }
            else if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\OracleDatabaseSettingsXML.xml"))
            {
                XMLReaderWriter.ReadXMLNodesFromOracleDatabaseXMLFile(AppDomain.CurrentDomain.BaseDirectory);
                Result = true;
            }
            else
            {
                OracleMessagesNotification msg = new OracleMessagesNotification(MainWindow.mw, OracleMessagesNotificationModes.OracleDatabaseSettingsXmlMissing);
                msg.ShowDialog();
                Result = false;
            }
            
            return Result;
        }

        public static bool CheckIf_SettingsXML_FileExists()
        {
            bool result = false;

            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\OracleSettingsXML.xml"))
            {
                ReadXMLNodesOracleSettingsXMLFile(AppDomain.CurrentDomain.BaseDirectory);
                result = true;
            }
            else
            {
                OracleMessagesNotification msg = new OracleMessagesNotification(MainWindow.mw, OracleMessagesNotificationModes.OracleSettingsXmlMissing);
                msg.ShowDialog();
                result = false;
            }

            return result;
        }
    }
}