using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using FontAwesome.WPF;

namespace Event_Horizon
{
    public class EventHorizonEventTypes
    {
        public static StackPanel GetEventTypeStackPanel(EventType eventType, bool overrideEffect = false)
        {
            StackPanel stackPanel = new StackPanel();

            stackPanel.Orientation = Orientation.Horizontal;

            Grid eventTypeIconEllipseGrid;

            Color iconColor = eventType.Color;

            FontAwesomeIcon fontAwesomeIcon = eventType.Icon;

            Border iconBorder = new Border { Width = 28, Height = 28, BorderThickness = new Thickness(0), CornerRadius = new CornerRadius(3), Background = new SolidColorBrush(iconColor), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top, Margin = new Thickness(1,2,0,0), Padding = new Thickness(0) };

            FontAwesome.WPF.FontAwesome fa = new FontAwesome.WPF.FontAwesome { Icon = fontAwesomeIcon, Width = 28, Height = 28, FontSize = 17, Foreground = new SolidColorBrush(Colors.White), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top, Margin = new Thickness(0,5,0,0) };

            iconBorder.Child = fa;

            eventTypeIconEllipseGrid = new Grid { Margin = new Thickness(0, 1, 3, 3), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };

            eventTypeIconEllipseGrid.Children.Add(iconBorder);

            eventTypeIconEllipseGrid.Opacity = 1;

            if (!overrideEffect)
            {
                eventTypeIconEllipseGrid.Effect = new DropShadowEffect
                {
                    Color = new Color { A = 255, R = 0, G = 0, B = 0 },
                    Direction = 320,
                    ShadowDepth = 1,
                    Opacity = 0.6
                };
            }

            TextBlock eventTypeName;

            if (!overrideEffect)
                eventTypeName = new TextBlock { Text = eventType.Name, Foreground = Brushes.Black, FontSize = 18, MaxWidth = 170, Margin = new Thickness(4, 5, 0, 0), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top, Padding = new Thickness(0) };
            else
                eventTypeName = new TextBlock { Text = eventType.Name, Foreground = Brushes.Black, FontSize = 18, MaxWidth = 170, Margin = new Thickness(4, 7, 0, 0), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top, Padding = new Thickness(0) };

            stackPanel.Tag = eventType.Name;

            //Console.Write("*******TAG = ");
            //Console.WriteLine(stackPanel.Tag);

            stackPanel.Children.Add(eventTypeIconEllipseGrid);
            stackPanel.Children.Add(eventTypeName);

            return stackPanel;
        }
    }
}