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

        private List<string> CompareTargets()
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

        /// <summary>
        /// Main Run Watcher , that check if execution environment it will sync changes , 
        /// if test environment it will write matched files in screen
        /// </summary>
        public void Run()
        {
            if (!Options.Execute)
            {
                if(Options.List)
                    WriteToScreen();
            }
            else
            {
                if (Options.List)
                    WriteToScreen();
                System.Threading.Thread.Sleep(1000);
                Syncronize();
                Console.WriteLine("Suncronize Successfuly.");
            }
        }

        /// <summary>
        /// Syncronize targets
        /// </summary>
        private void Syncronize()
        {
            // Progress for user interaction
            int length = CompareTargets().Count;
            int i = 1;

            foreach (string file in CompareTargets())
            {
                string newestFilePath=null;
                DateTime newestModificationDatetime = DateTime.MinValue;

                // iterate the target with to get the fresh matched file
                foreach (string target in Options.Targets)
                {
                    // ensure that all pathes contain '//' in the end of it
                    string path = target.TrimEnd('\\');
                    path += "\\";
                    string fullPath = path + file;

                    DateTime modificationTime = File.GetLastWriteTime(fullPath);
                    if (modificationTime > newestModificationDatetime)
                    {
                        newestModificationDatetime = modificationTime;
                        newestFilePath = fullPath;
                    }
                }

                // iterate the target to replace the old files with the newest one
                foreach (string target in Options.Targets)
                {
                    // ensure that all pathes contain '//' in the end of it
                    string path = target.TrimEnd('\\');
                    path += "\\";
                    string fullPath = path + file;

                    if (newestFilePath != fullPath)
                    {
                        File.Copy(newestFilePath, fullPath, true);
                    }
                }

                // Progress for user interaction
                Console.Clear();
                int step = (i * 100) / length;
                Console.WriteLine("Progress {0}%",step.ToString());
                i++; System.Threading.Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// Write CompareTarget() result in file
        /// </summary>
        private void WriteToFile()
        {
            Console.Write("Please enter the file path : ");
            string filePath = Console.ReadLine();
            FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stream);
            foreach (string file in this.CompareTargets())
                writer.WriteLine(file);
            writer.Close();
            Console.WriteLine("The CompareResult is saved successfuly in {0}", filePath);
        }

        /// <summary>
        /// Write CompareTarget() result in file
        /// </summary>
        private void WriteToScreen()
        {
            foreach (string file in this.CompareTargets())
                Console.WriteLine(file);
        }
    }
}
