using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;

namespace The_Oracle
{
    public class EventHorizonSources
    {
        public static StackPanel GetSourceStackPanel(SourceType sourceType)
        {
            StackPanel stackPanel = new StackPanel();

            stackPanel.Orientation = Orientation.Horizontal;

            Grid sourceIconEllipseGrid;
            Ellipse sourceIconEllipse;

            Color iconEllipseColor = XMLReaderWriter.SourceTypesList[sourceType.ID].Color;

            if (sourceType.ID > 0)
                sourceIconEllipse = new Ellipse { Width = 24, Height = 24, Fill = new SolidColorBrush(iconEllipseColor), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };
            else
                sourceIconEllipse = new Ellipse { Width = 24, Height = 24, Fill = new SolidColorBrush(iconEllipseColor), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };

            sourceIconEllipseGrid = new Grid { Margin = new Thickness(3, 1, 3, 3), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };

            sourceIconEllipseGrid.Children.Add(sourceIconEllipse);

            Label sourceIconEllipseLabel;

            if (sourceType.ID > 0)
                sourceIconEllipseLabel = new Label { Content = MiscFunctions.GetFirstCharsOfString(sourceType.Name), Foreground = Brushes.Black, FontSize = 10, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };
            else
                sourceIconEllipseLabel = new Label { Content = "★", Foreground = Brushes.Black, FontSize = 14, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(1, -2, 0, 0), MaxHeight = 24, Padding = new Thickness(0) };

            sourceIconEllipseGrid.Children.Add(sourceIconEllipseLabel);

            sourceIconEllipseGrid.Opacity = 1;

            sourceIconEllipseGrid.Effect = new DropShadowEffect
            {
                Color = new Color { A = 255, R = 0, G = 0, B = 0 },
                Direction = 320,
                ShadowDepth = 1,
                Opacity = 0.6
            };

            TextBlock sourceName = new TextBlock { Text = sourceType.Name, Foreground = Brushes.Black, FontSize = 18, MaxWidth = 170, Margin = new Thickness(4, 1, 0, 0), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top, Padding = new Thickness(0) };

            stackPanel.Tag = sourceType.Name;

            stackPanel.Children.Add(sourceIconEllipseGrid);
            stackPanel.Children.Add(sourceName);

            return stackPanel;
        }
    }
}