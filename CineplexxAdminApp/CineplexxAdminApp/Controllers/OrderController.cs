using ClosedXML.Excel;
using GemBox.Document;
using CineplexxAdminApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CineplexxAdminApp.Controllers
{
    public class OrderController : Controller
    {
       public OrderController() {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }

        public IActionResult Index()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            HttpClient client = new HttpClient();
                string url = "https://localhost:44334/api/Admin/GetOrders";
                HttpResponseMessage response = client.GetAsync(url).Result;
                var result = response.Content.ReadAsAsync<List<Order>>().Result;
                return View(result);
        }

        public IActionResult Details(Guid id)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            HttpClient client = new HttpClient();
            string url = "https://localhost:44334/api/Admin/GetDetailsForProduct";
            var model = new { Id = id };

            HttpContent content=new StringContent(JsonConvert.SerializeObject(model),Encoding.UTF8,"application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            var result = response.Content.ReadAsAsync<Order>().Result;

            return View(result);

        }
        [HttpGet]
        public FileContentResult ExportAllOrders() {

            string fileName = "Orders.xlsx";
            string contentType = "application/vn.openxmlformats-officedocument.spreadsheetl.sheet";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("All Orders");

                worksheet.Cell(1, 1).Value = "Нарачка ";
                worksheet.Cell(1, 2).Value = " Кориснички Е-пошта";

                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                HttpClient client = new HttpClient();
                string url = "https://localhost:44334/api/Admin/GetOrders";
                HttpResponseMessage response = client.GetAsync(url).Result;
                var result = response.Content.ReadAsAsync<List<Order>>().Result;

                for (int i = 0; i < result.Count; i++) {
                    var item = result[i];

                    worksheet.Cell(i+1, 1).Value=item.Id.ToString();
                    worksheet.Cell(i + 1, 2).Value = item.User.Email;

                    for(int j=0;j< item.ProductInOrders.Count;j++)
                    {
                        worksheet.Cell(1, j+3).Value="Филм-"+(j+1);
                        worksheet.Cell(i + 1, j + 3).Value = item.ProductInOrders.ElementAt(j).OrderedProduct.ProductName;

                    }
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content= stream.ToArray();

                    return File(content, contentType, fileName);
                }
            }
        }
        public FileContentResult CreateInvoice(Guid id)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            HttpClient client = new HttpClient();
            string url = "https://localhost:44334/api/Admin/GetDetailsForProduct";
           
            var model = new {Id= id};

            HttpContent content=new StringContent(JsonConvert.SerializeObject(model),Encoding.UTF8,"application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            var result = response.Content.ReadAsAsync<Order>().Result;

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.docx");
            var document = DocumentModel.Load(templatePath);


            document.Content.Replace("{{OrderNumber}}", result.Id.ToString());
            document.Content.Replace("{{UserName}}", result.User.UserName);

            StringBuilder sb = new StringBuilder();

            var totalPrice = 0.0;

            foreach (var item in result.ProductInOrders)
            {
                totalPrice += item.Quantity * item.OrderedProduct.Price;
                sb.AppendLine(item.OrderedProduct.ProductName + " with quantity: " + item.Quantity + " and price: " + item.OrderedProduct.Price + " den");
            }


            document.Content.Replace("{{ProductList}}", sb.ToString());
            document.Content.Replace("{{TotalPrice}}", totalPrice.ToString() + " den");


            var stream = new MemoryStream();

            document.Save(stream, new PdfSaveOptions());

            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "ExportInvoice.pdf");
        }

    }
}
