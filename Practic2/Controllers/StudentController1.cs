using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data.SqlClient;
using Practic2.Models;
using Practic2.DAL;
using System.Reflection;

namespace Practic2.Controllers
{
    public class StudentController1 : Controller
    {

        public Connectioncs? con { get; private set; }
        public IConfiguration _con { get; private set; }
        public StudentController1(IConfiguration configuration)
        {
            _con = configuration;
            con = new Connectioncs(_con["ConnectionStrings:DefaultConnection"].ToString());
        }
        
        public IActionResult Index()
        {
            ModelState.Clear();
            return View(con.GetStudents());
        }
        [HttpGet]
        public IActionResult Create()
        {
            var model = new StudentViewModel()
            {
                BookList = con.GetBookList()

             };
        return View(model);
        }
        [HttpPost]
        public IActionResult Create(StudentViewModel st)
        {
            if (ModelState.IsValid)
            {
                con.AddStudents(st);
                return RedirectToAction("Index");
            }
            return View(st);
        }
        public IActionResult Delete(int id)
        {
            if (con.Delete(id))
            {
                ViewBag.AlertMsg = "Student Record Is Delete Successfully ";
            }
            return RedirectToAction("Index");

        }
        public IActionResult Details(int id)
        {
            Student student = con.GetStudents().Where(o => o.StudentID == id).FirstOrDefault();
            return View(student);
        }
        [HttpGet]
        public IActionResult Edit(int id ) 
        {

            var bookview = con.GetStudents().Find(Models => Models.StudentID == id);
            var Allbooks = con.GetBookList();
            foreach (var book in Allbooks)
            {
                book.IsSelected = bookview.BookList.Any(b => b.BookID == book.BookID);
            }
            bookview.BookList = Allbooks;

            return View(bookview);
        }
        [HttpPost]
        public IActionResult Edit(StudentViewModel st)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    con.AddStudents(st);
                    return RedirectToAction("Index");
                }
                catch { throw; }
            }
            st.BookList = con.GetBookList();
            return View(st);
        }
    }
}
