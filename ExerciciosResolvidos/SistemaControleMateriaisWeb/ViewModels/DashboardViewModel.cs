namespace SistemaControleMateriaisWeb.ViewModels;

public class DashboardViewModel
{
   public int TotalMateriais { get; set; }
    public int TotalUnidades { get; set; }
    public int EstoqueBaixo { get; set; }
    public int EstoqueZerado { get; set; }
}