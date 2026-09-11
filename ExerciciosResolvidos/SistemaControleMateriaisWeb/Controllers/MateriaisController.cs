using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaControleMateriaisWeb.Models;
using SistemaControleMateriaisWeb.Repositories;

namespace SistemaControleMateriaisWeb.Controllers;

[Authorize]
public class MateriaisController : Controller
{
    private readonly MaterialRepository _materiais;

    public MateriaisController(MaterialRepository materiais)
    {
        _materiais = materiais;
    }

    public IActionResult Index()
    {
        List<Material> materiais = _materiais.ListarTodos();
        return View(materiais);
    }

    public IActionResult Details(int id)
    {
        Material? material = _materiais.BuscarPorId(id);

        if (material is null)
            return NotFound();

        return View(material);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new Material());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Material material)
    {
        if (!ModelState.IsValid)
            return View(material);

        _materiais.Inserir(material);
        TempData["Mensagem"] = "Material cadastrado com sucesso.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        Material? material = _materiais.BuscarPorId(id);

        if (material is null)
            return NotFound();

        return View(material);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Material material)
    {
        if (id != material.Id)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(material);

        if (!_materiais.Atualizar(material))
            return NotFound();

        TempData["Mensagem"] = "Material alterado com sucesso.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        Material? material = _materiais.BuscarPorId(id);

        if (material is null)
            return NotFound();

        return View(material);
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        if (!_materiais.Excluir(id))
            return NotFound();

        TempData["Mensagem"] = "Material excluído com sucesso.";
        return RedirectToAction(nameof(Index));
    }
}