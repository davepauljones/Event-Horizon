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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Event_Horizon
{
    /// <summary>
    /// Interaction logic for LCARS_TopHeader.xaml
    /// </summary>
    public partial class LCARS_TopHeader : UserControl
    {
        private int _LimitValue = DataTableManagement.RowLimitMin;

        public LCARS_TopHeader()
        {
            InitializeComponent();

            Init_RowLimitRowStepControls();
        }

        public void Init_RowLimitRowStepControls()
        {
            LimitValue = DataTableManagement.RowLimitMin;
            LimitDown.Opacity = DataTableManagement.RowLimitRowStepControlsDisabledOpacity;
            LimitDown.IsEnabled = false;
        }

        public int LimitValue
        {
            get { return _LimitValue; }
            set
            {
                _LimitValue = value;
                LimitTextBox.Text = value.ToString();
            }
        }

        public void LimitUp_Click(object sender, RoutedEventArgs e)
        {
            if (LimitValue <= DataTableManagement.RowLimitMax - DataTableManagement.RowLimitStep * 2)
            {
                LimitValue += DataTableManagement.RowLimitStep;
                LimitValueChanged();
            }
            else
            {
                LimitValue = DataTableManagement.RowLimitMax;
                LimitUp.Opacity = DataTableManagement.RowLimitRowStepControlsDisabledOpacity;
                LimitUp.IsEnabled = false;
                LimitValueChanged();
            }

            if (DataTableManagement.RowLimit >= DataTableManagement.RowLimitMin + DataTableManagement.RowLimitStep && DataTableManagement.RowLimit <= DataTableManagement.RowLimitMax - DataTableManagement.RowLimitStep)
            {
                LimitDown.Opacity = 1;
                LimitDown.IsEnabled = true;
            }
        }

        public void LimitDown_Click(object sender, RoutedEventArgs e)
        {
            if (LimitValue >= DataTableManagement.RowLimitMin + DataTableManagement.RowLimitStep * 2)
            {
                LimitValue -= DataTableManagement.RowLimitStep;
                LimitValueChanged();
            }
            else
            {
                LimitValue = DataTableManagement.RowLimitMin;
                LimitDown.Opacity = DataTableManagement.RowLimitRowStepControlsDisabledOpacity;
                LimitDown.IsEnabled = false;
                LimitValueChanged();
            }

            if (DataTableManagement.RowLimit <= DataTableManagement.RowLimitMax - DataTableManagement.RowLimitStep)
            {
                LimitUp.Opacity = 1;
                LimitUp.IsEnabled = true;
            }
        }

        public void LimitTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (LimitTextBox == null)
            {
                return;
            }

            if (!int.TryParse(LimitTextBox.Text, out _LimitValue))
            {
                LimitTextBox.Text = _LimitValue.ToString();
            }
        }

        public void LimitValueChanged()
        {
            DataTableManagement.RowLimit = _LimitValue;
            Console.Write("DataTableManagement.RowLimit = ");
            Console.WriteLine(DataTableManagement.RowLimit);

            //if (MainWindowIs_Loaded)
            //{
            //    if (DisplayMode == DisplayModes.Reminders)
            //        RefreshLog(ListViews.Reminder);
            //    else
            //        RefreshLog(ListViews.Log);
            //}
        }

    }
}
