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
    public class Frequency
    {
        public static Grid GetFrequency(int _frequency, bool ShowDropShadow = true)
        {
            Grid FrequenceyGrid = new Grid { IsEnabled = false };

            switch (_frequency)
            {
                case EventFrequencys.Common_OneTime:
                    FrequenceyGrid.Children.Add(CreateFrequencyVisual(1, 1, 96, 23, 0, 0, "#f7f8f9", "ONCE", ShowDropShadow));
                    break;
                case EventFrequencys.Common_Quarterly:
                    FrequenceyGrid.Children.Add(CreateFrequencyVisual(4, 1, 20.25, 23, 5, 0, "#FF6cab98", "QUARTERLY", ShowDropShadow));
                    break;
                case EventFrequencys.Common_SixMonthly:
                    FrequenceyGrid.Children.Add(CreateFrequencyVisual(2, 1, 45.5, 23, 5, 0, "#FF6cab98", "6 MONTHLY", ShowDropShadow)); 
                    break;
                case EventFrequencys.Common_Yearly:
                    FrequenceyGrid.Children.Add(CreateFrequencyVisual(1, 1, 96, 23, 0, 0, "#FF99ccff", "YEARLY", ShowDropShadow)); 
                    break;
                case EventFrequencys.Common_TwoYearly:
                    FrequenceyGrid.Children.Add(CreateFrequencyVisual(1, 2, 96, 9, 0, 2, "#FF99ccff", "2 YEARS", ShowDropShadow));
                    break;
                case EventFrequencys.Common_ThreeYearly:
                    FrequenceyGrid.Children.Add(CreateFrequencyVisual(1, 3, 96, 5.3, 0, 2, "#FF99ccff", "3 YEARS", ShowDropShadow)); 
                    break;
                case EventFrequencys.Common_FiveYearly:
                    FrequenceyGrid.Children.Add(CreateFrequencyVisual(1, 5, 96, 3, 0, 2, "#FF99ccff", "5 YEARS", ShowDropShadow));                
                    break;
                case EventFrequencys.Minutes_01:
                    FrequenceyGrid.Children.Add(CreateFrequencyVisual(1, 1, 96, 23, 0, 0, "#c99ac8", "MINUTE", ShowDropShadow));
                    break;
                case EventFrequencys.Minutes_05:
                    FrequenceyGrid.Children.Add(CreateFrequencyVisual(5, 1, 15.3, 23, 5, 0, "#c99ac8", "5 MINUTES", ShowDropShadow));
                    break;
                case EventFrequencys.Minutes_10:
                    FrequenceyGrid.Children.Add(CreateFrequencyVisual(10, 1, 7.6, 23, 2, 2, "#c99ac8", "10 MINUTES", ShowDropShadow));
                    break;
                case EventFrequencys.Minutes_30:
                    FrequenceyGrid.Children.Add(CreateFrequencyVisual(30, 1, 1.4, 23, 2, 0, "#c99ac8", "30 MINUTES", ShowDropShadow));
                    break;
                case EventFrequencys.Hours_01:
                    FrequenceyGrid.Children.Add(CreateFrequencyVisual(1, 1, 96, 23, 0, 0, "#FF71B1D9", "HOURLY", ShowDropShadow));
                    break;
                case EventFrequencys.Hours_02:
                    FrequenceyGrid.Children.Add(CreateFrequencyVisual(2, 1, 45.5, 23, 5, 0, "#FF71B1D9", "2 HOURLY", ShowDropShadow));
                    break;
                case EventFrequencys.Hours_03:
                    FrequenceyGrid.Children.Add(CreateFrequencyVisual(3, 1, 28, 23, 5, 0, "#FF71B1D9", "3 HOURLY", ShowDropShadow));
                    break;
                case EventFrequencys.Hours_08:
                    FrequenceyGrid.Children.Add(CreateFrequencyVisual(8, 1, 7.4, 23, 5, 0, "#FF71B1D9", "8 HOURLY", ShowDropShadow));
                    break;
                case EventFrequencys.Hours_12:
                    FrequenceyGrid.Children.Add(CreateFrequencyVisual(12, 1, 5.2, 23, 3, 0, "#FF71B1D9", "12 HOURLY", ShowDropShadow));
                    break;
                case EventFrequencys.Days_01:
                    FrequenceyGrid.Children.Add(CreateFrequencyVisual(1, 1, 96, 23, 0, 0, "#FFA0B392", "DAILY", ShowDropShadow));
                    break;
                case EventFrequencys.Days_02:
                    FrequenceyGrid.Children.Add(CreateFrequencyVisual(2, 1, 45.5, 23, 5, 0, "#FFA0B392", "2 DAILY", ShowDropShadow));
                    break;
                case EventFrequencys.Days_03:
                    FrequenceyGrid.Children.Add(CreateFrequencyVisual(3, 1, 28, 23, 5, 0, "#FFA0B392", "3 DAILY", ShowDropShadow));
                    break;
                case EventFrequencys.Weeks_01:
                    FrequenceyGrid.Children.Add(CreateFrequencyVisual(1, 1, 96, 23, 0, 0, "#FF18c792", "WEEKLY", ShowDropShadow));
                    break;
                case EventFrequencys.Weeks_02:
                    FrequenceyGrid.Children.Add(CreateFrequencyVisual(2, 1, 45.5, 23, 5, 0, "#FF18c792", "FORTNIGHTLY", ShowDropShadow));
                    break;
                case EventFrequencys.Months_01:
                    FrequenceyGrid.Children.Add(CreateFrequencyVisual(1, 1, 96, 23, 0, 0, "#FF6cab98", "MONTHLY", ShowDropShadow));
                    break;
                case EventFrequencys.Months_02:
                    FrequenceyGrid.Children.Add(CreateFrequencyVisual(2, 1, 45.5, 23, 5, 0, "#FF6cab98", "2 MONTHLY", ShowDropShadow));
                    break;
                case EventFrequencys.Months_03:
                    FrequenceyGrid.Children.Add(CreateFrequencyVisual(3, 1, 28, 23, 5, 0, "#FF6cab98", "3 MONTHLY", ShowDropShadow));
                    break;
                case EventFrequencys.Months_09:
                    FrequenceyGrid.Children.Add(CreateFrequencyVisual(9, 1, 6, 23, 5, 0, "#FF6cab98", "9 MONTHLY", ShowDropShadow));
                    break;
                default:
                    FrequenceyGrid.Children.Add(CreateFrequencyVisual(1, 1, 96, 23, 0, 0, "#f7f8f9", "ONE OFF", ShowDropShadow));
                    break;
            }

            return FrequenceyGrid;
        }

        public static Grid CreateFrequencyVisual(int SegmentsWide, int SegmentsTall, Double SegmentWidth, Double SegmentHeight, int SegmentXMargin, int SegmentYMargin, String SegmentColor, String LabelContent, bool ShowDropShadow = false)
        {
            Grid ReturnGrid = new Grid();

            StackPanel sp = new StackPanel { Orientation = Orientation.Horizontal, Width = 100 };

            Grid gd = new Grid { };
            StackPanel h = new StackPanel { Orientation = Orientation.Vertical, Height = 23 };

            Label label = new Label { Content = LabelContent, FontSize = 13, Width = 105, Height = 20, FontWeight = FontWeights.Normal, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, Padding = new System.Windows.Thickness(0), Margin = new Thickness(0,0,0,0) };
            Rectangle LabelBackground = new Rectangle { Fill = new SolidColorBrush(Colors.White), Width = 83, Height = 12, Stroke = new SolidColorBrush(Colors.Transparent), StrokeThickness = 0, Margin = new System.Windows.Thickness(11, -1, 11, 0) };

            Rectangle r;
            StackPanel sh;

            Double spaceX;
            Double spaceY;

            for (int st = 0; st < SegmentsTall; st++)
            {         
                sh = new StackPanel { Orientation = Orientation.Horizontal, Width = 96 };
                
                for (int sw = 0; sw < SegmentsWide; sw++)
                {
                    if (st > 0)
                        spaceY = SegmentYMargin;
                    else
                        spaceY = 0;

                    if (sw > 0)
                        spaceX = SegmentXMargin;
                    else
                        spaceX = 0;

                    r = new Rectangle { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(SegmentColor)), Width = SegmentWidth, Height = SegmentHeight, Margin = new System.Windows.Thickness(spaceX, spaceY, 0, 0) };

                    sh.Children.Add(r);
                }
                h.Children.Add(sh);
            }

            gd.Children.Add(h);            
            gd.Children.Add(LabelBackground);
            gd.Children.Add(label);
            sp.Children.Add(gd);

            ReturnGrid.Opacity = 1;
            ReturnGrid.Children.Add(sp);

            if (ShowDropShadow)
            {
                ReturnGrid.Effect = new DropShadowEffect
                {
                    Color = new Color { A = 255, R = 0, G = 0, B = 0 },
                    Direction = 320,
                    ShadowDepth = 1,
                    Opacity = 0.6
                };
            }

            return ReturnGrid;
        }
    }
}