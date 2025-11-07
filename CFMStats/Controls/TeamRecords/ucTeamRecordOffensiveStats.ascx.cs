using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using CFMStats.Classes;

namespace CFMStats.Controls.TeamRecords
{
    public partial class ucTeamRecordOffensiveStats : System.Web.UI.UserControl
    {
        public string Duration { get; set; }
        public int LeagueId { get; set; }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            Fetch(LeagueId, Duration, "PassYards");
            Fetch(LeagueId, Duration, "RushYards");
            Fetch(LeagueId, Duration, "TotalYards");
            Fetch(LeagueId, Duration, "Touchdowns");
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void Fetch(int leagueId, string duration, string category)
        {

            switch (category)
            {
                case "PassYards":
                    tablePassYards.InnerHtml = string.Empty;
                    break;
                case "RushYards":
                    tableRushYards.InnerHtml = string.Empty;
                    break;
                case "TotalYards":
                    tableTotalYards.InnerHtml = string.Empty;
                    break;
                case "Touchdowns":
                    tableTouchdowns.InnerHtml = string.Empty;
                    break;
            }

            var sp = new StoredProc
            {
                Name = "records.TeamOffensiveStats",
                DataConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString,
                ParameterSet = new System.Data.SqlClient.SqlCommand()
            };

            sp.ParameterSet.Parameters.AddWithValue("@leagueId", leagueId);
            sp.ParameterSet.Parameters.AddWithValue("@stageIndex", 1);
            sp.ParameterSet.Parameters.AddWithValue("@orderby", category);
            sp.ParameterSet.Parameters.AddWithValue("@duration", duration);


            var ds = StoredProc.ShowMeTheData(sp);

            var sbTable = new System.Text.StringBuilder();

            sbTable.Append("<table id='offensiverecords' class='table table-condensed table-bordered tablesorter' >");

            sbTable.Append("<thead>");
            sbTable.Append("<tr>");

            sbTable.Append("<th>Team</th>");

            if (duration != "career")
            {
                sbTable.Append("<th>Season</th>");
            }

            if (duration == "game")
            {
                sbTable.Append("<th>Week</th>");
                sbTable.Append("<th>Opponent</th>");
            }

            if (duration == "season")
            {
                sbTable.Append("<th>Games</th>");
            }

            sbTable.Append($"<th>{category}</th>");

            sbTable.Append("</tr>");
            sbTable.Append("</thead>");

            sbTable.Append("<tbody>");
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                sbTable.Append("<tr>");
                sbTable.Append(string.Format("<td class='c{0}'><div style='display:none;'>{0}</div></td>", item.Field<string>("team").Replace(" ", string.Empty)));

                if (duration != "career")
                {
                    sbTable.Append($"<td>{item.Field<int>("season") }</td>");
                }

                if (duration == "game")
                {
                    sbTable.Append($"<td>{item.Field<int>("week")}</td>");

                    // setup the box score link
                    var season = item.Field<int>("season");
                    var week = item.Field<int>("week") - 1;
                    var teamId = item.Field<int>("teamId");
                    var opponentTeamId = item.Field<int>(item.Field<int>("teamId") == item.Field<int>("awayTeamId") ? "homeTeamId" : "awayTeamId");
                    var opponentTeam = item.Field<string>(item.Field<int>("teamId") != item.Field<int>("awayTeamId") ? "awayTeamName" : "homeTeamName");

                    sbTable.Append($"<td style='text-align:left;'><a href='/BoxScore?id={opponentTeamId}&leagueId={leagueId}&season={season}&week={week}&type=1'>{opponentTeam}</a></td>");
                }

                if (duration == "season")
                {
                    sbTable.Append($"<td>{item.Field<int>("games")}</td>");
                }

                sbTable.Append($"<td>{item.Field<int>(category):n0}</td>");

                sbTable.Append("</tr>");
            }

            sbTable.Append("</tbody>");
            sbTable.Append("</table>");

            switch (category)
            {
                case "PassYards":
                    tablePassYards.InnerHtml = sbTable.ToString();
                    break;
                case "RushYards":
                    tableRushYards.InnerHtml = sbTable.ToString();
                    break;
                case "TotalYards":
                    tableTotalYards.InnerHtml = sbTable.ToString();
                    break;
                case "Touchdowns":
                    tableTouchdowns.InnerHtml = sbTable.ToString();
                    break;
            }

        }

        protected void UpdatePanel1_Unload(object sender, EventArgs e)
        {
            MethodInfo methodInfo = typeof(ScriptManager).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).Where(i => i.Name.Equals("System.Web.UI.IScriptManagerInternal.RegisterUpdatePanel")).First();
            methodInfo.Invoke(ScriptManager.GetCurrent(Page), new object[] { sender as UpdatePanel });
        }

    }
}
