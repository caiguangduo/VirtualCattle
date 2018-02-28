using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace VRCattle
{
    public class VRCattleExcelAPI : MonoBehaviour
    {
#if UNITY_EDITOR
        public List<Node> list = new List<Node>();

        private void Update()
        {
            if (Input.GetKey(KeyCode.Z) && Input.GetKeyDown(KeyCode.O))
            {
                string[] strs = WindowDll.ShowOpenFileDialog("打开Excel文件", "Excel files (*.xlsx)|*.xlsx", false);
                if (strs != null)
                {
                    Read(strs[0]);
                }
            }

            //if (Input.GetKey(KeyCode.Z) && Input.GetKeyDown(KeyCode.S))
            //{
            //    string path = WindowDll.ShowSaveFileDialog("打开Excel文件", "Excel files (*.xlsx|*.xlsx", ".xlsx");
            //    if(path!=null)
            //    {
            //        Write(path);
            //    }
            //}

            //if (Input.GetKey(KeyCode.O) && Input.GetKeyDown(KeyCode.P))
            //{
            //    List<Node> allNodes = VRCattleDataBase.instance.GetAllNode();
            //    for(int i = 0; i < allNodes.Count; i++)
            //    {
            //        Debug.Log(allNodes[i].ID);
            //    }
            //}
        }

        private void Read(string path)
        {
            FileInfo info = new FileInfo(path);
            using (ExcelPackage package = new ExcelPackage(info))
            {
                ExcelWorksheet ws = package.Workbook.Worksheets[1];
                int minRowNum = ws.Dimension.Start.Row;
                int maxRowNum = ws.Dimension.End.Row;
                for (int i = minRowNum + 1; i <= maxRowNum; i++)
                {
                    string ID = ws.Cells[i, 1].Text;
                    if (string.IsNullOrEmpty(ID)) continue;
                    string Name_CN = ws.Cells[i, 2].Text;
                    string Name_EN = ws.Cells[i, 3].Text;
                    string PID = ws.Cells[i, 4].Text;
                    string Description = ws.Cells[i, 5].Text;
                    string Sound = ws.Cells[i, 6].Text;
                    string Acronym = ws.Cells[i, 7].Text;
                    Node node = new Node(ID, Name_CN, Name_EN, PID, Description, Sound, Acronym);
                    list.Add(node);
                }

                List<Node> InsertList = new List<Node>();
                List<Node> UpdateList = new List<Node>();
                List<Node> allNodes = VRCattleDataBase.instance.GetAllNode();

                for(int i = 0; i < list.Count; i++)
                {
                    int j = 0;
                    for (; j < allNodes.Count; j++)
                    {
                        if (list[i].ID == allNodes[j].ID)
                        {
                            UpdateList.Add(list[i]);
                            break;
                        }
                    }
                    if (j == allNodes.Count)
                    {
                        InsertList.Add(list[i]);
                    }
                }

                VRCattleDataBase.instance.AddNode(InsertList);
                VRCattleDataBase.instance.UpdateNode(UpdateList);
            } 
        }

        private void Write(string filePath)
        {
            FileInfo info = new FileInfo(filePath);
            if (info.Exists)
            {
                info.Delete();
                info = new FileInfo(filePath);
            }

            using(ExcelPackage package=new ExcelPackage(info))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");
                List<Node> nodes = VRCattleDataBase.instance.GetAllNode();
                for(int i = 0; i < Node.Count; i++)
                {
                    for (int j = 0; j < nodes.Count + 1; j++)//行
                    {
                        if (j == 0) worksheet.Cells[j + 1, i + 1].Value = Node.GetTitle(i);
                        else worksheet.Cells[j + 1, i + 1].Value = nodes[j - 1][i];
                        worksheet.Cells[j + 1, i + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    }
                    worksheet.Column(i + 1).AutoFit();
                }
                worksheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                package.Save();
            }
            Application.OpenURL(filePath);
        }
#endif
    }
}

