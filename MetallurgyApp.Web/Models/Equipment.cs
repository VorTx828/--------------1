using System.ComponentModel.DataAnnotations;

namespace MetallurgyApp.Web.Models;

public enum EquipmentState
{
    [Display(Name = "Работает")] Working,
    [Display(Name = "На ремонте")] UnderRepair,
    [Display(Name = "Законсервировано")] Mothballed,
    [Display(Name = "Списано")] Decommissioned
}

public class Equipment
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Укажите название оборудования")]
    [Display(Name = "Название")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Модель")]
    public string Model { get; set; } = string.Empty;

    [Display(Name = "Производитель")]
    public string Manufacturer { get; set; } = string.Empty;

    [Range(1950, 2100, ErrorMessage = "Некорректный год")]
    [Display(Name = "Год выпуска")]
    public int ManufactureYear { get; set; }

    [Display(Name = "Состояние")]
    public EquipmentState State { get; set; }

    [Display(Name = "Производственный участок")]
    public int ProductionAreaId { get; set; }

    public ProductionArea? ProductionArea { get; set; }
}