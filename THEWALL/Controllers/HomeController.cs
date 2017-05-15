using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using THEWALL.Models;
// using DbConnection;

namespace THEWALL.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            //Creating list to make ViewBag iterable
            HttpContext.Session.SetInt32("UserId",0);
            ViewBag.regerrors = new List<Dictionary<string,object>>();
            return View();
        }

        //Logic for registration, inserts user into my database
        [HttpPost]
        [RouteAttribute("Register")]
         public IActionResult Register(User user){
            //if there are no errors
            if(ModelState.IsValid){
                //insert the user into the database
                string Querystring = $"INSERT INTO User(FirstName, LastName, Email, Password, created_at) Values('{user.FirstName}','{user.LastName}', '{user.Email}','{user.Password}', NOW())";
                DbConnector.Execute(Querystring);
                string GetUser = $"SELECT * FROM User WHERE Email = '{user.Email}'";
                Dictionary<string, object> TheUser = DbConnector.Query(GetUser).SingleOrDefault();
                HttpContext.Session.SetInt32("UserId", (int) TheUser["UserId"]);
                return RedirectToAction("MessageBoard");
            }else{
                ViewBag.regerrors = ModelState.Values;
                return View("Index");
            }
        }
        //Logic for my login page
        [HttpPost]
        [RouteAttribute("Login/{UserId}")]
        //login method passes arguments email and password, these arguments are submitted from my form
        public IActionResult Login(string Email, string Password, string UserId)
        {
            //find the user by matching the given email with an email in the database
            string QueryString2 = $"SELECT * FROM User WHERE Email = '{Email}'";
            Dictionary<string,object> theUser = DbConnector.Query(QueryString2).SingleOrDefault();
            HttpContext.Session.SetInt32("UserId", (int) theUser["UserId"]);
                if(theUser != null && Password != null){
                    if((string)theUser["Password"] == Password){
                        return RedirectToAction("MessageBoard");
                    }
                }
                ViewBag.logerror = "Invalid Combination";
                ViewBag.regerrors = new List<string>();
                return View("Index");
        }

        //This route handles the message that is inputed from the form. It stores the message in my database and later when I need that message I can always retrieve it.
        [HttpPost]
        [RouteAttribute("CreateMessage")]

        public IActionResult CreateMessage(string message){
            int? userid = HttpContext.Session.GetInt32("UserId");
            string QueryString3 = $"INSERT INTO Messages(UserId,Message,created_at) Values('{userid}', '{message}', NOW())";
            DbConnector.Execute(QueryString3);
            return RedirectToAction("MessageBoard");
        }

        [HttpPost]
        [RouteAttribute("CreateComment/{MessageId}/{UserId}")]
        
        public IActionResult CreateComment(string MessageId, string UserId, string Comment)
        {
            string QueryString5 = $"INSERT INTO Comments(MessageId,UserId,Comment,created_at) Values('{MessageId}','{UserId}','{Comment}', NOW())";
            DbConnector.Execute(QueryString5);
            return RedirectToAction("MessageBoard");
        }
        
        [HttpGet]
        [RouteAttribute("MessageBoard")]

        public IActionResult MessageBoard()
        {
            string QueryString5 = $"SELECT X.UserId, Y.Message, X.FirstName, Y.created_at, Y.MessageId FROM User X, Messages Y where X.UserId = Y.UserId";
            string QueryString6 = $"SELECT X.UserId, Y.Message, X.FirstName, Y.created_at, Y.MessageId,A.CommentId,A.Comment FROM User X, Messages Y, Comments A where X.UserId = Y.UserId and Y.MessageId = A.MessageId";
            List<Dictionary<string,object>> UserMessage = DbConnector.Query(QueryString5);
            List<Dictionary<string,object>> UserComment = DbConnector.Query(QueryString6);
            ViewBag.Message = UserMessage;
            ViewBag.Comment = UserComment;
            return View("MessageBoard");
        }

        [HttpPost]
        [RouteAttribute("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
