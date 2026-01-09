using System;
using System.IO;
using System.Windows;

namespace EventHorizon.Training
{
    public partial class VideoPlayerWindow : Window
    {
        public VideoPlayerWindow()
        {
            InitializeComponent();
            Loaded += VideoPlayerWindow_Loaded;
        }

        private void VideoPlayerWindow_Loaded(object sender, RoutedEventArgs e)
        {
            string videoPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "TrainingContent",
                "NG1",
                "UPAS_KP_V2_LOWVOL.mp4");

            TrainingVideo.Source = new Uri(videoPath, UriKind.Absolute);
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            TrainingVideo.Play();
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            TrainingVideo.Pause();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            TrainingVideo.Stop();
        }
    }
}

