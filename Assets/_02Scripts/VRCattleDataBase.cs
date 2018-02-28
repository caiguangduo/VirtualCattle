using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
using System;
using System.IO;

namespace VRCattle
{
    [Serializable]
    public class Node
    {
        public static int Count = 7;

        [PrimaryKey]
        public string ID { get; set; }
        public string Name_CN { get; set; }
        public string Name_EN { get; set; }
        public string PID { get; set; }
        public string Description { get; set; }
        public string Sound { get; set; }
        public string Acronym { get; set; }

        public Node()
        {

        }

        public Node(string ID,string Name_CN,string Name_EN, string PID, string Description,string Sound,string Acronym)
        {
            this.ID = ID;
            this.Name_CN = Name_CN;
            this.Name_EN = Name_EN;
            this.PID = PID;
            this.Description = Description;
            this.Sound = Sound;
            this.Acronym = Acronym;
        }

        public object this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:return ID;
                    case 1:return Name_CN;
                    case 2:return Name_EN;
                    case 3:return PID;
                    case 4:return Description;
                    case 5:return Sound;
                    case 6:return Acronym;
                }
                return null;
            }
        }

        public static string GetTitle(int index)
        {
            switch (index)
            {
                case 0: return "ID";
                case 1: return "Name_CN";
                case 2: return "Name_EN";
                case 3: return "PID";
                case 4: return "Description";
                case 5: return "Sound";
                case 6: return "Acronym";
            }
            return null;
        }
    }

    public class Tag
    {
        [PrimaryKey]
        public string ID { get; set; }
        public string TagName;
        public string NameCN { get; set; }
        public string NameEN { get; set; }
        public float P_X { get; set; }
        public float P_Y { get; set; }
        public float P_Z { get; set; }
        public float R_X { get; set; }
        public float R_Y { get; set; }
        public float R_Z { get; set; }
        public float S_X { get; set; }
        public float S_Y { get; set; }
        public float S_Z { get; set; }
        public string PID { get; set; }

        public Tag()
        {
        }
        public Tag(string ID,string TagName,string NameCN,string NameEN,float P_X,float P_Y,float P_Z,float R_X,float R_Y,float R_Z,float S_X,float S_Y,float S_Z,string PID)
        {
            this.ID = ID;
            this.TagName = TagName;
            this.NameCN = NameCN;
            this.NameEN = NameEN;
            this.P_X = P_X;
            this.P_Y = P_Y;
            this.P_Z = P_Z;
            this.R_X = R_X;
            this.R_Y = R_Y;
            this.R_Z = R_Z;
            this.S_X = S_X;
            this.S_Y = S_Y;
            this.S_Z = S_Z;
            this.PID = PID;
        }
    }

    public class VRCattleDataBase : MonoBehaviour
    {
        public static VRCattleDataBase instance;
        static SQLiteConnection connection;

        private void Awake()
        {
            if (instance != null)
                Destroy(this);
            else
                instance = this;
            ConnectToDataBase();
        }

        public static void ConnectToDataBase()
        {
            string connectionString = Application.streamingAssetsPath + "/database/VRCattle.db";
            bool hasTable = false;
            if (File.Exists(connectionString))
                hasTable = true;
            connection = new SQLiteConnection(connectionString, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
            if (!hasTable)
            {
                connection.CreateTable<Node>();
                connection.CreateTable<Tag>();
            }
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.C) && Input.GetKeyDown(KeyCode.D))
            {
                connection.DeleteAll<Node>();
            }
            if (Input.GetKey(KeyCode.C) && Input.GetKeyDown(KeyCode.T))
            {
                connection.DeleteAll<Tag>();
            }

        }

        private void OnDestroy()
        {
            CloseConnection();
        }
        public static void CloseConnection()
        {
            connection.Close();
            connection.Dispose();
        }

        public void AddNode(Node node)
        {
            connection.Insert(node);
        }
        public void AddNode(List<Node> list)
        {
            connection.InsertAll(list);
        }

        public void UpdateNode(Node node)
        {
            connection.Update(node);
        }
        public void UpdateNode(List<Node> node)
        {
            connection.UpdateAll(node);
        }

        public Node GetNodeByID(string ID)
        {
            TableQuery<Node> table = connection.Table<Node>().Where(x => x.ID == ID);
            if (table.Count() == 0)
                return null;
            return table.First();
        }

        public List<Node> GetNodeByAcronym(string Acronym)
        {
            return new List<Node>(connection.Table<Node>().Where(x => x.Acronym.StartsWith(Acronym)));
        }

        public List<Node> GetAllNode()
        {
            return new List<Node>(connection.Table<Node>());
        }


        public static void AddTag(Tag tag)
        {
            connection.Insert(tag);
        }
        public static void UpdateTag(Tag tag)
        {
            connection.Update(tag);
        }
        public static List<Tag> GetAllTag()
        {
            return new List<Tag>(connection.Table<Tag>());
        }
        public List<Tag> GetTagByPID(string pid)
        {
            TableQuery<Tag> table = connection.Table<Tag>().Where(x => x.PID == pid);
            if (table.Count() == 0)
                return null;
            else
            {
                int count = table.Count();
                List<Tag> list = new List<Tag>(count);
                for(int i = 0; i < count; i++)
                {
                    list.Add(table.ElementAt(i));
                }
                return list;
            }
        }

        public static List<Tag> GetTagByPidStatic(string pid)
        {
            TableQuery<Tag> table = connection.Table<Tag>().Where(x => x.PID == pid);
            if (table.Count() == 0)
                return null;
            else
            {
                int count = table.Count();
                List<Tag> list = new List<Tag>(count);
                for (int i = 0; i < count; i++)
                {
                    list.Add(table.ElementAt(i));
                }
                return list;
            }
        }
        public static void DeleteTags(List<Tag> tags)
        {
            for(int i = 0; i < tags.Count; i++)
            {
                connection.Delete(tags[i]);
            }
        }
    }
}

