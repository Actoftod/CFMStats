using System;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using CFMStats.Classes;

namespace CFMStats
{
    public partial class PlayerComparison : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Helper.StringNull(Request.QueryString["leagueId"]).Length == 0)
            {
                Response.Redirect("~/");
            }

            Session["leagueId"] = Helper.StringNull(Request.QueryString["leagueId"]);

            if (!IsPostBack)
            {
                LoadPlayers();
            }
        }

        protected void ddlPlayers_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            ComparePlayer();
        }

        private void LoadPlayers()
        {
            var sp = new StoredProc
            {
                Name = "Player_select",
                DataConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString,
                ParameterSet = new System.Data.SqlClient.SqlCommand()
            };

            sp.ParameterSet.Parameters.AddWithValue("@leagueId", Helper.IntegerNull(Session["leagueId"]));

            var ds = StoredProc.ShowMeTheData(sp);

            ddlPlayer1.Items.Add(new ListItem("Select Player...", "0"));
            ddlPlayer2.Items.Add(new ListItem("Select Player...", "0"));

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var playerName = $"{row["firstName"]} {row["lastName"]} ({row["position"]}) - {row["teamName"]}";
                var playerId = row["playerId"].ToString();

                ddlPlayer1.Items.Add(new ListItem(playerName, playerId));
                ddlPlayer2.Items.Add(new ListItem(playerName, playerId));
            }
        }

        private void ComparePlayer()
        {
            var player1Id = int.Parse(ddlPlayer1.SelectedValue);
            var player2Id = int.Parse(ddlPlayer2.SelectedValue);

            if (player1Id == 0 || player2Id == 0)
            {
                litComparison.Text = "<div class='alert alert-info'>Please select two players to compare</div>";
                return;
            }

            var player1Data = GetPlayerData(player1Id);
            var player2Data = GetPlayerData(player2Id);

            if (player1Data == null || player2Data == null)
            {
                litComparison.Text = "<div class='alert alert-danger'>Error loading player data</div>";
                return;
            }

            BuildComparisonTable(player1Data, player2Data);
        }

        private DataRow GetPlayerData(int playerId)
        {
            var sp = new StoredProc
            {
                Name = "GetPlayerRatings",
                DataConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString,
                ParameterSet = new System.Data.SqlClient.SqlCommand()
            };

            sp.ParameterSet.Parameters.AddWithValue("@playerId", playerId);

            var ds = StoredProc.ShowMeTheData(sp);

            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0];
            }

            return null;
        }

        private void BuildComparisonTable(DataRow player1, DataRow player2)
        {
            var sb = new StringBuilder();

            sb.Append("<div class='row'>");
            sb.Append("<div class='col-md-12'>");
            sb.Append("<table class='table table-bordered table-striped'>");

            // Header
            sb.Append("<thead class='table-dark'>");
            sb.Append("<tr>");
            sb.Append("<th>Attribute</th>");
            sb.Append($"<th class='text-center'>{player1["firstName"]} {player1["lastName"]}</th>");
            sb.Append($"<th class='text-center'>{player2["firstName"]} {player2["lastName"]}</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");

            sb.Append("<tbody>");

            // Basic Info
            AddComparisonRow(sb, "Position", player1["position"], player2["position"]);
            AddComparisonRow(sb, "Team", player1["teamName"], player2["teamName"]);
            AddComparisonRow(sb, "Overall Rating", player1["overallRating"], player2["overallRating"], true);
            AddComparisonRow(sb, "Age", player1["age"], player2["age"]);
            AddComparisonRow(sb, "Years Pro", player1["yearsPro"], player2["yearsPro"]);
            AddComparisonRow(sb, "Height", player1["height"], player2["height"]);
            AddComparisonRow(sb, "Weight", player1["weight"], player2["weight"]);

            // Key Ratings
            sb.Append("<tr class='table-secondary'><td colspan='3'><strong>Key Ratings</strong></td></tr>");
            AddComparisonRow(sb, "Speed", player1["speedRating"], player2["speedRating"], true);
            AddComparisonRow(sb, "Acceleration", player1["accelerationRating"], player2["accelerationRating"], true);
            AddComparisonRow(sb, "Agility", player1["agilityRating"], player2["agilityRating"], true);
            AddComparisonRow(sb, "Strength", player1["strengthRating"], player2["strengthRating"], true);
            AddComparisonRow(sb, "Awareness", player1["awarenessRating"], player2["awarenessRating"], true);
            AddComparisonRow(sb, "Stamina", player1["staminaRating"], player2["staminaRating"], true);
            AddComparisonRow(sb, "Injury", player1["injuryRating"], player2["injuryRating"], true);

            // Position-specific ratings
            var pos1 = player1["position"].ToString();
            var pos2 = player2["position"].ToString();

            if (pos1 == "QB" || pos2 == "QB")
            {
                sb.Append("<tr class='table-secondary'><td colspan='3'><strong>Passing Ratings</strong></td></tr>");
                AddComparisonRow(sb, "Throw Power", player1["throwPowerRating"], player2["throwPowerRating"], true);
                AddComparisonRow(sb, "Throw Accuracy Short", player1["throwAccuracyShortRating"], player2["throwAccuracyShortRating"], true);
                AddComparisonRow(sb, "Throw Accuracy Mid", player1["throwAccuracyMidRating"], player2["throwAccuracyMidRating"], true);
                AddComparisonRow(sb, "Throw Accuracy Deep", player1["throwAccuracyDeepRating"], player2["throwAccuracyDeepRating"], true);
            }

            if (pos1 == "HB" || pos2 == "HB" || pos1 == "FB" || pos2 == "FB")
            {
                sb.Append("<tr class='table-secondary'><td colspan='3'><strong>Rushing Ratings</strong></td></tr>");
                AddComparisonRow(sb, "Carrying", player1["carryingRating"], player2["carryingRating"], true);
                AddComparisonRow(sb, "Break Tackle", player1["breakTackleRating"], player2["breakTackleRating"], true);
                AddComparisonRow(sb, "Trucking", player1["truckingRating"], player2["truckingRating"], true);
                AddComparisonRow(sb, "Elusiveness", player1["elusivenessRating"], player2["elusivenessRating"], true);
            }

            if (pos1 == "WR" || pos2 == "WR" || pos1 == "TE" || pos2 == "TE")
            {
                sb.Append("<tr class='table-secondary'><td colspan='3'><strong>Receiving Ratings</strong></td></tr>");
                AddComparisonRow(sb, "Catching", player1["catchingRating"], player2["catchingRating"], true);
                AddComparisonRow(sb, "Catch In Traffic", player1["catchInTrafficRating"], player2["catchInTrafficRating"], true);
                AddComparisonRow(sb, "Spectacular Catch", player1["spectacularCatchRating"], player2["spectacularCatchRating"], true);
                AddComparisonRow(sb, "Route Running", player1["routeRunningRating"], player2["routeRunningRating"], true);
            }

            sb.Append("</tbody>");
            sb.Append("</table>");
            sb.Append("</div>");
            sb.Append("</div>");

            litComparison.Text = sb.ToString();
        }

        private void AddComparisonRow(StringBuilder sb, string label, object value1, object value2, bool isNumeric = false)
        {
            sb.Append("<tr>");
            sb.Append($"<td><strong>{label}</strong></td>");

            if (isNumeric)
            {
                var num1 = Convert.ToInt32(value1);
                var num2 = Convert.ToInt32(value2);

                var class1 = num1 > num2 ? "table-success" : (num1 < num2 ? "table-danger" : "");
                var class2 = num2 > num1 ? "table-success" : (num2 < num1 ? "table-danger" : "");

                sb.Append($"<td class='text-center {class1}'>{num1}</td>");
                sb.Append($"<td class='text-center {class2}'>{num2}</td>");
            }
            else
            {
                sb.Append($"<td class='text-center'>{value1}</td>");
                sb.Append($"<td class='text-center'>{value2}</td>");
            }

            sb.Append("</tr>");
        }
    }
}
