using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace App.Models
{
    public class MailingInformation
    {
        public int mailingID { get; set; }
        public string zipcode { get; set; }
        public string streetname { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public int locationID { get; set; }
        public int userID { get; set; }
        public string addressline { get; set; }
        public string contactNumber { get; set; }
        public string contactPerson { get; set; }
        public string locationName { get; set; }
    }

    public class MailingInfoModel
    {
        private DatabaseConnector db = new DatabaseConnector();

        public void addMailingInfo(MailingInformation mailInfo)
        {
            MySqlConnection conn = new MySqlConnection(db.getConnString());

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            using (conn)
            {
                using (adapter)
                {
                    
                    MySqlCommand command = new MySqlCommand("insert into requestdocdb.mailingaddress"
                                                             + " (zipcode, streetName, city, country, locationID, userID, addressline, contactPerson, contactNumber) "
                                                             + "VALUES (@zipcode, @streetName, @city, @country, @locationID, @userID, @addressline, @contactPerson, @contactNumber)", conn);

                    adapter.SelectCommand = new MySqlCommand("SELECT * FROM requestdocdb.mailingaddress", conn);
                    adapter.InsertCommand = command;

                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("zipcode", MySqlDbType.VarChar, 50, "zipcode"));
                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("streetName", MySqlDbType.VarChar, 50, "streetName"));
                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("city", MySqlDbType.VarChar, 50, "city"));
                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("country", MySqlDbType.VarChar, 100, "country"));
                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("locationID", MySqlDbType.VarChar, 50, "locationID"));
                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("userID", MySqlDbType.Int32, 11, "userID"));
                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("addressline", MySqlDbType.VarChar, 200, "addressline"));
                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("contactPerson", MySqlDbType.VarChar, 100, "contactPerson"));
                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("contactNumber", MySqlDbType.VarChar, 20, "contactNumber"));

                    using (DataSet dataSet = new DataSet())
                    {
                        adapter.Fill(dataSet, "mailingaddress");

                        DataRow newRow = dataSet.Tables[0].NewRow();

                        newRow["zipcode"] = mailInfo.zipcode;
                        newRow["streetName"] = mailInfo.streetname;
                        newRow["city"] = mailInfo.city;
                        newRow["country"] = mailInfo.country;
                        newRow["locationID"] = mailInfo.locationID;
                        newRow["userID"] = mailInfo.userID;

                        if (!(mailInfo.addressline == null))
                        {
                            newRow["addressline"] = mailInfo.addressline;
                        }

                        newRow["contactPerson"] = mailInfo.contactPerson;
                        newRow["contactNumber"] = mailInfo.contactNumber;

                        dataSet.Tables[0].Rows.Add(newRow);

                        adapter.Update(dataSet, "mailingaddress");
                    }
                }
            }
            conn.Close();
        }

        public List<MailingInformation> getMailInfos(int userID)
        {
            List<MailingInformation> listmailinfo = new List<MailingInformation>();
            DeliveryRateManager delivManager = new DeliveryRateManager();
            MySqlConnection conn = null;

            using (conn = new MySqlConnection(db.getConnString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM mailingaddress WHERE userID LIKE '" + userID + "';";
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MailingInformation mi = new Models.MailingInformation();
                            mi.mailingID = reader.GetInt32(0);
                            mi.zipcode = reader.GetString(1);
                            mi.streetname = reader.GetString(2);
                            mi.city = reader.GetString(3);
                            mi.country = reader.GetString(4);
                            mi.locationID = reader.GetInt32(5);
                            mi.userID = reader.GetInt32(6);

                            if (!reader.IsDBNull(7))
                            {
                                mi.addressline = reader.GetString(7);
                            }
                            else mi.addressline = "";

                            mi.contactPerson = reader.GetString(8);
                            mi.contactNumber = reader.GetString(9);
                            mi.locationName = delivManager.getLocation(mi.locationID);

                            listmailinfo.Add(mi);
                        }

                        if (!reader.HasRows)
                        {
                            listmailinfo = null;
                        }
                    }
                }
            }

            conn.Close();
            return listmailinfo;
        }

        public MailingInformation getMail(int mailingID)
        {
            MailingInformation mi = new MailingInformation();
            DeliveryRateManager delivManager = new DeliveryRateManager();
            MySqlConnection conn = null;

            using (conn = new MySqlConnection(db.getConnString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM mailingaddress WHERE mailingID LIKE '" + mailingID + "';";
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            mi.mailingID = reader.GetInt32(0);
                            mi.zipcode = reader.GetString(1);
                            mi.streetname = reader.GetString(2);
                            mi.city = reader.GetString(3);
                            mi.country = reader.GetString(4);
                            mi.locationID = reader.GetInt32(5);
                            mi.userID = reader.GetInt32(6);

                            if (!reader.IsDBNull(7))
                            {
                                mi.addressline = reader.GetString(7);
                            }
                            else mi.addressline = "";

                            mi.contactPerson = reader.GetString(8);
                            mi.contactNumber = reader.GetString(9);
                            mi.locationName = delivManager.getLocation(mi.locationID);
                        }

                        if (!reader.HasRows)
                        {
                            mi = null;
                        }
                    }
                }
            }

            conn.Close();
            return mi;
        }
    }
}