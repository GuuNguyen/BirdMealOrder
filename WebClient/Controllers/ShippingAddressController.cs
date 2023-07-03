using Microsoft.AspNetCore.Mvc;
using Repositories.DTOs.ShippingAddressDTO;
using System.Net.Http.Headers;
using System.Text.Json;
using WebClient.Models;
using WebClient.ViewModels;

public class ShippingAddressController : Controller
{
    private readonly HttpClient _client;
    private string ProviceAPIUrl = "";
    private string SAAPIUrl = "";

    public ShippingAddressController()
    {
        _client = new HttpClient();
        var contentType = new MediaTypeWithQualityHeaderValue("application/json");
        _client.DefaultRequestHeaders.Accept.Add(contentType);
        ProviceAPIUrl = "https://provinces.open-api.vn/api/?depth=3";
        SAAPIUrl = "https://localhost:7022/api/ShippingAddress";
    }

    public async Task<IActionResult> _DeliveryModal()
    {
        List<City> _cityList;
        List<District> _districtList;

        // Kiểm tra xem danh sách đã được lưu trong session chưa
        if (HttpContext.Session.TryGetValue("CityList", out var cityListBytes) &&
            HttpContext.Session.TryGetValue("DistrictList", out var districtListBytes))
        {
            // Đã có danh sách trong session, sử dụng danh sách đã lưu
            _cityList = JsonSerializer.Deserialize<List<City>>(cityListBytes);
            _districtList = JsonSerializer.Deserialize<List<District>>(districtListBytes);
        }
        else
        {
            // Chưa có danh sách trong session, tải dữ liệu từ API và lưu vào session
            HttpResponseMessage response = await _client.GetAsync(ProviceAPIUrl);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var cities = JsonSerializer.Deserialize<List<City>>(content, options);

                _cityList = cities.Select(city => new City
                {
                    Codename = city.Codename,
                    Name = city.Name,
                    Districts = city.Districts.Select(district => new District
                    {
                        Codename = district.Codename,
                        Name = district.Name,
                        Wards = district.Wards.Select(ward => new Ward
                        {
                            Codename = ward.Codename,
                            Name = ward.Name
                        }).ToList()
                    }).ToList()
                }).ToList();

                _districtList = _cityList.SelectMany(city => city.Districts).ToList();

                // Lưu danh sách vào session
                HttpContext.Session.Set("CityList", JsonSerializer.SerializeToUtf8Bytes(_cityList));
                HttpContext.Session.Set("DistrictList", JsonSerializer.SerializeToUtf8Bytes(_districtList));
            }
            else
            {
                return View("Error");
            }
        }

        var model = new ShippingAddressViewModel
        {
            Cities = _cityList,
        };

        return PartialView("_DeliveryModal", model);
    }

    [HttpGet]
    public JsonResult GetDistricts(string cityCodeName)
    {
        var cityListBytes = HttpContext.Session.Get("CityList");
        if (cityListBytes != null)
        {
            var cityList = JsonSerializer.Deserialize<List<City>>(cityListBytes);
            var selectedCity = cityList.FirstOrDefault(c => c.Codename == cityCodeName);
            if (selectedCity != null)
            {
                return Json(selectedCity.Districts);
            }
        }
        return Json(new List<District>());
    }

    [HttpGet]
    public JsonResult GetWards(string districtCodeName)
    {
        var districtListBytes = HttpContext.Session.Get("DistrictList");
        if (districtListBytes != null)
        {
            var districtList = JsonSerializer.Deserialize<List<District>>(districtListBytes);
            var selectedDistrict = districtList.FirstOrDefault(d => d.Codename == districtCodeName);
            if (selectedDistrict != null)
            {
                return Json(selectedDistrict.Wards);
            }
        }
        return Json(new List<Ward>());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSADTO sADTO)
    {
        var userId = HttpContext.Session.GetInt32("userID");
        sADTO.UserId = (int)userId;
        string strData = JsonSerializer.Serialize(sADTO);
        var contentData = new StringContent(strData, System.Text.Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _client.PostAsync(SAAPIUrl, contentData);
        if (response.IsSuccessStatusCode)
        {
            return Ok();
        }
        return BadRequest();
    }
}
