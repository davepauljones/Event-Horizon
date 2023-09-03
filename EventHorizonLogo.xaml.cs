using System.Windows.Controls;
using System.Windows.Navigation;
using System.Diagnostics;

namespace The_Oracle
{
    /// <summary>
    /// Interaction logic for EventHorizonLogo.xaml
    /// </summary>
    public partial class EventHorizonLogo : UserControl
    {
        public EventHorizonLogo()
        {
            InitializeComponent();
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