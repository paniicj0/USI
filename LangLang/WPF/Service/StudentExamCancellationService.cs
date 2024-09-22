using LangLang.Model;
using LangLang.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Service
{
    internal class StudentExamCancellationService
    {
        private static StudentExamCancellationService instance = null;
        private StudentExamCancellationRepository studentExamCancellationRepository;

        private StudentExamCancellationService()
        {
            studentExamCancellationRepository = StudentExamCancellationRepository.GetInstance();
        }

        public static StudentExamCancellationService GetInstance()
        {
            if (instance == null)
            {
                instance = new StudentExamCancellationService();
            }
            return instance;
        }

        public StudentExamCancellation GetById(int id)
        {
            return studentExamCancellationRepository.GetById(id);
        }

        public StudentExamCancellation GetByExamId(int id)
        {
            return studentExamCancellationRepository.GetByExamId(id);
        }

        public List<StudentExamCancellation> GetByStudentId(int id)
        {
            return studentExamCancellationRepository.GetByStudentId(id);
        }

        public List<StudentExamCancellation> GetAll()
        {
            return studentExamCancellationRepository.GetAll();
        }

        public StudentExamCancellation Create(StudentExamCancellation cancellation)
        {
            return studentExamCancellationRepository.Create(cancellation);
        }

        public void Update(StudentExamCancellation cancellation)
        {
            studentExamCancellationRepository.Update(cancellation);
        }

        public void Delete(int id)
        {
            studentExamCancellationRepository.Delete(id);
        }

        public void DeleteByStudentId(int id)
        {
            studentExamCancellationRepository.DeleteByStudentId(id);
        }

        public List<StudentExamCancellation> LoadFromFile()
        {
            return studentExamCancellationRepository.LoadFromFile();
        }
    }
}
