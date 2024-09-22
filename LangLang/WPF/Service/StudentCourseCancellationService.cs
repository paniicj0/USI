using LangLang.Model;
using LangLang.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LangLang.Service
{
    internal class StudentCourseCancellationService
    {
        private static StudentCourseCancellationService instance = null;
        private StudentCourseCancellationRepository studentCourseCancellationRepository;
        private List<StudentCourseCancellation> cancellations;

        private StudentCourseCancellationService()
        {
            studentCourseCancellationRepository = StudentCourseCancellationRepository.GetInstance();
            this.cancellations = studentCourseCancellationRepository.GetAll();
        }

        public static StudentCourseCancellationService GetInstance()
        {
            if (instance == null)
            {
                instance = new StudentCourseCancellationService();
            }
            return instance;
        }

        public StudentCourseCancellation GetById(int id)
        {
            return studentCourseCancellationRepository.GetById(id);
        }

        public StudentCourseCancellation GetByCourseId(int id)
        {
            return studentCourseCancellationRepository.GetByCourseId(id);
        }

        public List<StudentCourseCancellation> GetByStudentId(int id)
        {
            return studentCourseCancellationRepository.GetByStudentId(id);
        }

        public List<StudentCourseCancellation> GetAll()
        {
            return studentCourseCancellationRepository.GetAll();
        }

        public StudentCourseCancellation Create(StudentCourseCancellation cancellation)
        {
            return studentCourseCancellationRepository.Create(cancellation);
        }

        public void Update(StudentCourseCancellation cancellation)
        {
            studentCourseCancellationRepository.Update(cancellation);
        }

        public void Delete(int id)
        {
            studentCourseCancellationRepository.Delete(id);
        }

        public void DeleteByStudentId(int id)
        {
            studentCourseCancellationRepository.DeleteByStudentId(id);
        }

        public List<StudentCourseCancellation> LoadFromFile()
        {
            return studentCourseCancellationRepository.LoadFromFile();
        }

        public bool IsCancellationSent(int courseId, int studentId)
        {
            foreach (StudentCourseCancellation cancellation in cancellations)
            {
                if (cancellation.IdCourse == courseId && cancellation.IdStudent == studentId)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
