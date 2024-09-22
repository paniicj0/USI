using LangLang.Model;
using LangLang.ModelEnum;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Repository
{
    public class StudentCourseRequestRepository
    {
        private static StudentCourseRequestRepository instance = null;
        private List<StudentCourseRequest> requests;

        public event EventHandler StatusUpdate;

        public StudentCourseRequestRepository()
        {
            requests = new List<StudentCourseRequest>();
            requests = LoadFromFile();

        }

        public static StudentCourseRequestRepository GetInstance()
        {
            if (instance == null)
            {
                instance = new StudentCourseRequestRepository();
            }
            return instance;
        }

        public List<StudentCourseRequest> GetAll()
        {
            return new List<StudentCourseRequest>(requests);
        }

        public StudentCourseRequest GetById(int id)
        {
            foreach (StudentCourseRequest request in requests)
            {
                if (request.Id == id)
                {
                    return request;
                }
            }

            return null;
        }

        public StudentCourseRequest GetByCourseId(int id)
        {
            foreach (StudentCourseRequest request in requests)
            {
                if (request.IdCourse == id)
                {
                    return request;
                }
            }

            return null;
        }

        public List<StudentCourseRequest> GetByStudentId(int id)
        {
            List<StudentCourseRequest> deleteRequests = new List<StudentCourseRequest>();
            foreach (StudentCourseRequest request in requests)
            {
                if (request.IdStudent == id)
                {
                    deleteRequests.Add(request);
                }
            }
            return deleteRequests;
        }

        public StudentCourseRequest GetByCourseStudentId(int courseId, int studentId)
        {
            foreach (StudentCourseRequest request in requests)
            {
                if (request.IdStudent == studentId && request.IdCourse == courseId)
                {
                    return request;
                }
            }
            return null;
        }

        private int GenerateId()
        {
            int maxId = 0;
            foreach (StudentCourseRequest request in requests)
            {
                if (request.Id > maxId)
                {
                    maxId = request.Id;
                }
            }

            return maxId + 1;
        }

        public void Save()
        {
            try
            {
                StreamWriter file = new StreamWriter("../../../WPF/Data/StudentCourseRequestFile.csv", false);

                foreach (StudentCourseRequest request in requests)
                {
                    file.WriteLine(request.StringToCsv());
                }

                file.Flush();
                file.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public void Delete(int id)
        {
            StudentCourseRequest request = GetById(id);
            if (request == null)
            {
                return;
            }

            requests.Remove(request);
            Save();
        }

        public void DeleteByStudentId(int id)
        {
            List<StudentCourseRequest> deleteRequests = GetByStudentId(id);
            foreach (StudentCourseRequest deleteRequest in deleteRequests)
            {
                requests.Remove(deleteRequest);
            }
            Save();
        }

        public void Update(StudentCourseRequest request)
        {
            StudentCourseRequest oldRequest = GetById(request.Id);
            if (oldRequest == null)
            {
                return;
            }

            oldRequest.Status = request.Status;
            oldRequest.Message = request.Message;
            Save();
            StatusUpdate?.Invoke(this, EventArgs.Empty);
        }

        public StudentCourseRequest Create(StudentCourseRequest request)
        {
            request.Id = GenerateId();
            requests.Add(request);
            Save();
            
            return request;
        }

        public List<StudentCourseRequest> LoadFromFile()
        {

            string filename = "../../../WPF/Data/StudentCourseRequestFile.csv";

            using (StreamReader reader = new StreamReader(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] tokens = line.Split('|');
                    if (tokens.Length < 4)
                    {
                        continue;
                    }

                    List<StatusEnum.Status> days = new List<StatusEnum.Status>();

                    StudentCourseRequest request = new StudentCourseRequest(
                        id: Int32.Parse(tokens[0]),
                        idCourse: Int32.Parse(tokens[1]),
                        idStudent: Int32.Parse(tokens[2]),
                        status: (StatusEnum.Status)Enum.Parse(typeof(StatusEnum.Status), tokens[3]),
                        message: tokens[4]
                        );

                    requests.Add(request);
                }
            }
            return requests;


        }

        public StatusEnum.Status GetStatusForStudent(int id)
        {
            foreach (var request in requests)
            {
                if (request.Id == id)
                {
                    return request.Status;
                }
            }

            return StatusEnum.Status.Unknown;
        }

    }
}
