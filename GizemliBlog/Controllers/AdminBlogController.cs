using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GizemliBlog.Models;
using System.Web.Helpers;
using System.IO;

namespace GizemliBlog.Controllers
{
    public class AdminBlogController : Controller
    {
        BlogDBEntities db = new BlogDBEntities();
        // GET: AdminBlog
        public ActionResult Index()
        {
            var blog = db.Blogs.ToList();
            return View(blog);
        }

        // GET: AdminBlog/Details/5
        public ActionResult Details(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog==null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // GET: AdminBlog/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminBlog/Create
        [HttpPost]
        public ActionResult Create(Blog blog,HttpPostedFileBase Foto)
        {
            try
            {
                if (Foto!=null)
                {
                    WebImage webImage = new WebImage(Foto.InputStream);
                    FileInfo fileInfo = new FileInfo(Foto.FileName);
                    string newfoto = Guid.NewGuid().ToString() + fileInfo.Extension;
                    webImage.Resize(800, 350);
                    webImage.Save("~/Uploads/"+newfoto);
                    blog.Foto = "/Uploads/" + newfoto;
                }
                blog.BlogOkunmaSayisi = 0;
                blog.BlogTarih = DateTime.Now;
                db.Blogs.Add(blog);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminBlog/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var blog = db.Blogs.Where(b => b.BlogID == id).SingleOrDefault();
            if (blog==null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // POST: AdminBlog/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, HttpPostedFileBase Foto, Blog blog)
        {
            try
            {
                var blogGuncelle = db.Blogs.Where(b => b.BlogID == id).SingleOrDefault();
                if (Foto!=null)
                {
                    if (System.IO.File.Exists(Server.MapPath(blogGuncelle.Foto)))
                    {
                        System.IO.File.Exists(Server.MapPath(blogGuncelle.Foto));
                    }
                    WebImage webImage = new WebImage(Foto.InputStream);
                    FileInfo fileInfo = new FileInfo(Foto.FileName);
                    string newfoto = Guid.NewGuid().ToString() + fileInfo.Extension;
                    webImage.Resize(800, 350);
                    webImage.Save("~/Uploads/" + newfoto);
                    blogGuncelle.Foto = "/Uploads/" + newfoto;
                    blogGuncelle.BlogBaslik = blog.BlogBaslik;
                    blogGuncelle.BlogIcerik = blog.BlogIcerik;
                    blogGuncelle.BlogOkunmaSayisi = blog.BlogOkunmaSayisi;
                    blogGuncelle.BlogOkunmaSuresi = blog.BlogOkunmaSuresi;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View();
                
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            var blog=db.Blogs.Where(b=>b.BlogID == id).SingleOrDefault();
            if (blog==null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // POST: AdminBlog/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var blog = db.Blogs.Where(b => b.BlogID == id).SingleOrDefault();  
            try
            {
                
                if (blog==null)
                {
                    return HttpNotFound();
                }
                if (System.IO.File.Exists(Server.MapPath(blog.Foto)))
                {
                    System.IO.File.Exists(Server.MapPath(blog.Foto));
                }
                db.Blogs.Remove(blog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
