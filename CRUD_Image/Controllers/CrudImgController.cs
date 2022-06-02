using CRUD_Image.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CRUD_Image.Controllers
{
    public class CrudImgController : Controller
    {
        // GET: CrudImg
        dbn2Entities db = new dbn2Entities();
        public ActionResult Index()
        {
            return View(db.tbl_img.ToList());
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(HttpPostedFileBase file, tbl_img emp)
        {
            string filename = Path.GetFileName(file.FileName);
            string _filename = DateTime.Now.ToString("yymmssfff") + filename;
            string extension = Path.GetExtension(file.FileName);
            string path = Path.Combine(Server.MapPath("~/images/"), _filename);
            emp.Productimage = "~/images/" + _filename;
            if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
            {


                if (file.ContentLength <= 1000000)
                {
                    db.tbl_img.Add(emp);
                    if (db.SaveChanges() > 0)
                    {
                        file.SaveAs(path);
                        ViewBag.msg = "Record Added";
                        ModelState.Clear();
                    }
                }
                else
                {
                    ViewBag.msg = "Size is not valid";
                }
            }
            return View();
        }
        public ActionResult Edit(int ? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            var tbl_img = db.tbl_img.Find(id);
            Session["imgPath"] = tbl_img.Productimage;
            if (tbl_img == null)
            {
                return HttpNotFound();

            }
            return View(tbl_img);
        }
        [HttpPost]
        public ActionResult Edit(HttpPostedFileBase file,tbl_img emp)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string filename = Path.GetFileName(file.FileName);
                    string _filename = DateTime.Now.ToString("yymmssfff") + filename;
                    string extension = Path.GetExtension(file.FileName);
                    string path = Path.Combine(Server.MapPath("~/images/"), _filename);
                    emp.Productimage = "~/images/" + _filename;
                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                    {


                        if (file.ContentLength <= 1000000)
                        {
                            //db.tbl_img.Add(emp);
                            db.Entry(emp).State = EntityState.Modified;
                            string oldImgPath = Request.MapPath(Session["imgPath"].ToString());
                            if (db.SaveChanges() > 0)
                            {
                                file.SaveAs(path);
                                TempData["msg"] = "Record updated";
                                //ViewBag.msg = "Record Added";
                                //ModelState.Clear();
                            }
                        }
                        else
                        {
                            ViewBag.msg = "Size is not valid";
                        }
                    }
                }

            }
            else
            {
                emp.Productimage = Session["imgPath"].ToString();
                db.Entry(emp).State = EntityState.Modified;
                if (db.SaveChanges() > 0)
                {
                    TempData["img"] = "Data Updated";
                    return RedirectToAction("Index");
                }
            }
            return View();
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            var tbl_img = db.tbl_img.Find(id);
            //Session["imgPath"] = tbl_img.Productimage;
            if (tbl_img == null)
            {
                return HttpNotFound();

            }
            return View(tbl_img);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            var tbl_img = db.tbl_img.Find(id);
            //Session["imgPath"] = tbl_img.Productimage;
            if (tbl_img == null)
            {
                return HttpNotFound();

            }

            string currentime = Request.MapPath(tbl_img.Productimage);
            db.Entry(tbl_img).State = EntityState.Deleted;
            if (db.SaveChanges() > 0)
            {
                if (System.IO.File.Exists(currentime))
                {
                    System.IO.File.Delete(currentime);
                }
                TempData["msg"] = "Data Deleted!";
                return RedirectToAction("Index");
            }



            return View();
        }
    }
}