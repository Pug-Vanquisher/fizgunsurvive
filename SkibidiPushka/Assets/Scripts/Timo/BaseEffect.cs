using UnityEngine;
using System.Collections.Generic;
[System.Serializable]

public class BaseEffect// потом засунуть в наследование каждого уникального эффекта
{
    public string EventName;
    public string Description;
    public Color Color;
    public string Event;

    public BaseEffect(string _name, string _description, Color _color, string _EventName)
    {
        EventName = _name;
        Description = _description;
        Color = _color;
        Event = _EventName;
    }
}

public class DatabaseEffects
{
    public static List<BaseEffect> effects = new List<BaseEffect>()
    {
        new BaseEffect( "Повышение активности",
            "Появление большего количества врагов",
            Color.red, "HighActivity"),
        new BaseEffect("Повышение нагрузки",
            "Появление большего количества обьектов",
            new Color(0.19f, 1f, 1f), "HighLoad"),
        new BaseEffect("Повышение опасности",
            "Появление большего количества пуль и ракеты.",
            Color.yellow, "HighDanger"),
        new BaseEffect("Повышение безопасности",
            "Появление большего количества аптечек первой помощи",
            new Color(0.5f, 0.8f, 0,45f), "HighSequrity"),
        new BaseEffect("Повышение нестабильности",
            "Реальность дрожит, разрываясь большим количеством разломов",
            Color.magenta, "HighVolatility"),
        new BaseEffect("Повышение подвижности",
            "Повышение скорости пользователя",
            Color.cyan, "HighMobility"),
        new BaseEffect("Повышение темпа",
            "Повышение скорости стрельбы оружия",
            new Color(0.6f, 1f, 0.5f), "HighTemp")
    };
    public static Color LastUpgradeColor = Color.white;
}

