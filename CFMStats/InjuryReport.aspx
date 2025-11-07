<%@ Page Title="Injury Report" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InjuryReport.aspx.cs" Inherits="CFMStats.InjuryReport" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-sm-12">
            <h2>Injury Report</h2>
        </div>
    </div>

    <div class="row">
        <div class="col-sm-6">
            <div class="input-group input-group-sm mb-3">
                <span class="input-group-text bg-secondary">Team</span>
                <asp:DropDownList ID="ddlTeamSelector" runat="server" CssClass="form-control form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlTeamSelector_OnSelectedIndexChanged">
                    <asp:ListItem Text="All Teams" Value="0"></asp:ListItem>
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
                <asp:Literal ID="litInjuryReport" runat="server"></asp:Literal>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlTeamSelector" EventName="SelectedIndexChanged" />
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
