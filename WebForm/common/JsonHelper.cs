﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Common
{
    public class JsonHelper
    {
        /// <summary>
        /// 对象转JSON
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>JSON格式的字符串</returns>
        public static string ObjectToJSON(object obj)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                return jss.Serialize(obj);
            }
            catch (Exception ex)
            {

                throw new Exception("JSONHelper.ObjectToJSON(): " + ex.Message);
            }
        }

        /// <summary>
        /// 数据表转键值对集合
        /// 把DataTable转成 List集合, 存每一行
        /// 集合中放的是键值对字典,存每一列
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <returns>哈希表数组</returns>
        public List<Dictionary<string, object>> DataTableToList(DataTable dt)
        {
            List<Dictionary<string, object>> list
                 = new List<Dictionary<string, object>>();

            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    dic.Add(dc.ColumnName, dr[dc.ColumnName]);
                }
                list.Add(dic);
            }
            return list;
        }

        /// <summary>
        /// 数据集转键值对数组字典
        /// </summary>
        /// <param name="dataSet">数据集</param>
        /// <returns>键值对数组字典</returns>
        public Dictionary<string, List<Dictionary<string, object>>> DataSetToDic(DataSet ds)
        {
            Dictionary<string, List<Dictionary<string, object>>> result = new Dictionary<string, List<Dictionary<string, object>>>();

            foreach (DataTable dt in ds.Tables)
                result.Add(dt.TableName, DataTableToList(dt));

            return result;
        }

        /// <summary>
        /// 数据表转JSON
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <returns>JSON字符串</returns>
        public string DataTableToJSON(DataTable dt)
        {
            return ObjectToJSON(DataTableToList(dt));
        }

        /// <summary>
        /// 描述：数据表转适合ligerGuid的JSON
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <returns>JSON字符串</returns>
        public string GetJSONForLigerGrid(DataTable dt, int total)
        {
            string strRtn = this.DataTableToJSON(dt);

            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{\"Rows\":");
            jsonBuilder.Append(strRtn);
            jsonBuilder.Append(",\"Total\":" + total.ToString());
            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }

        /// <summary>
        /// JSON文本转对象,泛型方法
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="jsonText">JSON文本</param>
        /// <returns>指定类型的对象</returns>
        public T JSONToObject<T>(string jsonText)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                return jss.Deserialize<T>(jsonText);
            }
            catch (Exception ex)
            {
                throw new Exception("JSONHelper.JSONToObject(): " + ex.Message);
            }
        }

        /// <summary>
        /// 将JSON文本转换为数据表数据
        /// </summary>
        /// <param name="jsonText">JSON文本</param>
        /// <returns>数据表字典</returns>
        public Dictionary<string, List<Dictionary<string, object>>> TablesDataFromJSON(string jsonText)
        {
            return JSONToObject<Dictionary<string, List<Dictionary<string, object>>>>(jsonText);
        }

        /// <summary>
        /// 将JSON文本转换成数据行
        /// </summary>
        /// <param name="jsonText">JSON文本</param>
        /// <returns>数据行的字典</returns>
        public Dictionary<string, object> DataRowFromJSON(string jsonText)
        {
            return JSONToObject<Dictionary<string, object>>(jsonText);
        }

        /// <summary>
        /// 序列化对象为json
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">对象</param>
        /// <returns>返回 json串</returns>
        public static string Serialize<T>(T obj)
        {
            var dtConverter = new IsoDateTimeConverter();
            dtConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return JsonConvert.SerializeObject(obj, dtConverter);
        }
        public static string Serialize<T>(List<T> objlist)
        {
            var dtConverter = new IsoDateTimeConverter();
            dtConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return JsonConvert.SerializeObject(objlist, dtConverter);
        }
        public static string Serialize(DataTable dt)
        {
            var dtConverter = new IsoDateTimeConverter();
            dtConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return JsonConvert.SerializeObject(dt, dtConverter);
        }
        public static string SerializeObj(object parmObj)
        {
            return JsonConvert.SerializeObject(parmObj);
        }

        /// <summary>
        ///   LIST转换成 JSON
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="objlist">对象列表</param>
        /// <param name="RowTotal">总行数</param>
        /// <param name="PageIndex">页数</param>
        /// <returns></returns>
        public static string Serialize<T>(List<T> objlist, int RowTotal, int PageIndex)
        {
            var dtConverter = new IsoDateTimeConverter();
            dtConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            string returnvalue = "{\"Rows\":";
            returnvalue += JsonConvert.SerializeObject(objlist, dtConverter);
            return returnvalue + ",\"Total\":" + RowTotal + "}";
        }

        /// <summary>
        /// 渲染成表格的JSON对象
        /// </summary>
        /// <param name="dt">表</param>
        /// <param name="totalRows"></param>
        /// <param name="curPageNo"></param>
        /// <param name="totalPages"></param>
        /// <param name="pageRows"></param>
        /// <returns></returns>
        public static String ToJsonGrid(DataTable dt, int totalRows, int curPageNo, int totalPages, int pageRows)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{\"Rows\":");
            jsonBuilder.Append(Serialize(dt));
            jsonBuilder.Append(",\"Total\":" + totalRows);
            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }

        public static String ToJsonGridAll(DataTable dt, int totalRows)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{\"Rows\":");
            jsonBuilder.Append(Serialize(dt));
            jsonBuilder.Append(",\"Total\":" + totalRows);
            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }

        /// <summary>
        /// 渲染成JSON对象
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static String DataTableToJson(DataTable dt)
        {
            return Serialize(dt);
        }

        /// <summary>
        /// json对象反序列化
        /// </summary>
        /// <typeparam name="T"> 对象类型</typeparam>
        /// <param name="json">json串</param>
        /// <returns>对象</returns>
        public static T Deserialize<T>(string json)
        {
            return (T)JsonConvert.DeserializeObject(json, typeof(T));
        }
        public static List<T> DeserializeList<T>(string json)
        {
            return (List<T>)JsonConvert.DeserializeObject(json, typeof(List<T>));
        }

        public static XmlDocument getXml(string json)
        {
            return (XmlDocument)JsonConvert.DeserializeXmlNode(json);
        }

        /// <summary>
        /// * Description   : 转成ligerUI Grid需要的JSON格式字符串
        /// </summary>
        /// <param name="parmTotal">数据总数</param>
        /// <param name="parmList">数据集合</param>
        /// <returns></returns>
        public static string ConvertToGrid(int parmTotal, object parmList)
        {
            return "{\"Total\":" + parmTotal.ToString() + ",\"Rows\":" + Serialize(parmList) + "}";
        }
        
        /// <summary>
        /// 根据DataTable生成Json树结构
        /// </summary>
        /// <param name="tabel">数据源</param>
        /// <param name="idCol">ID列</param>
        /// <param name="valueCol">Value列</param>
        /// <param name="txtCol">Text列</param>
        /// <param name="rela">父节点</param>
        /// <param name="pId">根节点</param>
        public StringBuilder result = new StringBuilder();
        public StringBuilder sb = new StringBuilder();
        public void GetTreeJsonByTable(DataTable tabel, string idCol, string valueCol, string txtCol, string rela, object pId)
        {
            result.Append(sb.ToString());
            sb.Clear();
            if (tabel.Rows.Count > 0)
            {
                sb.Append("[");
                string filer = string.Format("{0}='{1}'", rela, pId);
                DataRow[] rows = tabel.Select(filer);
                if (rows.Length > 0)
                {
                    foreach (DataRow row in rows)
                    {
                        string code = row[idCol].ToString();
                        //if(code.Length <= 5)
                        sb.Append("{\"id\":\"" + row[idCol] + "\",\"value\":\"" + row[valueCol] + "\",\"text\":\"" + row[txtCol] + "\",\"state\":\"open\"");
                        //else
                        //    sb.Append("{\"id\":\"" + row[idCol] + "\",\"text\":\"" + row[txtCol] + "\",\"state\":\"closed\"");
                        if (tabel.Select(string.Format("{0}='{1}'", rela, row[valueCol])).Length > 0)
                        {
                            sb.Append(",\"children\":");
                            GetTreeJsonByTable(tabel, idCol, valueCol, txtCol, rela, row[valueCol]);
                            result.Append(sb.ToString());
                            sb.Clear();
                        }
                        result.Append(sb.ToString());
                        sb.Clear();
                        sb.Append("},");
                    }
                    sb = sb.Remove(sb.Length - 1, 1);
                }
                sb.Append("]");
                result.Append(sb.ToString());
                sb.Clear();
            }
        }

        /// <summary>
        /// 根据DataTable生成Json树结构
        /// </summary>
        /// <param name="tabel">数据源</param>
        /// <param name="idCol">ID列</param>
        /// <param name="valueCol">Value列</param>
        /// <param name="txtCol">Text列</param>
        /// <param name="rela">父节点</param>
        /// <param name="pId">根节点</param>
        public StringBuilder result1 = new StringBuilder();
        public StringBuilder sb1 = new StringBuilder();
        public void GetTreeJsonByTable1(DataTable tabel, string idCol, string valueCol, string txtCol, string rela, object pId)
        {
            result1.Append(sb1.ToString());
            sb1.Clear();
            if (tabel.Rows.Count > 0)
            {
                sb1.Append("[");
                string filer = string.Format("{0}='{1}'", rela, pId);
                DataRow[] rows = tabel.Select(filer);
                if (rows.Length > 0)
                {
                    foreach (DataRow row in rows)
                    {
                        string code = row[idCol].ToString();
                        //if(code.Length <= 5)
                        sb1.Append("{\"id\":\"" + row[valueCol] + "\",\"text\":\"" + row[txtCol] + "\",\"state\":\"open\"");
                        //else
                        //    sb.Append("{\"id\":\"" + row[idCol] + "\",\"text\":\"" + row[txtCol] + "\",\"state\":\"closed\"");
                        if (tabel.Select(string.Format("{0}='{1}'", rela, row[valueCol])).Length > 0)
                        {
                            sb1.Append(",\"children\":");
                            GetTreeJsonByTable1(tabel, idCol, valueCol, txtCol, rela, row[valueCol]);
                            result1.Append(sb1.ToString());
                            sb1.Clear();
                        }
                        result1.Append(sb1.ToString());
                        sb1.Clear();
                        sb1.Append("},");
                    }
                    sb1 = sb1.Remove(sb1.Length - 1, 1);
                }
                sb1.Append("]");
                result1.Append(sb1.ToString());
                sb1.Clear();
            }
        }

        /// <summary>
        /// 根据DataTable生成Json树结构
        /// </summary>
        /// <param name="tabel">数据源</param>
        /// <param name="idCol">ID列</param>
        /// <param name="valueCol">Value列</param>
        /// <param name="txtCol">Text列</param>
        /// <param name="rela">父节点</param>
        /// <param name="pId">根节点</param>
        public StringBuilder result2 = new StringBuilder();
        public StringBuilder sb2 = new StringBuilder();
        public void GetGridTreeJsonByTable(DataTable tabel, string idCol, string valueCol, string txtCol, string rela, object pId)
        {
            result2.Append(sb2.ToString());
            sb2.Clear();
            if (tabel.Rows.Count > 0)
            {
                sb2.Append("[");
                string filer = string.Format("{0}='{1}'", rela, pId);
                DataRow[] rows = tabel.Select(filer);
                if (rows.Length > 0)
                {
                    foreach (DataRow row in rows)
                    {
                        string code = row[idCol].ToString();
                        //if(code.Length <= 5)
                        sb2.Append("{\"id\":\"" + row[idCol] + "\",\"code\":\"" + row[valueCol] + "\",\"text\":\"" + row[txtCol] +
                                   "\",\"prename\":\"" + row["preName"] + "\",\"height\":\"" + row["height"] + "\",\"altitude\":\"" + row["altitude"] + "\",\"state\":\"open\"");
                        //else
                        //    sb2.Append("{\"id\":\"" + row[idCol] + "\",\"text\":\"" + row[txtCol] + "\",\"state\":\"closed\"");
                        if (tabel.Select(string.Format("{0}='{1}'", rela, row[valueCol])).Length > 0)
                        {
                            sb2.Append(",\"children\":");
                            GetGridTreeJsonByTable(tabel, idCol, valueCol, txtCol, rela, row[valueCol]);
                            result2.Append(sb2.ToString());
                            sb2.Clear();
                        }
                        result2.Append(sb2.ToString());
                        sb2.Clear();
                        sb2.Append("},");
                    }
                    sb2 = sb2.Remove(sb2.Length - 1, 1);
                }
                sb2.Append("]");
                result2.Append(sb2.ToString());
                sb2.Clear();
            }
        }

        /// <summary>
        /// 将json转换为DataTable
        /// </summary>
        /// <param name="strJson">得到的json</param>
        /// <returns></returns>
        public static DataTable JsonToDataTable(string strJson)
        {
            //转换json格式
            strJson = strJson.Replace(",\"", "*\"").Replace("\":", "\"#").ToString();
            //取出表名   
            var rg = new Regex(@"(?<={)[^:]+(?=:\[)", RegexOptions.IgnoreCase);
            string strName = rg.Match(strJson).Value;
            DataTable tb = null;
            //去除表名   
            strJson = strJson.Substring(strJson.IndexOf("[") + 1);
            strJson = strJson.Substring(0, strJson.IndexOf("]"));

            //获取数据   
            rg = new Regex(@"(?<={)[^}]+(?=})");
            MatchCollection mc = rg.Matches(strJson);
            for (int i = 0; i < mc.Count; i++)
            {
                string strRow = mc[i].Value;
                string[] strRows = strRow.Split('*');

                //创建表   
                if (tb == null)
                {
                    tb = new DataTable();
                    tb.TableName = strName;
                    foreach (string str in strRows)
                    {
                        var dc = new DataColumn();
                        string[] strCell = str.Split('#');

                        if (strCell[0].Substring(0, 1) == "\"")
                        {
                            int a = strCell[0].Length;
                            dc.ColumnName = strCell[0].Substring(1, a - 2);
                        }
                        else
                        {
                            dc.ColumnName = strCell[0];
                        }
                        tb.Columns.Add(dc);
                    }
                    tb.AcceptChanges();
                }

                //增加内容   
                DataRow dr = tb.NewRow();
                for (int r = 0; r < strRows.Length; r++)
                {
                    string title = strRows[r].Split('#')[0].Trim().Replace("，", ",").Replace("：", ":").Replace("\"", "");
                    try
                    {
                        string strValue = strRows[r].Split('#')[1].Trim().Replace("，", ",").Replace("：", ":").Replace("\"", "");
                        dr[title] = strValue == "null" ? "" : strValue;
                    }
                    catch { }
                }
                tb.Rows.Add(dr);
                tb.AcceptChanges();
            }

            return tb;
        }

        ///// <summary>
        ///// 按数组中的值从指定集合中查出符合条件的数据(泛型集合)
        ///// </summary>
        ///// <typeparam name="T">数据模型</typeparam>
        ///// <param name="objList">List数据源</param>
        ///// <param name="property">条件列明</param>
        ///// <param name="rPart">选择的节点</param>
        ///// <param name="rType">选择节点的类型</param>
        ///// <returns></returns>
        //public static List<T> ConventData<T>(List<T> objList, string property, string rPart, string rType)
        //{
        //    if (objList.Count > 0)
        //    {
        //        List<T> needData = objList.ToList();
        //        //获取所属
        //        DataTable strRids = GetRelationInfo(rPart, rType);

        //        //遍历所有数据，剔除不满足条件的数据
        //        foreach (T t in objList)
        //        {
        //            //指定rid字段名
        //            Type type = typeof(T);
        //            PropertyInfo pi = type.GetProperty(property);
        //            //根据字段名获取rid的值
        //            string strValue = pi.GetValue(t, null).ToString();

        //            //根据值，判断该条数据是否满足条件，不满足就剔除
        //            string filer = string.Format("rid='{0}'", strValue == "" ? "0" : strValue);

        //            DataRow[] rows = strRids.Select(filer);
        //            if (rows.Length == 0)
        //            {
        //                //移除不满足条件的数据
        //                needData.Remove(t);
        //            }
        //        }
        //        return needData;
        //    }
        //    else
        //    {
        //        return objList;
        //    }
        //}

        ///// <summary>
        ///// 按数组中的值从指定集合中查出符合条件的数据(DataTable数据集)
        ///// </summary>
        ///// <param name="data">DataTable数据源</param>
        ///// <param name="property">条件列明</param>
        ///// <param name="rPart">选择的节点</param>
        ///// <param name="rType">选择节点的类型</param>
        ///// <returns></returns>
        //public static DataTable ConventData1(DataTable data, string property, string rPart, string rType)
        //{
        //    if (data.Rows.Count > 0)
        //    {
        //        DataTable needData = data.Copy();
        //        //获取所属
        //        DataTable strRids = GetRelationInfo(rPart, rType);
        //        //遍历所有数据，剔除不满足条件的数据
        //        foreach (DataRow dr in data.Rows)
        //        {
        //            string strValue = dr[property].ToString();

        //            //根据值，判断该条数据是否满足条件，不满足就剔除
        //            string filer = string.Format("rid='{0}'", strValue == "" ? "0" : strValue);
        //            DataRow[] rows = strRids.Select(filer);
        //            if (rows.Length == 0)
        //            {
        //                string filer1 = string.Format(property + "='{0}'", strValue == "" ? "0" : strValue);
        //                DataRow[] rows1 = needData.Select(filer1);
        //                if (rows1.Length > 0)
        //                {
        //                    //移除不满足条件的数据
        //                    needData.Rows.Remove(rows1[0]);
        //                }
        //            }
        //        }
        //        return needData;
        //    }
        //    else
        //    {
        //        return data;
        //    }
        //}

        ///// <summary>
        ///// 获取关联关系信息(指定节点及下级所有节点)
        ///// </summary>
        ///// <param name="rid">选择的节点</param>
        ///// <param name="type">选择节点的类型</param>
        ///// <returns></returns>
        //public static DataTable GetRelationInfo(string rid, string type)
        //{
        //    DataService ds = new DataService();
        //    DataTable dt = ds.ExecuteDataset("FORECAST.PR_GETTREEINFO", rid, type, "").Tables[0];
        //    return dt;
        //}
    }
}
