using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;
using System.IO;

namespace Develapp.Watcher
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();
            ICommandLineParser parser = new CommandLineParser(new CommandLineParserSettings(Console.Error));
            if (!parser.ParseArguments(args, options))
                Environment.Exit(1);

            //Console.WriteLine("Test : "+options.Test);
            //Console.WriteLine("Execute : "+options.Execute);
            //Console.WriteLine("List : "+options.List);
            //for (int i = 0; i < options.Targets.Count; i++)
            //    Console.WriteLine("Target" + (i + 1).ToString() + " : " + options.Targets[i]);

            Watcher watcher = new Watcher(options);
            watcher.Run();

            Console.ReadLine();
            Environment.Exit(0);
        }
    }
}
