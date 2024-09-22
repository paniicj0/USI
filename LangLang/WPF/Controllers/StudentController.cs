using LangLang.Model;
using LangLang.Repository;
using LangLang.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Controllers
{
    public class StudentController
    {
        private StudentService studentService;

        public StudentController()
        {
            studentService = StudentService.GetInstance();
        }

        public List<Student> GetAll()
        {
            return studentService.GetAll();
        }

        public Student GetById(int id)
        {
            return studentService.GetById(id);
        }

        public List<Student> Create(Student student)
        {
            return studentService.Create(student);
        }

        public void Update(Student student)
        {
            studentService.Update(student);
        }

        public void Delete(int id)
        {
            studentService.Delete(id);
        }

        public List<Student> LoadFromFile()
        {
            return studentService.LoadFromFile();
        }

        public Student ConvertToStudent(object userObject)
        {
            return studentService.ConvertToStudent(userObject);
        }

        public List<Student> GetStudentPenaltyPoints()
        {
            return studentService.GetStudentPenaltyPoints();
        }
    }
}