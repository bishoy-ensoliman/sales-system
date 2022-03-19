using GarasERP;
using GarasSales.ChartsJSClasses;
using MyGeneration.dOOdads;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GarasSales.Target
{
    public partial class CreateTarget : System.Web.UI.Page
    {
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

                    return long.Parse(Encrypt_Decrypt.Decrypt(id, key));
                }
            }
            set
            {
                Session["UserID"] = value;
            }
        }

        private DataTable VSLocation
        {
            get
            {
                if (ViewState["Location"] == null)
                {
                    return new DataTable();
                }
                else
                {
                    return (DataTable)ViewState["Location"];
                }
            }
            set
            {
                ViewState["Location"] = value;
            }
        }

        private DataTable VSProds
        {
            get
            {
                if (ViewState["Products"] == null)
                {
                    return new DataTable();
                }
                else
                {
                    return (DataTable)ViewState["Products"];
                }
            }
            set
            {
                ViewState["Products"] = value;
            }
        }

        private DataTable VSProdsNew
        {
            get
            {
                if (ViewState["VSProdsNew"] == null)
                {
                    return new DataTable();
                }
                else
                {
                    return (DataTable)ViewState["VSProdsNew"];
                }
            }
            set
            {
                ViewState["VSProdsNew"] = value;
            }
        }

        private bool checkUserPriviledge()
        {
            int pageRoleID = 2;
            bool userCanAccess = false;
            try
            {
                UserRole userRole = new UserRole();
                userRole.Where.RoleID.Value = pageRoleID;
                userRole.Where.UserID.Value = UserID;
                if (userRole.Query.Load())
                {
                    if (userRole.DefaultView != null && userRole.DefaultView.Count > 0)
                    {
                        userCanAccess = true;
                    }
                }
                if (!userCanAccess)
                {
                    Group_User gpUser = new Group_User();
                    gpUser.Where.Active.Value = 1;
                    gpUser.Where.UserID.Value = UserID;
                    string gpIDs = "";
                    if (gpUser.Query.Load())
                    {
                        if (gpUser.DefaultView != null && gpUser.DefaultView.Count > 0)
                        {
                            do
                            {
                                if (gpIDs == "")
                                {
                                    gpIDs = "'" + gpUser.GroupID + "'";
                                }
                                else
                                {
                                    gpIDs += ",'" + gpUser.GroupID + "'";
                                }

                            } while (gpUser.MoveNext());
                        }
                    }
                    if (gpIDs != "")
                    {
                        GroupRole gpRole = new GroupRole();
                        gpRole.Where.GroupID.Value = gpIDs;
                        gpRole.Where.GroupID.Operator = WhereParameter.Operand.In;
                        gpRole.Where.RoleID.Value = pageRoleID;
                        gpRole.Query.Load();
                        if (gpRole.RowCount > 0)
                        {
                            userCanAccess = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
            }
            return userCanAccess;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (checkUserPriviledge())
            {
                if (!Page.IsPostBack)
                {
                    LoadTargetYears();
                    LoadLast5Years();
                    LoadTopSelling();
                    LoadTopProfit();
                    LoadTarget();
                    LoadLastBranchTargets();
                }
                if (chbxProdPlan.Checked)
                {
                    //divProductDetails.Style.Add("display", "block");
                    divProductDetails.Visible = true;
                    drawProdPlanChart();
                }
                else
                {
                    //divProductDetails.Style.Add("display", "none");
                    divProductDetails.Visible = false;
                }
                divPage.Visible = true;
            }
            else
            {
                divPage.Visible = false;
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "NoPriveledge", "alert('You Donot have sufficient priveledges!');window.location.href='/Sales/SalesHome.aspx';", true);
                //Response.Redirect("/Sales/SalesHome.aspx", false);
            }
        }

        private void drawProdPlanChart()
        {
            string chartHeaderWzData = getProdDataChart();
            //hdnProdChartHeader.Value = chartHeaderWzData[0];
            hdnProdChartData.Value = chartHeaderWzData;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "DrawProdChart",
                                                    "drawProdPlanChart();", true);
        }

        private string getProdDataChart()
        {
            string headerWzData = "";
            V_ProductTargetChart productTargetChart = new V_ProductTargetChart();
            productTargetChart.Where.Year.Value = ddlTargetYear.SelectedItem.Text;
            productTargetChart.Where.Year.Operator = WhereParameter.Operand.LessThan;
            productTargetChart.Query.AddOrderBy(V_ProductTargetChart.ColumnNames.Year, WhereParameter.Dir.DESC);
            productTargetChart.Query.Load();
            if(productTargetChart.RowCount > 0)
            {
                int yearCount = 0;
                int year = 0;
                DataTable dt = new DataTable();
                dt.Columns.Add("BranchName", typeof(string));
                dt.Columns.Add("ProductName", typeof(string));
                dt.Columns.Add("Amount", typeof(double));
                dt.AcceptChanges();
                List<string> branchNames = new List<string>();
                //branchNames.Add("Product");
                List<string> productNames = new List<string>();
                productTargetChart.Rewind();
                do
                {
                    if(year != productTargetChart.Year)
                    {
                        year = productTargetChart.Year;
                        yearCount++;
                    }
                    if(yearCount == 2)
                    {
                        break;
                    }
                    DataRow dr = dt.NewRow();
                    dr["BranchName"] = productTargetChart.BranchName;
                    dr["ProductName"] = productTargetChart.ProductName;
                    dr["Amount"] = productTargetChart.Amount;
                    dt.Rows.Add(dr);
                    if (!branchNames.Contains(productTargetChart.BranchName))
                    {
                        branchNames.Add(productTargetChart.BranchName);
                    }
                    if (!productNames.Contains(productTargetChart.ProductName))
                    {
                        productNames.Add(productTargetChart.ProductName);
                    }
                } while (productTargetChart.MoveNext());
                dt.AcceptChanges();
                
                string header = "['Product','" + String.Join("','",branchNames)+"']";
                headerWzData += header+",";
                string prodPlan = "";
                foreach(string prod in productNames)
                {
                    prodPlan = "['" + prod + "',";
                    foreach (string branch in branchNames)
                    {
                        DataRow[] drs = dt.Select("ProductName = '" + prod + "' and BranchName = '"+branch+"'").ToArray();
                        if(drs.Length == 0)
                        {
                            prodPlan += "0,";
                        }
                        else
                        {
                            prodPlan += drs[0]["Amount"]+",";
                        }
                    }
                    prodPlan = prodPlan.Substring(0, prodPlan.Length - 1);
                    prodPlan += "]";
                    headerWzData += prodPlan + ",";
                }

                headerWzData = headerWzData.Substring(0, headerWzData.Length - 1);
            }
            headerWzData = "[" + headerWzData + "]";
            return headerWzData;
        }

        private void LoadLastBranchTargets()
        {
            Branch branch = new Branch();
            branch.Where.Active.Value = 1;
            branch.Query.AddOrderBy(Branch.ColumnNames.ID, WhereParameter.Dir.ASC);
            branch.Query.Top = 2;// get first 2 branches
            branch.Query.Load();
            string[] chartsData = new string[2];
            string[] chartsColors = { "rgb(255, 99, 71)", "rgb(100, 149, 237)" };
            int i = 0;
            if (branch.RowCount > 0)
            {
                branch.Rewind(); //move to first record
                do
                {
                    chartsData[i] = LoadBranchTargets(branch.ID, chartsColors[i],branch.Name);
                    i++;
                } while (branch.MoveNext());
            }
            hdnFrstChart.Value = chartsData[0];
            hdnScndChart.Value = chartsData[1];
        }

        private string LoadBranchTargets(int branchID, string chartColor,string branchName)
        {
            string jsonChart = "";
            SalesBranchTarget salesBranchTarget = new SalesBranchTarget();
            //salesBranchTarget.Where.Active.Value = 1;
            salesBranchTarget.Where.BranchID.Value = branchID;
            salesBranchTarget.Query.AddOrderBy(SalesBranchTarget.ColumnNames.TargetID, WhereParameter.Dir.ASC);
            //salesBranchTarget.Query.Top = 10;
            salesBranchTarget.Query.Load();
            
            if (salesBranchTarget.RowCount > 0)
            {
                string[] taregtYears = new string[salesBranchTarget.RowCount];
                double[] branchTargets = new double[salesBranchTarget.RowCount];
                int i = 0;
                salesBranchTarget.Rewind(); //move to first record
                do
                {
                    taregtYears[i] = getTargetYear(salesBranchTarget.TargetID);
                    branchTargets[i] = (double)salesBranchTarget.Amount;
                    i++;
                } while (salesBranchTarget.MoveNext());
                TargetDataSet chartDataSet = new TargetDataSet(branchName + " Sales", chartColor, chartColor, branchTargets);
                ChartData chartData = new ChartData(taregtYears, new TargetDataSet[] { chartDataSet });
                jsonChart = new JavaScriptSerializer().Serialize(chartData);
            }
            return jsonChart;
        }

        private string getTargetYear(int targetID)
        {
            SalesTarget target = new SalesTarget();
            target.LoadByPrimaryKey(targetID);
            if(target.RowCount > 0)
            {
                return target.Year.ToString();
            }
            return "error";
        }

        private void LoadTarget()
        {
            SalesTarget target = LoadYearTarget();
            if (target != null)
            {
                btnCreate.Text = "Edit";
                tbxYearTarget.Text = target.Target.ToString();
                LoadLocationsByTarget(target);
            }
            else
            {
                btnCreate.Text = "Create";
                tbxYearTarget.Text = "";
                LoadLocations();
            }
            LoadProdByLoc();
        }

        private void LoadLocationsByTarget(SalesTarget target)
        {
            try
            {
                Branch branch = new Branch();
                branch.Where.Active.Value = 1;
                branch.Query.Load();
                if (branch.RowCount > 0)
                {
                    SalesBranchTarget salesBranchTarget = new SalesBranchTarget();
                    salesBranchTarget.Where.Active.Value = true;
                    salesBranchTarget.Where.TargetID.Value = target.ID;
                    salesBranchTarget.Query.Load();
                    if (salesBranchTarget.RowCount > 0)
                    {
                        VSLocation = getDataFromJoinedTable(branch.DefaultView.Table, salesBranchTarget.DefaultView.Table);
                        //***load VSProds;
                        rptrLocations.DataSource = VSLocation;
                        rptrLocations.DataBind();
                    }
                }
                else
                {
                    rptrLocations.DataSource = null;
                    rptrLocations.DataBind();
                }
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
            }
        }
        //****product editable load
        //private DataTable getProdData()
        //{
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("products", typeof(string));
        //    dt.Columns.Add("percentage", typeof(double));
        //    dt.Columns.Add("amount", typeof(double));
        //    dt.Columns.Add("selectedProdIndx", typeof(int));
        //    dt.Columns.Add("selectedValue", typeof(long));
        //    dt.Columns.Add("selectedProdTotalIndx", typeof(int));
        //    dt.Columns.Add("branchID", typeof(long));
        //    dt.Columns.Add("disabledIDs", typeof(string));
        //    dt.AcceptChanges();
        //}

        private DataTable getDataFromJoinedTable(DataTable branches, DataTable branchesTarget)
        {
            DataTable newTable = new DataTable();
            newTable.Columns.Add("ID", typeof(long));
            newTable.Columns.Add("Name", typeof(string));
            newTable.Columns.Add("percentage", typeof(double));
            newTable.Columns.Add("amount", typeof(double));
            newTable.Columns.Add("currency", typeof(string));
            newTable.AcceptChanges();
            //double equal_percent = 100.0 / table.Rows.Count;
            foreach (DataRow dr in branches.Rows)
            {
                DataRow newRow = newTable.NewRow();
                newRow["ID"] = long.Parse(dr["ID"].ToString());
                newRow["Name"] = dr["Name"].ToString();
                var result = branchesTarget.Select("BranchID = " + dr["ID"].ToString());
                if (result.Length > 0)
                {
                    newRow["percentage"] = (double)result[0]["Percentage"];
                    newRow["amount"] = (decimal)result[0]["Amount"];
                }
                else
                {
                    newRow["percentage"] = 0;
                    newRow["amount"] = 0;
                }
                newRow["currency"] = "LE";//***to be changed and read from DB.
                newTable.Rows.Add(newRow);
            }
            newTable.AcceptChanges();
            return newTable;
        }

        private SalesTarget LoadYearTarget()
        {
            SalesTarget salesTarget = new SalesTarget();
            salesTarget.Where.Active.Value = true;
            salesTarget.Where.Year.Value = ddlTargetYear.SelectedValue;
            salesTarget.Query.Load();
            if (salesTarget.RowCount > 0)
            {
                return salesTarget;
            }
            return null;
        }

        private void LoadTargetYears()
        {
            int lastYear = getLastTargetYear();
            List<int> years = new List<int>();
            for (int i = 0; i < 3; i++)
            {
                years.Add(DateTime.Now.AddYears(i).Year);
            }
            ddlTargetYear.DataSource = years;
            ddlTargetYear.DataBind();
            int indxLastYear = years.IndexOf(lastYear);
            ddlTargetYear.SelectedIndex = (indxLastYear < 0 || indxLastYear + 1 == years.Count) ? 0 : indxLastYear + 1;
            ddlTargetYear_SelectedIndexChanged(null, null);
        }

        private int getLastTargetYear()
        {
            SalesTarget salesTarget = new SalesTarget();
            salesTarget.Where.Active.Value = 1;
            salesTarget.Query.AddOrderBy(SalesTarget.ColumnNames.Year, WhereParameter.Dir.DESC);
            salesTarget.Query.Top = 1;
            salesTarget.Query.Load();
            if (salesTarget.RowCount > 0)
            {
                return salesTarget.Year;
            }
            return DateTime.Now.Year - 1;
        }

        private void LoadLast5Years()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("year", typeof(int));
            dt.Columns.Add("money", typeof(int));
            dt.Columns.Add("currency", typeof(string));

            dt.AcceptChanges();
            SalesTarget target = new SalesTarget();
            //target.Where.Active.Value = 1;
            target.Query.AddOrderBy(SalesTarget.ColumnNames.Year, WhereParameter.Dir.DESC);
            target.Query.Top = 5;
            target.Query.Load();
            if(target.RowCount > 0)
            {
                target.Rewind();
                do
                {
                    DataRow dr = dt.NewRow();
                    dr["year"] = target.Year;
                    dr["money"] = target.Target;
                    dr["currency"] = getCurrency(target.CurrencyID);
                    dt.Rows.Add(dr);
                } while (target.MoveNext());
            }
            //for (int i = 0; i < 5; i++)
            //{
            //    DataRow dr = dt.NewRow();
            //    dr["year"] = 2017 - i;
            //    dr["money"] = 20000;
            //    dr["currency"] = "LE";
            //    dt.Rows.Add(dr);
            //}
            dt.AcceptChanges();
            rptrLast5Years.DataSource = dt;
            rptrLast5Years.DataBind();
        }

        private string getCurrency(int currencyID)
        {
            string curShortName = "";
            Currency cur = new Currency();
            cur.LoadByPrimaryKey(currencyID);
            if(cur.RowCount > 0)
            {
                curShortName = cur.ShortName;
            }
            return curShortName;
        }

        private void LoadTopProfit()
        {

        }

        private void LoadTopSelling()
        {

        }

        private void LoadLocations()
        {
            try
            {
                Branch branch = new Branch();
                branch.Where.Active.Value = 1;
                branch.Query.Load();
                if (branch.RowCount > 0)
                {
                    VSLocation = getDataFromTable(branch.DefaultView.Table);
                    rptrLocations.DataSource = VSLocation;
                    rptrLocations.DataBind();
                }
                else
                {
                    rptrLocations.DataSource = null;
                    rptrLocations.DataBind();
                }
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
            }
        }

        private DataTable getDataFromTable(DataTable table)
        {
            DataTable newTable = new DataTable();
            newTable.Columns.Add("ID", typeof(long));
            newTable.Columns.Add("Name", typeof(string));
            newTable.Columns.Add("percentage", typeof(double));
            newTable.Columns.Add("amount", typeof(double));
            newTable.Columns.Add("currency", typeof(string));
            newTable.AcceptChanges();
            double equal_percent = 100.0 / table.Rows.Count;
            foreach (DataRow dr in table.Rows)
            {
                DataRow newRow = newTable.NewRow();
                newRow["ID"] = long.Parse(dr["ID"].ToString());
                newRow["Name"] = dr["Name"].ToString();
                newRow["percentage"] = equal_percent;
                newRow["amount"] = 0;
                newRow["currency"] = "LE";//***to be changed and read from DB.
                newTable.Rows.Add(newRow);
            }
            newTable.AcceptChanges();
            return newTable;
        }

        protected void ddlTargetYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblPlanYear.Text = ddlTargetYear.SelectedItem.Text;
            lblTargetYear.Text = ddlTargetYear.SelectedItem.Text;
            LoadTarget();
        }

        protected void rptrLast5Years_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    DataRowView yearStatisticsRow = ((DataRowView)e.Item.DataItem);
                    Label lblyearRep = ((Label)e.Item.FindControl("lblYear"));
                    Label lblmoneyRep = ((Label)e.Item.FindControl("lblMoney"));
                    Label lblcurrencyRep = ((Label)e.Item.FindControl("lblCurrency"));
                    lblyearRep.Text = yearStatisticsRow["year"].ToString();
                    lblmoneyRep.Text = yearStatisticsRow["money"].ToString();
                    lblcurrencyRep.Text = yearStatisticsRow["currency"].ToString();
                }
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
            }
        }

        protected void rptrTopSelling_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }

        protected void rptrTopProfit_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }

        protected void rptrLocations_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    DataRowView branchRow = ((DataRowView)e.Item.DataItem);
                    HiddenField hdnLocIDRep = ((HiddenField)e.Item.FindControl("hdnLocID"));
                    Label lblBranchNameRep = ((Label)e.Item.FindControl("lblBranchName"));
                    TextBox tbxPercentRep = ((TextBox)e.Item.FindControl("tbxPercent"));
                    TextBox tbxAmountRep = ((TextBox)e.Item.FindControl("tbxAmount"));
                    Label lblCurrencyRep = ((Label)e.Item.FindControl("lblCurrency"));
                    hdnLocIDRep.Value = branchRow["ID"].ToString();
                    lblBranchNameRep.Text = branchRow["Name"].ToString();
                    lblCurrencyRep.Text = branchRow["currency"].ToString();
                    tbxPercentRep.Text = branchRow["percentage"].ToString();
                    tbxAmountRep.Text = branchRow["amount"].ToString();
                    /*tbxPercentRep.Attributes.Add("onkeyup",
                        "changeNextTbxs(totalID,'percent'," + e.Item.ItemIndex + ",'fixed','percent');");
                    tbxAmountRep.Attributes.Add("onkeyup",
                        "changeNextTbxs(totalID,'fixed'," + e.Item.ItemIndex + ",'fixed','percent');");*/
                }
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
            }
        }

        protected void chbxProdPlan_CheckedChanged(object sender, EventArgs e)
        {
            if (chbxProdPlan.Checked)
            {
                //divProductDetails.Style.Add("display", "block");
                divProductDetails.Visible = true;
                drawProdPlanChart();
            }
            else
            {
                //divProductDetails.Style.Add("display", "none");
                divProductDetails.Visible = false;
            }
        }

        private void LoadProdByLoc()
        {
            rebindRepeaters();
        }

        private void rebindRepeaters()
        {
            rptrProdLocation.DataSource = VSLocation;
            rptrProdLocation.DataBind();
            rptrLocations.DataSource = VSLocation;
            rptrLocations.DataBind();
            updtPnlLocations.Update();
            updtPnlProdPlan.Update();
        }

        //protected void rptrProdLocation_ItemDataBound(object sender, RepeaterItemEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //        {
        //            DataRowView branchRow = ((DataRowView)e.Item.DataItem);
        //            HiddenField hdnLocIDRep = ((HiddenField)e.Item.FindControl("hdnLocID"));
        //            Label lblProdLocationRep = ((Label)e.Item.FindControl("lblProdLocation"));
        //            Label lblProdLocTargetRep = ((Label)e.Item.FindControl("lblProdLocTarget"));
        //            Label lblCurrencyRep = ((Label)e.Item.FindControl("lblCurrency"));
        //            Repeater rptrProds = ((Repeater)e.Item.FindControl("rptrProducts"));
        //            hdnLocIDRep.Value = branchRow["ID"].ToString();
        //            lblProdLocationRep.Text = branchRow["Name"].ToString();
        //            lblProdLocTargetRep.Text = branchRow["amount"].ToString();
        //            lblCurrencyRep.Text = branchRow["currency"].ToString();
        //            BranchProduct branchProd = new BranchProduct();
        //            branchProd.Where.Active.Value = 1;
        //            branchProd.Where.BranchID.Value = long.Parse(branchRow["ID"].ToString());
        //            branchProd.Query.Load();
        //            if (branchProd.RowCount > 0)
        //            {
        //                //long[] prodIDs = new long[branchProd.RowCount];
        //                StringBuilder strproducts = new StringBuilder();
        //                StringBuilder strproductsNames = new StringBuilder();
        //                long prodsCount = 0;
        //                long[] prodIDs = new long[branchProd.DefaultView.Table.Rows.Count];
        //                foreach (DataRow dr in branchProd.DefaultView.Table.Rows)
        //                {
        //                    prodIDs[prodsCount] = (long)dr["ProductID"];
        //                    strproducts.Append(dr["ProductID"].ToString() + "#-#");
        //                    strproductsNames.Append(getProdName((long)dr["ProductID"]) + "#-#");
        //                    prodsCount++;
        //                }
        //                //ProdIDs = prodIDs;
        //                DataTable dt = null;
        //                if (VSProds.Rows.Count == 0)
        //                {
        //                    dt = new DataTable();
        //                    dt.Columns.Add("products", typeof(string));
        //                    dt.Columns.Add("percentage", typeof(double));
        //                    dt.Columns.Add("amount", typeof(double));
        //                    dt.Columns.Add("selectedProdIndx", typeof(int));
        //                    dt.Columns.Add("selectedValue", typeof(long));
        //                    dt.Columns.Add("selectedProdTotalIndx", typeof(int));
        //                    dt.Columns.Add("branchID", typeof(long));
        //                    dt.Columns.Add("disabledIDs", typeof(string));
        //                    dt.AcceptChanges();
        //                }
        //                else
        //                {
        //                    dt = VSProds;
        //                }
        //                var rows = dt.Select("branchID = " + branchRow["ID"].ToString());
        //                foreach (var row in rows)
        //                    row.Delete();
        //                //if (dt.Select("branchID = " + branchRow["ID"].ToString()).Length == 0)
        //                //{
        //                int numProds = (int)Math.Min(5, prodsCount);
        //                for (int i = 0; i < numProds; i++)
        //                {
        //                    DataRow dr = dt.NewRow();
        //                    dr["products"] = strproducts.ToString() + "#+#" + strproductsNames.ToString();
        //                    dr["percentage"] = 100.0 / numProds;
        //                    dr["amount"] = 100.0 / numProds / 100.0 * ((double)branchRow["amount"]);
        //                    dr["selectedProdIndx"] = 0;
        //                    dr["selectedProdTotalIndx"] = i;
        //                    dr["selectedValue"] = prodIDs[i];
        //                    dr["branchID"] = branchRow["ID"];
        //                    int[] disabledIDs1 = Enumerable.Range(0, i).ToArray();
        //                    int[] disabledIDs2 = Enumerable.Range(i + 1, numProds - i - 1).ToArray();
        //                    int[] disabledIDstotal = new int[disabledIDs1.Length + disabledIDs2.Length];
        //                    disabledIDs1.CopyTo(disabledIDstotal, 0);
        //                    disabledIDs2.CopyTo(disabledIDstotal, disabledIDs1.Length);
        //                    dr["disabledIDs"] = string.Join(",", disabledIDstotal) + ",";

        //                    dt.Rows.Add(dr);
        //                }
        //                dt.AcceptChanges();
        //                //}
        //                VSProds = dt;
        //                rptrProds.DataSource = dt.Select("branchID = " + branchRow["ID"].ToString());
        //                rptrProds.DataBind();
        //            }
        //            else
        //            {
        //                rptrProds.DataSource = null;
        //                rptrProds.DataBind();
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //throw ex;
        //        //log exception
        //    }
        //}

        protected void rptrProdLocation_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    DataRowView branchRow = ((DataRowView)e.Item.DataItem);
                    HiddenField hdnLocIDRep = ((HiddenField)e.Item.FindControl("hdnLocID"));
                    Label lblProdLocationRep = ((Label)e.Item.FindControl("lblProdLocation"));
                    Label lblProdLocTargetRep = ((Label)e.Item.FindControl("lblProdLocTarget"));
                    Label lblCurrencyRep = ((Label)e.Item.FindControl("lblCurrency"));
                    Repeater rptrProds = ((Repeater)e.Item.FindControl("rptrProducts"));
                    hdnLocIDRep.Value = branchRow["ID"].ToString();
                    lblProdLocationRep.Text = branchRow["Name"].ToString();
                    lblProdLocTargetRep.Text = branchRow["amount"].ToString();
                    lblCurrencyRep.Text = branchRow["currency"].ToString();
                    BranchProduct branchProd = new BranchProduct();
                    branchProd.Where.Active.Value = 1;
                    branchProd.Where.BranchID.Value = long.Parse(branchRow["ID"].ToString());
                    branchProd.Query.Load();
                    if (branchProd.RowCount > 0)
                    {
                        DataTable dt = null;
                        if (VSProdsNew.Rows.Count == 0)
                        {
                            dt = new DataTable();
                            dt.Columns.Add("productID", typeof(long));
                            dt.Columns.Add("productName", typeof(string));
                            dt.Columns.Add("percentage", typeof(double));
                            dt.Columns.Add("amount", typeof(double));
                            dt.Columns.Add("branchID", typeof(long));
                            dt.AcceptChanges();
                        }
                        else
                        {
                            dt = VSProdsNew;
                        }
                        var rows = dt.Select("branchID = " + branchRow["ID"].ToString());
                        foreach (var row in rows)
                            row.Delete();
                        //if (dt.Select("branchID = " + branchRow["ID"].ToString()).Length == 0)
                        //{
                        int targetID = getTargetID();
                        do
                        {
                            DataRow dr = dt.NewRow();
                            dr["productID"] = branchProd.ProductID;
                            dr["productName"] = getProdName(branchProd.ProductID);
                            double[] prodTarget = new double[] { -1.0, -1.0 };
                            if(targetID != -1)
                                prodTarget = getProdTarget(targetID,(long)branchRow["ID"], branchProd.ProductID);
                            prodTarget[0] = prodTarget[0] == -1 ? (100.0 / branchProd.RowCount) : prodTarget[0];
                            dr["percentage"] = prodTarget[0];
                            prodTarget[1] = prodTarget[1] == -1 ? (100.0 / branchProd.RowCount / 100.0 * ((double)branchRow["amount"])) : prodTarget[1];
                            dr["amount"] = prodTarget[1];
                            dr["branchID"] = branchRow["ID"];
                            dt.Rows.Add(dr);
                        } while (branchProd.MoveNext());
                        dt.AcceptChanges();
                        //}
                        VSProdsNew = dt;
                        rptrProds.DataSource = dt.Select("branchID = " + branchRow["ID"].ToString());
                        rptrProds.DataBind();
                    }
                    else
                    {
                        rptrProds.DataSource = null;
                        rptrProds.DataBind();
                    }

                }
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
            }
        }

        private double[] getProdTarget(int targetID, long branchID, long productID)
        {
            double[] target = new double[] { -1.0, -1.0 };
            SalesBranchProductTarget prodTarget = new SalesBranchProductTarget();
            prodTarget.Where.Active.Value = 1;
            prodTarget.Where.BranchID.Value = branchID;
            prodTarget.Where.TargetID.Value = targetID;
            prodTarget.Where.ProductID.Value = productID;
            prodTarget.Query.Load();
            if(prodTarget.RowCount > 0)
            {
                target[0] = prodTarget.Percentage;
                target[1] = (double)prodTarget.Amount;
            }
            return target;
        }

        private int getTargetID()
        {
            SalesTarget salesTarget = new SalesTarget();
            salesTarget.Where.Active.Value = 1;
            salesTarget.Where.Year.Value = ddlTargetYear.SelectedItem.Text;
            salesTarget.Query.Load();
            if(salesTarget.RowCount > 0)
            {
                return salesTarget.ID;
            }
            return -1;
        }

        private string getProdName(long prodID)
        {
            string prodName = "Dummy";
            Product prod = new Product();
            prod.Where.ID.Value = prodID;
            prod.Query.Load();
            if (prod.RowCount > 0)
            {
                prodName = prod.Name;
            }
            return prodName;
        }

        //protected void rptrProducts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //        {
        //            DataRow branchProdRow = ((DataRow)e.Item.DataItem);
        //            string[] arrdisabledIDs = branchProdRow["disabledIDs"].ToString().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        //            string[] products = branchProdRow["products"].ToString().Split(new string[] { "#+#" }, StringSplitOptions.RemoveEmptyEntries);
        //            string[] productsIDs = products[0].Split(new string[] { "#-#" }, StringSplitOptions.RemoveEmptyEntries);
        //            string[] productsNames = products[1].Split(new string[] { "#-#" }, StringSplitOptions.RemoveEmptyEntries);
        //            double amount = (double)branchProdRow["amount"];
        //            double percent = (double)branchProdRow["percentage"];
        //            int selectedIndex = (int)branchProdRow["selectedProdIndx"];
        //            long selectedValue = (long)branchProdRow["selectedValue"];
        //            DropDownList ddlProductsRep = ((DropDownList)e.Item.FindControl("ddlProducts"));
        //            TextBox tbxAmount = ((TextBox)e.Item.FindControl("tbxProdFixed"));
        //            TextBox tbxPercent = ((TextBox)e.Item.FindControl("tbxProdPercent"));
        //            tbxAmount.Text = amount.ToString();
        //            tbxPercent.Text = percent.ToString();
        //            Dictionary<string, bool> disabledindeces = arrdisabledIDs.Distinct().ToDictionary(x => x, x => true);
        //            for (int i = 0; i < productsIDs.Length; i++)
        //            {
        //                if (!disabledindeces.ContainsKey(i.ToString()))
        //                {
        //                    ddlProductsRep.Items.Add(new ListItem(productsNames[i], productsIDs[i]));
        //                }
        //            }
        //            //ddlProductsRep.DataSource = products;
        //            //ddlProductsRep.DataBind();
        //            ddlProductsRep.SelectedValue = selectedValue.ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //throw ex;
        //        //log error
        //    }
        //}


        protected void rptrProducts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    DataRow branchProdRow = ((DataRow)e.Item.DataItem);
                    double amount = (double)branchProdRow["amount"];
                    double percent = (double)branchProdRow["percentage"];
                    Label lblProdNameRep = ((Label)e.Item.FindControl("lblProdName"));
                    HiddenField hdnProdIDRep = ((HiddenField)e.Item.FindControl("hdnProdID"));
                    TextBox tbxAmount = ((TextBox)e.Item.FindControl("tbxProdFixed"));
                    TextBox tbxPercent = ((TextBox)e.Item.FindControl("tbxProdPercent"));
                    tbxAmount.Text = amount.ToString();
                    tbxPercent.Text = percent.ToString();
                    lblProdNameRep.Text = branchProdRow["productName"].ToString();
                    hdnProdIDRep.Value = branchProdRow["productID"].ToString();
                }
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            decimal target = 0;
            decimal.TryParse(tbxYearTarget.Text, out target);
            if (tbxYearTarget.Text != "" && target != 0)
            {
                if (checkPercentages())
                {
                    int targetID = createByLocation();
                    if (chbxProdPlan.Checked)
                        createProductTarget(targetID);
                    sendBranchNotifications(targetID);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "gotohome",
                                                    "window.location.href='/Sales/SalesHome.aspx';", true);
                    //Response.Redirect("/Sales/SalesHome.aspx", false);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "percentageError", "alert('Percentages donot sum to 100%');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page),
                    "targetZeroError", "alert('Target Cannot be set to zero.');", true);
            }
        }

        private void sendBranchNotifications(int targetID)
        {
            Group gp = new Group();
            gp.Where.Name.Value = "SalesManagers";
            gp.Where.Active.Value = 1;
            gp.Query.Load();
            if (gp.RowCount > 0)
            {
                Group_User gpUser = new Group_User();
                gpUser.Where.Active.Value = 1;
                gpUser.Where.GroupID.Value = gp.ID;
                gpUser.Query.Load();
                string titleNotify = " Target Distribution";
                string descNotify = "New target distribution";
                if (btnCreate.Text == "Edit")
                {
                    titleNotify = " Target Redistribution";
                    descNotify = "Previous target redistribution";
                }
                if (gpUser.RowCount > 0)
                {
                    string URL = System.Web.Configuration.WebConfigurationManager.AppSettings["RootWeb"].ToString();
                    gpUser.Rewind(); //move to first record
                    do
                    {
                        Common.sendNotifications(gpUser.UserID, ddlTargetYear.SelectedValue + titleNotify, descNotify,
                                          URL +"/Sales/Target/TargetDistribution.aspx?Target=" + Encrypt_Decrypt.Encrypt(targetID.ToString(), key));
                    } while (gpUser.MoveNext());
                }
            }
        }

        private bool checkPercentages()
        {
            DataTable locations = VSLocation;
            double percentLocs = 0;
            foreach (DataRow dr in locations.Rows)
            {
                percentLocs += (double)dr["percentage"];
            }

            DataTable prods = VSProdsNew;//VSProds;
            double percentProds = 0;
            foreach (DataRow dr in prods.Rows)
            {
                percentProds += (double)dr["percentage"];
            }
            DataTable branches = prods.DefaultView.ToTable(true, "branchID");
            percentProds /= branches.Rows.Count;

            if (Math.Abs(percentLocs - 100.0) > 0.0000000001 || Math.Abs(percentProds - 100.0) > 0.0000000001)
            {
                return false;
            }
            return true;
        }

        private void createProductTarget(int targetID)
        {
            DataTable dt = VSProdsNew;//VSProds
            int error = 0;
            int createdNew = 0;
            int updated = 0;
            foreach (DataRow dr in dt.Rows)
            {
                //string[] products = dr["products"].ToString().Split(new string[] { "#+#" }, StringSplitOptions.RemoveEmptyEntries);
                //string[] productsIDs = products[0].Split(new string[] { "#-#" }, StringSplitOptions.RemoveEmptyEntries);
                long prodID = (long)dr["productID"];//(long)dr["selectedValue"];
                int branchID = int.Parse(dr["branchID"].ToString());
                double amount = (double)dr["amount"];
                double percent = (double)dr["percentage"];
                SalesBranchProductTarget salesBranchProductTarget = searchForSalesProductTarget(targetID, branchID, prodID);
                if (salesBranchProductTarget == null)
                {
                    createOrUpdateSalesProdTarget(salesBranchProductTarget, targetID, branchID, prodID, amount, percent);
                    createdNew++;
                }
                else if (salesBranchProductTarget.Active == false)
                {
                    error++;
                }
                else
                {
                    createOrUpdateSalesProdTarget(salesBranchProductTarget, targetID, branchID, prodID, amount, percent);
                    updated++;
                }
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "salesProdTargetNotActive",/* "alert('"
                                            + createdNew + " new sales products targets are created, "
                                            + updated + " sales products targets updated, "
                                            + error + " sales products targets are not changed as they are inactive');"
                                            +*/ "window.location.href='/Sales/SalesHome.aspx';", true);
            //Response.Redirect("/Sales/SalesHome.aspx",false);

        }

        private void createOrUpdateSalesProdTarget(SalesBranchProductTarget salesBranchProductTarget, int targetID, int branchID, long prodID, double amount, double percent)
        {
            DateTime now = DateTime.Now;
            if (salesBranchProductTarget == null)
            {
                salesBranchProductTarget = new SalesBranchProductTarget();
                salesBranchProductTarget.AddNew();
                salesBranchProductTarget.CreatedBy = UserID;
                salesBranchProductTarget.CreationDate = now;
            }
            salesBranchProductTarget.TargetID = targetID;
            salesBranchProductTarget.BranchID = branchID;
            salesBranchProductTarget.ProductID = prodID;
            salesBranchProductTarget.Percentage = percent;
            salesBranchProductTarget.Amount = (decimal)amount;
            salesBranchProductTarget.Active = true;
            salesBranchProductTarget.CurrencyID = 1;//*** needs change from table currency
            salesBranchProductTarget.ModifiedBy = UserID;
            salesBranchProductTarget.Modified = now;

            salesBranchProductTarget.Save();
            salesBranchProductTarget.AcceptChanges();
        }

        private SalesBranchProductTarget searchForSalesProductTarget(int targetID, int branchID, long prodID)
        {
            SalesBranchProductTarget salesBranchProductTarget = new SalesBranchProductTarget();
            salesBranchProductTarget.Where.Active.Value = 1;
            salesBranchProductTarget.Where.BranchID.Value = branchID;
            salesBranchProductTarget.Where.TargetID.Value = targetID;
            salesBranchProductTarget.Where.ProductID.Value = prodID;
            salesBranchProductTarget.Query.Load();
            if (salesBranchProductTarget.RowCount == 0)
            {
                salesBranchProductTarget = null;
            }
            return salesBranchProductTarget;
        }

        private int createByLocation()
        {
            //Response.Write("search for target\n");
            SalesTarget salesTarget = searchForSalesTarget();
            int targetID = -1;
            if (salesTarget == null)// create new sales target
            {
                //Response.Write("going to create target\n");
                targetID = createSalesTarget();
                //Response.Write("target ID: " + targetID + " going to create branch target.\n");
                createSalesBranchTarget(targetID);
                ScriptManager.RegisterStartupScript(Page, typeof(Page)
                    , "salesTargetNotActive", "alert('Sales Target Successfully Added!');", true);
            }
            else if (salesTarget.Active == false)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page),
                    "salesTargetNotActive", "alert('Sales Target exists and is inactive!');", true);
            }
            else if (salesTarget.CanEdit == false)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page),
                     "salesTargetNotEdit", "alert('Sales Target exists and cannot be edited!');", true);
            }
            else// update sales target
            {
                updateSalesTarget(salesTarget);
                createSalesBranchTarget(salesTarget.ID);
                ScriptManager.RegisterStartupScript(Page, typeof(Page),
                    "salesTargetUpdated", "alert('Sales Target Successfully Updated!');", true);
                targetID = salesTarget.ID;
            }
            return targetID;
        }

        private void updateSalesTarget(SalesTarget salesTarget)
        {
            salesTarget.Target = (decimal)convertToDouble(tbxYearTarget.Text);
            salesTarget.Modified = UserID;
            salesTarget.ModifiedDate = DateTime.Now;
            salesTarget.Active = true;
            salesTarget.CurrencyID = 1;//*** from currency table after bind
            salesTarget.CanEdit = true;
            salesTarget.Save();
            salesTarget.AcceptChanges();
        }

        private SalesTarget searchForSalesTarget()
        {
            SalesTarget salesTarget = new SalesTarget();
            salesTarget.Where.Year.Value = int.Parse(ddlTargetYear.SelectedValue);
            //salesTarget.Where.Active.Value = 1;
            salesTarget.Query.Load();
            if (salesTarget.RowCount == 0)
            {
                salesTarget = null;
            }
            return salesTarget;
        }

        private void createSalesBranchTarget(int targetID)
        {
            try
            {
                //Response.Write(0+"\n");
                SalesBranchTarget salesBranchTarget = new SalesBranchTarget();
                //Response.Write(1 + "\n");
                DataTable dt = VSLocation;
                //Response.Write(2 + "\n");
                Dictionary<int, long> branchesNeedsUpdate = getBranchesbySalesTarget(targetID);
                //Response.Write(3 + "\n");
                foreach (DataRow dr in dt.Rows)
                {
                    //  Response.Write("ID: "+dr["ID"].ToString() + "\n");
                    //  Response.Write(4 + "\n");
                    DateTime creationDate = DateTime.Now;
                    int branchID = int.Parse(dr["ID"].ToString());
                    //Response.Write(5 + "\n");
                    if (!branchesNeedsUpdate.ContainsKey(branchID))
                    {
                        //  Response.Write(6 + "\n");
                        salesBranchTarget.AddNew();
                        //Response.Write(7 + "\n");
                        salesBranchTarget.CreatedBy = UserID;
                        //Response.Write(8 + "\n");
                        salesBranchTarget.CreationDate = creationDate;
                        //Response.Write(9 + "\n");
                        salesBranchTarget.TargetID = targetID;
                        //Response.Write(10 + "\n");
                        salesBranchTarget.BranchID = int.Parse(dr["ID"].ToString());
                        //Response.Write(11 + "\n");
                    }
                    else
                    {
                        salesBranchTarget.LoadByPrimaryKey(branchesNeedsUpdate[branchID]);
                        //Response.Write(12 + "\n");
                    }
                    //Response.Write(13 + "\n");
                    salesBranchTarget.Percentage = convertToDouble(dr["percentage"].ToString());
                    //Response.Write(14 + "\n");
                    salesBranchTarget.Amount = (decimal)convertToDouble(dr["amount"].ToString());
                    //Response.Write(15 + "\n");
                    salesBranchTarget.ModifiedBy = UserID;
                    //Response.Write(16 + "\n");
                    salesBranchTarget.Modified = creationDate;
                    //Response.Write(17 + "\n");
                    salesBranchTarget.Active = true;
                    //Response.Write(18 + "\n");
                    salesBranchTarget.CurrencyID = 1;//*** to be changed from currency table
                                                     //Response.Write(19 + "\n");
                    salesBranchTarget.Save();
                    //Response.Write(20 + "\n");
                }
                salesBranchTarget.AcceptChanges();
                //    Response.Write(21 + "\n");
            }
            catch (Exception ex)
            {
                //throw ex;
            }
        }

        private Dictionary<int, long> getBranchesbySalesTarget(int targetID)
        {
            Dictionary<int, long> loadedBranches = new Dictionary<int, long>();
            SalesBranchTarget salesBranchTarget = new SalesBranchTarget();
            salesBranchTarget.Where.TargetID.Value = targetID;
            salesBranchTarget.Where.Active.Value = 1;
            salesBranchTarget.Query.Load();
            if (salesBranchTarget.RowCount > 0)
            {
                salesBranchTarget.Rewind(); //move to first record
                do
                {
                    int branchID = salesBranchTarget.BranchID;
                    if (!loadedBranches.ContainsKey(branchID))
                        loadedBranches.Add(branchID, salesBranchTarget.ID);
                } while (salesBranchTarget.MoveNext());
            }
            return loadedBranches;
        }

        private int createSalesTarget()
        {
            SalesTarget salesTarget = new SalesTarget();
            salesTarget.AddNew();
            int year = int.Parse(ddlTargetYear.SelectedValue);
            salesTarget.Year = year;
            salesTarget.FromDate = new DateTime(year, 1, 1);
            salesTarget.ToDate = new DateTime(year, 12, 31);
            salesTarget.Target = (decimal)convertToDouble(tbxYearTarget.Text);
            salesTarget.CreatedBy = UserID;
            DateTime creationDate = DateTime.Now;
            salesTarget.CreationDate = creationDate;
            salesTarget.Modified = UserID;
            salesTarget.ModifiedDate = creationDate;
            salesTarget.Active = true;
            salesTarget.CurrencyID = 1;//*** from currency table after bind
            salesTarget.CanEdit = true;
            salesTarget.Save();
            salesTarget.AcceptChanges();
            return salesTarget.ID;
        }

        protected void tbxYearTarget_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dTable = VSLocation;
                for (int i = 0; i < dTable.Rows.Count; i++)
                {
                    DataRow dr = dTable.Rows[i];
                    double percent = 0;
                    string strPercent = dr["percentage"].ToString();
                    if (!String.IsNullOrEmpty(strPercent))
                    {
                        if (!double.TryParse(strPercent, out percent))
                            percent = 0;
                    }
                    dr["percentage"] = percent;
                    TextBox textBoxtotal = sender as TextBox;
                    string tbxValue = "";
                    if (textBoxtotal != null)
                    {
                        tbxValue = textBoxtotal.Text;
                    }
                    double totalAmount = 0;
                    if (!String.IsNullOrEmpty(tbxValue))
                    {
                        if (!double.TryParse(tbxValue, out totalAmount))
                            totalAmount = 0;
                    }
                    dr["amount"] = percent * totalAmount / 100.0;
                }
                dTable.AcceptChanges();
                VSLocation = dTable;
                rebindRepeaters();
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
            }
        }

        private double convertToDouble(string strDoubleVal)
        {
            double val = 0;
            if (!String.IsNullOrEmpty(strDoubleVal))
            {
                if (!double.TryParse(strDoubleVal, out val))
                    val = 0;
            }
            return val;
        }

        protected void tbxPercent_TextChanged(object sender, EventArgs e)
        {
            TextBox textBoxPercent = sender as TextBox;
            int tbxIndex = 0;
            string tbxPercentVal = "";
            if (textBoxPercent != null)//&& textBoxPercent.Text !="")
            {
                tbxPercentVal = textBoxPercent.Text == "" ? "0" : textBoxPercent.Text;
                tbxIndex = ((RepeaterItem)textBoxPercent.NamingContainer).ItemIndex;
                double percent = convertToDouble(tbxPercentVal);
                double total = convertToDouble(tbxYearTarget.Text);
                DataTable dt = VSLocation;
                DataRow dr = dt.Rows[tbxIndex];
                dr["amount"] = percent * total / 100.0;
                dr["percentage"] = percent;
                dt.AcceptChanges();
                double totalPercent = 0;
                for (int i = 0; i <= tbxIndex; i++)
                {
                    dr = dt.Rows[i];
                    totalPercent += convertToDouble(dr["percentage"].ToString());
                }
                double newPercentage = (100.0 - totalPercent) / (dt.Rows.Count - tbxIndex - 1);
                for (int i = tbxIndex + 1; i < dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i];

                    dr["percentage"] = newPercentage;
                    dr["amount"] = newPercentage * total / 100.0;
                }
                dt.AcceptChanges();
                VSLocation = dt;
                rebindRepeaters();
            }

        }

        protected void tbxAmount_TextChanged(object sender, EventArgs e)
        {
            TextBox textBoxPercent = sender as TextBox;
            int tbxIndex = 0;
            string tbxFixedVal = "";
            if (textBoxPercent != null)//&& textBoxPercent.Text != "")
            {
                tbxFixedVal = textBoxPercent.Text == "" ? "0" : textBoxPercent.Text;
                tbxIndex = ((RepeaterItem)textBoxPercent.NamingContainer).ItemIndex;
                double fixedAmount = convertToDouble(tbxFixedVal);
                double total = convertToDouble(tbxYearTarget.Text);
                DataTable dt = VSLocation;
                DataRow dr = dt.Rows[tbxIndex];
                dr["amount"] = fixedAmount;
                dr["percentage"] = fixedAmount / total * 100.0;
                dt.AcceptChanges();
                double totalPercent = 0;
                for (int i = 0; i <= tbxIndex; i++)
                {
                    dr = dt.Rows[i];
                    totalPercent += convertToDouble(dr["percentage"].ToString());
                }
                double newPercentage = (100.0 - totalPercent) / (dt.Rows.Count - tbxIndex - 1);
                for (int i = tbxIndex + 1; i < dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i];
                    dr["percentage"] = newPercentage;
                    dr["amount"] = newPercentage * total / 100.0;
                }
                dt.AcceptChanges();
                VSLocation = dt;
                rebindRepeaters();
            }

        }

        protected void ddlProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlProds = sender as DropDownList;
            int tbxIndex = 0;
            int newSelectedDDLIndx = 0;
            if (ddlProds != null)
            {
                //selectedDDLIndx = ddlProds.SelectedIndex;
                long selectedProdID = long.Parse(ddlProds.SelectedValue);
                RepeaterItem rptrProdItem = (RepeaterItem)ddlProds.NamingContainer;
                Repeater rptrProds = (Repeater)rptrProdItem.NamingContainer;
                RepeaterItem rptrProdLocItem = (RepeaterItem)rptrProds.NamingContainer;
                HiddenField hdnBranchID = (HiddenField)rptrProdLocItem.FindControl("hdnLocID");
                tbxIndex = rptrProdItem.ItemIndex;
                DataTable dt = VSProds;
                DataRow dr;// = dt.Rows[tbxIndex];
                int oldSelectedIndx = -1;// (int)dr["selectedProdIndx"];                
                int count = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i];
                    if (dr["branchID"].ToString() == hdnBranchID.Value)
                    {
                        if (count == tbxIndex)
                        {
                            string[] products = dr["products"].ToString().Split(new string[] { "#+#" }, StringSplitOptions.RemoveEmptyEntries);
                            string[] productsIDs = products[0].Split(new string[] { "#-#" }, StringSplitOptions.RemoveEmptyEntries);
                            oldSelectedIndx = (int)dr["selectedProdTotalIndx"];//(int)dr["selectedProdIndx"];
                            dr["selectedProdTotalIndx"] = Array.FindIndex(productsIDs, x => x == selectedProdID.ToString());
                            //dr["selectedProdIndx"] = selectedDDLIndx;
                            newSelectedDDLIndx = (int)dr["selectedProdTotalIndx"];
                            dr["selectedValue"] = selectedProdID;
                            break;
                        }
                        count++;
                    }
                }
                count = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i];
                    if (dr["branchID"].ToString() == hdnBranchID.Value)
                    {
                        dr["disabledIDs"] = dr["disabledIDs"].ToString().Replace(oldSelectedIndx + ",", "");
                        if (count != tbxIndex)
                        {
                            dr["disabledIDs"] = dr["disabledIDs"].ToString() + newSelectedDDLIndx + ",";
                        }
                        count++;
                    }
                }
                dt.AcceptChanges();
                VSProds = dt;
                rptrProds.DataSource = dt.Select("branchID = " + hdnBranchID.Value); ;
                rptrProds.DataBind();
            }
        }

        protected void tbxProdPercent_TextChanged(object sender, EventArgs e)
        {
            TextBox textBoxPercent = sender as TextBox;
            int tbxIndex = 0;
            string tbxPercentVal = "";
            if (textBoxPercent != null)//&& textBoxPercent.Text != "")
            {
                tbxPercentVal = textBoxPercent.Text == "" ? "0" : textBoxPercent.Text;
                RepeaterItem rptrProdItem = (RepeaterItem)textBoxPercent.NamingContainer;
                Repeater rptrProds = (Repeater)rptrProdItem.NamingContainer;
                RepeaterItem rptrProdLocItem = (RepeaterItem)rptrProds.NamingContainer;
                HiddenField hdnBranchID = (HiddenField)rptrProdLocItem.FindControl("hdnLocID");
                tbxIndex = rptrProdItem.ItemIndex;
                int parentLocIndx = rptrProdLocItem.ItemIndex;
                double percent = convertToDouble(tbxPercentVal);
                DataTable LocsDT = VSLocation;
                DataRow LocsDR = LocsDT.Rows[parentLocIndx];
                double total = convertToDouble(LocsDR["amount"].ToString());
                DataTable dt = VSProdsNew;// VSProds;
                DataRow dr = null;//dt.Rows[tbxIndex];
                int count = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i];
                    if (dr["branchID"].ToString() == hdnBranchID.Value)
                    {
                        if (count == tbxIndex)
                        {
                            break;
                        }
                        count++;
                    }
                }
                dr["amount"] = percent * total / 100.0;
                dr["percentage"] = percent;
                dt.AcceptChanges();
                double totalPercent = 0;
                for (int i = 0; i <= tbxIndex; i++)
                {
                    dr = dt.Rows[i];
                    if (dr["branchID"].ToString() == hdnBranchID.Value)
                        totalPercent += convertToDouble(dr["percentage"].ToString());
                }
                double newPercentage = (100.0 - totalPercent) / (dt.Select("branchID = " + hdnBranchID.Value).Length - tbxIndex - 1);
                for (int i = tbxIndex + 1; i < dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i];
                    if (dr["branchID"].ToString() == hdnBranchID.Value)
                    {
                        dr["percentage"] = newPercentage;
                        dr["amount"] = newPercentage * total / 100.0;
                    }
                }
                dt.AcceptChanges();
                VSProdsNew = dt;// VSProds = dt;
                rptrProds.DataSource = dt.Select("branchID = " + hdnBranchID.Value);
                rptrProds.DataBind();
            }
        }

        protected void tbxProdFixed_TextChanged(object sender, EventArgs e)
        {
            TextBox textBoxPercent = sender as TextBox;
            int tbxIndex = 0;
            string tbxFixedVal = "";
            if (textBoxPercent != null)//&& textBoxPercent.Text != "")
            {
                tbxFixedVal = textBoxPercent.Text == "" ? "0" : textBoxPercent.Text;
                RepeaterItem rptrProdItem = (RepeaterItem)textBoxPercent.NamingContainer;
                Repeater rptrProds = (Repeater)rptrProdItem.NamingContainer;
                RepeaterItem rptrProdLocItem = (RepeaterItem)rptrProds.NamingContainer;
                HiddenField hdnBranchID = (HiddenField)rptrProdLocItem.FindControl("hdnLocID");
                tbxIndex = rptrProdItem.ItemIndex;
                int parentLocIndx = rptrProdLocItem.ItemIndex;
                double fixedAmount = convertToDouble(tbxFixedVal);
                DataTable LocsDT = VSLocation;
                DataRow LocsDR = LocsDT.Rows[parentLocIndx];
                double total = convertToDouble(LocsDR["amount"].ToString());
                DataTable dt = VSProdsNew;// VSProds;
                DataRow dr = null;//dt.Rows[tbxIndex];
                int count = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i];
                    if (dr["branchID"].ToString() == hdnBranchID.Value)
                    {
                        if (count == tbxIndex)
                        {
                            break;
                        }
                        count++;
                    }
                }
                dr["amount"] = fixedAmount;
                dr["percentage"] = fixedAmount / total * 100.0;
                dt.AcceptChanges();
                double totalPercent = 0;
                for (int i = 0; i <= tbxIndex; i++)
                {
                    dr = dt.Rows[i];
                    if (dr["branchID"].ToString() == hdnBranchID.Value)
                        totalPercent += convertToDouble(dr["percentage"].ToString());
                }
                double newPercentage = (100.0 - totalPercent) / (dt.Select("branchID = " + hdnBranchID.Value).Length - tbxIndex - 1);
                for (int i = tbxIndex + 1; i < dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i];
                    if (dr["branchID"].ToString() == hdnBranchID.Value)
                    {
                        dr["percentage"] = newPercentage;
                        dr["amount"] = newPercentage * total / 100.0;
                    }
                }
                VSProdsNew = dt;// VSProds = dt;
                rptrProds.DataSource = dt.Select("branchID = " + hdnBranchID.Value);
                rptrProds.DataBind();
            }
        }
    }
}