using ExcelDataReader;
using CineplexxAdminApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace CineplexxAdminApp.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ImportUsers(IFormFile file)
        {
            string path=$"{Directory.GetCurrentDirectory()}\\files\\{file.FileName}";
            using (FileStream fileStream =new FileStream(path, FileMode.Create)) { 
                file.CopyTo(fileStream);
                fileStream.Flush();
            }
            List<User> users = GetAllUsersFromFile(file.FileName);
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            HttpClient client = new HttpClient();
            string url = "https://localhost:44334/api/Admin/ImportAllUsers";
            HttpContent content= new StringContent(JsonConvert.SerializeObject(users),Encoding.UTF8, "application/json");
            HttpResponseMessage response=client.PostAsync(url, content).Result;
            if (!response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                return RedirectToAction("Index", "Order");
            }
            else
            {
                var errorContent = response.Content.ReadAsStringAsync().Result;
                ViewBag.ErrorMessage = "An error occurred while importing users.";
                ViewBag.StatusCode = response.StatusCode;
                ViewBag.ReasonPhrase = response.ReasonPhrase;
                ViewBag.ErrorContent = errorContent;
                return View("Error");
            }
        }

        private List<User> GetAllUsersFromFile(string fileName)
        {
            List<User> users=new List<User>();
            string path = $"{Directory.GetCurrentDirectory()}\\files\\{fileName}";

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using (var stream = new  FileStream(path, FileMode.Open))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    
                        while (reader.Read())
                        {
                            string email = reader.GetString(0);
                            string password = reader.GetString(1);
                            string confrimPassword = reader.GetString(1);
                            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(confrimPassword))
                            {
                                users.Add(new User()
                                {
                                    Email = email,
                                    Password = password,
                                    ConfrimPassword = confrimPassword,
                                });
                            }
                          
                        }
                    
                   
                    
                }
            }
            return users;
        }
    }
}
