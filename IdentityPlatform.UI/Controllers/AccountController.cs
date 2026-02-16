using IdentityPlatform.Core.Common;
using IdentityPlatform.UI.Models;
using IdentityPlatform.UI.Models.Enums;
using IdentityPlatform.UI.Services;
using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    private const string LoginViewName = "Login";
    private const string ErrorViewName = "Error";
    private const string ProfileViewName = "Profile";

    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }


    [HttpGet("login")]
    public async Task<IActionResult> Login(string clientId, string redirectUri)
    {
        var result = await _accountService.PrepareLoginAsync(clientId, redirectUri);

        if (!result.IsSuccess)
            return ErrorView(result);

        return View(LoginViewName, result.Data);
    }

    [HttpPost("login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(LoginViewName, model);

        var result = await _accountService.LoginAndGetCodeAsync(model);

        if (!result.IsSuccess)
            return View(LoginViewName, model.WithError(result.Errors.First()));

        return RedirectToAction(nameof(Consent), new
        {
            code = result.Data,
            model.ClientId,
            model.ClientName,
            model.RedirectUri
        });
    }



    [HttpGet("consent")]
    public IActionResult Consent(ConsentViewModel model)
        => View(model);

    [HttpPost("consent")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmConsent(ConsentPostModel model)
    {
        if (model.Decision != ConsentDecision.Accept)
            return View(ErrorViewName, "User denied permission.");

        var result = await _accountService.ProcessConsentAsync(model);

        if (!result.IsSuccess)
            return ErrorView(result);

        // Operations related to the database can be performed here

        return Redirect(model.RedirectUri);
    }



    [HttpGet("profile")]
    public IActionResult Profile(UserProfileViewModel model)
    {
        if (!model.IsAuthenticated)
            return RedirectToAction(nameof(Login));

        return View(ProfileViewName, model);
    }



    private IActionResult ErrorView<T>(Result<T> result)
    {
        var error = result.Errors?.FirstOrDefault() ?? "Unknown error";
        return View(ErrorViewName, error);
    }

}
