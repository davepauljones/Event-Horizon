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

namespace The_Oracle
{
    public class EventHorizonSources
    {
        public static StackPanel GetSourceStackPanel(SourceType s)
        {
            StackPanel sp = new StackPanel();

            sp.Orientation = Orientation.Horizontal;

            Grid SourceIconEllipseGrid;
            Ellipse SourceIconEllipse;

            Color IconEllipseColor = Colors.White;

            IconEllipseColor = XMLReaderWriter.SourceTypesList[s.ID].Color;

            if (s.ID > 0)
                SourceIconEllipse = new Ellipse { Width = 24, Height = 24, Fill = new SolidColorBrush(IconEllipseColor), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };
            else
                SourceIconEllipse = new Ellipse { Width = 24, Height = 24, Fill = new SolidColorBrush(IconEllipseColor), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };

            SourceIconEllipseGrid = new Grid { Margin = new Thickness(3, 1, 3, 3), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };

            SourceIconEllipseGrid.Children.Add(SourceIconEllipse);

            Label SourceIconEllipseLabel;

            if (s.ID > 0)
                SourceIconEllipseLabel = new Label { Content = MiscFunctions.GetFirstCharsOfString(s.Name), Foreground = Brushes.Black, FontSize = 10, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };
            else
                SourceIconEllipseLabel = new Label { Content = "★", Foreground = Brushes.Black, FontSize = 14, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, -3, 0, 0), MaxHeight = 24, Padding = new Thickness(0) };

            SourceIconEllipseGrid.Children.Add(SourceIconEllipseLabel);

            SourceIconEllipseGrid.Opacity = 1;

            SourceIconEllipseGrid.Effect = new DropShadowEffect
            {
                Color = new Color { A = 255, R = 0, G = 0, B = 0 },
                Direction = 320,
                ShadowDepth = 1,
                Opacity = 0.6
            };

            TextBlock SourceName = new TextBlock { Text = s.Name, Foreground = Brushes.Black, FontSize = 18, MaxWidth = 170, Margin = new Thickness(4, 1, 0, 0), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top, Padding = new Thickness(0) };

            sp.Tag = s.Name;

            sp.Children.Add(SourceIconEllipseGrid);
            sp.Children.Add(SourceName);

            return sp;
        }

    }
}