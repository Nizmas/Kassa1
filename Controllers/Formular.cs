using Kassa1.Models;
using Kassa1.UtilCode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Kassa1.UtilCode.Class1;

namespace Kassa1.Controllers
{
    public class FormularController : Controller
    {
        
        public ActionResult Index()
        {
            List<string> templatesListOut = new List<string>() { "Выберите шаблон" };
            List<string> foldersListOut = new List<string>() { "Выберите папку" };
            string pathFolderTemplates = AppDomain.CurrentDomain.BaseDirectory + "Resources\\";
            string pathFolderDirectories = AppDomain.CurrentDomain.BaseDirectory + "Documents\\";
            List<string> templatesList = Directory.GetFiles(pathFolderTemplates).ToList<string>();
            List<string> foldersList = Directory.GetDirectories(pathFolderDirectories).ToList<string>();

            foreach (string templates in templatesList) templatesListOut.Add(templates.Remove(0, templates.LastIndexOf("\\")+1));
            foreach (string folders in foldersList) foldersListOut.Add(folders.Remove(0, folders.LastIndexOf("\\")));

            ViewBag.Templates = new SelectList(templatesListOut); 
            ViewBag.Folders = new SelectList(foldersListOut);
            ClientInfo client = new ClientInfo();
            return View(client);
        }

        [HttpPost]
        public ActionResult Create(ClientInfo client)
        {
            if (ModelState.IsValid)
            {
                string imageName = Path.GetFileName(client.Image.FileName);
                client.Image.SaveAs(Server.MapPath("~/Resources/Images/" + imageName));

                DateTime currentDate = DateTime.Now;
                string pathTemplate = AppDomain.CurrentDomain.BaseDirectory + "Resources\\" + client.TemplateName;
                string pathOutputFolder = AppDomain.CurrentDomain.BaseDirectory + "Documents" + client.FolderName + "\\";

                string templateName = client.TemplateName.Remove(client.TemplateName.IndexOf("."));
                string newFileName = templateName + " " + client.FirstName + " " + client.LastName + " от " + currentDate.ToLongDateString().Replace(".", "") + ".docx";
                string pathNewFile = pathOutputFolder + newFileName;

                Replacer repl = new Replacer();
                string pathReturn = repl.NewDoc(pathTemplate, pathNewFile, client.LastName, client.FirstName, client.MiddleName, client.BirthDate.ToString(), client.LoanSum, imageName);
                ViewBag.Path = pathReturn/*.Replace("\\", "$")*/;
                return View();
            }
            return View("Ошбика");
        }
        public FileResult GetFile(string pathFile)
        {
            string file_path = Server.MapPath("~/" + pathFile);
            string file_type = "application/docx";
            string file_name = pathFile.Substring(pathFile.LastIndexOf('\\') + 1); ;
            return File(file_path, file_type, file_name);
        }

        public FileResult PrintFile(string pathFile)
        {
            string doc_path = Server.MapPath("~/" + pathFile);
            Converter conv = new Converter();
            string pdf_path = conv.ConvertFile(doc_path);
            string pdf_type = "application/pdf";
            string pdf_name = pdf_path.Substring(pdf_path.LastIndexOf('\\') + 1);
            return File(pdf_path, pdf_type, pdf_name);
        }
    }
}