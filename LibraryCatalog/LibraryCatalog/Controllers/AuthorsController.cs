using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using LibraryCatalog.Models;
using Newtonsoft.Json;

namespace LibraryCatalog.Controllers
{
    public class AuthorsController : Controller
    {
        private const string APP_PATH = "http://localhost:60727/";

        private HttpWebRequest request;

        private HttpWebResponse response;

        private List<Author> authors;

        public ActionResult Index()
        {
            string responseString = GetResponseString(APP_PATH + "api/Authors/", "GET");
            authors = JsonConvert.DeserializeObject<Author[]>(responseString).ToList();
            return View(authors);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string responseString = GetResponseString(APP_PATH + "api/Authors/" + id, "GET");
            Author author = JsonConvert.DeserializeObject<Author>(responseString);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "Id,Surname,Name,Patronymic")] Author author)
        {
            if (ModelState.IsValid)
            {
                GetResponseString(APP_PATH + "api/Authors/", "POST", JsonConvert.SerializeObject(author));
                return RedirectToAction("Index");
            }
            return View(author);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string responseString = GetResponseString(APP_PATH + "api/Authors/" + id, "GET");
            Author author = JsonConvert.DeserializeObject<Author>(responseString);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,Surname,Name,Patronymic")] Author author)
        {
            if (ModelState.IsValid)
            {
                GetResponseString(APP_PATH + "api/Authors/" + author.Id, "PUT", JsonConvert.SerializeObject(author));
                return RedirectToAction("Index");
            }
            return View(author);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string responseString = GetResponseString(APP_PATH + "api/Authors/" + id, "GET");
            Author author = JsonConvert.DeserializeObject<Author>(responseString);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            GetResponseString(APP_PATH + "api/Authors/" + id, "DELETE");
            return RedirectToAction("Index");
        }

        private string GetResponseString(string requestUriString, string method, string body = null)
        {
            request = GetRequest(requestUriString, method, body);
            response = (HttpWebResponse)request.GetResponse();
            return new StreamReader(response.GetResponseStream()).ReadToEnd();
        }

        private HttpWebRequest GetRequest(string requestUriString, string method, string body = null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUriString);
            request.ContentType = "application/json; charset=utf-8";
            request.Method = method;
            if (body != null)
            {
                using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(body);
                    streamWriter.Flush();
                }
            }
            return request;
        }
    }
}