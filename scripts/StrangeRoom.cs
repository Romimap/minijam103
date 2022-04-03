using Godot;
using System;
using System.Collections.Generic;

public class StrangeRoom : VisibilityNotifier {
    public static List<StrangeRoom> rooms = new List<StrangeRoom>();
    public static List<StrangeRoom> current = new List<StrangeRoom>();
    public static List<PackedScene> packedScenes = null;

    private static Random rng = new Random();
    
    [Export] private List<NodePath> nodePaths;
    private List<Spatial> ports = new List<Spatial>();
    private List<int> availablePorts = new List<int>();
    private List<int> usedPorts = new List<int>();
    private bool seen = false;
    private bool visited = false;



    private Area area;

    [System.Serializable]
    public struct Link {
        public StrangeRoom room;
        public int port;
    }
    
    public List<Link> links = new List<Link>();


    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        if (packedScenes == null) {
            packedScenes = new List<PackedScene>();
            packedScenes.Add(ResourceLoader.Load<PackedScene>("res://prefab/StrangeRooms/Corridor.tscn"));
            packedScenes.Add(ResourceLoader.Load<PackedScene>("res://prefab/StrangeRooms/Room.tscn"));
            packedScenes.Add(ResourceLoader.Load<PackedScene>("res://prefab/StrangeRooms/Stairs.tscn"));
            packedScenes.Add(ResourceLoader.Load<PackedScene>("res://prefab/StrangeRooms/ButtonRoom.tscn"));
        }

        rooms.Add(this);
        area = GetNode<Area>("Area");

        int i = 0;
        foreach(NodePath nodePath in nodePaths) {
            ports.Add(GetNode<Spatial>(nodePath));
            availablePorts.Add(i);
            i++;
        }
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta) {
        if (IsOnScreen()) seen = true;
        if (!seen) return;

        if (!IsOnScreen()) OnNotVisible();
        
        if (!current.Contains(this)) return; //Returns if the player is not on the room

        while (availablePorts.Count > 0) { //Tries to fill the ports
            int port = availablePorts[0];
            PackedScene packedScene = packedScenes[rng.Next(0, packedScenes.Count)];
            StrangeRoom roomToAdd = packedScene.Instance<StrangeRoom>();

            GetParent().AddChild(roomToAdd);
            GD.Print(Name + " Generated " + availablePorts.Count);
            LinkWith(roomToAdd, port);
        }
    }

    public void LinkWith (StrangeRoom other, int onPort) {
        int otherPort = other.availablePorts[rng.Next(0, other.availablePorts.Count)];
        other.availablePorts.Remove(otherPort);
        other.usedPorts.Add(otherPort);
        availablePorts.Remove(onPort);
        usedPorts.Add(onPort);

        Link otherLink;
        otherLink.port = otherPort;
        otherLink.room = this;

        Link link;
        link.port = onPort;
        link.room = other;

        other.links.Add(otherLink);
        links.Add(link);


        //NOT SURE :(        
        Quat otherQuat = new Quat(Vector3.Up, Mathf.Pi) * ports[onPort].GlobalTransform.basis.Quat() * other.ports[otherPort].Transform.basis.Quat().Inverse();
        other.GlobalTransform = new Transform(otherQuat, Vector3.Zero);
        other.GlobalTranslate(ports[onPort].GlobalTransform.origin - other.ports[otherPort].GlobalTransform.origin);
    }


    public void Unlink (Link link) {
        StrangeRoom other = link.room;
        int port = link.port;
        foreach (Link otherLink in other.links) {
            if (otherLink.room == this) {
                int otherPort = otherLink.port;
                other.availablePorts.Add(otherPort);
                other.usedPorts.Remove(otherPort);
                other.links.Remove(otherLink);

                break;
            }
        }

        availablePorts.Add(port);
        usedPorts.Remove(port);
        links.Remove(link);
    }


    //Recursively removes rooms that are attached to a room that the player can't see
    public void removeStrangeRoom () {
        if (current.Contains(this)) return; //Returns if the player is still on the room

        while (links.Count > 0) {
            StrangeRoom otherRoom = links[0].room;
            Unlink(links[0]);
            otherRoom.removeStrangeRoom();
        }

        GD.Print("Removed " + Name);
        QueueFree();
    }

    public virtual void OnFirstVisit () {

    }

    //Called when the room is not in the viewport
    public void OnNotVisible () {
        if (current.Contains(this)) return; //Returns if the player is still on the room
            removeStrangeRoom();
    }


    //Called when the player enters the room
    public void OnBodyEntered(Node body) {
        if (body is Player) {
            if (!current.Contains(this)) current.Add(this);
            if (!seen || visited) return;
            visited = true;
            OnFirstVisit();
        }
    }


    //Called when the player leaves the room
    public void OnBodyExited(Node body) {
        if (body is Player) {
            current.Remove(this);
        }
    }

}
