using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Media;
using Microsoft.Win32;

namespace The_Oracle
{
    /// <summary>
    /// Interaction logic for OracleLogin.xaml
    /// </summary>
    public partial class OracleLogin : Window
    {
        MainWindow mw;
        Int32 SelectedUserID;

        public OracleLogin(MainWindow mw)
        {
            InitializeComponent();
            this.Hide();

            this.mw = mw;

            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            //PlayNotificationSound();
            AddItemsToSelectUserComboBox();
        }

        private void AddItemsToSelectUserComboBox()
        {
            foreach (User user in XMLReaderWriter.UsersList)
            {
                if (user.ID > 0) SelectUserComboBox.Items.Add(EventHorizonUsers.GetUserStackPanel(user));
            }
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
                        if (PasswordPasswordBox.Password == XMLReaderWriter.DefaultPasswordString)
                        {
                            DialogResult = true;
                            Close();
                        }
                        else
                        {
                            PasswordRetries--;
                            PasswordPasswordBox.Password = string.Empty;
                            PasswordPasswordBox.Focus();
                            if (PasswordRetries == 0)
                            {
                                DialogResult = false;
                                Close();
                            }
                            e.Handled = true;
                        }
                        break;
                }
            }
        }

        private void SelectUserComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedUserID = SelectUserComboBox.SelectedIndex;

            Console.Write("SelectedUserID = ");
            Console.WriteLine(SelectedUserID);

            XMLReaderWriter.ChangeCurrentUser(SelectedUserID);
        }

        internal int PasswordRetries = 3;
        private void PasswordPasswordBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (PasswordPasswordBox.Password == XMLReaderWriter.DefaultPasswordString)
                {
                    DialogResult = true;
                    Close();
                }
                else
                {
                    PasswordRetries--;
                    PasswordPasswordBox.Password = string.Empty;
                    PasswordPasswordBox.Focus();
                    if (PasswordRetries == 0)
                    {
                        DialogResult = false;
                        Close();
                    }
                    e.Handled = true;
                }
            }
        }
    }
}