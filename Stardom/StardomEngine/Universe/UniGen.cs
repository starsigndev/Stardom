using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardomEngine.Universe
{
    public class UniGen
    {

        public List<string> FirstNames
        {
            get;
            set;
        }
        public List<string> Surnames
        {
            get;
            set;
        }

        public List<string> WorldFirst
        {
            get;
            set;
        }

        public List<string> WorldSecond
        {
            get;
            set;
        }

        public List<string> GalxaxyFirst
        {
            get;
            set;
        }

        public List<string> GalaxySecond
        {
            get;
            set;
        }
        Random rnd = new Random(Environment.TickCount);

        public static UniGen This = new UniGen();
        public UniGen()
        {
            This = this;
            FirstNames = File.ReadAllLines("game/data/names/personNames.txt").ToList();
            Surnames = File.ReadAllLines("game/data/names/personSurnames.txt").ToList();
            WorldFirst = File.ReadAllLines("game/data/names/worldnames.txt").ToList();
            WorldSecond = File.ReadAllLines("game/data/names/worldsurnames.txt").ToList();
            GalxaxyFirst = File.ReadAllLines("game/data/names/galaxynames.txt").ToList();
            GalaxySecond = File.ReadAllLines("game/data/names/galaxySurname.txt").ToList();

        }

        public string RandomGalaxyName()
        {

            string name = GalxaxyFirst[rnd.Next(0, GalxaxyFirst.Count)];
            string second = GalaxySecond[rnd.Next(0, GalaxySecond.Count)];

            return name + " " + second;


        }

        public string RandomPlanetName()
        {

            string name = WorldFirst[rnd.Next(0, WorldFirst.Count)];
            string second = WorldSecond[rnd.Next(0, WorldSecond.Count)];

            return name + " " + second;


        }

        public GameUniverse CreateUniverse(int min_g,int max_g,int min_p,int max_p)
        {

            Console.WriteLine("Generating random universe.");

            var universe = new GameUniverse();

            int galaxy_c = rnd.Next(min_g,max_g);

            for(int i = 0; i < galaxy_c; i++)
            {

                var name = RandomGalaxyName();

                Console.WriteLine("Galaxy:" + name);

                int planet_c = rnd.Next(min_p,max_p);

                for(int j=0;j<planet_c; j++)
                {

                    var planet_name = RandomPlanetName();

                    Console.WriteLine("Planet:" + planet_name);

                }


            }

            return universe;

        }

    }
}
