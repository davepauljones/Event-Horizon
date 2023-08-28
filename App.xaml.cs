using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using System.Threading;
using System.Diagnostics;

namespace The_Oracle
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //MainWindow main;
        private const int MINIMUM_SPLASH_TIME = 1500; // Miliseconds
        private const int SPLASH_FADE_TIME = 500;     // Miliseconds
        protected override void OnStartup(StartupEventArgs e)
        {
            // Step 1 - Load the splash screen
            SplashScreen splash = new SplashScreen("EventHorizonLogoNewSmall.png");
            splash.Show(false, true);

            // Step 2 - Start a stop watch
            Stopwatch timer = new Stopwatch();
            timer.Start();

            // Step 3 - Load your windows but don't show it yet
            base.OnStartup(e);
            //main = new MainWindow();

            // Step 4 - Make sure that the splash screen lasts at least two seconds
            timer.Stop();
            int remainingTimeToShowSplash = MINIMUM_SPLASH_TIME - (int)timer.ElapsedMilliseconds;
            if (remainingTimeToShowSplash > 0)
                Thread.Sleep(remainingTimeToShowSplash);

            // Step 5 - show the page
            splash.Close(TimeSpan.FromMilliseconds(SPLASH_FADE_TIME));

            //Correct display issues between windows versions
            //Uri uri = new Uri("PresentationFramework.Aero;V3.0.0.0;31bf3856ad364e35;component\\themes/aero.normalcolor.xaml", UriKind.Relative);
            //Resources.MergedDictionaries.Add(Application.LoadComponent(uri) as ResourceDictionary);

            //main.Show();
        }
    }
}
