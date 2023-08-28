using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Effects;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Input;
using System.Windows.Shapes;

using FontAwesome.WPF;

namespace The_Oracle
{
    public class OracleEventRow
    {
        //public static StackPanel CreateOracleEventRow(OracleEvent oe, StackPanel sp, byte ListViewToPopulate)
        //{
        //    int ListViewItemTag = 0;
        //    string CreatedDateTimeString = string.Empty;

        //    string EventTypeString = string.Empty;
        //    string SourceIDString = string.Empty;
        //    string DetailsString = string.Empty;
        //    string FrequencyIDString = string.Empty;
        //    string StatusIDString = string.Empty;

        //    string TargetUserIDString = string.Empty;

        //    string TargetDateTimeString = string.Empty;
        //    Int32 UserID = 0;
        //    Int32 TargetUserID = 0;

        //    FontAwesomeIcon EventTypeIcon = FontAwesomeIcon.Magic;
        //    Color EventIconColor = Colors.SlateGray;

        //    int Replies = 0;

        //    ListViewItemTag = oe.ID;

        //    Replies = MainWindow.mw.GetNumberOfReplies(ListViewItemTag);

        //    Console.Write("Replies for EventID ");
        //    Console.Write(ListViewItemTag);
        //    Console.Write(" is ");
        //    Console.WriteLine(Replies);

        //    EventTypeString = XMLReaderWriter.EventTypesList[oe.EventTypeID].Name;
        //    EventTypeIcon = XMLReaderWriter.EventTypesList[oe.EventTypeID].Icon;
        //    EventIconColor = XMLReaderWriter.EventTypesList[oe.EventTypeID].Color;

        //    SourceIDString = XMLReaderWriter.SourceTypesList[oe.SourceID].Name;

        //    DetailsString = oe.Details;

        //    switch (oe.StatusID)
        //    {
        //        case Statuses.Inactive:
        //            StatusIDString = "Inactive";
        //            break;
        //        case Statuses.Active:
        //            StatusIDString = "Active";
        //            break;
        //    }

        //    CreatedDateTimeString = oe.CreationDate.ToString();

        //    TargetDateTimeString = oe.TargetDate.ToString();

        //    UserID = oe.UserID;

        //    TargetUserID = oe.TargetUserID;

        //    DateTime cdt = DateTime.MinValue;

        //    if (DateTime.TryParse(CreatedDateTimeString, out cdt))
        //    {
        //        //Console.WriteLine(cdt);

        //        CreatedDateTimeString = cdt.ToString("dd/MM/y HH:mm");
        //    }
        //    else
        //    {
        //        Console.WriteLine("Unable to parse TargetDateTimeString '{0}'", CreatedDateTimeString);
        //    }

        //    DateTime tdt = DateTime.MinValue;

        //    if (DateTime.TryParse(TargetDateTimeString, out tdt))
        //    {
        //        //Console.WriteLine(tdt);

        //        if (tdt.TimeOfDay == TimeSpan.Zero)
        //        {
        //            TargetDateTimeString = tdt.ToString("dd/MM/y");
        //        }
        //        else
        //        {
        //            TargetDateTimeString = tdt.ToString("dd/MM/y HH:mm");
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("Unable to parse TargetDateTimeString '{0}'", TargetDateTimeString);
        //    }

        //    TimeSpan ts = MainWindow.mw.ReminderListTimeSpan;

        //    Grid IconEllipseGrid;
        //    Ellipse IconEllipse;

        //    IconEllipseGrid = new Grid { Margin = new Thickness(0, 1, 0, 0), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };

        //    bool AddToRemindersListView = false;

        //    switch (ListViewToPopulate)
        //    {
        //        case ListViews.Reminder:
        //            AddToRemindersListView = true;
        //            break;
        //        case ListViews.Log:
        //            AddToRemindersListView = false;
        //            break;
        //    }

        //    bool SkipReminderListIfBlue = false;

        //    int TotalDays = Convert.ToInt32((tdt.Date - DateTime.Today).Days);

        //    if (AddToRemindersListView)
        //    {
        //        if ((DateTime.Today + ts) > tdt.Date)
        //        {
        //            Color IconEllipeColor = Colors.Pink;

        //            //Console.Write("Total Days = ");
        //            //Console.WriteLine(TotalDays);

        //            switch (TotalDays)
        //            {
        //                case int n when (n <= 0):
        //                    IconEllipeColor = (Color)ColorConverter.ConvertFromString("#FFe60000");
        //                    break;
        //                case int n when (n > 0 && n <= 3):
        //                    IconEllipeColor = (Color)ColorConverter.ConvertFromString("#FFff7800");
        //                    break;
        //                case int n when (n > 3 && n <= 7):
        //                    IconEllipeColor = (Color)ColorConverter.ConvertFromString("#FF4cbb17");
        //                    break;
        //                case int n when (n > 7 && n <= 14):
        //                    IconEllipeColor = (Color)ColorConverter.ConvertFromString("#FF9fee79");
        //                    break;
        //                case int n when (n > 14 && n <= 28):
        //                    IconEllipeColor = (Color)ColorConverter.ConvertFromString("#FFcff6bb");
        //                    break;
        //                case int n when (n > 28):
        //                    IconEllipeColor = (Color)ColorConverter.ConvertFromString("#FFe7fadd");
        //                    break;
        //            }

        //            IconEllipse = new Ellipse { Width = 24, Height = 24, Fill = new SolidColorBrush(IconEllipeColor), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };
        //            SkipReminderListIfBlue = false;
        //        }
        //        else
        //        {
        //            IconEllipse = new Ellipse { Width = 24, Height = 24, Fill = new SolidColorBrush(Colors.DodgerBlue), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };
        //            SkipReminderListIfBlue = true;
        //        }
        //    }
        //    else
        //    {
        //        Color IconEllipeColor = Colors.Pink;

        //        //Console.Write("Total Days = ");
        //        //Console.WriteLine(TotalDays);

        //        switch (TotalDays)
        //        {
        //            case int n when (n <= 0):
        //                IconEllipeColor = (Color)ColorConverter.ConvertFromString("#FFe60000");
        //                break;
        //            case int n when (n > 0 && n <= 3):
        //                IconEllipeColor = (Color)ColorConverter.ConvertFromString("#FFff7800");
        //                break;
        //            case int n when (n > 3 && n <= 7):
        //                IconEllipeColor = (Color)ColorConverter.ConvertFromString("#FF4cbb17");
        //                break;
        //            case int n when (n > 7 && n <= 14):
        //                IconEllipeColor = (Color)ColorConverter.ConvertFromString("#FF9fee79");
        //                break;
        //            case int n when (n > 14 && n <= 28):
        //                IconEllipeColor = (Color)ColorConverter.ConvertFromString("#FFcff6bb");
        //                break;
        //            case int n when (n > 28):
        //                IconEllipeColor = (Color)ColorConverter.ConvertFromString("#FFe7fadd");
        //                break;
        //        }

        //        IconEllipse = new Ellipse { Width = 24, Height = 24, Fill = new SolidColorBrush(IconEllipeColor), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };

        //        SkipReminderListIfBlue = false;
        //    }

        //    String TotalDaysString = string.Empty;
        //    if (TotalDays < 0)
        //    {
        //        if (TotalDays < -30)
        //            TotalDaysString = "30";
        //        else
        //            TotalDaysString = Math.Abs(TotalDays).ToString();
        //    }
        //    else
        //    {
        //        if (TotalDays > 30)
        //            TotalDaysString = "30";
        //        else
        //            TotalDaysString = TotalDays.ToString();
        //    }

        //    IconEllipseGrid.Children.Add(IconEllipse);
        //    Label IconEllipseLabel = new Label { Content = TotalDaysString, Foreground = Brushes.Black, FontSize = 10, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };
        //    IconEllipseGrid.Children.Add(IconEllipseLabel);

        //    IconEllipseGrid.Effect = new DropShadowEffect
        //    {
        //        Color = new Color { A = 255, R = 0, G = 0, B = 0 },
        //        Direction = 320,
        //        ShadowDepth = 1,
        //        Opacity = 0.6
        //    };

        //    //switch (ListViewToPopulate)
        //    //{
        //    //    case ListViews.Reminder:
        //    //        if (!SkipReminderListIfBlue)
        //    //        {
        //    //            if (AddToRemindersListView)
        //    //            {
        //    //                MainWindow.mw.ReminderListView.Items.Add(sp);

        //    //                MainWindow.mw.ReminderListView.Items.MoveCurrentToNext();
        //    //                StackPanel rlvisp = (StackPanel)MainWindow.mw.ReminderListView.Items.CurrentItem;
        //    //                rlvisp.Tag = ListViewItemTag;
        //    //            }
        //    //        }
        //    //        break;
        //    //    case ListViews.Log:
        //    //        MainWindow.mw.LogListView.Items.Add(sp);

        //    //        MainWindow.mw.LogListView.Items.MoveCurrentToNext();
        //    //        StackPanel llvsp = (StackPanel)MainWindow.mw.LogListView.Items.CurrentItem;
        //    //        llvsp.Tag = ListViewItemTag;
        //    //        break;
        //    //}

        //    TextBlock CreatedDateTimeStringTextBlock = new TextBlock { Text = CreatedDateTimeString, Foreground = new SolidColorBrush(Colors.Black), FontSize = 18, Margin = new Thickness(10, 0, 0, 0) };
        //    TextBlock ListViewTypeTextBlock = new TextBlock { Text = EventTypeString, Foreground = new SolidColorBrush(Colors.Black), FontSize = 18, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(10, 0, 0, 0), MinWidth = 130, MaxWidth = 130 };

        //    Border b = new Border { Width = 28, Height = 28, Background = new SolidColorBrush(Colors.PaleGoldenrod), BorderThickness = new Thickness(0), BorderBrush = new SolidColorBrush(Colors.White), CornerRadius = new CornerRadius(3), Margin = new Thickness(0, 0, 2, 0), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top, Padding = new Thickness(0) };

        //    FontAwesome.WPF.FontAwesome fai = new FontAwesome.WPF.FontAwesome();
        //    fai.Icon = EventTypeIcon;
        //    fai.FontSize = 17;
        //    fai.Foreground = new SolidColorBrush(Colors.White);
        //    fai.Margin = new Thickness(0, 5, 0, 0);
        //    b.Background = new SolidColorBrush(EventIconColor);
        //    b.Child = fai;

        //    b.Effect = new DropShadowEffect
        //    {
        //        Color = new Color { A = 255, R = 0, G = 0, B = 0 },
        //        Direction = 320,
        //        ShadowDepth = 1,
        //        Opacity = 0.6
        //    };

        //    TextBlock SourceIDStringTextBlock = new TextBlock { Text = SourceIDString, Foreground = new SolidColorBrush(Colors.Black), FontSize = 18, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(14, 0, 0, 0), MinWidth = 110, MaxWidth = 110 };
        //    TextBlock DetailsStringTextBlock = new TextBlock { Text = DetailsString, Foreground = new SolidColorBrush(Colors.Black), FontSize = 18, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(10, 0, 0, 0), MinWidth = 385, MaxWidth = 385 };

        //    Grid FreqenceyGrid = new Grid { Margin = new Thickness(10, 3, 0, 0), MinWidth = 100, MaxWidth = 100, VerticalAlignment = VerticalAlignment.Top, IsEnabled = false };

        //    FreqenceyGrid.Children.Add(Frequency.GetFrequency(oe.FrequencyID));

        //    Grid StatusGrid = new Grid { Margin = new Thickness(10, 3, 0, 0), MinWidth = 97, MaxWidth = 97, VerticalAlignment = VerticalAlignment.Top, IsEnabled = false };

        //    switch (oe.StatusID)
        //    {
        //        case Statuses.Inactive:
        //            StatusGrid.Children.Add(StatusIcons.GetStatus(Statuses.Inactive));
        //            break;
        //        case Statuses.Active:
        //            StatusGrid.Children.Add(StatusIcons.GetStatus(Statuses.Active));
        //            break;
        //        case Statuses.ActiveNotified:
        //            StatusGrid.Children.Add(StatusIcons.GetStatus(Statuses.ActiveNotified));
        //            break;
        //        case Statuses.ActiveNotifiedRead:
        //            StatusGrid.Children.Add(StatusIcons.GetStatus(Statuses.ActiveNotifiedRead));
        //            break;
        //        case Statuses.ActiveNotifiedReadArchived:
        //            StatusGrid.Children.Add(StatusIcons.GetStatus(Statuses.ActiveNotifiedReadArchived));
        //            break;
        //    }

        //    Grid OriginUserIconEllipseGrid;
        //    Ellipse OriginUserIconEllipse;

        //    Color IconEllipseColor = Colors.White;

        //    IconEllipseColor = XMLReaderWriter.UsersList[MiscFunctions.GetListUserIDFromUserID(XMLReaderWriter.UsersList, UserID)].Color;

        //    if (MiscFunctions.GetListUserIDFromUserID(XMLReaderWriter.UsersList, UserID) > 0)
        //        OriginUserIconEllipse = new Ellipse { Width = 24, Height = 24, Fill = new SolidColorBrush(IconEllipseColor), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };
        //    else
        //        OriginUserIconEllipse = new Ellipse { Width = 24, Height = 24, Fill = new SolidColorBrush(IconEllipseColor), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };

        //    OriginUserIconEllipseGrid = new Grid { Margin = new Thickness(10, 1, 0, 0), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };

        //    OriginUserIconEllipseGrid.Children.Add(OriginUserIconEllipse);

        //    Label OriginUserIconEllipseLabel = new Label { Content = MiscFunctions.GetUsersInitalsFromUserID(XMLReaderWriter.UsersList, UserID), Foreground = Brushes.Black, FontSize = 10, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };

        //    OriginUserIconEllipseGrid.Children.Add(OriginUserIconEllipseLabel);

        //    OriginUserIconEllipseGrid.Effect = new DropShadowEffect
        //    {
        //        Color = new Color { A = 255, R = 0, G = 0, B = 0 },
        //        Direction = 320,
        //        ShadowDepth = 1,
        //        Opacity = 0.6
        //    };

        //    Grid UserIconEllipseGrid;
        //    Ellipse UserIconEllipse;

        //    Color TargetIconEllipseColor = Colors.White;

        //    UserIconEllipseGrid = new Grid { Margin = new Thickness(10, 1, 10, 0), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };

        //    TargetIconEllipseColor = XMLReaderWriter.UsersList[TargetUserID].Color;

        //    if (TargetUserID > 0)
        //        UserIconEllipse = new Ellipse { Width = 24, Height = 24, Fill = new SolidColorBrush(TargetIconEllipseColor), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };
        //    else
        //        UserIconEllipse = new Ellipse { Width = 24, Height = 24, Fill = new SolidColorBrush(TargetIconEllipseColor), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };


        //    UserIconEllipseGrid.Children.Add(UserIconEllipse);

        //    Label UserIconEllipseLabel;

        //    if (TargetUserID > 0)
        //        UserIconEllipseLabel = new Label { Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, TargetUserID), Foreground = Brushes.Black, FontSize = 10, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };
        //    else
        //        UserIconEllipseLabel = new Label { Content = "★", Foreground = Brushes.Black, FontSize = 14, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, -3, 0, 0), MaxHeight = 24, Padding = new Thickness(0) };

        //    UserIconEllipseGrid.Children.Add(UserIconEllipseLabel);

        //    UserIconEllipseGrid.Effect = new DropShadowEffect
        //    {
        //        Color = new Color { A = 255, R = 0, G = 0, B = 0 },
        //        Direction = 320,
        //        ShadowDepth = 1,
        //        Opacity = 0.6
        //    };

        //    TextBlock TargetDateTimeStringTextBlock = new TextBlock { Text = TargetDateTimeString, Foreground = new SolidColorBrush(Colors.Black), FontSize = 18, Margin = new Thickness(14, 0, 0, 0), MinWidth = 134, Width = 134 };


        //    Grid RepliesGrid = new Grid { HorizontalAlignment = HorizontalAlignment.Right };
        //    StackPanel RepliesStackPanel = new StackPanel();

        //    Label RepliesLabel = new Label { Content = Replies, Foreground = Brushes.White, FontSize = 10, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };

        //    RepliesGrid.Effect = new DropShadowEffect
        //    {
        //        Color = new Color { A = 255, R = 0, G = 0, B = 0 },
        //        Direction = 320,
        //        ShadowDepth = 1,
        //        Opacity = 0.6
        //    };

        //    RepliesStackPanel.Children.Add(RepliesLabel);

        //    Button RepliesButton = new Button();

        //    RepliesButton.Name = "RepliesBotton" + ListViewItemTag;
        //    RepliesButton.Tag = ListViewItemTag;

        //    RepliesButton.Focusable = false;
        //    RepliesButton.BorderThickness = new System.Windows.Thickness(0);

        //    RepliesButton.SetResourceReference(Control.StyleProperty, "RepliesButtonStyle");

        //    RepliesButton.Content = RepliesStackPanel;



        //    RepliesGrid.Children.Add(RepliesButton);

        //    if (Replies == 0)
        //    {
        //        RepliesGrid.Opacity = 0.1;
        //        RepliesButton.IsEnabled = false;
        //    }
        //    else
        //    {
        //        ListView RepliesListView = new ListView();
        //        RepliesListView.Name = "RepliesListView" + ListViewItemTag;
        //        RepliesListView.Tag = ListViewItemTag;
        //        RepliesListView.Visibility = Visibility.Collapsed;

        //        RepliesListView.ItemsSource = MainWindow.mw.GetReplies(ListViewItemTag).ToList();

        //        RepliesButton.Click += (sender, e) => { MainWindow.mw.Replies_ButtonClick(sender, e, RepliesListView); };

        //        switch (ListViewToPopulate)
        //        {
        //            case ListViews.Reminder:
        //                if (!SkipReminderListIfBlue)
        //                {
        //                    if (AddToRemindersListView)
        //                    {
        //                        MainWindow.mw.ReminderListView.Items.Add(sp);

        //                        MainWindow.mw.ReminderListView.Items.MoveCurrentToNext();
        //                        StackPanel rlvisp = (StackPanel)MainWindow.mw.ReminderListView.Items.CurrentItem;
        //                        rlvisp.Tag = ListViewItemTag;
        //                        sp.Children.Add(rlvisp);
        //                        //sp.Tag = ListViewItemTag;
        //                    }
        //                }
        //                break;
        //            case ListViews.Log:
        //                MainWindow.mw.LogListView.Items.Add(sp);

        //                MainWindow.mw.LogListView.Items.MoveCurrentToNext();
        //                StackPanel llvsp = (StackPanel)MainWindow.mw.LogListView.Items.CurrentItem;
        //                llvsp.Tag = ListViewItemTag;
        //                sp.Children.Add(llvsp);
        //                //sp.Tag = ListViewItemTag;
        //                break;
        //        }
        //    }

        //    sp.Children.Add(b);
        //    sp.Children.Add(ListViewTypeTextBlock);
        //    sp.Children.Add(CreatedDateTimeStringTextBlock);
        //    sp.Children.Add(SourceIDStringTextBlock);
        //    sp.Children.Add(DetailsStringTextBlock);
        //    sp.Children.Add(FreqenceyGrid);
        //    sp.Children.Add(StatusGrid);
        //    sp.Children.Add(OriginUserIconEllipseGrid);
        //    sp.Children.Add(UserIconEllipseGrid);
        //    sp.Children.Add(IconEllipseGrid);
        //    sp.Children.Add(TargetDateTimeStringTextBlock);
        //    sp.Children.Add(RepliesGrid);

        //    return sp;
        //}
    }
}
