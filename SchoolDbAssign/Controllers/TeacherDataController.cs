using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SchoolDbAssignmnet.Models;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace SchoolDbAssignmnet.Controllers
{
    public class TeacherDataController : ApiController
    {
        private SchoolDbContext School = new SchoolDbContext();

        /// <summary>
        /// List of Teachers in the Database
        /// </summary>
        /// <example>
        /// GET api/TeacherData/ListTeachers -->
        /// {{"TeacherId":"1","TeacherFname":  

        /// </example>
        /// <returns>
        /// Returns a list of Teachers
        /// </returns>

        [HttpGet]
        [Route("api/teacherdata/listteachers/{SearchKey?}")]
     
        public IEnumerable<Teacher> ListTeachers(string SearchKey=null)
        {
            //Create a connection with database
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection
            Conn.Open();

            //Establish a command for db
            MySqlCommand cmd = Conn.CreateCommand();

            //Query
            cmd.CommandText = "Select * from teachers where lower(teacherfname) like  lower(@key) or lower(teacherlname) like lower(@key) or concat(teacherfname,' ',teacherlname) like lower(@key)";
            cmd.Parameters.AddWithValue("key", "%"+SearchKey+"%");
            cmd.Prepare();

            //Store the result in a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create a list to store Tecaher names
            List<Teacher> Teachers = new List<Teacher>{};

            //Loop till the end of the list
            while (ResultSet.Read())
            {
                //Access columns 
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                string TeacherName = ResultSet["teacherfname"]+" " + ResultSet["teacherlname"];
               decimal Salary = Convert.ToDecimal(ResultSet["salary"]);
                int TecaherId = Convert.ToInt32(ResultSet["teacherid"]);
             //   DateTime HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                string EmpNumber = ResultSet["employeenumber"].ToString();
             //  string ClassName = ResultSet["classname"].ToString();

                Teacher NewTeacher = new Teacher();

                NewTeacher.TeacherId = TecaherId;
                NewTeacher.TeacherName = TeacherName;
               NewTeacher.Salary = Salary;
               // NewTeacher.HireDate = HireDate;
                NewTeacher.EmpNumber = EmpNumber;
              // NewTeacher.ClassName = ClassName;


                //Add the teacher name to the list
                Teachers.Add(NewTeacher);
            }

            //Close the connection between Database and server
            Conn.Close();

            //Return the list of teacher names
            return Teachers;
        }
     

  
        ///<summary>
        ///Finidng a Teacher through an ID
        ///</summary>
        ///<param name="id"> The teacher id </param>
        ///<return>
        /// Returns the Teacher object
        /// </return>>

        [HttpGet]
        [Route("api/teacherdata/findteacher/{teacherid}")]
        public Teacher FindTeacher(int TeacherId)
        {
            Teacher NewTeacher = new Teacher();

            //Creating connection instance
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between dtatabase and the server
            Conn.Open();

            //Establish a command for database
            MySqlCommand cmd = Conn.CreateCommand();

            //Query
            cmd.CommandText = "SELECT * FROM teachers LEFT JOIN classes ON teachers.teacherid = classes.teacherid WHERE teachers.teacherid = @TeacherId;";

            cmd.Parameters.AddWithValue("@TeacherId", TeacherId);
            cmd.Prepare();
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while(ResultSet.Read()) 
            {
                //Access columns 
                NewTeacher.TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                NewTeacher.TeacherFname = ResultSet["teacherfname"].ToString();
                NewTeacher.TeacherLname = ResultSet["teacherlname"].ToString();
                NewTeacher.TeacherName = ResultSet["teacherfname"] + " " + ResultSet["teacherlname"]; 
                NewTeacher.Salary = Convert.ToDecimal(ResultSet["salary"]);
                NewTeacher.HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                NewTeacher.EmpNumber = ResultSet["employeenumber"].ToString();
                //initiative results course name specified for the teacher
                NewTeacher.ClassName = ResultSet["classname"].ToString();


            }
            // Closing server and database connection
            Conn.Close();
            return NewTeacher;

        }


        


        /// <summary>
        /// Delete the selected teacher 
        /// </summary>
        /// <param name="id"></param>
        /// <example>
        /// POST: /api/TeacherData/DeleteTeacher/7
        /// </example>

        [HttpPost]
        public void DeleteTeacher(int id)
        {
            //Creating connection instance
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between dtatabase and the server
            Conn.Open();

            //Establish a command for database
            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "Delete from teachers where teacherid = @id;";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();
        }

        /// <summary>
        /// Adds an author to the database
        /// </summary>
        /// <param name="NewTeacher"> An object with fields that map to the columns of teacher table</param>
        /// <example>
        /// POST : api/TeacherData/AddTeacher
        /// 
        /// {
        /// Firstname : "Sravani"
        /// LastName : "Singani"
        /// Employee Number : T111
        /// Salary: 77.70
        /// }
        /// </example>

        [HttpPost]
        public void AddTeacher([FromBody]Teacher NewTeacher)
        {
            //Creating connection instance
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between dtatabase and the server
            Conn.Open();

            //Establish a command for database
            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "Insert into teachers (teacherfname, teacherlname, employeenumber, hiredate, salary) Values (@TeacherFname, @TeacherLname, @EmpNumber, CURRENT_DATE(), @Salary);";

            if (NewTeacher != null)
            {
                cmd.Parameters.AddWithValue("@TeacherFname", NewTeacher.TeacherFname);
                cmd.Parameters.AddWithValue("@TeacherLname", NewTeacher.TeacherLname);
                cmd.Parameters.AddWithValue("@EmpNumber", NewTeacher.EmpNumber);
                cmd.Parameters.AddWithValue("@Salary", NewTeacher.Salary);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            else
            {
                Debug.WriteLine("There is an error");
                // Handles when NewTeacher is null.
                
            }
          

           

            Conn.Close();
        }
    }
}