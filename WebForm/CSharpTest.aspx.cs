using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebForm
{
    public partial class CSharpTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //string s = GenerateGuid().ToString();

            DateTime dt1 = new DateTime(2018, 3, 31).AddMonths(-1);
        }

       
    }
}