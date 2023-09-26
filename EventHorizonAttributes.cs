using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using FontAwesome.WPF;

namespace The_Oracle
{
    public class EventHorizonAttributes
    {
        public static StackPanel GetAttributeStackPanel(AttributeType attributeType)
        {
            StackPanel stackPanel = new StackPanel();

            stackPanel.Orientation = Orientation.Horizontal;

            Grid attributeTypeIconEllipseGrid;

            Color iconColor = XMLReaderWriter.AttributeTypesList[attributeType.ID].Color;
            FontAwesomeIcon fontAwesomeIcon = XMLReaderWriter.AttributeTypesList[attributeType.ID].Icon;

            Border iconBorder = new Border { Width = 28, Height = 28, BorderThickness = new Thickness(0), CornerRadius = new CornerRadius(3), Background = new SolidColorBrush(iconColor), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top, Margin = new Thickness(1, 2, 0, 0), Padding = new Thickness(0) };

            FontAwesome.WPF.FontAwesome fa = new FontAwesome.WPF.FontAwesome { Icon = fontAwesomeIcon, Width = 28, Height = 28, FontSize = 17, Foreground = new SolidColorBrush(Colors.White), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top, Margin = new Thickness(0, 5, 0, 0) };

            iconBorder.Child = fa;

            attributeTypeIconEllipseGrid = new Grid { Margin = new Thickness(0, 1, 3, 3), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };

            attributeTypeIconEllipseGrid.Children.Add(iconBorder);

            attributeTypeIconEllipseGrid.Opacity = 1;

            attributeTypeIconEllipseGrid.Effect = new DropShadowEffect
            {
                Color = new Color { A = 255, R = 0, G = 0, B = 0 },
                Direction = 320,
                ShadowDepth = 1,
                Opacity = 0.6
            };

            TextBlock attributeTypeName = new TextBlock { Text = attributeType.Name, Foreground = Brushes.Black, FontSize = 18, MaxWidth = 170, Margin = new Thickness(4, 5, 0, 0), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top, Padding = new Thickness(0) };

            stackPanel.Tag = attributeType.Name;

            //Console.Write("*******TAG = ");
            //Console.WriteLine(stackPanel.Tag);

            stackPanel.Children.Add(attributeTypeIconEllipseGrid);
            stackPanel.Children.Add(attributeTypeName);

            return stackPanel;
        }
    }
}