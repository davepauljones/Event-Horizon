using FontAwesome.WPF;
using System;
using System.Drawing;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Event_Horizon
{
    /// <summary>
    /// Interaction logic for EngineerRow.xaml
    /// </summary>
    public partial class EngineerRow : UserControl
    {
        EventHorizonEngineerLINQ eventHorizonEngineersLINQ;

        public EngineerRow(EventHorizonEngineerLINQ eventHorizonEngineersLINQ)
        {
            InitializeComponent();

            this.eventHorizonEngineersLINQ = eventHorizonEngineersLINQ;
        }

        public static EngineerRow CreateEngineerRow(EventHorizonEngineerLINQ eventHorizonEngineersLINQ)
        {
            EngineerRow engineerRow = new EngineerRow(eventHorizonEngineersLINQ);

            engineerRow.RamsIDTextBlock.Text = eventHorizonEngineersLINQ.ID.ToString("D5");

            engineerRow.CreatedDateTimeTextBlock.Text = eventHorizonEngineersLINQ.CreationDate.ToString("dd/MM/y HH:mm");

            engineerRow.NameTextBlock.Text = eventHorizonEngineersLINQ.Name.ToString();
            engineerRow.RoleTextBlock.Text = eventHorizonEngineersLINQ.Role;
            engineerRow.CompetenceDetailsTextBlock.Text = eventHorizonEngineersLINQ.CompetenceDetails;

            engineerRow.Tag = eventHorizonEngineersLINQ;

            return engineerRow;
        }

    }
}