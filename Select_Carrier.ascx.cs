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
    public partial class Select_Carrier : PortletViewBase
    {
        string connString = ConfigurationManager.ConnectionStrings["ERP"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            lblConfirmMsg.Text = "Year=" + this.ParentPortlet.PortletViewState["year"].ToString() + ", ";
            lblConfirmMsg.Text += "Sess=" + this.ParentPortlet.PortletViewState["sess"].ToString() + ", ";

            showCurrentSelections();

            string YEAR = this.ParentPortlet.PortletViewState["year"].ToString();
            string SESS = this.ParentPortlet.PortletViewState["sess"].ToString();
            string DEVICE = this.ParentPortlet.PortletViewState["device"].ToString();
            string FINISH = this.ParentPortlet.PortletViewState["finish"].ToString();
            string MODEL = this.ParentPortlet.PortletViewState["model"].ToString();
            string CONNECT = this.ParentPortlet.PortletViewState["connect"].ToString();
            string PRICE = this.ParentPortlet.PortletViewState["price"].ToString();
            string ID = PortalUser.Current.HostID.TrimStart('0');

            if ((YEAR == "") || (SESS == ""))
            {
                lblErrMsg.Text = "ERROR: Year/Session not found (ipadparam).  Please notify the TSC if the error persits.";
                pnlErrorMsg.Visible = true;
                pnlSelCarrier.Visible = false;
            }
            else
            {
                OdbcConnection conn = new OdbcConnection();
                conn.ConnectionString = connString;
                conn.Open();


                // Read the iPad Device records, and display to user
                OdbcCommand command = new OdbcCommand();
                command.Connection = conn;
                command.CommandText = "select * from sc_ipadcarrier_table order by seq";
                try
                {
                    OdbcDataReader carrRec = command.ExecuteReader();
                    if (carrRec.HasRows)
                    {
                        while (carrRec.Read())
                        {
                            Panel pnlCarrier = new Panel();
                            pnlCarrier.CssClass = "pnlCarrier";

                            LinkButton lnkCarrier = new LinkButton();
                            lnkCarrier.Click += new EventHandler(btnSelect_Click);
                            lnkCarrier.CommandArgument = carrRec["carrier"].ToString().Trim();
                            //lnkCarrier.Text = "<h1>" + carrRec["carrier"].ToString().Trim() + "</h1>";
                            lnkCarrier.Text += "<img src='" + carrRec["logo_url"].ToString().Trim() + "'>";
                            pnlCarrier.Controls.Add(lnkCarrier);

                            plhChoices.Controls.Add(pnlCarrier);

                        }
                    }
                    else { }
                }
                catch //(Exception ex)
                {
                    lblErrMsg.Text = "ERROR: Data retrieval error (ipadcarrier).  Please notify the TSC if the error persits.";
                    //lblErrMsg.Text += "<br>" + ex.ToString();
                    pnlErrorMsg.Visible = true;
                    conn.Close();
                    return;
                }
            }
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
            command.CommandText = "select t1.*, t2.dev_image_url from sc_ipadorder_rec t1 join sc_ipaddevice_table t2 on t1.device=t2.device where sc_ipadorder_no=?";
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
                        this.ParentPortlet.PortletViewState["model"] = MODEL;
                        CONNECT = orderRec["connect"].ToString().Trim();
                        this.ParentPortlet.PortletViewState["connect"] = CONNECT;
                        CARRIER = orderRec["carrier"].ToString().Trim();
                        IMAGE = orderRec["dev_image_url"].ToString().Trim();
                        PRICE = Convert.ToDecimal(orderRec["price"]);
                        this.ParentPortlet.PortletViewState["price"] = PRICE;
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

        }


        protected void btnErase_Click(object sender, EventArgs e)
        {
            string ORDER = this.ParentPortlet.PortletViewState["ordernum"].ToString();
            OdbcConnection conn = new OdbcConnection();
            conn.ConnectionString = connString;
            OdbcDataAdapter dataAdapter = new OdbcDataAdapter();
            conn.Open();
            OdbcCommand command = new OdbcCommand();
            command.Connection = conn;
            command.Parameters.Clear();
            command.CommandText = "delete sc_ipadorder_rec where sc_ipadorder_no=?";
            command.Parameters.Add(new OdbcParameter("sc_ipadorder_no", ORDER));
            try
            {
                command.ExecuteNonQuery();
            }
            catch //(Exception ex)
            {
                lblErrMsg.Text = "ERROR: Unable to delete selections.  Please notify the TSC if the error persists.";
                //lblErrMsg.Text += "<br />" + ex.Message.ToString() + "<br />" + ex.ToString();
                pnlErrorMsg.Visible = true;
            }
            this.ParentPortlet.NextScreen("Select_Device");
        }


        protected void btnSelect_Click(object sender, EventArgs e)
        {
            string ORDER = this.ParentPortlet.PortletViewState["ordernum"].ToString();
            string CARRIER = ((LinkButton)sender).CommandArgument;
            this.ParentPortlet.PortletViewState["carrier"] = CARRIER;
            // Add the Step 4 info
            OdbcConnection conn = new OdbcConnection();
            conn.ConnectionString = connString;
            conn.Open();
            OdbcCommand command = new OdbcCommand();

            command.Connection = conn;
            command.Parameters.Clear();
            command.CommandText = "update sc_ipadorder_rec set carrier=?, order_step='4' where sc_ipadorder_no=?";
            command.Parameters.Add(new OdbcParameter("carrier", CARRIER));
            command.Parameters.Add(new OdbcParameter("sc_ipadorder_no", ORDER));
            try
            {
                command.ExecuteNonQuery();
            }
            catch //(Exception ex)
            {
                lblErrMsg.Text = "ERROR: Unable to update the order record.  Notify the TSC if the error persits.";
                //lblErrMsg.Text += "<br>" + ex.ToString();
                pnlErrorMsg.Visible = true;
                pnlConfirmMsg.Visible = false;
                conn.Close();
                return;
            }
            this.ParentPortlet.NextScreen("Place_Order");
        }

    
    }
}