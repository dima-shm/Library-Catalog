using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using LibraryCatalog.Models;
using Newtonsoft.Json;

namespace LibraryCatalog.Controllers
{
    public class BooksController : Controller
    {
        private const string APP_PATH = "http://localhost:60727/";

        private HttpWebRequest request;

        private HttpWebResponse response;

        private List<Book> books;

        public ActionResult Index(string search)
        {
            string path = APP_PATH + "api/Books/";
            if (!string.IsNullOrEmpty(search))
            {
                path += "?search=" + search;        
            }
            string responseString = GetResponseString(path, "GET");
            books = JsonConvert.DeserializeObject<Book[]>(responseString).ToList();
            foreach (Book book in books)
            {
                book.Author = GetAuthorById(book.AuthorId);
            }
            return View(books);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string responseString = GetResponseString(APP_PATH + "api/Books/" + id, "GET");
            Book book = JsonConvert.DeserializeObject<Book>(responseString);
            responseString = GetResponseString(APP_PATH + "api/Authors/" + book.AuthorId, "GET");
            Author author = JsonConvert.DeserializeObject<Author>(responseString);
            book.Author = author;
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        public ActionResult Create()
        {
            string responseString = GetResponseString(APP_PATH + "api/Authors/", "GET");
            List<Author> authors = JsonConvert.DeserializeObject<Author[]>(responseString).ToList();
            ViewBag.Authors = authors;
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "Id,Title,AuthorId,Description,Year,NumPages,Price")] Book book)
        {
            if (ModelState.IsValid)
            {
                GetResponseString(APP_PATH + "api/Books/", "POST", JsonConvert.SerializeObject(book));
                return RedirectToAction("Index");
            }
            string responseString = GetResponseString(APP_PATH + "api/Authors/", "GET");
            List<Author> authors = JsonConvert.DeserializeObject<Author[]>(responseString).ToList();
            ViewBag.AuthorId = new SelectList(authors, "Id", "Surname", book.AuthorId);
            return View(book);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string responseString = GetResponseString(APP_PATH + "api/Books/" + id, "GET");
            Book book = JsonConvert.DeserializeObject<Book>(responseString);
            if (book == null)
            {
                return HttpNotFound();
            }
            responseString = GetResponseString(APP_PATH + "api/Authors/", "GET");
            List<Author> authors = JsonConvert.DeserializeObject<Author[]>(responseString).ToList();
            ViewBag.Authors = authors;
            return View(book);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,Title,AuthorId,Description,Year,NumPages,Price")] Book book)
        {
            if (ModelState.IsValid)
            {
                GetResponseString(APP_PATH + "api/Books/" + book.Id, "PUT", JsonConvert.SerializeObject(book));
                return RedirectToAction("Index");
            }
            string responseString = GetResponseString(APP_PATH + "api/Authors/", "GET");
            List<Author> authors = JsonConvert.DeserializeObject<Author[]>(responseString).ToList();
            ViewBag.AuthorId = new SelectList(authors, "Id", "Surname", book.AuthorId);
            return View(book);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string responseString = GetResponseString(APP_PATH + "api/Books/" + id, "GET");
            Book book = JsonConvert.DeserializeObject<Book>(responseString);
            book.Author = GetAuthorById(book.AuthorId);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            GetResponseString(APP_PATH + "api/Books/" + id, "DELETE");
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

        private Author GetAuthorById(int id)
        {
            string responseString = GetResponseString(APP_PATH + "api/Authors/" + id, "GET");
            return JsonConvert.DeserializeObject<Author>(responseString);
        }
    }
}