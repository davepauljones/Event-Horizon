using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Event_Horizon
{
    /// <summary>
    /// Interaction logic for EventHorizonTitler.xaml
    /// </summary>
    public partial class EventHorizonTitler : Window
    {
        private DispatcherTimer _timer;
        private string _fullText;
        private int _currentIndex;

        private MediaPlayer _transitionSound = new MediaPlayer();

        public EventHorizonTitler()
        {
            InitializeComponent();

            // Optional sound
            _transitionSound.Open(
                new Uri("Assets/sweep.wav", UriKind.Relative)
            );

            ShowTitle(
                "Fire Safety – NG1",
                "Fire can develop rapidly and without warning.\n\n" +
                "Understanding how fires start, spread, and how to respond " +
                "is critical to protecting people, property, and the business."
            );
        }

        private void ShowTitle(string title, string body)
        {
            TitleText.Text = title;
            BodyText.Text = "";

            PlayTransitionSound();

            StartTypewriter(body);
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
                return;
            }

            BodyText.Text += _fullText[_currentIndex];
            _currentIndex++;
        }

        private void PlayTransitionSound()
        {
            _transitionSound.Stop();
            _transitionSound.Position = TimeSpan.Zero;
            _transitionSound.Play();
        }

        // ESC to exit fullscreen
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }
    }
}
