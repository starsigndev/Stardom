using StardomEngine.Resonance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardomEngine.App
{
    public class AppState
    {

        public GameUI UI
        {
            get;
            set;
        }

        public AppState()
        {

            UI = new GameUI();

        }

        public virtual void InitState()
        {

        }

        public virtual void UpdateState()
        {

        }

        public virtual void RenderState()
        {

        }

    }
}
