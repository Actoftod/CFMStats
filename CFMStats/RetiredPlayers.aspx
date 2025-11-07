<%@ Page Title="Retired Players Hall of Fame" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RetiredPlayers.aspx.cs" Inherits="CFMStats.RetiredPlayers" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-sm-12">
            <h2>Retired Players Hall of Fame</h2>
            <p>Honoring the legends who hung up their cleats</p>
        </div>
    </div>

    <div class="table-responsive">
        <asp:Literal ID="litRetiredPlayers" runat="server"></asp:Literal>
    </div>

    <%-- tablesorter --%>
    <link href="Content/tablesorter/theme.ice.min.css" rel="stylesheet" />
    <script src="Scripts/jquery.tablesorter.min.js"></script>

    <script>

        $(document).ready(function () {
            $('table').tablesorter({
                theme: 'ice',
                widthFixed: true
            });
        });

    </script>

</asp:Content>
