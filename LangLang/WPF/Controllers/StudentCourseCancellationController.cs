using LangLang.Model;
using LangLang.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Controllers
{
    public class StudentCourseCancellationController
    {
        private StudentCourseCancellationService studentCourseCancellationService;

        public StudentCourseCancellationController()
        {
            studentCourseCancellationService = StudentCourseCancellationService.GetInstance();
        }

        public List<StudentCourseCancellation> GetAll()
        {
            return studentCourseCancellationService.GetAll();
        }

        public StudentCourseCancellation GetById(int id)
        {
            return studentCourseCancellationService.GetById(id);
        }

        public StudentCourseCancellation GetByCourseId(int id)
        {
            return studentCourseCancellationService.GetByCourseId(id);
        }

        public List<StudentCourseCancellation> GetByStudentId(int id)
        {
            return studentCourseCancellationService.GetByStudentId(id);
        }

        public StudentCourseCancellation Create(StudentCourseCancellation cancellation)
        {
            return studentCourseCancellationService.Create(cancellation);
        }

        public void Update(StudentCourseCancellation cancellation)
        {
            studentCourseCancellationService.Update(cancellation);
        }

        public void Delete(int id)
        {
            studentCourseCancellationService.Delete(id);
        }

        public void DeleteByStudentId(int id)
        {
            studentCourseCancellationService.DeleteByStudentId(id);
        }

        public List<StudentCourseCancellation> LoadFromFile()
        {
            return studentCourseCancellationService.LoadFromFile();
        }

        public bool IsCancellationSent(int courseId, int studentId)
        {
            return studentCourseCancellationService.IsCancellationSent(courseId, studentId);
        }
    }
}
