using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SistemaControleMateriaisWeb.Models;
using SistemaControleMateriaisWeb.Repositories;
using SistemaControleMateriaisWeb.ViewModels;

namespace SistemaControleMateriaisWeb.Controllers;

public class AccountController : Controller
{
    private readonly UsuarioRepository _usuarios;
    private readonly IPasswordHasher<Usuario> _passwordHasher;

    public AccountController(
        UsuarioRepository usuarios,
        IPasswordHasher<Usuario> passwordHasher)
    {
        _usuarios = usuarios;
        _passwordHasher = passwordHasher;
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");

        ViewData["ReturnUrl"] = returnUrl;

        return View(new LoginViewModel());
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(
        LoginViewModel model,
        string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
            return View(model);

        Usuario? usuario =
            _usuarios.BuscarPorEmail(model.Email);

        if (usuario is null || !usuario.Ativo)
        {
            ModelState.AddModelError(
                string.Empty,
                "E-mail ou senha inválidos.");

            return View(model);
        }

        PasswordVerificationResult resultado =
            _passwordHasher.VerifyHashedPassword(
                usuario,
                usuario.SenhaHash,
                model.Senha);

        if (resultado ==
            PasswordVerificationResult.Failed)
        {
            ModelState.AddModelError(
                string.Empty,
                "E-mail ou senha inválidos.");

            return View(model);
        }

        var claims = new List<Claim>
        {
            new(
                ClaimTypes.NameIdentifier,
                usuario.Id.ToString()),

            new(
                ClaimTypes.Name,
                usuario.Nome),

            new(
                ClaimTypes.Email,
                usuario.Email),

            new(
                ClaimTypes.Role,
                usuario.Perfil)
        };

        var identity =
            new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults
                    .AuthenticationScheme);

        var principal =
            new ClaimsPrincipal(identity);

        var propriedades =
            new AuthenticationProperties
            {
                IsPersistent =
                    model.LembrarMe,

                AllowRefresh =
                    true
            };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults
                .AuthenticationScheme,
            principal,
            propriedades);

        if (!string.IsNullOrWhiteSpace(
                returnUrl)
            &&
            Url.IsLocalUrl(returnUrl))
        {
            return LocalRedirect(returnUrl);
        }

        return RedirectToAction(
            "Index",
            "Home");
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults
                .AuthenticationScheme);

        return RedirectToAction(
            nameof(Login));
    }

    [AllowAnonymous]
    public IActionResult AcessoNegado()
    {
        return View();
    }
}