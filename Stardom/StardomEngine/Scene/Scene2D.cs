using StardomEngine.Draw;
using StardomEngine.Scene.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardomEngine.Scene
{
    public class Scene2D
    {

        public SceneNode RootNode
        {
            get;
            set;
        }

        public SceneCam Camera
        {
            get;
            set;
        }

        private SmartDraw Draw
        {
            get;
            set;
        }

        public Scene2D()
        {

            RootNode = new SceneNode();
            Camera = new SceneCam();
            Draw = new SmartDraw();

        }

        public void AddNode(SceneNode node)
        {

            RootNode.AddNode(node);

        }

        public virtual void Update()
        {

        }

        public virtual void Render()
        {

            Draw.Begin();

            RootNode.Render(Camera,Draw);

            Draw.End();

        }

    }
}
