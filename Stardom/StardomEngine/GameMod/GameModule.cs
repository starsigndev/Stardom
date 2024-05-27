using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardomEngine.GameMod
{
    public class GameModule
    {

        public string ModName
        {
            get;set;
        }

        public string ModAuthor
        {
            get;
            set;
        }

        public string ModStarSystem
        {
            get;
            set;
        }
        public void Load(string path)
        {
            FileStream fs = new(path,FileMode.Open,FileAccess.Read);
            BinaryReader r = new(fs);

            LoadDetails(r);

            fs.Close();
        }
        public void Save(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            BinaryWriter w = new(fs);

            SaveDetails(w);

            w.Flush();
            fs.Flush();
            fs.Close();
        }

        void LoadDetails(BinaryReader r)
        {
            ModName = r.ReadString();
            ModAuthor = r.ReadString();
            ModStarSystem = r.ReadString();
        }
        void SaveDetails(BinaryWriter w)
        {

            w.Write(ModName);
            w.Write(ModAuthor);
            w.Write(ModStarSystem);

        }


    }
}
