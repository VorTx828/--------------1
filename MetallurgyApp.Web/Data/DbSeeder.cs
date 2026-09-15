using MetallurgyApp.Web.Models;

namespace MetallurgyApp.Web.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext db)
    {
        if (db.ProductionAreas.Any()) return;

        var areas = new List<ProductionArea>
        {
            new() { Name = "Доменный участок №1", Workshop = "Доменный цех",
                    Head = "Иванов И.И.", CommissionedAt = new DateTime(1985, 6, 1) },
            new() { Name = "Конвертерный участок", Workshop = "Сталеплавильный цех",
                    Head = "Петров П.П.", CommissionedAt = new DateTime(1998, 3, 15) },
            new() { Name = "Прокатный стан 2000", Workshop = "Прокатный цех",
                    Head = "Сидоров С.С.", CommissionedAt = new DateTime(2005, 11, 20) }
        };
        db.ProductionAreas.AddRange(areas);
        db.SaveChanges();

        db.Equipments.AddRange(
            new Equipment { Name = "Доменная печь", Model = "ДП-5000", Manufacturer = "Уралмаш",
                ManufactureYear = 1985, State = EquipmentState.Working, ProductionAreaId = areas[0].Id },
            new Equipment { Name = "Воздухонагреватель", Model = "ВН-1200", Manufacturer = "Металлургмаш",
                ManufactureYear = 1990, State = EquipmentState.Working, ProductionAreaId = areas[0].Id },
            new Equipment { Name = "Кислородный конвертер", Model = "КК-350", Manufacturer = "SMS Group",
                ManufactureYear = 1998, State = EquipmentState.Working, ProductionAreaId = areas[1].Id },
            new Equipment { Name = "Установка непрерывной разливки", Model = "УНРС-4", Manufacturer = "Danieli",
                ManufactureYear = 2000, State = EquipmentState.UnderRepair, ProductionAreaId = areas[1].Id },
            new Equipment { Name = "Прокатный стан", Model = "Стан-2000", Manufacturer = "НКМЗ",
                ManufactureYear = 2005, State = EquipmentState.Working, ProductionAreaId = areas[2].Id }
        );
        db.SaveChanges();
    }
}