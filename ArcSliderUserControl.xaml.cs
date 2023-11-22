using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Event_Horizon
{
    /// <summary>
    /// Interaction logic for ArcSliderUserControl.xaml
    /// </summary>
    public partial class ArcSliderUserControl : UserControl
    {
        public ArcSliderUserControl()
        {
            InitializeComponent();

            DataContext = new MainViewModel();
        }
        
    }
}
