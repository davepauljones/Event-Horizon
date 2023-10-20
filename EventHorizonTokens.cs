using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Event_Horizon
{
    public class EventHorizonTokens
    {
        internal static Dictionary<Int32, DateTime> UsersLastTimeOnlineDictionary = new Dictionary<int, DateTime>();
        internal static Dictionary<Int32, StackPanel> OnlineUsersStackPanelList = new Dictionary<int, StackPanel>();

        public static void LoadSourceTypesIntoWelcome(Grid grid)
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

        public static void LoadEventTypesIntoWelcome(Grid grid)
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

        public static void LoadUsersIntoOnlineUsersStackPanel(StackPanel parentStackPanel)
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
                        StackPanel childStackPanel = EventHorizonTokens.GetUserAsTokenStackPanel(user);

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
                    MainWindow.mw.widgetUsersOnline.NumberOfUsersOnlineLabel.Content = UsersOnlineCount + " of " + (XMLReaderWriter.UsersList.Count - 1);

                    if (UsersWhoJustCameOnlineCount > 0)
                    {
                        MiscFunctions.PlayFile(AppDomain.CurrentDomain.BaseDirectory + "\\Audio\\Notification.mp3");
                    }
                }
            }
        }

        public static void LoadUsersIntoWelcome(Grid grid)
        {
            WrapPanel stackPanel = new WrapPanel { Orientation = Orientation.Horizontal };

            int i = 1;

            foreach (User user in XMLReaderWriter.UsersList)
            {
                if (i > 1)
                {
                    Grid originUserIconEllipseGrid;

                    originUserIconEllipseGrid = new Grid();

                    originUserIconEllipseGrid.Children.Add(EventHorizonTokens.GetUserAsTokenStackPanel(XMLReaderWriter.UsersList[user.ID], true));

                    stackPanel.Children.Add(originUserIconEllipseGrid);
                }

                i++;
            }

            grid.Children.Add(stackPanel);
        }

        public static StackPanel GetUserAsTokenStackPanel(User user, bool JustUsersToken = false)
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
    }
}