using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.Media;

namespace Event_Horizon
{
    public partial class EventHorizonTitler : Window
    {
        private DispatcherTimer _timer;
        private string _fullText;
        private int _currentIndex;
        private MediaPlayer _transitionSound = new MediaPlayer();

        public EventHorizonTitler()
        {
            InitializeComponent();

            SetMainWindowTitle();

            // Load content from XML
            ShowTitleFromXml("N://EventHorizonRemoteDatabase/Online Staff training/FireSafety_NG1.xml");
        }

        public void SetMainWindowTitle()
        {
            string TitleString = "Event Horizon";
            TitleString += " - User ";
            TitleString += XMLReaderWriter.UserID + " ";
            TitleString += XMLReaderWriter.UserNameString;
            TitleString += " - Health & Safety - Staff Information";

            Title = TitleString;
        }

        private void ShowTitle(string title, string body)
        {
            TitleText.Text = title;
            BodyText.Text = "";

            PlayTransitionSound();
            StartTypewriter(body);
        }

        private void ShowTitleFromXml(string xmlPath)
        {
            if (!System.IO.File.Exists(xmlPath))
            {
                MessageBox.Show($"Missing content file:\n{xmlPath}");
                return;
            }

            var content = ContentLoader.Load(xmlPath);

            string bodyText = string.Join("\n\n", content.Body.Paragraphs);
            ShowTitle(content.Title, bodyText);
        }

        private void StartTypewriter(string text)
        {
            _fullText = text;
            _currentIndex = 0;
            BodyText.Text = "";

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(35) // speed
            };
            _timer.Tick += TypeNextCharacter;
            _timer.Start();
        }

        private void TypeNextCharacter(object sender, EventArgs e)
        {
            if (_currentIndex >= _fullText.Length)
            {
                _timer.Stop();
                NextButton.IsEnabled = true;
                return;
            }

            BodyText.Text += _fullText[_currentIndex];
            _currentIndex++;
        }

        private void PlayTransitionSound()
        {
            MiscFunctions.PlayFile(AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\whoosh.wav");
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

        private void NextButton_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}