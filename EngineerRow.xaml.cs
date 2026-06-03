using FontAwesome.WPF;
using System;
using System.Diagnostics;
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

        EventHorizonEngineerLINQ eventHorizonEngineerLINQ;

        public EngineerRow(EventHorizonEngineerLINQ eventHorizonEngineerLINQ)

        {
            InitializeComponent();

            this.eventHorizonEngineerLINQ = eventHorizonEngineerLINQ;
        }

        public static EngineerRow CreateEngineerRow(EventHorizonEngineerLINQ eventHorizonEngineerLINQ)

        {
            EngineerRow engineerRow = new EngineerRow(eventHorizonEngineerLINQ);

            engineerRow.RamsIDTextBlock.Text = eventHorizonEngineerLINQ.ID.ToString("D5");

            engineerRow.CreatedDateTimeTextBlock.Text = eventHorizonEngineerLINQ.CreationDate.ToString("dd/MM/y HH:mm");

            if (eventHorizonEngineerLINQ.UserID < XMLReaderWriter.UsersList.Count)
            {
                engineerRow.OriginUserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonEngineerLINQ.UserID].Color);
                engineerRow.OriginUserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonEngineerLINQ.UserID);
            }
            else
            {
                engineerRow.OriginUserEllipse.Fill = new SolidColorBrush(Colors.White);
                engineerRow.OriginUserLabel.Content = eventHorizonEngineerLINQ.UserID;
            }

            engineerRow.NameTextBlock.Text = eventHorizonEngineerLINQ.Name;
            engineerRow.RoleTextBlock.Text = eventHorizonEngineerLINQ.Role;
            engineerRow.CompetenceDetailsTextBlock.Text = eventHorizonEngineerLINQ.CompetenceDetails;

            engineerRow.Tag = eventHorizonEngineerLINQ;

            return engineerRow;
        }

    }
}