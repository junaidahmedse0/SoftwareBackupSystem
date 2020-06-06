using System.Linq;
using System.Web.DynamicData;
using BackUpSoftware.Models;
using System.Web.Mvc;
using System.Web.Security;
using BackUpSoftware.ViewModel;

namespace BackUpSoftware.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }
        /// <summary>
        /// Login Method for checking constraints of Login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(MemberShip model)
        {
            using (var db=new SoftwareBackUpEntities())
            {
                bool isValid = db.Users.Any(x => x.Email == model.Email && x.Password == model.Password && x.Status==false);
                
                if (isValid)
                {
                    FormsAuthentication.SetAuthCookie(model.Email, false);
                    var user = db.Users.FirstOrDefault(x => x.Email == model.Email);
                    var role = db.UserRoles.FirstOrDefault(x => x.UserId == user.Id);
                    if (role.Role == "admin")
                    {
                        return RedirectToAction("Index", "Admin");

                    }
                    else
                    {
                        return RedirectToAction("Index", "User");

                    }
                   


                    }
                ModelState.AddModelError("", "Invalid username and password");
                return View();

                
            }
           
        }
        public ActionResult SignUp()
        {
            return View();
        }
         /// <summary>
        /// For Signup User 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>  
        [HttpPost]   
        public ActionResult SignUp(RegisterViewModel model)
        {
            User user = new User();
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.Password = model.Password;
            user.Contact = model.Contact;
            user.Country = model.Country;
            UserRole role=new UserRole();
            role.UserId = user.Id;
            role.Role = "user";
            using (var db=new SoftwareBackUpEntities())
            {

                bool IsTrue = db.Users.Any(x => x.Email == model.Email);
                if (!IsTrue)
                {
                    db.Users.Add(user);
                    db.UserRoles.Add(role);
                    db.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError("", "Email Already Exist");

                    return View();
                }
            }

            return RedirectToAction("Login");
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}