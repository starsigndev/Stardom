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
