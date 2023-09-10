using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using FontAwesome.WPF;
using System.Windows.Media;
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

        public static bool TryReadNodesFrom_EventHorizonSettingsXMLFile(String PathFileName)
        {
            bool result = false;
            try
            {
                if (File.Exists(PathFileName))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(PathFileName);

                    XmlNodeList nodes = doc.DocumentElement.SelectNodes("/EventHorizon/Settings");

                    Console.WriteLine("User");
                    foreach (XmlNode node in nodes)
                    {
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

                    result = true;
                }
                else
                {
                    OracleRequesterNotification msg = new OracleRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "EventHorizonLocalSettings.xml file is missing", InformationTextBlock = "Event Horizon could not find a required xml file, located in the Event Horizon install folder!" }, RequesterTypes.OK);
                    msg.ShowDialog();
                    result = false;
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
                result = false;
            }

            return result;
        }

        public static void ReadXMLNodesOracleSettingsXMLFile(String PathName)
        {
            try
            {
                if (File.Exists(PathName + "\\EventHorizonLocalSettings.xml"))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(PathName + "\\EventHorizonLocalSettings.xml");

                    XmlNodeList nodes = doc.DocumentElement.SelectNodes("/EventHorizon/Settings");

                    Console.WriteLine("User");
                    foreach (XmlNode node in nodes)
                    {
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
                    OracleRequesterNotification msg = new OracleRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "EventHorizonLocalSettings.xml file is missing", InformationTextBlock = "Event Horizon could not find a required xml file, located in the Event Horizon install folder!" }, RequesterTypes.OK);
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

        public static bool TryReadNodesFrom_EventHorizonRemoteSettings_Users(String PathFileName)
        {
            bool result = false;
            try
            {
                if (File.Exists(PathFileName))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(PathFileName);

                    XmlNodeList UsersNodes = doc.DocumentElement.SelectNodes("/EventHorizonDatabase/Users");

                    UsersList.Clear();

                    UsersList.Add(new User { ID = 0, UserName = "None", Color = (Color)ColorConverter.ConvertFromString("#FFAAAAAA") });

                    int id = 1;

                    foreach (XmlNode node in UsersNodes)
                    {
                        if (UserNameString == node["UserName"].InnerText) UserID = id;

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

                    result = true;
                }
                else
                {
                    OracleRequesterNotification msg = new OracleRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "TryReadNodes - ", InformationTextBlock = "Missing " + XMLReaderWriter.DatabaseLocationString + "\\EventHorizonRemoteSettings.xml file." }, RequesterTypes.OK);
                    msg.ShowDialog();
                    result = false;
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
                result = false;
            }
            return result;
        }

        public static bool TryReadNodesFrom_EventHorizonRemoteSettings_EventTypes(String PathFileName)
        {
            bool result = false;
            try
            {
                if (File.Exists(PathFileName))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(PathFileName);

                    XmlNodeList EventTypesNodes = doc.DocumentElement.SelectNodes("/EventHorizonDatabase/EventTypes");

                    EventTypesList.Clear();

                    EventTypesList.Add(new EventType { ID = 0, Name = "All Events", Icon = FontAwesomeIcon.Star, Color = (Color)ColorConverter.ConvertFromString("#FFAAAAAA") });

                    int id = 1;
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

                    result = true;
                }
                else
                {
                    OracleRequesterNotification msg = new OracleRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "TryReadNodes - ", InformationTextBlock = "Missing " + XMLReaderWriter.DatabaseLocationString + "\\EventHorizonRemoteSettings.xml file." }, RequesterTypes.OK);
                    msg.ShowDialog();
                    result = false;
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
                result = false;
            }
            return result;
        }

        public static bool TryReadNodesFrom_EventHorizonRemoteSettings_SourceTypes(String PathFileName)
        {
            bool result = false;
            try
            {
                if (File.Exists(PathFileName))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(PathFileName);

                    XmlNodeList SourceTypesNodes = doc.DocumentElement.SelectNodes("/EventHorizonDatabase/SourceTypes");

                    SourceTypesList.Clear();

                    SourceTypesList.Add(new SourceType { ID = 0, Name = "None", Icon = FontAwesomeIcon.Star, Color = (Color)ColorConverter.ConvertFromString("#FFAAAAAA") });

                    int id = 1;
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
                    result = true;
                }
                else
                {
                    OracleRequesterNotification msg = new OracleRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "TryReadNodes - ", InformationTextBlock = "Missing " + XMLReaderWriter.DatabaseLocationString + "\\EventHorizonRemoteSettings.xml file." }, RequesterTypes.OK);
                    msg.ShowDialog();
                    result = false;
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
                result = false;
            }
            return result;
        }

        //public static void ReadXMLNodesFromOracleDatabaseXMLFile(String PathName)
        //{
        //    try
        //    {
        //        if (File.Exists(PathName + "\\EventHorizonRemoteSettings.xml"))
        //        {
        //            XmlDocument doc = new XmlDocument();
        //            doc.Load(PathName + "\\EventHorizonRemoteSettings.xml");

        //            XmlNodeList UsersNodes = doc.DocumentElement.SelectNodes("/EventHorizonDatabase/Users");

        //            UsersList.Clear();
                    
        //            UsersList.Add(new User { ID = 0, UserName = "None", Color = (Color)ColorConverter.ConvertFromString("#FFAAAAAA") });

        //            int id = 1;

        //            foreach (XmlNode node in UsersNodes)
        //            {
        //                if (UserNameString == node["UserName"].InnerText) UserID = id;

        //                UsersList.Add(new User { ID = id, UserName = node["UserName"].InnerText, Color = (Color)ColorConverter.ConvertFromString(node["Color"].InnerText) });
        //                id++;
        //            }

        //            Console.WriteLine("Users");
        //            foreach (User u in UsersList)
        //            {
        //                Console.Write("ID = ");
        //                Console.Write(u.ID);
        //                Console.Write(" UserName = ");
        //                Console.Write(u.UserName);
        //                Console.Write(" Color = ");
        //                Console.WriteLine(u.Color);
        //            }

        //            XmlNodeList EventTypesNodes = doc.DocumentElement.SelectNodes("/EventHorizonDatabase/EventTypes");

        //            EventTypesList.Clear();

        //            EventTypesList.Add(new EventType { ID = 0, Name = "All Events", Icon = FontAwesomeIcon.Star, Color = (Color)ColorConverter.ConvertFromString("#FFAAAAAA") });

        //            id = 1;
        //            foreach (XmlNode node in EventTypesNodes)
        //            {
        //                EventTypesList.Add(new EventType { ID = id, Name = node["Name"].InnerText, Icon = GetUIFontAwesome(node["Icon"].InnerText), Color = (Color)ColorConverter.ConvertFromString(node["Color"].InnerText) });
        //                id++;
        //            }

        //            Console.WriteLine("EventTypes");
        //            id = 0;
        //            foreach (EventType et in EventTypesList)
        //            {
        //                Console.WriteLine("FunctionKeyEventTypesList");        
        //                switch (id)
        //                {
        //                    case 1:
        //                        MainWindow.mw.F3ButtonLabel.Content = et.Name;
        //                        MainWindow.mw.F3FontAwesomeIcon.Icon = et.Icon;
        //                        MainWindow.mw.F3FontAwesomeIconBorder.Background = new SolidColorBrush(et.Color);
        //                        break;
        //                    case 2:
        //                        MainWindow.mw.F4ButtonLabel.Content = et.Name;
        //                        MainWindow.mw.F4FontAwesomeIcon.Icon = et.Icon;
        //                        MainWindow.mw.F4FontAwesomeIconBorder.Background = new SolidColorBrush(et.Color);
        //                        break;
        //                    case 3:
        //                        MainWindow.mw.F5ButtonLabel.Content = et.Name;
        //                        MainWindow.mw.F5FontAwesomeIcon.Icon = et.Icon;
        //                        MainWindow.mw.F5FontAwesomeIconBorder.Background = new SolidColorBrush(et.Color);
        //                        break;
        //                    case 4:
        //                        MainWindow.mw.F6ButtonLabel.Content = et.Name;
        //                        MainWindow.mw.F6FontAwesomeIcon.Icon = et.Icon;
        //                        MainWindow.mw.F6FontAwesomeIconBorder.Background = new SolidColorBrush(et.Color);
        //                        break;
        //                    case 5:
        //                        MainWindow.mw.F7ButtonLabel.Content = et.Name;
        //                        MainWindow.mw.F7FontAwesomeIcon.Icon = et.Icon;
        //                        MainWindow.mw.F7FontAwesomeIconBorder.Background = new SolidColorBrush(et.Color);
        //                        break;
        //                    case 6:
        //                        MainWindow.mw.F8ButtonLabel.Content = et.Name;
        //                        MainWindow.mw.F8FontAwesomeIcon.Icon = et.Icon;
        //                        MainWindow.mw.F8FontAwesomeIconBorder.Background = new SolidColorBrush(et.Color);
        //                        break;
        //                    case 7:
        //                        MainWindow.mw.F9ButtonLabel.Content = et.Name;
        //                        MainWindow.mw.F9FontAwesomeIcon.Icon = et.Icon;
        //                        MainWindow.mw.F9FontAwesomeIconBorder.Background = new SolidColorBrush(et.Color);
        //                        break;
        //                    case 8:
        //                        MainWindow.mw.F10ButtonLabel.Content = et.Name;
        //                        MainWindow.mw.F10FontAwesomeIcon.Icon = et.Icon;
        //                        MainWindow.mw.F10FontAwesomeIconBorder.Background = new SolidColorBrush(et.Color);
        //                        break;
        //                    case 9:
        //                        MainWindow.mw.F11ButtonLabel.Content = et.Name;
        //                        MainWindow.mw.F11FontAwesomeIcon.Icon = et.Icon;
        //                        MainWindow.mw.F11FontAwesomeIconBorder.Background = new SolidColorBrush(et.Color);
        //                        break;
        //                    case 10:
        //                        MainWindow.mw.F12ButtonLabel.Content = et.Name;
        //                        MainWindow.mw.F12FontAwesomeIcon.Icon = et.Icon;
        //                        MainWindow.mw.F12FontAwesomeIconBorder.Background = new SolidColorBrush(et.Color);
        //                        break;
        //                }

        //                id++;

        //                Console.Write("ID = ");
        //                Console.Write(et.ID);
        //                Console.Write(" Name = ");
        //                Console.Write(et.Name);
        //                Console.Write(" Icon = ");
        //                Console.Write(et.Icon);
        //                Console.Write(" Color = ");
        //                Console.WriteLine(et.Color);
        //            }

        //            XmlNodeList SourceTypesNodes = doc.DocumentElement.SelectNodes("/EventHorizonDatabase/SourceTypes");
                    
        //            SourceTypesList.Clear();

        //            SourceTypesList.Add(new SourceType { ID = 0, Name = "None", Icon = FontAwesomeIcon.Star, Color = (Color)ColorConverter.ConvertFromString("#FFAAAAAA") });

        //            id = 1;
        //            foreach (XmlNode node in SourceTypesNodes)
        //            {
        //                SourceTypesList.Add(new SourceType { ID = id, Name = node["Name"].InnerText, Icon = GetUIFontAwesome(node["Icon"].InnerText), Color = (Color)ColorConverter.ConvertFromString(node["Color"].InnerText) });
        //                id++;
        //            }

        //            Console.WriteLine("SourceTypes");
        //            foreach (SourceType et in SourceTypesList)
        //            {
        //                Console.Write("ID = ");
        //                Console.Write(et.ID);
        //                Console.Write(" Name = ");
        //                Console.Write(et.Name);
        //                Console.Write(" Icon = ");
        //                Console.Write(et.Icon);
        //                Console.Write(" Color = ");
        //                Console.WriteLine(et.Color);
        //            }
        //        }
        //        else
        //        {
        //            OracleMessagesNotification msg = new OracleMessagesNotification(MainWindow.mw, OracleMessagesNotificationModes.OracleDatabaseSettingsXmlMissing);
        //            msg.ShowDialog();
        //        }
        //    }
        //        catch (XmlException e)
        //        {
        //            Console.WriteLine("----------------------------------------");

        //            Console.WriteLine("An exception was thrown.");
        //            Console.WriteLine(e.Message);
        //            if (e.Data.Count > 0)
        //            {
        //                Console.WriteLine("  Extra details:");
        //                foreach (DictionaryEntry de in e.Data)
        //                    Console.WriteLine("    Key: {0,-20}      Value: {1}", "'" + de.Key.ToString() + "'", de.Value);
        //            }
        //    }
        //}

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
            XmlWriter writer = XmlWriter.Create(os.DatabaseLocation + @"\" + @"EventHorizonLocalSettings.xml", settings);

            Console.Write("os.DatabaseLocation = ");
            Console.WriteLine(os.DatabaseLocation + @"\EventHorizonLocalSettings.xml");

            writer.WriteStartDocument();

            writer.WriteComment("Event Horizon local settings file generated by the program.");

            writer.WriteStartElement("EventHorizon");
            writer.WriteStartElement("Settings");
            writer.WriteElementString("UserName", os.UserName);
            writer.WriteElementString("DatabaseLocation", os.DatabaseLocation);

            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
        }

        //public static bool CheckIf_DatabaseSettingsXML_FileExists()
        //{
        //    bool result = false;

        //    if (File.Exists(XMLReaderWriter.DatabaseLocationString + "\\EventHorizonRemoteSettings.xml"))
        //    {
        //        XMLReaderWriter.ReadXMLNodesFromOracleDatabaseXMLFile(XMLReaderWriter.DatabaseLocationString);
        //        result = true;
        //    }
        //    else if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\EventHorizonRemoteSettings.xml"))
        //    {
        //        XMLReaderWriter.ReadXMLNodesFromOracleDatabaseXMLFile(AppDomain.CurrentDomain.BaseDirectory);
        //        result = true;
        //    }
        //    else
        //    {
        //        OracleMessagesNotification msg = new OracleMessagesNotification(MainWindow.mw, OracleMessagesNotificationModes.OracleDatabaseSettingsXmlMissing);
        //        msg.ShowDialog();
        //        result = false;
        //    }
            
        //    return result;
        //}

        //public static bool CheckIf_SettingsXML_FileExists()
        //{
        //    bool result = false;

        //    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\EventHorizonLocalSettings.xml"))
        //    {
        //        ReadXMLNodesOracleSettingsXMLFile(AppDomain.CurrentDomain.BaseDirectory);
        //        result = true;
        //    }
        //    else
        //    {
        //        OracleMessagesNotification msg = new OracleMessagesNotification(MainWindow.mw, OracleMessagesNotificationModes.OracleSettingsXmlMissing);
        //        msg.ShowDialog();
        //        result = false;
        //    }

        //    return result;
        //}
    }
}