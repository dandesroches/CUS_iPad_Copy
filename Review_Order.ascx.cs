using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jenzabar.Common.Web.UI.Controls;
using Jenzabar.Portal.Framework.Web.UI;
using Jenzabar.Common.Globalization;  // added for FWK_Globalizations table
using Jenzabar.ICS.Web.Portlets.Common; // added for setting (and preference) check
using System.Text;  // Needed for Stringbuilder
using Jenzabar.Portal.Framework;  // Needed for PortalUser.Current.Username
using System.Configuration; // For the ConfigurationManager
using System.Data; // general ADO.NET
using System.Data.Common; // general/common ADO.NET
using System.Data.Odbc; // ODBC Specific Implementation of the ADO.NET Standard/Specification

namespace CUS_iPad
{
    public partial class Review_Order : PortletViewBase
    {
        string connString = ConfigurationManager.ConnectionStrings["ERP"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            lblConfirmMsg.Text = "Year=" + this.ParentPortlet.PortletViewState["year"].ToString() + ", ";
            lblConfirmMsg.Text += "Sess=" + this.ParentPortlet.PortletViewState["sess"].ToString() + ", ";

            showCurrentSelections();

        }

        private void showCurrentSelections()
        {
            string YEAR = this.ParentPortlet.PortletViewState["year"].ToString();
            string SESS = this.ParentPortlet.PortletViewState["sess"].ToString();
            string ID = PortalUser.Current.HostID.TrimStart('0');
            string ORDER = this.ParentPortlet.PortletViewState["ordernum"].ToString();
            string DEVICE = "";
            string FINISH = "";
            string MODEL = "";
            string CONNECT = "";
            string CARRIER = "";
            string IMAGE = "";
            int PAYMENTS = 0;
            decimal PRICE = 0;

            OdbcConnection conn = new OdbcConnection();
            conn.ConnectionString = connString;
            conn.Open();

            OdbcCommand command = new OdbcCommand();
            command.Connection = conn;
            command.Parameters.Clear();
            command.CommandText = "select t1.*, t2.fin_image_url from sc_ipadorder_rec t1 join sc_ipadfinish_table t2 on t1.device=t2.device and t1.finish=t2.finish where sc_ipadorder_no=?";
            command.Parameters.Add(new OdbcParameter("sc_ipadorder_no", ORDER));
            try
            {
                OdbcDataReader orderRec = command.ExecuteReader();
                if (orderRec.HasRows)
                {
                    while (orderRec.Read())
                    {
                        DEVICE = orderRec["device"].ToString().Trim();
                        this.ParentPortlet.PortletViewState["device"] = DEVICE;
                        FINISH = orderRec["finish"].ToString().Trim();
                        this.ParentPortlet.PortletViewState["finish"] = FINISH;
                        MODEL = orderRec["model"].ToString().Trim();
                        CONNECT = orderRec["connect"].ToString().Trim();
                        CARRIER = orderRec["carrier"].ToString().Trim();
                        IMAGE = orderRec["fin_image_url"].ToString().Trim();
                        PRICE = Convert.ToDecimal(orderRec["price"]);
                        PAYMENTS = Convert.ToInt16(orderRec["pay_option"]);
                    }
                }
                else
                {
                    lblErrMsg.Text = "ERROR: Order record not found.  Notify the TSC if the error persits.";
                    pnlErrorMsg.Visible = true;
                    pnlConfirmMsg.Visible = false;
                    conn.Close();
                    return;
                }
            }
            catch //(Exception ex)
            {
                lblErrMsg.Text = "ERROR: Data retrieval error (ipadorder).  Notify the TSC if the error persits.";
                //lblErrMsg.Text += "<br>" + ex.ToString();
                pnlErrorMsg.Visible = true;
                pnlConfirmMsg.Visible = false;
                conn.Close();
                return;
            }

            imgDevice.ImageUrl = IMAGE;
            lblDevice.Text = DEVICE;
            if (FINISH != "") { lblFinish.Text = FINISH; }
            if (MODEL != "") { lblModel.Text = MODEL; }
            if (CONNECT != "") { lblConnect.Text = CONNECT; }
            if (CARRIER != "") { lblCarrier.Text = CARRIER; }
            if (PRICE != 0) { lblPrice.Text = PRICE.ToString(); }
            if (PAYMENTS != 0) { lblPayments.Text = PAYMENTS.ToString(); }

        }


    }
}