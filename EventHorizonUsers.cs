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
    public class EventHorizonUsers
    {
        public static StackPanel GetUserStackPanel(User u)
        {
            StackPanel sp = new StackPanel();

            sp.Orientation = Orientation.Horizontal;

            Grid OriginUserIconEllipseGrid;
            Ellipse OriginUserIconEllipse;

            Color IconEllipseColor = Colors.White;

            IconEllipseColor = XMLReaderWriter.UsersList[u.ID].Color;

            if (u.ID > 0)
                OriginUserIconEllipse = new Ellipse { Width = 24, Height = 24, Fill = new SolidColorBrush(IconEllipseColor), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };
            else
                OriginUserIconEllipse = new Ellipse { Width = 24, Height = 24, Fill = new SolidColorBrush(IconEllipseColor), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };

            OriginUserIconEllipseGrid = new Grid { Margin = new Thickness(3, 1, 3, 3), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };

            OriginUserIconEllipseGrid.Children.Add(OriginUserIconEllipse);

            Label OriginUserIconEllipseLabel;

            if (u.ID > 0)
                OriginUserIconEllipseLabel = new Label { Content = MiscFunctions.GetFirstCharsOfString(u.UserName), Foreground = Brushes.Black, FontSize = 10, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };
            else
                OriginUserIconEllipseLabel = new Label { Content = "★", Foreground = Brushes.Black, FontSize = 14, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, -3, 0, 0), MaxHeight = 24, Padding = new Thickness(0) };

            OriginUserIconEllipseGrid.Children.Add(OriginUserIconEllipseLabel);

            OriginUserIconEllipseGrid.Opacity = 1;

            OriginUserIconEllipseGrid.Effect = new DropShadowEffect
            {
                Color = new Color { A = 255, R = 0, G = 0, B = 0 },
                Direction = 320,
                ShadowDepth = 1,
                Opacity = 0.6
            };

            TextBlock UsersName = new TextBlock { Text = u.UserName, Foreground = Brushes.Black, FontSize = 18, MaxWidth = 170, Margin = new Thickness(4, 1, 0, 0), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top, Padding = new Thickness(0) };

            sp.Children.Add(OriginUserIconEllipseGrid);
            sp.Children.Add(UsersName);

            return sp;
        }

    }
}