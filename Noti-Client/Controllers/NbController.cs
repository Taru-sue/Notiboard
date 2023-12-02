using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Noti_Client.Models;
using System.Text;

namespace Noti_Client.Controllers
{
    public class NbController : Controller
    {
        Uri baseaddress = new Uri("https://localhost:7233/api/BD");
        private readonly HttpClient _httpClient;
        public NbController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseaddress;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Board> BdList = new List<Board>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress).Result;
            if (response.IsSuccessStatusCode)
            {
                string Data = response.Content.ReadAsStringAsync().Result;
                BdList = JsonConvert.DeserializeObject<List<Board>>(Data);


            }
            return View(BdList);
        }
        [HttpGet]
        public IActionResult Create()
        {
            HttpResponseMessage res = _httpClient.GetAsync(baseaddress + "/GetGroup").Result;
            var DropLst = res.Content.ReadAsStringAsync().Result;
            List<Group> GList = JsonConvert.DeserializeObject<List<Group>>(DropLst);
            ViewModel model = new ViewModel
            {
                GroupList = GList.Select(D => new SelectListItem
                {
                    Text = D.Name,
                    Value = D.ID.ToString(),

                }).ToList(),
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(ViewModel Model)
        {
            try
            {
                var board = new Board
                {
                   ID= Model.ID,
                   Posted = DateTime.Now,
                   Description = Model.Description,
                   Information = Model.Information,
                   GroupID = Model.SelectedId,
                   UserName = Model.UserName,

                };
               
                String Data = JsonConvert.SerializeObject(board);
                StringContent Content = new StringContent(Data, Encoding.UTF8, "application/json"); 
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress, Content).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");

                }

            }
            catch(Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }
            return View(Model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Board board = new Board();
            HttpResponseMessage response = _httpClient.GetAsync($"{_httpClient.BaseAddress}/{id}").Result;
            if (response.IsSuccessStatusCode)
            {

                string data = response.Content.ReadAsStringAsync().Result;
               board = JsonConvert.DeserializeObject<Board>(data);
            }
            return View(board);
        }

        [HttpPost]
        public IActionResult Edit(Board board)
        {
             try
            {
              string data = JsonConvert.SerializeObject(board);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PutAsync($"{_httpClient.BaseAddress}/{board.ID}", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }

            return View(board);
        }

        [HttpGet]
        public IActionResult Delete() { return View(); }
     
        [HttpPost]
        public IActionResult Delete(int id)
        {
            HttpResponseMessage responce = _httpClient.DeleteAsync($"{_httpClient.BaseAddress}/{id}").Result;

            if (responce.IsSuccessStatusCode)
            {


                TempData["successMessage"] = "Employee deleted successfully";
            }
            else
            {
                TempData["errorMessage"] = "Failed to delete employee";
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult GetGroup(int id)
        {


            List<Group> List = new List<Group>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress+ "/GetGroup").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                List = JsonConvert.DeserializeObject<List<Group>>(data);
            }
            return View(List);

        }
        [HttpGet]
        public IActionResult CreateGroup()
        {

            return View();
        }

        [HttpPost]
        public IActionResult CreateGroup(Group group)
        {
            String Data = JsonConvert.SerializeObject(group);
            StringContent content = new StringContent(Data, Encoding.UTF8, "application/json");
            HttpResponseMessage res = _httpClient.PostAsync(_httpClient.BaseAddress + "/PostGroup", content).Result;
            if (res.IsSuccessStatusCode)
            {
                return RedirectToAction("GetGroup");
            }
            return View();
        }


        [HttpGet]
        public IActionResult DeleteGroup() { return View(); }

        [HttpPost]
        public IActionResult DeleteGroup(int id)
        {
            HttpResponseMessage responce = _httpClient.DeleteAsync($"{_httpClient.BaseAddress+ "/DeleteGroup"}/{id}").Result;

            if (responce.IsSuccessStatusCode)
            {


                TempData["successMessage"] = "Deleted successfully";
            }
            else
            {
                TempData["errorMessage"] = "Failed to delete";
            }
            return RedirectToAction("GetGroup");
        }

        [HttpGet]
        public IActionResult ByGroup(int groupId)
        {
            List<Board> boardsInGroup = new List<Board>();
            HttpResponseMessage response = _httpClient.GetAsync($"{_httpClient.BaseAddress}/GetByGroup/{groupId}").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                boardsInGroup = JsonConvert.DeserializeObject<List<Board>>(data);
            }

            ViewBag.GroupId = groupId; // Pass the groupId to the view

            return View(boardsInGroup);
        }
    }

 }

