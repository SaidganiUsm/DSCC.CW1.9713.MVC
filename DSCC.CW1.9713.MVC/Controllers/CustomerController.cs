using DSCC.CW1._9713.MVC.Models;
using DSCC.CW1._9713.MVC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace DSCC.CW1._9713.MVC.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IConfiguration _configuration;

        public CustomerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        string BaseUrl = "https://localhost:7097/";

        // GET: CustomerController
        public async Task<ActionResult> Index()
        {
            List<Customer> _customer = new List<Customer>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync("api/Customer/GetAllCustomers");

                if (Res.IsSuccessStatusCode)
                {
                    var PrResponse = await Res.Content.ReadAsStringAsync();

                    _customer = JsonConvert.DeserializeObject<List<Customer>>(PrResponse);
                }
            }
            return View(_customer);
        }

        // GET: CustomerController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var customer = new Customer();

            using var client = new HttpClient();
            client.BaseAddress = new Uri(BaseUrl);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.GetAsync($"api/Customer/GetCustomer/{id}");
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                customer = JsonConvert.DeserializeObject<Customer>(responseContent);
            }

            return View(customer);
        }

        // GET: CustomerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Customer viewModel)
        {
            var customer = new Customer
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Email = viewModel.Email,
                City = viewModel.City,
                Country = viewModel.Country,
                Phone = viewModel.Phone,
                OrderId = viewModel.OrderId,
                Order = new Order
                {
                    Id = 0,
                    Name = "string",
                    Amount = 0,
                    TotalPrice = 0
                }
            };

            using var client = new HttpClient();
            client.BaseAddress = new Uri(BaseUrl);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var customerJson = JsonConvert.SerializeObject(customer);
            var content = new StringContent(customerJson, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/Customer/CreateCustomer", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index"); // Redirect to the customer list or another appropriate action
            }
            else
            {
                // Handle the error, possibly by displaying an error message or returning an error view
                return View("Error");
            }

            return View(viewModel);
        }

        // GET: CustomerController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CustomerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Customer viewModel)
        {
            var customer = new Customer
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Email = viewModel.Email,
                City = viewModel.City,
                Country = viewModel.Country,
                Phone = viewModel.Phone,
                OrderId = viewModel.OrderId,
                Order = new Order
                {
                    Id = 0,
                    Name = "string",
                    Amount = 0,
                    TotalPrice = 0
                }
            };

            using var client = new HttpClient();
            client.BaseAddress = new Uri(BaseUrl);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Serialize the modified customer object to JSON and send it in the request body
            var customerJson = JsonConvert.SerializeObject(customer);
            var content = new StringContent(customerJson, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"api/Customer/UpdateCustomer/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index"); // Redirect to the customer list or another appropriate action
            }

            // Handle the case where the update failed or ModelState is not valid
            return View(customer);
        }

        // GET: CustomerController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CustomerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(BaseUrl);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.DeleteAsync($"api/Customer/DeleteCustomer/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index"); // Redirect to the order list or another appropriate action
            }
            else
            {
                return View("Error");
            }
        }
    }
}
