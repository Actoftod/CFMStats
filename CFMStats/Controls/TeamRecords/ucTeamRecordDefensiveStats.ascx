<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucTeamRecordDefensiveStats.ascx.cs" Inherits="CFMStats.Controls.TeamRecords.ucTeamRecordDefensiveStats" %>


<div class="row">
    <div class="col-sm-6">
        <strong>Most Tackles</strong>
        <div class="table-responsive table-bordered-curved">
            <div id="tableTackles" runat="server" visible="true"></div>
        </div>
    </div>
    <div class="col-sm-6">
        <strong>Most Sacks</strong>
        <div class="table-responsive table-bordered-curved">
            <div id="tableSacks" runat="server" visible="true"></div>
        </div>
    </div>
</div>

<div class="hidden-xs">
    <br />
</div>

<div class="row">
    <div class="col-sm-6">
        <strong>Most Interceptions</strong>
        <div class="table-responsive table-bordered-curved">
            <div id="tableInterceptions" runat="server" visible="true"></div>
        </div>
    </div>
    <div class="col-sm-6">
        <strong>Most Defensive Touchdowns</strong>
        <div class="table-responsive table-bordered-curved">
            <div id="tableTouchdowns" runat="server" visible="true"></div>
        </div>
    </div>
</div>
