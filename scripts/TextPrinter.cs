using Godot;
using System;
using System.Collections.Generic;


public class TextPrinter : MeshInstance
{
    public static List<TextPrinter> textPrinters = new List<TextPrinter>();
    RichTextLabel m_buttonLabel;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        textPrinters.Add(this);
        m_buttonLabel = GetNode<RichTextLabel>("/root/Spatial/Control/RichTextLabel");

    }

    public override void _ExitTree () {
        textPrinters.Remove(this);
        if (textPrinters.Count == 0) m_buttonLabel.Modulate = new Color(0, 0, 0, 0);
    }


    public override void _Process(float delta) {
        if (textPrinters[0] != this) return;

        float mindist = 999;

        foreach (TextPrinter tp in textPrinters) {
            float d = tp.GlobalTransform.origin.DistanceTo(Player.Singleton.GlobalTransform.origin);
            if (d < mindist) {
                mindist = d;
            }
        }

        if (mindist < 3) {
            if (Input.IsActionPressed("action")) Player.Singleton.Die();
            float alpha = (3 - mindist);
            m_buttonLabel.Modulate = new Color(1, 1, 1, alpha);
        }
    }
}
