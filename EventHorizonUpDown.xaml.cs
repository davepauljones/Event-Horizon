using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Event_Horizon
{
    /// <summary>
    /// Interaction logic for EventHorizonUpDown.xaml
    /// </summary>
    public partial class EventHorizonUpDown : UserControl
    {
        // Define a property to set a value from the parent control
        public int UpDownValue
        {
            get { return (int)GetValue(UpDownValueProperty); }
            set
            {
                SetValue(UpDownValueProperty, value);
                UpDownValueTextBox.Text = value.ToString();
            }
        }
        // Define a property to set a value from the parent control
        public int UpDownMin
        {
            get { return (int)GetValue(UpDownMinProperty); }
            set { SetValue(UpDownMinProperty, value); }
        }
        // Define a property to set a value from the parent control
        public int UpDownMax
        {
            get { return (int)GetValue(UpDownMaxProperty); }
            set { SetValue(UpDownMaxProperty, value); }
        }
        // Define a property to set a value from the parent control
        public int UpDownStep
        {
            get { return (int)GetValue(UpDownStepProperty); }
            set { SetValue(UpDownStepProperty, value); }
        }
        // Define a property to set a value from the parent control
        public string TitleLabelString
        {
            get { return (string)GetValue(TitleLabelStringProperty); }
            set
            {
                SetValue(TitleLabelStringProperty, value);
                TitleLabel.Content = value;
            }
        }

        public static readonly DependencyProperty UpDownValueProperty =
            DependencyProperty.Register("UpDownValue", typeof(int), typeof(EventHorizonUpDown), new PropertyMetadata(0, new PropertyChangedCallback(OnValueChanged)));

        public static readonly DependencyProperty UpDownMinProperty =
            DependencyProperty.Register("UpDownMin", typeof(int), typeof(EventHorizonUpDown), new PropertyMetadata(0));

        public static readonly DependencyProperty UpDownMaxProperty =
            DependencyProperty.Register("UpDownMax", typeof(int), typeof(EventHorizonUpDown), new PropertyMetadata(0));
        
        public static readonly DependencyProperty UpDownStepProperty =
            DependencyProperty.Register("UpDownStep", typeof(int), typeof(EventHorizonUpDown), new PropertyMetadata(0));

        public static readonly DependencyProperty TitleLabelStringProperty =
            DependencyProperty.Register("TitleLabelString", typeof(string), typeof(EventHorizonUpDown), new PropertyMetadata(string.Empty));

        public delegate void UpDownCallbackDelegate(int value);

        UpDownCallbackDelegate Callback;

        public EventHorizonUpDown(string heading, int valueToUpDown, int valueToUpDownMin, int valueToUpDownMax, int valueToUpDownStep, UpDownCallbackDelegate callback)
        {
            InitializeComponent();

            this.TitleLabelString = heading;
            this.UpDownValue = valueToUpDown;
            this.UpDownMin = valueToUpDownMin;
            this.UpDownMax = valueToUpDownMax;
            this.UpDownStep = valueToUpDownStep;
            this.Callback = callback;

            Init_RowLimitRowStepControls();
        }

        public void Init_RowLimitRowStepControls()
        {
            TitleLabel.Content = TitleLabelString;
            UpDownValue = UpDownMin;
            Background = new SolidColorBrush(Colors.Transparent);
            DownButton.Foreground = new SolidColorBrush(Colors.Firebrick);
            DownButton.IsEnabled = false;
        }

        public void IsControlEnabled(bool isEnabled)
        {
            if (isEnabled)
            {
                UpDownGrid.IsEnabled = true;
                UpDownGrid.Opacity = 1;
                TitleLabel.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
            {
                UpDownGrid.IsEnabled = false;
                UpDownGrid.Opacity = 0.7;
                TitleLabel.Foreground = new SolidColorBrush(Colors.LightGray);
            }
        }

        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            EventHorizonUpDown control = obj as EventHorizonUpDown;
            control.UpDownValue = (int)e.NewValue;
        }

        public void UpButton_Click(object sender, RoutedEventArgs e)
        {
            if (UpDownValue <= UpDownMax - UpDownStep * 2)
            {
                UpDownValue += UpDownStep;
                LimitValueChanged();
            }
            else
            {
                UpDownValue = UpDownMax;
                UpButton.Foreground = new SolidColorBrush(Colors.Firebrick);
                UpButton.IsEnabled = false;
                LimitValueChanged();
            }

            if (UpDownValue >= UpDownMin + UpDownStep && UpDownValue <= UpDownMax - UpDownStep)
            {
                DownButton.Foreground = new SolidColorBrush(Colors.White);
                DownButton.IsEnabled = true;
            }
        }

        public void DownButton_Click(object sender, RoutedEventArgs e)
        {
            if (UpDownValue >= UpDownMin + UpDownStep * 2)
            {
                UpDownValue -= UpDownStep;
                LimitValueChanged();
            }
            else
            {
                UpDownValue = UpDownMin;
                DownButton.Foreground = new SolidColorBrush(Colors.Firebrick);
                DownButton.IsEnabled = false;
                LimitValueChanged();
            }

            if (UpDownValue <= UpDownMax - UpDownStep)
            {
                UpButton.Foreground = new SolidColorBrush(Colors.White);
                UpButton.IsEnabled = true;
            }
        }

        public void LimitTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (UpDownValueTextBox == null)
            {
                return;
            }
            int UpDownValue;
            if (!int.TryParse(UpDownValueTextBox.Text, out UpDownValue))
            {
                UpDownValueTextBox.Text = UpDownValue.ToString();
                Callback(UpDownValue);
            }
        }

        public void LimitValueChanged()
        {
            Console.Write("UpDownValue = ");
            Console.WriteLine(UpDownValue);
            Callback(UpDownValue);
        }

    }
}
