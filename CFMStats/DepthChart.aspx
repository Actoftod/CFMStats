<%@ Page Title="Depth Chart" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DepthChart.aspx.cs" Inherits="CFMStats.DepthChart" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        .depth-chart-container {
            margin: 20px 0;
        }
        .position-group {
            margin-bottom: 30px;
            border: 1px solid #ddd;
            border-radius: 5px;
            padding: 15px;
            background-color: #f8f9fa;
        }
        .position-title {
            font-weight: bold;
            font-size: 18px;
            margin-bottom: 15px;
            color: #333;
        }
        .depth-player {
            padding: 10px;
            margin: 5px 0;
            background-color: white;
            border-left: 4px solid #007bff;
            border-radius: 3px;
        }
        .depth-player.starter {
            border-left-color: #28a745;
            font-weight: bold;
        }
        .depth-player.backup {
            border-left-color: #ffc107;
        }
        .depth-number {
            display: inline-block;
            width: 30px;
            text-align: center;
            font-weight: bold;
            color: #666;
        }
    </style>

    <div class="row">
        <div class="col-sm-12">
            <h2>Team Depth Chart</h2>
        </div>
    </div>

    <div class="row">
        <div class="col-sm-6">
            <div class="input-group input-group-sm mb-3">
                <span class="input-group-text bg-secondary">Team</span>
                <asp:DropDownList ID="ddlTeamSelector" runat="server" CssClass="form-control form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlTeamSelector_OnSelectedIndexChanged">
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

            <div class="depth-chart-container">
                <asp:Literal ID="litDepthChart" runat="server"></asp:Literal>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlTeamSelector" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>
