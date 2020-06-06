using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using BackUpSoftware.ViewModel;
using System.Runtime.InteropServices;

namespace BackUpSoftware.Controllers
{

    [Authorize]
    public class UserController : Controller
    {
  
        /// <summary>
        /// This function is used to get files and Folders of Current User
        /// 
        /// </summary>
        /// <param name="path">Pass path of folder That you Want to open</param>
        /// <returns></returns>
        public ActionResult Index(string  path)
        {
           
            Folder folder;
            string[] dirs;
            string[] files;
            string p = "";
            using (var db=new SoftwareBackUpEntities())
            {
               //Find Account of Current Login User 
                var user = db.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
                //Find Directory of Currently login User
                folder = db.Folders.FirstOrDefault(x => x.UserId == user.Id);
                //Find all folders in a main directory of a 

                if (!String.IsNullOrEmpty(path))
                {
                    p = path;
                    //dirs = Directory.GetDirectories(path, "*", SearchOption.AllDirectories);
                    dirs = Directory.GetDirectories(path);
                    //files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
                    files = Directory.GetFiles(path);

                }
                else
                {
                    p = folder.Path;
                    //dirs = Directory.GetDirectories(folder.Path, "*", SearchOption.AllDirectories);
                    dirs = Directory.GetDirectories(folder.Path);
                    //files = Directory.GetFiles(folder.Path, "*", SearchOption.AllDirectories);
                    files = Directory.GetFiles(folder.Path);

                }
                var viewModel = new FolderViewModel()
                {
                    Path = p,
                    User=user,
                    Files = files,
                    Folders = dirs



                };
                if(TempData["error"]==null)
                {
                    TempData["error"] = "";
                }
                ViewBag.ErrorMessage = TempData["error"].ToString();
                //var dirs = Directory.GetDirectories(folder.Path).ToList();
                return View(viewModel);
            }
           
        }

        /// <summary>
      /// This function is Used to Upload Files (images, text,videos)
      /// </summary>
      /// <param name="file"></param>
      /// <returns></returns>
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file, string folderPath)
        {

            using (var db=new SoftwareBackUpEntities())
            {
             
         
                string fileName = Path.GetFileName(file.FileName);

                //Combine path
                string fullPath = Path.Combine(folderPath, fileName);
              

                    file.SaveAs(fullPath);
     
                return RedirectToAction("Index",new { Path=folderPath});

            }


      
        }
        /// <summary>
        /// Function is Used to Download file of current Path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public FileResult Download(string path)
        {

            string fileName = Path.GetFileName(path);
            return File(path, "image/jpeg",fileName);


        }
        /// <summary>
        /// Function is Used to Create Folder
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public ActionResult CreateFolder(string folderName, string folderPath)
        {
            folderName = @"\" + folderName;

            using (var db=new SoftwareBackUpEntities())
            {
                //var user = db.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
                //var folder = db.Folders.FirstOrDefault(x => x.UserId == user.Id);
              
                //var path = string.Concat(folder.Path,folderName);
                var path = string.Concat(folderPath, folderName);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    Console.WriteLine("Folder Created");
                   
                }
                else
                {
                    TempData["error"] = "Folder Already Exist";
                    return RedirectToAction("Index", new { path = folderPath });
                }
            }
          
            

            return RedirectToAction("Index",new { path=folderPath });
        }
        /// <summary>
        /// Function is Used to Delte File
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public ActionResult DeleteFile(string path,string filepath)
        {
            FileInfo fi = new FileInfo(path);
            fi.Delete();
            return RedirectToAction("Index",new { path= filepath});
        }
        /// <summary>
        /// Function is Used to Delete folder
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public ActionResult DeleteFolder(string path, string folderpath)
        {
            Directory.Delete(path,true);
            return RedirectToAction("Index", new { path = folderpath });
        }

    }
}