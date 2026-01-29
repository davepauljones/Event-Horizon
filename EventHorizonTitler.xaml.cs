using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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

        private string[] _infoFiles;
        private string[] _quizFiles;
        private int _currentInfoIndex = 0;
        private int _currentQuizIndex = 0;

        private List<bool> _quizResults = new List<bool>();

        private enum AppState { Info, Quiz, Certificate }
        private AppState _currentState = AppState.Info;

        public EventHorizonTitler()
        {
            InitializeComponent();
            SetMainWindowTitle();

            _infoFiles = new string[]
            {
                "N://EventHorizonRemoteDatabase/Online Staff training/FireSafety_NG1.xml",
                "N://EventHorizonRemoteDatabase/Online Staff training/FireSafety_NG2.xml",
                "N://EventHorizonRemoteDatabase/Online Staff training/FireSafety_NG1.xml"
            };

            _quizFiles = new string[]
            {
                "N://EventHorizonRemoteDatabase/Online Staff training/FireSafety_NG1_Quiz.xml"
            };

            LoadCurrentInfoPage();
        }

        private void SetMainWindowTitle()
        {
            Title = $"Event Horizon - User {XMLReaderWriter.UserID} {XMLReaderWriter.UserNameString} - Health & Safety - Staff Information";
        }

        #region Info Pages

        private void LoadCurrentInfoPage()
        {
            _currentState = AppState.Info;
            if (_currentInfoIndex >= _infoFiles.Length)
            {
                _currentQuizIndex = 0;
                LoadCurrentQuizPage();
                return;
            }

            string xmlPath = _infoFiles[_currentInfoIndex];
            if (!System.IO.File.Exists(xmlPath))
            {
                MessageBox.Show($"Missing content file:\n{xmlPath}");
                return;
            }

            var content = ContentLoader.Load(xmlPath);
            string bodyText = string.Join("\n\n", content.Body.Paragraphs);

            ShowTitle(content.Title, bodyText);
            OptionsPanel.Visibility = Visibility.Collapsed;
            NextButton.IsEnabled = false;
        }

        #endregion

        #region Quiz Pages

        private void LoadCurrentQuizPage()
        {
            _currentState = AppState.Quiz;
            if (_currentQuizIndex >= _quizFiles.Length)
            {
                ShowCertificate();
                return;
            }

            string xmlPath = _quizFiles[_currentQuizIndex];
            if (!System.IO.File.Exists(xmlPath))
            {
                MessageBox.Show($"Missing quiz file:\n{xmlPath}");
                return;
            }

            var content = ContentLoader.Load(xmlPath);
            string bodyText = string.Join("\n\n", content.Body.Paragraphs);

            ShowTitle(content.Title, bodyText);
            NextButton.IsEnabled = false;

            if (content.Question != null && content.Question.Options.Count > 0)
            {
                OptionsPanel.Children.Clear();
                OptionsPanel.Visibility = Visibility.Collapsed;

                _timer.Tick += (s, e) =>
                {
                    if (_currentIndex >= _fullText.Length)
                    {
                        _timer.Stop();
                        ShowOptions(content.Question.Options);
                    }
                };
            }
        }

        private void ShowOptions(List<Option> options)
        {
            OptionsPanel.Children.Clear();
            OptionsPanel.Visibility = Visibility.Visible;

            foreach (var opt in options)
            {
                var btn = new Button
                {
                    Content = opt.Text,
                    FontSize = 28,
                    Margin = new Thickness(0, 10, 0, 10),
                    Tag = opt.Correct
                };
                btn.Click += OptionButton_Click;
                OptionsPanel.Children.Add(btn);
            }
        }

        private void OptionButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            bool correct = (bool)btn.Tag;
            _quizResults.Add(correct);

            MessageBox.Show(correct ? "Correct!" : "Incorrect.");

            foreach (Button b in OptionsPanel.Children)
                b.IsEnabled = false;

            NextButton.IsEnabled = true;
        }

        #endregion

        #region Certificate

        private void ShowCertificate()
        {
            _currentState = AppState.Certificate;
            OptionsPanel.Visibility = Visibility.Collapsed;
            NextButton.IsEnabled = false;

            int score = _quizResults.Count(x => x);
            int total = _quizResults.Count;

            string bodyText = $"Congratulations {XMLReaderWriter.UserNameString}!\n\n" +
                              $"You completed the training.\nScore: {score} / {total} ({(int)((double)score / total * 100)}%)";

            ShowTitle("Certificate of Completion", bodyText);

            var printBtn = new Button
            {
                Content = "Print Certificate",
                FontSize = 28,
                Margin = new Thickness(0, 20, 0, 0)
            };
            printBtn.Click += (s, e) => PrintCertificate();
            OptionsPanel.Children.Clear();
            OptionsPanel.Children.Add(printBtn);
            OptionsPanel.Visibility = Visibility.Visible;
        }

        private void PrintCertificate()
        {
            PrintDialog pd = new PrintDialog();
            if (pd.ShowDialog() == true)
            {
                pd.PrintVisual(this, "Staff Training Certificate");
            }
        }

        #endregion

        #region Typewriter & Sound

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

            _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(35) };
            _timer.Tick += TypeNextCharacter;
            _timer.Start();
        }

        private void TypeNextCharacter(object sender, EventArgs e)
        {
            if (_currentIndex >= _fullText.Length)
            {
                _timer.Stop();
                if (_currentState == AppState.Info)
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

        #endregion

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

        private void NextButton_Button_Click(object sender, RoutedEventArgs e)
        {
            switch (_currentState)
            {
                case AppState.Info:
                    _currentInfoIndex++;
                    LoadCurrentInfoPage();
                    break;
                case AppState.Quiz:
                    _currentQuizIndex++;
                    LoadCurrentQuizPage();
                    break;
                case AppState.Certificate:
                    Close();
                    break;
            }
        }
    }
}
