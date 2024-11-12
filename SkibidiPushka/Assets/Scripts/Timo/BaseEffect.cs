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
public class RiftEffect
{
    public string EventName;
    public string Event;
    public float EventTime;
    public RiftEffect(string _name, string _EventName, float _Time = 0f)
    {
        EventName = _name;
        Event = _EventName;
        EventTime = _Time;
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
            Color.yellow, "HighLoad"),
        new BaseEffect("Повышение здоровье",
            "В ваших венах течет сила, укрепляя пользователя",
            new Color(0, 0.8f, 0.7f), "HighHealth"),
        new BaseEffect("Повышение нестабильности",
            "Реальность дрожит, разрываясь большим количеством разломов",
            Color.magenta, "HighVolatility"),
        new BaseEffect("Протокол: стабильность",
            "Порядок сшивает ткани реальности, закрывая все разломы",
            Color.cyan, "RiftStabilize")
    };
    public static Color LastUpgradeColor = Color.white;
}

public class DatabaseRiftEffects
{
    public static List<RiftEffect> effects = new List<RiftEffect>()
    {
        new RiftEffect("Все Недоброжелатели обратятся в камень...", "EnemiesToWalls"),
        new RiftEffect("Их станет все больше...", "x5Enemies"),
        new RiftEffect("Твои глаза могут видеть больше...", "AnotherSprite"),
        new RiftEffect("Ты станешь чем то большим...", "EnemiesToWalls"),
        new RiftEffect("Твои грехи будут прощены...", "Invulnerability", 10f),
        new RiftEffect("Разрушение явит себя...", "Atomic"),
        new RiftEffect("Даже стены не удержат тебя", "Ghost", 10f)
    };
}