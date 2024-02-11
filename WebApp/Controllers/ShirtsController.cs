using Microsoft.AspNetCore.Mvc;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class ShirtsController : Controller
    {
        private readonly IWebApiExecuter webApiExecuter;

        public ShirtsController(IWebApiExecuter webApiExecuter)
        {
            this.webApiExecuter = webApiExecuter;
        }

        public async Task<IActionResult> Index()
        {
            return View(await webApiExecuter.InvokeGet<List<Shirt>>("shirts"));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Shirt shirt)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var response = await webApiExecuter.InvokePost("shirts", shirt);

                    if (response != null)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch(WebApiException ex)
                {
                    HandleWebApiException(ex);
                }
            }

            return View(shirt);
        }

        public async Task<IActionResult> Update(int shirtId)
        {
            try
            {
                var shirt = await webApiExecuter.InvokeGet<Shirt>($"shirts/{shirtId}");

                if (shirt != null)
                {
                    return View(shirt);
                }
            }
            catch(WebApiException ex)
            {
                HandleWebApiException(ex);
                return View();
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Update(Shirt shirt)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await webApiExecuter.InvokePut($"shirts/{shirt.ShirtId}", shirt);
                    return RedirectToAction(nameof(Index));
                }
                catch(WebApiException ex)
                {
                    HandleWebApiException(ex);
                }
            }

            return View(shirt);
        }

        public async Task<IActionResult> Delete(int ShirtId)
        {
            try
            {
                await webApiExecuter.InvokeDelete($"shirts/{ShirtId}");
                return RedirectToAction(nameof(Index));
            }
            catch(WebApiException ex)
            {
                HandleWebApiException(ex);
                return View(nameof(Index), await webApiExecuter.InvokeGet<List<Shirt>>("shirts"));
            }
        }

        private void HandleWebApiException(WebApiException ex)
        {
            if (ex.ErrorResponse != null &&
                        ex.ErrorResponse.Errors != null &&
                        ex.ErrorResponse.Errors.Count > 0)
            {
                foreach (var error in ex.ErrorResponse.Errors)
                {
                    ModelState.AddModelError(error.Key, string.Join("; ", error.Value));
                }
            }
        }
    }
}

