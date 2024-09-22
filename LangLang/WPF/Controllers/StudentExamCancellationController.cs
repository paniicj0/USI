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
    public class StudentExamCancellationController
    {
        private StudentExamCancellationService studentExamCancellationService;

        public StudentExamCancellationController() { 
            studentExamCancellationService=StudentExamCancellationService.GetInstance();
        }

        public StudentExamCancellation GetById(int id)
        {
            return studentExamCancellationService.GetById(id);
        }

        public StudentExamCancellation GetByExamId(int id)
        {
            return studentExamCancellationService.GetByExamId(id);
        }

        public List<StudentExamCancellation> GetByStudentId(int id)
        {
            return studentExamCancellationService.GetByStudentId(id);
        }

        public List<StudentExamCancellation> GetAll()
        {
            return studentExamCancellationService.GetAll();
        }

        public StudentExamCancellation Create(StudentExamCancellation cancellation)
        {
            return studentExamCancellationService.Create(cancellation);
        }

        public void Update(StudentExamCancellation cancellation)
        {
            studentExamCancellationService.Update(cancellation);
        }

        public void Delete(int id)
        {
            studentExamCancellationService.Delete(id);
        }

        public void DeleteByStudentId(int id)
        {
            studentExamCancellationService.DeleteByStudentId(id);
        }

        public List<StudentExamCancellation> LoadFromFile()
        {
            return studentExamCancellationService.LoadFromFile();
        }
    }
}
