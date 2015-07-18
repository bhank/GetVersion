using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace GetVersion
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length != 1)
            {
                Console.Error.WriteLine("Pass it a file to get version information.");
                Environment.Exit(1);
            }

            string filename = args[0];

            if (!File.Exists(filename))
            {
                Console.Error.WriteLine("File does not exist: " + filename);
                Environment.Exit(2);
            }

            string dotnetversion = null;
            try
            {
                Assembly assembly = Assembly.ReflectionOnlyLoadFrom(filename); // LoadFile requires an absolute path
                dotnetversion = assembly.GetName().Version.ToString();
            }
            catch (BadImageFormatException e)
            {
                if (e.Message.Contains("This assembly is built by a runtime newer than the currently loaded runtime and cannot be loaded."))
                {
                    Console.Error.WriteLine("Failed to load assembly version: " + e.Message);
                }
            }
            catch (Exception e)
            {
            }

            if(dotnetversion != null)
            {
                Console.WriteLine(" .NET Assembly Version: " + dotnetversion);
            }

            FileVersionInfo fileVersionInfo = null;
            try
            {
                fileVersionInfo = FileVersionInfo.GetVersionInfo(filename);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("GetVersionInfo failed: " + e.Message);
                Environment.Exit(3);
            }

            string fileVersionNumbers = string.Format("{0}.{1}.{2}.{3}", fileVersionInfo.FileMajorPart, fileVersionInfo.FileMinorPart, fileVersionInfo.FileBuildPart, fileVersionInfo.FilePrivatePart);
            Console.WriteLine("          File Version: " + fileVersionNumbers);
            if (fileVersionInfo.FileVersion != fileVersionNumbers)
            {
                Console.WriteLine("   File Version String: " + fileVersionInfo.FileVersion);
            }

            string productVersionNumbers = string.Format("{0}.{1}.{2}.{3}", fileVersionInfo.ProductMajorPart, fileVersionInfo.ProductMinorPart, fileVersionInfo.ProductBuildPart, fileVersionInfo.ProductPrivatePart);
            Console.WriteLine("       Product Version: " + productVersionNumbers);
            if (fileVersionInfo.ProductVersion != productVersionNumbers)
            {
                Console.WriteLine("Product Version String: " + fileVersionInfo.ProductVersion);
            }

            if (!string.IsNullOrEmpty(fileVersionInfo.ProductName))
            {
                Console.WriteLine();
                Console.WriteLine("Product Name: " + fileVersionInfo.ProductName);
            }
            if (!string.IsNullOrEmpty(fileVersionInfo.Comments))
            {
                Console.WriteLine();
                Console.WriteLine("Comments: " + fileVersionInfo.Comments);
            }
        }
    }
}
