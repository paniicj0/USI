using LangLang.Model;
using LangLang.ModelEnum;
using LangLang.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace LangLang.Service
{
    internal class ExamService
    {
        private static ExamService instance = null;
        private ExamRepository examRepository;

        private ExamService()
        {
            examRepository = ExamRepository.GetInstance();
        }

        public static ExamService GetInstance()
        {
            if (instance == null)
            {
                instance = new ExamService();
            }
            return instance;
        }

        public Exam GetById(int id)
        {
            return examRepository.GetById(id);
        }

        public List<Exam> GetAll()
        {
            return examRepository.GetAll();
        }

        public Exam Create(Exam exam)
        {
            return examRepository.Create(exam);
        }

        public void Update(Exam exam)
        {
            examRepository.Update(exam);
        }

        public void Delete(int id)
        {
            examRepository.Delete(id);
        }

        public void DeleteTutorsExams(int id)
        {
            examRepository.DeleteTutorsExams(id);
        }

        public int SmartChoiceTutorForExam(LanguageEnum.Language language, LanguageLevelEnum.LanguageLevel level, DateTime? dateExam, TimeSpan? time)
        {
           return examRepository.SmartChoiceTutorForExam(language, level, dateExam, time);
        }

    }
}
