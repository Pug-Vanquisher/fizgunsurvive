using UnityEngine;
using System.Collections.Generic;
[System.Serializable]

public class BaseEffect// ����� �������� � ������������ ������� ����������� �������
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
        new BaseEffect( "��������� ����������",
            "��������� �������� ���������� ������",
            Color.red, "HighActivity"),
        new BaseEffect("��������� ��������",
            "��������� �������� ���������� ��������",
            new Color(0.19f, 1f, 1f), "HighLoad"),
        new BaseEffect("��������� ���������",
            "��������� �������� ���������� ���� � ������.",
            Color.yellow, "HighDanger"),
        new BaseEffect("��������� ������������",
            "��������� �������� ���������� ������� ������ ������",
            new Color(0.5f, 0.8f, 0,45f), "HighSequrity"),
        new BaseEffect("��������� ��������������",
            "���������� ������, ���������� ������� ����������� ��������",
            Color.magenta, "HighVolatility"),
        new BaseEffect("��������� �����������",
            "��������� �������� ������������",
            Color.cyan, "HighMobility"),
        new BaseEffect("��������� �����",
            "��������� �������� �������� ������",
            new Color(0.6f, 1f, 0.5f), "HighTemp")
    };
    public static Color LastUpgradeColor = Color.white;
}

