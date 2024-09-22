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
    class AppliedExamController
    {
        private AppliedExamService appliedExamService;

        public AppliedExamController()
        {
            appliedExamService = AppliedExamService.GetInstance();
        }

        public List<AppliedExam> GetAll()
        {
            return appliedExamService.GetAll();
        }

        public AppliedExam GetById(int id)
        {
            return appliedExamService.GetById(id);
        }

        public AppliedExam GetByExamId(int id)
        {
            return appliedExamService.GetByExamId(id);
        }

        public AppliedExam GetByStudentId(int id)
        {
            return appliedExamService.GetByStudentId(id);
        }

        public AppliedExam Create(AppliedExam appliedExam)
        {
            return appliedExamService.Create(appliedExam);
        }

        public void Update(AppliedExam appliedExam)
        {
            appliedExamService.Update(appliedExam);
        }
        public void Delete(int id)
        {
            appliedExamService.Delete(id);
        }

        public List<AppliedExam> LoadFromFile()
        {
            return appliedExamService.LoadFromFile();
        }

        public bool BanStudent(int studentId)
        {
            return appliedExamService.BanStudent(studentId);
        }

        public List<Student> GetBestStudentsKnowledge(int courseId)
        {
            return appliedExamService.GetBestStudentsKnowledge(courseId);
        }

        public List<Student> GetBestStudentsActivity(int courseId)
        {
            return appliedExamService.GetBestStudentsActivity(courseId);
        }
    }
}