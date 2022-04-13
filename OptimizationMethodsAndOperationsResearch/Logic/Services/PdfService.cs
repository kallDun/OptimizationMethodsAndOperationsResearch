using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Xps.Packaging;

namespace OptimizationMethodsAndOperationsResearch.Logic.Services
{
    public static class PdfService
    {
        public static void SaveToPdf(IEnumerable<Page> pages)
        {
            var dialog = new SaveFileDialog();
            dialog.AddExtension = true;
            dialog.DefaultExt = "pdf";
            dialog.Filter = "PDF Document (*.pdf)|*.pdf";
            if (dialog.ShowDialog() == false) return;

            List<KeyValuePair<Page, Panel>> pages_contents = new();
            List<KeyValuePair<Grid, Panel>> grids_contents = new();

            FixedDocument fixedDoc = new();
            foreach (var page in pages)
            {
                PageContent pageContent = new();
                FixedPage fixedPage = new();
                var panel = page.Content as Panel;
                page.Content = null;

                //page.Content = null;
                var size_grid = new Grid()
                {
                    Width = 800,
                    Height = 450
                };
                size_grid.Children.Add(panel);
                fixedPage.Children.Add(size_grid);
                
                pages_contents.Add(new KeyValuePair<Page, Panel>(page, panel));
                grids_contents.Add(new KeyValuePair<Grid, Panel>(size_grid, panel));

                ((IAddChild)pageContent).AddChild(fixedPage);
                fixedDoc.Pages.Add(pageContent);
            }

            // write to PDF file
            string tempFilename = "temp.xps";
            File.Delete(tempFilename);
            XpsDocument xpsd = new(tempFilename, FileAccess.ReadWrite);
            System.Windows.Xps.XpsDocumentWriter xw = XpsDocument.CreateXpsDocumentWriter(xpsd);
            xw.Write(fixedDoc);
            xpsd.Close();
            PdfSharp.Xps.XpsConverter.Convert(tempFilename, dialog.FileName, 1);

            foreach (var item in grids_contents) item.Key.Children.Clear();
            foreach (var item in pages_contents) item.Key.Content = item.Value;
        }
    }
}