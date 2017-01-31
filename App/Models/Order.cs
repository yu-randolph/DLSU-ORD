using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Data;
using Newtonsoft.Json;

namespace App.Models
{
    public class Order
    {
        [JsonProperty("docuID")]
        public int docuID { get; set; }
        [JsonProperty("docuName")]
        public string docuName { get; set; }
        [JsonProperty("deliveryRate")]
        public string deliveryRate { get; set; }
        [JsonProperty("packaging")]
        public string packaging { get; set; }
        [JsonProperty("quantity")]
        public int quantity { get; set; }
        [JsonProperty("degree")]
        public string degree { get; set; }
        [JsonProperty("price")]
        public float price { get; set; }
    }

    class orderManager
    {
            private DatabaseConnector db = new DatabaseConnector();

            public void saveOrder(Order temp, int transactionID)
            {
                    Order or = temp;
                    MySqlConnection conn = new MySqlConnection(db.getConnString());

                    MySqlDataAdapter adapter = new MySqlDataAdapter();

                    using(conn)
                    {
                            using(adapter)
                            {
                                    adapter.SelectCommand = new MySqlCommand("SELECT * FROM requestdocdb.orders", conn);

                                    adapter.InsertCommand = new MySqlCommand("insert into requestdocdb.orders"
                                                             + " (docuID, transactionID, deliveryRate, packaging, quantity, degree) "
                                                             + "VALUES (@docuID, @transactionID, @deliveryRate, @packaging, @quantity, @degree)", conn);

                                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("docuID", MySqlDbType.Int32, 11, "docuID"));
                                    //adapter.InsertCommand.Parameters.Add(new MySqlParameter("docuName", MySqlDbType.VarChar, 100, "docuName"));
                                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("deliveryRate", MySqlDbType.VarChar, 100, "deliveryRate"));
                                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("packaging", MySqlDbType.VarChar, 100, "packaging"));
                                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("quantity", MySqlDbType.Int32, 11, "quantity"));
                                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("degree", MySqlDbType.VarChar, 100, "degree"));
                                    //adapter.InsertCommand.Parameters.Add(new MySqlParameter("price", MySqlDbType.Float, 4, "price"));
                                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("transactionID", MySqlDbType.Int32, 11, "transactionID"));

                                    using (DataSet dataSet = new DataSet())
                                    {
                                        adapter.Fill(dataSet, "orders");

                                        DataRow newRow = dataSet.Tables[0].NewRow();

                                        newRow["docuID"] = or.docuID;
                                       // newRow["docuName"] = or.docuName;
                                        newRow["deliveryRate"] = or.deliveryRate;
                                        newRow["packaging"] = or.packaging;
                                        newRow["quantity"] = or.quantity;
                                        newRow["degree"] = or.degree;
                             
                                        //newRow["price"] = or.price;
                                       newRow["transactionID"] = transactionID;

                                        dataSet.Tables[0].Rows.Add(newRow);

                                        adapter.Update(dataSet, "orders");
                                    }
                            }
                    }
            }

            //return is list but only return one order
            public List<Order> getOrder(int transactionID)
            {
                    List<Order> listOr = new List<Order>();
                    MySqlConnection conn = null;

                    using (conn = new MySqlConnection(db.getConnString()))
                    {
                        conn.Open();
                        using (MySqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = "SELECT * FROM orders WHERE transactionID LIKE '" + transactionID + "';";

                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Order or = new Models.Order();

                                    or.docuID = reader.GetInt32(1);
                                   // or.docuName = reader.GetString();
                                    or.deliveryRate = reader.GetString(3);
                                    or.packaging = reader.GetString(4);
                                    or.quantity = reader.GetInt32(6);
                                    or.degree = reader.GetString(5);
                                   // or.price = reader.GetInt32(6);

                                    listOr.Add(or);
                                    or = new Models.Order();
                                }

                                if (!reader.HasRows)
                                {
                                    listOr = null;
                                }
                            }
                        }
                    }
                    
                    conn.Close();
                    return listOr;
            }
    }
}