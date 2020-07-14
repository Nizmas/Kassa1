using Microsoft.Office.Interop.Word;

namespace Kassa1.UtilCode
{
    public class Class1
    {
        public class Converter
        {
            public string ConvertFile(string pathDoc)
            {

                string parhPdf = pathDoc.Replace(".docx", ".pdf");

                Application appWord = new Application();
                var wordDocument = appWord.Documents.Open(pathDoc);
                wordDocument.ExportAsFixedFormat(parhPdf, WdExportFormat.wdExportFormatPDF);

                return parhPdf;
            }
        }
    }
}