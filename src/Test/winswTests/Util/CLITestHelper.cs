﻿using System;
using System.IO;
using JetBrains.Annotations;
using winsw;

namespace winswTests.Util
{
    /// <summary>
    /// Helper for WinSW CLI testing
    /// </summary>
    public static class CLITestHelper
    {
        private const string SeedXml = "<service>"
                                    + "<id>service.exe</id>"
                                    + "<name>Service</name>"
                                    + "<description>The service.</description>"
                                    + "<executable>node.exe</executable>"
                                    + "<arguments>My Arguments</arguments>"
                                    + "<logmode>rotate</logmode>"
                                    + "<workingdirectory>"
                                    + @"C:\winsw\workdir"
                                    + "</workingdirectory>"
                                    + @"<logpath>C:\winsw\logs</logpath>"
                                    + "</service>";
        private static readonly ServiceDescriptor DefaultServiceDescriptor = ServiceDescriptor.FromXML(SeedXml);

        /// <summary>
        /// Runs a simle test, which returns the output CLI
        /// </summary>
        /// <param name="args">CLI arguments to be passed</param>
        /// <param name="descriptor">Optional Service descriptor (will be used for initializationpurposes)</param>
        /// <returns>STDOUT if there's no exceptions</returns>
        /// <exception cref="Exception">Command failure</exception>
        [NotNull]
        public static string CLITest(String[] args, ServiceDescriptor descriptor = null)
        {
            using (StringWriter sw = new StringWriter())
            {
                Arguments arguments = new Arguments();
                arguments.Action = args[0];
                TextWriter tmp = Console.Out;
                Console.SetOut(sw);
                WrapperService.Run(arguments, descriptor ?? DefaultServiceDescriptor);
                Console.SetOut(tmp);
                Console.Write(sw.ToString());
                return sw.ToString();
            }
        }

        /// <summary>
        /// Runs a simle test, which returns the output CLI
        /// </summary>
        /// <param name="args">CLI arguments to be passed</param>
        /// <param name="descriptor">Optional Service descriptor (will be used for initializationpurposes)</param>
        /// <returns>Test results</returns>
        [NotNull]
        public static CLITestResult CLIErrorTest(String[] args, ServiceDescriptor descriptor = null)
        {
            Arguments arguments = new Arguments();
            arguments.Action = args[0];
            StringWriter swOut, swErr;
            Exception testEx = null;
            TextWriter tmpOut = Console.Out;
            TextWriter tmpErr = Console.Error;

            using (swOut = new StringWriter())
            using (swErr = new StringWriter())
                try
                {              
                    Console.SetOut(swOut);
                    Console.SetError(swErr);
                    WrapperService.Run(arguments, descriptor ?? DefaultServiceDescriptor);
                }
                catch (Exception ex)
                {
                    testEx = ex;
                }
                finally
                {
                    Console.SetOut(tmpOut);
                    Console.SetError(tmpErr);
                    Console.WriteLine("\n>>> Output: ");
                    Console.Write(swOut.ToString());
                    Console.WriteLine("\n>>> Error: ");
                    Console.Write(swErr.ToString());
                    if (testEx != null)
                    {
                        Console.WriteLine("\n>>> Exception: ");
                        Console.WriteLine(testEx);
                    }
                }
                
            return new CLITestResult(swOut.ToString(), swErr.ToString(), testEx);
        }
    }

    /// <summary>
    /// Aggregated test report
    /// </summary>
    public class CLITestResult
    {
        [NotNull]
        public String Out { get; private set; }
        
        [NotNull]
        public String Err { get; private set; }
        
        [CanBeNull]
        public Exception Exception { get; private set; }

        public bool HasException { get { return Exception != null; } }

        public CLITestResult(String output, String err, Exception exception = null)
        {
            Out = output;
            Err = err;
            Exception = exception;
        }
    }
}
