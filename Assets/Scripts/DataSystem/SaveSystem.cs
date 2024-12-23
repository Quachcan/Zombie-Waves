using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace DataSystem
{
    public static class SaveSystem
    {
        private static string GetSavePath()
        {
            return Application.persistentDataPath + "/AutoSave1.save";
        }

        public static void SavePlayer(PlayerScripts.Player player)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = GetSavePath();
            FileStream stream = new FileStream(path, FileMode.Create);

            PlayerData data = new PlayerData(player);

            formatter.Serialize(stream, data);
            stream.Close();

            Debug.Log("Game saved at " + path);
        }

        public static PlayerData LoadPlayer()
        {
            string path = GetSavePath();
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                PlayerData data = formatter.Deserialize(stream) as PlayerData;
                stream.Close();

                Debug.Log("Game loaded from " + path);
                return data;
            }
            else
            {
                Debug.LogError("Save file not found in " + path);
                return null;
            }
        }
    }
}