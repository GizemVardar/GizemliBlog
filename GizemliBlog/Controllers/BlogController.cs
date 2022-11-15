using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GizemliBlog.Models;
using PagedList;
using PagedList.Mvc;

namespace GizemliBlog.Controllers
{
    public class BlogController : Controller
    {
        BlogDBEntities db = new BlogDBEntities();
        // GET: Blog
        public ActionResult Index(int page=1)
        {
            var blog = db.Blogs.OrderByDescending(b=>b.BlogID).ToPagedList(page,3);

            return View(blog);
        }
        public ActionResult Hakkinda()
        {
            var hakkinda = db.Hakkindas.ToList();
            return View(hakkinda); 
        }
        public ActionResult BlogDetay(int id)
        {
            var blogDetay = db.Blogs.Where(b => b.BlogID == id).SingleOrDefault();
            if (blogDetay==null)
            {
                return HttpNotFound();
            }
            return View(blogDetay);
        }
        public ActionResult OkunmaArttir(int blogid)
        {
            var blog = db.Blogs.Where(b => b.BlogID == blogid).SingleOrDefault();
            blog.BlogOkunmaSayisi = blog.BlogOkunmaSayisi + 1;
            db.SaveChanges();
            return View();
        }
    }
}