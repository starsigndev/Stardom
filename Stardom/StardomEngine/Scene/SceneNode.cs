using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using StardomEngine.Draw;
using StardomEngine.Scene.Nodes;

namespace StardomEngine.Scene
{
    public class SceneNode
    {

        public Vector3 Position
        {
            get;
            set;
        }

        public Vector2 Size
        {
            get;
            set;
        }

        public List<SceneNode> SubNodes
        {
            get;
            set;
        }

        public SceneNode()
        {

            SubNodes = new List<SceneNode>();
            Size = new Vector2(128, 128);

        }

        public void AddNode(SceneNode node)
        {

            SubNodes.Add(node);

        }

        public virtual void Init() { }
        public virtual void Update() { }
        public virtual void Render(SceneCam camera,SmartDraw draw)
        {

            RenderSubNodes(camera, draw);

        }

        public virtual void RenderSubNodes(SceneCam camera, SmartDraw draw)
        {

            foreach(var node in SubNodes)
            {
                node.Render(camera, draw);
            }

        }


    }
}
