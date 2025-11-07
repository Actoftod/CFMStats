using System;
using System.Configuration;
using System.Data;
using System.Text;
using CFMStats.Classes;

namespace CFMStats
{
    public partial class TradeHistory : System.Web.UI.Page
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
                LoadTeams();
                LoadTradeHistory();
            }
        }

        protected void ddlSeasonSelector_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTradeHistory();
        }

        protected void ddlTeamSelector_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTradeHistory();
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

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ddlSeasonSelector.Items.Add(new System.Web.UI.WebControls.ListItem(row["seasonIndex"].ToString(), row["seasonIndex"].ToString()));
            }
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
        }

        private void LoadTradeHistory()
        {
            var sp = new StoredProc
            {
                Name = "TradeHistory_select",
                DataConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString,
                ParameterSet = new System.Data.SqlClient.SqlCommand()
            };

            sp.ParameterSet.Parameters.AddWithValue("@leagueId", Helper.IntegerNull(Session["leagueId"]));

            var seasonIndex = int.Parse(ddlSeasonSelector.SelectedValue);
            if (seasonIndex > 0)
            {
                sp.ParameterSet.Parameters.AddWithValue("@seasonIndex", seasonIndex);
            }
            else
            {
                sp.ParameterSet.Parameters.AddWithValue("@seasonIndex", DBNull.Value);
            }

            var teamId = int.Parse(ddlTeamSelector.SelectedValue);
            if (teamId > 0)
            {
                sp.ParameterSet.Parameters.AddWithValue("@teamId", teamId);
            }
            else
            {
                sp.ParameterSet.Parameters.AddWithValue("@teamId", DBNull.Value);
            }

            var ds = StoredProc.ShowMeTheData(sp);

            if (ds.Tables[0].Rows.Count == 0)
            {
                litTradeHistory.Text = "<div class='alert alert-info'>No trade history found. Trades will be tracked automatically as players change teams.</div>";
                return;
            }

            var sb = new StringBuilder();
            sb.Append("<table class='table table-striped table-bordered tablesorter'>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th>Season</th>");
            sb.Append("<th>Week</th>");
            sb.Append("<th>Player</th>");
            sb.Append("<th>Position</th>");
            sb.Append("<th>From</th>");
            sb.Append("<th>To</th>");
            sb.Append("<th>Date</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                sb.Append("<tr>");
                sb.Append($"<td>{row["seasonIndex"]}</td>");
                sb.Append($"<td>{row["weekIndex"]}</td>");
                sb.Append($"<td><a href='/Profile?id={row["playerId"]}&leagueId={Session["leagueId"]}'>{row["playerName"]}</a></td>");
                sb.Append($"<td>{row["position"]}</td>");
                sb.Append($"<td class='c{row["fromTeamName"].ToString().Replace(" ", string.Empty)}'><div style='display:none;'>{row["fromTeamName"].ToString().Replace(" ", string.Empty)}</div>{row["fromTeamName"]}</td>");
                sb.Append($"<td class='c{row["toTeamName"].ToString().Replace(" ", string.Empty)}'><div style='display:none;'>{row["toTeamName"].ToString().Replace(" ", string.Empty)}</div>{row["toTeamName"]}</td>");
                sb.Append($"<td>{Convert.ToDateTime(row["tradeDate"]).ToString("MMM dd, yyyy")}</td>");
                sb.Append("</tr>");
            }

            sb.Append("</tbody>");
            sb.Append("</table>");

            litTradeHistory.Text = sb.ToString();
        }
    }
}
