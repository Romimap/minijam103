using Godot;
using System;

public class Days : RichTextLabel {
    public static int days = 0;
    public static Random rng = new Random();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        days += rng.Next(9999);
        BbcodeText = "[center] Day " + days;
    }
}
