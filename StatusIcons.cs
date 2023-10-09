using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows;

namespace Event_Horizon
{
    public class StatusIcons
    {
        public static Grid GetStatus(int _Status)
        {
            Grid statusGrid = new Grid();

            StackPanel stackPanel = new StackPanel { Orientation = Orientation.Horizontal, Width = 107};

            Grid grid;
            StackPanel horizontalStackPanel;
            Label l1;
            Label l2;
            Label l3;
            Label l4;
            Rectangle r1;
            Rectangle r2;
            Rectangle r3;
            Rectangle r4;
            Grid g1;
            Grid g2;
            Grid g3;
            Grid g4;

            grid = new Grid { };
            horizontalStackPanel = new StackPanel { Orientation = Orientation.Horizontal };

            horizontalStackPanel.Margin = new Thickness(4, 0, 0, 0);

            switch (_Status)
            {
                case Statuses.Inactive:
                    g1 = new Grid();
                    r1 = new Rectangle { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFe9effe")), Width = 23, Height = 23, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0 };
                    g1.Children.Add(r1);
                    l1 = new Label { Content = "A", FontSize = 10, Width = 23, Height = 23, Foreground = Brushes.Black, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, Padding = new System.Windows.Thickness(0) };
                    g1.Children.Add(l1);
                    g1.Opacity = 0.1;
                    horizontalStackPanel.Children.Add(g1);

                    g2 = new Grid { Margin = new Thickness(3, 0, 0, 0) };
                    r2 = new Rectangle { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFe9effe")), Width = 23, Height = 23, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0 };
                    g2.Children.Add(r2);
                    l2 = new Label { Content = "N", FontSize = 10, Width = 23, Height = 23, Foreground = Brushes.Black, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, Padding = new System.Windows.Thickness(0) };
                    g2.Children.Add(l2);
                    g2.Opacity = 0.1;
                    horizontalStackPanel.Children.Add(g2);
                    
                    g3 = new Grid { Margin = new Thickness(3, 0, 0, 0) };
                    r3 = new Rectangle { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFe9effe")), Width = 23, Height = 23, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0 };
                    g3.Children.Add(r3);
                    l3 = new Label { Content = "R", FontSize = 10, Width = 23, Height = 23, Foreground = Brushes.Black, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, Padding = new System.Windows.Thickness(0) };
                    g3.Children.Add(l3);
                    g3.Opacity = 0.1;
                    horizontalStackPanel.Children.Add(g3);
                    
                    g4 = new Grid { Margin = new Thickness(3, 0, 0, 0) };
                    r4 = new Rectangle { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFe9effe")), Width = 23, Height = 23, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0 };
                    g4.Children.Add(r4);
                    l4 = new Label { Content = "A", FontSize = 10, Width = 23, Height = 23, Foreground = Brushes.Black, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, Padding = new System.Windows.Thickness(0) };
                    g4.Children.Add(l4);
                    g4.Opacity = 0.1;
                    horizontalStackPanel.Children.Add(g4);
                    break;
                case Statuses.Active:
                    g1 = new Grid();
                    r1 = new Rectangle { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA0B392")), Width = 23, Height = 23, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0 };
                    g1.Children.Add(r1);
                    l1 = new Label { Content = "A", FontSize = 10, Width = 23, Height = 23, Foreground = Brushes.Black, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, Padding = new System.Windows.Thickness(0) };
                    g1.Children.Add(l1);
                    g1.Opacity = 1;
                    horizontalStackPanel.Children.Add(g1);

                    g2 = new Grid { Margin = new Thickness(3, 0, 0, 0) };
                    r2 = new Rectangle { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFe9effe")), Width = 23, Height = 23, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0 };
                    g2.Children.Add(r2);
                    l2 = new Label { Content = "N", FontSize = 10, Width = 23, Height = 23, Foreground = Brushes.Black, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, Padding = new System.Windows.Thickness(0) };
                    g2.Children.Add(l2);
                    g2.Opacity = 0.1;
                    horizontalStackPanel.Children.Add(g2);

                    g3 = new Grid { Margin = new Thickness(3, 0, 0, 0) };
                    r3 = new Rectangle { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFe9effe")), Width = 23, Height = 23, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0 };
                    g3.Children.Add(r3);
                    l3 = new Label { Content = "R", FontSize = 10, Width = 23, Height = 23, Foreground = Brushes.Black, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, Padding = new System.Windows.Thickness(0) };
                    g3.Children.Add(l3);
                    g3.Opacity = 0.1;
                    horizontalStackPanel.Children.Add(g3);

                    g4 = new Grid { Margin = new Thickness(3, 0, 0, 0) };
                    r4 = new Rectangle { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFe9effe")), Width = 23, Height = 23, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0 };
                    g4.Children.Add(r4);
                    l4 = new Label { Content = "A", FontSize = 10, Width = 23, Height = 23, Foreground = Brushes.Black, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, Padding = new System.Windows.Thickness(0) };
                    g4.Children.Add(l4);
                    g4.Opacity = 0.1;
                    horizontalStackPanel.Children.Add(g4);
                    break;
                case Statuses.ActiveNotified:
                    g1 = new Grid();
                    r1 = new Rectangle { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA0B392")), Width = 23, Height = 23, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0 };
                    g1.Children.Add(r1);
                    l1 = new Label { Content = "A", FontSize = 10, Width = 23, Height = 23, Foreground = Brushes.Black, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, Padding = new System.Windows.Thickness(0) };
                    g1.Children.Add(l1);
                    g1.Opacity = 1;
                    horizontalStackPanel.Children.Add(g1);

                    g2 = new Grid { Margin = new Thickness(3, 0, 0, 0) };
                    r2 = new Rectangle { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF71B1D9")), Width = 23, Height = 23, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0 };
                    g2.Children.Add(r2);
                    l2 = new Label { Content = "N", FontSize = 10, Width = 23, Height = 23, Foreground = Brushes.Black, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, Padding = new System.Windows.Thickness(0) };
                    g2.Children.Add(l2);
                    g2.Opacity = 1;
                    horizontalStackPanel.Children.Add(g2);

                    g3 = new Grid { Margin = new Thickness(3, 0, 0, 0) };
                    r3 = new Rectangle { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFe9effe")), Width = 23, Height = 23, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0 };
                    g3.Children.Add(r3);
                    l3 = new Label { Content = "R", FontSize = 10, Width = 23, Height = 23, Foreground = Brushes.Black, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, Padding = new System.Windows.Thickness(0) };
                    g3.Children.Add(l3);
                    g3.Opacity = 0.1;
                    horizontalStackPanel.Children.Add(g3);

                    g4 = new Grid { Margin = new Thickness(3, 0, 0, 0) };
                    r4 = new Rectangle { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFe9effe")), Width = 23, Height = 23, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0 };
                    g4.Children.Add(r4);
                    l4 = new Label { Content = "A", FontSize = 10, Width = 23, Height = 23, Foreground = Brushes.Black, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, Padding = new System.Windows.Thickness(0)};
                    g4.Children.Add(l4);
                    g4.Opacity = 0.1;
                    horizontalStackPanel.Children.Add(g4);
                    break;
                case Statuses.ActiveNotifiedRead:
                    g1 = new Grid();
                    r1 = new Rectangle { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA0B392")), Width = 23, Height = 23, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0 };
                    g1.Children.Add(r1);
                    l1 = new Label { Content = "A", FontSize = 10, Width = 23, Height = 23, Foreground = Brushes.Black, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, Padding = new System.Windows.Thickness(0) };
                    g1.Children.Add(l1);
                    g1.Opacity = 1;
                    horizontalStackPanel.Children.Add(g1);

                    g2 = new Grid { Margin = new Thickness(3, 0, 0, 0) };
                    r2 = new Rectangle { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF71B1D9")), Width = 23, Height = 23, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0 };
                    g2.Children.Add(r2);
                    l2 = new Label { Content = "N", FontSize = 10, Width = 23, Height = 23, Foreground = Brushes.Black, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, Padding = new System.Windows.Thickness(0) };
                    g2.Children.Add(l2);
                    g2.Opacity = 1;
                    horizontalStackPanel.Children.Add(g2);

                    g3 = new Grid { Margin = new Thickness(3, 0, 0, 0) };
                    r3 = new Rectangle { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#c99ac8")), Width = 23, Height = 23, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0 };
                    g3.Children.Add(r3);
                    l3 = new Label { Content = "R", FontSize = 10, Width = 23, Height = 23, Foreground = Brushes.Black, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, Padding = new System.Windows.Thickness(0)};
                    g3.Children.Add(l3);
                    g3.Opacity = 1;
                    horizontalStackPanel.Children.Add(g3);

                    g4 = new Grid { Margin = new Thickness(3, 0, 0, 0) };
                    r4 = new Rectangle { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFe9effe")), Width = 23, Height = 23, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0 };
                    g4.Children.Add(r4);
                    l4 = new Label { Content = "A", FontSize = 10, Width = 23, Height = 23, Foreground = Brushes.Black, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, Padding = new System.Windows.Thickness(0)};
                    g4.Children.Add(l4);
                    g4.Opacity = 0.1;
                    horizontalStackPanel.Children.Add(g4);
                    break;
                case Statuses.ActiveNotifiedReadArchived:
                    g1 = new Grid();
                    r1 = new Rectangle { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA0B392")), Width = 23, Height = 23, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0 };
                    g1.Children.Add(r1);
                    l1 = new Label { Content = "A", FontSize = 10, Width = 23, Height = 23, Foreground = Brushes.Black, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, Padding = new System.Windows.Thickness(0) };
                    g1.Children.Add(l1);
                    g1.Opacity = 1;
                    horizontalStackPanel.Children.Add(g1);

                    g2 = new Grid { Margin = new Thickness(3, 0, 0, 0) };
                    r2 = new Rectangle { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF71B1D9")), Width = 23, Height = 23, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0 };
                    g2.Children.Add(r2);
                    l2 = new Label { Content = "N", FontSize = 10, Width = 23, Height = 23, Foreground = Brushes.Black, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, Padding = new System.Windows.Thickness(0) };
                    g2.Children.Add(l2);
                    g2.Opacity = 1;
                    horizontalStackPanel.Children.Add(g2);

                    g3 = new Grid { Margin = new Thickness(3, 0, 0, 0) };
                    r3 = new Rectangle { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#c99ac8")), Width = 23, Height = 23, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0 };
                    g3.Children.Add(r3);
                    l3 = new Label { Content = "R", FontSize = 10, Width = 23, Height = 23, Foreground = Brushes.Black, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, Padding = new System.Windows.Thickness(0)};
                    g3.Children.Add(l3);
                    g3.Opacity = 1;
                    horizontalStackPanel.Children.Add(g3);

                    g4 = new Grid { Margin = new Thickness(3, 0, 0, 0) };
                    r4 = new Rectangle { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e0c36a")), Width = 23, Height = 23, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0 };
                    g4.Children.Add(r4);
                    l4 = new Label { Content = "A", FontSize = 10, Width = 23, Height = 23, Foreground = Brushes.Black, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, Padding = new System.Windows.Thickness(0) };
                    g4.Children.Add(l4);
                    g4.Opacity = 1;
                    horizontalStackPanel.Children.Add(g4);
                    break;
            }

            grid.Children.Add(horizontalStackPanel);
            stackPanel.Children.Add(grid);

            statusGrid.Opacity = 1;
            statusGrid.Children.Add(stackPanel);

            statusGrid.Effect = new DropShadowEffect
            {
                Color = new Color { A = 255, R = 0, G = 0, B = 0 },
                Direction = 320,
                ShadowDepth = 1,
                Opacity = 0.6
            };

            return statusGrid;
        }
    }
}