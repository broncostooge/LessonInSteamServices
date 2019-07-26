using LessonInSteam.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace LessonInSteam.Services
{
    public class DatabaseService
    {
        string connectionString = "Server=tcp:steamgamereview.database.windows.net,1433;Initial Catalog=LessonInSteam;Persist Security Info=False;User ID=broncostooge;Password=Toyota86!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        SqlConnection cnn;
        SqlDataAdapter adapter = new SqlDataAdapter();

        public async Task RegisterUserToDBAsync(User user)
        {
            SteamDataService SteamData = new SteamDataService();
            SteamUserContainer SteamUser = new SteamUserContainer();
            SteamGameContainer SteamGameList = new SteamGameContainer();

            string steamUserID;
            List<SteamGame> games;

            SteamUser = await SteamData.GetSteamUser64IDAsync(user.username);
            SteamGameList = await SteamData.GetUsersGames(SteamUser.response.steamID);

            steamUserID = SteamUser.response.steamID;
            games = SteamGameList.response.games;

            cnn = new SqlConnection(connectionString);
            cnn.Open();
            SqlCommand cmd = new SqlCommand();

            AddUserCredentialsToTable(user, cmd);

            AddUserSteamIDToTable(user, steamUserID, cmd);

            AddUserGameListToTable(games, steamUserID, cmd);

            cnn.Close();
        }

        private void AddUserCredentialsToTable(User user, SqlCommand cmd)
        {
            //Add User to Login Table
            cmd = new SqlCommand("Login_CreateUser", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Username", user.username);
            cmd.Parameters.AddWithValue("@Password", user.password);
            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
        }

        private void AddUserSteamIDToTable(User user, string steamUserID, SqlCommand cmd)
        {
            //Add User's SteamID to SteamUser Table
            cmd = new SqlCommand("SteamUser_CreateSteamUser", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SteamUserName", user.username);
            cmd.Parameters.AddWithValue("@SteamUser64ID", steamUserID);
            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
        }

        private void AddUserGameListToTable(List<SteamGame> games, string steamUserID, SqlCommand cmd)
        {
            //Add User's Game List to GameList Table
            cmd = new SqlCommand("SteamGameList_CreateSteamGameList", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (var game in games)
            {
                cmd.Parameters.AddWithValue("@SteamUser64ID", steamUserID);
                cmd.Parameters.AddWithValue("@AppID", game.appid);
                cmd.Parameters.AddWithValue("@Img_Icon_URL", game.img_icon_url);
                cmd.Parameters.AddWithValue("@Img_Logo_URL", game.img_logo_url);
                cmd.Parameters.AddWithValue("@GameName", game.name);
                cmd.Parameters.AddWithValue("@Playtime_Forever", game.playtime_forever);
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }
        }

        public string LoginUser(User user)
        {
            string accepted = null;

            cnn = new SqlConnection(connectionString);
            cnn.Open();

            SqlCommand cmd = new SqlCommand("Login_CheckUserCredentials", cnn);
            cmd.Parameters.AddWithValue("@Username", user.username);
            cmd.Parameters.AddWithValue("@Password", user.password);

            cmd.CommandType = CommandType.StoredProcedure;

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    accepted = reader["Accepted"].ToString(); 
                }
            }
            cnn.Close();
            
            return accepted;
        }

        public void DeleteUserFromLoginTable(User user)
        {
            cnn = new SqlConnection(connectionString);
            cnn.Open();

            SqlCommand cmd = new SqlCommand("Login_DeleteUser", cnn);
            cmd.Parameters.AddWithValue("@Username", user.username);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();
            cnn.Close();
        }

        public void UpdateUserPassword(User user)
        {
            cnn = new SqlConnection(connectionString);
            cnn.Open();

            SqlCommand cmd = new SqlCommand("Login_UpdateUserPassword", cnn);
            cmd.Parameters.AddWithValue("@Username", user.username);
            cmd.Parameters.AddWithValue("@Password", user.password);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();
            cnn.Close();
        }

        public List<SteamGame> GetUserSteamGameInfo(User user)
        {

            List<SteamGame> UserSteamGameList = new List<SteamGame>();

            cnn = new SqlConnection(connectionString);
            cnn.Open();

            SqlCommand cmd = new SqlCommand("GetUserSteamGameInfo", cnn);
            cmd.Parameters.AddWithValue("@Username", user.username);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    SteamGame steamGame = new SteamGame();

                    steamGame.appid = Int32.Parse(Convert.ToString(reader["AppID"]));
                    steamGame.name = reader["GameName"].ToString();
                    steamGame.img_icon_url = reader["Img_Icon_URL"].ToString();
                    steamGame.img_logo_url = reader["Img_Logo_URL"].ToString();
                    steamGame.playtime_forever = Int32.Parse(Convert.ToString(reader["Playtime_Forever"]));

                    UserSteamGameList.Add(steamGame);
                }
            }
            cnn.Close();

            return UserSteamGameList;

        }
    }
}