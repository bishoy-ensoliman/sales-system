<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/GarasERP.Master" AutoEventWireup="true" CodeBehind="TargetDistribution.aspx.cs" Inherits="GarasSales.Sales.Target.TargetDistribution" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div id="divPage" runat="server" visible="false">
        <asp:UpdatePanel ID="updPnlTargetDist" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="container mt-5">
                    <div class="row upperCard">
                        <div class="col">
                            <div class="outerDiv">
                                <div class="innerDiv">
                                    <div class="contaienr">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="d-flex justify-content-between">
                                                    <div>
                                                        <p class="d-inline-block mr-4 target">Sales Target Distribution for</p>
                                                        <div class="dropdown d-inline-block">
                                                            <asp:DropDownList class="btn dropdown-toggle px-3 py-1" ID="ddlTargetYear" runat="server" OnSelectedIndexChanged="ddlTargetYear_SelectedIndexChanged" AutoPostBack="true">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                     <asp:Button ID="btnRedistributeTarget" runat="server" class="btn edit ml-2 primaryColorBlue text-white robotoMed shadow-sm" Text="Redistribute Target" OnClick="btnRedistributeTarget_Click" />
                                                     </div>
                                                    <div>
                                                        <p class="d-inline-block mr-4 target">Branch Target </p>
                                                        <div class="color d-inline-block px-4">
                                                            <strong>
                                                                <asp:Label ID="lblBranchTarget" runat="server"></asp:Label>
                                                            </strong>
                                                        </div>
                                                       
                                                   
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>










                <div class="container targetDistrbution mt-5 mb-5">
                    <div class="row secondCard">
                        <%--upperCard--%>
                        <div class="col">
                            <div class="outerDiv">
                                <div class="innerDiv">
                                    <h5 class="d-inline-block mr-5">TARGET DISTRIBUTION</h5>
                                    <label class="checkContainer">
                                        <asp:RadioButton GroupName="distType" ID="chbxEqual" runat="server" Checked="true" OnCheckedChanged="chbxEqual_CheckedChanged" AutoPostBack="true" />
                                        <span class="checkmark"></span>
                                    </label>
                                    <p class="d-inline-block mr-5">EQUAL</p>
                                    <label class="checkContainer">
                                        <asp:RadioButton GroupName="distType" ID="chbxVar" runat="server" OnCheckedChanged="chbxVar_CheckedChanged" AutoPostBack="true" />
                                        <span class="checkmark"></span>
                                    </label>
                                    <p class="d-inline-block">VARIABLE</p>
                                    <div class="row">
                                        <div class="col-md-2 offset-md-1 text-center">
                                            <asp:Button ID="btnSortName" runat="server" OnClick="btnSortName_Click" Style="display: none;" />
                                            <button class="btn edit px-4 py-0 headerTarget" type="button" onclick="document.getElementById('<%= btnSortName.ClientID  %>').click();">
                                                NAME
                                            <img src="/UI/Images/exchange.png"></button>
                                            </button>
                                        </div>
                                        <div class="col-md-2 text-center headerTarget">
                                            <asp:Button ID="btnSortYear" runat="server" OnClick="btnSortYear_Click" Style="display: none;" />
                                            <button type="button" name="button" class="btn edit px-4 py-0" onclick="document.getElementById('<%= btnSortYear.ClientID  %>').click();">
                                                <asp:Label ID="lblLastYear" runat="server"></asp:Label>
                                                <img src="/UI/Images/exchange.png"></button>
                                        </div>
                                        <div class="col-md-3 text-center headerTarget">
                                            <asp:Button ID="btnSortAvgYears" runat="server" OnClick="btnSortAvgYears_Click" Style="display: none;" />
                                            <button type="button" name="button" class="btn edit px-4 py-0" onclick="document.getElementById('<%= btnSortAvgYears.ClientID  %>').click();">
                                                AVG.LAST 5 YEARS<img src="/UI/Images/exchange.png"></button>
                                        </div>
                                        <div class="col-md-2 text-center headerTarget">
                                            <asp:Button ID="btnSortCommit" runat="server" OnClick="btnSortCommit_Click" Style="display: none;" />
                                            <button type="button" name="button" class="btn edit px-3 py-0" onclick="document.getElementById('<%= btnSortCommit.ClientID  %>').click();">
                                                COMMITMENT<img src="/UI/Images/exchange.png"></button>
                                        </div>
                                        <div class="col-md-2 text-center headerTarget">
                                            <asp:Button ID="btnSortTarget" runat="server" OnClick="btnSortTarget_Click" Style="display: none;" />
                                            <button type="button" name="button" class="btn edit px-3 py-0" onclick="document.getElementById('<%= btnSortTarget.ClientID  %>').click();">
                                                NEW TARGET<img src="/UI/Images/exchange.png"></button>
                                        </div>
                                    </div>
                                    <div class="mt-3">
                                        <asp:Repeater ID="rptrSalesMen" runat="server" OnItemDataBound="rptrSalesMen_ItemDataBound">
                                            <ItemTemplate>
                                                <div class="row text-center">
                                                    <div class="col-md-1">
                                                        <label class="checkContainer">
                                                            <asp:CheckBox ID="chbxSalesMan" runat="server" OnCheckedChanged="chbxSalesMan_CheckedChanged" AutoPostBack="true" />
                                                            <span class="checkmark"></span>
                                                        </label>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <p>
                                                            <asp:Label ID="lblSalesManName" runat="server"></asp:Label>
                                                        </p>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <p>
                                                            <asp:Label ID="lblLastYearTarget" runat="server"></asp:Label>
                                                        </p>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <p>
                                                            <asp:Label ID="lblAvg5Years" runat="server"></asp:Label>
                                                        </p>
                                                    </div>
                                                    <div class="col-md-2"></div>
                                                    <div class="col-md-2 text-center">
                                                        <asp:TextBox class="dottedDiv" ID="tbxNewTarget" runat="server" OnTextChanged="tbxNewTarget_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="ReqFldSalesManTarget" runat="server"
                                                            ControlToValidate="tbxNewTarget"
                                                            ErrorMessage="Sales Target is required."
                                                            ForeColor="Red"
                                                            ValidationGroup="SalesManTargetCreate">
                                                        </asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                    <div class="row mt-5">
                                        <div class="col-md-3">
                                            <h5 class="d-inline-block ml-5">Add General Notes :</h5>
                                        </div>
                                        <div class="col-md-9 mt-4">
                                            <asp:TextBox ID="txtAreaComment" class="form-control border-0 p-5" 
                                                TextMode="multiline" Columns="50" Rows="3" runat="server" 
                                                placeholder="Write your text here"/>
                                        </div>
                                    </div>
                                    <div class="d-flex justify-content-end mt-3">
                                        <asp:Button ID="btnSave" runat="server" ValidationGroup="SalesManTargetCreate" class="btn create orangeBg text-white edit px-4 py-1" Text="Save" OnClick="btnSave_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
