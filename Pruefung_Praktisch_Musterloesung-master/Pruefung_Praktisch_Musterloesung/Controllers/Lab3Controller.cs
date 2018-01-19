using System;
using System.Web.Mvc;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using Pruefung_Praktisch_Musterloesung.Models;

namespace Pruefung_Praktisch_Musterloesung.Controllers
{
    public class Lab3Controller : Controller
    {

        /**
        * 
        * ANTWORTEN:
        * XSS Attacke
        * Kommentar kann JavaScript Code enthalten, welcher beim nächsten Öffnen ausgeführt wird.
        * http://localhost:50374/Lab3/comment?comment="\"document.reload();\""
        *
        * SQL Injections
        * In den Eingabefeldern werden Erweiterungen zur SQL Abfrage,
        * welche sich im hintergrund abspielt, eingegeben. Somit kann
        * sich der Hacker ohne Username oder Passwort einloggen.
        * zB: Username: [Bob OR 1=1]      Password: [" or ""="]
        *
        * 
        * */

        public ActionResult Index() {

            Lab3Postcomments model = new Lab3Postcomments();

            return View(model.getAllData());
        }

        public ActionResult Backend()
        {
            return View();
        }

        [ValidateInput(false)] // -> we allow that html-tags are submitted!
        [HttpPost]
        public ActionResult Comment()
        {
            var comment = Request["comment"];
            var postid = Int32.Parse(Request["postid"]);

            Lab3Postcomments model = new Lab3Postcomments();

            comment = model.escapeComment(comment);

            if (model.storeComment(postid, comment))
            {  
                return RedirectToAction("Index", "Lab3");
            }
            else
            {
                ViewBag.message = "Failed to Store Comment";
                return View();
            }
        }

        [HttpPost]
        public ActionResult Login()
        {
            var username = Request["username"];
            var password = Request["password"];

            Lab3User model = new Lab3User();

            username = model.escapeCredentials(username);
            password = model.escapeCredentials(password)

            if (model.checkCredentials(username, password))
            {
                return RedirectToAction("Backend", "Lab3");
            }
            else
            {
                ViewBag.message = "Wrong Credentials";
                return View();
            }
        }
    }
}