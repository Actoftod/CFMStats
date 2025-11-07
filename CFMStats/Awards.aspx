<%@ Page Title="Awards" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Awards.aspx.cs" Inherits="CFMStats.Awards" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-sm-12">
            <h2>League Awards</h2>
        </div>
    </div>

    <div class="row">
        <div class="col-sm-6">
            <div class="input-group input-group-sm mb-3">
                <span class="input-group-text bg-secondary">Season</span>
                <asp:DropDownList ID="ddlSeasonSelector" runat="server" CssClass="form-control form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlSeasonSelector_OnSelectedIndexChanged">
                    <asp:ListItem Text="All Seasons" Value="0"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-sm-6">
            <div class="input-group input-group-sm mb-3">
                <span class="input-group-text bg-secondary">Award Type</span>
                <asp:DropDownList ID="ddlAwardType" runat="server" CssClass="form-control form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlAwardType_OnSelectedIndexChanged">
                    <asp:ListItem Text="All Awards" Value=""></asp:ListItem>
                    <asp:ListItem Text="MVP" Value="MVP"></asp:ListItem>
                    <asp:ListItem Text="Offensive Player of the Year" Value="OPOY"></asp:ListItem>
                    <asp:ListItem Text="Defensive Player of the Year" Value="DPOY"></asp:ListItem>
                    <asp:ListItem Text="Offensive Rookie of the Year" Value="OROY"></asp:ListItem>
                    <asp:ListItem Text="Defensive Rookie of the Year" Value="DROY"></asp:ListItem>
                    <asp:ListItem Text="Pro Bowl" Value="ProBowl"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
    </div>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                <ProgressTemplate>
                    <div style="text-align: center;">
                        <label class="badge bg-warning">... LOADING ...</label>
                        <label class="badge bg-danger">... LOADING ...</label>
                        <label class="badge bg-success">... LOADING ...</label><br />
                    </div>
                    <br />
                </ProgressTemplate>
            </asp:UpdateProgress>

            <div class="table-responsive">
                <asp:Literal ID="litAwards" runat="server"></asp:Literal>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlSeasonSelector" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlAwardType" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>

    <%-- tablesorter --%>
    <link href="Content/tablesorter/theme.ice.min.css" rel="stylesheet" />
    <script src="Scripts/jquery.tablesorter.min.js"></script>

    <script>

        //Initial bind
        $(document).ready(function () {
            BindControlEvents();
        });

        //Re-bind for callbacks
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_endRequest(function () {
            BindControlEvents();
        });

        function BindControlEvents() {
            $('table').tablesorter({
                theme: 'ice',
                widthFixed: true
            });
        }

    </script>

</asp:Content>
