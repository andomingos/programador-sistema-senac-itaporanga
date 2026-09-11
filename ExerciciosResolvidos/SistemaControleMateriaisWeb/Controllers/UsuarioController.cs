using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SistemaControleMateriaisWeb.Models;
using SistemaControleMateriaisWeb.Repositories;
using SistemaControleMateriaisWeb.ViewModels;

namespace SistemaControleMateriaisWeb.Controllers;

[Authorize(Roles = "Administrador")]
public class UsuariosController : Controller
{
    private readonly UsuarioRepository _usuarios;
    private readonly IPasswordHasher<Usuario> _passwordHasher;

    public UsuariosController(
        UsuarioRepository usuarios,
        IPasswordHasher<Usuario> passwordHasher)
    {
        _usuarios = usuarios;
        _passwordHasher = passwordHasher;
    }

    public IActionResult Index()
    {
        List<Usuario> usuarios =
            _usuarios.ListarTodos();

        return View(usuarios);
    }

    public IActionResult Details(int id)
    {
        Usuario? usuario =
            _usuarios.BuscarPorId(id);

        if (usuario == null)
            return NotFound();

        return View(usuario);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(
            new CriarUsuarioViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(
        CriarUsuarioViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        Usuario? usuarioExistente =
            _usuarios.BuscarPorEmail(
                model.Email);

        if (usuarioExistente != null)
        {
            ModelState.AddModelError(
                nameof(model.Email),
                "Já existe um usuário com este e-mail.");

            return View(model);
        }

        var usuario = new Usuario
        {
            Nome = model.Nome.Trim(),
            Email = model.Email.Trim(),
            Ativo = true,
            CriadoEm = DateTime.Now,
            Perfil = "Usuario"
        };

        usuario.SenhaHash =
            _passwordHasher.HashPassword(
                usuario,
                model.Senha);

        _usuarios.Inserir(usuario);

        TempData["Mensagem"] =
            "Usuário cadastrado com sucesso.";

        return RedirectToAction(
            nameof(Index));
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        Usuario? usuario =
            _usuarios.BuscarPorId(id);

        if (usuario == null)
            return NotFound();

        var model =
            new EditarUsuarioViewModel
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Ativo = usuario.Ativo
            };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(
        EditarUsuarioViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        Usuario? usuario =
            _usuarios.BuscarPorId(
                model.Id);

        if (usuario == null)
            return NotFound();

        Usuario? emailExistente =
            _usuarios.BuscarPorEmail(
                model.Email);

        if (emailExistente != null &&
            emailExistente.Id != model.Id)
        {
            ModelState.AddModelError(
                nameof(model.Email),
                "Já existe outro usuário com este e-mail.");

            return View(model);
        }

        usuario.Nome =
            model.Nome.Trim();

        usuario.Email =
            model.Email.Trim();

        bool administrador =
            usuario.Perfil ==
            "Administrador";

        if (administrador)
        {
            usuario.Ativo = true;
        }
        else
        {
            usuario.Ativo =
                model.Ativo;
        }

        if (!string.IsNullOrWhiteSpace(
            model.NovaSenha))
        {
            usuario.SenhaHash =
                _passwordHasher.HashPassword(
                    usuario,
                    model.NovaSenha);
        }

        _usuarios.Atualizar(usuario);

        TempData["Mensagem"] =
            "Usuário alterado com sucesso.";

        return RedirectToAction(
            nameof(Index));
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        Usuario? usuario =
            _usuarios.BuscarPorId(id);

        if (usuario == null)
            return NotFound();

        if (usuario.Perfil ==
            "Administrador")
        {
            TempData["Erro"] =
                "O usuário Administrador não pode ser excluído.";

            return RedirectToAction(
                nameof(Index));
        }

        return View(usuario);
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(
        int id)
    {
        Usuario? usuario =
            _usuarios.BuscarPorId(id);

        if (usuario == null)
            return NotFound();

        if (usuario.Perfil ==
            "Administrador")
        {
            TempData["Erro"] =
                "O usuário Administrador não pode ser excluído.";

            return RedirectToAction(
                nameof(Index));
        }

        _usuarios.Excluir(id);

        TempData["Mensagem"] =
            "Usuário excluído com sucesso.";

        return RedirectToAction(
            nameof(Index));
    }
}