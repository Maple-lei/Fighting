using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebForm.common;

namespace WebForm.Tool
{
    public partial class DateTime_Long_Convert : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime dt = Convert.ToDateTime("2018-04-03");

            var result = DatetimeConvert.ConvertDataTimeToLong(dt);
        }
    }
}