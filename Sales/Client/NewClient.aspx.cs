using GarasERP;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace GarasSales.Sales
{
    public partial class NewClient : System.Web.UI.Page
    {
        string selectedRadio = String.Empty;
        string selectedName = String.Empty;
        string selectedConsultantName = String.Empty;
        //DataTable dt = new DataTable();
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

        private DataTable VS_Speciality
        {
            get
            {
                if(ViewState["VS_Speciality"] == null)
                {
                    return new DataTable();
                }
                return (DataTable)ViewState["VS_Speciality"];
            }
            set
            {
                ViewState["VS_Speciality"] = value;
            }
        }

        private DataTable VS_Country
        {
            get
            {
                if (ViewState["VS_Country"] == null)
                {
                    return new DataTable();
                }
                return (DataTable)ViewState["VS_Country"];
            }
            set
            {
                ViewState["VS_Country"] = value;
            }
        }

        private DataTable VS_Consultant
        {
            get
            {
                if (ViewState["VS_Consultant"] == null)
                {
                    return new DataTable();
                }
                return (DataTable)ViewState["VS_Consultant"];
            }
            set
            {
                ViewState["VS_Consultant"] = value;
            }
        }

        private DataRow VS_Consultant_Footer
        {
            get
            {
                if (ViewState["VS_Consultant_Footer"] == null)
                {
                    return null;
                }
                return (DataRow)ViewState["VS_Consultant_Footer"];
            }
            set
            {
                ViewState["VS_Consultant_Footer"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Session["UserID"] = 1;
                if (GetUserPermission() == true)
                {
                    newClientPage.Visible = true;
                    noAccess.Visible = false;
                    if (!IsPostBack)
                    {
                        BindAllDDLs();
                        //InitializeConsultantDT();
                        //InitializeDataTable();
                        //ViewState["DT"] = dt;
                        //clientPhoneRepeater.DataSource = dt;
                        //clientPhoneRepeater.DataBind();
                    }
                    //dt = (DataTable)ViewState["DT"];                    
                    rebuildVS_Consultant(false);
                    LoadNameBasedOnType();
                    LoadConsultantDivIfAvail();
                }
                else
                {
                    newClientPage.Visible = false;
                    noAccess.Visible = true;
                }
            }
            catch(Exception ex)
            {
                
            }
        }


        //Show and hide textboxes & labels based on type chosen by user
        protected void LoadNameBasedOnType()
        {
            if (rbBig.Checked == true)
            {
                lblCompanyName.Visible = true;
                tbxCompanyName.Visible = true;

                lblBranch.Visible = true;
                tbxBranch.Visible = true;
                RFV_tbxBranch.Enabled = true;

                lblGroupName.Visible = false;
                tbxGroupName.Visible = false;
                RFV_tbxGroupName.Enabled = false;

                selectedRadio = "Big Company (Multiple Branches)";
                selectedName = tbxCompanyName.Text;
            }

            else if (rbCompanies.Checked == true)
            {
                lblGroupName.Visible = true;
                tbxGroupName.Visible = true;
                RFV_tbxGroupName.Enabled = true;

                lblCompanyName.Visible = true;
                tbxCompanyName.Visible = true;

                lblBranch.Visible = true;
                tbxBranch.Visible = true;
                RFV_tbxBranch.Enabled = true;

                selectedRadio = "Group of Companies";
                selectedName = tbxCompanyName.Text;
            }
            else if (rbSmall.Checked == true)
            {
                lblCompanyName.Visible = true;
                tbxCompanyName.Visible = true;

                lblGroupName.Visible = false;
                tbxGroupName.Visible = false;
                RFV_tbxGroupName.Enabled = false;

                lblBranch.Visible = false;
                tbxBranch.Visible = false;
                RFV_tbxBranch.Enabled = false;

                selectedRadio = "Small Company (One Branch)";
                selectedName = tbxCompanyName.Text;
            }
            else if (rbIndividual.Checked == true)
            {
                lblCompanyName.Visible = true;
                tbxCompanyName.Visible = true;

                lblGroupName.Visible = false;
                tbxGroupName.Visible = false;
                RFV_tbxGroupName.Enabled = false;

                lblBranch.Visible = false;
                tbxBranch.Visible = false;
                RFV_tbxBranch.Enabled = false;

                selectedRadio = "Individual";
                selectedName = lblCompanyName.Text;
            }
            else
            {
                lblGroupName.Visible = false;
                tbxGroupName.Visible = false;
                RFV_tbxGroupName.Enabled = false;
                lblCompanyName.Visible = false;
                tbxCompanyName.Visible = false;
                lblBranch.Visible = false;
                tbxBranch.Visible = false;
                RFV_tbxBranch.Enabled = false;

            }
        }

        protected void LoadConsultantDivIfAvail()
        {
            if (chbxAvailable.Checked != true)
            {
                rptrConsaltant.Visible = false;
            }
            else
            {
                rptrConsaltant.Visible = true;
            }
            updatePanelConsultantType.Update();
        }

        #region Binding DropDownLists

        protected void BindAllDDLs()
        {
            BindCountryDDL();
            BindGovDDL();
            BindGovDDL2();
            BindGovDDL3();
            //BindConsGovDDL();
            BindSalesMen();
            BindFollowUpPeriod();
            BindSpeciality();
            LoadJobTitle();
            LoadArea();
            LoadArea2();
            LoadArea3();
        }

        private void BindConsGovDDL(DropDownList ddlConsultantCountry, DropDownList ddlConsultantGovernorate)
        {
            GarasERP.Governorate governorate = new GarasERP.Governorate();
            //governorate.LoadAll();
            governorate.Where.CountryID.Value = ddlConsultantCountry.SelectedValue;
            governorate.Where.Active.Value = true;
            governorate.Query.AddOrderBy(Governorate.ColumnNames.Name, MyGeneration.dOOdads.WhereParameter.Dir.ASC);
            ddlConsultantGovernorate.Items.Clear();
            if (governorate.Query.Load())
            {

                ddlConsultantGovernorate.DataSource = governorate.DefaultView;
                ddlConsultantGovernorate.DataTextField = GarasERP.Governorate.ColumnNames.Name;
                ddlConsultantGovernorate.DataValueField = GarasERP.Governorate.ColumnNames.ID;
                ddlConsultantGovernorate.DataBind();
            }
        }

        protected void BindSpeciality()
        {
            GarasERP.Speciality sp = new GarasERP.Speciality();
            sp.Where.Active.Value = true;
            sp.Query.AddOrderBy(Speciality.ColumnNames.Name, MyGeneration.dOOdads.WhereParameter.Dir.ASC);
            if (sp.Query.Load())
            {

                ddlSpeciality.DataSource = sp.DefaultView;
                ddlSpeciality.DataTextField = GarasERP.Speciality.ColumnNames.Name;
                ddlSpeciality.DataValueField = GarasERP.Speciality.ColumnNames.ID;
                ddlSpeciality.DataBind();

                ddlSpeciality2.DataSource = sp.DefaultView;
                ddlSpeciality2.DataTextField = GarasERP.Speciality.ColumnNames.Name;
                ddlSpeciality2.DataValueField = GarasERP.Speciality.ColumnNames.ID;
                ddlSpeciality2.DataBind();

                ddlSpeciality3.DataSource = sp.DefaultView;
                ddlSpeciality3.DataTextField = GarasERP.Speciality.ColumnNames.Name;
                ddlSpeciality3.DataValueField = GarasERP.Speciality.ColumnNames.ID;
                ddlSpeciality3.DataBind();

                ddlSpeciality4.DataSource = sp.DefaultView;
                ddlSpeciality4.DataTextField = GarasERP.Speciality.ColumnNames.Name;
                ddlSpeciality4.DataValueField = GarasERP.Speciality.ColumnNames.ID;
                ddlSpeciality4.DataBind();

                VS_Speciality = sp.DefaultView.ToTable();                
            }
        }
        protected void BindCountryDDL()
        {
            Country country = new GarasERP.Country();
            country.Where.Active.Value = true;
            country.Query.AddOrderBy(Country.ColumnNames.Name, MyGeneration.dOOdads.WhereParameter.Dir.ASC);
            if (country.Query.Load())
            {

                ddlCountry.DataSource = country.DefaultView;
                ddlCountry.DataTextField = GarasERP.Country.ColumnNames.Name;
                ddlCountry.DataValueField = GarasERP.Country.ColumnNames.ID;
                ddlCountry.DataBind();



                ddlCountry2.DataSource = country.DefaultView;
                ddlCountry2.DataTextField = GarasERP.Country.ColumnNames.Name;
                ddlCountry2.DataValueField = GarasERP.Country.ColumnNames.ID;
                ddlCountry2.DataBind();



                ddlCountry3.DataSource = country.DefaultView;
                ddlCountry3.DataTextField = GarasERP.Country.ColumnNames.Name;
                ddlCountry3.DataValueField = GarasERP.Country.ColumnNames.ID;
                ddlCountry3.DataBind();

                VS_Country = country.DefaultView.ToTable();                
            }
        }

        protected void BindGovDDL()
        {
            GarasERP.Governorate governorate = new GarasERP.Governorate();
            //governorate.LoadAll();
            governorate.Where.CountryID.Value = ddlCountry.SelectedValue;
            governorate.Where.Active.Value = true;
            governorate.Query.AddOrderBy(Governorate.ColumnNames.Name, MyGeneration.dOOdads.WhereParameter.Dir.ASC);
            ddlGovernate.Items.Clear();
            if (governorate.Query.Load())
            {

                ddlGovernate.DataSource = governorate.DefaultView;
                ddlGovernate.DataTextField = GarasERP.Governorate.ColumnNames.Name;
                ddlGovernate.DataValueField = GarasERP.Governorate.ColumnNames.ID;
                ddlGovernate.DataBind();
            }


            //Governorate consultantGovernorate = new GarasERP.Governorate();
            //consultantGovernorate.Where.CountryID.Value = ddlConsultantCountry.SelectedValue;
            //consultantGovernorate.Query.Load();

            //ddlConsultantGovernorate.DataSource = consultantGovernorate.DefaultView;
            //ddlConsultantGovernorate.DataTextField = GarasERP.Governorate.ColumnNames.Name;
            //ddlConsultantGovernorate.DataValueField = GarasERP.Governorate.ColumnNames.ID;
            //ddlConsultantGovernorate.DataBind();
        }

        protected void BindGovDDL2()
        {
            GarasERP.Governorate governorate = new GarasERP.Governorate();
            //governorate.LoadAll();
            governorate.Where.CountryID.Value = ddlCountry2.SelectedValue;
            governorate.Where.Active.Value = true;
            governorate.Query.AddOrderBy(Governorate.ColumnNames.Name, MyGeneration.dOOdads.WhereParameter.Dir.ASC);
            ddlGovernate2.Items.Clear();
            if (governorate.Query.Load())
            {

                ddlGovernate2.DataSource = governorate.DefaultView;
                ddlGovernate2.DataTextField = GarasERP.Governorate.ColumnNames.Name;
                ddlGovernate2.DataValueField = GarasERP.Governorate.ColumnNames.ID;
                ddlGovernate2.DataBind();
            }

        }

        protected void BindGovDDL3()
        {
            GarasERP.Governorate governorate = new GarasERP.Governorate();
            //governorate.LoadAll();
            governorate.Where.CountryID.Value = ddlCountry3.SelectedValue;
            governorate.Where.Active.Value = true;
            governorate.Query.AddOrderBy(Governorate.ColumnNames.Name, MyGeneration.dOOdads.WhereParameter.Dir.ASC);
            ddlGovernate3.Items.Clear();
            if (governorate.Query.Load())
            {

                ddlGovernate3.DataSource = governorate.DefaultView;
                ddlGovernate3.DataTextField = GarasERP.Governorate.ColumnNames.Name;
                ddlGovernate3.DataValueField = GarasERP.Governorate.ColumnNames.ID;
                ddlGovernate3.DataBind();


                //GarasERP.Governorate consultantGovernorate = new GarasERP.Governorate();
                //consultantGovernorate.Where.CountryID.Value = ddlConsultantCountry.SelectedValue;
                //consultantGovernorate.Query.Load();

                //ddlConsultantGovernorate.DataSource = consultantGovernorate.DefaultView;
                //ddlConsultantGovernorate.DataTextField = GarasERP.Governorate.ColumnNames.Name;
                //ddlConsultantGovernorate.DataValueField = GarasERP.Governorate.ColumnNames.ID;
                //ddlConsultantGovernorate.DataBind();
            }
        }

        protected void BindSalesMen()
        {
            V_GroupUser_Branch users = new V_GroupUser_Branch();
            users.Where.BranchID.Value = Common.GetUserBranchID(UserID);
            users.Where.GroupName.Value = "SalesMen";
            users.Where.UserGroupActive.Value = true;
            users.Where.FirstName.Value= "System";
            users.Where.FirstName.Operator = MyGeneration.dOOdads.WhereParameter.Operand.NotEqual;
            users.Query.AddOrderBy(V_GroupUser_Branch.ColumnNames.FirstName, MyGeneration.dOOdads.WhereParameter.Dir.ASC);
            users.Query.AddOrderBy(V_GroupUser_Branch.ColumnNames.LastName, MyGeneration.dOOdads.WhereParameter.Dir.ASC);
            users.Query.AddResultColumn(V_GroupUser_Branch.ColumnNames.FirstName);
            users.Query.AddResultColumn(V_GroupUser_Branch.ColumnNames.LastName);
            users.Query.AddResultColumn(V_GroupUser_Branch.ColumnNames.UserID);
            ddlAssignedTo.Items.Clear();

            if(users.Query.Load())
            {
                if(users.DefaultView !=null && users.DefaultView.Count>0)
                {
                    do
                    {
                        ddlAssignedTo.Items.Add(new ListItem(users.FirstName + " " + users.LastName, users.s_UserID));

                    } while (users.MoveNext());
                }
            }
           
        }

        protected void BindFollowUpPeriod()
        {
            ddlFollowUpPeriod.DataSource = Enumerable.Range(1, 12);
            ddlFollowUpPeriod.DataBind();
            ddlFollowUpPeriod.SelectedValue = "3";
        }

        protected void LoadJobTitle()
        {
            try
            {
                JobTitle Title = new JobTitle();
                Title.Where.Active.Value = true;
                Title.Query.AddOrderBy(JobTitle.ColumnNames.Name, MyGeneration.dOOdads.WhereParameter.Dir.ASC);
                if(Title.Query.Load())
                {
                    if(Title.DefaultView!=null && Title.DefaultView.Count>0)
                    {
                        DDL_Title.DataSource = Title.DefaultView;
                        DDL_Title.DataValueField = JobTitle.ColumnNames.Name;
                        DDL_Title.DataTextField = JobTitle.ColumnNames.Name;
                        DDL_Title.DataBind();

                        DDL_Title2.DataSource = Title.DefaultView;
                        DDL_Title2.DataValueField = JobTitle.ColumnNames.Name;
                        DDL_Title2.DataTextField = JobTitle.ColumnNames.Name;
                        DDL_Title2.DataBind();

                        DDL_Title3.DataSource = Title.DefaultView;
                        DDL_Title3.DataValueField = JobTitle.ColumnNames.Name;
                        DDL_Title3.DataTextField = JobTitle.ColumnNames.Name;
                        DDL_Title3.DataBind();
                    }
                }

            }
            catch(Exception ex)
            {
                
            }
        }

        protected void LoadArea()
        {
            try
            {
                DDL_Area.Items.Clear();
                if (ddlGovernate.SelectedValue != "")
                {
                    Area area = new Area();
                    area.Where.GovernorateID.Value = ddlGovernate.SelectedValue;
                    area.Query.AddOrderBy(Area.ColumnNames.Name, MyGeneration.dOOdads.WhereParameter.Dir.ASC);
                    if (area.Query.Load())
                    {
                        if (area.DefaultView != null && area.DefaultView.Count > 0)
                        {
                            DDL_Area.DataSource = area.DefaultView;
                            DDL_Area.DataValueField = Area.ColumnNames.ID;
                            DDL_Area.DataTextField = Area.ColumnNames.Name;
                            DDL_Area.DataBind();

                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        protected void LoadArea2()
        {
            try
            {
                DDL_Area2.Items.Clear();
                if (ddlGovernate2.SelectedValue != "")
                {
                    Area area = new Area();
                    area.Where.GovernorateID.Value = ddlGovernate2.SelectedValue;
                    area.Query.AddOrderBy(Area.ColumnNames.Name, MyGeneration.dOOdads.WhereParameter.Dir.ASC);
                    if (area.Query.Load())
                    {
                        if (area.DefaultView != null && area.DefaultView.Count > 0)
                        {
                            DDL_Area2.DataSource = area.DefaultView;
                            DDL_Area2.DataValueField = Area.ColumnNames.ID;
                            DDL_Area2.DataTextField = Area.ColumnNames.Name;
                            DDL_Area2.DataBind();

                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        protected void LoadArea3()
        {
            try
            {
                DDL_Area3.Items.Clear();
                if (ddlGovernate3.SelectedValue != "")
                {
                    Area area = new Area();
                    area.Where.GovernorateID.Value = ddlGovernate3.SelectedValue;
                    area.Query.AddOrderBy(Area.ColumnNames.Name, MyGeneration.dOOdads.WhereParameter.Dir.ASC);
                    if (area.Query.Load())
                    {
                        if (area.DefaultView != null && area.DefaultView.Count > 0)
                        {
                            DDL_Area3.DataSource = area.DefaultView;
                            DDL_Area3.DataValueField = Area.ColumnNames.ID;
                            DDL_Area3.DataTextField = Area.ColumnNames.Name;
                            DDL_Area3.DataBind();

                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        #endregion




        #region Adding Client Data
        protected bool ValidateDuplicates(string Flag)
        {
            if (Flag == "Name")
            {
                GarasERP.Client searchForClientinDB = new GarasERP.Client();
                searchForClientinDB.Where.Email.Value = tbxEmail.Text;
               // searchForClientinDB.Where.Email.Operator = MyGeneration.dOOdads.WhereParameter.Operand.Like;
                searchForClientinDB.Where.Email.Conjuction = MyGeneration.dOOdads.WhereParameter.Conj.Or;
                searchForClientinDB.Where.Name.Conjuction = MyGeneration.dOOdads.WhereParameter.Conj.Or;
                searchForClientinDB.Where.Name.Value = tbxCompanyName.Text;
               // searchForClientinDB.Where.Name.Operator = MyGeneration.dOOdads.WhereParameter.Operand.Like;
               
                searchForClientinDB.Where.WebSite.Conjuction = MyGeneration.dOOdads.WhereParameter.Conj.Or;
                searchForClientinDB.Where.WebSite.Value = tbxWebsite.Text;
              //  searchForClientinDB.Where.WebSite.Operator = MyGeneration.dOOdads.WhereParameter.Operand.Like;
                searchForClientinDB.Query.Load();

                if (searchForClientinDB.RowCount > 0)
                    return false;
                else
                    return true;
            }
            else if(Flag == "Mobile")
            {
                GarasERP.ClientMobile searchForClientMobile = new GarasERP.ClientMobile();
                searchForClientMobile.Where.Mobile.Value = tbxMobile.Text;
                searchForClientMobile.Query.Load();

                if (searchForClientMobile.RowCount > 0)
                    return false;
                else
                    return true;
            }            
            else
                return true;

            //return true;
        }

        private void InitializeConsultantDT()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ConsltName", typeof(string));
            dt.Columns.Add("ConsltOffice", typeof(string));
            dt.Columns.Add("ConsltFor", typeof(string));
            dt.Columns.Add("ConsltCountry", typeof(string));
            dt.Columns.Add("ConsltGvrnrt", typeof(string));
            dt.Columns.Add("ConsltStreet", typeof(string));
            dt.Columns.Add("ConsltBuilding", typeof(string));
            dt.Columns.Add("ConsltFloor", typeof(string));
            dt.Columns.Add("ConsltDesc", typeof(string));
            dt.Columns.Add("ConsltEmail", typeof(string));
            dt.Columns.Add("ConsltEmail1", typeof(string));
            dt.Columns.Add("ConsltEmail2", typeof(string));
            dt.Columns.Add("ConsltPhone", typeof(string));
            dt.Columns.Add("ConsltPhone1", typeof(string));
            dt.Columns.Add("ConsltPhone2", typeof(string));
            dt.Columns.Add("ConsltMobile", typeof(string));
            dt.Columns.Add("ConsltMobile1", typeof(string));
            dt.Columns.Add("ConsltMobile2", typeof(string));
            dt.Columns.Add("ConsltFax", typeof(string));
            dt.Columns.Add("ConsltFax1", typeof(string));
            dt.Columns.Add("ConsltFax2", typeof(string));
            dt.Columns.Add("Special1", typeof(string));
            dt.Columns.Add("Special2", typeof(string));
            dt.Columns.Add("Special3", typeof(string));
            dt.AcceptChanges();
            VS_Consultant = dt;
        }

        protected void InsertClientData()
        {
            string script = "";
            try
            {
                GarasERP.Client client = new GarasERP.Client();
                if (FillClientName(client))
                {
                    FillClientAddress(client);
                    FillClientContacts(client);
                    FillClientContactPerson(client);
                    Fill_Client_Specialty(client);
                    if (chbxAvailable.Checked)
                    {
                        rebuildVS_Consultant(true);
                        foreach (DataRow consult in VS_Consultant.Rows)
                        {
                            GarasERP.ClientConsultant consultant = new GarasERP.ClientConsultant();
                            selectedConsultantName = consult["ConsltName"].ToString();
                            InsertClientConsultantData(client, consultant, consult);
                            //client.Consultant = selectedConsultantName;                            
                        }
                    }
                    client.Save();
                    //if (rbConsultantIndiv.Checked == true)
                    //{
                    //    GarasERP.ClientConsultant consultant = new GarasERP.ClientConsultant();
                    //    selectedConsultantName = tbxConsultantName.Text;
                    //    InsertClientConsultantData(client, consultant);
                    //    client.Consultant = selectedConsultantName;
                    //    client.Save();
                    //}
                    //else if (rbConsultantCompany.Checked == true)
                    //{
                    //    GarasERP.ClientConsultant consultant = new GarasERP.ClientConsultant();
                    //    selectedConsultantName = tbxConsultantOffice.Text;
                    //    InsertClientConsultantData(client, consultant);
                    //    client.Consultant = selectedConsultantName;
                    //    client.Save();
                    //}

                    //if (uploadPhoto.HasFile)
                    //{
                    //    client.Logo = uploadPhoto.FileBytes;
                    //    client.Save();
                    //}
                    CreateAttachmentDirectory(client);
                    CreateNotification(client);
                    script = "alert('Client Registered Successfully!');";
                    this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "Registeration Successful", script, true /* addScriptTags */);
                    //this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "Registeration Successful", "javascript:window.open( '/Sales/Client/MyClients.aspx');", true /* addScriptTags */);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Registeration Successful", "javascript:window.location.href('/Sales/Client/MyClients.aspx');", true /* addScriptTags */);
                    
                }
                else
                {
                    script = "alert('A Client with the same data already exists!');";
                    Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "Duplicate Record", script, true /* addScriptTags */);
                }
                //Response.Redirect("/Sales/SalesHome.aspx", false);
            }
            catch (Exception ex)
            {
                //Log exception
                script = "alert('Client Registration cannot be done right now, Please try again later!');";
                Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "Registeration Failed", script, true /* addScriptTags */);
            }
        }

        private void rebuildVS_Consultant(bool inSave)
        {
            InitializeConsultantDT();
            DataTable dt = VS_Consultant;
            foreach (RepeaterItem item in rptrConsaltant.Items)
            {
                string ConsltName = ((TextBox)item.FindControl("tbxConsultantName")).Text;
                string ConsltOffice = ((TextBox)item.FindControl("tbxConsultantOffice")).Text;
                string ConsltFor = ((DropDownList)item.FindControl("DDl_For")).SelectedValue;
                string ConsltCountry = ((DropDownList)item.FindControl("ddlConsultantCountry")).SelectedValue;
                string ConsltGvrnrt = ((DropDownList)item.FindControl("ddlConsultantGovernorate")).SelectedValue;
                string ConsltStreet = ((TextBox)item.FindControl("tbxConsultantStreet")).Text;
                string ConsltBuilding = ((TextBox)item.FindControl("tbxConsultantBuilding")).Text;
                string ConsltFloor = ((TextBox)item.FindControl("tbxConsultantFloor")).Text;
                string ConsltDesc = ((TextBox)item.FindControl("tbxConsultantDescription")).Text;
                string ConsltEmail = ((TextBox)item.FindControl("tbxConsultantEmail")).Text;
                string ConsltEmail1 = ((TextBox)item.FindControl("tbxConsultantEmail2")).Text;
                string ConsltEmail2 = ((TextBox)item.FindControl("tbxConsultantEmail3")).Text;
                string ConsltPhone = ((TextBox)item.FindControl("tbxConsultantPhone")).Text;
                string ConsltPhone1 = ((TextBox)item.FindControl("tbxConsultantPhone2")).Text;
                string ConsltPhone2 = ((TextBox)item.FindControl("tbxConsultantPhone3")).Text;
                string ConsltMobile = ((TextBox)item.FindControl("tbxConsultantMobile")).Text;
                string ConsltMobile1 = ((TextBox)item.FindControl("tbxConsultantMobile2")).Text;
                string ConsltMobile2 = ((TextBox)item.FindControl("tbxConsultantMobile3")).Text;
                string ConsltFax = ((TextBox)item.FindControl("tbxConsultantFax")).Text;
                string ConsltFax1 = ((TextBox)item.FindControl("tbxConsultantFax2")).Text;
                string ConsltFax2 = ((TextBox)item.FindControl("tbxConsultantFax3")).Text;
                string Special1 = ((DropDownList)item.FindControl("DDL_ConsultantSpecialty")).SelectedValue;
                string Special2 = ((DropDownList)item.FindControl("DDL_ConsultantSpecialty2")).SelectedValue;
                string Special3 = ((DropDownList)item.FindControl("ddlSpeciality3")).SelectedValue;
                if (item.ItemType == ListItemType.Item 
                    || item.ItemType == ListItemType.AlternatingItem
                    ||(inSave && item.ItemType == ListItemType.Footer && ((TextBox)item.FindControl("tbxConsultantName")).Text.Trim() != ""))
                {
                    if (chbxAvailable.Checked)
                    {
                        ((RequiredFieldValidator)item.FindControl("reqFldVldConsltName")).Enabled = true;
                    }
                    else
                    {
                        ((RequiredFieldValidator)item.FindControl("reqFldVldConsltName")).Enabled = false;
                    }
                    DataRow dr = dt.NewRow();
                    dr["ConsltName"] = ConsltName;
                    dr["ConsltOffice"] = ConsltOffice;
                    dr["ConsltFor"] = ConsltFor;
                    dr["ConsltCountry"] = ConsltCountry;
                    dr["ConsltGvrnrt"] = ConsltGvrnrt;
                    dr["ConsltStreet"] = ConsltStreet;
                    dr["ConsltBuilding"] = ConsltBuilding;
                    dr["ConsltFloor"] = ConsltFloor;
                    dr["ConsltDesc"] = ConsltDesc;
                    dr["ConsltEmail"] = ConsltEmail;
                    dr["ConsltEmail1"] = ConsltEmail1;
                    dr["ConsltEmail2"] = ConsltEmail2;
                    dr["ConsltPhone"] = ConsltPhone;
                    dr["ConsltPhone1"] = ConsltPhone1;
                    dr["ConsltPhone2"] = ConsltPhone2;
                    dr["ConsltMobile"] = ConsltMobile;
                    dr["ConsltMobile1"] = ConsltMobile1;
                    dr["ConsltMobile2"] = ConsltMobile2;
                    dr["ConsltFax"] = ConsltFax;
                    dr["ConsltFax1"] = ConsltFax1;
                    dr["ConsltFax2"] = ConsltFax2;
                    dr["Special1"] = Special1;
                    dr["Special2"] = Special2;
                    dr["Special3"] = Special3;
                    dt.Rows.Add(dr);                    
                }
                if(item.ItemType == ListItemType.Footer)
                {
                    DataRow dr = dt.NewRow();
                    dr["ConsltName"] = ConsltName;
                    dr["ConsltOffice"] = ConsltOffice;
                    dr["ConsltFor"] = ConsltFor;
                    dr["ConsltCountry"] = ConsltCountry;
                    dr["ConsltGvrnrt"] = ConsltGvrnrt;
                    dr["ConsltStreet"] = ConsltStreet;
                    dr["ConsltBuilding"] = ConsltBuilding;
                    dr["ConsltFloor"] = ConsltFloor;
                    dr["ConsltDesc"] = ConsltDesc;
                    dr["ConsltEmail"] = ConsltEmail;
                    dr["ConsltEmail1"] = ConsltEmail1;
                    dr["ConsltEmail2"] = ConsltEmail2;
                    dr["ConsltPhone"] = ConsltPhone;
                    dr["ConsltPhone1"] = ConsltPhone1;
                    dr["ConsltPhone2"] = ConsltPhone2;
                    dr["ConsltMobile"] = ConsltMobile;
                    dr["ConsltMobile1"] = ConsltMobile1;
                    dr["ConsltMobile2"] = ConsltMobile2;
                    dr["ConsltFax"] = ConsltFax;
                    dr["ConsltFax1"] = ConsltFax1;
                    dr["ConsltFax2"] = ConsltFax2;
                    dr["Special1"] = Special1;
                    dr["Special2"] = Special2;
                    dr["Special3"] = Special3;
                    VS_Consultant_Footer = dr;
                }
            }
            VS_Consultant = dt;
            rptrConsaltant.DataSource = VS_Consultant;
            rptrConsaltant.DataBind();
        }

        protected bool FillClientName(GarasERP.Client client)
        {
            if (ValidateDuplicates("Name") == false || !ValidateDuplicates("Mobile"))
            {
                return false;
            }
            else
            {
                if (tbxCompanyName.Text.Trim() != "" && ddlAssignedTo.Items != null && ddlAssignedTo.SelectedValue != "")
                {
                    client.AddNew();
                    client.Name = tbxCompanyName.Text;
                    client.GroupName = tbxGroupName.Text;

                    client.Type = selectedRadio;
                    client.Email = tbxEmail.Text;
                    client.WebSite = tbxWebsite.Text;
                    client.CreatedBy = UserID;
                    client.CreationDate = DateTime.Now;
                    client.SalesPersonID = long.Parse(ddlAssignedTo.SelectedValue);
                    client.Note = tbxNote.Text;
                    //client.Rate = 1;

                    client.FollowUpPeriod = int.Parse(ddlFollowUpPeriod.SelectedValue);
                    client.BranchName = tbxBranch.Text;
                    if (!chbxAvailable.Checked)
                        client.ConsultantType = "N/A";
                    else
                    {
                        client.ConsultantType = "Has Consultant";
                    }
                    //if (rbConsultantCompany.Checked)
                    //    client.ConsultantType = "Company / Office";
                    //if (rbConsultantIndiv.Checked)
                    //    client.ConsultantType = "Individual";
                    client.GroupName = tbxGroupName.Text;
                    if (uploadPhoto.HasFile)
                    {
                        client.Logo = uploadPhoto.FileBytes;
                        client.HasLogo = true;
                        //client.Save();
                    }
                    else
                        client.HasLogo = false;
                    client.Rate = 0;

                    client.SupportedByCompany = CHB_SupportedByCompany.Checked;
                    if(CHB_SupportedByCompany.Checked)
                    {
                        client.SupportedBy = DDL_Supported.SelectedValue;
                    }
                    client.BranchID = Common.GetUserBranchID(client.SalesPersonID);
                    client.LastReportDate = DateTime.Now;
                    client.Save();
                    return true;
                }
                else
                    return false;
            }
        }
        protected void FillClientAddress(GarasERP.Client client)
        {
            if (ddlCountry.Items != null && ddlGovernate.Items != null && ddlCountry.SelectedValue != "" && ddlGovernate.SelectedValue != "" && tbxStreet.Text.Trim()!="")
            {
                GarasERP.ClientAddress clientAddress = new GarasERP.ClientAddress();
                clientAddress.AddNew();
                clientAddress.ClientID = client.ID;
                clientAddress.CountryID = int.Parse(ddlCountry.SelectedValue);
                clientAddress.GovernorateID = int.Parse(ddlGovernate.SelectedValue);
                clientAddress.Address = tbxStreet.Text;
                clientAddress.CreatedBy = UserID;
                clientAddress.CreationDate = DateTime.Now;
                clientAddress.Active = true;
                clientAddress.BuildingNumber = tbxBuilding.Text;
                clientAddress.Floor = tbxFloor.Text;
                clientAddress.Description = tbxDesc.Text;
                if(DDL_Area.Items !=null && DDL_Area.Items.Count>0 && DDL_Area.SelectedValue!="")
                    clientAddress.AreaID = long.Parse( DDL_Area.SelectedValue);
                clientAddress.Save();
            }

            if (address2.Visible && ddlCountry2.Items != null && ddlGovernate2.Items != null && ddlCountry2.SelectedValue != "" && ddlGovernate2.SelectedValue != "" && tbxStreet2.Text.Trim()!="")
            {
                GarasERP.ClientAddress clientAddress2 = new GarasERP.ClientAddress();
                clientAddress2.AddNew();
                clientAddress2.ClientID = client.ID;
                clientAddress2.CountryID = int.Parse(ddlCountry2.SelectedValue);
                clientAddress2.GovernorateID = int.Parse(ddlGovernate2.SelectedValue);
                clientAddress2.Address = tbxStreet2.Text;
                clientAddress2.CreatedBy = UserID;
                clientAddress2.CreationDate = DateTime.Now;
                clientAddress2.Active = true;
                clientAddress2.BuildingNumber = tbxBuilding2.Text;
                clientAddress2.Floor = tbxFloor2.Text;
                clientAddress2.Description = tbxDesc2.Text;
                if (DDL_Area2.Items != null && DDL_Area2.Items.Count > 0 && DDL_Area2.SelectedValue != "")
                    clientAddress2.AreaID = long.Parse(DDL_Area2.SelectedValue);
                clientAddress2.Save();
            }

            if (address3.Visible && ddlCountry3.Items != null && ddlGovernate3.Items != null && ddlCountry3.SelectedValue != "" && ddlGovernate3.SelectedValue != "" && tbxStreet3.Text.Trim() != "")
            {
                GarasERP.ClientAddress clientAddress3 = new GarasERP.ClientAddress();
                clientAddress3.AddNew();
                clientAddress3.ClientID = client.ID;
                clientAddress3.CountryID = int.Parse(ddlCountry3.SelectedValue);
                clientAddress3.GovernorateID = int.Parse(ddlGovernate3.SelectedValue);
                clientAddress3.Address = tbxStreet3.Text;
                clientAddress3.CreatedBy = UserID;
                clientAddress3.CreationDate = DateTime.Now;
                clientAddress3.Active = true;
                clientAddress3.BuildingNumber = tbxBuilding3.Text;
                clientAddress3.Floor = tbxFloor3.Text;
                clientAddress3.Description = tbxDesc3.Text;
                if (DDL_Area3.Items != null && DDL_Area3.Items.Count > 0 && DDL_Area3.SelectedValue != "")
                    clientAddress3.AreaID = long.Parse(DDL_Area3.SelectedValue);
                clientAddress3.Save();
            }
        }
        protected bool FillClientContacts(GarasERP.Client client)
        {
            if (ValidateDuplicates("Mobile") == false)
            {
                return false;
            }
            else
            {
                if (tbxMobile.Text.Trim() != "")
                {
                    GarasERP.ClientMobile clientMobile = new GarasERP.ClientMobile();
                    clientMobile.AddNew();
                    clientMobile.ClientID = client.ID;
                    clientMobile.Mobile = tbxMobile.Text == String.Empty ? "N/A" : tbxMobile.Text;
                    clientMobile.Active = true;
                    clientMobile.CreatedBy = UserID;
                    clientMobile.CreationDate = DateTime.Now;
                    clientMobile.Save();
                }

                if (newMobile2.Visible && tbxMobile2.Text.Trim() != "")
                {
                    GarasERP.ClientMobile clientMobile2 = new GarasERP.ClientMobile();
                    clientMobile2.AddNew();
                    clientMobile2.ClientID = client.ID;
                    clientMobile2.Mobile = tbxMobile2.Text == String.Empty ? "N/A" : tbxMobile2.Text; ;
                    clientMobile2.Active = true;
                    clientMobile2.CreatedBy = UserID;
                    clientMobile2.CreationDate = DateTime.Now;
                    clientMobile2.Save();
                }

                if (newMobile3.Visible && tbxMobile3.Text.Trim()!="")
                {
                    GarasERP.ClientMobile clientMobile3 = new GarasERP.ClientMobile();
                    clientMobile3.AddNew();
                    clientMobile3.ClientID = client.ID;
                    clientMobile3.Mobile = tbxMobile3.Text == String.Empty ? "N/A" : tbxMobile3.Text; ;
                    clientMobile3.Active = true;
                    clientMobile3.CreatedBy = UserID;
                    clientMobile3.CreationDate = DateTime.Now;
                    clientMobile3.Save();
                }

                
                if (tbxPhone.Text.Trim() != "")
                {
                    GarasERP.ClientPhone clientPhone = new GarasERP.ClientPhone();
                    clientPhone.AddNew();
                    clientPhone.ClientID = client.ID;
                    clientPhone.Phone = tbxPhone.Text == String.Empty ? "N/A" : tbxPhone.Text;
                    clientPhone.Active = true;
                    clientPhone.CreatedBy = UserID;
                    clientPhone.CreationDate = DateTime.Now;
                    clientPhone.Save();
                }

                if (newPhone.Visible && tbxPhone2.Text.Trim() != "")
                {
                    GarasERP.ClientPhone clientPhone2 = new GarasERP.ClientPhone();
                    clientPhone2.AddNew();
                    clientPhone2.ClientID = client.ID;
                    clientPhone2.Phone = tbxPhone2.Text == String.Empty ? "N/A" : tbxPhone2.Text;
                    clientPhone2.Active = true;
                    clientPhone2.CreatedBy = UserID;
                    clientPhone2.CreationDate = DateTime.Now;
                    clientPhone2.Save();
                }

                if (newPhone3.Visible && tbxPhone3.Text.Trim() != "")
                {
                    GarasERP.ClientPhone clientPhone3 = new GarasERP.ClientPhone();
                    clientPhone3.AddNew();
                    clientPhone3.ClientID = client.ID;
                    clientPhone3.Phone = tbxPhone3.Text == String.Empty ? "N/A" : tbxPhone3.Text;
                    clientPhone3.Active = true;
                    clientPhone3.CreatedBy = UserID;
                    clientPhone3.CreationDate = DateTime.Now;
                    clientPhone3.Save();
                }

                if (tbxFax.Text.Trim() != "")
                {
                    GarasERP.ClientFax clientFax = new GarasERP.ClientFax();
                    clientFax.AddNew();
                    clientFax.ClientID = client.ID;
                    clientFax.Fax = tbxFax.Text == String.Empty ? "N/A" : tbxFax.Text;
                    clientFax.Active = true;
                    clientFax.CreatedBy = UserID;
                    clientFax.CreationDate = DateTime.Now;
                    clientFax.Save();
                }

                if (newFax2.Visible && tbxFax2.Text.Trim() != "")
                {
                    GarasERP.ClientFax clientFax2 = new GarasERP.ClientFax();
                    clientFax2.AddNew();
                    clientFax2.ClientID = client.ID;
                    clientFax2.Fax = tbxFax2.Text == String.Empty ? "N/A" : tbxFax2.Text;
                    clientFax2.Active = true;
                    clientFax2.CreatedBy = UserID;
                    clientFax2.CreationDate = DateTime.Now;
                    clientFax2.Save();
                }

                if (newFax3.Visible && tbxFax3.Text.Trim() != "")
                {
                    GarasERP.ClientFax clientFax3 = new GarasERP.ClientFax();
                    clientFax3.AddNew();
                    clientFax3.ClientID = client.ID;
                    clientFax3.Fax = tbxFax3.Text == String.Empty ? "N/A" : tbxFax3.Text;
                    clientFax3.Active = true;
                    clientFax3.CreatedBy = UserID;
                    clientFax3.CreationDate = DateTime.Now;
                    clientFax3.Save();
                }
                return true;
            }
        }
        protected void FillClientContactPerson(GarasERP.Client client)
        {
            GarasERP.ClientContactPerson contact = new GarasERP.ClientContactPerson();
            if (tbxContactPerson.Text.Trim() != "" && DDL_Title.SelectedValue != "" && tbxContactMobile.Text.Trim() != "")
            {
                contact.AddNew();
                contact.ClientID = client.ID;
                contact.CreatedBy = UserID;
                contact.CreationDate = DateTime.Now;
                contact.Active = true;
                contact.Name = tbxContactPerson.Text;
                contact.Title = DDL_Title.SelectedValue;
                contact.Location = tbxContactLocation.Text;
                contact.Email = tbxContactEmail.Text;
                contact.Mobile = tbxContactMobile.Text;
                contact.Save();
            }

            if (newContact2.Visible && tbxContactName2.Text.Trim() != "" && DDL_Title2.SelectedValue != "" && tbxContactMobile2.Text.Trim() != "")
            {
                GarasERP.ClientContactPerson contact2 = new GarasERP.ClientContactPerson();

                contact2.AddNew();
                contact2.ClientID = client.ID;
                contact2.CreatedBy = UserID;
                contact2.CreationDate = DateTime.Now;
                contact2.Active = true;
                contact2.Name = tbxContactName2.Text;
                contact2.Title = DDL_Title2.SelectedValue;
                contact2.Location = tbxContactLocation2.Text;
                contact2.Email = tbxContactEmail2.Text;
                contact2.Mobile = tbxContactMobile2.Text;
                contact2.Save();
            }

            if (newContact3.Visible && tbxContactName3.Text.Trim() != "" && DDL_Title3.SelectedValue != "" && tbxContactMobile3.Text.Trim() != "")
            {
                GarasERP.ClientContactPerson contact3 = new GarasERP.ClientContactPerson();

                contact3.AddNew();
                contact3.ClientID = client.ID;
                contact3.CreatedBy = UserID;
                contact3.CreationDate = DateTime.Now;
                contact3.Active = true;
                contact3.Name = tbxContactName3.Text;
                contact3.Title = DDL_Title3.SelectedValue;
                contact3.Location = tbxContactLocation3.Text;
                contact3.Email = tbxContactEmail3.Text;
                contact3.Mobile = tbxContactMobile3.Text;
                contact3.Save();
            }

        }
        protected void Fill_Client_Specialty(GarasERP.Client client)
        {
            if (ddlSpeciality.Items != null && ddlSpeciality.SelectedValue != "")
            {
                GarasERP.ClientSpeciality clientSpecialty = new GarasERP.ClientSpeciality();
                clientSpecialty.AddNew();
                clientSpecialty.ClientID = client.ID;
                clientSpecialty.SpecialityID = int.Parse(ddlSpeciality.SelectedValue);
                clientSpecialty.CreatedBy = UserID;
                clientSpecialty.CreationDate = DateTime.Now;
                clientSpecialty.Active = true;
                clientSpecialty.Save();
            }

            if (speciality2.Visible && ddlSpeciality2.Items != null && ddlSpeciality2.SelectedValue != "")
            {
                GarasERP.ClientSpeciality clientSpecialty2 = new GarasERP.ClientSpeciality();
                clientSpecialty2.AddNew();
                clientSpecialty2.ClientID = client.ID;
                clientSpecialty2.SpecialityID = int.Parse(ddlSpeciality2.SelectedValue);
                clientSpecialty2.CreatedBy = UserID;
                clientSpecialty2.CreationDate = DateTime.Now;
                clientSpecialty2.Active = true;
                clientSpecialty2.Save();
            }

            if (speciality3.Visible && ddlSpeciality3.Items != null && ddlSpeciality3.SelectedValue != "")
            {
                GarasERP.ClientSpeciality clientSpecialty3 = new GarasERP.ClientSpeciality();
                clientSpecialty3.AddNew();
                clientSpecialty3.ClientID = client.ID;
                clientSpecialty3.SpecialityID = int.Parse(ddlSpeciality3.SelectedValue);
                clientSpecialty3.CreatedBy = UserID;
                clientSpecialty3.CreationDate = DateTime.Now;
                clientSpecialty3.Active = true;
                clientSpecialty3.Save();
            }

            if (speciality4.Visible && ddlSpeciality4.Items != null && ddlSpeciality4.SelectedValue != "")
            {
                GarasERP.ClientSpeciality clientSpecialty4 = new GarasERP.ClientSpeciality();
                clientSpecialty4.AddNew();
                clientSpecialty4.ClientID = client.ID;
                clientSpecialty4.SpecialityID = int.Parse(ddlSpeciality4.SelectedValue);
                clientSpecialty4.CreatedBy = UserID;
                clientSpecialty4.CreationDate = DateTime.Now;
                clientSpecialty4.Active = true;
                clientSpecialty4.Save();
            }


        }

        protected void InsertClientConsultantData(GarasERP.Client client
            , GarasERP.ClientConsultant consultant
            , DataRow conslt)
        {
            //foreach (DataRow conslt in VS_Consultant.Rows)
            {
                AddClientConsultantName(client, consultant,conslt["ConsltName"].ToString()
                                        , conslt["ConsltOffice"].ToString()
                                        , conslt["ConsltFor"].ToString());
                AddClientConsultantAddress(consultant, conslt["ConsltCountry"].ToString()
                                        , conslt["ConsltGvrnrt"].ToString()
                                        , conslt["ConsltStreet"].ToString()
                                        , conslt["ConsltBuilding"].ToString()
                                        , conslt["ConsltFloor"].ToString()
                                        , conslt["ConsltDesc"].ToString());
                AddClientConsultantEmail(consultant, conslt["ConsltEmail"].ToString()
                                        , conslt["ConsltEmail1"].ToString()
                                        , conslt["ConsltEmail2"].ToString());
                AddClientConsultantPhone(consultant, conslt["ConsltPhone"].ToString()
                                        , conslt["ConsltPhone1"].ToString()
                                        , conslt["ConsltPhone2"].ToString());
                AddClientConsultantMobile(consultant, conslt["ConsltMobile"].ToString()
                                        , conslt["ConsltMobile1"].ToString()
                                        , conslt["ConsltMobile2"].ToString());
                AddClientConsultantFax(consultant, conslt["ConsltFax"].ToString()
                                        , conslt["ConsltFax1"].ToString()
                                        , conslt["ConsltFax2"].ToString());
                AddClientConsultantSpeciality(consultant,conslt["Special1"].ToString()
                                            , conslt["Special2"].ToString()
                                            , conslt["Special3"].ToString());
            }
        }

        private void AddClientConsultantSpeciality(ClientConsultant consultant
                                                    ,string consultantSpeciality
                                                    ,string consultantSpeciality2
                                                    ,string consultantSpeciality3)
        {

            //if (DDL_ConsultantSpecialty.Items != null && DDL_ConsultantSpecialty.SelectedValue != "")
            if(consultantSpeciality != "")
            {
                ClientConsultantSpecialilty consultantSpicial = new GarasERP.ClientConsultantSpecialilty();
                
                consultantSpicial.AddNew();
                consultantSpicial.CreatedBy = UserID;
                consultantSpicial.CreationDate = DateTime.Now;
                

                consultantSpicial.ConsultantID = consultant.ID;
                consultantSpicial.SpecialityID = int.Parse(consultantSpeciality/*DDL_ConsultantSpecialty.SelectedValue*/);
                consultantSpicial.ModifiedBy = UserID;
                consultantSpicial.Modified = DateTime.Now;
                consultantSpicial.Active = true;
                consultantSpicial.Save();
            }
            
            if (speciality2.Visible && consultantSpeciality2 != "")//DDL_ConsultantSpecialty2.Items != null && DDL_ConsultantSpecialty2.SelectedValue != "")
            {
                ClientConsultantSpecialilty consultantSpicial = new GarasERP.ClientConsultantSpecialilty();
                
                consultantSpicial.AddNew();
                consultantSpicial.CreatedBy = UserID;
                consultantSpicial.CreationDate = DateTime.Now;
               

                consultantSpicial.ConsultantID = consultant.ID;
                consultantSpicial.SpecialityID = int.Parse(consultantSpeciality2/*DDL_ConsultantSpecialty2.SelectedValue*/);
                consultantSpicial.ModifiedBy = UserID;
                consultantSpicial.Modified = DateTime.Now;
                consultantSpicial.Active = true;
                consultantSpicial.Save();
            }
            

            if (speciality3.Visible && consultantSpeciality3 != "")// ddlSpeciality3.Items != null && ddlSpeciality3.SelectedValue != "")
            {
                ClientConsultantSpecialilty consultantSpicial = new GarasERP.ClientConsultantSpecialilty();
                
                    consultantSpicial.AddNew();
                    consultantSpicial.CreatedBy = UserID;
                    consultantSpicial.CreationDate = DateTime.Now;
               

                consultantSpicial.ConsultantID = consultant.ID;
                consultantSpicial.SpecialityID = int.Parse(consultantSpeciality3/*ddlSpeciality3.SelectedValue*/);
                consultantSpicial.ModifiedBy = UserID;
                consultantSpicial.Modified = DateTime.Now;
                consultantSpicial.Active = true;
                consultantSpicial.Save();
            }
           
        }

        protected void AddClientConsultantName(GarasERP.Client client
                                              , GarasERP.ClientConsultant consultant
                                              ,string consltName, string consltOffice
                                              ,string selectedFor)
        {
            consultant.AddNew();
            consultant.ClientID = client.ID;
            consultant.ConsultantName = consltName;//tbxConsultantName.Text;
            consultant.Company = consltOffice;// tbxConsultantOffice.Text;
            consultant.CreatedBy = UserID;
            consultant.CreationDate = DateTime.Now;
            consultant.Active = true;
            consultant.ConsultantFor = selectedFor;//DDl_For.SelectedValue;
            consultant.Save();
        }

        protected void AddClientConsultantAddress(GarasERP.ClientConsultant consultant
                                                  ,string consltCountry
                                                  ,string consltGovrnrt
                                                  ,string consltStreet
                                                  ,string consltBuilding
                                                  , string consltFloor
                                                  , string consltDesc)
        {
            GarasERP.ClientConsultantAddress consultantAddress = new GarasERP.ClientConsultantAddress();
            consultantAddress.AddNew();
            consultantAddress.ConsultantID = consultant.ID;
            consultantAddress.CountryID = int.Parse(consltCountry/*ddlConsultantCountry.SelectedValue*/);
            consultantAddress.GovernorateID = int.Parse(consltGovrnrt /*ddlConsultantGovernorate.SelectedValue*/);
            consultantAddress.Address = consltStreet;// tbxConsultantStreet.Text;
            consultantAddress.BuildingNumber = consltBuilding;// tbxConsultantBuilding.Text;
            consultantAddress.Floor = consltFloor;// tbxConsultantFloor.Text;
            consultantAddress.Description = consltDesc;// tbxConsultantDescription.Text;
            consultantAddress.Active = true;
            consultantAddress.CreatedBy = UserID;
            consultantAddress.CreationDate = DateTime.Now;
            consultantAddress.Save();

        }

        protected void AddClientConsultantEmail(GarasERP.ClientConsultant consultant
                                                ,string email
                                                ,string email1
                                                ,string email2)
        {
            if (email.Trim() != "" )//tbxConsultantEmail.Text.Trim() != "")
            {
                GarasERP.ClientConsultantEmail consultantEmail = new GarasERP.ClientConsultantEmail();
                consultantEmail.AddNew();
                consultantEmail.ConsultantID = consultant.ID;
                consultantEmail.Email = email;// tbxConsultantEmail.Text;
                consultantEmail.CreatedBy = UserID;
                consultantEmail.CreationDate = DateTime.Now;
                consultantEmail.Active = true;
                consultantEmail.Save();
            }


            if (email1.Trim() != "")//newConsultantEmail2.Visible && tbxConsultantEmail2.Text.Trim() != "")
            {
                GarasERP.ClientConsultantEmail consultantEmail2 = new GarasERP.ClientConsultantEmail();
                consultantEmail2.AddNew();
                consultantEmail2.ConsultantID = consultant.ID;
                consultantEmail2.Email = email1;// tbxConsultantEmail2.Text;
                consultantEmail2.CreatedBy = UserID;
                consultantEmail2.CreationDate = DateTime.Now;
                consultantEmail2.Active = true;
                consultantEmail2.Save();
            }

            if (email2.Trim() != "" )//newConsultantEmail3.Visible && tbxConsultantEmail3.Text.Trim() != "")
            {
                GarasERP.ClientConsultantEmail consultantEmail3 = new GarasERP.ClientConsultantEmail();
                consultantEmail3.AddNew();
                consultantEmail3.ConsultantID = consultant.ID;
                consultantEmail3.Email = email2;// tbxConsultantEmail3.Text;
                consultantEmail3.CreatedBy = UserID;
                consultantEmail3.CreationDate = DateTime.Now;
                consultantEmail3.Active = true;
                consultantEmail3.Save();
            }
        }

        protected void AddClientConsultantPhone(GarasERP.ClientConsultant consultant
                                                ,string consltPhone
                                                , string consltPhone1
                                                , string consltPhone2)
        {
            if (consltPhone.Trim() != "")//tbxConsultantPhone.Text.Trim() != "")
            {
                GarasERP.ClientConsultantPhone consultantPhone = new GarasERP.ClientConsultantPhone();
                consultantPhone.AddNew();
                consultantPhone.ConsultantID = consultant.ID;
                consultantPhone.Phone = consltPhone;
                consultantPhone.CreatedBy = UserID;
                consultantPhone.CreationDate = DateTime.Now;
                consultantPhone.Active = true;
                consultantPhone.Save();
            }

            if (consltPhone1.Trim() != "")//newConsultantPhone2.Visible && tbxConsultantPhone2.Text.Trim() != "")
            {
                GarasERP.ClientConsultantPhone consultantPhone2 = new GarasERP.ClientConsultantPhone();
                consultantPhone2.AddNew();
                consultantPhone2.ConsultantID = consultant.ID;
                consultantPhone2.Phone = consltPhone1;//tbxConsultantPhone2.Text;
                consultantPhone2.CreatedBy = UserID;
                consultantPhone2.CreationDate = DateTime.Now;
                consultantPhone2.Active = true;
                consultantPhone2.Save();
            }

            if (consltPhone2.Trim() != "")//newConsultantPhone3.Visible && tbxConsultantPhone3.Text.Trim() != "")
            {
                GarasERP.ClientConsultantPhone consultantPhone3 = new GarasERP.ClientConsultantPhone();
                consultantPhone3.AddNew();
                consultantPhone3.ConsultantID = consultant.ID;
                consultantPhone3.Phone = consltPhone2;// tbxConsultantPhone3.Text;
                consultantPhone3.CreatedBy = UserID;
                consultantPhone3.CreationDate = DateTime.Now;
                consultantPhone3.Active = true;
                consultantPhone3.Save();
            }

        }

        protected void AddClientConsultantFax(GarasERP.ClientConsultant consultant
                                                ,string consltFax
                                                , string consltFax1
                                                , string consltFax2)
        {
            if (consltFax.Trim() != "")//tbxConsultantFax.Text.Trim() != "")
            {
                GarasERP.ClientConsultantFax consultantFax = new GarasERP.ClientConsultantFax();
                consultantFax.AddNew();
                consultantFax.ConsultantID = consultant.ID;
                consultantFax.Fax = consltFax;// tbxConsultantFax.Text;
                consultantFax.CreatedBy = UserID;
                consultantFax.CreationDate = DateTime.Now;
                consultantFax.Active = true;
                consultantFax.Save();
            }

            if (consltFax1.Trim() != "")//newConsultantFax2.Visible && tbxConsultantFax2.Text.Trim() != "")
            {
                GarasERP.ClientConsultantFax consultantFax2 = new GarasERP.ClientConsultantFax();
                consultantFax2.AddNew();
                consultantFax2.ConsultantID = consultant.ID;
                consultantFax2.Fax = consltFax1;// tbxConsultantFax2.Text;
                consultantFax2.CreatedBy = UserID;
                consultantFax2.CreationDate = DateTime.Now;
                consultantFax2.Active = true;
                consultantFax2.Save();
            }

            if (consltFax2.Trim() != "")//newConsultantFax3.Visible && tbxConsultantFax3.Text.Trim() != "")
            {
                GarasERP.ClientConsultantFax consultantFax3 = new GarasERP.ClientConsultantFax();
                consultantFax3.AddNew();
                consultantFax3.ConsultantID = consultant.ID;
                consultantFax3.Fax = consltFax2;// tbxConsultantFax3.Text;
                consultantFax3.CreatedBy = UserID;
                consultantFax3.CreationDate = DateTime.Now;
                consultantFax3.Active = true;
                consultantFax3.Save();
            }

        }

        protected void AddClientConsultantMobile(GarasERP.ClientConsultant consultant
                                                 ,string consltMobile
                                                , string consltMobile1
                                                , string consltMobile2)
        {
            if (consltMobile.Trim() != "")//tbxConsultantMobile.Text.Trim() != "")
            {
                GarasERP.ClientConsultantMobile consultantMobile = new GarasERP.ClientConsultantMobile();
                consultantMobile.AddNew();
                consultantMobile.ConsultantID = consultant.ID;
                consultantMobile.Mobile = consltMobile;// tbxConsultantMobile.Text;
                consultantMobile.CreatedBy = UserID;
                consultantMobile.CreationDate = DateTime.Now;
                consultantMobile.Active = true;
                consultantMobile.Save();
            }

            if (consltMobile1.Trim() != "")//newConsultantMobile2.Visible && tbxConsultantMobile2.Text.Trim() != "")
            {
                GarasERP.ClientConsultantMobile consultantMobile2 = new GarasERP.ClientConsultantMobile();
                consultantMobile2.AddNew();
                consultantMobile2.ConsultantID = consultant.ID;
                consultantMobile2.Mobile = consltMobile1;// tbxConsultantMobile2.Text;
                consultantMobile2.CreatedBy = UserID;
                consultantMobile2.CreationDate = DateTime.Now;
                consultantMobile2.Active = true;
                consultantMobile2.Save();
            }

            if (consltMobile2.Trim() != "")//newConsultantMobile3.Visible && tbxConsultantMobile3.Text.Trim() != "")
            {
                GarasERP.ClientConsultantMobile consultantMobile3 = new GarasERP.ClientConsultantMobile();
                consultantMobile3.AddNew();
                consultantMobile3.ConsultantID = consultant.ID;
                consultantMobile3.Mobile = consltMobile2;// tbxConsultantMobile3.Text;
                consultantMobile3.CreatedBy = UserID;
                consultantMobile3.CreationDate = DateTime.Now;
                consultantMobile3.Active = true;
                consultantMobile3.Save();
            }

        }


        protected void CreateAttachmentDirectory(GarasERP.Client client)
        {
            if (FileUpload1.HasFile || FileUpload2.HasFile || FileUpload3.HasFile)
            {
                var folder = Server.MapPath("~/Attachments/Clients/");
                var clientFolder = Server.MapPath("~/Attachments/Clients/" + client.ID + "/");
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                if (!Directory.Exists(clientFolder))
                {
                    Directory.CreateDirectory(clientFolder);
                }

                if (FileUpload1.HasFile)
                {
                    GarasERP.ClientAttachment attachment = new GarasERP.ClientAttachment();
                    attachment.AddNew();
                    attachment.ClientID = client.ID;
                    attachment.CreatedBy = UserID;
                    attachment.CreationDate = DateTime.Now;
                    attachment.Active = true;
                    attachment.Type = "Business Cards";


                    string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                    string extension = fileName.Split('.').Last();
                    string Date = DateTime.Now.ToFileTime() + "_";
                    FileUpload1.PostedFile.SaveAs((clientFolder) + Date+ fileName);
                    attachment.FileName = fileName;
                    attachment.AttachmentPath = (clientFolder) + Date +fileName;
                    if (attachment.AttachmentPath == DBNull.Value.ToString())
                        attachment.AttachmentPath = "N/A";
                    attachment.FileExtenssion = extension;
                    attachment.Save();

                    //Response.Redirect(Request.Url.AbsoluteUri);
                }
                if (FileUpload2.HasFile)
                {
                    GarasERP.ClientAttachment attachment = new GarasERP.ClientAttachment();
                    attachment.AddNew();
                    attachment.ClientID = client.ID;
                    attachment.CreatedBy = UserID;
                    attachment.CreationDate = DateTime.Now;
                    attachment.Active = true;
                    attachment.Type = "Brochure";
                    string fileName = Path.GetFileName(FileUpload2.PostedFile.FileName);
                    string extension = fileName.Split('.').Last();
                    string Date = DateTime.Now.ToFileTime()+"_";
                    FileUpload2.PostedFile.SaveAs((clientFolder)  +Date + fileName);
                    attachment.FileName = fileName;
                    attachment.FileExtenssion = extension;
                    
                    attachment.AttachmentPath = (clientFolder) + Date + fileName;
                    if (attachment.AttachmentPath == DBNull.Value.ToString())
                        attachment.AttachmentPath = "N/A";
                    attachment.Save();
                    //Response.Redirect(Request.Url.AbsoluteUri);
                }
                if (FileUpload3.HasFile)
                {
                    GarasERP.ClientAttachment attachment = new GarasERP.ClientAttachment();
                    attachment.AddNew();
                    attachment.ClientID = client.ID;
                    attachment.CreatedBy = UserID;
                    attachment.CreationDate = DateTime.Now;
                    attachment.Active = true;
                    attachment.Type = "Other";

                    string fileName = Path.GetFileName(FileUpload3.PostedFile.FileName);
                    string extension = fileName.Split('.').Last();
                    string Date = DateTime.Now.ToFileTime() + "_";
                    FileUpload3.PostedFile.SaveAs((clientFolder) +Date +  fileName);
                    attachment.FileName = fileName;
                    attachment.FileExtenssion = extension;
                    attachment.AttachmentPath = (clientFolder) + Date +fileName;
                    if (attachment.AttachmentPath == DBNull.Value.ToString())
                        attachment.AttachmentPath = "N/A";
                    attachment.Save();
                    //Response.Redirect(Request.Url.AbsoluteUri);
                }

            }
        }
        #endregion


        protected void CreateNotification(GarasERP.Client client)
        {
            GarasERP.Notification notification = new GarasERP.Notification();
            notification.AddNew();
            notification.Title = "New Client Assigned";
            if (CHB_SupportedByCompany.Checked)
            {
                notification.Description = client.Name + " is now assigned to you. They are supported by the company. Kindly call the client.";
            }
            else
            {
                notification.Description = client.Name + " is now assigned to you.";
            }
            notification.Date = DateTime.Now;
            notification.New = true;
            notification.URL = EncryptRedirect(client.ID.ToString()); ;
            notification.UserID = long.Parse(ddlAssignedTo.SelectedValue);
            notification.Save();
        }
        private string EncryptRedirect(string clientID)
        {
            string url = "~/Sales/Client/ClientProfile.aspx?CID=";
            string queryStr = GarasERP.Encrypt_Decrypt.Encrypt(clientID, key);
            return url + Server.UrlEncode(queryStr);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (GetUserPermission())
            {
                InsertClientData();
            }
            else
            {
                string script = "alert('Sorry, You donot have access to Add Clients. Please contact your Administrator!');";
                this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "Registration Failed", script, true /* addScriptTags */);
            }

            ////Clear textboxes after Registraion
            //CleartextBoxes(this);
        }

        protected bool GetUserPermission()
        {
            GarasERP.UserRole userRole = new GarasERP.UserRole();
            userRole.Where.UserID.Value = UserID;
            userRole.Where.RoleID.Conjuction = MyGeneration.dOOdads.WhereParameter.Conj.And;
            userRole.Where.RoleID.Value = 1;//RoleID = AddClient
            userRole.Query.Load();

            ////userRole.Where.RoleID.Conjuction = MyGeneration.dOOdads.WhereParameter.Conj.Or;
            //userRole.Where.RoleID.Value = 1;

            if (userRole.RowCount > 0)
            {
                if (userRole.RoleID == 1)
                {
                    return true;
                }
                else
                {
                    return false;
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
                    return true;
                }
                else
                {
                    return false;
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

        public void CleartextBoxes(Control parent)
        {
            foreach (Control tbx in parent.Controls)
            {
                if ((tbx.GetType() == typeof(TextBox)))
                {
                    ((TextBox)(tbx)).Text = String.Empty;
                }
                if (tbx.HasControls())
                {
                    CleartextBoxes(tbx);
                }
            }
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGovDDL();
            LoadArea();
        }

        protected void ddlCountry2_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGovDDL2();
            LoadArea2();
        }

        protected void ddlCountry3_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGovDDL3();
            LoadArea3();
        }


        #region Unwanted Parts *Commented*

        //protected void InitializeDataTable()
        //{
        //    dt = new DataTable();
        //    dt.Columns.Add(new DataColumn("MyColumn", typeof(string)));
        //    dt.AcceptChanges();
        //}
        //protected void lnk_Click(object sender, EventArgs e)
        //{
        //    DataRow dr = dt.NewRow();
        //    dr["MyColumn"] = "gggg";// ((TextBox)reptr.Controls[reptr.Controls.Count - 1].Controls[0].FindControl("tbx")).Text;
        //    dt.Rows.Add(dr);
        //    ViewState["DT"] = dt;
        //    //reptr.DataSource = dt;
        //    //reptr.DataBind();
        //}

        //protected void reptr_ItemCommand(object source, RepeaterCommandEventArgs e)
        //{

        //}

        //protected void clientPhoneRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        //{
        //    if (e.CommandName == "Add")
        //    {
        //        TextBox tbx = (TextBox)e.Item.FindControl("clientPhoneTbx");                              
        //        DataRow dr = dt.NewRow();
        //        dr["MyColumn"] = tbx.Text;// ((TextBox)reptr.Controls[reptr.Controls.Count - 1].Controls[0].FindControl("tbx")).Text;
        //        dt.Rows.Add(dr);
        //        ViewState["DT"] = dt;
        //        //clientPhoneRepeater.DataSource = dt;
        //        //clientPhoneRepeater.DataBind();
        //    }
        //}

        #endregion


        #region Add Extra Fields

        protected void btnAddSpeciality_Click(object sender, EventArgs e)
        {
            speciality2.Visible = true;
            btnAddSpeciality.Visible = false;
        }

        protected void btnRemoveSpeciality_Click(object sender, EventArgs e)
        {
            speciality2.Visible = false;
            btnAddSpeciality.Visible = true;
        }

        protected void btnAddSpeciality3_Click(object sender, EventArgs e)
        {
            speciality3.Visible = true;
            btnAddSpeciality3.Visible = false;
        }
        protected void btnRemoveSpeciality3_Click(object sender, EventArgs e)
        {
            speciality3.Visible = false;
            btnAddSpeciality3.Visible = true;
        }

        protected void btnRemoveSpeciality4_Click(object sender, EventArgs e)
        {
            speciality4.Visible = false;
            btnAddSpeciality4.Visible = true;

        }

        protected void btnAddSpeciality4_Click(object sender, EventArgs e)
        {
            speciality4.Visible = true;
            btnAddSpeciality4.Visible = false;
        }
        protected void btnAddNewAddress1_Click(object sender, EventArgs e)
        {
            address2.Visible = true;
            pAddNewAddress1.Visible = false;

        }

        protected void btnRemoveAddress2_Click(object sender, EventArgs e)
        {
            address2.Visible = false;
            pAddNewAddress1.Visible = true;
        }

        protected void btnAddNewAddress2_Click(object sender, EventArgs e)
        {
            address3.Visible = true;
            pAddNewAddress2.Visible = false;
        }

        protected void btnRemoveAddress3_Click(object sender, EventArgs e)
        {
            address3.Visible = false;
            pAddNewAddress2.Visible = true;
        }

        protected void btnAddNewPhone_Click(object sender, EventArgs e)
        {
            newPhone.Visible = true;
            btnAddNewPhone.Visible = false;
            RegularExpressionValidator6.Enabled = true;
        }

        protected void removeNewButton_Click(object sender, EventArgs e)
        {
            newPhone.Visible = false;
            btnAddNewPhone.Visible = true;
            RegularExpressionValidator6.Enabled = false;
        }

        protected void btnAddNewPhone2_Click(object sender, EventArgs e)
        {
            newPhone3.Visible = true;
            btnAddNewPhone2.Visible = false;
            RegularExpressionValidator7.Enabled = true;
        }

        protected void removeNewButton3_Click(object sender, EventArgs e)
        {
            newPhone3.Visible = false;
            btnAddNewPhone2.Visible = true;
            RegularExpressionValidator7.Enabled = false;
        }

        protected void btnAddNewMobile_Click(object sender, EventArgs e)
        {
            newMobile2.Visible = true;
            btnAddNewMobile.Visible = false;
            RegularExpressionValidator8.Enabled = true;
        }

        protected void btnRemoveMobile2_Click(object sender, EventArgs e)
        {
            newMobile2.Visible = false;
            btnAddNewMobile.Visible = true;
            RegularExpressionValidator8.Enabled = false;

        }

        protected void btnAddNewMobile3_Click(object sender, EventArgs e)
        {
            newMobile3.Visible = true;
            btnAddNewMobile3.Visible = false;
            RegularExpressionValidator9.Enabled = true;
        }

        protected void btnRemoveNewMobile3_Click(object sender, EventArgs e)
        {
            newMobile3.Visible = false;
            btnAddNewMobile3.Visible = true;
            RegularExpressionValidator9.Enabled = false;
        }

        protected void btnAddNewFax_Click(object sender, EventArgs e)
        {
            newFax2.Visible = true;
            btnAddNewFax.Visible = false;
            RegularExpressionValidator10.Enabled = true;
        }

        protected void btnRemoveFax2_Click(object sender, EventArgs e)
        {
            newFax2.Visible = false;
            btnAddNewFax.Visible = true;
            RegularExpressionValidator10.Enabled = false;

        }

        protected void btnAddNewFax3_Click(object sender, EventArgs e)
        {
            newFax3.Visible = true;
            btnAddNewFax3.Visible = false;
            RegularExpressionValidator11.Enabled = true;
        }

        protected void btnRemoveFax3_Click(object sender, EventArgs e)
        {
            newFax3.Visible = false;
            btnAddNewFax3.Visible = true;
            RegularExpressionValidator11.Enabled = false;

        }

        protected void btnAddNewContact_Click(object sender, EventArgs e)
        {
            newContact2.Visible = true;
            pAddNewContact.Visible = false;
            RequiredFieldValidator3.Enabled = true;
            RequiredFieldValidator4.Enabled = true;
            RequiredFieldValidator5.Enabled = true;

        }

        protected void btnRemoveContact_Click(object sender, EventArgs e)
        {
            newContact2.Visible = false;
            pAddNewContact.Visible = true;
            RequiredFieldValidator3.Enabled = false;
            RequiredFieldValidator4.Enabled = false;
            RequiredFieldValidator5.Enabled = false;
        }
        protected void btnAddNewContact2_Click(object sender, EventArgs e)
        {
            newContact3.Visible = true;
            pAddNewContact2.Visible = false;
            RequiredFieldValidator6.Enabled = true;
            RequiredFieldValidator7.Enabled = true;
            RequiredFieldValidator8.Enabled = true;
        }

        protected void btnRemoveContact3_Click(object sender, EventArgs e)
        {
            newContact3.Visible = false;
            pAddNewContact2.Visible = true;
            RequiredFieldValidator6.Enabled = false;
            RequiredFieldValidator7.Enabled = false;
            RequiredFieldValidator8.Enabled = false;
        }

        //protected void btnAddConsultantEmail_Click(object sender, EventArgs e)
        //{
        //    newConsultantEmail2.Visible = true;
        //    btnAddConsultantEmail.Visible = false;
        //}

        //protected void btnRemoveConsultantEmail_Click(object sender, EventArgs e)
        //{
        //    newConsultantEmail2.Visible = false;
        //    btnAddConsultantEmail.Visible = true;
        //}

        //protected void btnAddConsultantEmail3_Click(object sender, EventArgs e)
        //{
        //    newConsultantEmail3.Visible = true;
        //    btnAddConsultantEmail3.Visible = false;
        //}

        //protected void btnRemoveConsultantEmail3_Click(object sender, EventArgs e)
        //{
        //    newConsultantEmail3.Visible = false;
        //    btnAddConsultantEmail3.Visible = true;
        //}

        //protected void btnAddNewConsultantPhone_Click(object sender, EventArgs e)
        //{
        //    newConsultantPhone2.Visible = true;
        //    btnAddNewConsultantPhone.Visible = false;

        //}

        //protected void btnRemoveConsultantPhone2_Click(object sender, EventArgs e)
        //{
        //    newConsultantPhone2.Visible = false;
        //    btnAddNewConsultantPhone.Visible = true;
        //}

        //protected void btnAddNewConsultantPhone2_Click(object sender, EventArgs e)
        //{
        //    newConsultantPhone3.Visible = true;
        //    btnAddNewConsultantPhone2.Visible = false;

        //}

        //protected void btnRemoveConsultantPhone3_Click(object sender, EventArgs e)
        //{
        //    newConsultantPhone3.Visible = false;
        //    btnAddNewConsultantPhone2.Visible = true;
        //}


        //protected void btnAddNewConsultantMobile_Click(object sender, EventArgs e)
        //{
        //    newConsultantMobile2.Visible = true;
        //    btnAddNewConsultantMobile.Visible = false;
        //}

        //protected void btnRemoveConsultantMobile2_Click(object sender, EventArgs e)
        //{
        //    newConsultantMobile2.Visible = false;
        //    btnAddNewConsultantMobile.Visible = true;
        //}

        //protected void btnAddNewConsultantMobile3_Click(object sender, EventArgs e)
        //{
        //    newConsultantMobile3.Visible = true;
        //    btnAddNewConsultantMobile3.Visible = false;
        //}

        //protected void btnRemoveConsultantMobile3_Click(object sender, EventArgs e)
        //{
        //    newConsultantMobile3.Visible = false;
        //    btnAddNewConsultantMobile3.Visible = true;
        //}

        //protected void btnAddConsultantFax_Click(object sender, EventArgs e)
        //{
        //    newConsultantFax2.Visible = true;
        //    btnAddConsultantFax.Visible = false;
        //}

        //protected void btnRemoveConsultantFax2_Click(object sender, EventArgs e)
        //{
        //    newConsultantFax2.Visible = false;
        //    btnAddConsultantFax.Visible = true;
        //}

        //protected void btnAddConsultantFax3_Click(object sender, EventArgs e)
        //{
        //    newConsultantFax3.Visible = true;
        //    btnAddConsultantFax3.Visible = false;
        //}

        //protected void btnRemoveConsultantFax3_Click(object sender, EventArgs e)
        //{
        //    newConsultantFax3.Visible = false;
        //    btnAddConsultantFax3.Visible = true;
        //}


        #endregion

        

        //protected void btnAddConsultantSpeciality_Click(object sender, EventArgs e)
        //{
        //    //btnAddConsultantSpeciality.Visible = false;
        //    consultantSpeciality2.Visible = true;

        //}

        //protected void btnRemoveConsultantSpeciality_Click(object sender, EventArgs e)
        //{
        //   // btnAddConsultantSpeciality.Visible = true;
        //    consultantSpeciality2.Visible = false;
        //}
        //protected void btnAddConsultantSpeciality2_Click(object sender, EventArgs e)
        //{
        //    btnAddConsultantSpeciality2.Visible = false;
        //    consultantSpeciality3.Visible = true;
        //}

        //protected void btnRemoveConsultantSpecialty3_Click(object sender, EventArgs e)
        //{
        //    btnAddConsultantSpeciality2.Visible = true;
        //    consultantSpeciality3.Visible = false;
        //}

        protected void CHB_SupportedByCompany_CheckedChanged(object sender, EventArgs e)
        {
            DDL_Supported.Visible = CHB_SupportedByCompany.Checked;
        }

        protected void ddlGovernate_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadArea();
        }

        protected void ddlGovernate2_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadArea2();
        }

        protected void ddlGovernate3_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadArea3();
        }

        protected void rptrConsaltant_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item 
                    || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    DataRowView consultRow = ((DataRowView)e.Item.DataItem);
                    DropDownList DDl_For = ((DropDownList)e.Item.FindControl("DDl_For"));
                    DropDownList ddlConsultantCountry = ((DropDownList)e.Item.FindControl("ddlConsultantCountry"));
                    DropDownList ddlConsultantGovernorate = ((DropDownList)e.Item.FindControl("ddlConsultantGovernorate"));
                    //RadioButton rbConsultantCompany = (RadioButton)e.Item.FindControl("rbConsultantCompany");
                    HtmlGenericControl consultantFirstDiv = (HtmlGenericControl)e.Item.FindControl("consultantFirstDiv");
                    HtmlGenericControl consultantSecondDiv = (HtmlGenericControl)e.Item.FindControl("consultantSecondDiv");
                    Label lblConsultantOffice = (Label)e.Item.FindControl("lblConsultantOffice");
                    TextBox tbxConsultantOffice = (TextBox)e.Item.FindControl("tbxConsultantOffice");
                    Label lblConsultantName = (Label)e.Item.FindControl("lblConsultantName");
                    TextBox tbxConsultantName = (TextBox)e.Item.FindControl("tbxConsultantName");
                    //RadioButton rbConsultantIndiv = (RadioButton)e.Item.FindControl("rbConsultantIndiv");
                    DropDownList DDL_ConsultantSpecialty = ((DropDownList)e.Item.FindControl("DDL_ConsultantSpecialty"));
                    DropDownList DDL_ConsultantSpecialty2 = ((DropDownList)e.Item.FindControl("DDL_ConsultantSpecialty2"));
                    DropDownList DDL_ConsultantSpeciality3 = ((DropDownList)e.Item.FindControl("DDL_ConsultantSpeciality3"));
                    TextBox tbxConsultantEmail = (TextBox)e.Item.FindControl("tbxConsultantEmail");
                    TextBox tbxConsultantEmail2 = (TextBox)e.Item.FindControl("tbxConsultantEmail2");
                    TextBox tbxConsultantEmail3 = (TextBox)e.Item.FindControl("tbxConsultantEmail3");
                    TextBox tbxConsultantPhone = (TextBox)e.Item.FindControl("tbxConsultantPhone");
                    TextBox tbxConsultantPhone2 = (TextBox)e.Item.FindControl("tbxConsultantPhone2");
                    TextBox tbxConsultantPhone3 = (TextBox)e.Item.FindControl("tbxConsultantPhone3");
                    TextBox tbxConsultantMobile = (TextBox)e.Item.FindControl("tbxConsultantMobile");
                    TextBox tbxConsultantMobile2 = (TextBox)e.Item.FindControl("tbxConsultantMobile2");
                    TextBox tbxConsultantMobile3 = (TextBox)e.Item.FindControl("tbxConsultantMobile3");
                    TextBox tbxConsultantFax = (TextBox)e.Item.FindControl("tbxConsultantFax");
                    TextBox tbxConsultantFax2 = (TextBox)e.Item.FindControl("tbxConsultantFax2");
                    TextBox tbxConsultantFax3 = (TextBox)e.Item.FindControl("tbxConsultantFax3");
                    TextBox tbxConsultantStreet = (TextBox)e.Item.FindControl("tbxConsultantStreet");
                    TextBox tbxConsultantBuilding = (TextBox)e.Item.FindControl("tbxConsultantBuilding");
                    TextBox tbxConsultantFloor = (TextBox)e.Item.FindControl("tbxConsultantFloor");
                    TextBox tbxConsultantDescription = (TextBox)e.Item.FindControl("tbxConsultantDescription");


                    DDL_ConsultantSpecialty.DataSource = VS_Speciality;
                    DDL_ConsultantSpecialty.DataTextField = GarasERP.Speciality.ColumnNames.Name;
                    DDL_ConsultantSpecialty.DataValueField = GarasERP.Speciality.ColumnNames.ID;
                    DDL_ConsultantSpecialty.DataBind();

                    DDL_ConsultantSpecialty2.DataSource = VS_Speciality;
                    DDL_ConsultantSpecialty2.DataTextField = GarasERP.Speciality.ColumnNames.Name;
                    DDL_ConsultantSpecialty2.DataValueField = GarasERP.Speciality.ColumnNames.ID;
                    DDL_ConsultantSpecialty2.DataBind();

                    DDL_ConsultantSpeciality3.DataSource = VS_Speciality;
                    DDL_ConsultantSpeciality3.DataTextField = GarasERP.Speciality.ColumnNames.Name;
                    DDL_ConsultantSpeciality3.DataValueField = GarasERP.Speciality.ColumnNames.ID;
                    DDL_ConsultantSpeciality3.DataBind();

                    BindConsGovDDL(ddlConsultantCountry, ddlConsultantGovernorate);

                    ddlConsultantCountry.DataSource = VS_Country;
                    ddlConsultantCountry.DataTextField = GarasERP.Country.ColumnNames.Name;
                    ddlConsultantCountry.DataValueField = GarasERP.Country.ColumnNames.ID;
                    ddlConsultantCountry.DataBind();
                    tbxConsultantOffice.Text = consultRow["ConsltOffice"].ToString();
                    tbxConsultantName.Text = consultRow["ConsltName"].ToString();
                    DDL_ConsultantSpecialty.SelectedValue = consultRow["Special1"].ToString();
                    DDl_For.SelectedValue = consultRow["ConsltFor"].ToString();
                    DDL_ConsultantSpecialty2.SelectedValue = consultRow["Special2"].ToString();
                    DDL_ConsultantSpeciality3.SelectedValue = consultRow["Special3"].ToString();
                    tbxConsultantEmail.Text = consultRow["ConsltEmail"].ToString();
                    tbxConsultantEmail2.Text = consultRow["ConsltEmail1"].ToString();
                    tbxConsultantEmail3.Text = consultRow["ConsltEmail2"].ToString();
                    tbxConsultantPhone.Text = consultRow["ConsltPhone"].ToString();
                    tbxConsultantPhone2.Text = consultRow["ConsltPhone1"].ToString();
                    tbxConsultantPhone3.Text = consultRow["ConsltPhone2"].ToString();
                    tbxConsultantMobile.Text = consultRow["ConsltMobile"].ToString();
                    tbxConsultantMobile2.Text = consultRow["ConsltMobile1"].ToString();
                    tbxConsultantMobile3.Text = consultRow["ConsltMobile2"].ToString();
                    tbxConsultantFax.Text = consultRow["ConsltFax"].ToString();
                    tbxConsultantFax2.Text = consultRow["ConsltFax1"].ToString();
                    tbxConsultantFax3.Text = consultRow["ConsltFax2"].ToString();
                    ddlConsultantCountry.SelectedValue = consultRow["ConsltCountry"].ToString();
                    ddlConsultantGovernorate.SelectedValue = consultRow["ConsltGvrnrt"].ToString();
                    tbxConsultantStreet.Text = consultRow["ConsltStreet"].ToString();
                    tbxConsultantBuilding.Text = consultRow["ConsltBuilding"].ToString();
                    tbxConsultantFloor.Text = consultRow["ConsltFloor"].ToString();
                    tbxConsultantDescription.Text = consultRow["ConsltDesc"].ToString();
                    //if (rbConsultantCompany.Checked == true)
                    //{
                    consultantFirstDiv.Visible = true;
                        consultantSecondDiv.Visible = true;

                        lblConsultantOffice.Visible = true;
                        tbxConsultantOffice.Visible = true;

                        lblConsultantName.Visible = true;
                        tbxConsultantName.Visible = true;
                    //}
                    //else if (rbConsultantIndiv.Checked == true)
                    //{
                    //    consultantFirstDiv.Visible = true;
                    //    consultantSecondDiv.Visible = true;
                    //    //consultantContacts.Visible = true;

                    //    lblConsultantOffice.Visible = false;
                    //    tbxConsultantOffice.Visible = false;

                    //    lblConsultantName.Visible = true;
                    //    tbxConsultantName.Visible = true;
                    //}
                }
                
            }
            catch (Exception ex)
            {
                //log error
            }
        }

        private void bindConsultFooter()
        {
            //(rptrConsaltant.Controls[rptrConsaltant.Controls.Count - 1].Controls[0].FindControl("lblTotal") as Label).Text = totalMarks.ToString();
        }
   
protected void rptrConsaltant_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            //Button button = (source as Button);
            RepeaterItem item = e.Item;//button.NamingContainer as RepeaterItem;
            HtmlGenericControl consultantSpeciality2 = (HtmlGenericControl)item.FindControl("consultantSpeciality2");
            Button btnAddConsultantSpeciality2 = (Button)item.FindControl("btnAddConsultantSpeciality2");
            HtmlGenericControl consultantSpeciality3 = (HtmlGenericControl)item.FindControl("consultantSpeciality3");
            Button btnAddConsultantEmail = (Button)item.FindControl("btnAddConsultantEmail");
            HtmlGenericControl newConsultantEmail2 = (HtmlGenericControl)item.FindControl("newConsultantEmail2");
            Button btnAddConsultantEmail3 = (Button)item.FindControl("btnAddConsultantEmail3");
            HtmlGenericControl newConsultantEmail3 = (HtmlGenericControl)item.FindControl("newConsultantEmail3");
            Button btnAddNewConsultantPhone = (Button)item.FindControl("btnAddNewConsultantPhone");
            HtmlGenericControl newConsultantPhone2 = (HtmlGenericControl)item.FindControl("newConsultantPhone2");
            Button btnAddNewConsultantPhone2 = (Button)item.FindControl("btnAddNewConsultantPhone2");
            HtmlGenericControl newConsultantPhone3 = (HtmlGenericControl)item.FindControl("newConsultantPhone3");
            Button btnAddNewConsultantMobile = (Button)item.FindControl("btnAddNewConsultantMobile");
            HtmlGenericControl newConsultantMobile2 = (HtmlGenericControl)item.FindControl("newConsultantMobile2");
            Button btnAddNewConsultantMobile3 = (Button)item.FindControl("btnAddNewConsultantMobile3");
            HtmlGenericControl newConsultantMobile3 = (HtmlGenericControl)item.FindControl("newConsultantMobile3");
            Button btnAddConsultantFax = (Button)item.FindControl("btnAddConsultantFax");
            HtmlGenericControl newConsultantFax2 = (HtmlGenericControl)item.FindControl("newConsultantFax2");
            Button btnAddConsultantFax3 = (Button)item.FindControl("btnAddConsultantFax3");
            HtmlGenericControl newConsultantFax3 = (HtmlGenericControl)item.FindControl("newConsultantFax3");
            switch (e.CommandName)
            {
                case "AddConsultantSpeciality":                    
                    consultantSpeciality2.Visible = true;
                    break;
                case "RemoveConsultantSpeciality":
                    consultantSpeciality2.Visible = false;
                    break;
                case "AddConsultantSpeciality2":
                    btnAddConsultantSpeciality2.Visible = false;
                    consultantSpeciality3.Visible = true;
                    break;
                case "RemoveConsultantSpecialty3":
                    btnAddConsultantSpeciality2.Visible = true;
                    consultantSpeciality3.Visible = false;
                    break;
                case "AddConsultantEmail":
                    newConsultantEmail2.Visible = true;
                    btnAddConsultantEmail.Visible = false;
                    break;
                case "RemoveConsultantEmail":
                    newConsultantEmail2.Visible = false;
                    btnAddConsultantEmail.Visible = true;
                    break;
                case "AddConsultantEmail3":
                    newConsultantEmail3.Visible = true;
                    btnAddConsultantEmail3.Visible = false;
                    break;
                case "RemoveConsultantEmail3":
                    newConsultantEmail3.Visible = false;
                    btnAddConsultantEmail3.Visible = true;
                    break;
                case "AddNewConsultantPhone":
                    newConsultantPhone2.Visible = true;
                    btnAddNewConsultantPhone.Visible = false;
                    break;
                case "RemoveConsultantPhone2":
                    newConsultantPhone2.Visible = false;
                    btnAddNewConsultantPhone.Visible = true;
                    break;
                case "AddNewConsultantPhone2":
                    newConsultantPhone3.Visible = true;
                    btnAddNewConsultantPhone2.Visible = false;
                    break;
                case "RemoveConsultantPhone3":
                    newConsultantPhone3.Visible = false;
                    btnAddNewConsultantPhone2.Visible = true;
                    break;
                case "AddNewConsultantMobile":
                    newConsultantMobile2.Visible = true;
                    btnAddNewConsultantMobile.Visible = false;
                    break;
                case "RemoveConsultantMobile2":
                    newConsultantMobile2.Visible = false;
                    btnAddNewConsultantMobile.Visible = true;
                    break;
                case "AddNewConsultantMobile3":
                    newConsultantMobile3.Visible = true;
                    btnAddNewConsultantMobile3.Visible = false;
                    break;
                case "RemoveConsultantMobile3":
                    newConsultantMobile3.Visible = false;
                    btnAddNewConsultantMobile3.Visible = true;
                    break;
                case "AddConsultantFax":
                    newConsultantFax2.Visible = true;
                    btnAddConsultantFax.Visible = false;
                    break;
                case "RemoveConsultantFax2":
                    newConsultantFax2.Visible = false;
                    btnAddConsultantFax.Visible = true;
                    break;
                case "AddConsultantFax3":
                    newConsultantFax3.Visible = true;
                    btnAddConsultantFax3.Visible = false;
                    break;
                case "RemoveConsultantFax3":
                    newConsultantFax3.Visible = false;
                    btnAddConsultantFax3.Visible = true;
                    break;
                case "AddConsultant":
                    addItemToVS_Consultant(e.Item);
                    rptrConsaltant.DataSource = VS_Consultant;
                    rptrConsaltant.DataBind();
                    break;
                case "RemoveConsultant":
                    DataTable dt = VS_Consultant;
                    dt.Rows.RemoveAt(item.ItemIndex);
                    VS_Consultant = dt;
                    rptrConsaltant.DataSource = VS_Consultant;
                    rptrConsaltant.DataBind();
                    break;
            }
        }

        private void addItemToVS_Consultant(RepeaterItem item)
        {
            DataTable dt = VS_Consultant;
            DataRow dr = dt.NewRow();
            dr["ConsltName"] = ((TextBox)item.FindControl("tbxConsultantName")).Text;
            dr["ConsltOffice"] = ((TextBox)item.FindControl("tbxConsultantOffice")).Text;
            dr["ConsltFor"] = ((DropDownList)item.FindControl("DDl_For")).SelectedValue; 
            dr["ConsltCountry"] = ((DropDownList)item.FindControl("ddlConsultantCountry")).SelectedValue;
            dr["ConsltGvrnrt"] = ((DropDownList)item.FindControl("ddlConsultantGovernorate")).SelectedValue;
            dr["ConsltStreet"] = ((TextBox)item.FindControl("tbxConsultantStreet")).Text; 
            dr["ConsltBuilding"] = ((TextBox)item.FindControl("tbxConsultantBuilding")).Text;
            dr["ConsltFloor"] = ((TextBox)item.FindControl("tbxConsultantFloor")).Text;
            dr["ConsltDesc"] = ((TextBox)item.FindControl("tbxConsultantDescription")).Text;
            dr["ConsltEmail"] = ((TextBox)item.FindControl("tbxConsultantEmail")).Text;
            dr["ConsltEmail1"] = ((TextBox)item.FindControl("tbxConsultantEmail2")).Text;
            dr["ConsltEmail2"] = ((TextBox)item.FindControl("tbxConsultantEmail3")).Text;
            dr["ConsltPhone"] = ((TextBox)item.FindControl("tbxConsultantPhone")).Text;
            dr["ConsltPhone1"] = ((TextBox)item.FindControl("tbxConsultantPhone2")).Text;
            dr["ConsltPhone2"] = ((TextBox)item.FindControl("tbxConsultantPhone3")).Text;
            dr["ConsltMobile"] = ((TextBox)item.FindControl("tbxConsultantMobile")).Text;
            dr["ConsltMobile1"] = ((TextBox)item.FindControl("tbxConsultantMobile2")).Text;
            dr["ConsltMobile2"] = ((TextBox)item.FindControl("tbxConsultantMobile3")).Text;
            dr["ConsltFax"] = ((TextBox)item.FindControl("tbxConsultantFax")).Text;
            dr["ConsltFax1"] = ((TextBox)item.FindControl("tbxConsultantFax2")).Text;
            dr["ConsltFax2"] = ((TextBox)item.FindControl("tbxConsultantFax3")).Text;
            dr["Special1"] = ((DropDownList)item.FindControl("DDL_ConsultantSpecialty")).SelectedValue;
            dr["Special2"] = ((DropDownList)item.FindControl("DDL_ConsultantSpecialty2")).SelectedValue;
            dr["Special3"] = ((DropDownList)item.FindControl("ddlSpeciality3")).SelectedValue;
            dt.Rows.Add(dr);
            VS_Consultant = dt;
        }
    }
}