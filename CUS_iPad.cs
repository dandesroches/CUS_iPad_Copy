using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jenzabar.Portal.Framework.Web.UI;
using Jenzabar.Common.Globalization;  // added for FWK_Globalizations table
using Jenzabar.ICS.Web.Portlets.Common; // added for setting (and preference) check
using System.Web;
using Jenzabar.Common.Web.UI.Controls;
using Jenzabar.Portal.Framework;  // Needed for PortalUser.Current.Username
using System.Configuration; // For the ConfigurationManager
using System.Data; // general ADO.NET
using System.Data.Common; // general/common ADO.NET
using System.Data.Odbc; // ODBC Specific Implementation of the ADO.NET Standard/Specification



namespace CUS_iPad
{
    public class CUS_iPad : LinkablePortletBase
    {
        string connString = ConfigurationManager.ConnectionStrings["ERP"].ConnectionString;

        protected override PortletViewBase GetCurrentScreen()
        {
            PortletViewBase screen = null;
            PortletViewState["ordernum"] = "";
            PortletViewState["reqcarrier"] = "";
            string STEP = completedStep();
            string NEEDCARRIER = PortletViewState["reqcarrier"].ToString();
            switch (CurrentPortletScreenName)
            {
                case "Select_Device":
                    screen = LoadPortletView("ICS/CUS_iPad/Select_Device.ascx");
                    break;
                case "Select_Finish":
                    screen = LoadPortletView("ICS/CUS_iPad/Select_Finish.ascx");
                    break;
                case "Select_Model":
                    screen = LoadPortletView("ICS/CUS_iPad/Select_Model.ascx");
                    break;
                case "Select_Carrier":
                    screen = LoadPortletView("ICS/CUS_iPad/Select_Carrier.ascx");
                    break;
                case "Place_Order":
                    screen = LoadPortletView("ICS/CUS_iPad/Place_Order.ascx");
                    break;
                case "Review_Order":
                    screen = LoadPortletView("ICS/CUS_iPad/Review_Order.ascx");
                    break;
                case "Default":
                default:
                    switch (STEP)
                    {
                        case "0":
                            screen = LoadPortletView("ICS/CUS_iPad/Select_Device.ascx");
                            break;
                        case "1":
                            screen = LoadPortletView("ICS/CUS_iPad/Select_Finish.ascx");
                            break;
                        case "2":
                            screen = LoadPortletView("ICS/CUS_iPad/Select_Model.ascx");
                            break;
                        case "3":
                            if (NEEDCARRIER == "Y")
                            {
                                screen = LoadPortletView("ICS/CUS_iPad/Select_Carrier.ascx");
                                break;
                            }
                            else
                            {
                                screen = LoadPortletView("ICS/CUS_iPad/Place_Order.ascx");
                                break;
                            }
                        case "4":
                            screen = LoadPortletView("ICS/CUS_iPad/Place_Order.ascx");
                            break;
                        case "C":
                            screen = LoadPortletView("ICS/CUS_iPad/Review_Order.ascx");
                            break;
                    }
                    break;
            }
            return screen;
        }


        private string completedStep()
        {
            string step = "0";
            OdbcConnection conn = new OdbcConnection();
            conn.ConnectionString = connString;
            conn.Open();

            // Get the Year and Session from the parameter file
            OdbcCommand command = new OdbcCommand();
            command.Connection = conn;
            command.Parameters.Clear();
            command.CommandText = "select first 1 yr, sess from sc_ipadparam_table where today>=order_period_start and today<=order_period_end order by order_period_start desc";
            try
            {
                OdbcDataReader paramRec = command.ExecuteReader();
                if (paramRec.HasRows)
                {
                    while (paramRec.Read())
                    {
                        PortletViewState["year"] = paramRec["yr"].ToString();
                        PortletViewState["sess"] = paramRec["sess"].ToString();
                        // Check to see if the student already has an order in progress.  If so, proceed to the next step.
                        command = new OdbcCommand();
                        command.Connection = conn;
                        command.Parameters.Clear();
                        command.CommandText = "select t1.*, req_carrier from sc_ipadorder_rec t1 ";
                        command.CommandText += " left join sc_ipadmodel_table t2 on t1.device=t2.device and t1.model=t2.model and t1.connect=t2.connect where yr=? and sess=? and id=?";
                        command.Parameters.Add(new OdbcParameter("yr", paramRec["yr"].ToString()));
                        command.Parameters.Add(new OdbcParameter("sess", paramRec["sess"].ToString()));
                        command.Parameters.Add(new OdbcParameter("id", PortalUser.Current.HostID.TrimStart('0')));
                        try
                        {
                            OdbcDataReader orderRec = command.ExecuteReader();
                            if (orderRec.HasRows)
                            {
                                while (orderRec.Read())
                                {
                                    step = orderRec["order_step"].ToString();
                                    PortletViewState["ordernum"] = orderRec["sc_ipadorder_no"].ToString();
                                    PortletViewState["reqcarrier"] = orderRec["req_carrier"].ToString();
                                }
                            }
                        }
                        catch { /* do nothing */ }
                    }
                 }
            }
            catch { /* do nothing */ }
            return step; 
        }


    }
}
