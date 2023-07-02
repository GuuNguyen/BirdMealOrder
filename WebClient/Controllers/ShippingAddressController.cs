using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using WebClient.Models;
using WebClient.ViewModels;

namespace WebClient.Controllers
{
    public class ShippingAddressController : Controller
    {
        private readonly HttpClient _client;
        private string ProviceAPIUrl = "";
        private List<City> _cityList = new List<City>();
        private List<District> _districtList = new List<District>();
        public ShippingAddressController()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            ProviceAPIUrl = "https://provinces.open-api.vn/api/?depth=3";
        }

        public async Task<IActionResult> _DeliveryModal()
        {
            HttpResponseMessage response = await _client.GetAsync(ProviceAPIUrl);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var cities = JsonSerializer.Deserialize<List<City>>(content, options);

                _cityList.AddRange(cities);
                foreach (var city in cities)
                {
                    _districtList.AddRange(city.Districts);
                }
                var model = new ShippingAddressViewModel
                {
                    Cities = _cityList,
                    SelectedCity = cities?.FirstOrDefault()?.Name
                };
                return PartialView("_DeliveryModal", model);
            }
            return View("Error");
        }

        [HttpGet]
        public JsonResult GetDistricts(string cityCodeName)
        {
            var selectedCity = _cityList.FirstOrDefault(c => c.Codename == cityCodeName);
            if (selectedCity != null)
            {
                return Json(selectedCity.Districts);
            }
            return Json(new List<District>());
        }

        [HttpGet]
        public JsonResult GetWards(string districtCodeName)
        {
            var selectedDistrict = _districtList.FirstOrDefault(d => d.Codename == districtCodeName);
            if (selectedDistrict != null)
            {
                return Json(selectedDistrict.Wards);
            }
            return Json(new List<Ward>());
        }
    }
}
