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
        public Object Create(ClientInfo client, string submit)
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
                string pathDoc = repl.NewDoc(pathTemplate, pathNewFile, client.LastName, client.FirstName, client.MiddleName, client.BirthDate.ToString(), client.LoanSum, imageName);
                string nameDoc = pathDoc.Substring(pathDoc.LastIndexOf('\\') + 1);

                Converter conv = new Converter();
                string pathPdf = conv.ConvertFile(pathDoc);
                string namePdf = pathPdf.Substring(pathPdf.LastIndexOf('\\') + 1);

                return (namePdf);
                /*switch (submit)
                {
                    case "Скачать":
                        return File(pathDoc, "application/docx", nameDoc);
                    case "Распечатать":
                        return File(pathPdf.Replace(".pdf", "_.pdf"), "application/pdf", namePdf);
                }*/
            }
            //set error status
            //Response.StatusCode = 400;
            // Important for live environment.
            //Response.TrySkipIisCustomErrors = true;

            IEnumerable<Error> modelErrors = ModelState.AllErrors(); // <<<<<<<<< SEE HERE
            return (modelErrors);
            //return Json(client.BirthDate + client.FirstName + client.LastName + client.LoanSum + client.MiddleName + client.TemplateName );
        }

        public FileResult PrintFile(string fileName)
        {
            string pdf_path = Server.MapPath("~/Documents/" + fileName);
            string pdf_type = "application/pdf";
            return File(pdf_path, pdf_type, fileName);
        }
    }





}