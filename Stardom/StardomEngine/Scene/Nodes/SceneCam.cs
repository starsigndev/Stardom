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

    }
}
