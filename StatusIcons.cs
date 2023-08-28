using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows;

namespace The_Oracle
{
    public class StatusIcons
    {
        public static Grid GetStatus(int _Status)
        {
            Grid StatusGrid = new Grid();

            StackPanel sp = new StackPanel { Orientation = Orientation.Horizontal, Width = 107};

            Grid gd;
            StackPanel s;
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

            gd = new Grid { };
            s = new StackPanel { Orientation = Orientation.Horizontal };

            switch (_Status)
            {
                case Statuses.Inactive:
                    g1 = new Grid();
                    r1 = new Rectangle { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFe9effe")), Width = 23, Height = 23, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0 };
                    g1.Children.Add(r1);
                    l1 = new Label { Content = "A", FontSize = 10, Width = 23, Height = 23, Foreground = Brushes.Black, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, Padding = new System.Windows.Thickness(0) };
                    g1.Children.Add(l1);
                    g1.Opacity = 0.1;
                    s.Children.Add(g1);

                    g2 = new Grid { Margin = new Thickness(3, 0, 0, 0) };
                    r2 = new Rectangle { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFe9effe")), Width = 23, Height = 23, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0 };
                    g2.Children.Add(r2);
                    l2 = new Label { Content = "N", FontSize = 10, Width = 23, Height = 23, Foreground = Brushes.Black, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, Padding = new System.Windows.Thickness(0) };
                    g2.Children.Add(l2);
                    g2.Opacity = 0.1;
                    s.Children.Add(g2);
                    
                    g3 = new Grid { Margin = new Thickness(3, 0, 0, 0) };
                    r3 = new Rectangle { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFe9effe")), Width = 23, Height = 23, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0 };
                    g3.Children.Add(r3);
                    l3 = new Label { Content = "R", FontSize = 10, Width = 23, Height = 23, Foreground = Brushes.Black, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, Padding = new System.Windows.Thickness(0) };
                    g3.Children.Add(l3);
                    g3.Opacity = 0.1;
                    s.Children.Add(g3);
                    
                    g4 = new Grid { Margin = new Thickness(3, 0, 0, 0) };
                    r4 = new Rectangle { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFe9effe")), Width = 23, Height = 23, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0 };
                    g4.Children.Add(r4);
                    l4 = new Label { Content = "A", FontSize = 10, Width = 23, Height = 23, Foreground = Brushes.Black, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, Padding = new System.Windows.Thickness(0) };
                    g4.Children.Add(l4);
                    g4.Opacity = 0.1;
                    s.Children.Add(g4);
                    break;
                case Statuses.Active:
                    g1 = new Grid();
                    r1 = new Rectangle { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA0B392")), Width = 23, Height = 23, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0 };
                    g1.Children.Add(r1);
                    l1 = new Label { Content = "A", FontSize = 10, Width = 23, Height = 23, Foreground = Brushes.Black, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, Padding = new System.Windows.Thickness(0) };
                    g1.Children.Add(l1);
                    g1.Opacity = 1;
                    s.Children.Add(g1);

                    g2 = new Grid { Margin = new Thickness(3, 0, 0, 0) };
                    r2 = new Rectangle { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFe9effe")), Width = 23, Height = 23, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0 };
                    g2.Children.Add(r2);
                    l2 = new Label { Content = "N", FontSize = 10, Width = 23, Height = 23, Foreground = Brushes.Black, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, Padding = new System.Windows.Thickness(0) };
                    g2.Children.Add(l2);
                    g2.Opacity = 0.1;
                    s.Children.Add(g2);

                    g3 = new Grid { Margin = new Thickness(3, 0, 0, 0) };
                    r3 = new Rectangle { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFe9effe")), Width = 23, Height = 23, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0 };
                    g3.Children.Add(r3);
                    l3 = new Label { Content = "R", FontSize = 10, Width = 23, Height = 23, Foreground = Brushes.Black, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, Padding = new System.Windows.Thickness(0) };
                    g3.Children.Add(l3);
                    g3.Opacity = 0.1;
                    s.Children.Add(g3);

                    g4 = new Grid { Margin = new Thickness(3, 0, 0, 0) };
                    r4 = new Rectangle { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFe9effe")), Width = 23, Height = 23, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0 };
                    g4.Children.Add(r4);
                    l4 = new Label { Content = "A", FontSize = 10, Width = 23, Height = 23, Foreground = Brushes.Black, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, Padding = new System.Windows.Thickness(0) };
                    g4.Children.Add(l4);
                    g4.Opacity = 0.1;
                    s.Children.Add(g4);
                    break;
                case Statuses.ActiveNotified:
                    g1 = new Grid();
                    r1 = new Rectangle { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA0B392")), Width = 23, Height = 23, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0 };
                    g1.Children.Add(r1);
                    l1 = new Label { Content = "A", FontSize = 10, Width = 23, Height = 23, Foreground = Brushes.Black, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, Padding = new System.Windows.Thickness(0) };
                    g1.Children.Add(l1);
                    g1.Opacity = 1;
                    s.Children.Add(g1);

                    g2 = new Grid { Margin = new Thickness(3, 0, 0, 0) };
                    r2 = new Rectangle { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF71B1D9")), Width = 23, Height = 23, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0 };
                    g2.Children.Add(r2);
                    l2 = new Label { Content = "N", FontSize = 10, Width = 23, Height = 23, Foreground = Brushes.Black, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, Padding = new System.Windows.Thickness(0) };
                    g2.Children.Add(l2);
                    g2.Opacity = 1;
                    s.Children.Add(g2);

                    g3 = new Grid { Margin = new Thickness(3, 0, 0, 0) };
                    r3 = new Rectangle { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFe9effe")), Width = 23, Height = 23, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0 };
                    g3.Children.Add(r3);
                    l3 = new Label { Content = "R", FontSize = 10, Width = 23, Height = 23, Foreground = Brushes.Black, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, Padding = new System.Windows.Thickness(0) };
                    g3.Children.Add(l3);
                    g3.Opacity = 0.1;
                    s.Children.Add(g3);

                    g4 = new Grid { Margin = new Thickness(3, 0, 0, 0) };
                    r4 = new Rectangle { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFe9effe")), Width = 23, Height = 23, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0 };
                    g4.Children.Add(r4);
                    l4 = new Label { Content = "A", FontSize = 10, Width = 23, Height = 23, Foreground = Brushes.Black, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, Padding = new System.Windows.Thickness(0)};
                    g4.Children.Add(l4);
                    g4.Opacity = 0.1;
                    s.Children.Add(g4);
                    break;
                case Statuses.ActiveNotifiedRead:
                    g1 = new Grid();
                    r1 = new Rectangle { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA0B392")), Width = 23, Height = 23, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0 };
                    g1.Children.Add(r1);
                    l1 = new Label { Content = "A", FontSize = 10, Width = 23, Height = 23, Foreground = Brushes.Black, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, Padding = new System.Windows.Thickness(0) };
                    g1.Children.Add(l1);
                    g1.Opacity = 1;
                    s.Children.Add(g1);

                    g2 = new Grid { Margin = new Thickness(3, 0, 0, 0) };
                    r2 = new Rectangle { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF71B1D9")), Width = 23, Height = 23, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0 };
                    g2.Children.Add(r2);
                    l2 = new Label { Content = "N", FontSize = 10, Width = 23, Height = 23, Foreground = Brushes.Black, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, Padding = new System.Windows.Thickness(0) };
                    g2.Children.Add(l2);
                    g2.Opacity = 1;
                    s.Children.Add(g2);

                    g3 = new Grid { Margin = new Thickness(3, 0, 0, 0) };
                    r3 = new Rectangle { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#c99ac8")), Width = 23, Height = 23, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0 };
                    g3.Children.Add(r3);
                    l3 = new Label { Content = "R", FontSize = 10, Width = 23, Height = 23, Foreground = Brushes.Black, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, Padding = new System.Windows.Thickness(0)};
                    g3.Children.Add(l3);
                    g3.Opacity = 1;
                    s.Children.Add(g3);

                    g4 = new Grid { Margin = new Thickness(3, 0, 0, 0) };
                    r4 = new Rectangle { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFe9effe")), Width = 23, Height = 23, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0 };
                    g4.Children.Add(r4);
                    l4 = new Label { Content = "A", FontSize = 10, Width = 23, Height = 23, Foreground = Brushes.Black, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, Padding = new System.Windows.Thickness(0)};
                    g4.Children.Add(l4);
                    g4.Opacity = 0.1;
                    s.Children.Add(g4);
                    break;
                case Statuses.ActiveNotifiedReadArchived:
                    g1 = new Grid();
                    r1 = new Rectangle { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA0B392")), Width = 23, Height = 23, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0 };
                    g1.Children.Add(r1);
                    l1 = new Label { Content = "A", FontSize = 10, Width = 23, Height = 23, Foreground = Brushes.Black, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, Padding = new System.Windows.Thickness(0) };
                    g1.Children.Add(l1);
                    g1.Opacity = 1;
                    s.Children.Add(g1);

                    g2 = new Grid { Margin = new Thickness(3, 0, 0, 0) };
                    r2 = new Rectangle { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF71B1D9")), Width = 23, Height = 23, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0 };
                    g2.Children.Add(r2);
                    l2 = new Label { Content = "N", FontSize = 10, Width = 23, Height = 23, Foreground = Brushes.Black, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, Padding = new System.Windows.Thickness(0) };
                    g2.Children.Add(l2);
                    g2.Opacity = 1;
                    s.Children.Add(g2);

                    g3 = new Grid { Margin = new Thickness(3, 0, 0, 0) };
                    r3 = new Rectangle { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#c99ac8")), Width = 23, Height = 23, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0 };
                    g3.Children.Add(r3);
                    l3 = new Label { Content = "R", FontSize = 10, Width = 23, Height = 23, Foreground = Brushes.Black, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, Padding = new System.Windows.Thickness(0)};
                    g3.Children.Add(l3);
                    g3.Opacity = 1;
                    s.Children.Add(g3);

                    g4 = new Grid { Margin = new Thickness(3, 0, 0, 0) };
                    r4 = new Rectangle { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e0c36a")), Width = 23, Height = 23, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 0 };
                    g4.Children.Add(r4);
                    l4 = new Label { Content = "A", FontSize = 10, Width = 23, Height = 23, Foreground = Brushes.Black, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, Padding = new System.Windows.Thickness(0) };
                    g4.Children.Add(l4);
                    g4.Opacity = 1;
                    s.Children.Add(g4);
                    break;
            }

            gd.Children.Add(s);
            //r6 = new Rectangle { Fill = new SolidColorBrush(Colors.White), Width = 74, Height = 12, Stroke = new SolidColorBrush(Colors.Transparent), StrokeThickness = 0, Margin = new System.Windows.Thickness(11, 0, 11, 0) };
            //gd.Children.Add(r6);
            sp.Children.Add(gd);

            StatusGrid.Opacity = 1;
            StatusGrid.Children.Add(sp);

            StatusGrid.Effect = new DropShadowEffect
            {
                Color = new Color { A = 255, R = 0, G = 0, B = 0 },
                Direction = 320,
                ShadowDepth = 1,
                Opacity = 0.6
            };

            return StatusGrid;
        }
    }
}
