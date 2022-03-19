using GarasERP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GarasSales.Sales.Reports
{
    public partial class Reports : System.Web.UI.Page
    {
        System.Data.DataTable ReportDT = new System.Data.DataTable();
        static string key = "SalesGarasPass";
        private long UserID
        {
            get
            {
                if (Session["UserID"] == null)
                {
                    return 0;
                }
                else
                {
                    string id = Session["UserID"].ToString();

                    return long.Parse(GarasERP.Encrypt_Decrypt.Decrypt(id, key));
                }
            }
            set
            {
                Session["UserID"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if(!IsPostBack)
                {
                    LoadReportData();
                }
                else
                {
                    if (ViewState["ReportDT"] != null)
                        ReportDT = (System.Data.DataTable)ViewState["ReportDT"];
                }
            }
            catch(Exception ex)
            {
                Page.Response.Write(ex.Message);
            }

        }

        private void LoadReportData()
        {
            try
            {
                bool ViewAll = Common.CheckUserInRole(UserID, 4); // view all daily reports
                ReportDT = InitializeReportGridViewDataTable();
                V_DailyReport report = new V_DailyReport();
                if (!ViewAll)
                    report.Where.UserID.Value = UserID;
                else
                {
                    report.Where.BranchID.Value = Common.GetUserBranchID(UserID);
                    //report.Where.Status.Value = "Not Filled";
                    //report.Where.Status.Operator = MyGeneration.dOOdads.WhereParameter.Operand.NotEqual;
                }
                report.Query.AddOrderBy(V_DailyReport.ColumnNames.ReprotDate, MyGeneration.dOOdads.WhereParameter.Dir.DESC);
                if(report.Query.Load())
                {
                    do
                    {
                        DataRow dr = ReportDT.NewRow();
                        dr["ID"] = report.ID;
                        dr["ReportDate"] = report.ReprotDate.ToString("dd/MM/yyyy");
                        dr["UserName"] = report.FirstName +" "+report.LastName;
                        dr["ModifiedDate"] = report.ModifiedDate.ToString("dd/MM/yyyy");
                        dr["Status"] = report.Status;
                        dr["ViewURL"] = "ViewReport.aspx?RID=" + Server.UrlEncode(Encrypt_Decrypt.Encrypt(report.s_ID, key));
                        dr["Review"] = report.s_Review;
                        dr["Reviewed"] = report.Reviewed;
                        if (UserID == report.UserID && report.Status == "Not Filled")
                        {
                            dr["CanEdit"] = true;
                            dr["URL"] = "EditReport.aspx?RID="+ Server.UrlEncode( Encrypt_Decrypt.Encrypt(report.s_ID,key));
                        }
                        else
                        {
                            dr["CanEdit"] = false;
                            dr["URL"] = "#";
                        }
                        List<string> groups = new List<string>();
                        groups.Add("SalesManagers");
                        if (Common.CheckUserInGroups(UserID, groups) && report.Status == "Pending Verification")
                        {
                            dr["CanVer"] = true;
                            dr["URL"] = "VarifyReport.aspx?RID=" + Server.UrlEncode(Encrypt_Decrypt.Encrypt(report.s_ID, key));
                        }
                        else
                        {
                            dr["CanVer"] = false;
                         //   dr["URL"] = "#";
                        }

                        if (UserID == report.UserID || (report.Status != "Not Filled"))
                        {
                            ReportDT.Rows.Add(dr);
                        }
                        
                    }
                    while (report.MoveNext());
                }
                GV_Reports.DataSource = ReportDT;
                GV_Reports.DataBind();
                ViewState["ReportDT"] = ReportDT;
            }
            catch(Exception ex)
            {
                Page.Response.Write(ex.Message);
            }
        }

        private System.Data.DataTable InitializeReportGridViewDataTable()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("ReportDate");
            dt.Columns.Add("UserName");
            dt.Columns.Add("ModifiedDate");
            dt.Columns.Add("Status");
            dt.Columns.Add("URL");
            dt.Columns.Add("ViewURL");
            dt.Columns.Add("CanEdit",typeof(bool));
            dt.Columns.Add("CanVer", typeof(bool));
            dt.Columns.Add("Review");
            dt.Columns.Add("Reviewed", typeof(bool));
            


            return dt;
        }

        protected void GV_Reports_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GV_Reports.PageIndex = e.NewPageIndex;
            GV_Reports.DataSource = (DataTable)ViewState["ReportDT"];
            GV_Reports.DataBind();

        }
    }
}