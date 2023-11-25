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
using System.Windows.Shapes;

using uno;
using com.sun.star.lang;
using com.sun.star.frame;
using com.sun.star.text;


namespace Event_Horizon
{
    /// <summary>
    /// Interaction logic for LibraOfficeUno.xaml
    /// </summary>
    public partial class LibraOfficeUno : Window
    {
        public LibraOfficeUno()
        {
            InitializeComponent();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            // Create a new instance of the LibreOffice desktop
            XComponentContext localContext = uno.util.Bootstrap.bootstrap();
            XMultiServiceFactory multiServiceFactory = (XMultiServiceFactory)localContext.getServiceManager();

            XComponentLoader componentLoader = (XComponentLoader)multiServiceFactory.createInstance("com.sun.star.frame.Desktop");

            // Create a new text document
            XComponent document = componentLoader.loadComponentFromURL("private:factory/swriter", "_blank", 0, new PropertyValue[0]);

            // Get the text interface of the document
            XTextDocument textDocument = (XTextDocument)document;

            // Insert text into the document
            XText text = textDocument.getText();
            text.setString("Hello, LibreOffice from C#!");

            // Save the document
            XStorable storable = (XStorable)document;
            PropertyValue[] storeProperties = new PropertyValue[0];
            storable.storeToURL("file:///path/to/your/document.odt", storeProperties);

            // Close the document
            XCloseable closeable = (XCloseable)document;
            closeable.close(false);

            Console.WriteLine("Document saved and closed.");
        }

        private void Replace_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
