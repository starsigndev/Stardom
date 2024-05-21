﻿using OpenTK.Mathematics;
using StardomEngine.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardomEngine.Scene.Nodes
{
    public class SceneLight : SceneNode
    {

        public Vector3 Diffuse
        {
            get;
            set;
        }

        public float Range
        {
            get;
            set;
        }


        public Texture2D ShadowMap
        {
            get;
            set;
        }

        public SceneLight()
        {

            Diffuse = new Vector3(1, 1, 1);
            Range = 512;
            ShadowMap = new Texture2D(360, 1,3);



        }


    }
}
