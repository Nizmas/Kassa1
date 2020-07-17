using Microsoft.Office.Interop.Word;
using System.IO;

namespace Kassa1.UtilCode
{
    public class Converter
    {
        public string ConvertFile(string pathDoc)
        {

            string pathPdf = pathDoc.Replace(".docx", ".pdf");

            Application appWord = new Application();
            var wordDocument = appWord.Documents.Open(pathDoc);
            wordDocument.ExportAsFixedFormat(pathPdf, WdExportFormat.wdExportFormatPDF);
            wordDocument.Close();
            File.Copy(pathPdf, pathPdf.Replace(".pdf", "_.pdf"), true);
            return pathPdf;
        }
    }
}