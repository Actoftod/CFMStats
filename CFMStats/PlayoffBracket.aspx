<%@ Page Title="Playoff Bracket" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PlayoffBracket.aspx.cs" Inherits="CFMStats.PlayoffBracket" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        .playoff-bracket {
            display: flex;
            flex-direction: column;
            gap: 30px;
        }

        .conference-bracket {
            border: 2px solid #ddd;
            padding: 20px;
            border-radius: 10px;
            background-color: #f8f9fa;
        }

        .conference-title {
            text-align: center;
            font-size: 24px;
            font-weight: bold;
            margin-bottom: 20px;
            color: #333;
        }

        .bracket-rounds {
            display: flex;
            justify-content: space-around;
            gap: 20px;
        }

        .round {
            display: flex;
            flex-direction: column;
            gap: 20px;
            min-width: 200px;
        }

        .round-title {
            text-align: center;
            font-weight: bold;
            font-size: 16px;
            margin-bottom: 10px;
            color: #555;
        }

        .matchup {
            border: 1px solid #ccc;
            border-radius: 5px;
            padding: 10px;
            background-color: white;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }

        .team {
            display: flex;
            justify-content: space-between;
            padding: 8px;
            border-bottom: 1px solid #eee;
        }

        .team:last-child {
            border-bottom: none;
        }

        .team.winner {
            background-color: #d4edda;
            font-weight: bold;
        }

        .team-name {
            flex: 1;
        }

        .team-score {
            font-weight: bold;
            margin-left: 10px;
        }

        .super-bowl {
            text-align: center;
            margin-top: 30px;
        }

        .super-bowl .matchup {
            display: inline-block;
            min-width: 300px;
        }
    </style>

    <div class="row">
        <div class="col-sm-12">
            <h2>Playoff Bracket - Season <asp:Label ID="lblSeasonIndex" runat="server"></asp:Label></h2>
        </div>
    </div>

    <div class="row">
        <div class="col-sm-6">
            <div class="input-group input-group-sm mb-3">
                <span class="input-group-text bg-secondary">Season</span>
                <asp:DropDownList ID="ddlSeasonSelector" runat="server" CssClass="form-control form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlSeasonSelector_OnSelectedIndexChanged">
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

            <div class="playoff-bracket">
                <asp:PlaceHolder ID="phBracketHolder" runat="server"></asp:PlaceHolder>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlSeasonSelector" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>
