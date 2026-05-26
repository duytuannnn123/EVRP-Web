using EVRP_Web.Data;
using EVRP_Web.Models;
using EVRP_Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EVRP_Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ALNSService _alnsService;

        public AdminController(
            ApplicationDbContext db,
            ALNSService alnsService)
        {
            _db = db;
            _alnsService = alnsService;
        }

        // =========================
        // /Admin
        // =========================
        public async Task<IActionResult> Index()
        {
            ViewBag.Nodes = await _db.Nodes
                .AsNoTracking()
                .ToListAsync();

            ViewBag.Drivers = await _db.Drivers
                .AsNoTracking()
                .ToListAsync();

            ViewBag.Customers = await _db.Customers
                .AsNoTracking()
                .ToListAsync();

            return View(await _db.Nodes.AsNoTracking().ToListAsync());
        }

        // =========================
        // /Admin/Management
        // =========================
        public async Task<IActionResult> Management()
        {
            ViewBag.Nodes = await _db.Nodes
                .AsNoTracking()
                .ToListAsync();

            ViewBag.Drivers = await _db.Drivers
                .AsNoTracking()
                .ToListAsync();

            ViewBag.Customers = await _db.Customers
                .AsNoTracking()
                .ToListAsync();

            return View();
        }

        // =========================
        // /Admin/Customers
        // =========================
        public async Task<IActionResult> Customers()
        {
            var customers = await _db.Customers
                .AsNoTracking()
                .ToListAsync();

            return View(customers);
        }

        // =========================
        // /Admin/Drivers
        // =========================
        public async Task<IActionResult> Drivers()
        {
            var drivers = await _db.Drivers
                .AsNoTracking()
                .ToListAsync();

            return View(drivers);
        }

        // =========================
        // /Admin/Create
        // =========================
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // =========================
        // SAVE NODE
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Node node)
        {
            if (!ModelState.IsValid)
            {
                return View(node);
            }

            await _db.Nodes.AddAsync(node);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Management));
        }

        // =========================
        // ADD CUSTOMER
        // =========================
        [HttpPost]
        public async Task<IActionResult> AddCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Management));
            }

            await _db.Customers.AddAsync(customer);

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Customers));
        }

        // =========================
        // ADD DRIVER
        // =========================
        [HttpPost]
        public async Task<IActionResult> AddDriver(Driver driver)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Management));
            }

            await _db.Drivers.AddAsync(driver);

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Drivers));
        }

        // =========================
        // ROUTING
        // =========================
        public async Task<IActionResult> Routing(int? vehicleCount)
        {
            int currentVehicleCount = vehicleCount ?? 3;

            var rawRoutes =
                await _alnsService.SolveFromDbAsync(currentVehicleCount);

            var routes = rawRoutes
                .Select((nodes, index) => new EVRP_Web.Models.Route
                {
                    DriverId = index + 1,
                    DriverName = $"Tài xế {index + 1}",
                    Nodes = nodes,
                    TotalDistance = 0,
                    RemainingBattery = 100
                })
                .ToList();

            ViewBag.AllCustomers = await _db.Customers
                .AsNoTracking()
                .ToListAsync();

            return View(routes);
        }

        // =========================
        // UPDATE DELIVERY
        // =========================
        [HttpPost]
        public async Task<IActionResult> UpdateDeliveryStatus(
            int customerId,
            string deliveryTime)
        {
            var customer = await _db.Customers
                .FirstOrDefaultAsync(x => x.Id == customerId);

            if (customer == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Không tìm thấy khách hàng"
                });
            }

            customer.IsDelivered = true;
            customer.ActualDeliveryTime = deliveryTime;

            await _db.SaveChangesAsync();

            return Json(new
            {
                success = true
            });
        }
    }
}
