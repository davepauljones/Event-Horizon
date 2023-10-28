using System;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows;

namespace Event_Horizon
{
    public class Frequency
    {
        public static Grid GetFrequency(int frequency, bool showDropShadow = true)
        {
            Grid frequenceyGrid = new Grid { IsEnabled = false };

            switch (frequency)
            {
                case EventFrequencys.Common_OneTime:
                    frequenceyGrid.Children.Add(CreateFrequencyVisual(1, 1, 96, 23, 0, 0, "#f7f8f9", "ONCE", showDropShadow));
                    frequenceyGrid.Visibility = Visibility.Hidden;
                    break;
                case EventFrequencys.Common_Quarterly:
                    frequenceyGrid.Children.Add(CreateFrequencyVisual(4, 1, 20.25, 23, 5, 0, "#FF6cab98", "QUARTERLY", showDropShadow));
                    break;
                case EventFrequencys.Common_SixMonthly:
                    frequenceyGrid.Children.Add(CreateFrequencyVisual(2, 1, 45.5, 23, 5, 0, "#FF6cab98", "6 MONTHLY", showDropShadow)); 
                    break;
                case EventFrequencys.Common_Yearly:
                    frequenceyGrid.Children.Add(CreateFrequencyVisual(1, 1, 96, 23, 0, 0, "#FF99ccff", "YEARLY", showDropShadow)); 
                    break;
                case EventFrequencys.Common_TwoYearly:
                    frequenceyGrid.Children.Add(CreateFrequencyVisual(1, 2, 96, 9, 0, 2, "#FF99ccff", "2 YEARS", showDropShadow));
                    break;
                case EventFrequencys.Common_ThreeYearly:
                    frequenceyGrid.Children.Add(CreateFrequencyVisual(1, 3, 96, 5.3, 0, 2, "#FF99ccff", "3 YEARS", showDropShadow)); 
                    break;
                case EventFrequencys.Common_FiveYearly:
                    frequenceyGrid.Children.Add(CreateFrequencyVisual(1, 5, 96, 3, 0, 2, "#FF99ccff", "5 YEARS", showDropShadow));                
                    break;
                case EventFrequencys.Minutes_01:
                    frequenceyGrid.Children.Add(CreateFrequencyVisual(1, 1, 96, 23, 0, 0, "#c99ac8", "MINUTE", showDropShadow));
                    break;
                case EventFrequencys.Minutes_05:
                    frequenceyGrid.Children.Add(CreateFrequencyVisual(5, 1, 15.3, 23, 5, 0, "#c99ac8", "5 MINUTES", showDropShadow));
                    break;
                case EventFrequencys.Minutes_10:
                    frequenceyGrid.Children.Add(CreateFrequencyVisual(10, 1, 7.6, 23, 2, 2, "#c99ac8", "10 MINUTES", showDropShadow));
                    break;
                case EventFrequencys.Minutes_30:
                    frequenceyGrid.Children.Add(CreateFrequencyVisual(30, 1, 1.4, 23, 2, 0, "#c99ac8", "30 MINUTES", showDropShadow));
                    break;
                case EventFrequencys.Hours_01:
                    frequenceyGrid.Children.Add(CreateFrequencyVisual(1, 1, 96, 23, 0, 0, "#FF71B1D9", "HOURLY", showDropShadow));
                    break;
                case EventFrequencys.Hours_02:
                    frequenceyGrid.Children.Add(CreateFrequencyVisual(2, 1, 45.5, 23, 5, 0, "#FF71B1D9", "2 HOURLY", showDropShadow));
                    break;
                case EventFrequencys.Hours_03:
                    frequenceyGrid.Children.Add(CreateFrequencyVisual(3, 1, 28, 23, 5, 0, "#FF71B1D9", "3 HOURLY", showDropShadow));
                    break;
                case EventFrequencys.Hours_08:
                    frequenceyGrid.Children.Add(CreateFrequencyVisual(8, 1, 7.4, 23, 5, 0, "#FF71B1D9", "8 HOURLY", showDropShadow));
                    break;
                case EventFrequencys.Hours_12:
                    frequenceyGrid.Children.Add(CreateFrequencyVisual(12, 1, 5.2, 23, 3, 0, "#FF71B1D9", "12 HOURLY", showDropShadow));
                    break;
                case EventFrequencys.Days_01:
                    frequenceyGrid.Children.Add(CreateFrequencyVisual(1, 1, 96, 23, 0, 0, "#FFA0B392", "DAILY", showDropShadow));
                    break;
                case EventFrequencys.Days_02:
                    frequenceyGrid.Children.Add(CreateFrequencyVisual(2, 1, 45.5, 23, 5, 0, "#FFA0B392", "2 DAILY", showDropShadow));
                    break;
                case EventFrequencys.Days_03:
                    frequenceyGrid.Children.Add(CreateFrequencyVisual(3, 1, 28, 23, 5, 0, "#FFA0B392", "3 DAILY", showDropShadow));
                    break;
                case EventFrequencys.Weeks_01:
                    frequenceyGrid.Children.Add(CreateFrequencyVisual(1, 1, 96, 23, 0, 0, "#FF18c792", "WEEKLY", showDropShadow));
                    break;
                case EventFrequencys.Weeks_02:
                    frequenceyGrid.Children.Add(CreateFrequencyVisual(2, 1, 45.5, 23, 5, 0, "#FF18c792", "FORTNIGHTLY", showDropShadow));
                    break;
                case EventFrequencys.Months_01:
                    frequenceyGrid.Children.Add(CreateFrequencyVisual(1, 1, 96, 23, 0, 0, "#FF6cab98", "MONTHLY", showDropShadow));
                    break;
                case EventFrequencys.Months_02:
                    frequenceyGrid.Children.Add(CreateFrequencyVisual(2, 1, 45.5, 23, 5, 0, "#FF6cab98", "2 MONTHLY", showDropShadow));
                    break;
                case EventFrequencys.Months_03:
                    frequenceyGrid.Children.Add(CreateFrequencyVisual(3, 1, 28, 23, 5, 0, "#FF6cab98", "3 MONTHLY", showDropShadow));
                    break;
                case EventFrequencys.Months_09:
                    frequenceyGrid.Children.Add(CreateFrequencyVisual(9, 1, 6, 23, 5, 0, "#FF6cab98", "9 MONTHLY", showDropShadow));
                    break;
                default:
                    frequenceyGrid.Children.Add(CreateFrequencyVisual(1, 1, 96, 23, 0, 0, "#f7f8f9", "ONE OFF", showDropShadow));
                    break;
            }
            return frequenceyGrid;
        }

        public static Grid CreateFrequencyVisual(int segmentsWide, int segmentsTall, Double segmentWidth, Double segmentHeight, int segmentXMargin, int segmentYMargin, String segmentColor, String labelContent, bool showDropShadow = false)
        {
            Grid returnGrid = new Grid();

            StackPanel stackPanel = new StackPanel { Orientation = Orientation.Horizontal, Width = 100 };

            Grid grid = new Grid { };
            StackPanel verticalStackPanel = new StackPanel { Orientation = Orientation.Vertical, Height = 23 };

            Label label = new Label { Content = labelContent, FontSize = 13, Width = 105, Height = 20, FontWeight = FontWeights.Normal, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, Padding = new System.Windows.Thickness(0), Margin = new Thickness(0,0,0,0) };
            Rectangle labelBackgroundRectangle = new Rectangle { Fill = new SolidColorBrush(Colors.White), Width = 83, Height = 16, Stroke = new SolidColorBrush(Colors.Transparent), StrokeThickness = 0, Margin = new System.Windows.Thickness(11, 0, 11, 0) };

            for (int segmentTall = 0; segmentTall < segmentsTall; segmentTall++)
            {
                StackPanel horizontalStackPanel = new StackPanel { Orientation = Orientation.Horizontal, Width = 96 };
                
                for (int segmentWide = 0; segmentWide < segmentsWide; segmentWide++)
                {
                    Double spaceX;
                    Double spaceY;

                    if (segmentTall > 0)
                        spaceY = segmentYMargin;
                    else
                        spaceY = 0;

                    if (segmentWide > 0)
                        spaceX = segmentXMargin;
                    else
                        spaceX = 0;

                    Rectangle rectangle = new Rectangle { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(segmentColor)), Width = segmentWidth, Height = segmentHeight, Margin = new System.Windows.Thickness(spaceX, spaceY, 0, 0) };

                    horizontalStackPanel.Children.Add(rectangle);
                }
                verticalStackPanel.Children.Add(horizontalStackPanel);
            }

            grid.Children.Add(verticalStackPanel);
            grid.Children.Add(labelBackgroundRectangle);
            grid.Children.Add(label);
            stackPanel.Children.Add(grid);

            returnGrid.Opacity = 1;
            returnGrid.Children.Add(stackPanel);

            if (showDropShadow)
            {
                returnGrid.Effect = new DropShadowEffect
                {
                    Color = new Color { A = 255, R = 0, G = 0, B = 0 },
                    Direction = 320,
                    ShadowDepth = 1,
                    Opacity = 0.6
                };
            }
            return returnGrid;
        }
    }
}