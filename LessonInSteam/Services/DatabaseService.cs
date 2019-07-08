using LessonInSteam.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LessonInSteam.Services
{
    public class DatabaseService
    {
        string connectionString = "Server=tcp:steamgamereview.database.windows.net,1433;Initial Catalog=LessonInSteam;Persist Security Info=False;User ID=broncostooge;Password=Toyota86!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        SqlConnection cnn;
        SqlDataAdapter adapter = new SqlDataAdapter();

        public async System.Threading.Tasks.Task RegisterUserToDBAsync(User user)
        {
            SteamDataService SteamData = new SteamDataService();
            SteamUserContainer SteamUser = new SteamUserContainer();
            SteamGameContainer SteamGameList = new SteamGameContainer();
            
            SteamUser = await SteamData.GetSteamUser64IDAsync(user.username);
            SteamGameList = await SteamData.GetUsersGames(SteamUser.response.steamID);

            cnn = new SqlConnection(connectionString);
            cnn.Open();

            //Add User to Login Table
            SqlCommand cmd = new SqlCommand("Login_CreateUser", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Username", user.username);
            cmd.Parameters.AddWithValue("@Password", user.password);
            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();

            //Add User's SteamID to SteamUser Table
            cmd = new SqlCommand("SteamUser_CreateSteamUser", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SteamUserName", user.username);
            cmd.Parameters.AddWithValue("@SteamUser64ID", SteamUser.response.steamID);
            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();

            //Add User's Game List to GameList Table
            cmd = new SqlCommand("SteamGameList_CreateSteamGameList", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            foreach(var game in SteamGameList.response.games)
            {
                cmd.Parameters.AddWithValue("@SteamUser64ID", SteamUser.response.steamID);
                cmd.Parameters.AddWithValue("@AppID", game.appid);
                cmd.Parameters.AddWithValue("@Img_Icon_URL", game.img_icon_url);
                cmd.Parameters.AddWithValue("@Img_Logo_URL", game.img_logo_url);
                cmd.Parameters.AddWithValue("@GameName", game.name);
                cmd.Parameters.AddWithValue("@Playtime_Forever", game.playtime_forever);
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }
            cnn.Close();
        }

        public string ReturnUserHashPassword(User user)
        {
            string password = null;
            cnn = new SqlConnection(connectionString);
            cnn.Open();
            SqlCommand cmd = new SqlCommand("Login_GetUserPassword", cnn);
            cmd.Parameters.AddWithValue("@Username", user.username);
            cmd.CommandType = CommandType.StoredProcedure;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    password = reader["password"].ToString(); 
                }
            }
            cnn.Close();

            return password;
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
    }
}