using MarketLink.Services;
using Microsoft.AspNetCore.Mvc;

namespace MarketLink.Controllers;

public class ChatbotController : Controller
{
    private readonly ChatbotService _bot;
    private readonly IAuthService _auth;

    public ChatbotController(ChatbotService bot, IAuthService auth)
    {
        _bot = bot;
        _auth = auth;
    }

    public IActionResult Index() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Ask(string message)
    {
        var userId = _auth.CurrentUserId(User);
        var reply = await _bot.ReplyAsync(message ?? string.Empty, userId);
        return Json(new { reply });
    }
}
