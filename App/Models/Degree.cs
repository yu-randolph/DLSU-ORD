using App.Controllers;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace App.Models
{
    public class Degree
    {
        public int degreeID { get; set; }
        public string degreeName { get; set; }
        public string level { get; set; }
        public int yearAdmitted { get; set; }
        public string campusAttended { get; set; }
        public string admittedAs { get; set; }
        public string graduated { get; set; }
        public int yearLevel { get; set; }
        public int userID { get; set; }
        public string lastSchoolAttendedPrevDlsu { get; set; }
        public int graduatedYear { get; set; }
        public int graduatedMonth { get; set; }
        public int term { get; set; }
        public string academicYear { get; set; }

    }

    class degreeManager
    {
        private DatabaseConnector db = new DatabaseConnector();

        public void saveDegree(Degree deg)
        {
            Degree degree = deg;
            MySqlConnection conn = new MySqlConnection(db.getConnString());

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            using (conn)
            {
                using (adapter)
                {
                    adapter.SelectCommand = new MySqlCommand("SELECT * FROM requestdocdb.degreesofuser", conn);

                    adapter.InsertCommand = new MySqlCommand("insert into requestdocdb.degreesofuser"
                                                             + " (degreeName, level, yearAdmitted, campusAttended, admittedAs, graduated,"
                                                             + " yearLevel, userID, lastSchoolAttendedPrevDlsu, graduatedYear, graduatedMonth, term, academicYear) "
                                                             + "VALUES (@degreeName, @level, @yearAdmitted, @campusAttended, @admittedAs, @graduated, "
                                                             + "@yearLevel, @userID, @lastSchoolAttendedPrevDlsu, @graduatedYear, @graduatedMonth, @term, @academicYear)", conn);

                    
                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("degreeName", MySqlDbType.VarChar, 50, "degreeName"));
                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("level", MySqlDbType.VarChar, 50, "level"));
                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("yearAdmitted", MySqlDbType.Int32, 11, "yearAdmitted"));
                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("campusAttended", MySqlDbType.VarChar, 200, "campusAttended"));
                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("admittedAs", MySqlDbType.VarChar, 50, "admittedAs"));
                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("graduated", MySqlDbType.VarChar, 3, "graduated"));
                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("yearLevel", MySqlDbType.Int32, 11, "yearLevel"));
                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("userID", MySqlDbType.Int32, 11, "userID"));
                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("lastSchoolAttendedPrevDlsu", MySqlDbType.VarChar, 100, "lastSchoolAttendedPrevDlsu"));
                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("graduatedYear", MySqlDbType.Int32, 11, "graduatedYear"));
                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("graduatedMonth", MySqlDbType.Int32, 11, "graduatedMonth"));
                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("term", MySqlDbType.Int32, 11, "term"));
                    adapter.InsertCommand.Parameters.Add(new MySqlParameter("academicYear", MySqlDbType.VarChar, 9, "academicYear"));

                    using (DataSet dataSet = new DataSet())
                    {
                        adapter.Fill(dataSet, "degreesofuser");

                        DataRow newRow = dataSet.Tables[0].NewRow();
                        
                        newRow["degreeName"] = deg.degreeName;
                        newRow["level"] = deg.level;
                        newRow["yearAdmitted"] = deg.yearAdmitted;
                        newRow["campusAttended"] = deg.campusAttended;
                        newRow["admittedAs"] = deg.admittedAs;
                        newRow["graduated"] = deg.graduated;

                        if (!(deg.lastSchoolAttendedPrevDlsu == null))
                        {
                            newRow["lastSchoolAttendedPrevDlsu"] = deg.lastSchoolAttendedPrevDlsu;
                        }

                        newRow["userID"] = deg.userID;

                        if (!(deg.graduatedYear == 0))
                        {
                            newRow["graduatedYear"] = deg.graduatedYear;
                        }

                        if (!(deg.graduatedMonth == 0))
                        {
                            newRow["graduatedMonth"] = deg.graduatedMonth;
                        }

                        if (!(deg.term == 0))
                        {
                            newRow["term"] = deg.term;
                        }

                        if (!(deg.academicYear == null))
                        {
                            newRow["academicYear"] = deg.academicYear;
                        }

                        dataSet.Tables[0].Rows.Add(newRow);

                        adapter.Update(dataSet, "degreesofuser");
                    }
                }
            }
            conn.Close();
        }

        public List<Degree> getDegree(int userID)
        {
            List<Degree> listDeg = new List<Degree>();
            MySqlConnection conn = null;

            using (conn = new MySqlConnection(db.getConnString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM degreesofuser WHERE userID LIKE '" + userID + "';";
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Degree degree = new Models.Degree();
                            degree.degreeID = reader.GetInt32(0);
                            degree.degreeName = reader.GetString(1);

                            if (!reader.IsDBNull(2))
                            {
                                degree.level = reader.GetString(2);
                            }
                            else degree.level = "";


                            degree.yearAdmitted = reader.GetInt32(3);
                            degree.campusAttended = reader.GetString(4);
                            degree.admittedAs = reader.GetString(5);
                            degree.graduated = reader.GetString(6);

                            if (!reader.IsDBNull(7))
                            {
                                degree.yearLevel = reader.GetInt32(7);
                            }
                            else degree.yearLevel = 0;

                            degree.userID = reader.GetInt32(8);
                            degree.lastSchoolAttendedPrevDlsu = reader.GetString(9);

                            if (!reader.IsDBNull(10))
                            {
                                degree.graduatedYear = reader.GetInt32(10);
                            }
                            else degree.graduatedYear = 0;

                            if (!reader.IsDBNull(11))
                            {
                                degree.graduatedMonth = reader.GetInt32(11);
                            } else degree.graduatedMonth = 0;

                            if (!reader.IsDBNull(12))
                            {
                                degree.term = reader.GetInt32(12);
                            }
                            else degree.term = 0;

                            if (!reader.IsDBNull(13))
                            {
                                degree.academicYear = reader.GetString(13);
                            }
                            else degree.academicYear = "";

                            listDeg.Add(degree);
                            degree = new Models.Degree();
                        }

                        if (!reader.HasRows)
                        {
                            listDeg = null;
                        }
                    }
                }
            }
            
            conn.Close();
            return listDeg;
        }

    }

}