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
            List<string> matchedFiles = new List<string>();
            List<List<string>> allFiles = new List<List<string>>();

            // iterate in Options.Targets to get all files in its
            foreach (string target in Options.Targets)
            {
                // ensure that all pathes contain '//' in the end of it
                string path=target.TrimEnd('\\');
                path += "\\";

                // get all files in directory target and all subdirectoies
                string[] files=Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);

                // add files name ( replace the directory name ) to all files list
                allFiles.Add(files.Select(sub=>sub.Substring(path.Length)).ToList<string>());
            }

            // compare allfiles to get all repeated files
            foreach (string file in allFiles[0])
            {
                bool exists = true;
                for (int i = 1; i < allFiles.Count; i++)
                    exists = exists && allFiles[i].Contains(file);
                if (exists)
                    matchedFiles.Add(file);
            }

            return matchedFiles;
        }
    }
}
