using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Develapp.Watcher
{
    public class Watcher
    {
        public Options Options { get; private set; }

        public Watcher(Options options)
        {
            Options = options;
        }

        public List<string> CompareTargets()
        {
            //use options.Targets and list all matching files
            List<string> returned = new List<string>();
            List<List<string>> allFiles = new List<List<string>>();

            foreach (string s in Options.Targets)
            {
                string ss=s.TrimEnd('\\');
                ss += "\\";
                string[] sss=Directory.GetFiles(ss, "*.*", SearchOption.AllDirectories);
                allFiles.Add(sss.Select(sub=>sub.Substring(ss.Length)).ToList<string>());
            }

            foreach (string s in allFiles[0])
            {
                bool existed = true;
                for (int i = 1; i < allFiles.Count; i++)
                    existed = existed && allFiles[i].Contains(s);
                if (existed)
                    returned.Add(s);
            }

            return returned;
        }
    }
}
