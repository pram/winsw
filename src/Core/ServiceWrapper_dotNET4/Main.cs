using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;

namespace winsw.dotNET4
{
    public class Options
    {
        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }
    }
    /// <summary>
    /// Just a wrapper class, which redirects the Main entry point to the WinSW main method.
    /// </summary>
    public class dotNET4Support
    {
        public static int Main(string[] args)
        {
            Arguments arguments = new Arguments();
            
            Parser.Default.ParseArguments<Options>(args).WithParsed<Options>(o =>
                {
                    if (o.Verbose)
                    {
                        Console.WriteLine($"Verbose output enabled. Current Arguments: -v {o.Verbose}");
                        Console.WriteLine("Quick Start Example! App is in Verbose mode!");
                        arguments.Action = "sdsd";
                    }
                    else
                    {
                        Console.WriteLine($"Current Arguments: -v {o.Verbose}");
                        Console.WriteLine("Quick Start Example!");
                    }
                });
        
            winsw.WrapperService.Run(arguments);

            return 0;
        }
    }
}
