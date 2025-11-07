using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using CFMStats.Classes;

namespace CFMStats
{
    public partial class DepthChart : System.Web.UI.Page
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
                LoadTeams();
            }
        }

        protected void ddlTeamSelector_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDepthChart();
        }

        private void LoadTeams()
        {
            var sp = new StoredProc
            {
                Name = "TeamInfo_select",
                DataConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString,
                ParameterSet = new System.Data.SqlClient.SqlCommand()
            };

            sp.ParameterSet.Parameters.AddWithValue("@leagueId", Helper.IntegerNull(Session["leagueId"]));

            var ds = StoredProc.ShowMeTheData(sp);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ddlTeamSelector.Items.Add(new System.Web.UI.WebControls.ListItem(row["displayName"].ToString(), row["teamId"].ToString()));
            }

            if (ddlTeamSelector.Items.Count > 0)
            {
                LoadDepthChart();
            }
        }

        private void LoadDepthChart()
        {
            var teamId = int.Parse(ddlTeamSelector.SelectedValue);

            // Get all players for the team
            var sp = new StoredProc
            {
                Name = "PlayerProfile_select",
                DataConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString,
                ParameterSet = new System.Data.SqlClient.SqlCommand()
            };

            sp.ParameterSet.Parameters.AddWithValue("@leagueId", Helper.IntegerNull(Session["leagueId"]));
            sp.ParameterSet.Parameters.AddWithValue("@teamId", teamId);

            var ds = StoredProc.ShowMeTheData(sp);

            if (ds.Tables[0].Rows.Count == 0)
            {
                litDepthChart.Text = "<div class='alert alert-info'>No players found for this team.</div>";
                return;
            }

            // Get ratings for sorting
            var spRatings = new StoredProc
            {
                Name = "PlayerRatings_select",
                DataConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString,
                ParameterSet = new System.Data.SqlClient.SqlCommand()
            };

            spRatings.ParameterSet.Parameters.AddWithValue("@leagueId", Helper.IntegerNull(Session["leagueId"]));
            spRatings.ParameterSet.Parameters.AddWithValue("@teamId", teamId);

            var dsRatings = StoredProc.ShowMeTheData(spRatings);

            // Merge ratings data
            var playerData = from player in ds.Tables[0].AsEnumerable()
                            join rating in dsRatings.Tables[0].AsEnumerable()
                            on player.Field<int>("playerId") equals rating.Field<int>("playerId") into ratings
                            from rating in ratings.DefaultIfEmpty()
                            select new
                            {
                                PlayerId = player.Field<int>("playerId"),
                                PlayerName = player.Field<string>("firstName") + " " + player.Field<string>("lastName"),
                                Position = player.Field<string>("position"),
                                Overall = rating != null ? rating.Field<int>("overallRating") : 0,
                                JerseyNum = player.Field<int>("jerseyNum")
                            };

            // Group by position
            var positionGroups = playerData.GroupBy(p => p.Position).OrderBy(g => GetPositionOrder(g.Key));

            var sb = new StringBuilder();

            foreach (var group in positionGroups)
            {
                sb.Append("<div class='position-group'>");
                sb.Append($"<div class='position-title'>{group.Key} - {GetPositionName(group.Key)}</div>");

                var depth = 1;
                foreach (var player in group.OrderByDescending(p => p.Overall))
                {
                    var depthClass = depth == 1 ? "starter" : (depth == 2 ? "backup" : "");

                    sb.Append($"<div class='depth-player {depthClass}'>");
                    sb.Append($"<span class='depth-number'>{depth}.</span> ");
                    sb.Append($"#{player.JerseyNum} <a href='/Profile?id={player.PlayerId}&leagueId={Session["leagueId"]}'>{player.PlayerName}</a> ");
                    sb.Append($"<span class='badge bg-secondary'>OVR {player.Overall}</span>");
                    sb.Append("</div>");

                    depth++;
                }

                sb.Append("</div>");
            }

            litDepthChart.Text = sb.ToString();
        }

        private int GetPositionOrder(string position)
        {
            var order = new System.Collections.Generic.Dictionary<string, int>
            {
                {"QB", 1}, {"HB", 2}, {"FB", 3}, {"WR", 4}, {"TE", 5},
                {"LT", 6}, {"LG", 7}, {"C", 8}, {"RG", 9}, {"RT", 10},
                {"LE", 11}, {"RE", 12}, {"DT", 13}, {"LOLB", 14}, {"MLB", 15}, {"ROLB", 16},
                {"CB", 17}, {"FS", 18}, {"SS", 19},
                {"K", 20}, {"P", 21}
            };

            return order.ContainsKey(position) ? order[position] : 99;
        }

        private string GetPositionName(string position)
        {
            var names = new System.Collections.Generic.Dictionary<string, string>
            {
                {"QB", "Quarterback"}, {"HB", "Halfback"}, {"FB", "Fullback"},
                {"WR", "Wide Receiver"}, {"TE", "Tight End"},
                {"LT", "Left Tackle"}, {"LG", "Left Guard"}, {"C", "Center"},
                {"RG", "Right Guard"}, {"RT", "Right Tackle"},
                {"LE", "Left End"}, {"RE", "Right End"}, {"DT", "Defensive Tackle"},
                {"LOLB", "Left Outside Linebacker"}, {"MLB", "Middle Linebacker"}, {"ROLB", "Right Outside Linebacker"},
                {"CB", "Cornerback"}, {"FS", "Free Safety"}, {"SS", "Strong Safety"},
                {"K", "Kicker"}, {"P", "Punter"}
            };

            return names.ContainsKey(position) ? names[position] : position;
        }
    }
}
