using App.Controllers;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace App.Models
{
    public class Account
    {
        public int userID { get; set; }
        public string idNumber { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string middleName { get; set; }
        public string name { get; set; }              //FOR ADMIN
        public char gender { get; set; }
        public int birthYear { get; set; }
        public int birthDay { get; set; }
        public int birthMonth { get; set; }
        public string citizenship { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string placeOfBirth { get; set; }
        public string currentAddress { get; set; }
        public string phoneNo { get; set; }
        public string alternatePhoneNo { get; set; }
        public string alternateEmail { get; set; }
        public Boolean verified { get; set; }
        public string verifiedString { get; set; }  //FOR ADMIN
        public string registeredDate { get; set; }
        public List<Degree> degrees { get; set; }
        public List<Transaction> transactions { get; set; }
        public List<MailingInformation> mailInfos { get; set; }
        public List<Document> cart { get; set; }
    }

    class AccountManager
    {
        private DatabaseConnector db = new DatabaseConnector();

        public Account getAccount(string email, string password)
        {
            Account account = new Models.Account();
            degreeManager dm = new degreeManager();
            MailingInfoModel mim = new MailingInfoModel();
            MySqlConnection conn = null;

            using (conn = new MySqlConnection(db.getConnString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM user WHERE email LIKE '" + email + "' and password like '" + password + "';";
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            account.userID = reader.GetInt32(0);

                            if (!reader.IsDBNull(1))
                            {
                                account.idNumber = reader.GetString(1);
                            }
                            else account.idNumber = "";

                            account.lastName = reader.GetString(2);
                            account.firstName = reader.GetString(3);
                            account.middleName = reader.GetString(4);
                            account.gender = reader.GetChar(5);
                            account.birthYear = reader.GetInt32(6);
                            account.birthMonth = reader.GetInt32(7);
                            account.birthDay = reader.GetInt32(8);
                            account.citizenship = reader.GetString(9);
                            account.placeOfBirth = reader.GetString(10);
                            account.currentAddress = reader.GetString(11);
                            account.phoneNo = reader.GetString(12);

                            if (!reader.IsDBNull(13))
                            {
                                account.alternatePhoneNo = reader.GetString(13);
                            }
                            else account.alternatePhoneNo = "";


                            account.email = reader.GetString(14);

                            if (!reader.IsDBNull(15))
                            {
                                account.alternateEmail = reader.GetString(15);
                            }
                            else account.alternateEmail = "";

                            account.password = reader.GetString(16);

                            if (reader.GetString(17) == "not verified")
                                account.verified = false;
                            else account.verified = true;

                            account.registeredDate = reader.GetString(18);
                            account.degrees = dm.getDegree(account.userID);
                            account.mailInfos = mim.getMailInfos(account.userID);
                            account.cart = new List<Document>();
                        }

                        if (!reader.HasRows)
                        {
                            account = null;
                        }
                    }
                }
            }

            conn.Close();
            return account;
        }

        public Account saveAccount(Account acc)
        {
            Account account = acc;
            MySqlConnection conn = new MySqlConnection(db.getConnString());

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            using (conn)
            {
                using (adapter)
                {
                    adapter.SelectCommand = new MySqlCommand("SELECT * FROM requestdocdb.user", conn);

                    adapter.InsertCommand = new MySqlCommand("INSERT INTO requestdocdb.user"
                                                             + " (idNumber, lastName, firstName, middleName, gender, birthYear, birthMonth,"
                                                             + " birthDay, citizenship, placeOfBirth, currentAddress, phoneNo,"
                                                             + " alternatePhoneNo, email, alternateEmail, password, registeredDate) "
                                                             + "VALUES (@idNumber, @lastName, @firstName, @middleName, @gender, @birthYear, @birthMonth, "
                                                             + "@birthDay, @citizenship, @placeOfBirth, @currentAddress, @phoneNo, "
                                                             + "@alternatePhoneNo, @email, @alternateEmail, @password, @registeredDate)", conn);
                    
                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("idNumber", MySqlDbType.VarChar, 11, "idNumber"));
                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("lastName", MySqlDbType.VarChar, 100, "lastName"));
                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("firstName", MySqlDbType.VarChar, 100, "firstName"));
                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("middleName", MySqlDbType.VarChar, 100, "middleName"));
                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("gender", MySqlDbType.VarChar, 1, "gender"));
                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("birthYear", MySqlDbType.Int32, 11, "birthYear"));
                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("birthMonth", MySqlDbType.Int32, 11, "birthMonth"));
                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("birthDay", MySqlDbType.Int32, 11, "birthDay"));
                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("citizenship", MySqlDbType.VarChar, 100, "citizenship"));
                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("placeOfBirth", MySqlDbType.VarChar, 500, "placeOfBirth"));
                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("currentAddress", MySqlDbType.VarChar, 500, "currentAddress"));
                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("phoneNo", MySqlDbType.VarChar, 50, "phoneNo"));
                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("alternatePhoneNo", MySqlDbType.VarChar, 50, "alternatePhoneNo"));
                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("email", MySqlDbType.VarChar, 100, "email"));
                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("alternateEmail", MySqlDbType.VarChar, 100, "alternateEmail"));
                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("password", MySqlDbType.VarChar, 100, "password"));
                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("registeredDate", MySqlDbType.VarChar, 100, "registeredDate"));

                    using (DataSet dataSet = new DataSet())
                    {
                        adapter.Fill(dataSet, "user");

                        DataRow newRow = dataSet.Tables[0].NewRow();
                        
                        if(!(acc.idNumber == null))
                        {
                            newRow["idNumber"] = acc.idNumber;
                        }

                        newRow["lastName"] = acc.lastName;
                        newRow["firstName"] = acc.firstName;
                        newRow["middleName"] = acc.middleName;
                        newRow["gender"] = acc.gender;
                        newRow["birthYear"] = acc.birthYear;
                        newRow["birthMonth"] = acc.birthMonth;
                        newRow["birthDay"] = acc.birthDay;
                        newRow["citizenship"] = acc.citizenship;
                        newRow["placeOfBirth"] = acc.placeOfBirth;
                        newRow["currentAddress"] = acc.currentAddress;
                        newRow["phoneNo"] = acc.phoneNo;

                        if (!(acc.alternateEmail == null))
                        {
                            newRow["alternatePhoneNo"] = acc.alternatePhoneNo;
                        }
                        
                        newRow["email"] = acc.email;

                        if (!(acc.alternateEmail == null))
                        {
                            newRow["alternateEmail"] = acc.alternateEmail;
                        }
                        
                        newRow["password"] = acc.password;
                        string now = DateTime.Today.ToString();
                        newRow["registeredDate"] = now;

                        dataSet.Tables[0].Rows.Add(newRow);

                        adapter.Update(dataSet, "user");
                    }
                }
            }

            conn.Close();
            return this.getAccount(acc.email, acc.password);
        }
        
    }

    class adminVerifyManager
    {

        private DatabaseConnector db = new DatabaseConnector();


        public List<Account> getUserList()
        {
            List<Account> userList = new List<Account>();
            Account user = new Account();

            MySqlConnection conn = null;
            DataTable dt = new DataTable();
            MySqlDataAdapter sda = new MySqlDataAdapter();

            using (conn = new MySqlConnection(db.getConnString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT user.idNumber, CONCAT(CONCAT(user.lastName, ', '), user.firstName) as 'Name', user.verified FROM user;";
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user = new Account();

                            //user.userID = reader.GetInt32(0);

                            if (!reader.IsDBNull(0))
                            {
                                user.idNumber = reader.GetString(0);
                            }
                            else user.idNumber = "";



                            user.name = reader.GetString(1);
                            user.verifiedString = reader.GetString(2);

                            userList.Add(user);
                        }

                        if (!reader.HasRows)
                        {
                            user = null;
                        }
                    }
                }
            }

            return userList;
        }
    }

}
