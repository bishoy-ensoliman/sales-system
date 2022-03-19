<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/GarasERP.Master" AutoEventWireup="true" CodeBehind="ClientProfile.aspx.cs" Inherits="GarasSales.Sales.ClientProfile1" %>
<%@ Register TagPrefix="uc" TagName="Attachments" Src="~/Offers/AttachmentsUserControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Client Profile</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="container mt-5 clientPage">
        <div class="row upperCard">
            <div class="col">
                <div class="outerDiv">
                    <div class="innerDiv">
                        <asp:Label ID="LBL_MSG" runat="server" Text="" Visible="false" ForeColor="Red"></asp:Label>
                        <div class="contaienr" runat="server" id="DIV_Content" visible="false">

                            <div class="row">
                                <div class="col-md-2 pr-0">
                                    <div class="outerImg">
                                        <div class="clientImg text-center">
                                            <%--<img src="/UI/Images/beats-512.png">--%>
                                            <asp:Image ID="IMG_ClientImage" runat="server" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3 pl-0">
                                    <h3 class="robotoBold ">
                                        <asp:Label ID="LBL_ClientName" class="primaryBlue" runat="server"></asp:Label>
                                        <%--<span class="primaryBlue">Company</span>  <br> <span class="orangeColor">Name</span>--%>  

                                    </h3>
                                </div>
                                <div class="col-md-3 offset-md-4">
                                    <div class="evaluation px-2 py-1">
                                        <p class="d-inline-block mb-0 robotoReg pl-2">EVALUATION</p>

                                    </div>
                                </div>
                            </div>
                            <div class="row my-4">
                                <div class="col-md-9">
                                    <div class="d-inline-block companyCards text-center robotoBold ">
                                        <p class="mb-0 px-3">
                                            SINCE
                                            <br>
                                            <asp:Label ID="LBL_ClientDuration" class="number" runat="server"></asp:Label>
                                            <%--<span class="number">  12 </span>--%><br>
                                            YEAR
                                        </p>
                                    </div>

                                    <a href="#" class="d-inline-block companyCards text-center robotoBold">
                                        <p class="mb-0 pt-4 px-3">
                                            <%--<span class="number">25 </span>--%>
                                            <asp:Label ID="LBL_ClientOrders" class="number" Text="0" runat="server"></asp:Label>
                                            <br>
                                            ORDERS
                                        </p>
                                    </a>

                                    <div class="d-inline-block companyCards text-right robotoBold">
                                        <p class="mb-0 px-2">
                                            LE
                                            <br>
                                            <%--                                            <span class="number">2,000,000</span>--%>
                                            <asp:Label ID="LBL_clientBusinessVolume" Text="0" class="number" runat="server"></asp:Label>
                                            <br>
                                            <span class="mr-1">BUSINESS VOLUME </span>
                                        </p>
                                    </div>
                                    <a href="#" class="d-inline-block companyCards text-center robotoBold">
                                        <p class="mb-0 px-3">
                                            OPEN
                                            <br>
                                            <asp:Label ID="LBL_clientOpenOffers" class="number" Text="0" runat="server"></asp:Label>
                                            <%--                                            <span class="number">1 </span>--%>
                                            <br>
                                            OFFER
                                        </p>
                                    </a>
                                    <a href="#" class="d-inline-block companyCards text-center robotoBold">
                                        <p class="mb-0">
                                            CURRENT
                                            <br>
                                            <asp:Label ID="clientCurrentProjects" class="number" Text="0" runat="server"></asp:Label>

                                            <%--                                            <span class="number">2 </span>--%>
                                            <br>
                                            PROJECTS
                                        </p>
                                    </a>
                                </div>
                                <div class="col-md-3 robotoBold">
                                    <%--                                    <button class="btn CreateBtn text-white px-5">Create New Offer</button>--%>
                                    <asp:Button ID="btnCreateNewOffer" OnClick="btnCreateNewOffer_Click" Visible="false" class="btn CreateBtn text-white px-5" runat="server" Text="Create New Offer" />

                                    <%--                                    <button class="btn CreateBtn text-white px-5 my-2">Create Evaluation</button>--%>
                                    <asp:Button ID="btnCreateEvaluation" class="btn CreateBtn text-white px-5 my-2" runat="server" Text="Create Evaluation" />

                                    <button class="btn CreateBtn text-white px-5">Create Follow Up</button>
                                </div>
                            </div>

                            <h5 class="robotoMed darkBlueColor mt-5">CLIENT INFORMATION</h5>
                            <div class="clinetInfoBorder p-4">
                                <div class="container">
                                    <div class="row w-100 mx-auto my-3">

                                        <div class="col-md-1">
                                            <p class="robotoBold">TYPE </p>
                                        </div>
                                        <div id="radioButtonsDiv" runat="server" class="col-md-8 robotoReg">
                                            <asp:Label ID="LBL_ClientType" CssClass="containerRadio" runat="server" Text=""></asp:Label>
                                             <div class=" mt-3">
                                               <asp:CheckBox ID="CHB_SupportedByCompany" Text="Supported by the Company" Enabled="false" runat="server" />
                                                 <asp:TextBox ID="TXT_SupportedBy" Visible="false" ReadOnly="true" runat="server" CssClass="greyInput clinetInfoInput"></asp:TextBox>
                                            </div>
                                            <%--<label class="containerRadio">
                                                Small Company (One Branch)
                            <asp:RadioButton ID="rbSmall" runat="server" GroupName="rbgrpClientType" AutoPostBack="true" />
                                                <span class="radioCheck greyBorder disabledColor"></span>
                                            </label>



                                            <label class="containerRadio">
                                                Big Company (Multiple Branches)
                <asp:RadioButton ID="rbBig" runat="server" GroupName="rbgrpClientType" AutoPostBack="true" />
                                                <span class="radioCheck  greyBorder disabledColor"></span>
                                            </label>
                                            <label class="containerRadio">
                                                Group of Companies
                <asp:RadioButton ID="rbCompanies" runat="server" GroupName="rbgrpClientType" AutoPostBack="true" />
                                                <span class="radioCheck greyBorder disabledColor"></span>
                                            </label>
                                            <label class="containerRadio">
                                                Individual
                <asp:RadioButton ID="rbIndividual" runat="server" GroupName="rbgrpClientType" AutoPostBack="true" />
                                                <span class="radioCheck greyBorder disabledColor"></span>
                                            </label>--%>
                                        </div>
                                        
                                        <div class="col-md-3">
                                            <div class="clientCard p-0 m-0 text-center pt-3">
                                                <h5 class="text-right d-inline-block robotoBold primaryBlue">
                                                    <asp:Label ID="LBL_SalesBersonName" runat="server" Text=""></asp:Label><br>
                                                    <small class="robotoReg orangeColor">SALES PERSON</small></h5>
                                                <%--<img src="/UI/Images/user.jpg" class="mb-3">--%>
                                                <asp:Image ID="IMG_SalesManImage" CssClass="mb-3" runat="server" />
                                            </div>
                                            
                                            <div class="d-flex justify-content-end mt-3">
                                                <%--                                            <button class="btn text-white save px-5 py-1 robotoBold primaryColorBlue">EDIT</button>--%>
                                                <asp:LinkButton ID="LBTN_Edit" CssClass="btn text-white save px-5 py-1 robotoBold primaryColorBlue" runat="server" Text="Edit" OnClick="LBTN_Edit_Click"></asp:LinkButton>

                                            </div>
                                        </div>
                                    </div>
                                    <p class="robotoBold">Client Information </p>
                                    <%--                                                                        <asp:Panel ID="pnlClientInfo" runat="server" Enabled="false">--%>

                                    <div class="row">
                                        <%--<div class="col-md-9 mb-5">
                                            <p class="robotoReg d-inline-block mb-2 mr-5">Group Name</p>
                                            <%--<input type="text" class="greyInput  ml-4 px-5 clinetInfoInput">
                                            <asp:TextBox ID="tbxClientName" class="greyInput  ml-4 px-5 clinetInfoInput" runat="server"></asp:TextBox>
                                            <br>
                                            <p class="robotoReg d-inline-block mb-2">Company Name</p>
                                            <input type="text" class="greyInput ml-5 px-5 clinetInfoInput"><br>
                                            <p class="robotoReg d-inline-block mb-2">Branch \ Business Unit \ Project</p>
                                            <input type="text" class="greyInput ml-5 px-5"><br>
                                        </div>--%>

                                        <div class="col-md-9 mb-5">
                                            <asp:Label ID="lblGroupName" runat="server" class="robotoReg d-inline-block mb-2 mr-5" Visible="false">Group Name</asp:Label>
                                            <%--                                    <input type="text" class="greyInput  ml-4 px-5 clinetInfoInput">--%>
                                            <asp:TextBox ID="tbxGroupName" Enabled="false" class="greyInput  ml-4 px-5 clinetInfoInput" runat="server" Visible="false"></asp:TextBox>
                                            <p style="height: 1px; margin-bottom: 0px; margin-top: 0px"></p>
                                            <%--                                    <p class="robotoReg d-inline-block mb-2">Company Name</p>--%>
                                            <asp:Label ID="lblCompanyName" class="robotoReg d-inline-block mb-2" runat="server" Visible="false">Company Name</asp:Label>

                                            <%--                                    <input type="text" class="greyInput ml-5 px-5 clinetInfoInput"><br>--%>
                                            <asp:TextBox ID="tbxCompanyName" Enabled="false" class="greyInput ml-5 px-3 clinetInfoInput" runat="server" Visible="false"></asp:TextBox>

                                            <%--                                    <p class="robotoReg d-inline-block mb-2">Branch \ Business Unit \ Project</p>--%>
                                            <asp:Label ID="lblBranch" class="robotoReg d-inline-block mb-2" runat="server" Visible="false">Branch \ Business Unit \ Project</asp:Label>

                                            <%--                                    <input type="text" class="greyInput ml-5 px-5"><br>--%>
                                            <asp:TextBox ID="tbxBranch" Enabled="false" class="greyInput ml-5 px-3" runat="server" Visible="false"></asp:TextBox>

                                        </div>
                                    </div>
                                    <p class="robotoBold">Address </p>
                                    <asp:Repeater ID="RPT_Address" runat="server">
                                       
                                        <ItemTemplate>
                                            <div class="dropdown d-inline-block">
                                                <p class="robotoBold">Country </p>
                                                <asp:Label ID="LBL_Countery" runat="server" Text='<%# Eval("Country") %>'></asp:Label>
                                            </div>
                                             <div class="dropdown d-inline-block">
                                                <p class="robotoBold">Governate </p>
                                                <asp:Label ID="LBL_Governate" runat="server" Text='<%# Eval("Governate") %>'></asp:Label> 
                                            </div>
                                             <div class="dropdown d-inline-block">
                                                <p class="robotoBold">Area </p>
                                                <asp:Label ID="LBL_Area" runat="server" Text='<%# Eval("Area") %>'></asp:Label> 
                                            </div>
                                            <br />
                                            <p class="robotoReg d-inline-block mt-2 mr-3 mb-0">Street</p>
                                            <asp:TextBox ID="tbxStreet" Text='<%# Eval("Street") %>' ReadOnly="true" class="greyInput ml-5 px-5 clinetInfoInput" runat="server"></asp:TextBox><br />

                                            <%--                            <input type="text" class="greyInput ml-5 px-5 clinetInfoInput"><br>--%>
                                            <p class="robotoReg d-inline-block mt-2  mb-0">Building Number</p>
                                            <asp:TextBox ID="tbxBuilding" Text='<%# Eval("Bulding") %>' ReadOnly="true" type="text" class="greyInput ml-3 px-3" runat="server"></asp:TextBox><br />

                                            <%--                            <input type="text" class="greyInput ml-3  "><br>--%>
                                            <p class="robotoReg d-inline-block mt-2 mr-5 mb-0">Floor</p>
                                            <asp:TextBox ID="tbxFloor" Text='<%# Eval("Floor") %>' ReadOnly="true" class="greyInput ml-5 px-3" runat="server"></asp:TextBox><br />

                                            <%--                            <input type="text" class="greyInput ml-5"><br>--%>
                                            <p class="robotoReg d-inline-block mt-2 mr-5 mb-0">Description</p>
                                            <br>
                                            <%--                                    <asp:TextBox ID="tbxDesc" runat="server" TextMode="MultiLine"></asp:TextBox>--%>
                                            <asp:TextBox ID="tbxDesc" Text='<%# Eval("Desc") %>' ReadOnly="true" runat="server" Rows="4" Columns="100" class="p-3 w-100" TextMode="MultiLine"></asp:TextBox>

                                        </ItemTemplate>
                                       


                                    </asp:Repeater>

                                    <div class="row">
                                        <div class="col-md-9 mb-5">
                                            <p class="robotoBold">Contacts </p>
                                            <p class="robotoReg d-inline-block mb-2 mr-3">Email</p>
                                            <%--                                  <input type="text" class="greyInput ml-3 inputWidth2"><br>--%>
                                            <asp:TextBox ID="TXT_Email" ReadOnly="true" class="greyInput ml-3 inputWidth2 px-3" runat="server"></asp:TextBox><br />

                                            <p class="robotoReg d-inline-block mb-2 ">Website</p>
                                            <%--                                  <input type="text" class="greyInput ml-3 inputWidth2"><br>--%>
                                            <asp:TextBox ID="TXT_Website" ReadOnly="true" class="greyInput ml-3 inputWidth2 px-3" runat="server"></asp:TextBox><br />

                                            <asp:Repeater runat="server" ID="RPT_ContactPhone">
                                                <ItemTemplate>
                                                     <p class="robotoReg d-inline-block mb-2 ">Phone Number :</p>
                                            
                                                     <asp:TextBox ID="tbxPhone" ReadOnly="true" class="greyInput ml-3 px-3" Text='<%# Eval("Phone") %>' runat="server"></asp:TextBox>
                                                    <br/>
                                                </ItemTemplate>
                                            </asp:Repeater>

                                            <asp:Repeater runat="server" ID="RPT_ContactMobile">
                                                <ItemTemplate>
                                                      <p class="robotoReg d-inline-block mb-2 ">Mobile :</p>
                                                        <asp:TextBox ID="tbxMobile" ReadOnly="true" class="greyInput ml-3 px-3" Text='<%# Eval("Mobile") %>' runat="server"></asp:TextBox>
                                            
                                                     
                                                    <br/>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                            
                                           <asp:Repeater runat="server" ID="RPT_ContactFFAX">
                                                <ItemTemplate>
                                                      <p class="robotoReg d-inline-block mb-2 ">Fax :</p>
                                                        <asp:TextBox ID="tbxFAX" ReadOnly="true" class="greyInput ml-3 px-3" Text='<%# Eval("Fax") %>' runat="server"></asp:TextBox>
                                            
                                                     
                                                    <br/>
                                                </ItemTemplate>
                                            </asp:Repeater>

                                        </div>
                                    </div>

                                    <p class="robotoBold">Contact Persons</p>
                                    <asp:Repeater ID="RPT_ContactPerson" runat="server">
                                        <ItemTemplate>
                                            <div class="row">
                                                <div class="col-md-3 px-2">
                                                     <p class="robotoReg d-inline-block mb-2 ">NAME :</p>
                                                    <asp:TextBox ID="tbxContactName" ReadOnly="true" Text='<%# Eval("Name") %>' class="greyInput w-100 px-3" placeholder="NAME" runat="server"></asp:TextBox>
                                                    <br />
                                                </div>
                                            <div class="col-md-3 px-2">
                                                <p class="robotoReg d-inline-block mb-2 ">TITLE :</p>
                                                <asp:TextBox ID="tbxContactTitle" ReadOnly="true" Text='<%# Eval("Title") %>' class="greyInput w-100 px-3" placeholder="TITLE" runat="server"></asp:TextBox>
                                                <br />
                                            </div>
                                            <div class="col-md-3 px-2">
                                                <p class="robotoReg d-inline-block mb-2 ">E-MAIL :</p>
                                                <asp:TextBox ID="tbxContactEmail" ReadOnly="true" Text='<%# Eval("Email") %>' class="greyInput w-100 px-3 emailInput" placeholder="E-MAIL" runat="server"></asp:TextBox>
                                                <br />
                                               
                                            </div>
                                            <div class="col-md-3 px-2">
                                                <p class="robotoReg d-inline-block mb-2 ">MOBILE :</p>
                                                <asp:TextBox ID="tbxContactMobile" ReadOnly="true" Text='<%# Eval("Mobile") %>' class="greyInput w-100 px-3 emailInput " placeholder="MOBILE" runat="server"></asp:TextBox>
                                                <br />
                                                
                                            </div>
                                    </div>
                                        <div class="row mt-2">
                                            <div class="col-md-6 px-2">
                                                <p class="robotoReg d-inline-block mb-2 ">LOCATION :</p>
                                                <asp:TextBox ID="tbxContactLocation" ReadOnly="true" Text='<%# Eval("Location") %>' class="greyInput w-100 px-3" placeholder="LOCATION" runat="server"></asp:TextBox>

                                               <br />
                                                <br />
                                            </div>
                                        </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    
                                    
                                    <div class="row">
                                        <div class="col-md-1">
                                            <p class="robotoBold">Speciality</p>
                                        </div>
                                        <div class="col-md-8">
                                            <div class="dropdown">
                                               
                                                <asp:Repeater ID="RPT_ContactSpciality" runat="server">
                                                    <ItemTemplate>
                                                         <asp:TextBox ID="tbxContactLocation" ReadOnly="true" Text='<%# Eval("Speciality") %>' class="greyInput w-100 px-3" placeholder="LOCATION" runat="server"></asp:TextBox>
                                                          <br />

                                                    </ItemTemplate>
                                                </asp:Repeater>
                                               
                                                
                                            </div>
                                          
                                        </div>
                                    </div>
                                    <div class="row my-4">
                                        
                                    </div>
                                    <p class="robotoBold mt-4">Consultant</p>
                                    <div class="row my-2">
                                        <div class="col-md-4 offset-md-1 robotoReg">
                                            <asp:Label ID="LBL_ConsultanatType" runat="server" Text="N/A" CssClass="containerRadio"></asp:Label>
                                            
                                        </div>
                                    </div>
                                   
                                    <asp:Repeater ID="RPT_Consultant" OnItemDataBound="RPT_Consultant_ItemDataBound" runat="server" >
                                        <ItemTemplate>
                                         <div id="consultantFirstDiv" class="row" runat="server">
                                            <div class="col-md-9 ">
                                                <p class="robotoBold mt-4">Consultant</p>
                                                <asp:HiddenField ID="HDN_ID" runat="server" Value='<%# Eval("ID") %>' />
                                                <asp:Label ID="lblConsultantOffice" class="robotoReg d-inline-block mb-2 mr-1"  runat="server" Visible='<%# Eval("HasCompany") %>'>Company \ Office Name</asp:Label>
                                                <asp:TextBox ID="tbxConsultantOffice" runat="server" ReadOnly="true" Text='<%# Eval("Company") %>' class="greyInput  ml-4 px-5 clinetInfoInput" Visible='<%# Eval("HasCompany") %>'></asp:TextBox>
                                            <br>
                                                <asp:Label ID="lblConsultantName" class="robotoReg d-inline-block mb-2 mr-4" runat="server">Consultant Name</asp:Label>
                                                <asp:TextBox ID="tbxConsultantName" ReadOnly="true" Text='<%# Eval("ConsultantName") %>' class="greyInput ml-5 px-5 clinetInfoInput" runat="server"></asp:TextBox>
                                                <br>
                                                <div class="row">

                                                
                                                    <div class="col-md-4">
                                                        <p class="d-inline-block robotoReg">FOR</p>
                                                        <div class="dropdown d-inline-block">
                                                            

                                                            <asp:Label ID="LBL_Group" Text='<%# Eval("ConsultantFor") %>' runat="server" CssClass="btn btn-light dropdown-toggle py-0" ></asp:Label>
                                                        </div>
                                                    </div>
                                                    
                                                  
                                                    <div class="container">
                                                        <div class="row">
                                                            <div class="col-md-9">
                                                                <p class="robotoBold">Contacts </p>
                                                                 <asp:Repeater runat="server" ID="RPT_ConsContactEmail">
                                                                    <ItemTemplate>
                                                                         <p class="robotoReg d-inline-block mb-2 ">Email :</p>
                                            
                                                                         <asp:TextBox ID="tbxEmail" ReadOnly="true" class="greyInput ml-3 px-3" Text='<%# Eval("Email") %>' runat="server"></asp:TextBox>
                                                                        <br/>
                                                                    </ItemTemplate>
                                                                </asp:Repeater>
                                                                 <asp:Repeater runat="server" ID="RPT_ConsContactPhone">
                                                                    <ItemTemplate>
                                                                         <p class="robotoReg d-inline-block mb-2 ">Phone Number :</p>
                                            
                                                                         <asp:TextBox ID="tbxPhone" ReadOnly="true" class="greyInput ml-3 px-3" Text='<%# Eval("Phone") %>' runat="server"></asp:TextBox>
                                                                        <br/>
                                                                    </ItemTemplate>
                                                                </asp:Repeater>

                                                                <asp:Repeater runat="server" ID="RPT_ConsContactMobile">
                                                                    <ItemTemplate>
                                                                          <p class="robotoReg d-inline-block mb-2 ">Mobile :</p>
                                                                            <asp:TextBox ID="tbxMobile" ReadOnly="true" class="greyInput ml-3 px-3" Text='<%# Eval("Mobile") %>' runat="server"></asp:TextBox>
                                            
                                                     
                                                                        <br/>
                                                                    </ItemTemplate>
                                                                </asp:Repeater>
                                            
                                                               <asp:Repeater runat="server" ID="RPT_ConsContactFFAX">
                                                                    <ItemTemplate>
                                                                          <p class="robotoReg d-inline-block mb-2 ">Fax :</p>
                                                                            <asp:TextBox ID="tbxFAX" ReadOnly="true" class="greyInput ml-3 px-3" Text='<%# Eval("Fax") %>' runat="server"></asp:TextBox>
                                            
                                                     
                                                                        <br/>
                                                                    </ItemTemplate>
                                                                </asp:Repeater>

                                                               

                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>

                                            </div>

                                        </div>
                                            <div id="consultantSecondDiv" runat="server">
                                        <p class="robotoBold ">Address </p>
                                         <asp:Repeater ID="RPT_ConsaltsntAddress" runat="server">
                                       
                                        <ItemTemplate>
                                            <div class="dropdown d-inline-block">
                                                <p class="robotoBold">Country </p>
                                                <asp:Label ID="LBL_Countery" runat="server" Text='<%# Eval("Country") %>'></asp:Label>
                                            </div>
                                             <div class="dropdown d-inline-block">
                                                <p class="robotoBold">Governate </p>
                                                <asp:Label ID="LBL_Governate" runat="server" Text='<%# Eval("Governate") %>'></asp:Label> 
                                            </div><br />
                                            <p class="robotoReg d-inline-block mt-2 mr-3 mb-0">Street</p>
                                            <asp:TextBox ID="tbxStreet" Text='<%# Eval("Street") %>' ReadOnly="true" class="greyInput ml-5 px-5 clinetInfoInput" runat="server"></asp:TextBox><br />

                                            <%--                            <input type="text" class="greyInput ml-5 px-5 clinetInfoInput"><br>--%>
                                            <p class="robotoReg d-inline-block mt-2  mb-0">Building Number</p>
                                            <asp:TextBox ID="tbxBuilding" Text='<%# Eval("Bulding") %>' ReadOnly="true" type="text" class="greyInput ml-3 px-3" runat="server"></asp:TextBox><br />

                                            <%--                            <input type="text" class="greyInput ml-3  "><br>--%>
                                            <p class="robotoReg d-inline-block mt-2 mr-5 mb-0">Floor</p>
                                            <asp:TextBox ID="tbxFloor" Text='<%# Eval("Floor") %>' ReadOnly="true" class="greyInput ml-5 px-3" runat="server"></asp:TextBox><br />

                                            <%--                            <input type="text" class="greyInput ml-5"><br>--%>
                                            <p class="robotoReg d-inline-block mt-2 mr-5 mb-0">Description</p>
                                            <br>
                                            <%--                                    <asp:TextBox ID="tbxDesc" runat="server" TextMode="MultiLine"></asp:TextBox>--%>
                                            <asp:TextBox ID="tbxDesc" Text='<%# Eval("Desc") %>' ReadOnly="true" runat="server" Rows="4" Columns="100" class="p-3 w-100" TextMode="MultiLine"></asp:TextBox>

                                        </ItemTemplate>
                                       


                                    </asp:Repeater>
                                    </div>

                                       </ItemTemplate>
                                    </asp:Repeater>
                                    
                                  
                                    <p class="robotoBold mt-4">Follow-Up Period</p>
                                    <div class="dropdown d-inline-block">
                                        
                                        <asp:Label ID="LBL_FollowUpPeriod" runat="server" Text=""></asp:Label>
                                    </div>
                                    <label for="" class="robotoReg">Months</label>
                                    


                                   
                                    <p class="robotoBold mt-4">Attachment :</p>
                                    <div class="container">
                                       
                                       
                                            <uc:Attachments runat="server" id="AttachmentsBC" />
                                        <br>
                                        
                                    </div>
                                    <!-- Attachment END -->
                                    <div class=" mt-3">

                                        <h5 class="d-inline-block greyColorDisable">General Notes :</h5>


                                        <%--<textarea name="text" id="" cols="50" rows="5" placeholder="Write Your Text Here" class=" p-3 w-100 lightGreyDisable"></textarea>--%>
                                        <asp:TextBox ID="tbxNote" ReadOnly="true" runat="server" Rows="5" Columns="50" placeholder="Write Your Text Here" class=" p-3 w-100 lightGreyDisable" TextMode="MultiLine"></asp:TextBox>
                                    </div>
                                   <%-- <div class="d-flex justify-content-end my-3 mr-4">
                                       
                                        <asp:Button ID="btnSave" class="btn text-white save px-5 py-1 robotoBold primaryColorBlue" CausesValidation="true" ValidationGroup="updateClientValidation" runat="server" Text="Save" OnClick="btnSave_Click" />

                                    </div>--%>
                                    <%--                                                                        </asp:Panel>--%>
                                </div>

                            </div>




                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
