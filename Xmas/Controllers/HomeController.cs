using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Xmas.Areas.Membre.Models;
using Xmas.DAL.Models;
using Xmas.DAL.Repositories;
using Xmas.Models;
using Xmas.Tools;
using Xmas.Tools.Mappers;

namespace Xmas.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

  
     [HttpGet] 
        public ActionResult Login()
        {
            if (SessionUtils.IsConnected)
            {
                return RedirectToAction("Index", new { controller = "Home", area = "Membre" });
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel m)
        {
            //I intanciate a MembreRepository with the connectionstring.
            // The connectionstring is stored into web.config and i get it with the ConfigurationManager
            // If you want more information about ConfigurationManager don't forget to use F12.

            MembreRepository Mr = new MembreRepository(ConfigurationManager.ConnectionStrings["CnstrDev"].ConnectionString);
           
            //I use the function VerifLogin from the MembreRepository
            //For use this function, I have to convert The LoginModel to the
            ProfileModel Mmodel = MapToDBModel.MemberToProfile(Mr.VerifLogin(MapToDBModel.LoginToMembre(m)));
            if (Mmodel != null)
            {
                SessionUtils.ConnectedUser =Mmodel;
                SessionUtils.IsConnected = true;
                return RedirectToAction("Index", new { controller = "Home", area = "Membre" });
            }
            else
            {
                ViewBag.ErrorLoginMessage = "Erreur Login/Mot de passe";
                return View();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ViewResult Register(RegisterModel Rm, HttpPostedFileBase ProfilePicture)
        {

            //HttpPostedFileBase - Use to retrieve picture uploaded from form
            //We have to verify the mime type and the image size
            List<string> matchContentType = new List<string>() { "image/jpeg", "image/png", "image/gif" };
            if (!matchContentType.Contains(ProfilePicture.ContentType) || ProfilePicture.ContentLength>80000)
            {
                ViewBag.ErrorMessage = "Le fichier ne possède pas une extension autorisée (png, jpg,gif)";
                return View("Login");
            }

            //We can't save the file before to save the member in the database

            //Check if data annotations are respected ==> See the RegisterModel Class
            if (!ModelState.IsValid)
            {
                //I want to get all error on the model like wrong email format, wrong password repetiton,....
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        //add the error message into a viewbag to display on the view
                        ViewBag.ErrorMessage += error.ErrorMessage + "<br>";
                    }
                }
            }
            else
            {
                MembreRepository Mr = new MembreRepository(ConfigurationManager.ConnectionStrings["CnstrDev"].ConnectionString);
                //I have to call the Insert function from MembreRepository
                //If the insert succeed, we get a complete Member with id value (calculated by the database)
                //If the insert failed, we receive a null value
                //We have to convert the registerModel(viewmodel) to a MembreModel(Dal) before to call the function
                // this is why we call the static function RegisterToMembre from the Static lass MapToDBModel
                Membre M = Mr.Insert(MapToDBModel.RegisterToMembre(Rm));
                if ( M!= null)
                {
                    //Now I can save the picture
                    //1 - Get the filename and extract the extension
                    string[] splitFileName = ProfilePicture.FileName.Split(new char[]{'.'}, StringSplitOptions.RemoveEmptyEntries);
                    string ext = splitFileName[splitFileName.Length - 1]; //Get the last collumn of the array which contains the extension of the picture

                    //2- Generate the new file name
                    string newFileName = M.Id + "." + ext;

                    //3- Save the picture
                    //3.1 - Get the physic path of the photos folder
                    string folderpath =  Server.MapPath("~/photos/");
                    //3.2 - Combine folder path and new filename
                    string FileNameToSave = folderpath + "/" + newFileName;
                    //3.3 - Save

                    try
                    {
                        //SaveAs is a procedure and not a function thus we have to surround with try catch to
                        // get error if the SaveAs failed
                        ProfilePicture.SaveAs(FileNameToSave);
                    }
                    catch (Exception)
                    {
                        ViewBag.ErrorMessage = "L'image n'a pas pu être sauvée";
                        throw;
                    }



                    //I want to pre-fill the login html input if the register succeed.
                    //Thus , I use ViewBag to store the Email and a success message to communicate with the guest
                    ViewBag.Login = Rm.Email;
                    ViewBag.SuccessMessage = "Vous pouvez vous connecter";
                }
                else
                {
                    //If there is an issuer, I want to dispay a message on the view.
                    //Thus I use Viewbag to send the message to the view
                    ViewBag.ErrorMessage = "Erreur lors de l'insertion";
                }

            }

            return View("Login");

        }


        public RedirectToRouteResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index");
        }

    }
}