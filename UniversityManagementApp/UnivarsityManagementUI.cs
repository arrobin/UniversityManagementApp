using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniversityManagementApp
{
    public partial class UnivarsityManagementUI : Form
    {
        public UnivarsityManagementUI()
        {
            InitializeComponent();
        }

        static string connectionstring = ConfigurationManager.ConnectionStrings["UniversityManagement"].ConnectionString;
        SqlConnection connection = new SqlConnection(connectionstring);
        
        private void saveButton_Click(object sender, EventArgs e)
        {
            string name = nameTextBox.Text;
            string regNo = regNoTextBox.Text;
            string address = addressTextBox.Text;

            if (isRegExist(regNo))
            {
                MessageBox.Show("This Registration Number already exist");
            }

            else
            {
                //1.connet to database-i.e. server,database,authentication(connection string)

                string connectionstring = ConfigurationManager.ConnectionStrings["UniversityManagement"].ConnectionString;
                SqlConnection connection = new SqlConnection(connectionstring);

                connection.Open();
                //2. write query
                string query = "INSERT INTO t_student VALUES('" + name + "','" + regNo + "','" + address + "')";

                //3. execute query

                SqlCommand command = new SqlCommand(query, connection);


                int rowAffected = command.ExecuteNonQuery();

                connection.Close();

                //4. see result

                if (rowAffected > 0)
                {
                    MessageBox.Show("Data Saved Successfully");
                }
                else
                {
                    MessageBox.Show("Faild");
                }    
            }
            
        }


        public bool isRegExist(string regNo)
        {
            string connectionstring = ConfigurationManager.ConnectionStrings["UniversityManagement"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionstring);

            connection.Open();
            string query = string.Format("SELECT * FROM t_student WHERE regno='{0}'", regNo);

            SqlCommand aCommand = new SqlCommand(query, connection);
            SqlDataReader aReader = aCommand.ExecuteReader();
            bool msg = aReader.HasRows;
            connection.Close();
            return msg;
        }

        private void showButton_Click(object sender, EventArgs e)
        {
            connection.Open();
            string query = string.Format("SELECT * FROM t_student");

            SqlCommand aCommand = new SqlCommand(query, connection);
            SqlDataReader aReader = aCommand.ExecuteReader();


            List<Student> studentList=new List<Student>();

            while (aReader.Read())
            {
                Student aStudent = new Student();
                aStudent.id = (int) aReader[0];
                aStudent.name = aReader[1].ToString();
                aStudent.regNo = aReader[2].ToString();
                aStudent.address = aReader[3].ToString();

                studentList.Add(aStudent);
            }

            aReader.Close();
            connection.Close();
            ListViewShow(studentList);
        }

        private void ListViewShow(List<Student> studentList)
        {
            foreach (var student in studentList)
            {
                studentListView.Items.Clear();
                
                ListViewItem item = new ListViewItem(student.id.ToString());
                item.SubItems.Add(student.name);
                item.SubItems.Add(student.regNo);
                item.SubItems.Add(student.address);

                studentListView.Items.Add(item);

            }
        }
    }
}
