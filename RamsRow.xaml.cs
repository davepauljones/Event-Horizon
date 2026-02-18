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
    /// Interaction logic for RamsRow.xaml
    /// </summary>
    public partial class RamsRow : UserControl
    {
        EventHorizonRamsLINQ eventHorizonRamsLINQ;

        public RamsRow(EventHorizonRamsLINQ eventHorizonRamsLINQ)
        {
            InitializeComponent();

            this.eventHorizonRamsLINQ = eventHorizonRamsLINQ;
        }

        public static RamsRow CreateRamsRow(EventHorizonRamsLINQ eventHorizonRamsLINQ)
        {
            RamsRow ramsRow = new RamsRow(eventHorizonRamsLINQ);

            ramsRow.RamsIDTextBlock.Text = eventHorizonRamsLINQ.ID.ToString("D5");

            ramsRow.CreatedDateTimeTextBlock.Text = eventHorizonRamsLINQ.CreationDate.ToString("dd/MM/y HH:mm");

            ramsRow.JobNoTextBlock.Text = eventHorizonRamsLINQ.JobNo.ToString();

            ramsRow.DescriptionTextBlock.Text = eventHorizonRamsLINQ.Description;

            if (eventHorizonRamsLINQ.RamsProfileTypeID < DataTableManagementRams.RamsProfileTypesList.Count)
            {
                ramsRow.RamsProfileTypeFontAwesomeIconBorder.Background = new SolidColorBrush(Colors.Beige);
                ramsRow.RamsProfileTypeFontAwesomeIcon.Icon = FontAwesomeIcon.Star;
                ramsRow.RamsProfileTypeTextBlock.Text = DataTableManagementRams.RamsProfileTypesList[eventHorizonRamsLINQ.RamsProfileTypeID].ProfileName;
                ramsRow.BackgroundGrid.Background = new SolidColorBrush(Colors.Transparent);
            }
            else
            {
                ramsRow.RamsProfileTypeFontAwesomeIconBorder.Background = new SolidColorBrush(Colors.White);
                ramsRow.RamsProfileTypeFontAwesomeIcon.Icon = FontAwesomeIcon.Question;
                ramsRow.RamsProfileTypeTextBlock.Text = "Error";
                ramsRow.BackgroundGrid.Background = new SolidColorBrush(Colors.Transparent);
            }


            if (eventHorizonRamsLINQ.UserID < XMLReaderWriter.UsersList.Count)
            {
                ramsRow.UserEllipse.Fill = new SolidColorBrush(XMLReaderWriter.UsersList[eventHorizonRamsLINQ.UserID].Color);
                ramsRow.UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonRamsLINQ.UserID);
            }
            else
            {
                ramsRow.UserEllipse.Fill = new SolidColorBrush(Colors.White);
                ramsRow.UserLabel.Content = eventHorizonRamsLINQ.UserID;
            }

            if (eventHorizonRamsLINQ.UserID < XMLReaderWriter.UsersList.Count)
            {
                if (eventHorizonRamsLINQ.UserID > 0)
                    ramsRow.UserLabel.Content = MiscFunctions.GetUsersInitalsFromID(XMLReaderWriter.UsersList, eventHorizonRamsLINQ.UserID);
                else
                {
                    ramsRow.UserLabel.Content = "★";
                    ramsRow.UserLabel.Margin = new Thickness(0, -3, 0, 0);
                    ramsRow.UserLabel.FontSize = 14;
                }
            }
            else
            {
                ramsRow.UserEllipse.Fill = new SolidColorBrush(Colors.White);
                ramsRow.UserLabel.Content = eventHorizonRamsLINQ.UserID;
            }


            ramsRow.Tag = eventHorizonRamsLINQ;

            return ramsRow;
        }

    }
}