using HealthyChefCreationsMVC.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace HealthyChefCreationsMVC.Controllers
{
    public class ContactusController : Controller
    {
        // GET: Contactus
        [HttpGet]
        public ActionResult Index()
        {
            ContactUsViewModel contactUsViewModel = new ContactUsViewModel();

            return View(contactUsViewModel);
        }

        [HttpPost]
        public ActionResult Index(ContactUsViewModel contactUsViewModel)
        {
            if(ModelState.IsValid)
            {
                HealthyChef.Email.EmailController ec = new HealthyChef.Email.EmailController();
                bool isSuccess= ec.SendMail_ContactUs(contactUsViewModel.FirstName, contactUsViewModel.LastName, contactUsViewModel.Address, contactUsViewModel.City, contactUsViewModel.State, contactUsViewModel.PostalCode, contactUsViewModel.PhoneNumber, contactUsViewModel.EmailAddress, contactUsViewModel.QuestionsComments);
                if (isSuccess)
                {
                    return Redirect("/contact-us/thank-you.aspx");
                    //return RedirectToAction("Display", "Home");
                }
                else
                {
                    return null;
                }
            }
            return View(contactUsViewModel);
        }
    }
}