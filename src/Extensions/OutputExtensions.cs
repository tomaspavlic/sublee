using System.Drawing;
using Console = Colorful.Console;

namespace Topdev.Sublee.Cli.Extensions
{
    public static class OutputExtensions
    {
        public static void WriteInfo(string value)
        {
            Console.Write("[*] ", Color.Blue);
            Console.WriteLine(value);
        }

        public static void WriteWarning(string value)
        {
            Console.Write("[!] ", Color.Yellow);
            Console.WriteLine(value);
        }

        public static void WriteError(string value)
        {
            Console.Write("[-] ", Color.DarkRed);
            Console.WriteLine(value);
        }

        public static void WriteVerbose(string value)
        {
            Console.Write("[+] ", Color.Orange);
            Console.WriteLine(value);
        }
    }
}