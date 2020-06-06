using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BackUpSoftware.ViewModel;
using BackUpSoftware.Models;
using System.IO;
using System;

namespace BackUpSoftware.Controllers
{

    [Authorize]

    public class AdminController : Controller
    {
        private SoftwareBackUpEntities db = new SoftwareBackUpEntities();

        /// <summary>
        /// Showing list of Users on the side of Admin
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {

            
                        return View(db.Users.ToList());
        }
        /// <summary>
        /// Admin Block Users on the basis of users id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult BlockUser(int id)
        {

            using (var db = new SoftwareBackUpEntities())
            {

                var user = db.Users.FirstOrDefault(x => x.Id == id);
                user.Status = true;
                db.SaveChanges();
                return RedirectToAction("Index");

            }

        }
        /// <summary>
        /// Admin can unblock user on the basis of id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult UnBlock(int id)
        {

            using (var db = new SoftwareBackUpEntities())
            {

                var user = db.Users.FirstOrDefault(x => x.Id == id);
                user.Status = false;
                db.SaveChanges();
                return RedirectToAction("Index");

            }

        }
        /// <summary>
        /// Show details of Users
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
         public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return Json(new { FirstName=user.FirstName,LastName=user.LastName,Password=user.Password, Email =user.Email, Contact =user.Contact,Country=user.Country},JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Admin Can Create User 
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Save Data into db of new created user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RegisterViewModel user)
        {
            if (ModelState.IsValid)
            {
                bool IsTrue = db.Users.Any(x => x.Email == user.Email);
                if (!IsTrue)
                {
                    var p = Server.MapPath("~/Data/");
                    var path = string.Concat(p, user.Email);



                    var u = new User();
                    u.FirstName = user.FirstName;
                    u.LastName = user.LastName;
                    u.Password = user.Password;
                    u.Contact = user.Contact;
                    u.Email = user.Email;
                    u.Country = user.Country;


                    UserRole role = new UserRole();

                    Folder folder = new Folder();
                    folder.Path = path;
                    folder.UserId = u.Id;

                    role.Role = "user";
                    role.UserId = u.Id;

                    db.Users.Add(u);

                    db.Folders.Add(folder);

                    db.UserRoles.Add(role);
                    db.SaveChanges();
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                        Console.WriteLine("Folder Created");
                    }
                    else
                    {
                        Console.WriteLine("Folder  Already Exists");
                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Email Already Exist");

                    return View();
                }
            }

            return View(user);
        }
        /// <summary>
        /// Edit user details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return Json(new { FirstName=user.FirstName,LastName=user.LastName,Password=user.Password, Email =user.Email, Contact =user.Contact,Country=user.Country},JsonRequestBehavior.AllowGet);
            
        }
        /// <summary>
        /// Save data of admin edited data into  db...
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }
        /// <summary>
        /// Delete User and folder of user 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int id)
        {

          
            User user = db.Users.Find(id);
            var folder = db.Folders.Where(x => x.UserId == user.Id).FirstOrDefault();
            var path = folder.Path;
            Directory.Delete(path,true);
            UserRole role = db.UserRoles.Where(x => x.UserId == id).FirstOrDefault();
            db.Folders.Remove(folder);
            db.UserRoles.Remove(role);
            db.Users.Remove(user);

            db.SaveChanges();
            return Json(new { status=true},JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Disposing Objects 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
