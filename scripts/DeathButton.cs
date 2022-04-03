using Godot;
using System;

public class DeathButton : StrangeRoom
{
    RichTextLabel m_buttonLabel;

    public static int apparitions = 0;


    public string[] texts = {
        "DANGER, VENT OXYGEN INTO SPACE",
        "VENT OXYGEN INTO SPACE",
        "MAYBE THATS WHAT I WANT ?",
        "WHAT CHOICE DO I HAVE ?",
        "I HAVE NO CHOICE",
        "DIE"
    };


    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        base._Ready();
        m_buttonLabel = GetNode<RichTextLabel>("./Viewport/RichTextLabel");
        if (apparitions > 5) m_buttonLabel.BbcodeText = "[center]" + texts[5];
        else m_buttonLabel.BbcodeText = "[center]" + texts[apparitions];
    }

    public override void OnFirstVisit() {
        base.OnFirstVisit();
        apparitions++;
        StrangeRoom.packedScenes.Clear();
        if (apparitions == 1) {
            StrangeRoom.packedScenes.Add(ResourceLoader.Load<PackedScene>("res://prefab/StrangeRooms/Corridor.tscn"));
            StrangeRoom.packedScenes.Add(ResourceLoader.Load<PackedScene>("res://prefab/StrangeRooms/Room.tscn"));
            StrangeRoom.packedScenes.Add(ResourceLoader.Load<PackedScene>("res://prefab/StrangeRooms/Stairs.tscn"));
            StrangeRoom.packedScenes.Add(ResourceLoader.Load<PackedScene>("res://prefab/StrangeRooms/ButtonRoom.tscn"));
        }
        if (apparitions == 2) {
            StrangeRoom.packedScenes.Add(ResourceLoader.Load<PackedScene>("res://prefab/StrangeRooms/Corridor.tscn"));
            StrangeRoom.packedScenes.Add(ResourceLoader.Load<PackedScene>("res://prefab/StrangeRooms/Room.tscn"));
            StrangeRoom.packedScenes.Add(ResourceLoader.Load<PackedScene>("res://prefab/StrangeRooms/Stairs.tscn"));
            StrangeRoom.packedScenes.Add(ResourceLoader.Load<PackedScene>("res://prefab/StrangeRooms/CorridorDelabre.tscn"));
            StrangeRoom.packedScenes.Add(ResourceLoader.Load<PackedScene>("res://prefab/StrangeRooms/RoomDelabre.tscn"));
            StrangeRoom.packedScenes.Add(ResourceLoader.Load<PackedScene>("res://prefab/StrangeRooms/StairsDelabre.tscn"));
            StrangeRoom.packedScenes.Add(ResourceLoader.Load<PackedScene>("res://prefab/StrangeRooms/ButtonRoom.tscn"));
            StrangeRoom.packedScenes.Add(ResourceLoader.Load<PackedScene>("res://prefab/StrangeRooms/ButtonRoom.tscn"));
        }
        if (apparitions == 3) {
            StrangeRoom.packedScenes.Add(ResourceLoader.Load<PackedScene>("res://prefab/StrangeRooms/CorridorDelabre.tscn"));
            StrangeRoom.packedScenes.Add(ResourceLoader.Load<PackedScene>("res://prefab/StrangeRooms/RoomDelabre.tscn"));
            StrangeRoom.packedScenes.Add(ResourceLoader.Load<PackedScene>("res://prefab/StrangeRooms/StairsDelabre.tscn"));
            StrangeRoom.packedScenes.Add(ResourceLoader.Load<PackedScene>("res://prefab/StrangeRooms/ButtonRoom.tscn"));
        }
        if (apparitions == 4) {
            StrangeRoom.packedScenes.Add(ResourceLoader.Load<PackedScene>("res://prefab/StrangeRooms/CorridorDelabre.tscn"));
            StrangeRoom.packedScenes.Add(ResourceLoader.Load<PackedScene>("res://prefab/StrangeRooms/RoomDelabre.tscn"));
            StrangeRoom.packedScenes.Add(ResourceLoader.Load<PackedScene>("res://prefab/StrangeRooms/StairsDelabre.tscn"));
            StrangeRoom.packedScenes.Add(ResourceLoader.Load<PackedScene>("res://prefab/StrangeRooms/ButtonRoom.tscn"));
        }
        if (apparitions >= 5) {
            StrangeRoom.packedScenes.Add(ResourceLoader.Load<PackedScene>("res://prefab/StrangeRooms/ButtonRoom.tscn"));
        }
    }
}
