using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaControleMateriaisWeb.Models;
using SistemaControleMateriaisWeb.Repositories;
using SistemaControleMateriaisWeb.ViewModels;

namespace SistemaControleMateriaisWeb.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly MaterialRepository _materiais;

    public HomeController(MaterialRepository materiais)
    {
        _materiais = materiais;
    }

    public IActionResult Index()
    {
        List<Material> materiais = _materiais.ListarTodos();

        var model = new DashboardViewModel
        {
            TotalMateriais = materiais.Count
        };

        foreach (Material material in materiais)
        {
            model.TotalUnidades += material.Quantidade;

            if (material.Quantidade == 0)
            {
                model.EstoqueZerado++;
            }
            else if (material.Quantidade <= material.EstoqueMinimo)
            {
                model.EstoqueBaixo++;
            }
        }

        return View(model);
    }
}