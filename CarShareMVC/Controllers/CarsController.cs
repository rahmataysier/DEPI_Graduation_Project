using CarShareBLL.Services;
using CarShareBLL.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CarShareMVC.Controllers
{
    public class CarsController : Controller
    {
        private readonly ICarService _carService;

        public CarsController(ICarService carService)
        {
            _carService = carService;
        }

        // GET: /Cars (Browse all available cars)
        [HttpGet]
        public async Task<IActionResult> Index(string search, string carType, string brand, 
            decimal? minPrice, decimal? maxPrice, string location)
        {
            IEnumerable<CarDto> cars;

            // If any filter is applied
            if (!string.IsNullOrEmpty(search) || !string.IsNullOrEmpty(carType) || 
                !string.IsNullOrEmpty(brand) || minPrice.HasValue || maxPrice.HasValue ||
                !string.IsNullOrEmpty(location))
            {
                var filterDto = new CarSearchFilterDto
                {
                    SearchTerm = search,
                    CarType = carType,
                    Brand = brand,
                    MinPrice = minPrice,
                    MaxPrice = maxPrice,
                    Location = location
                };

                cars = await _carService.FilterCarsAsync(filterDto);
            }
            else
            {
                cars = await _carService.GetAvailableCarsAsync();
            }

            ViewBag.Search = search;
            ViewBag.CarType = carType;
            ViewBag.Brand = brand;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.Location = location;

            return View(cars);
        }

        // GET: /Cars/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var car = await _carService.GetCarByIdAsync(id);

            if (car == null)
            {
                TempData["ErrorMessage"] = "Car not found";
                return RedirectToAction(nameof(Index));
            }

            return View(car);
        }

        // GET: /Cars/Create
        [HttpGet]
        public IActionResult Create()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var userRole = HttpContext.Session.GetString("UserRole");

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            if (userRole != "CarOwner")
            {
                TempData["ErrorMessage"] = "Only car owners can create car posts";
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // POST: /Cars/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCarDto model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _carService.CreateCarAsync(userId.Value, model);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(model);
            }

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction(nameof(MyCars));
        }

        // GET: /Cars/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var car = await _carService.GetCarByIdAsync(id);

            if (car == null)
            {
                TempData["ErrorMessage"] = "Car not found";
                return RedirectToAction(nameof(MyCars));
            }

            if (car.OwnerId != userId.Value)
            {
                TempData["ErrorMessage"] = "You can only edit your own cars";
                return RedirectToAction(nameof(MyCars));
            }

            var updateDto = new UpdateCarDto
            {
                Id = car.Id,
                Title = car.Title,
                Description = car.Description,
                PricePerDay = car.PricePerDay,
                Location = car.Location,
                AvailableFrom = car.AvailableFrom,
                AvailableTo = car.AvailableTo,
                MainImageUrl = car.MainImageUrl,
                AdditionalImages = car.AdditionalImages,s
            };

            return View(updateDto);
        }

        // POST: /Cars/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateCarDto model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _carService.UpdateCarAsync(userId.Value, model);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(model);
            }

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction(nameof(MyCars));
        }

        // POST: /Cars/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var result = await _carService.DeleteCarAsync(id, userId.Value);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
            }
            else
            {
                TempData["SuccessMessage"] = result.Message;
            }

            return RedirectToAction(nameof(MyCars));
        }

        // GET: /Cars/MyCars
        [HttpGet]
        public async Task<IActionResult> MyCars()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var cars = await _carService.GetCarsByOwnerIdAsync(userId.Value);
            return View(cars);
        }
    }
}