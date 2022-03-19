using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GarasSales.Sales
{
    public partial class MyClients : System.Web.UI.Page
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

                    return long.Parse(GarasERP.Encrypt_Decrypt.Decrypt(id, key));
                }
            }
            set
            {
                Session["UserID"] = value;
            }
        }

        readonly PagedDataSource _pgsource = new PagedDataSource();
        int _firstIndex, _lastIndex;
        private int _pageSize = 20;
        private int CurrentPage
        {
            get
            {
                if (ViewState["CurrentPage"] == null)
                {
                    return 0;
                }
                return ((int)ViewState["CurrentPage"]);
            }
            set
            {
                ViewState["CurrentPage"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Session["UserID"] = 4;
                ViewState["myOrder"] = "ASC";
                BindRepeater(ViewState["myOrder"].ToString());
                //GetUserPermission();
                if (CanAddClient())
                    btnAddClient.Visible = true;
                    //hyprLnk.Visible = true;
                else
                    btnAddClient.Visible = false;
                    //hyprLnk.Visible = false;

            }
        }

        protected void BindRepeater(string key)
        {
            //ViewAllClientsAllBranches = 5
            //ViewAllClientsMyBranch = 6            
            bool UserInViewAllBranches = GarasERP.Common.CheckUserInRole(UserID, 5);
            bool UserInViewMyBranch = GarasERP.Common.CheckUserInRole(UserID, 6);
            ViewState["UserInViewAllBranches"] = UserInViewAllBranches;
            ViewState["UserInViewMyBranch"] = UserInViewMyBranch;
            //List<string> groups = new List<string>();
            //groups.Add("SalesMen");
            //bool UserInSalesMenGroup = GarasERP.Common.CheckUserInGroups(UserID, groups);


            if (UserInViewAllBranches)
            {
                GarasERP.Client client = new GarasERP.Client();
                if (Page.Request.QueryString != null && Page.Request.QueryString["CRM"] != null && Session["CRMIDs"] != null)
                {
                    client.Where.ID.Value = Session["CRMIDs"].ToString();
                    client.Where.ID.Operator = MyGeneration.dOOdads.WhereParameter.Operand.In;
                }
                
                    if (key == "ASC")
                        client.Query.AddOrderBy(GarasERP.Client.ColumnNames.Name, MyGeneration.dOOdads.WhereParameter.Dir.ASC);
                    else
                        client.Query.AddOrderBy(GarasERP.Client.ColumnNames.Name, MyGeneration.dOOdads.WhereParameter.Dir.DESC);
               
                //client.Where.SalesPersonID.Value = Session["UserID"]; //1;
                // client.LoadAll();
                client.Query.AddResultColumn(GarasERP.Client.ColumnNames.ID);
                client.Query.AddResultColumn(GarasERP.Client.ColumnNames.Name);
                client.Query.AddResultColumn(GarasERP.Client.ColumnNames.HasLogo);
                client.Query.Load();

                lblClientCount.Text = client.RowCount.ToString() + " clients";

                if (client.DefaultView != null && client.DefaultView.Count > 0)
                {
                    //rptrClientList.DataSource = client.DefaultView;
                    //rptrClientList.DataBind();
                    Session["CData"] = client.DefaultView.ToTable() ;

                    _pgsource.DataSource = client.DefaultView;
                    _pgsource.AllowPaging = true;
                    // Number of items to be displayed in the Repeater
                    _pgsource.PageSize = _pageSize;
                    _pgsource.CurrentPageIndex = CurrentPage;
                    // Keep the Total pages in View State
                    ViewState["TotalPages"] = _pgsource.PageCount;
                    // Example: "Page 1 of 10"
                    lblpage.Text = "Page " + (CurrentPage + 1) + " of " + _pgsource.PageCount;
                    // Enable First, Last, Previous, Next buttons
                    lbPrevious.Enabled = !_pgsource.IsFirstPage;
                    lbNext.Enabled = !_pgsource.IsLastPage;
                    lbFirst.Enabled = !_pgsource.IsFirstPage;
                    lbLast.Enabled = !_pgsource.IsLastPage;

                    // Bind data into repeater
                    rptrClientList.DataSource = _pgsource;
                    rptrClientList.DataBind();

                    // Call the function to do paging
                    HandlePaging();

                    noClients.Visible = false;
                }
                else
                {
                    rptrClientList.DataSource = null;
                    rptrClientList.DataBind();
                    Session["CData"] = null;
                    noClients.Visible = true;
                }
            }
            else if (UserInViewMyBranch)
            {
                GarasERP.User user = new GarasERP.User();
                user.Where.ID.Value = UserID;
                user.Query.AddResultColumn(GarasERP.User.ColumnNames.BranchID);
                user.Query.Load();
                ViewState["MyBranch"] = user.BranchID;
                if (user.DefaultView != null && user.DefaultView.Count > 0)
                {
                    GarasERP.V_Client_Useer clientView = new GarasERP.V_Client_Useer();
                    if (key == "ASC")
                        clientView.Query.AddOrderBy(GarasERP.Client.ColumnNames.Name, MyGeneration.dOOdads.WhereParameter.Dir.ASC);
                    else
                        clientView.Query.AddOrderBy(GarasERP.Client.ColumnNames.Name, MyGeneration.dOOdads.WhereParameter.Dir.DESC);
                    if (Page.Request.QueryString != null && Page.Request.QueryString["CRM"] != null && Session["CRMIDs"] != null)
                    {
                        clientView.Where.ID.Value = Session["CRMIDs"].ToString();
                        clientView.Where.ID.Operator = MyGeneration.dOOdads.WhereParameter.Operand.In;
                    }
                    else
                    {
                        //client.Where.SalesPersonID.Value = Session["UserID"]; //1;
                        clientView.Where.BranchID.Value = user.BranchID;
                    }
                    clientView.Query.AddResultColumn(GarasERP.V_Client_Useer.ColumnNames.ID);
                    clientView.Query.AddResultColumn(GarasERP.V_Client_Useer.ColumnNames.Name);
                    clientView.Query.AddResultColumn(GarasERP.V_Client_Useer.ColumnNames.HasLogo);
                    clientView.Query.Load();

                    lblClientCount.Text = clientView.RowCount.ToString() + " clients";

                    if (clientView.RowCount > 0)
                    {
                        //rptrClientList.DataSource = clientView.DefaultView;
                        //rptrClientList.DataBind();
                        Session["CData"] = clientView.DefaultView.ToTable();
                        noClients.Visible = false;


                        _pgsource.DataSource = clientView.DefaultView;
                        _pgsource.AllowPaging = true;
                        // Number of items to be displayed in the Repeater
                        _pgsource.PageSize = _pageSize;
                        _pgsource.CurrentPageIndex = CurrentPage;
                        // Keep the Total pages in View State
                        ViewState["TotalPages"] = _pgsource.PageCount;
                        // Example: "Page 1 of 10"
                        lblpage.Text = "Page " + (CurrentPage + 1) + " of " + _pgsource.PageCount;
                        // Enable First, Last, Previous, Next buttons
                        lbPrevious.Enabled = !_pgsource.IsFirstPage;
                        lbNext.Enabled = !_pgsource.IsLastPage;
                        lbFirst.Enabled = !_pgsource.IsFirstPage;
                        lbLast.Enabled = !_pgsource.IsLastPage;

                        // Bind data into repeater
                        rptrClientList.DataSource = _pgsource;
                        rptrClientList.DataBind();

                        // Call the function to do paging
                        HandlePaging();
                    }
                    else
                    {
                        rptrClientList.DataSource = null;
                        rptrClientList.DataBind();
                        Session["CData"] = null;
                        noClients.Visible = true;
                    }

                }

            }
            //else if(UserInSalesMenGroup)
            else
            {
                GarasERP.Client client = new GarasERP.Client();
                if (key == "ASC")
                    client.Query.AddOrderBy(GarasERP.Client.ColumnNames.Name, MyGeneration.dOOdads.WhereParameter.Dir.ASC);
                else
                    client.Query.AddOrderBy(GarasERP.Client.ColumnNames.Name, MyGeneration.dOOdads.WhereParameter.Dir.DESC);

                if (Page.Request.QueryString != null && Page.Request.QueryString["CRM"] != null && Session["CRMIDs"] != null)
                {
                    client.Where.ID.Value = Session["CRMIDs"].ToString();
                    client.Where.ID.Operator = MyGeneration.dOOdads.WhereParameter.Operand.In;
                }
                else
                {
                    client.Where.SalesPersonID.Value = UserID;//Session["UserID"]; //1;
                }
               
                client.Query.AddResultColumn(GarasERP.Client.ColumnNames.ID);
                client.Query.AddResultColumn(GarasERP.Client.ColumnNames.Name);
                client.Query.AddResultColumn(GarasERP.Client.ColumnNames.HasLogo);
                client.Query.Load();

                lblClientCount.Text = client.RowCount.ToString() + " clients";

                if (client.RowCount > 0)
                {
                    //rptrClientList.DataSource = client.DefaultView;
                    //rptrClientList.DataBind();
                    Session["CData"] = client.DefaultView.ToTable();
                    noClients.Visible = false;

                    _pgsource.DataSource = client.DefaultView;
                    _pgsource.AllowPaging = true;
                    // Number of items to be displayed in the Repeater
                    _pgsource.PageSize = _pageSize;
                    _pgsource.CurrentPageIndex = CurrentPage;
                    // Keep the Total pages in View State
                    ViewState["TotalPages"] = _pgsource.PageCount;
                    // Example: "Page 1 of 10"
                    lblpage.Text = "Page " + (CurrentPage + 1) + " of " + _pgsource.PageCount;
                    // Enable First, Last, Previous, Next buttons
                    lbPrevious.Enabled = !_pgsource.IsFirstPage;
                    lbNext.Enabled = !_pgsource.IsLastPage;
                    lbFirst.Enabled = !_pgsource.IsFirstPage;
                    lbLast.Enabled = !_pgsource.IsLastPage;

                    // Bind data into repeater
                    rptrClientList.DataSource = _pgsource;
                    rptrClientList.DataBind();

                    // Call the function to do paging
                    HandlePaging();
                }
                else
                {
                    rptrClientList.DataSource = null;
                    rptrClientList.DataBind();
                    Session["CData"] = null;
                    noClients.Visible = true;
                }
            }
            
               
               

            

        }

        private void HandlePaging()
        {
            var dt = new DataTable();
            dt.Columns.Add("PageIndex"); //Start from 0
            dt.Columns.Add("PageText"); //Start from 1

            _firstIndex = CurrentPage - 5;
            if (CurrentPage > 5)
                _lastIndex = CurrentPage + 5;
            else
                _lastIndex = 10;

            // Check last page is greater than total page then reduced it 
            // to total no. of page is last index
            if (_lastIndex > Convert.ToInt32(ViewState["TotalPages"]))
            {
                _lastIndex = Convert.ToInt32(ViewState["TotalPages"]);
                _firstIndex = _lastIndex - 10;
            }

            if (_firstIndex < 0)
                _firstIndex = 0;

            // Now creating page number based on above first and last page index
            for (var i = _firstIndex; i < _lastIndex; i++)
            {
                var dr = dt.NewRow();
                dr[0] = i;
                dr[1] = i + 1;
                dt.Rows.Add(dr);
            }

            rptPaging.DataSource = dt;
            rptPaging.DataBind();
        }



        protected void rptrClientList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    DataRowView client = ((DataRowView)e.Item.DataItem);
                    System.Web.UI.WebControls.Image imgClient = ((System.Web.UI.WebControls.Image)e.Item.FindControl("imgClient"));
                    if (client["HasLogo"] != DBNull.Value && bool.Parse(client["HasLogo"].ToString()))
                    {
                        //var img = client["Logo"];
                        //string base64String = Convert.ToBase64String((byte[])img);
                        //imgClient.ImageUrl = "data:image/jpg;base64," + base64String;
                        imgClient.ImageUrl = "~/Common/ShowImage.ashx?ImageID="+client["ID"].ToString() +"&type=client";
                    }
                    else
                    {
                        imgClient.ImageUrl = "~/UI/Images/male.png";
                    }
                  //  Label lblClientName = ((Label)e.Item.FindControl("lblClientName"));
                    HyperLink lnkBtnView = ((HyperLink)e.Item.FindControl("lnkView"));
                   // lblClientName.Text = client["Name"].ToString();
                    lnkBtnView.NavigateUrl = EncryptRedirect(client["ID"].ToString());
                  //  //lblClientName.Text = GarasERP.Encrypt_Decrypt.Decrypt(getTaskLinkEnc(client["ID"].ToString()), "1");
                    lnkBtnView.Target = "_blank";
                }
            }
            catch (Exception ex)
            {
                //log exception
            }
        }

        private string EncryptRedirect(string clientID)
        {
            string url = "~/Sales/Client/ClientProfile.aspx?CID=";
            string queryStr = GarasERP.Encrypt_Decrypt.Encrypt(clientID, key);
            return url + Server.UrlEncode(queryStr);
        }

        protected void btnSort_Click(object sender, EventArgs e)
        {
            if (ViewState["myOrder"].ToString() == "ASC")
            {
                // BindRepeater("DESC");
                if (Session["CData"] != null)
                {
                    DataTable DT = (DataTable)Session["CData"];
                    DT.DefaultView.Sort = "Name DESC";
                    //rptrClientList.DataSource = DT;
                    //rptrClientList.DataBind();
                    _pgsource.DataSource = DT.DefaultView;
                    _pgsource.AllowPaging = true;
                    // Number of items to be displayed in the Repeater
                    _pgsource.PageSize = _pageSize;
                    _pgsource.CurrentPageIndex = CurrentPage;
                    // Keep the Total pages in View State
                    ViewState["TotalPages"] = _pgsource.PageCount;
                    // Example: "Page 1 of 10"
                    lblpage.Text = "Page " + (CurrentPage + 1) + " of " + _pgsource.PageCount;
                    // Enable First, Last, Previous, Next buttons
                    lbPrevious.Enabled = !_pgsource.IsFirstPage;
                    lbNext.Enabled = !_pgsource.IsLastPage;
                    lbFirst.Enabled = !_pgsource.IsFirstPage;
                    lbLast.Enabled = !_pgsource.IsLastPage;

                    // Bind data into repeater
                    rptrClientList.DataSource = _pgsource;
                    rptrClientList.DataBind();

                    // Call the function to do paging
                    HandlePaging();

                    ViewState["myOrder"] = "DESC";
                }
            }

            else
            {
                if (Session["CData"] != null)
                {
                    DataTable DT = (DataTable)Session["CData"];
                    DT.DefaultView.Sort = "Name ASC";
                    //rptrClientList.DataSource = DT;
                    //rptrClientList.DataBind();

                    _pgsource.DataSource = DT.DefaultView;
                    _pgsource.AllowPaging = true;
                    // Number of items to be displayed in the Repeater
                    _pgsource.PageSize = _pageSize;
                    _pgsource.CurrentPageIndex = CurrentPage;
                    // Keep the Total pages in View State
                    ViewState["TotalPages"] = _pgsource.PageCount;
                    // Example: "Page 1 of 10"
                    lblpage.Text = "Page " + (CurrentPage + 1) + " of " + _pgsource.PageCount;
                    // Enable First, Last, Previous, Next buttons
                    lbPrevious.Enabled = !_pgsource.IsFirstPage;
                    lbNext.Enabled = !_pgsource.IsLastPage;
                    lbFirst.Enabled = !_pgsource.IsFirstPage;
                    lbLast.Enabled = !_pgsource.IsLastPage;

                    // Bind data into repeater
                    rptrClientList.DataSource = _pgsource;
                    rptrClientList.DataBind();

                    // Call the function to do paging
                    HandlePaging();

                    ViewState["myOrder"] = "ASC";
                }
                //BindRepeater("ASC");
                //ViewState["myOrder"] = "ASC";
            }

        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            CurrentPage = 0;
            if (Session["CData"] != null)
            {
                // lblClientCount.Text = "Mike";
                DataTable DT = (DataTable)Session["CData"];
                if (tbxFilter.Text.Trim() != "")
                {
                    DT.DefaultView.RowFilter = "Name like '%" + tbxFilter.Text.Trim() + "%'";
                }
                else
                    DT.DefaultView.RowFilter = "";
                //rptrClientList.DataSource = DT;
                //rptrClientList.DataBind();
                lblClientCount.Text = "";
                _pgsource.DataSource = DT.DefaultView;
                _pgsource.AllowPaging = true;
                // Number of items to be displayed in the Repeater
                _pgsource.PageSize = _pageSize;
                _pgsource.CurrentPageIndex = CurrentPage;
                // Keep the Total pages in View State
                ViewState["TotalPages"] = _pgsource.PageCount;
                // Example: "Page 1 of 10"
                lblpage.Text = "Page " + (CurrentPage + 1) + " of " + _pgsource.PageCount;
                // Enable First, Last, Previous, Next buttons
                lbPrevious.Enabled = !_pgsource.IsFirstPage;
                lbNext.Enabled = !_pgsource.IsLastPage;
                lbFirst.Enabled = !_pgsource.IsFirstPage;
                lbLast.Enabled = !_pgsource.IsLastPage;

                // Bind data into repeater
                rptrClientList.DataSource = _pgsource;
                rptrClientList.DataBind();

                // Call the function to do paging
                HandlePaging();

            }
        }

        //protected void btnFilter_Click(object sender, EventArgs e)
        //{
        //    if (tbxFilter.Text.Trim() != "")
        //    {

        //        // bool UserInViewAllBranches = GarasERP.Common.CheckUserInRole(UserID, 5);
        //        bool UserInViewAllBranches = (bool)ViewState["UserInViewAllBranches"];
        //        //List<string> groups = new List<string>();
        //        //groups.Add("SalesMen");
        //        //bool UserInSalesMenGroup = GarasERP.Common.CheckUserInGroups(UserID, groups);
        //        if (UserInViewAllBranches)
        //        {
        //            GarasERP.Client client = new GarasERP.Client();
        //            client.Where.Name.Value = "%" + tbxFilter.Text + "%";
        //            client.Where.Name.Operator = MyGeneration.dOOdads.WhereParameter.Operand.Like;
        //            client.Query.AddOrderBy(GarasERP.Client.ColumnNames.Name,
        //                MyGeneration.dOOdads.WhereParameter.Dir.ASC);
        //            //client.Where.SalesPersonID.Value = 1;// Session["UserID"];
        //            client.Query.AddResultColumn(GarasERP.Client.ColumnNames.ID);
        //            client.Query.AddResultColumn(GarasERP.Client.ColumnNames.Name);
        //            client.Query.AddResultColumn(GarasERP.Client.ColumnNames.HasLogo);
        //            client.Query.Load();

        //            lblClientCount.Text = client.RowCount == 1 ? client.RowCount.ToString() + " client" :
        //                client.RowCount.ToString() + " clients";

        //            if (client.RowCount > 0)
        //            {
        //                //rptrClientList.DataSource = client.DefaultView;
        //                //rptrClientList.DataBind();
        //                Session["CData"] = client.DefaultView.ToTable();
        //                noClients.Visible = false;

        //                _pgsource.DataSource = client.DefaultView;
        //                _pgsource.AllowPaging = true;
        //                // Number of items to be displayed in the Repeater
        //                _pgsource.PageSize = _pageSize;
        //                _pgsource.CurrentPageIndex = CurrentPage;
        //                // Keep the Total pages in View State
        //                ViewState["TotalPages"] = _pgsource.PageCount;
        //                // Example: "Page 1 of 10"
        //                lblpage.Text = "Page " + (CurrentPage + 1) + " of " + _pgsource.PageCount;
        //                // Enable First, Last, Previous, Next buttons
        //                lbPrevious.Enabled = !_pgsource.IsFirstPage;
        //                lbNext.Enabled = !_pgsource.IsLastPage;
        //                lbFirst.Enabled = !_pgsource.IsFirstPage;
        //                lbLast.Enabled = !_pgsource.IsLastPage;

        //                // Bind data into repeater
        //                rptrClientList.DataSource = _pgsource;
        //                rptrClientList.DataBind();

        //                // Call the function to do paging
        //                HandlePaging();
        //            }
        //            else
        //            {
        //                rptrClientList.DataSource = null;
        //                rptrClientList.DataBind();
        //                Session["CData"] = null;
        //                noClients.Visible = true;
        //            }
        //        }
        //        else
        //        {
        //            bool UserInViewMyBranch = (bool)ViewState["UserInViewMyBranch"];//GarasERP.Common.CheckUserInRole(UserID, 6);
        //            if (UserInViewMyBranch)
        //            {
        //                //GarasERP.User user = new GarasERP.User();
        //                //user.Where.ID.Value = UserID;
        //                //user.Query.Load();

        //                //if (user.DefaultView != null && user.DefaultView.Count > 0)
        //                //{
        //                GarasERP.V_Client_Useer clientView = new GarasERP.V_Client_Useer();
        //                if (key == "ASC")
        //                    clientView.Query.AddOrderBy(GarasERP.Client.ColumnNames.Name, MyGeneration.dOOdads.WhereParameter.Dir.ASC);
        //                else
        //                    clientView.Query.AddOrderBy(GarasERP.Client.ColumnNames.Name, MyGeneration.dOOdads.WhereParameter.Dir.DESC);
        //                //client.Where.SalesPersonID.Value = Session["UserID"]; //1;
        //                clientView.Where.Name.Value = "%" + tbxFilter.Text + "%";
        //                clientView.Where.BranchID.Value = ViewState["MyBranch"];//user.BranchID;
        //                clientView.Query.AddResultColumn(GarasERP.Client.ColumnNames.ID);
        //                clientView.Query.AddResultColumn(GarasERP.Client.ColumnNames.Name);
        //                clientView.Query.AddResultColumn(GarasERP.Client.ColumnNames.HasLogo);
        //                clientView.Query.Load();

        //                lblClientCount.Text = clientView.RowCount.ToString() + " clients";

        //                if (clientView.RowCount > 0)
        //                {
        //                    //rptrClientList.DataSource = clientView.DefaultView;
        //                    //rptrClientList.DataBind();
        //                    Session["CData"] = clientView.DefaultView.ToTable();
        //                    noClients.Visible = false;

        //                    _pgsource.DataSource = clientView.DefaultView;
        //                    _pgsource.AllowPaging = true;
        //                    // Number of items to be displayed in the Repeater
        //                    _pgsource.PageSize = _pageSize;
        //                    _pgsource.CurrentPageIndex = CurrentPage;
        //                    // Keep the Total pages in View State
        //                    ViewState["TotalPages"] = _pgsource.PageCount;
        //                    // Example: "Page 1 of 10"
        //                    lblpage.Text = "Page " + (CurrentPage + 1) + " of " + _pgsource.PageCount;
        //                    // Enable First, Last, Previous, Next buttons
        //                    lbPrevious.Enabled = !_pgsource.IsFirstPage;
        //                    lbNext.Enabled = !_pgsource.IsLastPage;
        //                    lbFirst.Enabled = !_pgsource.IsFirstPage;
        //                    lbLast.Enabled = !_pgsource.IsLastPage;

        //                    // Bind data into repeater
        //                    rptrClientList.DataSource = _pgsource;
        //                    rptrClientList.DataBind();

        //                    // Call the function to do paging
        //                    HandlePaging();
        //                }
        //                else
        //                {
        //                    rptrClientList.DataSource = null;
        //                    rptrClientList.DataBind();
        //                    Session["CData"] = null;
        //                    noClients.Visible = true;
        //                }

        //                // }
        //            }
        //            else
        //            {
        //                GarasERP.Client client = new GarasERP.Client();
        //                client.Where.Name.Value = "%" + tbxFilter.Text + "%";
        //                client.Where.Name.Operator = MyGeneration.dOOdads.WhereParameter.Operand.Like;
        //                client.Where.SalesPersonID.Value = UserID;
        //                client.Query.AddResultColumn(GarasERP.Client.ColumnNames.ID);
        //                client.Query.AddResultColumn(GarasERP.Client.ColumnNames.Name);
        //                client.Query.AddResultColumn(GarasERP.Client.ColumnNames.HasLogo);
        //                client.Query.Load();

        //                lblClientCount.Text = client.RowCount == 1 ? client.RowCount.ToString() + " client" :
        //                    client.RowCount.ToString() + " clients";

        //                if (client.RowCount > 0)
        //                {
        //                    //rptrClientList.DataSource = client.DefaultView;
        //                    //rptrClientList.DataBind();
        //                    Session["CData"] = client.DefaultView.ToTable();
        //                    noClients.Visible = false;
        //                    _pgsource.DataSource = client.DefaultView;
        //                    _pgsource.AllowPaging = true;
        //                    // Number of items to be displayed in the Repeater
        //                    _pgsource.PageSize = _pageSize;
        //                    _pgsource.CurrentPageIndex = CurrentPage;
        //                    // Keep the Total pages in View State
        //                    ViewState["TotalPages"] = _pgsource.PageCount;
        //                    // Example: "Page 1 of 10"
        //                    lblpage.Text = "Page " + (CurrentPage + 1) + " of " + _pgsource.PageCount;
        //                    // Enable First, Last, Previous, Next buttons
        //                    lbPrevious.Enabled = !_pgsource.IsFirstPage;
        //                    lbNext.Enabled = !_pgsource.IsLastPage;
        //                    lbFirst.Enabled = !_pgsource.IsFirstPage;
        //                    lbLast.Enabled = !_pgsource.IsLastPage;

        //                    // Bind data into repeater
        //                    rptrClientList.DataSource = _pgsource;
        //                    rptrClientList.DataBind();

        //                    // Call the function to do paging
        //                    HandlePaging();
        //                }
        //                else
        //                {
        //                    rptrClientList.DataSource = null;
        //                    rptrClientList.DataBind();
        //                    Session["CData"] = null;
        //                    noClients.Visible = true;
        //                }
        //            }


        //        }
        //        //else if (UserInSalesMenGroup)

        //    }
        //    else
        //        BindRepeater(ViewState["myOrder"].ToString());

        //}

        protected bool CanAddClient()
        {
            bool canAdd = GarasERP.Common.CheckUserInRole(UserID, 1);
            return canAdd;
        }

        protected void GetUserPermission()
        {
            GarasERP.UserRole userRole = new GarasERP.UserRole();
            userRole.Where.UserID.Value = UserID;
            userRole.Where.RoleID.Conjuction = MyGeneration.dOOdads.WhereParameter.Conj.And;
            userRole.Where.RoleID.Value = 1;
            userRole.Query.Load();

            ////userRole.Where.RoleID.Conjuction = MyGeneration.dOOdads.WhereParameter.Conj.Or;
            //userRole.Where.RoleID.Value = 1;

            if (userRole.RowCount > 0)
            {
                if (userRole.RoleID == 1)
                {
                    btnAddClient.Visible = true;
                    //hyprLnk.Visible = true;
                }
                else
                {
                    btnAddClient.Visible = false;
                    //hyprLnk.Visible = false;
                }

            }
            else
            {
                string GroupIDs = "";
                GarasERP.GroupRole groupRole = new GarasERP.GroupRole();
                groupRole.Where.RoleID.Value = 1;
                groupRole.Where.GroupID.Conjuction = MyGeneration.dOOdads.WhereParameter.Conj.And;

                GarasERP.Group_User groupUser = new GarasERP.Group_User();
                groupUser.Where.UserID.Value = UserID;
                if (groupUser.Query.Load())
                {
                    if (groupUser.DefaultView != null && groupUser.DefaultView.Count > 0)
                    {
                        do
                        {
                            if (GroupIDs == "")
                                GroupIDs = "'" + groupUser.GroupID + "'";
                            else
                                GroupIDs += ",'" + groupUser.GroupID + "'";

                        } while (groupUser.MoveNext());
                    }
                }

                groupRole.Where.GroupID.Value = GroupIDs;
                groupRole.Where.GroupID.Operator = MyGeneration.dOOdads.WhereParameter.Operand.In;
                if (GroupIDs != "")
                    groupRole.Query.Load();

                if (groupRole.RowCount > 0)
                {
                    btnAddClient.Visible = true;
                    //hyprLnk.Visible = true;
                }
                else
                {
                    btnAddClient.Visible = false;
                    //hyprLnk.Visible = false;
                }

            }


        }

        protected bool IsHighLevelPermission()
        {
            bool result = false;

            GarasERP.Group_User group = new GarasERP.Group_User();
            group.Where.UserID.Value = UserID;
            group.Query.Load();

            if (group.RowCount > 0)
            {
                if (group.GroupID == 1 || group.GroupID == 2)
                {
                    result = true;
                }
                else
                    result = false;
            }

            return result;
        }

        protected void lbFirst_Click(object sender, EventArgs e)
        {
            CurrentPage = 0;
            BindRepeater(ViewState["myOrder"].ToString());
        }

        protected void lbPrevious_Click(object sender, EventArgs e)
        {
            CurrentPage -= 1;
            BindRepeater(ViewState["myOrder"].ToString());
        }

        protected void rptPaging_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (!e.CommandName.Equals("newPage")) return;
            CurrentPage = Convert.ToInt32(e.CommandArgument.ToString());
            BindRepeater(ViewState["myOrder"].ToString());
        }

        protected void rptPaging_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            var lnkPage = (LinkButton)e.Item.FindControl("lbPaging");
            if (lnkPage.CommandArgument != CurrentPage.ToString()) return;
            lnkPage.Enabled = false;
            lnkPage.BackColor = Color.FromName("#00FF00");
        }

        protected void lbNext_Click(object sender, EventArgs e)
        {
            CurrentPage += 1;
            BindRepeater(ViewState["myOrder"].ToString());
        }

        protected void lbLast_Click(object sender, EventArgs e)
        {

            CurrentPage = (Convert.ToInt32(ViewState["TotalPages"]) - 1);
            BindRepeater(ViewState["myOrder"].ToString());
        }
    }
}