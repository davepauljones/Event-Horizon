using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;

namespace Event_Horizon
{
    public class EventHorizonUsers
    {
        public static StackPanel GetUserStackPanel(User user)
        {
            StackPanel stackPanel = new StackPanel();

            stackPanel.Orientation = Orientation.Horizontal;

            Grid originUserIconEllipseGrid;
            Ellipse originUserIconEllipse;

            Color iconEllipseColor = Colors.White;

            iconEllipseColor = XMLReaderWriter.UsersList[user.ID].Color;

            if (user.ID > 0)
                originUserIconEllipse = new Ellipse { Width = 24, Height = 24, Fill = new SolidColorBrush(iconEllipseColor), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };
            else
                originUserIconEllipse = new Ellipse { Width = 24, Height = 24, Fill = new SolidColorBrush(iconEllipseColor), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };

            originUserIconEllipseGrid = new Grid { Margin = new Thickness(3, 1, 3, 3), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };

            originUserIconEllipseGrid.Children.Add(originUserIconEllipse);

            Label originUserIconEllipseLabel;

            if (user.ID > 0)
                originUserIconEllipseLabel = new Label { Content = MiscFunctions.GetFirstCharsOfString(user.UserName), Foreground = Brushes.Black, FontSize = 10, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };
            else
                originUserIconEllipseLabel = new Label { Content = "★", Foreground = Brushes.Black, FontSize = 14, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(1, -2, 0, 0), MaxHeight = 24, Padding = new Thickness(0) };

            originUserIconEllipseGrid.Children.Add(originUserIconEllipseLabel);

            originUserIconEllipseGrid.Opacity = 1;

            originUserIconEllipseGrid.Effect = new DropShadowEffect
            {
                Color = new Color { A = 255, R = 0, G = 0, B = 0 },
                Direction = 320,
                ShadowDepth = 1,
                Opacity = 0.6
            };

            TextBlock usersName = new TextBlock { Text = user.UserName, Foreground = Brushes.Black, FontSize = 18, MaxWidth = 170, Margin = new Thickness(4, 1, 0, 0), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top, Padding = new Thickness(0) };

            stackPanel.Children.Add(originUserIconEllipseGrid);
            stackPanel.Children.Add(usersName);

            return stackPanel;
        }
    }
}