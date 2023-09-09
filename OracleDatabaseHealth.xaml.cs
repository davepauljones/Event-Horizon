using System;
using System.Windows.Controls;
using System.IO;

namespace The_Oracle
{
    /// <summary>
    /// Interaction logic for OracleDatabaseHealth.xaml
    /// </summary>
    public partial class OracleDatabaseHealth : UserControl
    {
        public OracleDatabaseHealth()
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
    }
}