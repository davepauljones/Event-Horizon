using System.Windows.Controls;
using System.Windows.Navigation;
using System.Diagnostics;

namespace Event_Horizon
{
    /// <summary>
    /// Interaction logic for EventHorizonLogo.xaml
    /// </summary>
    public partial class EventHorizonLogo : UserControl
    {
        public EventHorizonLogo()
        {
            InitializeComponent();

            LicenceLabel.Content = "David Paul Jones GPL V3 ( 2023 - " + System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString("yyyy") + " )";
            string swv = System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString("dd/MM/yyyy HH:mm:ss");
            BuildTextBlock.Text = "Nightly Build " + swv;
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            // for .NET Core you need to add UseShellExecute = true
            // see https://learn.microsoft.com/dotnet/api/system.diagnostics.processstartinfo.useshellexecute#property-value
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}