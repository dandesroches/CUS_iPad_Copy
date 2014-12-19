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
    public partial class Select_Device : PortletViewBase
    {
        string connString = ConfigurationManager.ConnectionStrings["ERP"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            string YEAR = this.ParentPortlet.PortletViewState["year"].ToString();
            string SESS = this.ParentPortlet.PortletViewState["sess"].ToString();
            string ID = PortalUser.Current.HostID.TrimStart('0');

            if ((YEAR=="") || (SESS==""))
            {
                lblErrMsg.Text = "ERROR: Year/Session not found (ipadparam).  Please notify the TSC if the error persits.  ";
                pnlErrorMsg.Visible = true;
                pnlSelDevice.Visible = false;
            }
            else
            {
                OdbcConnection conn = new OdbcConnection();
                conn.ConnectionString = connString;
                conn.Open();


                // Read the iPad Device records, and display to user
                OdbcCommand command = new OdbcCommand();
                command.Connection = conn;
                command.CommandText =  "select * from sc_ipaddevice_table where device_status='A' order by seq";
                try
                {
                    OdbcDataReader devRec = command.ExecuteReader();
                    if (devRec.HasRows)
                    {
                        while (devRec.Read())
                        {
                            Panel pnlDevice = new Panel();
                            pnlDevice.CssClass = "pnlDevice";

                            LinkButton lnkDevice = new LinkButton();
                            lnkDevice.Click += new EventHandler(btnSelect_Click);
                            lnkDevice.CommandArgument = devRec["device"].ToString().Trim();
                            lnkDevice.Text = "<h1>" + devRec["device"].ToString().Trim();
                            lnkDevice.Text += "<br /><span class='devExt'>" + devRec["dev_ext"].ToString().Trim() + "</span></h1>";
                            lnkDevice.Text += "<img src='" + devRec["dev_image_url"].ToString().Trim() + "'>";
                            pnlDevice.Controls.Add(lnkDevice);

                            plhChoices.Controls.Add(pnlDevice);

                        }
                    }
                    else {  }
                }
                catch //(Exception ex)
                {
                    lblErrMsg.Text = "ERROR: Data retrieval error (ipaddevice).  Please notify the TSC if the error persits.";
                    //lblErrMsg.Text += "<br>" + ex.ToString();
                    pnlErrorMsg.Visible = true;
                    conn.Close();
                    return;
                }
            }
        }



        protected void btnSelect_Click(object sender, EventArgs e)
        {

            string DEVICE = ((LinkButton)sender).CommandArgument;
            this.ParentPortlet.PortletViewState["device"] = DEVICE;
            // Add the Step 1 Record
            OdbcConnection conn = new OdbcConnection();
            conn.ConnectionString = connString;
            conn.Open();
            OdbcCommand command = new OdbcCommand();

            command.Connection = conn;
            command.Parameters.Clear();
            command.CommandText = "INSERT INTO sc_ipadorder_rec (yr, sess, id, device, order_step, step_date, order_status)";
            command.CommandText += " VALUES (?, ?, ?, ?, '1', today, 'A');";

            command.Parameters.Add(new OdbcParameter("yr", this.ParentPortlet.PortletViewState["year"]));
            command.Parameters.Add(new OdbcParameter("sess", this.ParentPortlet.PortletViewState["sess"]));
            command.Parameters.Add(new OdbcParameter("id", PortalUser.Current.HostID.TrimStart('0')));
            command.Parameters.Add(new OdbcParameter("device", DEVICE));
            try
            {
                command.ExecuteNonQuery();
            }
            catch //(Exception ex)
            {
                lblErrMsg.Text = "ERROR: Unable to insert the order record.  Notify the TSC if the error persits.";
                //lblErrMsg.Text += "<br>" + ex.ToString();
                pnlErrorMsg.Visible = true;
                pnlConfirmMsg.Visible = false;
                conn.Close();
                return;
            }
            this.ParentPortlet.NextScreen("Select_Finish");
        }


    }
}