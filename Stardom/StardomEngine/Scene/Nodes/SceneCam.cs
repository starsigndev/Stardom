using OpenTK.Mathematics;
using StardomEngine.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardomEngine.Scene.Nodes
{
    public class SceneCam : SceneNode
    {

        public float Rotation
        {
            get;
            set;

        }

        public float Zoom
        {
            get;
            set;
        }

        public SceneCam()
        {

            Rotation = 0.0f;
            Zoom = 1.0f;

        }

        public void AddZoom(float zoom)
        {

            Zoom = Zoom + (zoom * Zoom);

        }

        public void SmartZoom(float zoom,Vector2 os)
        {
            AddZoom(zoom);
            if (zoom != 0)
            {
                Move(os*zoom);
            }

        }

        public override void Move(Vector2 delta)
        {
            //base.Move(delta);
            var move = GameMaths.RotateAndScale(delta, -Rotation, 1.0f);

            Position = new Vector3(Position.X + move.X/Zoom, Position.Y + move.Y/Zoom, Position.Z);
            //Position.Y = Position.Y + move.Y;



        }


    }
}
