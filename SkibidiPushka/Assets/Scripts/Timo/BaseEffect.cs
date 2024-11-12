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
        new BaseEffect( "��������� ����������",
            "��������� �������� ���������� ������",
            Color.red, "HighActivity"),
        new BaseEffect("��������� ��������",
            "��������� �������� ���������� ��������",
            Color.yellow, "HighLoad"),
        new BaseEffect("��������� ��������",
            "� ����� ����� ����� ����, �������� ������������",
            new Color(0, 0.8f, 0.7f), "HighHealth"),
        new BaseEffect("��������� ��������������",
            "���������� ������, ���������� ������� ����������� ��������",
            Color.magenta, "HighVolatility"),
        new BaseEffect("��������: ������������",
            "������� ������� ����� ����������, �������� ��� �������",
            Color.cyan, "RiftStabilize")
    };
    public static Color LastUpgradeColor = Color.white;
}

public class DatabaseRiftEffects
{
    public static List<RiftEffect> effects = new List<RiftEffect>()
    {
        new RiftEffect("��� ��������������� ��������� � ������...", "EnemiesToWalls"),
        new RiftEffect("�� ������ ��� ������...", "x5Enemies"),
        new RiftEffect("���� ����� ����� ������ ������...", "AnotherSprite"),
        new RiftEffect("�� ������� ��� �� �������...", "EnemiesToWalls"),
        new RiftEffect("���� ����� ����� �������...", "Invulnerability", 10f),
        new RiftEffect("���������� ���� ����...", "Atomic"),
        new RiftEffect("���� ����� �� ������� ����", "Ghost", 10f)
    };
}