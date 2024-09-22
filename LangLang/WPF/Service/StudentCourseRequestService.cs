using LangLang.Model;
using LangLang.ModelEnum;
using LangLang.Repository;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Service
{
    internal class StudentCourseRequestService
    {
        private static StudentCourseRequestService instance = null;
        private StudentCourseRequestRepository studentCourseRequestRepository;
        private List<StudentCourseRequest> requests;

        private StudentCourseRequestService()
        {
            studentCourseRequestRepository = StudentCourseRequestRepository.GetInstance();
            this.requests = studentCourseRequestRepository.GetAll();
        }

        public static StudentCourseRequestService GetInstance()
        {
            if (instance == null)
            {
                instance = new StudentCourseRequestService();
            }
            return instance;
        }

        public StudentCourseRequest GetById(int id)
        {
            return studentCourseRequestRepository.GetById(id);
        }

        public StudentCourseRequest GetByCourseId(int id)
        {
            return studentCourseRequestRepository.GetByCourseId(id);
        }

        public StudentCourseRequest GetByCourseStudentId(int courseId, int studentId)
        {
            return studentCourseRequestRepository.GetByCourseStudentId(courseId, studentId);
        }

        public List<StudentCourseRequest> GetByStudentId(int id)
        {
            return studentCourseRequestRepository.GetByStudentId(id);
        }

        public List<StudentCourseRequest> GetAll()
        {
            return studentCourseRequestRepository.GetAll();
        }

        public StudentCourseRequest Create(StudentCourseRequest request)
        {
            return studentCourseRequestRepository.Create(request);
        }

        public void Update(StudentCourseRequest request)
        {
            studentCourseRequestRepository.Update(request);
        }

        public void Delete(int id)
        {
            studentCourseRequestRepository.Delete(id);
        }

        public void DeleteByStudentId(int id)
        {
            studentCourseRequestRepository.DeleteByStudentId(id);
        }

        public List<StudentCourseRequest> LoadFromFile()
        {
            return studentCourseRequestRepository.LoadFromFile();
        }

        public StatusEnum.Status GetStatusForStudent(int studentId)
        {
            return studentCourseRequestRepository.GetStatusForStudent(studentId);
        }

        public bool IsAlreadyApplied(int courseId, int studentId)
        {
            bool alreadyApplied = false;
            foreach (StudentCourseRequest request in requests)
            {
                if (courseId == request.IdCourse && studentId == request.IdStudent)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
