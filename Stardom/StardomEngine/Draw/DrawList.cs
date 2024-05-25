using StardomEngine.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardomEngine.Draw
{
    public class DrawList
    {

        public List<DrawCall> Calls { get; set; }
        public float[] v_data = new float[1024 * 1008];
        public uint[] i_data = new uint[1024 * 1008];
        public int dataIndex = 0;
        public uint indexIndex = 0;
        public int CallsNum = 0;
        public Texture2D Image, Normals;

        public DrawList()
        {

            Calls = new List<DrawCall>();

        }

        public void AddCall(DrawCall call)
        {

            Calls.Add(call);

        }

    }
}
