﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace The_Oracle
{
    /// <summary>
    /// Interaction logic for EventRowStatusBar.xaml
    /// </summary>
    public partial class EventRowStatusBar : UserControl
    {
        EventHorizonLINQ eventHorizonLINQ;

        public EventRowStatusBar(EventHorizonLINQ eventHorizonLINQ)
        {
            InitializeComponent();

            this.eventHorizonLINQ = eventHorizonLINQ;
        }

        private void RepliesButton_Click(object sender, RoutedEventArgs e)
        {
            //if (RepliesListView.Visibility == Visibility.Visible)
            //{
            //    BackgroundGrid.Background = new SolidColorBrush(Colors.White);
            //    RepliesListView.Visibility = Visibility.Collapsed;
            //}
            //else
            //{
            //    BackgroundGrid.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f7f8f9"));
            //    RepliesListView.Visibility = Visibility.Visible;
            //}
        }

        private void RepliesListView_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is EventRow))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            EventRow item = (EventRow)dep;

            MainWindow.mw.eventHorizonLINQ = (EventHorizonLINQ)item.Tag;

            Console.WriteLine();
            Console.WriteLine(">S>>EventRow RepliesListView_PreviewMouseDoubleClick<<<<");
            Console.WriteLine();

            Console.Write("item.Tag MainWindow.mw.eventHorizonLINQ.Source_Mode = ");
            Console.WriteLine(MainWindow.mw.eventHorizonLINQ.Source_Mode);

            Console.Write("item.Tag MainWindow.mw.eventHorizonLINQ.ID = ");
            Console.WriteLine(MainWindow.mw.eventHorizonLINQ.ID);

            Console.Write("item.Tag MainWindow.mw.eventHorizonLINQ.ParentEventID = ");
            Console.WriteLine(MainWindow.mw.eventHorizonLINQ.Source_ParentEventID);

            Console.Write("item.Tag MainWindow.mw.eventHorizonLINQ.Details = ");
            Console.WriteLine(MainWindow.mw.eventHorizonLINQ.Details);

            Console.WriteLine();
            Console.WriteLine(">F>>EventRow RepliesListView_PreviewMouseDoubleClick<<<<");
            Console.WriteLine();

            //MiscFunctions.ConsoleWriteEventHorizonLINQ(eventHorizonLINQ);

            if (MainWindow.mw.eventHorizonLINQ != null)
            {
                if (MainWindow.mw.eventHorizonLINQ.EventModeID == EventModes.NoteEvent)
                {
                    EventWindow eventWindow = new EventWindow(MainWindow.mw, EventWindowModes.ViewNote, MainWindow.mw.eventHorizonLINQ);
                    eventWindow.Show();
                }
                else if (MainWindow.mw.eventHorizonLINQ.EventModeID == EventModes.ReplyEvent)
                {
                    EventWindow eventWindow = new EventWindow(MainWindow.mw, EventWindowModes.ViewReply, MainWindow.mw.eventHorizonLINQ);
                    eventWindow.Show();
                }
            }

            //prevents parent firing
            e.Handled = true;
        }
    }
}