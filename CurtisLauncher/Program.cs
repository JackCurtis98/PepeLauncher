using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace CurtisLauncher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DllInjector procInjector = new DllInjector();
            List<Process> processList = new List<Process>();
            int instances;
            var injectedProcs = new List<int>();

            Console.WriteLine($"          ████████     ██████\r\n         █▓▓▓▓▓▓▓▓██ ██▓▓▓▓▓▓█\r\n        █▓▓▓▓▓▓▓▓▓▓▓█▓▓▓▓▓▓▓▓▓█\r\n       █▓▓▓▓▓▓▓███▓▓▓█▓▓▓▓▓▓▓▓▓█\r\n       █▓▓▓▓███▓▓▓███▓█▓▓▓████▓█\r\n      █▓▓▓██▓▓▓▓▓▓▓▓███▓██▓▓▓▓██\r\n     █▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓█▓▓▓▓▓▓▓▓███\r\n    █▓▓▓▓▓▓▓▓▓▓▓▓▓██████▓▓▓▓▓████▓▓█\r\n    █▓▓▓▓▓▓▓▓▓█████▓▓▓████▓▓██▓▓██▓▓█\r\n   ██▓▓▓▓▓▓▓███▓▓▓▓▓▓▓▓▓▓█▓▓▓▓▓▓▓▓███\r\n  █▓▓▓▓▓▓▓▓▓▓▓▓▓▓█████████▓▓█████████\r\n █▓▓▓▓▓▓▓▓▓▓█████ ████   ████ █████  █\r\n █▓▓▓▓▓▓▓▓▓▓█     █ ███  █    ███ █   █\r\n█▓▓▓▓▓▓▓▓▓▓▓▓█   ████ ████    ██ ██████\r\n▓▓▓▓▓▓▓▓▓▓▓▓▓█████████▓▓▓████████▓▓▓█\r\n▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓█▓▓▓▓▓█▓▓▓▓▓▓▓▓▓▓▓▓█\r\n▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓██▓▓▓▓█▓▓▓▓▓▓██\r\n▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓██▓▓▓▓▓▓▓███████\r\n▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓██▓▓▓▓▓▓▓▓▓▓█▓▓▓▓▓█\r\n▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓█\r\n▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓█\r\n▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓█\r\n▓▓▓▓▓▓▓▓▓▓▓█████████▓▓▓▓▓▓▓▓▓▓▓▓▓▓██\r\n▓▓▓▓▓▓▓▓▓▓█▒▒▒▒▒▒▒▒███████████████▒▒█\r\n▓▓▓▓▓▓▓▓▓█▒▒███████▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒█\r\n▓▓▓▓▓▓▓▓▓█▒▒▒▒▒▒▒▒▒█████████████████\r\n▓▓▓▓▓▓▓▓▓▓████████▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒█\r\n▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓██████████████████\r\n▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓█\r\n██▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓██\r\n▓██▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓██\r\n▓▓▓███▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓█\r\n▓▓▓▓▓▓███▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓██\r\n▓▓▓▓▓▓▓▓▓███████████████▓▓█\r\n▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓██\r\n▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓█\r\n▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓█");
            Console.WriteLine("Welcome to the shitty launcher...");
            Console.Write("How many instances are you wanting to run?: ");
            while (!int.TryParse(Console.ReadLine(), out instances)) Console.WriteLine("Integers only allowed."); // This line will do the trick

            for (int i = 0; i < instances; i++)
            {
                //Start shit i guess
                var proc = Process.Start("C:\\ProgramData\\Jagex\\launcher\\rs2client.exe");

                while (processList.Count == 0)
                {
                    System.Threading.Thread.Sleep(1500);
                    var newProc = Process.GetProcessesByName("Runescape").Where(process => !injectedProcs.Contains(process.Id)).First();
                    injectedProcs.Add(newProc.Id);
                    System.Threading.Thread.Sleep(1500);
                    processList = (List<Process>)Util.GetChildProcesses(newProc);
                    System.Threading.Thread.Sleep(3000);
                }

                var rs2ClientProc = processList.First();
                var injectionStatus = procInjector.Inject(rs2ClientProc.Id, Directory.GetCurrentDirectory() + "\\MemoryError.dll");
                processList.Clear(); // Reset list for next iteration JANKY ASF IK :)

                if (injectionStatus == DllInjectionResult.Success)
                {
                    Console.WriteLine("Successfully injected into RS Instance PID: " + rs2ClientProc.Id);
                }
                else
                {
                    Console.WriteLine("Failure to inject DLL into RS Instance PID: " + rs2ClientProc.Id + " FAILURE: " + injectionStatus.ToString());
                }
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }

    public static class Util
    {
        public static IList<Process> GetChildProcesses(Process process)
            => new ManagementObjectSearcher(
                    $"Select * From Win32_Process Where ParentProcessID={process.Id}")
                .Get()
                .Cast<ManagementObject>()
                .Select(mo =>
                    Process.GetProcessById(Convert.ToInt32(mo["ProcessID"])))
                .ToList();
    }
}
