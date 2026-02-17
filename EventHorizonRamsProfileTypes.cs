using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using FontAwesome.WPF;

namespace Event_Horizon
{
    public class EventHorizonRamsProfileTypes
    {
        public static StackPanel GetRamsProfileTypeStackPanel(RamsProfileType ramsProfileType, bool overrideEffect = false)
        {
            StackPanel stackPanel = new StackPanel();

            stackPanel.Orientation = Orientation.Horizontal;

            Grid ramsProfileTypeIconEllipseGrid;

            Color iconColor = ramsProfileType.Color;

            FontAwesomeIcon fontAwesomeIcon = ramsProfileType.Icon;

            Border iconBorder = new Border { Width = 28, Height = 28, BorderThickness = new Thickness(0), CornerRadius = new CornerRadius(3), Background = new SolidColorBrush(iconColor), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top, Margin = new Thickness(1,2,0,0), Padding = new Thickness(0) };

            FontAwesome.WPF.FontAwesome fa = new FontAwesome.WPF.FontAwesome { Icon = fontAwesomeIcon, Width = 28, Height = 28, FontSize = 17, Foreground = new SolidColorBrush(Colors.White), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top, Margin = new Thickness(0,5,0,0) };

            iconBorder.Child = fa;

            ramsProfileTypeIconEllipseGrid = new Grid { Margin = new Thickness(0, 1, 3, 3), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };

            ramsProfileTypeIconEllipseGrid.Children.Add(iconBorder);

            ramsProfileTypeIconEllipseGrid.Opacity = 1;

            if (!overrideEffect)
            {
                ramsProfileTypeIconEllipseGrid.Effect = new DropShadowEffect
                {
                    Color = new Color { A = 255, R = 0, G = 0, B = 0 },
                    Direction = 320,
                    ShadowDepth = 1,
                    Opacity = 0.6
                };
            }

            TextBlock eventTypeName;

            if (!overrideEffect)
                eventTypeName = new TextBlock { Text = ramsProfileType.ProfileName, Foreground = Brushes.Black, FontSize = 18, MaxWidth = 470, Margin = new Thickness(4, 5, 0, 0), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top, Padding = new Thickness(0) };
            else
                eventTypeName = new TextBlock { Text = ramsProfileType.ProfileName, Foreground = Brushes.Black, FontSize = 18, MaxWidth = 470, Margin = new Thickness(4, 7, 0, 0), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top, Padding = new Thickness(0) };

            stackPanel.Tag = ramsProfileType.ProfileName;

            //Console.Write("*******TAG = ");
            //Console.WriteLine(stackPanel.Tag);

            stackPanel.Children.Add(ramsProfileTypeIconEllipseGrid);
            stackPanel.Children.Add(eventTypeName);

            return stackPanel;
        }
    }
}