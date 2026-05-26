using EVRP_Web.Data;
using EVRP_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EVRP_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Thống kê tổng số lượng từ các bảng trong database MySQL để làm Dashboard báo cáo
            ViewBag.TotalCustomers = _context.Customers.Count();
            ViewBag.TotalDrivers = _context.Drivers.Count(d => d.IsActive);
            ViewBag.TotalStations = _context.Nodes.Count(n => (n.NodeType ?? "").ToLower().Contains("station"));
            
            // Tính tổng khối lượng hàng hóa cần giao trong ngày
            ViewBag.TotalDemand = _context.Customers.Sum(c => (double?)c.Demand) ?? 0;

            // Lấy danh sách khách hàng mới cập nhật gần đây nhất để hiển thị bảng theo dõi nhanh
            var recentCustomers = _context.Customers
                .OrderByDescending(c => c.Id)
                .Take(5)
                .AsNoTracking()
                .ToList();

            return View(recentCustomers);
        }
    }
}