using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DojoConnections.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Exam2.Controllers
{
    public class HomeController : Controller
    {
        // GET: /Home/
       
       private DojoConnectionsContext _context;
 
        public HomeController(DojoConnectionsContext context)
        {
            _context = context;
        }
       
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            ViewBag.regerrors = new List<string>();
            ViewBag.logerror = "";
            return View("Index");
        }

        [HttpPost]
        [Route("RegistrationLogic")]

        public IActionResult RegistrationLogic(User user, Network network)
        {
            ViewBag.regerrors = new List<string>();
            if(ModelState.IsValid){
                _context.Add(user);
                network.UserId = user.UserId;
                network.AcceptedInvite = false;
                network.IgnoredInvite = false;
                _context.Add(network);
                _context.SaveChanges();
                _context.Remove(user);
                _context.Add(network);
                _context.SaveChanges();
                user.NetworkId = network.NetworkId;
                _context.Add(user);
                _context.SaveChanges();
                User Lookup = _context.User.SingleOrDefault(x => x.Email == user.Email);
                HttpContext.Session.SetInt32("UserId",Lookup.UserId );
                return RedirectToAction("ProfilePage");
            }else{
                ViewBag.regerrors = ModelState.Values;
                return View("Index");
            } 
        }
    
        
        [HttpPost]
        [Route("LoginLogic")]
        public IActionResult LoginLogic(string Email, string Password)
        {
            User UserIdLookup = _context.User.SingleOrDefault(user => user.Email == Email);
            if(UserIdLookup!= null && Password != null){
                if(Password == UserIdLookup.Password){
                    //save id in session
                    HttpContext.Session.SetInt32("UserId",UserIdLookup.UserId);
                    return RedirectToAction("ProfilePage");
                }
            } 
            ViewBag.logerror = "Invalid Combination";
            ViewBag.regerrors = new List<string>();
            return View("Index");
        }

        [HttpGet]
        [Route("ProfilePage")]

        public IActionResult ProfilePage()
        {
            int? loggedUser = HttpContext.Session.GetInt32("UserId");
            User Lookup = _context.User.SingleOrDefault(x => x.UserId == loggedUser);
            Network Network = _context.Network.Include(i => i.Invitations).Include(u => u.Users).SingleOrDefault(n => n.UserId == loggedUser);
            List<User> AllUsers = _context.User.ToList();
            @ViewBag.User = Lookup;
            @ViewBag.Network = Network;
            @ViewBag.AllUsers = AllUsers;
            return View("ProfilePage");
        }

        [HttpPost]
        [Route("Logout")]
        public IActionResult logout()
        {
            ViewBag.regerrors = new List<string>();
            HttpContext.Session.Clear();           
            return View("Index");
        }

        [HttpGet]
        [Route("AllUsers")]

        public IActionResult AllUsers()
        {
            int? loggedUser = HttpContext.Session.GetInt32("UserId");
            Network UserNetwork = _context.Network.Include(u => u.Users).Include(i =>i.Invitations).SingleOrDefault(n => n.UserId == loggedUser);
            List<User> AllUsers = _context.User.ToList();
            User Lookup = _context.User.SingleOrDefault(x=> x.UserId == loggedUser);
            @ViewBag.UserNetwork = UserNetwork;
            @ViewBag.AllUsers = AllUsers;
            @ViewBag.LoggedUser = Lookup;
            return View("AllUsers");      
        }

        [HttpGet]
        [Route("Connect/{networkid}")]

        public IActionResult Connect(Invitation invitation,int networkid)
        {
            int? loggedUser = HttpContext.Session.GetInt32("UserId");
            invitation.UserId = (int)loggedUser;
            invitation.NetworkId = networkid;
            _context.Add(invitation);
            _context.SaveChanges();
            return RedirectToAction("ProfilePage");
        }

        [HttpGet]
        [Route("Accept/{userid}")]

        public IActionResult Accept(int userid)
        {
            int? loggedUser = HttpContext.Session.GetInt32("UserId");
            User Lookup = _context.User.SingleOrDefault(x => x.UserId == userid);
            Network UserNetwork = _context.Network.Include(i => i.Users).SingleOrDefault(n => n.UserId == loggedUser);
            Invitation Invitation = _context.Invitation.Where(u => u.UserId == userid).SingleOrDefault(n => n.NetworkId == UserNetwork.NetworkId);
            UserNetwork.Users.Add(Lookup);
            _context.SaveChanges();
            _context.Invitation.Remove(Invitation);
            _context.SaveChanges();
            return RedirectToAction("ProfilePage");
        }

        [HttpGet]
        [Route("Ignore/{userid}")]

        public IActionResult Ignore(int userid)
        {
            int? loggedUser = HttpContext.Session.GetInt32("UserId");
            Network UserNetwork = _context.Network.Include(i => i.Users).SingleOrDefault(n => n.UserId == loggedUser);
            Invitation Invitation = _context.Invitation.Where(u => u.UserId == userid).SingleOrDefault(n => n.NetworkId == UserNetwork.NetworkId);
            _context.Invitation.Remove(Invitation);
            _context.SaveChanges();
            return RedirectToAction("ProfilePage");
        }
    }
}