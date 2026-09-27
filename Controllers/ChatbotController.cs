
using Microsoft.AspNetCore.Mvc;

namespace MarketLink.Controllers;

public class ChatbotController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Ask(string message)
    {
        var m = (message ?? "").ToLowerInvariant();

        var answer = m.Contains("order")
            ? "You can place an order from Marketplace and review it in your Customer Dashboard."

            : m.Contains("farmer")
                ? "Farmers can register, add products, manage stock and review related orders."

                : m.Contains("price")
                    ? "Product prices are shown in the Marketplace and are set by the farmer."

                    : "I can help with Marketplace, orders, farmers, products and account navigation.";

        return Json(new { answer });
    }
}

