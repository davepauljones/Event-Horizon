using System;
using System.Windows.Controls;
using System.IO;
using System.Windows.Media;

namespace The_Oracle
{
    /// <summary>
    /// Interaction logic for EventHorizonDatabaseHealth.xaml
    /// </summary>
    public partial class EventHorizonDatabaseHealth : UserControl
    {
        public EventHorizonDatabaseHealth()
        {
            InitializeComponent();
        }
        
        public void UpdateLastWriteDateTime(DateTime lastWriteDateTime)
        {
            string fileName = XMLReaderWriter.DatabaseLocationString + "\\EventHorizonRemoteDatabase.mdb";
            FileInfo fi = new FileInfo(fileName);

            bool exists = fi.Exists;

            if (fi.Exists)
            {
                LastWriteDateTimeLabel.Content = lastWriteDateTime.ToString("dd/MM/y HH:mm:ss");
                SizeLabel.Content = MiscFunctions.SizeSuffix(fi.Length);
                CreationTimeLabel.Content = fi.CreationTime.ToString("dd/MM/y HH:mm:ss");
            }
        }

        public void UpdateLastWriteLabel(bool highlightLabel)
        {
            if (highlightLabel)
                LastWriteLabel.Foreground = new SolidColorBrush(Colors.Firebrick);
            else
                LastWriteLabel.Foreground = new SolidColorBrush(Colors.DarkSlateGray);
        }
    }
}