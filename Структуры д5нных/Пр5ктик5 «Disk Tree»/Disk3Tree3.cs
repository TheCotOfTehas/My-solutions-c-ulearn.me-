using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Не сделано
namespace DiskTree
{
    public class Level
    {
        public Dictionary<string, Level> Children;

        public Level(string name, Level endWay)
        {
            Children.Add(name, endWay);
        }

        public Level()
        {
            Children = new Dictionary<string, Level>();
        }
    }

    public class DiskTreeTask
    {
        internal static IEnumerable<string> Solve(List<string> input)
        {
            Level root = new Level();
            foreach (var way in input)
            {
                var receivedListWay = way.Split('\\');
                Level currentRoot = root;
                for (int i = 0; i < receivedListWay.Length; i++)
                {
                    string name = receivedListWay[i];
                    if (!currentRoot.Children.ContainsKey(name))
                        currentRoot.Children[name] = new Level();

                    currentRoot = currentRoot.Children[name];
                }
            }
            List<string> result = new List<string>();
            string currentA = "";
            BuildWay(root, result, currentA);
            return result;
        }

        public static void BuildWay(Level root, List<string> result, string currentA)
        {
            if (root.Children.Count == 0)
                return;

            var listNameRoots = root.Children.Keys.ToList();
            listNameRoots.Sort((one, two) => string.CompareOrdinal(one, two));

            foreach (var item in listNameRoots)
            {
                result.Add(currentA + item);
                BuildWay(root.Children[item], result, currentA + " ");
            }
        }
    }
}