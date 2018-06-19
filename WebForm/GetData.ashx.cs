using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebForm
{
    /// <summary>
    /// GetData 的摘要说明
    /// </summary>
    public class GetData : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
           
            string aciton = context.Request["action"];
            string message = string.Empty;

            switch (aciton)
            {
                case "GetComboboxData":
                    message = GetComboboxData();
                    break;
            }

            //context.Response.Clear();
            context.Response.ContentType = "text/plain";
            context.Response.Write(message);
            context.Response.End();
        }

        public string GetComboboxData()
        {
            List<Test> list = new List<Test>();

            list.Add(new Test { id = 1, name = "huyiheng" });
            list.Add(new Test { id = 2, name = "shijuan" });

            return Common.JsonHelper.ObjectToJSON(list);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    public class Test
    {
        public int id { get; set; }

        public string name { get; set; }
    }
}