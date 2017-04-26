using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


namespace Hunter.GameData
{
    sealed class Vector2SerializationSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            Vector2 v2 = (Vector2)obj;
            info.AddValue("x", v2.x);
            info.AddValue("y", v2.y);
        }

        public object SetObjectData(object obj, System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context, System.Runtime.Serialization.ISurrogateSelector selector)
        {
            Vector2 v2 = (Vector2)obj;
            v2.x = (float)info.GetValue("x", typeof(float));
            v2.y = (float)info.GetValue("y", typeof(float));

            return (object)v2;
        }
    }

    public interface IDataIOMgr
    {
        bool LoadData();
        bool SaveData(IDataManager data);

        event Action LoadDataFinish;
        event Action SaveDataStart;
    }

    public class DataIOMgr : IDataIOMgr
    {
        IDataManager _dataMgr;

        private bool isLoadDataFinish = false;
        private event Action loadDataFinish;
        public event Action LoadDataFinish 
        {
            add
            {
                if (!isLoadDataFinish)
                    loadDataFinish += value;
                else
                {
                    loadDataFinish = value;
                    loadDataFinish();
                }
            }
            remove
            {
                loadDataFinish -= value;
            }
        }

        private event Action saveDataStart;
        public event Action SaveDataStart
        {
            add
            {
                saveDataStart += value;

            }
            remove
            {
                saveDataStart -= value;
            }
        }

        DataIOMgr(IDataManager dataMgr)
        {
            _dataMgr = dataMgr;
        }

        public bool SaveData(IDataManager data)
        {
            if (saveDataStart != null)
            {
                saveDataStart();
            }

            IFormatter formatter = new BinaryFormatter();
            SurrogateSelector ss = new SurrogateSelector();
            Vector2SerializationSurrogate v2Surrogate = new Vector2SerializationSurrogate();
            ss.AddSurrogate(typeof(Vector2), new StreamingContext(StreamingContextStates.All), v2Surrogate);

            formatter.SurrogateSelector = ss;

            Stream stream = new FileStream(Hunter.Utility.FileUtility.GetPersistentFilePath("hunter_game_data.bin"), FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, data);
            stream.Close();
            return true;
        }

        public bool LoadData()
        {
            DataManager data = null;
            if (File.Exists(Hunter.Utility.FileUtility.GetPersistentFilePath("hunter_game_data.bin")))
            { 
                IFormatter formatter = new BinaryFormatter();
                SurrogateSelector ss = new SurrogateSelector();
                Vector2SerializationSurrogate v2Surrogate = new Vector2SerializationSurrogate();
                ss.AddSurrogate(typeof(Vector2), new StreamingContext(StreamingContextStates.All), v2Surrogate);

                formatter.SurrogateSelector = ss;
                Stream stream = new FileStream(Hunter.Utility.FileUtility.GetPersistentFilePath("hunter_game_data.bin"), FileMode.Open, FileAccess.Read, FileShare.Read);
                data = (DataManager)formatter.Deserialize(stream);
                stream.Close();
            }
            Assert.IsTrue(_dataMgr.LoadAllData(data));
            isLoadDataFinish = true;
            if (loadDataFinish != null)
            {
                loadDataFinish();
            }
            return true;
        }
    }
}

