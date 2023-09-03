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
                SizeLabel.Content = SizeSuffix(fi.Length);
                CreationTimeLabel.Content = fi.CreationTime.ToString("dd/MM/y HH:mm:ss");
            }
        }
        
        static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        static string SizeSuffix(Int64 value)
        {
            if (value < 0) { return "-" + SizeSuffix(-value); }

            int i = 0;
            decimal dValue = (decimal)value;
            while (Math.Round(dValue / 1024) >= 1)
            {
                dValue /= 1024;
                i++;
            }

            return string.Format("{0:n1} {1}", dValue, SizeSuffixes[i]);
        }
    }
}