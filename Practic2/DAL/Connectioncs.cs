using Microsoft.Data.SqlClient;
using Practic2.Models;
using System.Data;

namespace Practic2.DAL
{
    public class Connectioncs
    {
      private readonly string _connectionString;
        public Connectioncs(string connectionString)
        {
            _connectionString = connectionString;
        }
        private SqlConnection con;
        private void connection()
        {
            con = new SqlConnection(_connectionString);
        }
        public List<StudentViewModel> GetStudents()
        {
            connection();
            List<StudentViewModel> list = new List<StudentViewModel>();
            SqlCommand cmd = new SqlCommand("GetAllData", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            SqlDataAdapter dt = new SqlDataAdapter();
            dt.SelectCommand = cmd;
           DataSet st = new DataSet();
            con.Open();
            dt.Fill(st);
            con.Close();
            if (st.Tables[0].Rows.Count > 0 )
            {
                foreach (DataRow dtr in st.Tables[0].Rows) 
                {
                    list.Add(new StudentViewModel
                    {
                        StudentID = Convert.ToInt32(dtr["StudentID"]),
                        StudentName = Convert.ToString(dtr["StudentName"]),
                        Email = Convert.ToString(dtr["Email"]),
                        Password = Convert.ToString(dtr["Password"]),
                        Phone = Convert.ToString(dtr["Phone"]),
                        IsAdmin = Convert.ToBoolean(dtr["IsAdmin"])
                    });
                }
            }
            if (st.Tables[1].Rows.Count > 0)
            {
                list.ForEach(dr =>
                {
                    var row = st.Tables[1].AsEnumerable().Where(obj => obj.Field<int>("StudentID")
                    == dr.StudentID).ToList();
                    dr.BookList = new List<BookViewModel>();
                    row.ForEach(dtr =>
                    {
                        dr.BookList.Add(new BookViewModel
                        {
                            BookID = Convert.ToInt32(dtr["BookID"]),
                            BookName = Convert.ToString(dtr["BookName"]),
                        });
                    });
                });
            }
            return list;
        }
        public bool AddStudents(StudentViewModel st)
        {
      
            var selectedList = st.BookList.Where(obj => obj.IsSelected == true).Select(o => o.BookID).ToList();
            var BookList = string.Join(",", selectedList).ToString();
            connection();
            SqlCommand cmd = new SqlCommand(" AddDetails ",  con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("StudentID" , st.StudentID);
            cmd.Parameters.AddWithValue("StudentName" , st.StudentName);
            cmd.Parameters.AddWithValue("Email" , st.Email);
            cmd.Parameters.AddWithValue("Password" , st.Password);
            cmd.Parameters.AddWithValue("Phone" , st.Phone);
            cmd.Parameters.AddWithValue("bookIDs" , BookList);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i > 0) return true;
            else return false;
        }
        public bool Delete( int id)
        {
            connection();
            SqlCommand cmd = new SqlCommand("DeleteStudent", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("StudentID", id);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i > 0) return true;
            else return false;
        }
        public List<BookViewModel> GetBookList()
        {
            connection();
            SqlCommand cmd = new SqlCommand("GetAllBooks", con);
            List<BookViewModel> books = new List<BookViewModel>(); 
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                books.Add(new BookViewModel
                {
                    BookID    = (int)dr["BookID"],
                    BookName  = dr["BookName"].ToString(),
                });
               
            }
            return books;
        }
    }
}
