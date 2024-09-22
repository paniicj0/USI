using LangLang.Model;
using LangLang.ModelEnum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Repository
{
    public class StudentCourseCancellationRepository
    {
        private static StudentCourseCancellationRepository instance = null;
        private List<StudentCourseCancellation> cancellations;

        public event EventHandler StatusUpdate;

        private StudentCourseCancellationRepository()
        {
            cancellations = new List<StudentCourseCancellation>();
            cancellations = LoadFromFile();
        }

        public static StudentCourseCancellationRepository GetInstance()
        {
            if (instance == null)
            {
                instance = new StudentCourseCancellationRepository();
            }
            return instance;
        }

        public List<StudentCourseCancellation> GetAll()
        {
            return new List<StudentCourseCancellation>(cancellations);
        }

        public StudentCourseCancellation GetById(int id)
        {
            foreach (StudentCourseCancellation cancellation in cancellations)
            {
                if (cancellation.Id == id)
                {
                    return cancellation;
                }
            }

            return null;
        }

        public StudentCourseCancellation GetByCourseId(int id)
        {
            foreach (StudentCourseCancellation cancellation in cancellations)
            {
                if (cancellation.IdCourse == id)
                {
                    return cancellation;
                }
            }

            return null;
        }

        public List<StudentCourseCancellation> GetByStudentId(int id)
        {
            List<StudentCourseCancellation> deleteCancellations = new List<StudentCourseCancellation>();
            foreach (StudentCourseCancellation cancellation in cancellations)
            {
                if (cancellation.IdStudent == id)
                {
                    deleteCancellations.Add(cancellation);
                }
            }
            return deleteCancellations;
        }

        private int GenerateId()
        {
            int maxId = 0;
            foreach (StudentCourseCancellation cancellation in cancellations)
            {
                if (cancellation.Id > maxId)
                {
                    maxId = cancellation.Id;
                }
            }

            return maxId + 1;
        }

        public void Save()
        {
            try
            {
                StreamWriter file = new StreamWriter("../../../WPF/Data/StudentCourseCancellationFile.csv", false);

                foreach (StudentCourseCancellation cancellation in cancellations)
                {
                    file.WriteLine(cancellation.StringToCsv());
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
            StudentCourseCancellation cancellation = GetById(id);
            if (cancellation == null)
            {
                return;
            }

            cancellations.Remove(cancellation);
            Save();
        }

        public void DeleteByStudentId(int id)
        {
            List<StudentCourseCancellation> deleteCancellations = GetByStudentId(id);
            foreach (StudentCourseCancellation deleteCancellation in deleteCancellations)
            {
                cancellations.Remove(deleteCancellation);
            }
            Save();
        }

        public void Update(StudentCourseCancellation cancellation)
        {
            StudentCourseCancellation oldCancellation = GetById(cancellation.Id);
            if (oldCancellation == null)
            {
                return;
            }

            oldCancellation.Status = cancellation.Status;
            oldCancellation.Message = cancellation.Message;
            Save();
            StatusUpdate?.Invoke(this, EventArgs.Empty);
        }

        public StudentCourseCancellation Create(StudentCourseCancellation cancellation)
        {
            cancellation.Id = GenerateId();
            cancellations.Add(cancellation);
            Save();

            return cancellation;
        }

        public List<StudentCourseCancellation> LoadFromFile()
        {

            string filename = "../../../WPF/Data/StudentCourseCancellationFile.csv";

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

                    StudentCourseCancellation cancellation = new StudentCourseCancellation(
                        id: Int32.Parse(tokens[0]),
                        idCourse: Int32.Parse(tokens[1]),
                        idStudent: Int32.Parse(tokens[2]),
                        status: (StatusEnum.Status)Enum.Parse(typeof(StatusEnum.Status), tokens[3]),
                        message: tokens[4]
                        );

                    cancellations.Add(cancellation);
                }
            }
            return cancellations;


        }
    }
}
