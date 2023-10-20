using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using FontAwesome.WPF;

namespace Event_Horizon
{
    public class EventHorizonSources
    {
        public static StackPanel GetSourceStackPanel(SourceType sourceType)
        {
            StackPanel stackPanel = new StackPanel();

            stackPanel.Orientation = Orientation.Horizontal;

            Grid sourceTypeIconEllipseGrid;

            Color iconColor = XMLReaderWriter.SourceTypesList[sourceType.ID].Color;
            FontAwesomeIcon fontAwesomeIcon = XMLReaderWriter.SourceTypesList[sourceType.ID].Icon;

            Border iconBorder = new Border { Width = 28, Height = 28, BorderThickness = new Thickness(0), CornerRadius = new CornerRadius(3), Background = new SolidColorBrush(iconColor), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top, Margin = new Thickness(1, 2, 0, 0), Padding = new Thickness(0) };

            FontAwesome.WPF.FontAwesome fa = new FontAwesome.WPF.FontAwesome { Icon = fontAwesomeIcon, Width = 28, Height = 28, FontSize = 17, Foreground = new SolidColorBrush(Colors.White), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top, Margin = new Thickness(0, 5, 0, 0) };

            iconBorder.Child = fa;

            sourceTypeIconEllipseGrid = new Grid { Margin = new Thickness(0, 1, 3, 3), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };

            sourceTypeIconEllipseGrid.Children.Add(iconBorder);

            sourceTypeIconEllipseGrid.Opacity = 1;

            sourceTypeIconEllipseGrid.Effect = new DropShadowEffect
            {
                Color = new Color { A = 255, R = 0, G = 0, B = 0 },
                Direction = 320,
                ShadowDepth = 1,
                Opacity = 0.6
            };

            TextBlock eventTypeName = new TextBlock { Text = sourceType.Name, Foreground = Brushes.Black, FontSize = 18, MaxWidth = 170, Margin = new Thickness(4, 5, 0, 0), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top, Padding = new Thickness(0) };

            stackPanel.Tag = sourceType.Name;

            //Console.Write("*******TAG = ");
            //Console.WriteLine(stackPanel.Tag);

            stackPanel.Children.Add(sourceTypeIconEllipseGrid);
            stackPanel.Children.Add(eventTypeName);

            return stackPanel;
        }
    }
}