<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/GarasERP.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="NewClient.aspx.cs" Inherits="GarasSales.Sales.NewClient" %>

<%-- !!! THE LINE BELOW ACTS INSTEAD OF <br />  !!! --%>
<%--<p style="height: 1px; margin-bottom: 0px; margin-top: 0px"></p>--%>

<asp:Content ID="NewClientHead" ContentPlaceHolderID="head" runat="server">
    <title>Add New Client</title>
</asp:Content>

<asp:Content ID="NewClientBody" ContentPlaceHolderID="body" runat="server">
    <div class="container mt-5 mb-5">
        <div class="row upperCard">
            <div class="col">
                <div class="outerDiv">
                    <div class="innerDiv pb-5">
                        <div id="newClientPage" runat="server" visible="false" class="addContainer">

                            <!-- Client Type Radiobutton Groups BEGIN -->
                            <asp:UpdatePanel ID="updatePanelClientType" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <h4 class="robotoMed primaryBlue">ADD NEW CLIENT</h4>
                                    <div class="row my-5">
                                        <div class="col-md-1">
                                            <p class="robotoBold">TYPE </p>
                                        </div>
                                        <div class="col-md-4 robotoReg">
                                            <label class="containerRadio">
                                                Small Company (One Branch)
                <asp:RadioButton ID="rbSmall" runat="server" GroupName="rbgrpClientType" AutoPostBack="true" Checked="true" />
                                                <span class="radioCheck"></span>
                                            </label>

                                            <label class="containerRadio">
                                                Big Company (Multiple Branches)
                <asp:RadioButton ID="rbBig" runat="server" GroupName="rbgrpClientType" AutoPostBack="true" />
                                                <span class="radioCheck"></span>
                                            </label>

                                            <label class="containerRadio">
                                                Group of Companies
                <asp:RadioButton ID="rbCompanies" runat="server" GroupName="rbgrpClientType" AutoPostBack="true" />
                                                <span class="radioCheck"></span>
                                            </label>

                                            <label class="containerRadio">
                                                Individual
                <asp:RadioButton ID="rbIndividual" runat="server" GroupName="rbgrpClientType" AutoPostBack="true" />
                                                <span class="radioCheck"></span>
                                            </label>

                                        </div>
                                         <div class="col-md-4 robotoReg">
                                             <asp:CheckBox ID="CHB_SupportedByCompany" Text="Supported by the Company" AutoPostBack="true" OnCheckedChanged="CHB_SupportedByCompany_CheckedChanged" runat="server" />
                                             <asp:DropDownList ID="DDL_Supported" Visible="false" runat="server" CssClass="btn btn-light dropdown-toggle py-0 form-control">
                                                 <asp:ListItem Text="Facebook" Value="Facebook"></asp:ListItem>
                                                 <asp:ListItem Text="Yellow Pages" Value="Yellow Pages"></asp:ListItem>
                                                 <asp:ListItem Text="Mobile" Value="Mobile"></asp:ListItem>
                                                 <asp:ListItem Text="Fax" Value="Fax"></asp:ListItem>
                                                 <asp:ListItem Text="Phone" Value="Phone"></asp:ListItem>
                                                 <asp:ListItem Text="Visit" Value="Visit"></asp:ListItem>
                                             </asp:DropDownList>
                                        </div>
                                    </div>
                                    <!-- Client Type Radiobutton Groups END -->

                                    <!-- Client Info BEGIN -->
                                    <p class="robotoBold">Client Information </p>
                                    <div class="row">
                                        <div class="col-md-9 mb-5">
                                            <div id="clientPhoto">
                                                <p class="robotoReg d-inline-block mb-2 robotoReg">Client Photo</p>
                                                <asp:FileUpload ID="uploadPhoto" runat="server" accept=".png,.jpg,.jpeg,.gif" class="ml-3"/>
                                                <%--                                        <ajaxToolkit:AjaxFileUpload ID="fileUpload" runat="server" />--%>
                                            </div>
                                            <asp:Label ID="lblGroupName" runat="server" class="robotoReg d-inline-block mb-2 mr-5" Visible="false">Group Name <span>*</span></asp:Label>
                                            <%--                                    <input type="text" class="greyInput  ml-4 px-5 clinetInfoInput">--%>
                                            <asp:TextBox ID="tbxGroupName" class="greyInput  ml-4 px-5 clinetInfoInput" runat="server" Visible="false"></asp:TextBox>
                                           <asp:RequiredFieldValidator ID="RFV_tbxGroupName" ValidationGroup="NewClientValidation" ControlToValidate="tbxGroupName" Display="Dynamic" SetFocusOnError="true" runat="server" Enabled="false" ErrorMessage="*"></asp:RequiredFieldValidator>
                                             <p style="height: 1px; margin-bottom: 0px; margin-top: 0px"></p>
                                            <%--                                    <p class="robotoReg d-inline-block mb-2">Company Name</p>--%>
                                            <asp:Label ID="lblCompanyName" class="robotoReg d-inline-block mb-2" runat="server" Visible="false">Company Name <span>*</span></asp:Label>

                                            <%--                                    <input type="text" class="greyInput ml-5 px-5 clinetInfoInput"><br>--%>
                                            <asp:TextBox ID="tbxCompanyName"  class="greyInput ml-5 px-5 clinetInfoInput" runat="server" Visible="false"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RFV_CompanyName" ValidationGroup="NewClientValidation" ControlToValidate="tbxCompanyName" Display="Dynamic" SetFocusOnError="true" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                            <%--                                    <p class="robotoReg d-inline-block mb-2">Branch \ Business Unit \ Project</p>--%>
                                            <asp:Label ID="lblBranch" class="robotoReg d-inline-block mb-2" runat="server" Visible="false">Branch \ Business Unit \ Project <span>*</span></asp:Label>

                                            <%--                                    <input type="text" class="greyInput ml-5 px-5"><br>--%>
                                            <asp:TextBox ID="tbxBranch" class="greyInput ml-5 px-5" runat="server" Visible="false"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RFV_tbxBranch" ValidationGroup="NewClientValidation" ControlToValidate="tbxGroupName" Display="Dynamic" SetFocusOnError="true" runat="server" Enabled="false" ErrorMessage="*"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>


                                    <p class="robotoBold">Address </p>

                                    <div class="dropdown d-inline-block">
                                        <asp:DropDownList ID="ddlCountry" CssClass="btn btn-light dropdown-toggle py-0 form-control" runat="server" ToolTip="Country" AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                                        </asp:DropDownList>
                                      
                                    </div>
                                    <div class="dropdown d-inline-block">
                                        <asp:DropDownList ID="ddlGovernate" OnSelectedIndexChanged="ddlGovernate_SelectedIndexChanged" AutoPostBack="true" runat="server" class="btn btn-light dropdown-toggle py-0 form-control">
                                        </asp:DropDownList>
                                      
                                    </div>

                                     <div class="dropdown d-inline-block">
                                        <asp:DropDownList ID="DDL_Area" runat="server" class="btn btn-light dropdown-toggle py-0 form-control">
                                        </asp:DropDownList>
                                      
                                    </div>
                                    <br>
                                    <p class="robotoReg d-inline-block mt-2 mr-3 mb-0">Street</p>
                                    <asp:TextBox ID="tbxStreet" class="greyInput ml-5 px-5 clinetInfoInput" runat="server"></asp:TextBox><br />

                                    <%--                            <input type="text" class="greyInput ml-5 px-5 clinetInfoInput"><br>--%>
                                    <p class="robotoReg d-inline-block mt-2  mb-0">Building Number</p>
                                    <asp:TextBox ID="tbxBuilding" type="text" class="greyInput ml-3 px-3" runat="server"></asp:TextBox><br />

                                    <%--                            <input type="text" class="greyInput ml-3  "><br>--%>
                                    <p class="robotoReg d-inline-block mt-2 mr-5 mb-0">Floor</p>
                                    <asp:TextBox ID="tbxFloor" class="greyInput ml-5 px-3" runat="server"></asp:TextBox><br />

                                    <%--                            <input type="text" class="greyInput ml-5"><br>--%>
                                    <p class="robotoReg d-inline-block mt-2 mr-5 mb-0">Description</p>
                                    <br>
                                    <%--                                    <asp:TextBox ID="tbxDesc" runat="server" TextMode="MultiLine"></asp:TextBox>--%>
                                    <asp:TextBox ID="tbxDesc" runat="server" Rows="4" Columns="100" class="p-3 w-100" TextMode="MultiLine"></asp:TextBox>

                                    <%--                            <textarea name="" id="" cols="100" rows="4"></textarea>--%>
                                    <div class="row my-4">
                                        <div class="col-md-4 offset-md-8">
                                            <div class="buttons ml-5">
                                                <p class="d-inline-block orangeColor robotoMed ml-2" runat="server" id="pAddNewAddress1">
                                                    <asp:Button ID="btnAddNewAddress1" runat="server" class="btn mr-3 orangeBg text-white" CausesValidation="false" Text="+" OnClick="btnAddNewAddress1_Click" />Add Other Location
                                                </p>
                                            </div>
                                        </div>
                                    </div>

                                    <!-- Address 2 -->
                                    <div id="address2" runat="server" visible="false">
                                        <p class="robotoBold">Address </p>
                                        <asp:Button ID="btnRemoveAddress" class="btn mr-3 orangeBg text-white" runat="server" CausesValidation="false" Text="X" OnClick="btnRemoveAddress2_Click" />

                                        <div class="dropdown d-inline-block">
                                            <asp:DropDownList ID="ddlCountry2" CssClass="btn btn-light dropdown-toggle py-0 form-control" runat="server" ToolTip="Country" AutoPostBack="true" OnSelectedIndexChanged="ddlCountry2_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>

                                        <div class="dropdown d-inline-block">
                                            <asp:DropDownList ID="ddlGovernate2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlGovernate2_SelectedIndexChanged" class="btn btn-light dropdown-toggle py-0 form-control">
                                            </asp:DropDownList>

                                        </div>
                                        <div class="dropdown d-inline-block">
                                            <asp:DropDownList ID="DDL_Area2" runat="server" class="btn btn-light dropdown-toggle py-0 form-control">
                                            </asp:DropDownList>
                                      
                                        </div>
                                        <br>
                                        <p class="robotoReg d-inline-block mt-2 mr-3 mb-0">Street</p>
                                        <asp:TextBox ID="tbxStreet2" class="greyInput ml-5 px-5 clinetInfoInput" runat="server"></asp:TextBox><br />

                                        <p class="robotoReg d-inline-block mt-2  mb-0">Building Number</p>
                                        <asp:TextBox ID="tbxBuilding2" type="text" class="greyInput ml-3" runat="server"></asp:TextBox><br />

                                        <p class="robotoReg d-inline-block mt-2 mr-5 mb-0">Floor</p>
                                        <asp:TextBox ID="tbxFloor2" class="greyInput ml-5" runat="server"></asp:TextBox><br />

                                        <p class="robotoReg d-inline-block mt-2 mr-5 mb-0">Description</p>
                                        <br>
                                        <asp:TextBox ID="tbxDesc2" runat="server" Rows="4" Columns="100" class="p-3 w-100" TextMode="MultiLine"></asp:TextBox>

                                        <div class="row my-4">
                                            <div class="col-md-4 offset-md-8">
                                                <div class="buttons ml-5">
                                                    <p class="d-inline-block orangeColor robotoMed ml-2" runat="server" id="pAddNewAddress2">
                                                        <asp:Button ID="btnAddNewAddress2" runat="server" class="btn mr-3 orangeBg text-white" CausesValidation="false" Text="+" OnClick="btnAddNewAddress2_Click" />Add Other Location
                                                    </p>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- Address 2 END -->

                                    <!-- Address 3 -->
                                    <div id="address3" runat="server" visible="false">
                                        <p class="robotoBold">Address </p>
                                        <asp:Button ID="btnRemoveAddress3" class="btn mr-3 orangeBg text-white" runat="server" CausesValidation="false" Text="X" OnClick="btnRemoveAddress3_Click" />

                                        <div class="dropdown d-inline-block">
                                            <asp:DropDownList ID="ddlCountry3" CssClass="btn btn-light dropdown-toggle py-0 form-control" runat="server" ToolTip="Country" AutoPostBack="true" OnSelectedIndexChanged="ddlCountry3_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>

                                        <div class="dropdown d-inline-block">
                                            <asp:DropDownList ID="ddlGovernate3" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlGovernate3_SelectedIndexChanged" class="btn btn-light dropdown-toggle py-0 form-control">
                                            </asp:DropDownList>

                                        </div>
                                        <div class="dropdown d-inline-block">
                                            <asp:DropDownList ID="DDL_Area3" runat="server" class="btn btn-light dropdown-toggle py-0 form-control">
                                            </asp:DropDownList>
                                      
                                        </div>

                                        <br>
                                        <p class="robotoReg d-inline-block mt-2 mr-3 mb-0">Street</p>
                                        <asp:TextBox ID="tbxStreet3" class="greyInput ml-5 px-5 clinetInfoInput" runat="server"></asp:TextBox><br />

                                        <p class="robotoReg d-inline-block mt-2  mb-0">Building Number</p>
                                        <asp:TextBox ID="tbxBuilding3" type="text" class="greyInput ml-3" runat="server"></asp:TextBox><br />

                                        <p class="robotoReg d-inline-block mt-2 mr-5 mb-0">Floor</p>
                                        <asp:TextBox ID="tbxFloor3" class="greyInput ml-5" runat="server"></asp:TextBox><br />

                                        <p class="robotoReg d-inline-block mt-2 mr-5 mb-0">Description</p>
                                        <br>
                                        <asp:TextBox ID="tbxDesc3" runat="server" Rows="4" Columns="100" class="p-3 w-100" TextMode="MultiLine"></asp:TextBox>


                                    </div>
                                    <!-- Address 3 END -->


                                    <!-- Client Info END -->

                                    <!-- Client Contacts BEGIN -->
                                    <div class="row">
                                        <div class="col-md-9 mb-5">
                                            <p class="robotoBold">Contacts </p>
                                            <p class="robotoReg d-inline-block mb-2 mr-3">Email</p>
                                            <asp:TextBox ID="tbxEmail" class="greyInput ml-3 inputWidth2" runat="server"></asp:TextBox>
                                            <%--<asp:RequiredFieldValidator ID="RequiredEmail" runat="server" ControlToValidate="tbxEmail"
                                        ErrorMessage="You must Enter an email" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>--%>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="tbxEmail"
                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="NewClientValidation"
                                                ErrorMessage="Please enter vaild Email" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>
                                            <br>
                                            <p class="robotoReg d-inline-block mb-2 ">Website</p>
                                            <asp:TextBox ID="tbxWebsite" class="greyInput ml-3 inputWidth2" runat="server"></asp:TextBox>
                                            <br>
                                            <p class="robotoReg d-inline-block mb-2 ">Phone Number :</p>
                                            <asp:TextBox ID="tbxPhone" class="greyInput ml-3" runat="server"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="vldPhone" runat="server" ControlToValidate="tbxPhone"
                                                ValidationExpression="\d+" ValidationGroup="NewClientValidation"
                                                ErrorMessage="Please enter numbers only" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>

                                            <asp:Button ID="btnAddNewPhone" runat="server" Text="+" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" OnClick="btnAddNewPhone_Click" />

                                            <div id="newPhone" runat="server" visible="false">
                                                <asp:Button ID="removeNewButton" runat="server" Text="x" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" OnClick="removeNewButton_Click" />

                                                <asp:TextBox ID="tbxPhone2" class="greyInput ml-3" runat="server"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="tbxPhone2"
                                                    ValidationExpression="\d+" Enabled="false" ValidationGroup="NewClientValidation"
                                                    ErrorMessage="Please enter numbers only" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>
                                                <asp:Button ID="btnAddNewPhone2" runat="server" Text="+" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" OnClick="btnAddNewPhone2_Click" />

                                            </div>

                                            <div id="newPhone3" runat="server" visible="false">
                                                <asp:Button ID="removeNewButton3" runat="server" Text="x" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" OnClick="removeNewButton3_Click" />

                                                <asp:TextBox ID="tbxPhone3" class="greyInput ml-3" runat="server"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ControlToValidate="tbxPhone3"
                                                    ValidationExpression="\d+" Enabled="false"
                                                    ValidationGroup="NewClientValidation" ErrorMessage="Please enter numbers only" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>


                                            </div>

                                            <br>

                                            <p class="robotoReg d-inline-block mb-2 ">Mobile :</p>
                                            <%--                                    <input type="text" class="greyInput ml-3">--%>
                                            <asp:TextBox ID="tbxMobile" class="greyInput ml-3" runat="server"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="vldMobile" runat="server" ControlToValidate="tbxMobile"
                                                ValidationExpression="\d+"
                                                ValidationGroup="NewClientValidation" ErrorMessage="Please enter numbers only" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>
                                            <%--                                    <button type="button" class="btn mr-3 darkBlue text-white robotoMed">+</button>--%>
                                            <asp:Button ID="btnAddNewMobile" runat="server" Text="+" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" OnClick="btnAddNewMobile_Click" />

                                            <div id="newMobile2" runat="server" visible="false">
                                                <asp:Button ID="btnRemoveMobile2" runat="server" Text="x" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" OnClick="btnRemoveMobile2_Click" />

                                                <asp:TextBox ID="tbxMobile2" class="greyInput ml-3" runat="server"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ControlToValidate="tbxMobile2"
                                                    ValidationExpression="\d+" Enabled="false"
                                                    ValidationGroup="NewClientValidation" ErrorMessage="Please enter numbers only" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>
                                                <asp:Button ID="btnAddNewMobile3" runat="server" Text="+" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" OnClick="btnAddNewMobile3_Click" />

                                            </div>

                                            <div id="newMobile3" runat="server" visible="false">
                                                <asp:Button ID="btnRemoveNewMobile3" runat="server" Text="x" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" OnClick="btnRemoveNewMobile3_Click" />

                                                <asp:TextBox ID="tbxMobile3" class="greyInput ml-3" runat="server"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" ControlToValidate="tbxMobile3"
                                                    ValidationExpression="\d+" Enabled="false"
                                                    ValidationGroup="NewClientValidation" ErrorMessage="Please enter numbers only" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>


                                            </div>

                                            <br>
                                            <p class="robotoReg d-inline-block mb-2 ">Fax :</p>
                                            <%--                                    <input type="text" class="greyInput ml-3">--%>
                                            <asp:TextBox ID="tbxFax" class="greyInput ml-3" runat="server"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="vldFax" runat="server" ControlToValidate="tbxFax"
                                                ValidationExpression="\d+"
                                                ValidationGroup="NewClientValidation" ErrorMessage="Please enter numbers only" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>
                                            <%--                                            <button type="button" class="btn mr-3 darkBlue text-white robotoMed">+</button>--%>

                                            <asp:Button ID="btnAddNewFax" runat="server" Text="+" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" OnClick="btnAddNewFax_Click" />


                                            <div id="newFax2" runat="server" visible="false">
                                                <asp:Button ID="btnRemoveFax2" runat="server" Text="x" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" OnClick="btnRemoveFax2_Click" />

                                                <asp:TextBox ID="tbxFax2" class="greyInput ml-3" runat="server"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator10" runat="server" ControlToValidate="tbxFax2"
                                                    ValidationExpression="\d+" Enabled="false"
                                                    ValidationGroup="NewClientValidation" ErrorMessage="Please enter numbers only" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>
                                                <asp:Button ID="btnAddNewFax3" runat="server" Text="+" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" OnClick="btnAddNewFax3_Click" />

                                            </div>

                                            <div id="newFax3" runat="server" visible="false">
                                                <asp:Button ID="btnRemoveFax3" runat="server" Text="x" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" OnClick="btnRemoveFax3_Click" />

                                                <asp:TextBox ID="tbxFax3" class="greyInput ml-3" runat="server"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator11" runat="server" ControlToValidate="tbxFax3"
                                                    ValidationExpression="\d+" Enabled="false"
                                                    ValidationGroup="NewClientValidation" ErrorMessage="Please enter numbers only" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>


                                            </div>


                                        </div>
                                    </div>
                                    <!-- Client Contacts END -->


                                    <!-- Contact Persons BEGIN -->
                                    <p class="robotoBold">Contact Persons <span>*</span></p>
                                    <div class="row">
                                        <div class="col-md-3 px-2">
                                            <%--                                    <input type="text" class="greyInput w-100 px-3" placeholder="NAME">--%>
                                            <asp:TextBox ID="tbxContactPerson" placeholder="Name" class="greyInput w-100 px-3" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredEmail" runat="server" ControlToValidate="tbxContactPerson"
                                                ValidationGroup="NewClientValidation" ErrorMessage="You must Enter a Contact Person" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-3 px-2">
                                            <%--                                    <input type="text" class="greyInput w-100 px-3" placeholder="TITLE">--%>
                                           <%-- <asp:TextBox ID="tbxContactTitle" class="greyInput w-100 px-3" placeholder="Title" runat="server"></asp:TextBox>--%>
                                            <asp:DropDownList ID="DDL_Title" runat="server" class="btn btn-light dropdown-toggle py-0 form-control"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="DDL_Title"
                                                ValidationGroup="NewClientValidation" ErrorMessage="You must Enter the Contact Person Title" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-3 px-2">
                                            <%--                                    <input type="text" class="greyInput w-100 px-3 emailInput" placeholder="E-MAIL">--%>
                                            <asp:TextBox ID="tbxContactEmail" runat="server" class="greyInput w-100 px-3 emailInput" placeholder="E-mail"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3 px-2">
                                            <%--                                    <input type="text" class="greyInput w-100 px-3 emailInput " placeholder="MOBILE">--%>
                                            <asp:TextBox ID="tbxContactMobile" class="greyInput w-100 px-3 emailInput " placeholder="Mobile" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbxContactMobile"
                                                ValidationGroup="NewClientValidation" ErrorMessage="You must Enter the Contact Person Title" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="row mt-2">
                                        <div class="col-md-6 px-2">
                                            <%--<input type="text" class="greyInput w-100 px-3" placeholder="LOCATION">--%>
                                            <asp:TextBox ID="tbxContactLocation" class="greyInput w-100 px-3" placeholder="Location" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row my-4">
                                        <div class="col-md-4 offset-md-8">
                                            <div class="buttons ml-5">
                                                <p id="pAddNewContact" runat="server" class="d-inline-block orangeColor robotoMed ml-2">
                                                    <asp:Button ID="btnAddNewContact" CausesValidation="false" runat="server" class="btn mr-3 orangeBg text-white" Text="+" OnClick="btnAddNewContact_Click" />Add New Contact Person
                                                </p>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- Contact Persons END -->

                                    <div id="newContact2" runat="server" visible="false">

                                        <!-- Contact Persons 2 BEGIN -->
                                        <p class="robotoBold">Contact Persons <span>*</span></p>
                                        <asp:Button ID="btnRemoveContact" CausesValidation="false" runat="server" class="btn mr-3 orangeBg text-white" Text="x" OnClick="btnRemoveContact_Click" />

                                        <div class="row">
                                            <div class="col-md-3 px-2">
                                                <asp:TextBox ID="tbxContactName2" placeholder="Name" class="greyInput w-100 px-3" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" Enabled="false" runat="server" ControlToValidate="tbxContactName2"
                                                    ValidationGroup="NewClientValidation" ErrorMessage="You must Enter a Contact Person" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-md-3 px-2">
                                                <%--<asp:TextBox ID="tbxContactTitle2" class="greyInput w-100 px-3" placeholder="Title" runat="server"></asp:TextBox>--%>
                                                <asp:DropDownList ID="DDL_Title2" runat="server" class="btn btn-light dropdown-toggle py-0 form-control"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" Enabled="false" runat="server" ControlToValidate="DDL_Title2"
                                                    ValidationGroup="NewClientValidation" ErrorMessage="You must Enter the Contact Person Title" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-md-3 px-2">
                                                <asp:TextBox ID="tbxContactEmail2" runat="server" class="greyInput w-100 px-3 emailInput" placeholder="E-mail"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 px-2">
                                                <asp:TextBox ID="tbxContactMobile2" class="greyInput w-100 px-3 emailInput " placeholder="Mobile" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" Enabled="false" runat="server" ControlToValidate="tbxContactMobile2"
                                                    ValidationGroup="NewClientValidation" ErrorMessage="You must Enter the Contact Person Title" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="row mt-2">
                                            <div class="col-md-6 px-2">
                                                <asp:TextBox ID="tbxContactLocation2" class="greyInput w-100 px-3" placeholder="Location" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row my-4">
                                            <div class="col-md-4 offset-md-8">
                                                <div class="buttons ml-5">
                                                    <p id="pAddNewContact2" runat="server" class="d-inline-block orangeColor robotoMed ml-2">
                                                        <asp:Button ID="btnAddNewContact2" CausesValidation="false" runat="server" class="btn mr-3 orangeBg text-white" Text="+" OnClick="btnAddNewContact2_Click" />Add New Contact Person
                                                    </p>
                                                </div>
                                            </div>
                                        </div>
                                        <!-- Contact Persons 2 END -->

                                    </div>

                                    <div id="newContact3" runat="server" visible="false">
                                        <!-- Contact Persons 3 BEGIN -->
                                        <p class="robotoBold">Contact Persons <span>*</span></p>
                                        <asp:Button ID="btnRemoveContact3" CausesValidation="false" runat="server" class="btn mr-3 orangeBg text-white" Text="x" OnClick="btnRemoveContact3_Click" />

                                        <div class="row">
                                            <div class="col-md-3 px-2">
                                                <asp:TextBox ID="tbxContactName3" placeholder="Name" class="greyInput w-100 px-3" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" Enabled="false" runat="server" ControlToValidate="tbxContactName3"
                                                    ValidationGroup="NewClientValidation" ErrorMessage="You must Enter a Contact Person" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-md-3 px-2">
                                               <%-- <asp:TextBox ID="tbxContactTitle3" class="greyInput w-100 px-3" placeholder="Title" runat="server"></asp:TextBox>--%>
                                               <asp:DropDownList ID="DDL_Title3" runat="server" class="btn btn-light dropdown-toggle py-0 form-control"></asp:DropDownList>
                                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator7" Enabled="false" runat="server" ControlToValidate="DDL_Title3"
                                                    ValidationGroup="NewClientValidation" ErrorMessage="You must Enter the Contact Person Title" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-md-3 px-2">
                                                <asp:TextBox ID="tbxContactEmail3" runat="server" class="greyInput w-100 px-3 emailInput" placeholder="E-mail"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 px-2">
                                                <asp:TextBox ID="tbxContactMobile3" class="greyInput w-100 px-3 emailInput " placeholder="Mobile" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" Enabled="false" runat="server" ControlToValidate="tbxContactMobile3"
                                                    ValidationGroup="NewClientValidation" ErrorMessage="You must Enter the Contact Person Title" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="row mt-2">
                                            <div class="col-md-6 px-2">
                                                <asp:TextBox ID="tbxContactLocation3" class="greyInput w-100 px-3" placeholder="Location" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                        <!-- Contact Persons 3 BEGIN -->

                                    </div>


                                    <!-- Speciality BEGIN -->
                                    <div class="row">
                                        <div class="col-md-1">
                                            <p class="robotoBold">Speciality</p>
                                        </div>
                                        <div class="col-md-8">
                                            <div class="dropdown">
                                                <%--  <a class="btn btn-light dropdown-toggle py-0" href="#" role="button" id="dropdownMenuLink" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                            <span class="pr-5 mr-5">1.</span>
                                        </a>--%>
                                                <asp:DropDownList ID="ddlSpeciality" runat="server" class="btn btn-light dropdown-toggle py-0"></asp:DropDownList>

                                                <asp:Button ID="btnAddSpeciality" runat="server" class="btn mr-3 orangeBg text-white" Text="+" OnClick="btnAddSpeciality_Click" />

                                            </div>


                                            <div id="speciality2" runat="server" visible="false">
                                                <div class="dropdown">
                                                    <asp:Button ID="btnRemoveSpeciality" runat="server" Text="x" class="btn mr-3 orangeBg text-white" OnClick="btnRemoveSpeciality_Click" />
                                                    <asp:DropDownList ID="ddlSpeciality2" runat="server" class="btn btn-light dropdown-toggle py-0"></asp:DropDownList>

                                                    <asp:Button ID="btnAddSpeciality3" runat="server" class="btn mr-3 orangeBg text-white" Text="+" OnClick="btnAddSpeciality3_Click" />



                                                </div>

                                            </div>
                                            <div id="speciality3" runat="server" visible="false">
                                                <div class="dropdown">
                                                    <asp:Button ID="btnRemoveSpeciality3" runat="server" Text="x" OnClick="btnRemoveSpeciality3_Click" class="btn mr-3 orangeBg text-white" />
                                                    <%--<a class="btn btn-light dropdown-toggle py-0" href="#" role="button" id="dropdownMenuLink" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                <span class="pr-5 mr-5">2.</span>
                                            </a>--%>
                                                    <asp:DropDownList ID="ddlSpeciality3" runat="server" class="btn btn-light dropdown-toggle py-0"></asp:DropDownList>
                                                    <asp:Button ID="btnAddSpeciality4" runat="server" Text="+" OnClick="btnAddSpeciality4_Click" class="btn mr-3 orangeBg text-white" />

                                                </div>
                                            </div>
                                            <div id="speciality4" runat="server" visible="false">
                                                <div class="dropdown">
                                                    <asp:Button ID="btnRemoveSpeciality4" runat="server" Text="x" OnClick="btnRemoveSpeciality4_Click" class="btn mr-3 orangeBg text-white" />
                                                    <%--<a class="btn btn-light dropdown-toggle py-0" href="#" role="button" id="dropdownMenuLink" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                <span class="pr-5 mr-5">2.</span>
                                            </a>--%>
                                                    <asp:DropDownList ID="ddlSpeciality4" runat="server" class="btn btn-light dropdown-toggle py-0"></asp:DropDownList>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                    <%--<div class="row my-4">
                                <div class="col-md-4 offset-md-8">
                                    <div class="buttons ml-5">
                                        <p class="d-inline-block orangeColor robotoMed ml-2">
                                            <button type="button" class="btn mr-3 orangeBg text-white">+</button>Add Speciality
                                        </p>
                                    </div>
                                </div>
                            </div>--%>
                                    <!-- Speciality END -->
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <!-- Consultant BEGIN -->
                            <asp:UpdatePanel ID="updatePanelConsultantType" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <p class="robotoBold mt-4">Consultant</p>
                                    <div class="row my-2">
                                        <div class="col-md-4 offset-md-1 robotoReg">
                                            <%--<label class="containerRadio">
                                                N/A                                        
                                                <asp:RadioButton ID="rbConsultantNA" runat="server" GroupName="rbgrpConsultantAvail" AutoPostBack="true" Checked="true" />
                                                <span class="radioCheck"></span>
                                            </label>--%>
                                            <label class="containerRadio">
                                                Has Consultant                                        
                                                <asp:CheckBox ID="chbxAvailable" runat="server" AutoPostBack="true" />
                                                <span class="radioCheck"></span>
                                            </label>
                                            </div>
                                    </div>
                                    <asp:Repeater ID="rptrConsaltant" runat="server" OnItemDataBound="rptrConsaltant_ItemDataBound" OnItemCommand="rptrConsaltant_ItemCommand" >
                                    <ItemTemplate>                                        
                                    <%--<div class="row my-2">
                                        <div class="col-md-4 offset-md-1 robotoReg">                                            
                                            <label class="containerRadio">
                                                Individual                      
                                                <asp:RadioButton ID="rbConsultantIndiv" runat="server" GroupName="rbgrpConsultantType" AutoPostBack="true" Checked="true"/>
                                                <span class="radioCheck"></span>
                                            </label>
                                            <label class="containerRadio">
                                                Company \ Office                                        
                                                <asp:RadioButton ID="rbConsultantCompany" runat="server" GroupName="rbgrpConsultantType" AutoPostBack="true" />
                                                <span class="radioCheck"></span>
                                            </label>
                                        </div>
                                    </div>--%>
                                    <div id="consultantFirstDiv" class="row" runat="server" >
                                        <div class="col-md-9 ">
                                            <%--<p class="robotoReg d-inline-block mb-2 mr-1">Company \ Office Name</p>
                                    <input type="text" class="greyInput  ml-4 px-5 clinetInfoInput">--%>
                                            <asp:Label ID="lblConsultantOffice" class="robotoReg d-inline-block mb-2 mr-1" runat="server" >Company \ Office Name</asp:Label>
                                            <asp:TextBox ID="tbxConsultantOffice" runat="server" class="greyInput  ml-4 px-5 clinetInfoInput" ></asp:TextBox>
                                            <%--                                    <br>--%>
                                            <%--<p class="robotoReg d-inline-block mb-2 mr-4">Consultant Name</p>
                                    <input type="text" class="greyInput ml-5 px-5 clinetInfoInput">--%>
                                            <asp:Label ID="lblConsultantName" class="robotoReg d-inline-block mb-2 mr-4" runat="server" >Consultant Name <span>*</span></asp:Label>
                                            <asp:TextBox ID="tbxConsultantName" class="greyInput ml-5 px-5 clinetInfoInput" runat="server" ></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="reqFldVldConsltName" Enabled="false" runat="server" ControlToValidate="tbxConsultantName"
                                                    ValidationGroup="NewClientValidation" ErrorMessage="You must Enter the Consultant Name" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <br>
                                            <div class="row">
                                                 <div class="col-md-1 mr-5">
                                                    <p class="robotoReg">Specialilty</p>
                                                </div>
                                               <div class="col-md-3 px-2 dropdown d-inline-block">
                                                    
                                                    <asp:DropDownList ID="DDL_ConsultantSpecialty" runat="server"></asp:DropDownList>
                                                    
                                                   
                                                    <asp:Button ID="btnAddConsultantSpeciality" runat="server" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" Text="+" CommandName="AddConsultantSpeciality" />
                                                </div>
                                                <div class="col-md-4">
                                                    <p class="d-inline-block robotoReg">FOR</p>
                                                    <div class="dropdown d-inline-block">
                                                        <%--<a class="btn btn-light dropdown-toggle py-0" href="#" role="button" id="dropdownMenuLink" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                            <span class="pr-5 mr-2">Group</span>
                                                        </a>--%>
                                                        <asp:DropDownList CssClass="dropdown" ID="DDl_For" runat="server">
                                                            <asp:ListItem Value="Group" Text="Group"></asp:ListItem>
                                                            <asp:ListItem Value="Company" Text="Company"></asp:ListItem>
                                                            <asp:ListItem Value="Project" Text="Project"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        
                                                    </div>
                                                </div>
                                                 <div id="consultantSpeciality2" runat="server" visible="false">
                                                    <div class="col-md-1 mr-5">
                                                        <p class="robotoReg">Specialilty</p>
                                                    </div>
                                                    <div class="col-md-3 px-2">
                                                        
                                                        <asp:HiddenField ID="HDN_ConsultantSpecialtyID2" runat="server" />
                                                        <asp:DropDownList ID="DDL_ConsultantSpecialty2" runat="server"></asp:DropDownList>
                                                        <asp:Button ID="btnRemoveConsultantSpeciality" runat="server" Text="x" CommandName="RemoveConsultantSpeciality" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" />
                                                        <asp:Button ID="btnAddConsultantSpeciality2" runat="server" class="btn mr-3 darkBlue text-white robotoMed" Text="+" CommandName="AddConsultantSpeciality2" />
                                                        
                                                    </div>
                                                </div>
                                                <div id="consultantSpeciality3" runat="server" visible="false">
                                                    <div class="col-md-1 mr-5">
                                                        <p class="robotoReg">Specialilty</p>
                                                    </div>
                                                    <div class="col-md-3 px-2">
                                                        
                                                        <asp:HiddenField ID="HDN_ConsultantSpecialityID3" runat="server" />
                                                        <asp:DropDownList ID="DDL_ConsultantSpeciality3" runat="server"></asp:DropDownList>
                                                        <asp:Button ID="btnRemoveConsultantSpecialty3" runat="server" Text="x" CommandName="RemoveConsultantSpecialty3" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" />
                                                        
                                                        <%--<asp:Button ID="Button2" runat="server" class="btn darkBlue text-white robotoMed addEmail m-0 p-0" Text="+" OnClick="btnAddConsultantSpeciality2_Click" />--%>
                                                        
                                                    </div>
                                                </div>
                                                <div class="container">
                                                    <div class="row">
                                                        <div class="col-md-9">
                                                            <p class="robotoBold">Contacts </p>
                                                            <p class="robotoReg d-inline-block mb-2 mr-3">Email</p>
                                                            <%--<input type="text" class="greyInput ml-3 inputWidth2">--%>
                                                            <asp:TextBox ID="tbxConsultantEmail" class="greyInput ml-3 inputWidth2" runat="server"></asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="tbxConsultantEmail"
                                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="NewClientValidation"
                                                                ErrorMessage="Please enter vaild Email" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>
                                                            <asp:Button ID="btnAddConsultantEmail" CausesValidation="false" runat="server" Text="+" class="btn mr-3 darkBlue text-white robotoMed" CommandName="AddConsultantEmail" />


                                                            <div id="newConsultantEmail2" runat="server" visible="false">
                                                                <asp:Button ID="btnRemoveConsultantEmail" runat="server" Text="x" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" CommandName="RemoveConsultantEmail" />

                                                                <asp:TextBox ID="tbxConsultantEmail2" class="greyInput ml-3" runat="server"></asp:TextBox>
                                                                <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator12" runat="server" ControlToValidate="tbxConsultantEmail2"
                                                                    ValidationExpression="\d+" Enabled="false"
                                                                    ErrorMessage="Please enter numbers only" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>--%>
                                                                <asp:Button ID="btnAddConsultantEmail3" runat="server" Text="+" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" CommandName="AddConsultantEmail3" />

                                                            </div>

                                                            <div id="newConsultantEmail3" runat="server" visible="false">
                                                                <asp:Button ID="btnRemoveConsultantEmail3" runat="server" Text="x" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" CommandName="RemoveConsultantEmail3" />

                                                                <asp:TextBox ID="tbxConsultantEmail3" class="greyInput ml-3" runat="server"></asp:TextBox>
                                                                <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator12" runat="server" ControlToValidate="tbxConsultantEmail3"
                                                                    ValidationExpression="\d+" Enabled="false"
                                                                    ErrorMessage="Please enter numbers only" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>--%>
                                                            </div>

                                                            <br>
                                                            <p class="robotoReg d-inline-block mb-2 mr-1">Phone</p>
                                                            <%--<input type="text" class="greyInput ml-4">--%>
                                                            <asp:TextBox ID="tbxConsultantPhone" class="greyInput ml-4" runat="server"></asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="tbxConsultantPhone"
                                                                ValidationExpression="\d+" ValidationGroup="NewClientValidation"
                                                                ErrorMessage="Please enter numbers only" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>
                                                            <asp:Button ID="btnAddNewConsultantPhone" runat="server" Text="+" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" CommandName="AddNewConsultantPhone" />

                                                            <div id="newConsultantPhone2" runat="server" visible="false">
                                                                <asp:Button ID="btnRemoveConsultantPhone2" runat="server" Text="x" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" CommandName="RemoveConsultantPhone2"/>

                                                                <asp:TextBox ID="tbxConsultantPhone2" class="greyInput ml-3" runat="server"></asp:TextBox>
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator12" runat="server" ControlToValidate="tbxConsultantPhone2"
                                                                    ValidationExpression="\d+" ValidationGroup="NewClientValidation"
                                                                    ErrorMessage="Please enter numbers only" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>
                                                                <asp:Button ID="btnAddNewConsultantPhone2" runat="server" Text="+" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" CommandName="AddNewConsultantPhone2" />

                                                            </div>

                                                            <div id="newConsultantPhone3" runat="server" visible="false">
                                                                <asp:Button ID="btnRemoveConsultantPhone3" runat="server" Text="x" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" CommandName="RemoveConsultantPhone3" />

                                                                <asp:TextBox ID="tbxConsultantPhone3" class="greyInput ml-3" runat="server"></asp:TextBox>
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator13" runat="server" ControlToValidate="tbxConsultantPhone3"
                                                                    ValidationExpression="\d+" ValidationGroup="NewClientValidation"
                                                                    ErrorMessage="Please enter numbers only" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>

                                                            </div>



                                                            <br>
                                                            <p class="robotoReg d-inline-block mb-2 ">Mobile</p>
                                                            <asp:TextBox ID="tbxConsultantMobile" class="greyInput ml-4" runat="server"></asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="tbxConsultantMobile"
                                                                ValidationExpression="\d+" ValidationGroup="NewClientValidation"
                                                                ErrorMessage="Please enter numbers only" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>
                                                            <asp:Button ID="btnAddNewConsultantMobile" runat="server" Text="+" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" CommandName="AddNewConsultantMobile" />


                                                            <div id="newConsultantMobile2" runat="server" visible="false">
                                                                <asp:Button ID="btnRemoveConsultantMobile2" runat="server" Text="x" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" CommandName="RemoveConsultantMobile2" />

                                                                <asp:TextBox ID="tbxConsultantMobile2" class="greyInput ml-3" runat="server"></asp:TextBox>
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator14" runat="server" ControlToValidate="tbxConsultantMobile2"
                                                                    ValidationExpression="\d+" ValidationGroup="NewClientValidation"
                                                                    ErrorMessage="Please enter numbers only" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>
                                                                <asp:Button ID="btnAddNewConsultantMobile3" runat="server" Text="+" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" CommandName="AddNewConsultantMobile3" />

                                                            </div>
                                                            <div id="newConsultantMobile3" runat="server" visible="false">
                                                                <asp:Button ID="btnRemoveConsultantMobile3" runat="server" Text="x" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" CommandName="RemoveConsultantMobile3" />

                                                                <asp:TextBox ID="tbxConsultantMobile3" class="greyInput ml-3" runat="server"></asp:TextBox>
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator15" runat="server" ControlToValidate="tbxConsultantMobile3"
                                                                    ValidationExpression="\d+" ValidationGroup="NewClientValidation"
                                                                    ErrorMessage="Please enter numbers only" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>

                                                            </div>


                                                            <br>
                                                            <p class="robotoReg d-inline-block mb-2 ">Fax</p>
                                                            <%--                                                    <input type="text" class="greyInput ml-5">--%>
                                                            <asp:TextBox ID="tbxConsultantFax" class="greyInput ml-5" runat="server"></asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="tbxConsultantFax"
                                                                ValidationExpression="\d+" ValidationGroup="NewClientValidation"
                                                                ErrorMessage="Please enter numbers only" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>
                                                            <asp:Button ID="btnAddConsultantFax" runat="server" Text="+" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" CommandName="AddConsultantFax" />

                                                            <div id="newConsultantFax2" runat="server" visible="false">
                                                                <asp:Button ID="btnRemoveConsultantFax2" runat="server" Text="x" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" CommandName="RemoveConsultantFax2" />

                                                                <asp:TextBox ID="tbxConsultantFax2" class="greyInput ml-3" runat="server"></asp:TextBox>
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator16" runat="server" ControlToValidate="tbxConsultantFax2"
                                                                    ValidationExpression="\d+" ValidationGroup="NewClientValidation"
                                                                    ErrorMessage="Please enter numbers only" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>
                                                                <asp:Button ID="btnAddConsultantFax3" runat="server" Text="+" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" CommandName="AddConsultantFax3" />

                                                            </div>

                                                            <div id="newConsultantFax3" runat="server" visible="false">
                                                                <asp:Button ID="btnRemoveConsultantFax3" runat="server" Text="x" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" CommandName="RemoveConsultantFax3" />

                                                                <asp:TextBox ID="tbxConsultantFax3" class="greyInput ml-3" runat="server"></asp:TextBox>
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator17" runat="server" ControlToValidate="tbxConsultantFax3"
                                                                    ValidationExpression="\d+" ValidationGroup="NewClientValidation"
                                                                    ErrorMessage="Please enter numbers only" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>

                                                            </div>

                                                        </div>
                                                    </div>
                                                </div>

                                            </div>

                                        </div>

                                    </div>

                                    <div id="consultantSecondDiv" runat="server" >
                                        <p class="robotoBold ">Address </p>
                                        <div class="dropdown d-inline-block">
                                            <%--<a class="btn btn-light dropdown-toggle py-0" href="#" role="button" id="dropdownMenuLink" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    <span class="pr-5 mr-5">Country</span>
                                </a>--%>
                                            <asp:DropDownList ID="ddlConsultantCountry" class="btn btn-light dropdown-toggle py-0 form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                                            </asp:DropDownList>

                                            <div class="dropdown-menu" aria-labelledby="dropdownMenuLink">
                                                <a class="dropdown-item" href="#">Action</a>
                                                <a class="dropdown-item" href="#">Another action</a>
                                                <a class="dropdown-item" href="#">Something else here</a>
                                            </div>
                                        </div>
                                        <div class="dropdown d-inline-block">
                                            <%--<a class="btn btn-light dropdown-toggle py-0 " href="#" role="button" id="dropdownMenuLink" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    <span class="pr-5 mr-5">City</span>
                                </a>--%>
                                            <asp:DropDownList ID="ddlConsultantGovernorate" class="btn btn-light dropdown-toggle py-0 form-control" runat="server">
                                            </asp:DropDownList>

                                            <div class="dropdown-menu" aria-labelledby="dropdownMenuLink">
                                                <a class="dropdown-item" href="#">Action</a>
                                                <a class="dropdown-item" href="#">Another action</a>
                                                <a class="dropdown-item" href="#">Something else here</a>
                                            </div>
                                        </div>
                                        <br>
                                        <p class="robotoReg d-inline-block mt-2 mr-3 mb-0">Street</p>
                                        <%--<input type="text" class="greyInput ml-5 px-5 clinetInfoInput">--%>
                                        <asp:TextBox ID="tbxConsultantStreet" class="greyInput ml-5 px-5 clinetInfoInput" runat="server"></asp:TextBox>

                                        <br>
                                        <p class="robotoReg d-inline-block mt-2  mb-0">Building Number</p>
                                        <%--<input type="text" class="greyInput ml-3  ">--%>
                                        <asp:TextBox ID="tbxConsultantBuilding" class="greyInput ml-3  " runat="server"></asp:TextBox>
                                        <br>
                                        <p class="robotoReg d-inline-block mt-2 mr-5 mb-0">Floor</p>
                                        <%--<input type="text" class="greyInput ml-5">--%>
                                        <asp:TextBox ID="tbxConsultantFloor" class="greyInput ml-5" runat="server"></asp:TextBox>
                                        <br>
                                        <p class="robotoReg d-inline-block mt-2 mr-5 mb-0">Description</p>
                                        <br>
                                        <%--<textarea name="" id="" cols="100" rows="4"></textarea>--%>
                                        <asp:TextBox ID="tbxConsultantDescription" Columns="100" Rows="4" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        
                                    </div>
                                        <div class="row my-4">
                                            <div class="col-md-4 offset-md-8">
                                                <div class="buttons ml-5">
                                                    <p class="d-inline-block orangeColor robotoMed ml-2">
                                                        <asp:Button ID="btnRemoveConslt" runat="server" class="btn mr-3 orangeBg text-white" Text="-" CommandName="RemoveConsultant">
                                                        </asp:Button>Remove Consultant
                                                    </p>
                                                </div>
                                            </div>
                                        </div>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <%--<p class="robotoBold mt-4">Consultant</p>--%>
                                    <%--<div class="row my-2">
                                        <div class="col-md-4 offset-md-1 robotoReg">
                                            <label class="containerRadio">
                                                Individual
                                                <asp:RadioButton ID="rbConsultantIndiv" runat="server" GroupName="rbgrpConsultantType" AutoPostBack="true" Checked="true" />
                                                <span class="radioCheck"></span>
                                            </label>
                                            <label class="containerRadio">
                                                Company \ Office
                                                <asp:RadioButton ID="rbConsultantCompany" runat="server" GroupName="rbgrpConsultantType" AutoPostBack="true" />
                                                <span class="radioCheck"></span>
                                            </label>
                                        </div>
                                    </div>--%>
                                    <div id="consultantFirstDiv" class="row" runat="server" >
                                        <div class="col-md-9 ">
                                            <%--<p class="robotoReg d-inline-block mb-2 mr-1">Company \ Office Name</p>
                                    <input type="text" class="greyInput  ml-4 px-5 clinetInfoInput">--%>
                                            <asp:Label ID="lblConsultantOffice" class="robotoReg d-inline-block mb-2 mr-1" runat="server" >Company \ Office Name</asp:Label>
                                            <asp:TextBox ID="tbxConsultantOffice" runat="server" class="greyInput  ml-4 px-5 clinetInfoInput" ></asp:TextBox>
                                            <%--                                    <br>--%>
                                            <%--<p class="robotoReg d-inline-block mb-2 mr-4">Consultant Name</p>
                                    <input type="text" class="greyInput ml-5 px-5 clinetInfoInput">--%>
                                            <asp:Label ID="lblConsultantName" class="robotoReg d-inline-block mb-2 mr-4" runat="server" >Consultant Name<span>*</span></asp:Label>
                                            <asp:TextBox ID="tbxConsultantName" class="greyInput ml-5 px-5 clinetInfoInput" runat="server" ></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="reqFldVldConsltName" Enabled="false" runat="server" ControlToValidate="tbxConsultantName"
                                                    ValidationGroup="NewClientValidation" ErrorMessage="You must Enter the Consultant Name" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <br>
                                            <div class="row">
                                                 <div class="col-md-1 mr-5">
                                                    <p class="robotoReg">Specialilty</p>
                                                </div>
                                               <div class="col-md-3 px-2 dropdown d-inline-block">
                                                    
                                                    <asp:DropDownList ID="DDL_ConsultantSpecialty" runat="server"></asp:DropDownList>
                                                    
                                                   
                                                    <asp:Button ID="btnAddConsultantSpeciality" runat="server" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" Text="+" CommandName="AddConsultantSpeciality" />
                                                </div>
                                                <div class="col-md-4">
                                                    <p class="d-inline-block robotoReg">FOR</p>
                                                    <div class="dropdown d-inline-block">
                                                        <%--<a class="btn btn-light dropdown-toggle py-0" href="#" role="button" id="dropdownMenuLink" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                            <span class="pr-5 mr-2">Group</span>
                                                        </a>--%>
                                                        <asp:DropDownList CssClass="dropdown" ID="DDl_For" runat="server">
                                                            <asp:ListItem Value="Group" Text="Group"></asp:ListItem>
                                                            <asp:ListItem Value="Company" Text="Company"></asp:ListItem>
                                                            <asp:ListItem Value="Project" Text="Project"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        
                                                    </div>
                                                </div>
                                                 <div id="consultantSpeciality2" runat="server" visible="false">
                                                    <div class="col-md-1 mr-5">
                                                        <p class="robotoReg">Specialilty</p>
                                                    </div>
                                                    <div class="col-md-3 px-2">
                                                        
                                                        <asp:HiddenField ID="HDN_ConsultantSpecialtyID2" runat="server" />
                                                        <asp:DropDownList ID="DDL_ConsultantSpecialty2" runat="server"></asp:DropDownList>
                                                        <asp:Button ID="btnRemoveConsultantSpeciality" runat="server" Text="x" CommandName="RemoveConsultantSpeciality" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" />
                                                        <asp:Button ID="btnAddConsultantSpeciality2" runat="server" class="btn mr-3 darkBlue text-white robotoMed" Text="+" CommandName="AddConsultantSpeciality2" />
                                                        
                                                    </div>
                                                </div>
                                                <div id="consultantSpeciality3" runat="server" visible="false">
                                                    <div class="col-md-1 mr-5">
                                                        <p class="robotoReg">Specialilty</p>
                                                    </div>
                                                    <div class="col-md-3 px-2">
                                                        
                                                        <asp:HiddenField ID="HDN_ConsultantSpecialityID3" runat="server" />
                                                        <asp:DropDownList ID="DDL_ConsultantSpeciality3" runat="server"></asp:DropDownList>
                                                        <asp:Button ID="btnRemoveConsultantSpecialty3" runat="server" Text="x" CommandName="RemoveConsultantSpecialty3" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" />
                                                        
                                                        <%--<asp:Button ID="Button2" runat="server" class="btn darkBlue text-white robotoMed addEmail m-0 p-0" Text="+" OnClick="btnAddConsultantSpeciality2_Click" />--%>
                                                        
                                                    </div>
                                                </div>
                                                <div class="container">
                                                    <div class="row">
                                                        <div class="col-md-9">
                                                            <p class="robotoBold">Contacts </p>
                                                            <p class="robotoReg d-inline-block mb-2 mr-3">Email</p>
                                                            <%--<input type="text" class="greyInput ml-3 inputWidth2">--%>
                                                            <asp:TextBox ID="tbxConsultantEmail" class="greyInput ml-3 inputWidth2" runat="server"></asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="tbxConsultantEmail"
                                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="NewClientValidation"
                                                                ErrorMessage="Please enter vaild Email" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>
                                                            <asp:Button ID="btnAddConsultantEmail" CausesValidation="false" runat="server" Text="+" class="btn mr-3 darkBlue text-white robotoMed" CommandName="AddConsultantEmail" />


                                                            <div id="newConsultantEmail2" runat="server" visible="false">
                                                                <asp:Button ID="btnRemoveConsultantEmail" runat="server" Text="x" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" CommandName="RemoveConsultantEmail" />

                                                                <asp:TextBox ID="tbxConsultantEmail2" class="greyInput ml-3" runat="server"></asp:TextBox>
                                                                <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator12" runat="server" ControlToValidate="tbxConsultantEmail2"
                                                                    ValidationExpression="\d+" Enabled="false"
                                                                    ErrorMessage="Please enter numbers only" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>--%>
                                                                <asp:Button ID="btnAddConsultantEmail3" runat="server" Text="+" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" CommandName="AddConsultantEmail3" />

                                                            </div>

                                                            <div id="newConsultantEmail3" runat="server" visible="false">
                                                                <asp:Button ID="btnRemoveConsultantEmail3" runat="server" Text="x" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" CommandName="RemoveConsultantEmail3" />

                                                                <asp:TextBox ID="tbxConsultantEmail3" class="greyInput ml-3" runat="server"></asp:TextBox>
                                                                <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator12" runat="server" ControlToValidate="tbxConsultantEmail3"
                                                                    ValidationExpression="\d+" Enabled="false"
                                                                    ErrorMessage="Please enter numbers only" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>--%>
                                                            </div>

                                                            <br>
                                                            <p class="robotoReg d-inline-block mb-2 mr-1">Phone</p>
                                                            <%--<input type="text" class="greyInput ml-4">--%>
                                                            <asp:TextBox ID="tbxConsultantPhone" class="greyInput ml-4" runat="server"></asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="tbxConsultantPhone"
                                                                ValidationExpression="\d+" ValidationGroup="NewClientValidation"
                                                                ErrorMessage="Please enter numbers only" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>
                                                            <asp:Button ID="btnAddNewConsultantPhone" runat="server" Text="+" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" CommandName="AddNewConsultantPhone" />

                                                            <div id="newConsultantPhone2" runat="server" visible="false">
                                                                <asp:Button ID="btnRemoveConsultantPhone2" runat="server" Text="x" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" CommandName="RemoveConsultantPhone2" />

                                                                <asp:TextBox ID="tbxConsultantPhone2" class="greyInput ml-3" runat="server"></asp:TextBox>
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator12" runat="server" ControlToValidate="tbxConsultantPhone2"
                                                                    ValidationExpression="\d+" ValidationGroup="NewClientValidation"
                                                                    ErrorMessage="Please enter numbers only" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>
                                                                <asp:Button ID="btnAddNewConsultantPhone2" runat="server" Text="+" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" CommandName="AddNewConsultantPhone2" />

                                                            </div>

                                                            <div id="newConsultantPhone3" runat="server" visible="false">
                                                                <asp:Button ID="btnRemoveConsultantPhone3" runat="server" Text="x" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" CommandName="RemoveConsultantPhone3" />

                                                                <asp:TextBox ID="tbxConsultantPhone3" class="greyInput ml-3" runat="server"></asp:TextBox>
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator13" runat="server" ControlToValidate="tbxConsultantPhone3"
                                                                    ValidationExpression="\d+" ValidationGroup="NewClientValidation"
                                                                    ErrorMessage="Please enter numbers only" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>

                                                            </div>



                                                            <br>
                                                            <p class="robotoReg d-inline-block mb-2 ">Mobile</p>
                                                            <asp:TextBox ID="tbxConsultantMobile" class="greyInput ml-4" runat="server"></asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="tbxConsultantMobile"
                                                                ValidationExpression="\d+" ValidationGroup="NewClientValidation"
                                                                ErrorMessage="Please enter numbers only" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>
                                                            <asp:Button ID="btnAddNewConsultantMobile" runat="server" Text="+" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" CommandName="AddNewConsultantMobile" />


                                                            <div id="newConsultantMobile2" runat="server" visible="false">
                                                                <asp:Button ID="btnRemoveConsultantMobile2" runat="server" Text="x" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" CommandName="RemoveConsultantMobile2" />

                                                                <asp:TextBox ID="tbxConsultantMobile2" class="greyInput ml-3" runat="server"></asp:TextBox>
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator14" runat="server" ControlToValidate="tbxConsultantMobile2"
                                                                    ValidationExpression="\d+" ValidationGroup="NewClientValidation"
                                                                    ErrorMessage="Please enter numbers only" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>
                                                                <asp:Button ID="btnAddNewConsultantMobile3" runat="server" Text="+" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" CommandName="AddNewConsultantMobile3" />

                                                            </div>
                                                            <div id="newConsultantMobile3" runat="server" visible="false">
                                                                <asp:Button ID="btnRemoveConsultantMobile3" runat="server" Text="x" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" CommandName="RemoveConsultantMobile3" />

                                                                <asp:TextBox ID="tbxConsultantMobile3" class="greyInput ml-3" runat="server"></asp:TextBox>
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator15" runat="server" ControlToValidate="tbxConsultantMobile3"
                                                                    ValidationExpression="\d+" ValidationGroup="NewClientValidation"
                                                                    ErrorMessage="Please enter numbers only" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>

                                                            </div>


                                                            <br>
                                                            <p class="robotoReg d-inline-block mb-2 ">Fax</p>
                                                            <%--                                                    <input type="text" class="greyInput ml-5">--%>
                                                            <asp:TextBox ID="tbxConsultantFax" class="greyInput ml-5" runat="server"></asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="tbxConsultantFax"
                                                                ValidationExpression="\d+" ValidationGroup="NewClientValidation"
                                                                ErrorMessage="Please enter numbers only" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>
                                                            <asp:Button ID="btnAddConsultantFax" runat="server" Text="+" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" CommandName="AddConsultantFax" />

                                                            <div id="newConsultantFax2" runat="server" visible="false">
                                                                <asp:Button ID="btnRemoveConsultantFax2" runat="server" Text="x" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" CommandName="RemoveConsultantFax2" />

                                                                <asp:TextBox ID="tbxConsultantFax2" class="greyInput ml-3" runat="server"></asp:TextBox>
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator16" runat="server" ControlToValidate="tbxConsultantFax2"
                                                                    ValidationExpression="\d+" ValidationGroup="NewClientValidation"
                                                                    ErrorMessage="Please enter numbers only" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>
                                                                <asp:Button ID="btnAddConsultantFax3" runat="server" Text="+" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" CommandName="AddConsultantFax3" />

                                                            </div>

                                                            <div id="newConsultantFax3" runat="server" visible="false">
                                                                <asp:Button ID="btnRemoveConsultantFax3" runat="server" Text="x" CausesValidation="false" class="btn mr-3 darkBlue text-white robotoMed" CommandName="RemoveConsultantFax3" />

                                                                <asp:TextBox ID="tbxConsultantFax3" class="greyInput ml-3" runat="server"></asp:TextBox>
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator17" runat="server" ControlToValidate="tbxConsultantFax3"
                                                                    ValidationExpression="\d+" ValidationGroup="NewClientValidation"
                                                                    ErrorMessage="Please enter numbers only" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>

                                                            </div>

                                                        </div>
                                                    </div>
                                                </div>

                                            </div>

                                        </div>

                                    </div>

                                    <div id="consultantSecondDiv" runat="server" >
                                        <p class="robotoBold ">Address </p>
                                        <div class="dropdown d-inline-block">
                                            <%--<a class="btn btn-light dropdown-toggle py-0" href="#" role="button" id="dropdownMenuLink" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    <span class="pr-5 mr-5">Country</span>
                                </a>--%>
                                            <asp:DropDownList ID="ddlConsultantCountry" class="btn btn-light dropdown-toggle py-0 form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                                            </asp:DropDownList>

                                            <div class="dropdown-menu" aria-labelledby="dropdownMenuLink">
                                                <a class="dropdown-item" href="#">Action</a>
                                                <a class="dropdown-item" href="#">Another action</a>
                                                <a class="dropdown-item" href="#">Something else here</a>
                                            </div>
                                        </div>
                                        <div class="dropdown d-inline-block">
                                            <%--<a class="btn btn-light dropdown-toggle py-0 " href="#" role="button" id="dropdownMenuLink" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    <span class="pr-5 mr-5">City</span>
                                </a>--%>
                                            <asp:DropDownList ID="ddlConsultantGovernorate" class="btn btn-light dropdown-toggle py-0 form-control" runat="server">
                                            </asp:DropDownList>

                                            <div class="dropdown-menu" aria-labelledby="dropdownMenuLink">
                                                <a class="dropdown-item" href="#">Action</a>
                                                <a class="dropdown-item" href="#">Another action</a>
                                                <a class="dropdown-item" href="#">Something else here</a>
                                            </div>
                                        </div>
                                        <br>
                                        <p class="robotoReg d-inline-block mt-2 mr-3 mb-0">Street</p>
                                        <%--<input type="text" class="greyInput ml-5 px-5 clinetInfoInput">--%>
                                        <asp:TextBox ID="tbxConsultantStreet" class="greyInput ml-5 px-5 clinetInfoInput" runat="server"></asp:TextBox>

                                        <br>
                                        <p class="robotoReg d-inline-block mt-2  mb-0">Building Number</p>
                                        <%--<input type="text" class="greyInput ml-3  ">--%>
                                        <asp:TextBox ID="tbxConsultantBuilding" class="greyInput ml-3  " runat="server"></asp:TextBox>
                                        <br>
                                        <p class="robotoReg d-inline-block mt-2 mr-5 mb-0">Floor</p>
                                        <%--<input type="text" class="greyInput ml-5">--%>
                                        <asp:TextBox ID="tbxConsultantFloor" class="greyInput ml-5" runat="server"></asp:TextBox>
                                        <br>
                                        <p class="robotoReg d-inline-block mt-2 mr-5 mb-0">Description</p>
                                        <br>
                                        <%--<textarea name="" id="" cols="100" rows="4"></textarea>--%>
                                        <asp:TextBox ID="tbxConsultantDescription" Columns="100" Rows="4" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        
                                    </div>
                                            <div class="row my-4">
                                            <div class="col-md-4 offset-md-8">
                                                <div class="buttons ml-5">
                                                    <p class="d-inline-block orangeColor robotoMed ml-2">
                                                        <asp:Button ID="btnAddConslt" runat="server" class="btn mr-3 orangeBg text-white" Text="+" CommandName="AddConsultant">
                                                        </asp:Button>Add New Consultant
                                                    </p>
                                                </div>
                                            </div>
                                        </div>
                                        </FooterTemplate>
                                        </asp:Repeater>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <!-- Consultant END -->
                            <!-- Follow-up BEGIN -->
                            <p class="robotoBold mt-4">Follow-Up Period</p>
                            <div class="dropdown d-inline-block">
                                <%--<a class="btn btn-light dropdown-toggle py-0 " href="#" role="button" id="dropdownMenuLink" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    <span class="pr-5 mr-5">Months</span>
                                </a>--%>
                                <asp:DropDownList ID="ddlFollowUpPeriod" class="btn btn-light dropdown-toggle py-0 form-control" runat="server"></asp:DropDownList>

                                <div class="dropdown-menu" aria-labelledby="dropdownMenuLink">
                                    <a class="dropdown-item" href="#">Action</a>
                                    <a class="dropdown-item" href="#">Another action</a>
                                    <a class="dropdown-item" href="#">Something else here</a>
                                </div>
                            </div>
                            <label for="" class="robotoReg">Months</label>
                            <!-- Follow-up END -->
                            <!-- Assign to BEGIN -->
                            <p class="robotoBold mt-4">Assign To</p>
                            <label for="" class="robotoReg mr-3">Sales Man</label>
                            <div class="dropdown d-inline-block">
                                <%--<a class="btn btn-light dropdown-toggle py-0 " href="#" role="button" id="dropdownMenuLink" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                  <span class="pr-5 mr-5">Choose</span>
                              </a>--%>
                                <asp:DropDownList ID="ddlAssignedTo" class="btn btn-light dropdown-toggle py-0 form-control " runat="server"></asp:DropDownList>

                                <div class="dropdown-menu" aria-labelledby="dropdownMenuLink">
                                    <a class="dropdown-item" href="#">Action</a>
                                    <a class="dropdown-item" href="#">Another action</a>
                                    <a class="dropdown-item" href="#">Something else here</a>
                                </div>
                            </div>
                            <!-- Assign to END -->
                            <!-- Attachment BEGIN -->
                            <p class="robotoBold mt-4">Attachment :</p>
                            <div class="container">
                                <p class="robotoReg d-inline-block mb-2 robotoMed">- Business Card </p>
                                <asp:FileUpload ID="FileUpload1" runat="server" />
                                <%--<asp:LinkButton ID="lnkBusinessCard" runat="server" OnClick="lnkBusinessCard_Click" Text="Test" CssClass="btn mr-3 darkBlueBtn text-white robotoMed p-0">
                                </asp:LinkButton>
                                                                    <img src="/UI/Images/paper-clip.png">--%>

                                <%--<button type="button" class="btn mr-3 darkBlueBtn text-white robotoMed p-0">
                                    <img src="/UI/Images/paper-clip.png"></button>--%>
                                <br>
                                <p class="robotoReg d-inline-block mb-2 robotoMed">- Brochure </p>
                                <asp:FileUpload ID="FileUpload2" runat="server" />
                                <%--<button type="button" class="btn mr-3 darkBlueBtn text-white robotoMed p-0">
                                    <img src="/UI/Images/paper-clip.png"></button>--%>
                                <br>
                                <p class="robotoReg d-inline-block mb-2 robotoMed">- Other </p>
                                <asp:FileUpload ID="FileUpload3" runat="server" />
                                <%--<button type="button" class="btn mr-3 darkBlueBtn text-white robotoMed p-0">
                                    <img src="/UI/Images/paper-clip.png"></button>--%>
                                <br>
                            </div>
                            <!-- Attachment END -->
                            <!-- General Notes BEGIN -->
                            <div class="row mt-5">
                                <div class="col-md-3">
                                    <h5 class="d-inline-block orangeColor">Add General Notes :</h5>
                                </div>
                                <div class="col-md-9 mt-4">
                                    <asp:TextBox ID="tbxNote" runat="server" Rows="5" Columns="50" placeholder="Write Your Text Here" class="p-3 w-100" TextMode="MultiLine"></asp:TextBox>
                                    <%--                                    <textarea name="text" id="" cols="50" rows="5" placeholder="Write Your Text Here" class="p-3 w-100"></textarea>--%>
                                </div>
                            </div>
                            <!-- General Notes END -->

                            <!-- Save Button BEGIN -->
                            <div class="d-flex justify-content-end mt-3">
                                <%--<button class="btn text-white save px-4 py-1 robotoBold primaryColorBlue">Save</button>--%>
                                <asp:Button ID="btnSave" class="btn text-white save px-4 py-1 robotoBold primaryColorBlue" CausesValidation="true" ValidationGroup="NewClientValidation" runat="server" Text="Save" OnClick="btnSave_Click" />
                            </div>
                            <!-- Save Button End -->

                        </div>
                        <div id="noAccess" runat="server" visible="false">
                            <p>Sorry, You cannot do actions in this Page</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <%--<script src="node_modules/jquery/dist/jquery.min.js"></script>
<script src="node_modules/bootstrap/dist/js/bootstrap.bundle.js"></script>--%>
</asp:Content>
