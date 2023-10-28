using DSCC.CW1._9713.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace DSCC.CW1._9713.MVC.Controllers
{
    public class OrderController : Controller
    {
        string BaseUrl = "https://localhost:7097/";

        // GET: OrderController
        public async Task<ActionResult> Index()
        {
            List<Order> _orders = new List<Order>();

            using (var client =  new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync("api/Order/GetAllOrders");

                if (Res.IsSuccessStatusCode)
                {
                    var PrResponse = await Res.Content.ReadAsStringAsync();

                    _orders = JsonConvert.DeserializeObject<List<Order>>(PrResponse);
                }
            }
            return View(_orders);
        }

        // GET: OrderController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var order = new Order();

            using var client = new HttpClient();
            client.BaseAddress = new Uri(BaseUrl);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.GetAsync($"api/Order/GetOrder/{id}");
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                order = JsonConvert.DeserializeObject<Order>(responseContent);
            }

            return View(order);
        }

        // GET: OrderController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id, Name, Amount, TotalPrice")] Order order)
        {
            if (ModelState.IsValid)
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Serialize the order object to JSON and send it in the request body
                var orderJson = JsonConvert.SerializeObject(order);
                var content = new StringContent(orderJson, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("api/Order/CreateOrder", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index"); // Redirect to the order list or another appropriate action
                }
                else
                {
                    // Handle the error, possibly by displaying an error message or returning an error view
                    return View("Error");
                }
            }

            return View(order);
        }

        // GET: OrderController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OrderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Order order)
        {
            if (ModelState.IsValid)
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Serialize the modified order object to JSON and send it in the request body
                var orderJson = JsonConvert.SerializeObject(order);
                var content = new StringContent(orderJson, Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"api/Order/UpdateOrder/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index"); // Redirect to the order list or another appropriate action
                }
            }

            // Handle the case where the update failed or ModelState is not valid
            return View(order);
        }

        // GET: OrderController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OrderController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(BaseUrl);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.DeleteAsync($"api/Order/DeleteOrder/{id}");

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
