using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using SchoolDbAssignmnet.Models;

namespace SchoolDbAssignmnet.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Teacher
        // Route to Views/Teacher/List.cshtml
        public ActionResult List(string SearchKey=null)
        {
            //method provides list of teachers

            List<Teacher> Teachers = new List<Teacher>();

            // use teacher data controller

            TeacherDataController Controller = new TeacherDataController();

            Teachers = (List<Teacher>)Controller.ListTeachers(SearchKey);


            return View(Teachers);
        }

        // Get: /Teacher/Show/{TeacherId}

        public ActionResult Show(int id)
        {
            TeacherDataController Controller = new TeacherDataController();

            // getting teacher info from database
            Teacher NewTeacher = Controller.FindTeacher(id);

            return View(NewTeacher);
        }

        //GET: /Teacher/DeleteConfirm/{id}
        public ActionResult DeleteConfirm(int id)
        {
            TeacherDataController Controller = new TeacherDataController();

            // getting teacher info from database
            Teacher NewTeacher = Controller.FindTeacher(id);

            return View(NewTeacher);
        }

        //POST: /Teacher/Delete/{id}
        [HttpPost]
        public ActionResult Delete(int id)
        {
            TeacherDataController Controller = new TeacherDataController();
            Controller.DeleteTeacher(id);
            return RedirectToAction("List");
        }

        //GET: /Author/New
        public ActionResult New()
        {
            return View();
        }

        //POST: /Teacher/Create
        [HttpPost]
        public ActionResult Create(string TeacherFname, string TeacherLname, string EmpNumber, decimal Salary)
        {
           

            Debug.WriteLine(TeacherFname);
            Debug.WriteLine(TeacherLname);
            Debug.WriteLine(EmpNumber);
            Debug.WriteLine(Salary);

            Teacher NewTeacher = new Teacher();
            NewTeacher.TeacherFname = TeacherFname;
            NewTeacher.TeacherLname = TeacherLname;
            NewTeacher.EmpNumber = EmpNumber;
            NewTeacher.Salary = Salary;

            TeacherDataController controller = new TeacherDataController();
            controller.AddTeacher(NewTeacher);

            return RedirectToAction("List");
        }
    }
}