﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Media;
using Microsoft.Win32;

namespace The_Oracle
{
    /// <summary>
    /// Interaction logic for EventHorizonRequesterNotification.xaml
    /// </summary>
    public partial class EventHorizonRequesterNotification : Window
    {
        MainWindow mw;
        OracleCustomMessage oracleCustomMessage;
        int requesterType;
        bool overrideNotificationSound;

        public EventHorizonRequesterNotification(MainWindow mw, OracleCustomMessage oracleCustomMessage, int requesterType, bool overrideNotificationSound=true)
        {
            InitializeComponent();
            this.Hide();

            this.mw = mw;
            this.oracleCustomMessage = oracleCustomMessage;
            this.requesterType = requesterType;
            this.overrideNotificationSound = overrideNotificationSound;

            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            SetRequester();
        }

        private void SetRequester()
        {
            MessageTitleTextBlock.Text = oracleCustomMessage.MessageTitleTextBlock;
            InformationTextBlock.Text = oracleCustomMessage.InformationTextBlock;

            switch (requesterType)
            {
                case RequesterTypes.NoYes:
                    NoButton.Content = "No";
                    YesButton.Content = "Yes";
                    break;
                case RequesterTypes.OK:
                    NoButton.Content = "";
                    NoButton.Visibility = Visibility.Hidden;
                    YesButton.Content = "Ok";
                    break;
            }

            if (!overrideNotificationSound) PlayNotificationSound();
        }

        private void PlayNotificationSound()
        {
            bool found = false;
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"AppEvents\Schemes\Apps\.Default\Notification.Default\.Current"))
                {
                    if (key != null)
                    {
                        Object o = key.GetValue(null); // pass null to get (Default)
                        if (o != null)
                        {
                            SoundPlayer theSound = new SoundPlayer((String)o);
                            theSound.Play();
                            found = true;
                        }
                    }
                }
            }
            catch
            { }
            if (!found)
                SystemSounds.Beep.Play(); // consolation prize
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void TreeView_ButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            int buttonID = 255;

            bool success = Int32.TryParse(button.Tag.ToString(), out buttonID);

            if (button != null && success)
            {

                switch (buttonID)
                {
                    case 0:
                        DialogResult = false;
                        Close();
                        break;
                    case 1:
                        DialogResult = true;
                        Close();
                        break;
                }
            }
        }
    }
}
