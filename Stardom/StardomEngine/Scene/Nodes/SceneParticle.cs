using StardomEngine.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Mathematics;
using System.Text;
using System.Threading.Tasks;

namespace StardomEngine.Scene.Nodes
{
    public enum ParticleType
    {
        Sprite,Spark
    }
    public class SceneParticle
    {

        public ParticleType Type
        {
            get;
            set;
        }

        public Vector3 Position
        {
            get;
            set;
        }

        public Vector3 Inertia
        {
            get;
            set;
        }

        public Vector3 Gravity
        {
            get;
            set;
        }

        public Texture2D Image
        {
            get;
            set;
        }

        public bool CastShadows
        {
            get;
            set;
        }

        public bool RecvShadows
        {
            get;
            set;
        }

        public float Rotation
        {
            get;
            set;
        }

        public SceneParticleSystem Owner
        {
            get;
            set;
        }

        public SceneParticle()
        {

            CastShadows = false;
            RecvShadows = false;

        }

        public SceneParticle Clone()
        {

            SceneParticle p = new SceneParticle();

            p.Position = Position;
            p.Inertia = Inertia;
            p.Gravity = Gravity;
            p.Image = Image;
            p.Type = Type;
            p.RecvShadows = RecvShadows;
            p.CastShadows = CastShadows;

            return p;

        }

    }
}
