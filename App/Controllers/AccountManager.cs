using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MySql.Data.MySqlClient;
using App.Models;

namespace App.Controllers
{
    public class AccountManager
    {
        private DatabaseController db = new DatabaseController();

        public Account getAccount()
        {
            Account account = new Models.Account();
            MySqlConnection conn = null;

            using (conn = new MySqlConnection(db.getConnString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM user WHERE userID LIKE '11425598';";
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            account.userID = reader.GetInt32(0);
                            account.lastName = reader.GetString(1);
                            account.firstName = reader.GetString(2);
                            account.middleName = reader.GetString(3);
                            account.gender = reader.GetChar(4);
                            account.birthYear = reader.GetInt32(5);
                            account.birthMonth = reader.GetInt32(6);
                            account.birthDay = reader.GetInt32(7);
                            account.citizenship = reader.GetString(8);
                        }
                    }
                }
            }

            conn.Close();
            return account;
        }
    }
}