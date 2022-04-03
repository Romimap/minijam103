using Godot;
using System;

public class Player : KinematicBody
{
    public static Player Singleton;
    private RayCast m_rayCast;
    private Camera m_camera;
    [Export] private float m_speed = 10;
    [Export] private float m_height = 0.8f;
    [Export] private float m_mouseSensitivity = 0.01f;

    private float azimuth = 0;
    private float elevation = 0;

    private Vector3 m_velocity = new Vector3();
    private Vector3 m_targetVelocity = new Vector3();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        Singleton = this;
        m_rayCast = GetNode<RayCast>("./RayCast");
        m_camera = GetNode<Camera>("./Camera");

        //Captures the mouse
        Input.SetMouseMode(Input.MouseMode.Captured);

        GetNode<AnimationPlayer>("../Control/AnimationPlayer").Play("FadeIn");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta) {
        m_camera.Rotation = new Vector3(elevation, azimuth, 0);


        if (m_rayCast.IsColliding()) {
            Vector3 collisionPoint = m_rayCast.GetCollisionPoint();
            Transform t = GlobalTransform;
            t.origin = collisionPoint + Vector3.Up * m_height;
            GlobalTransform = t;
        }

        Vector3 m_input = new Vector3();
        if (Input.IsActionPressed("forward")) m_input.z -= 1;
        if (Input.IsActionPressed("back")) m_input.z += 1;
        if (Input.IsActionPressed("left")) m_input.x -= 1;
        if (Input.IsActionPressed("right")) m_input.x += 1;
        GD.Print(m_input);

        Vector3 forward = m_camera.Transform.basis.z;
        forward.y = 0;
        forward = forward.Normalized();

        Vector3 right = m_camera.Transform.basis.x;
        right.y = 0;
        right = right.Normalized();

        m_targetVelocity = forward * m_input.z * m_speed;
        m_targetVelocity += right * m_input.x * m_speed;

        MoveAndSlide(m_velocity * delta, Vector3.Up);
    }

    public override void _Input(InputEvent @event) {
        if (@event is InputEventMouseMotion) {
            InputEventMouseMotion mouseMotion = (InputEventMouseMotion)@event;
            Vector2 delta = mouseMotion.Relative;
            azimuth -= delta.x * m_mouseSensitivity;
            elevation -= delta.y * m_mouseSensitivity;
            elevation = Mathf.Clamp(elevation, -Mathf.Pi / 2, Mathf.Pi / 2);
        }
    }

    public override void _PhysicsProcess(float delta) {
        m_velocity = m_velocity * 0.9f + m_targetVelocity * 0.1f;
    }

    public void Die () {
        GetNode<AnimationPlayer>("../Control/AnimationPlayer").Play("FadeOut");
    }
}
