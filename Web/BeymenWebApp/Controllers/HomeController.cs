using BeymenWebApp.Models;
using BeymenWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BeymenWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfigurationService _configurationService;

        public HomeController(ILogger<HomeController> logger, IConfigurationService configurationService)
        {
            _logger = logger;
            _configurationService = configurationService;
        }

        public IActionResult Index()
        {
            var configurationList = _configurationService.GetAllConfigurations().Select(x => new BeymenWebApp.Models.Configuration()
            {
                ID = x.ID,
                ApplicationName = x.ApplicationName,
                IsActive = x.IsActive,
                Name = x.Name,
                Type = x.Type,
                Value = x.Value
            });
            return View(configurationList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var configuration = _configurationService.GetById(id);
            if (configuration == null)
            {
                return NotFound();
            }

            return View(configuration);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Type,IsActive,ApplicationName,Value")] Configuration configuration)
        {
            if (ModelState.IsValid)
            {
                _configurationService.Add(configuration);
                return RedirectToAction(nameof(Index));
            }
            return View(configuration);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var configuration = _configurationService.GetById(id);
            if (configuration == null)
            {
                return NotFound();
            }

            return View(configuration);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Type,IsActive,ApplicationName,Value")] Configuration configuration)
        {
            if (id != configuration.ID)
            {
                return NotFound();
            }

            _configurationService.Update(configuration);
            return RedirectToAction(nameof(Index));
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var configuration = _configurationService.GetById(id);
            if (configuration == null)
            {
                return NotFound();
            }

            return View(configuration);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var configuration = _configurationService.GetById(id);
            if (configuration == null)
            {
                return Problem("Entity Not Found");
            }

            _configurationService.Delete(configuration);

            return RedirectToAction(nameof(Index));
        }

    }
}