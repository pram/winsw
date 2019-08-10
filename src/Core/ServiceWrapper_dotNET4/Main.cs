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

        [Option(shortName: 'u', longName: "username", Required = false, HelpText = "Set the username")]
        public string username { get; set; }

        [Option(shortName: 'p', longName: "password", Required = false, HelpText = "Set the password")]
        public string password { get; set; }
    }

    /// <summary>
    /// Just a wrapper class, which redirects the Main entry point to the WinSW main method.
    /// </summary>
    public class dotNET4Support
    {
        public static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                throw new Exception("Please supply an action for the service");
            }
            
            Arguments arguments = new Arguments();
            arguments.Action = args[0];

            Parser.Default.ParseArguments<Options>(args).WithParsed<Options>(o =>
            {
                if (!String.IsNullOrEmpty(o.username))
                {
                    arguments.username = o.username;
                }

                if (!String.IsNullOrEmpty(o.password))
                {
                    arguments.password = o.password;
                }
                
                if (o.Verbose)
                {
                    Console.WriteLine($"Verbose output enabled. Current Arguments: -v {o.Verbose}");
                    Console.WriteLine("Quick Start Example! App is in Verbose mode!");
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