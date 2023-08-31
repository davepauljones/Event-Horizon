using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using FontAwesome.WPF;

namespace The_Oracle
{
    public class EventHorizonEventTypes
    {
        public static StackPanel GetEventTypeStackPanel(EventType e)
        {
            StackPanel sp = new StackPanel();

            sp.Orientation = Orientation.Horizontal;

            Grid EventTypeIconEllipseGrid;

            Color IconColor = XMLReaderWriter.EventTypesList[e.ID].Color;
            FontAwesomeIcon fai = XMLReaderWriter.EventTypesList[e.ID].Icon;

            Border IconBorder = new Border { Width = 28, Height = 28, BorderThickness = new Thickness(0), CornerRadius = new CornerRadius(3), Background = new SolidColorBrush(IconColor), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top, Margin = new Thickness(0,2,0,0), Padding = new Thickness(0) };

            FontAwesome.WPF.FontAwesome fa = new FontAwesome.WPF.FontAwesome { Icon = fai, Width = 28, Height = 28, FontSize = 17, Foreground = new SolidColorBrush(Colors.White), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top, Margin = new Thickness(0,5,0,0) };

            IconBorder.Child = fa;
           
            EventTypeIconEllipseGrid = new Grid { Margin = new Thickness(0, 1, 3, 3), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };

            EventTypeIconEllipseGrid.Children.Add(IconBorder);

            EventTypeIconEllipseGrid.Opacity = 1;

            EventTypeIconEllipseGrid.Effect = new DropShadowEffect
            {
                Color = new Color { A = 255, R = 0, G = 0, B = 0 },
                Direction = 320,
                ShadowDepth = 1,
                Opacity = 0.6
            };

            TextBlock EventTypeName = new TextBlock { Text = e.Name, Foreground = Brushes.Black, FontSize = 18, MaxWidth = 170, Margin = new Thickness(4, 5, 0, 0), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top, Padding = new Thickness(0) };

            sp.Tag = e.Name;

            Console.Write("*******TAG = ");
            Console.WriteLine(sp.Tag);

            sp.Children.Add(EventTypeIconEllipseGrid);
            sp.Children.Add(EventTypeName);

            return sp;
        }

    }
}