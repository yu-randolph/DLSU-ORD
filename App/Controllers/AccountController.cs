using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Models;
using System.Web.Routing;

namespace App.Controllers
{
    public class AccountController : Controller
    {
        AccountManager manager = new AccountManager();
        degreeManager degManager = new degreeManager();
        documentManager docuManager = new documentManager();
        DeliveryRateManager delivManager = new DeliveryRateManager();
        MailingInfoModel mailManager = new MailingInfoModel();

        public ActionResult Index()
        {
            // working
            // must go to home index
            return RedirectToAction("Index", "Home");
        }

        public ActionResult login()
        {
            return View();
        }

        public ActionResult register()
        {
            return View();
        }

        public ActionResult regerror()
        {
            return View();
        }

        public ActionResult error()
        {
            return View();
        }

        [HttpPost]
        public ActionResult verify(string email, string password)
        {
            ViewBag.String = "email: " + email + " password: " + password;
            

            var acc = manager.getAccount(email, password);

            // Means acc does not exist 
            if (acc == null)
            {
                return RedirectToAction("error", "Account");
            }
            else
            {
                Session["user"] = acc;
                return RedirectToAction("order", "Transaction");

            }
        }

        [HttpPost]
        public ActionResult save(string email, string altEmail, string password, 
            string lastName, string firstName, string middleName, string gender, string birthday, string citizenship, string birthPlace,
            string street, string city, string address, string country, string zipCode, string phoneNumber, string altPhoneNumber, string delivArea,
            string highSchool, string degreeLevel, string idNumber, string college, string degreeProgram, string gradRadio,
            string monthGraduate, string yearGraduate, string yrLvl, string lastTerm, string lastTermStart, string lastTermEnd,
            string admissionRadio, string admissionYear, string lastSchool, string campusAttended)
        {

            Account acc = new Account();
            MailingInformation mail = new MailingInformation();
            DeliveryRate deliv = new DeliveryRate();
            Degree deg = new Degree();

            // account
            acc.email = email;
            acc.alternateEmail = altEmail;
            acc.password = password;

            // personal
            acc.idNumber = idNumber;
            acc.lastName = lastName;
            acc.firstName = firstName;
            acc.middleName = middleName;

            if (gender == "Male")
                acc.gender = 'M';
            else acc.gender = 'F';

            string year = birthday[0] + "" + birthday[1] + "" + birthday[2] + "" + birthday[3] + "";
            string month = birthday[5] + "" + birthday[6] + "";
            string day = birthday[8] + "" + birthday[9] + "";

            acc.birthYear = Int32.Parse(year);
            acc.birthMonth = Int32.Parse(month);
            acc.birthDay = Int32.Parse(day);

            acc.citizenship = citizenship;
            acc.placeOfBirth = birthPlace;
            acc.currentAddress = address;
            acc.phoneNo = phoneNumber;
            acc.alternatePhoneNo = altPhoneNumber;
            acc.registeredDate = DateTime.Now.ToString();
            acc = manager.saveAccount(acc);

            // mailing
            deliv = delivManager.getDeliveryRate(delivArea);

            mail.zipcode = zipCode;
            mail.streetname = street;
            mail.city = city;
            mail.country = country;
            mail.locationID = deliv.locationId;
            mail.userID = acc.userID;
            mail.addressline = address;
            mail.contactPerson = firstName + " " + lastName;
            mail.contactNumber = phoneNumber;

            mailManager.addMailingInfo(mail);
            acc.mailInfos = mailManager.getMailInfos(acc.userID);

            // academic
            /*deg.degreeName = degreeProgram;
            deg.level = degreeLevel;
            deg.yearAdmitted = Int32.Parse(admissionYear);
            deg.campusAttended = campusAttended;
            deg.admittedAs = admissionRadio;
            deg.graduated = gradRadio;

            if(yrLvl != "")
            {
                deg.yearLevel = Int32.Parse(yrLvl);
            }

            
            deg.userID = acc.userID;

            if(lastSchool != "")
            {
                deg.lastSchoolAttendedPrevDlsu = lastSchool;
            }

            if(yearGraduate != "")
            {
                deg.graduatedYear = Int32.Parse(yearGraduate);
            }
            
            if(monthGraduate != "")
            {
                deg.graduatedMonth = Int32.Parse(monthGraduate);
            }
            
            if(lastTerm != "")
            {
                deg.term = Int32.Parse(lastTerm);
            }

            if(lastTermStart != "")
            {
                int start = Int32.Parse(lastTermStart);
                deg.academicYear = (start + 2000) + "-" + (start + 1 + 2000);
            }
            */
            

            //degManager.saveDegree(deg);
            //acc.degrees = degManager.getDegree(acc.userID);

            Session["user"] = acc;
            return RedirectToAction("Order", "Transaction"); // go to next step
            //return RedirectToAction("Index", "Home");
        }
    }
}