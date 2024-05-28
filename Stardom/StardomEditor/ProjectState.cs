using StardomEngine.GameMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardomEditor
{
    public class ProjectState
    {

        public static GameModule EditModule
        {
            get;
            set;
        }

        public static void LoadIf()
        {
            if(File.Exists("edit/edit.project"))
            {
                EditModule = new GameModule();
                EditModule.Load("edit/edit.project");
            }
        }

        public static void Save()
        {
            EditModule.Save("edit/edit.project");
        }

    }
}
