using System;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web.UI;
using CFMStats.Classes;

namespace CFMStats
{
    public partial class PlayoffBracket : System.Web.UI.Page
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
                LoadSeasons();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack && ddlSeasonSelector.Items.Count > 0)
            {
                LoadBracket();
            }
        }

        protected void ddlSeasonSelector_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            LoadBracket();
        }

        private void LoadSeasons()
        {
            var sp = new StoredProc
            {
                Name = "Seasons_select",
                DataConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString,
                ParameterSet = new System.Data.SqlClient.SqlCommand()
            };

            sp.ParameterSet.Parameters.AddWithValue("@leagueId", Helper.IntegerNull(Session["leagueId"]));

            var ds = StoredProc.ShowMeTheData(sp);

            ddlSeasonSelector.DataSource = ds.Tables[0];
            ddlSeasonSelector.DataTextField = "seasonIndex";
            ddlSeasonSelector.DataValueField = "seasonIndex";
            ddlSeasonSelector.DataBind();
        }

        private void LoadBracket()
        {
            if (phBracketHolder.Controls.Count > 0)
            {
                phBracketHolder.Controls.Clear();
            }

            if (ddlSeasonSelector.SelectedValue == null)
            {
                return;
            }

            var seasonIndex = int.Parse(ddlSeasonSelector.SelectedValue);
            lblSeasonIndex.Text = seasonIndex.ToString();

            var sp = new StoredProc
            {
                Name = "PlayoffBracket_select",
                DataConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString,
                ParameterSet = new System.Data.SqlClient.SqlCommand()
            };

            sp.ParameterSet.Parameters.AddWithValue("@leagueId", Helper.IntegerNull(Session["leagueId"]));
            sp.ParameterSet.Parameters.AddWithValue("@seasonIndex", seasonIndex);

            var ds = StoredProc.ShowMeTheData(sp);

            if (ds.Tables[0].Rows.Count == 0)
            {
                var noData = new LiteralControl("<div class='alert alert-info'>No playoff data available for this season. Playoff bracket will be populated as games are synced from Madden.</div>");
                phBracketHolder.Controls.Add(noData);
                return;
            }

            // Build AFC Bracket
            var afcBracket = BuildConferenceBracket(ds.Tables[0], "AFC", seasonIndex);
            phBracketHolder.Controls.Add(new LiteralControl(afcBracket));

            // Build NFC Bracket
            var nfcBracket = BuildConferenceBracket(ds.Tables[0], "NFC", seasonIndex);
            phBracketHolder.Controls.Add(new LiteralControl(nfcBracket));

            // Build Super Bowl
            var superBowl = BuildSuperBowl(ds.Tables[0], seasonIndex);
            if (!string.IsNullOrEmpty(superBowl))
            {
                phBracketHolder.Controls.Add(new LiteralControl(superBowl));
            }
        }

        private string BuildConferenceBracket(DataTable dt, string conference, int seasonIndex)
        {
            var sb = new StringBuilder();

            sb.Append($"<div class='conference-bracket'>");
            sb.Append($"<div class='conference-title'>{conference} Playoffs</div>");
            sb.Append("<div class='bracket-rounds'>");

            // Wild Card Round
            sb.Append("<div class='round'>");
            sb.Append("<div class='round-title'>Wild Card</div>");
            sb.Append(BuildRoundMatchups(dt, conference, 1));
            sb.Append("</div>");

            // Divisional Round
            sb.Append("<div class='round'>");
            sb.Append("<div class='round-title'>Divisional</div>");
            sb.Append(BuildRoundMatchups(dt, conference, 2));
            sb.Append("</div>");

            // Conference Championship
            sb.Append("<div class='round'>");
            sb.Append("<div class='round-title'>Championship</div>");
            sb.Append(BuildRoundMatchups(dt, conference, 3));
            sb.Append("</div>");

            sb.Append("</div>"); // bracket-rounds
            sb.Append("</div>"); // conference-bracket

            return sb.ToString();
        }

        private string BuildRoundMatchups(DataTable dt, string conference, int round)
        {
            var sb = new StringBuilder();
            var leagueId = Helper.IntegerNull(Session["leagueId"]);

            var rows = dt.Select($"conference = '{conference}' AND round = {round}");

            foreach (DataRow row in rows)
            {
                var homeTeamId = row["homeTeamId"] != DBNull.Value ? (int)row["homeTeamId"] : 0;
                var awayTeamId = row["awayTeamId"] != DBNull.Value ? (int)row["awayTeamId"] : 0;
                var winnerTeamId = row["winnerTeamId"] != DBNull.Value ? (int)row["winnerTeamId"] : 0;
                var homeTeamScore = row["homeTeamScore"] != DBNull.Value ? (int)row["homeTeamScore"] : (int?)null;
                var awayTeamScore = row["awayTeamScore"] != DBNull.Value ? (int)row["awayTeamScore"] : (int?)null;

                var homeTeamName = row["homeTeamName"] != DBNull.Value ? row["homeTeamName"].ToString() : "TBD";
                var awayTeamName = row["awayTeamName"] != DBNull.Value ? row["awayTeamName"].ToString() : "TBD";

                sb.Append("<div class='matchup'>");

                // Away Team
                var awayWinnerClass = winnerTeamId == awayTeamId && winnerTeamId > 0 ? "winner" : "";
                sb.Append($"<div class='team {awayWinnerClass}'>");
                sb.Append($"<span class='team-name'>{awayTeamName}</span>");
                if (awayTeamScore.HasValue)
                {
                    sb.Append($"<span class='team-score'>{awayTeamScore.Value}</span>");
                }
                sb.Append("</div>");

                // Home Team
                var homeWinnerClass = winnerTeamId == homeTeamId && winnerTeamId > 0 ? "winner" : "";
                sb.Append($"<div class='team {homeWinnerClass}'>");
                sb.Append($"<span class='team-name'>{homeTeamName}</span>");
                if (homeTeamScore.HasValue)
                {
                    sb.Append($"<span class='team-score'>{homeTeamScore.Value}</span>");
                }
                sb.Append("</div>");

                sb.Append("</div>"); // matchup
            }

            return sb.ToString();
        }

        private string BuildSuperBowl(DataTable dt, int seasonIndex)
        {
            var sb = new StringBuilder();
            var leagueId = Helper.IntegerNull(Session["leagueId"]);

            var rows = dt.Select("round = 4");

            if (rows.Length == 0)
            {
                return string.Empty;
            }

            sb.Append("<div class='super-bowl'>");
            sb.Append("<div class='conference-title'>Super Bowl</div>");

            foreach (DataRow row in rows)
            {
                var homeTeamId = row["homeTeamId"] != DBNull.Value ? (int)row["homeTeamId"] : 0;
                var awayTeamId = row["awayTeamId"] != DBNull.Value ? (int)row["awayTeamId"] : 0;
                var winnerTeamId = row["winnerTeamId"] != DBNull.Value ? (int)row["winnerTeamId"] : 0;
                var homeTeamScore = row["homeTeamScore"] != DBNull.Value ? (int)row["homeTeamScore"] : (int?)null;
                var awayTeamScore = row["awayTeamScore"] != DBNull.Value ? (int)row["awayTeamScore"] : (int?)null;

                var homeTeamName = row["homeTeamName"] != DBNull.Value ? row["homeTeamName"].ToString() : "TBD";
                var awayTeamName = row["awayTeamName"] != DBNull.Value ? row["awayTeamName"].ToString() : "TBD";

                sb.Append("<div class='matchup'>");

                // Away Team
                var awayWinnerClass = winnerTeamId == awayTeamId && winnerTeamId > 0 ? "winner" : "";
                sb.Append($"<div class='team {awayWinnerClass}'>");
                sb.Append($"<span class='team-name'>{awayTeamName}</span>");
                if (awayTeamScore.HasValue)
                {
                    sb.Append($"<span class='team-score'>{awayTeamScore.Value}</span>");
                }
                sb.Append("</div>");

                // Home Team
                var homeWinnerClass = winnerTeamId == homeTeamId && winnerTeamId > 0 ? "winner" : "";
                sb.Append($"<div class='team {homeWinnerClass}'>");
                sb.Append($"<span class='team-name'>{homeTeamName}</span>");
                if (homeTeamScore.HasValue)
                {
                    sb.Append($"<span class='team-score'>{homeTeamScore.Value}</span>");
                }
                sb.Append("</div>");

                sb.Append("</div>"); // matchup
            }

            sb.Append("</div>"); // super-bowl

            return sb.ToString();
        }
    }
}
