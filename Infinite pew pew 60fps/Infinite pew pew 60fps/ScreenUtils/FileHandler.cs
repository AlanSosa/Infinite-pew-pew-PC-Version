using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.IsolatedStorage;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using Infinite_pew_pew.ScreenManager;

namespace GameStateManagement.ScreensUtils
{

    public static class FileHandler
    {
        private static string highScoreFilename = "highscore.pewpew";
        private static string configFilename = "config.pewpew";

        public static void WriteConfigFile(ConfigData config)
        {
            // Open the file, creating it if necessary
            FileStream stream = File.Open(configFilename, FileMode.OpenOrCreate);
            try
            {
                // Convert the object to XML data and put it in the stream
                XmlSerializer serializer = new XmlSerializer(typeof(ConfigData));
                serializer.Serialize(stream, config);
            }
            finally
            {
                // Close the file
                stream.Close();
            }
        }

        public static ConfigData GetConfigData()
        {
            FileStream stream;
            ConfigData data;
            using (stream = File.Open(configFilename, FileMode.OpenOrCreate, FileAccess.Read))
            {
                if (stream != null)
                {
                    try
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ConfigData));
                        data = (ConfigData)serializer.Deserialize(stream);
                        return data;
                    }
                    catch(Exception e)
                    {
                        Logger.log( Log_Type.ERROR, " File Hander.cs : GetConfigData() : Error : " + e.ToString() +" : Explanation : "+ e.Message);
                        data = new ConfigData();
                        data.Add(true);
                        data.Add(true);
                        data.Add(false);
                        data.Add(true);
                        return data;
                    }
                }
                else
                {
                    data = new ConfigData();
                    data.Add(true);
                    data.Add(true);
                    data.Add(false);
                    data.Add(true);
                    return data;
                }
            }
        }

        public static void SaveHighScores(PlayersList players)
        {
            // Open the file, creating it if necessary
            FileStream stream = File.Open(highScoreFilename, FileMode.OpenOrCreate);
            try
            {
                // Convert the object to XML data and put it in the stream
                XmlSerializer serializer = new XmlSerializer(typeof(PlayersList));
                serializer.Serialize(stream, players);
            }
            finally
            {
                // Close the file
                stream.Close();
            }
        }

        public static PlayersList LoadHighScores()
        {
            FileStream stream;
            PlayersList data;
            using (stream = File.Open(highScoreFilename, FileMode.OpenOrCreate, FileAccess.Read))
            {
                if (stream != null)
                {
                    try
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(PlayersList));

                        data = (PlayersList)serializer.Deserialize(stream);
                        return data;
                    }
                    catch(Exception e)
                    {
                        Logger.log(Log_Type.ERROR, " File Hander.cs : GetConfigData() : Error : " + e.ToString() + " : Explanation : " + e.Message);

                        data = new PlayersList();
                        data.Add(new Player { Name = "Fulano", Score = 6000 });
                        data.Add(new Player { Name = "Compadre", Score = 7000 });
                        data.Add(new Player { Name = "AnotherDude", Score = 8000 });
                        data.Add(new Player { Name = "Dude", Score = 9000 });
                        data.Add(new Player { Name = "Bacon Lover", Score = 10000 });
                        return data;
                    }
                }
                else
                {
                    data = new PlayersList();
                    data.Add(new Player { Name = "Fulano", Score = 6000 });
                    data.Add(new Player { Name = "Compadre", Score = 7000 });
                    data.Add(new Player { Name = "AnotherDude", Score = 8000 });
                    data.Add(new Player { Name = "Dude", Score = 9000 });
                    data.Add(new Player { Name = "Bacon Lover", Score = 10000 });
                    return data;
                }
            }
        }
    }
    public class Player
    {
        public string Name { get; set; }
        public int Score { get; set; }
    }

    [Serializable]
    public class PlayersList : List<Player>
    {

    }

    [Serializable]
    public class ConfigData : List<bool>
    {
    }
}
