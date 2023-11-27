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

//using Word = Microsoft.Office.Interop.Word;

namespace Event_Horizon
{
    /// <summary>
    /// Interaction logic for MethodStatement.xaml
    /// </summary>
    public partial class MethodStatement : System.Windows.Window
    {
        // Create a new Word application
        //Word.Application wordApp;
        //Word.Application wordApp = new Word.Application();
        //Word.Document doc;

        public MethodStatement()
        {
            InitializeComponent();

            //Init_WordApp();
        }

        private void Init_WordApp()
        {
            // Create a new Word application
            //wordApp = new Word.Application();

            // Make Word visible (optional)
            //wordApp.Visible = true;
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            // Specify the path to the Word document
            //string filePath = @"N:\DPJ - Word Templates\Method Statement.dotm";

            // Open the Word document
            //doc = wordApp.Documents.Open(filePath, ReadOnly:true);
 
            // Set macro security to a lower level (Medium)
            //wordApp.Application.AutomationSecurity = Microsoft.Office.Core.MsoAutomationSecurity.msoAutomationSecurityLow;
        }
        
        private void PopulateButton_Click(object sender, RoutedEventArgs e)
        {
            // Perform the search and replace
            FindAndReplaceText("%JOB_NUMBER%", JobNoTextBox.Text);
            //FindAndReplaceText("%CONTRACT_TITLE%", replaceText);
            
            FindAndReplaceFooterText("%DATE%", DateDatePicker.Text);

            Console.WriteLine("Search and replace completed.");
        }
        
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            // Specify the path to the Word document
            //string saveFilePath = @"N:\DPJ - Word Templates\Method Statement22.docx";
            // Save the document with a new file name (Save As)
            //doc.SaveAs2(saveFilePath);

            // Close the document without saving changes
            //doc.Close(SaveChanges: false);
            // Quit the Word application
            //wordApp.Quit();
        }

        private void FindAndReplaceText(string searchText, string replaceText)
        {
            // Access the first section of the document (assuming a single-section document)
            //Word.Section section = doc.Sections[1];

            //section.Range.Find.ClearFormatting();
            //section.Range.Find.Execute(searchText, ReplaceWith: replaceText);
        }

        private void FindAndReplaceFooterText(string searchText, string replaceText)
        {
            // Access the primary footer range
            //Word.Range footerRange = doc.Sections[1].Footers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
            
            //footerRange.Find.ClearFormatting();
            //footerRange.Find.Execute(searchText, ReplaceWith: replaceText);
        }

    }
}
