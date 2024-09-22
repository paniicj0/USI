using LangLang.Model;
using LangLang.ModelEnum;
using LangLang.Repository;
using LangLang.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Controllers
{
    public class StudentCourseRequestController
    {

        private StudentCourseRequestService studentCourseRequestService;

        public StudentCourseRequestController()
        {
            studentCourseRequestService = StudentCourseRequestService.GetInstance();
        }

        public List<StudentCourseRequest> GetAll()
        {
            return studentCourseRequestService.GetAll();
        }

        public StudentCourseRequest GetById(int id)
        {
            return studentCourseRequestService.GetById(id);
        }

        public StudentCourseRequest GetByCourseId(int id)
        {
            return studentCourseRequestService.GetByCourseId(id);
        }

        public List<StudentCourseRequest> GetByStudentId(int id)
        {
            return studentCourseRequestService.GetByStudentId(id);
        }

        public StudentCourseRequest GetByCourseStudentId(int idCourse, int idStudent)
        {
            return studentCourseRequestService.GetByCourseStudentId(idCourse, idStudent);
        }

        public StudentCourseRequest Create(StudentCourseRequest request)
        {
            return studentCourseRequestService.Create(request);
        }

        public void Update(StudentCourseRequest request)
        {
            studentCourseRequestService.Update(request);
        }

        public void Delete(int id)
        {
            studentCourseRequestService.Delete(id);
        }

        public void DeleteByStudentId(int id)
        {
            studentCourseRequestService.DeleteByStudentId(id);
        }

        public List<StudentCourseRequest> LoadFromFile()
        {
            return studentCourseRequestService.LoadFromFile();
        }

        public StatusEnum.Status GetStatusForStudent(int studentId)
        {
            return studentCourseRequestService.GetStatusForStudent(studentId);
        }

        public bool IsAlreadyApplied(int courseId, int studentId)
        {
            return studentCourseRequestService.IsAlreadyApplied(courseId, studentId);
        }
    }
}
