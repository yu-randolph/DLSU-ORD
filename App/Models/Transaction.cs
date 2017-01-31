using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Data;
using Newtonsoft.Json;

namespace App.Models
{
    public class Transaction
    {
        public int userID { get; set; }
        public float price { get; set; }
        public int transactionID { get; set; }
        public int mailingID { get; set; }
        public string deliveryProcessing { get; set; }
        //----------------------------------------------
        // dateValue.ToString("yyyy-MM-ddHH:mm:ss");
        public string estimatedDeliveryDate { get; set; }
        public string dateRequested { get; set; } 
        public string dateDue { get; set; }
        //-----------------------------------------------
        public string userName { get; set; } //FOR ADMIN
        public string priceStr { get; set; } //FOR ADMIN
    }

    class transactionManager
    {
            private DatabaseConnector db = new DatabaseConnector();
        

            public void saveTransaction(Transaction temp)
            {
                    Transaction tran = temp;
                    MySqlConnection conn = new MySqlConnection(db.getConnString());

                    MySqlDataAdapter adapter = new MySqlDataAdapter();

                    using(conn)
                    {
                            using(adapter)
                            {
                                    adapter.SelectCommand = new MySqlCommand("SELECT * FROM requestdocdb.transactions", conn);

                                    adapter.InsertCommand = new MySqlCommand("insert into requestdocdb.transactions"
                                                             + " (userID, price, mailingID, deliveryProcessing, estimatedDeliveryDate, dateRequested, dateDue) "
                                                             + "VALUES (@userID, @price, @mailingID, @deliveryProcessing, @estimatedDeliveryDate, @dateRequested, @dateDue)", conn);

                                    // + " (userID, price, transcationID, mailingID, deliveryProcessing, estimatedDeliveryDate, dateRequested, dateDue, orderID) "
                                    // + "VALUES (@userID, @price, @transcationID, @mailingID, @deliveryProcessing, @estimatedDeliveryDate, @dateRequested, @dateDue, @orderID)", conn);

                                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("userID", MySqlDbType.Int32, 11, "userID"));
                                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("price", MySqlDbType.Float, 4, "price"));
                                    // adapter.InsertCommand.Parameters.Add(new MySqlParameter("transactionID", MySqlDbType.Int32, 11, "transactionID"));
                                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("mailingID", MySqlDbType.Int32, 11, "mailingID"));
                                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("deliveryProcessing", MySqlDbType.VarChar, 100, "deliveryProcessing"));
                                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("estimatedDeliveryDate", MySqlDbType.VarChar, 100, "estimatedDeliveryDate"));
                                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("dateRequested", MySqlDbType.VarChar, 100, "dateRequested"));
                                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("dateDue", MySqlDbType.VarChar, 100, "dateDue"));
                                    //adapter.InsertCommand.Parameters.Add(new MySqlParameter("orderID", MySqlDbType.Int32, 11, "orderID"));

                                    using (DataSet dataSet = new DataSet())
                                    {
                                        adapter.Fill(dataSet, "transactions");

                                        DataRow newRow = dataSet.Tables[0].NewRow();

                                        newRow["userID"] = tran.userID;
                                        newRow["price"] = tran.price;
                                        //newRow["transcationID"] = tran.transactionID;
                                        newRow["mailingID"] = tran.mailingID;
                                        newRow["deliveryProcessing"] = tran.deliveryProcessing;
                                        newRow["estimatedDeliveryDate"] = tran.estimatedDeliveryDate;
                                        newRow["dateRequested"] = tran.dateRequested;
                                        newRow["dateDue"] = tran.dateDue;
                                        //newRow["orderID"] = tran.orderID;

                                        dataSet.Tables[0].Rows.Add(newRow);

                                        adapter.Update(dataSet, "transactions");
                                    }
                            }
                    }
            }


        public List<Transaction> getTransaction(int userID)
        {
            List<Transaction> listTran = new List<Transaction>();
            MySqlConnection conn = null;

            using (conn = new MySqlConnection(db.getConnString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM transactions WHERE userID LIKE '" + userID + "';";
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Transaction tran = new Models.Transaction();
                            tran.userID = reader.GetInt32(1);
                            tran.price = reader.GetInt32(7);
                            tran.transactionID = reader.GetInt32(0);
                            tran.mailingID = reader.GetInt32(2);
                            tran.deliveryProcessing = reader.GetString(3);
                            tran.estimatedDeliveryDate = reader.GetString(6);
                            tran.dateRequested = reader.GetString(4);
                            tran.dateDue = reader.GetString(5);

                            listTran.Add(tran);
                            tran = new Models.Transaction();
                        }

                        if (!reader.HasRows)
                        {
                            listTran = null;
                        }
                    }
                }
                return listTran;
            }
        }

        public void checkout(string jsonCart, int userID, float price, int mailingID)
        {
            List<Order> cart = JsonConvert.DeserializeObject<List<Order>>(jsonCart);
            Transaction temp = new Transaction();
            string deliveryProcessing;
            bool hasExpress = false;
            bool hasRegular = false;

            for (int i = 0; i < cart.Count(); i++)
            {
                if ((cart.ElementAt(i).deliveryRate).Equals("regular"))
                {
                    hasRegular = true;
                }

                if ((cart.ElementAt(i).deliveryRate).Equals("express"))
                {
                    hasExpress = true;
                }
            }

            DateTime dateValue = DateTime.Now;
            DateTime dateToday = DateTime.Now;
            temp.dateRequested = dateToday.ToString("yyyy-MM-ddHH:mm:ss");
            if (hasExpress && hasRegular)
            {
                deliveryProcessing = "mutant";
                temp.dateDue = dateValue.AddDays(3).ToString("yyyy-MM-ddHH:mm:ss");
                temp.estimatedDeliveryDate = temp.dateDue;
            }
            else if (hasExpress)
            {
                deliveryProcessing = "express";
                temp.dateDue = dateValue.AddDays(3).ToString("yyyy-MM-ddHH:mm:ss");
                temp.estimatedDeliveryDate = temp.dateDue;
            }
            else
            {
                deliveryProcessing = "regular";
                temp.dateDue = dateValue.AddDays(6).ToString("yyyy-MM-ddHH:mm:ss");
                temp.estimatedDeliveryDate = temp.dateDue;
            }

            temp.userID = userID;
            temp.price = price;
            temp.mailingID = mailingID;
            temp.deliveryProcessing = deliveryProcessing;
          

            transactionManager tManager = new transactionManager();
            tManager.saveTransaction(temp);
            //Vars to Access DB
            orderManager orDB = new orderManager();

            for (int i = 0; i < cart.Count(); i++)
            {
                orDB.saveOrder(cart.ElementAt(i), tManager.getTransaction(userID).ElementAt(0).transactionID);
            }
        }
    }


}