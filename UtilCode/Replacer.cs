using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using System.IO;
using System.Drawing;
using System;

namespace Kassa1.UtilCode
{
    public class Replacer
    {
        public string NewDoc(string pathTemplate, string pathFile, string lastName, string firstName, string middleName, string birthDate, int loanSum, string inputImage)
        {
            if (!File.Exists(pathTemplate)) return "Папка не найдена, обратитесь к администратору или выберите другую";
            string imagesFolder = AppDomain.CurrentDomain.BaseDirectory + "Resources\\Images\\";

            Document document = new Document();
            document.LoadFromFile(pathTemplate);

            TextSelection[] selections = document.FindAllString("[image]", true, true);
            int index = 0;
            TextRange range = null;

            foreach (TextSelection selection in selections)
            {
                DocPicture pic = new DocPicture(document);
                try
                {
                    pic.LoadImage(Image.FromFile(imagesFolder + inputImage));
                }
                catch
                {
                    System.Threading.Thread.Sleep(2000);
                    pic.LoadImage(Image.FromFile(imagesFolder + inputImage));
                }
                pic.Width = 250;
                pic.Height = 268;
                range = selection.GetAsOneRange();
                index = range.OwnerParagraph.ChildObjects.IndexOf(range);
                range.OwnerParagraph.ChildObjects.Insert(index, pic);
                range.OwnerParagraph.ChildObjects.Remove(range);
            }

            document.Replace("[lastName]", lastName, false, true);
            document.Replace("[firstName]", firstName, false, true);
            document.Replace("[middleName]", middleName, false, true);
            document.Replace("[birthDate]", birthDate.Remove(birthDate.IndexOf(" ")), false, true);
            document.Replace("[loanSum]", loanSum.ToString(), false, true);
            document.SaveToFile(pathFile, FileFormat.Docx);
            document.Close();

            ImgDeleter img = new ImgDeleter();
            img.Remove(imagesFolder);

            return pathFile;
        }
    }
}