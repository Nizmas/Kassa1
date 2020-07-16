using Kassa1.Models;
using Kassa1.UtilCode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Kassa1.Controllers
{
    public class FormularController : Controller
    {
        public ActionResult Index()
        {
            List<string> templatesListOut = new List<string>() { "Выберите шаблон" };
            string pathFolderTemplates = AppDomain.CurrentDomain.BaseDirectory + "Resources\\";
            List<string> templatesList = Directory.GetFiles(pathFolderTemplates).ToList<string>();

            foreach (string templates in templatesList) templatesListOut.Add(templates.Remove(0, templates.LastIndexOf("\\")+1));

            ViewBag.Templates = new SelectList(templatesListOut);

            Deleter del = new Deleter();
            del.DeleteFiles(pathFolderTemplates + "Images\\");
            del.DeleteFiles(pathFolderTemplates.Replace("Resources", "Documents"));

            ClientInfo client = new ClientInfo();
            return View(client);
        }

        [HttpPost]
        public ActionResult Create(ClientInfo client)
        {
            Random rnd = new Random();
            if (ModelState.IsValid)
            {
                string imageName = Path.GetFileName(client.Image.FileName);
                try
                {
                    client.Image.SaveAs(Server.MapPath("~/Resources/Images/" + imageName));
                }
                catch
                {
                    imageName = imageName.Replace(".", rnd.Next(0, 10).ToString() + ".");
                    client.Image.SaveAs(Server.MapPath("~/Resources/Images/" + imageName));
                }

                DateTime currentDate = DateTime.Now;
                string pathTemplate = AppDomain.CurrentDomain.BaseDirectory + "Resources\\" + client.TemplateName;
                string pathOutputFolder = AppDomain.CurrentDomain.BaseDirectory + "Documents\\";

                string templateName = client.TemplateName.Remove(client.TemplateName.IndexOf("."));
                string newFileName = templateName + " " + client.FirstName + " " + client.LastName + " от " + currentDate.ToLongDateString().Replace(".", "") + ".docx";
                string pathNewFile = pathOutputFolder + newFileName;

                Replacer repl = new Replacer();
                repl.NewDoc(pathTemplate, pathNewFile, client.LastName, client.FirstName, client.MiddleName, client.BirthDate.ToString(), client.LoanSum, imageName);

                return Content(@"<body> <script type='text/javascript'>
                         window.close();
                       </script> </body> ");
            }
            return Redirect("/");
        }
        public FileResult GetFile(string fileName)
        {
            string file_path = Server.MapPath("~/Documents/" + fileName);
            string file_type = "application/docx";
            //string file_name = pathFile.Substring(pathFile.LastIndexOf('\\') + 1);
            return File(file_path, file_type, fileName);
        }

        public FileResult PrintFile(string fileName)
        {
            string doc_path = Server.MapPath("~/Documents/" + fileName);
            Converter conv = new Converter();
            string pdf_path = conv.ConvertFile(doc_path);
            string pdf_type = "application/pdf";
            string pdf_name = pdf_path.Substring(pdf_path.LastIndexOf('\\') + 1);
            return File(pdf_path, pdf_type, pdf_name);
        }
    }
}