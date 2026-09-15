using System.ComponentModel.DataAnnotations;

namespace MetallurgyApp.Web.Models;

public class ProductionArea
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Укажите название участка")]
    [Display(Name = "Название участка")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Цех")]
    public string Workshop { get; set; } = string.Empty;

    [Display(Name = "Начальник участка")]
    public string Head { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    [Display(Name = "Дата ввода в эксплуатацию")]
    public DateTime CommissionedAt { get; set; }

    public List<Equipment> Equipments { get; set; } = new();
}