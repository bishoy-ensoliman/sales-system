<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/GarasERP.Master" AutoEventWireup="true" CodeBehind="MyClients.aspx.cs" Inherits="GarasSales.Sales.MyClients" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>My Clients</title>
        <link rel="stylesheet" href="/UI/CSS/pag.css">
    <style type="text/css">
        body
        {
            margin: 0;
            padding: 0;
            font-family: Arial;
        }
        .modal1
        {
            position: fixed;
            z-index: 999;
            height: 100%;
            width: 100%;
            top: 0;
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
            -moz-opacity: 0.8;
        }
        .center1
        {
            z-index: 1000;
            margin: 300px auto;
            padding: 10px;
            width: 130px;
            background-color: White;
            border-radius: 10px;
            filter: alpha(opacity=100);
            opacity: 1;
            -moz-opacity: 1;
        }
        .center1 img
        {
            height: 128px;
            width: 128px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
     <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="clientListUpdatePanel">
                <ProgressTemplate>
                    <div class="modal1">
                        <div class="center1">
                            <img alt="" src="../../UI/Images/fancybox_loading2x.gif" />
                        </div>
                    </div>
                </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="clientListUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="container mt-5 mb-5">
                <div class="row upperCard">
                    <div class="col">
                        <div class="outerDiv">
                            <div class="innerDiv pb-5">
                                <div class="d-flex justify-content-between">
                                    <div class="">
                                        <%--<img src="/UI/Images/man-user.png">--%>
                                        <h3 class="mt-4 ml-5">MY CLIENT LIST</h3>
                                        <p class="clientsDiv px-4 ml-5 text-center">
                                            <asp:Label ID="lblClientCount" class="clientsDiv mt-4 mb-0 px-4" runat="server"></asp:Label>
                                        </p>
                                    </div>
                                    <%--<div>
                                        <asp:HyperLink ID="hyprLnk" Visible="false" runat="server" Text="Add New Client" NavigateUrl="~/Sales/Client/NewClient.aspx"></asp:HyperLink>
                                    </div>--%>
                                    <div class="headerTarget mt-4">
                                        <asp:TextBox ID="tbxFilter" class="greyInput px-3" runat="server" placeholder="Search"></asp:TextBox>
                                        <%--                                <button type="button" name="button" class="btn edit px-4 py-0">FILTER<img src="/UI/Images/exchange.png"></button>--%>
                                        <asp:Button ID="btnFilter" runat="server" CssClass="btn edit px-4 py-0" Text="Filter" OnClick="btnFilter_Click"  />
                                      <%--  <button type="button" name="button" class="btn edit px-4 py-0" onclick="document.getElementById('<%= btnFilter.ClientID  %>').click();">
                                            Filter
                                            <img src="/UI/Images/exchange.png">
                                        </button>--%>
                                        <%--                                <button type="button" name="button" class="btn edit px-4 py-0">SORT<img src="/UI/Images/exchange.png"></button>--%>
                                        <asp:Button ID="btnSort" CssClass="btn edit px-4 py-0" runat="server" Text="Sort" OnClick="btnSort_Click"  /><%--<img src="/UI/Images/exchange.png">--%>
                                        <%--<button type="button" name="button" class="btn edit px-4 py-0" onclick="document.getElementById('<%= btnSort.ClientID  %>').click();">
                                            Sort
                    <img src="/UI/Images/exchange.png">
                                        </button>--%>
                                        <button id="btnAddClient" runat="server" type="button" name="button" class="btn orangeBg text-white shadow-sm edit px-4 py-0" onclick="window.location.href='/Sales/Client/NewClient.aspx';return false;">Add Client</button>
                                        <%--                                 <button type="button" name="button" class="btn edit px-4 py-0">VIEW<img src="/UI/Images/exchange.png"></button>--%>
                                    </div>
                                </div>

                                <div class="w-75 mx-auto">
                                    <div class="row">

                                        <asp:Repeater ID="rptrClientList" runat="server" OnItemDataBound="rptrClientList_ItemDataBound">
                                            <ItemTemplate>

                                                <div class="col-lg-3 col-md-4 col-sm-6 mt-5 d-flex  align-items-stretch">
                                                    <div class="clinetCard text-center py-5 px-3">


                                                        <asp:Image ID="imgClient"  runat="server" />
                                                        <asp:Label ID="lblClientName" Text='<%# Eval("Name") %>' class="d-inline-block" runat="server" Style="word-wrap: normal; word-break: break-all;">Company Name</asp:Label>
                                                        <asp:HyperLink ID="lnkView" class="btn view px-4 py-0 text-white" runat="server" Text="View Profile"></asp:HyperLink>


                                                    </div>
                                                </div>

                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                     <%--<div class="pagination d-flex justify-content-center mt-5"></div>--%>
                                    <div style="margin-top: 20px;">
                                        <table style="width: 600px;">
                                            <tr>
                                                <td>
                                                    <asp:LinkButton ID="lbFirst" runat="server" OnClick="lbFirst_Click">First</asp:LinkButton>
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="lbPrevious" runat="server" OnClick="lbPrevious_Click">Previous</asp:LinkButton>
                                                </td>
                                                <td>
                                                    <asp:DataList ID="rptPaging" runat="server" OnItemCommand="rptPaging_ItemCommand"
                                                        OnItemDataBound="rptPaging_ItemDataBound" RepeatDirection="Horizontal">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>' 
                                                                CommandName="newPage" Text='<%# Eval("PageText") %> ' Width="20px">
						                                    </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:DataList>
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="lbNext" runat="server" OnClick="lbNext_Click">Next</asp:LinkButton>
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="lbLast" runat="server" OnClick="lbLast_Click">Last</asp:LinkButton>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblpage" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                        </table>

                                    </div>
                                </div>
                            </div>

                            <div id="noClients" runat="server" visible="false">
                                <asp:Label ID="lblNoClients" runat="server" Text="No Clients Available"></asp:Label>
                            </div>


                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
      <script src="/UI/JavaScript/paginate.js"></script>
<script src="/UI/JavaScript/custom.js"></script>
</asp:Content>
