using Microsoft.AspNetCore.Mvc;
using MVC_DB_Mit_Bild.Models;
using NuGet.Packaging.Signing;
using System.Configuration;
using System.IO;

namespace MVC_DB_Mit_Bild.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAccessable dal;
        private readonly IWebHostEnvironment hostInfo;
        public HomeController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            string connString = configuration.GetConnectionString("SqlServer");
            dal = new PersonCrud(connString);
            hostInfo = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View(dal.GetAllPerson());
        }

        public IActionResult admin(int? id)
        {
            if (id != null)
            {
                int newId = (int)id;
                Person deletedPerson = dal.GetPersonById(newId);
                dal.DeletePersonById(newId);
                System.IO.DirectoryInfo di = new DirectoryInfo("YourPath");
                FileInfo fi = new FileInfo("\\images");
                string pathToFile = hostInfo.WebRootPath + "\\images\\" + deletedPerson.Bild;
                if (System.IO.File.Exists(pathToFile) && !pathToFile.Contains("keinBild"))
                {
                    System.IO.File.Delete(pathToFile);
                }
            }
            return View(dal.GetAllPerson());
        }

        [HttpGet]
        public IActionResult insert()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> insertAsync(Person person)
        {
            ModelState.Remove("Bild");
            if (!ModelState.IsValid)
            {
                return View(person);
            }

            if (person.Uploaddatei != null)
            {
                //Upload-Datei speichern
                string pfad = hostInfo.WebRootPath + "\\images";
                string guid = Guid.NewGuid().ToString();
                //string Extension = System.IO.Path.GetExtension(person.Uploaddatei.FileName);
                string NeuerName = guid + person.Uploaddatei.FileName;

                using (var stream = new FileStream(pfad + "\\" + NeuerName, FileMode.Create))
                {
                    await person.Uploaddatei.CopyToAsync(stream);
                }
                person.Bild = NeuerName;
            }
            
            dal.InsertPerson(person);
            return RedirectToAction("Index"); 
        }
        public IActionResult details(int id)
        {
            return View(dal.GetPersonById(id));
        }
        [HttpGet]
        public IActionResult edit(int id)
        {
            return View(dal.GetPersonById(id));
        }
        [HttpPost]
        public async Task<IActionResult> editAsync(Person person)
        {
            //Alte Bildname
            Person altePerson = dal.GetPersonById(person.PID);
            if (person.Uploaddatei == null)
            {
                person.Bild = altePerson.Bild;
            }
            else
            {
                //Alte Image löschen
                string pathToFile = hostInfo.WebRootPath + "\\images\\" + altePerson.Bild;
                if (System.IO.File.Exists(pathToFile) && !pathToFile.Contains("keinBild"))
                {
                    System.IO.File.Delete(pathToFile);
                }

                //Neue Image hinzufügen
                string guid = Guid.NewGuid().ToString();
                string NeuerName = guid + person.Uploaddatei.FileName;
                person.Bild = NeuerName;
                using (var stream = new FileStream(hostInfo.WebRootPath + "\\images\\" + NeuerName, FileMode.Create))
                {
                    await person.Uploaddatei.CopyToAsync(stream);
                }
            }

            dal.UpdatePerson(person);

            return RedirectToAction("Index");
        }
    }
}
