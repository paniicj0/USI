using LangLang.Model;
using LangLang.ModelEnum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LangLang.Repository
{
    public class StudentExamCancellationRepository
    {
        private static StudentExamCancellationRepository instance = null;
        private List<StudentExamCancellation> cancellations;
        public event EventHandler AppliedForExam;
        public StudentExamCancellationRepository()
        {
            cancellations = new List<StudentExamCancellation>();
            cancellations = LoadFromFile();
        }

        public static StudentExamCancellationRepository GetInstance()
        {
            if (instance == null)
            {
                instance = new StudentExamCancellationRepository();
            }
                return instance;
        }

        public List<StudentExamCancellation> GetAll()
        { 
            return cancellations;
        }

        public StudentExamCancellation GetById(int id)
        {
            foreach (StudentExamCancellation cancelation in cancellations) 
            {
                if(cancelation.Id == id)
                {
                    return cancelation;
                }
            }
            return null;
        }

        public List<StudentExamCancellation> GetByStudentId(int id)
        {
            List<StudentExamCancellation> deleteCancellations = new List<StudentExamCancellation>();
            foreach (StudentExamCancellation cancelation in cancellations)
            {
                if (cancelation.StudentId == id)
                {
                    deleteCancellations.Add(cancelation);
                }
            }
            return deleteCancellations;
        }

        public StudentExamCancellation GetByExamId(int id)
        {
            foreach (StudentExamCancellation cancelation in cancellations)
            {
                if (cancelation.ExamId == id)
                {
                    return cancelation;
                }
            }
            return null;
        }

        private int GenerateId()
        {
            int maxId = 0;
            foreach (StudentExamCancellation cancellation in cancellations)
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
                StreamWriter file = new StreamWriter("../../../WPF/Data/StudentExamCancellationFile.csv", false);

                foreach (StudentExamCancellation cancellation in cancellations)
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
            StudentExamCancellation cancellation = GetById(id);
            if (cancellation == null)
            {
                return;
            }

            cancellations.Remove(cancellation);
            Save();
        }

        public void DeleteByStudentId(int id)
        {
            List<StudentExamCancellation> deleteCancellations = GetByStudentId(id);
            foreach (StudentExamCancellation deleteCancellation in deleteCancellations)
            {
                cancellations.Remove(deleteCancellation);
            }
            Save();
        }

        public void Update(StudentExamCancellation cancellation)
        {
            StudentExamCancellation oldCancellation = GetById(cancellation.Id);
            if (oldCancellation == null)
            {
                return;
            }

            oldCancellation.Applied = cancellation.Applied;
            Save();
        }

        public StudentExamCancellation Create(StudentExamCancellation cancellation)
        {
            cancellation.Id = GenerateId();
            cancellations.Add(cancellation);
            Save();
            AppliedForExam?.Invoke(this, EventArgs.Empty);
            return cancellation;
        }

        public List<StudentExamCancellation> LoadFromFile()
        {

            string filename = "../../../WPF/Data/StudentExamCancellationFile.csv";

            using (StreamReader reader = new StreamReader(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] tokens = line.Split('|');
                    if (tokens.Length < 3)
                    {
                        continue;
                    }

                    StudentExamCancellation cancellation = new StudentExamCancellation(
                        id: Int32.Parse(tokens[0]),
                        examId: Int32.Parse(tokens[1]),
                        studentId: Int32.Parse(tokens[2]),
                        applied: bool.Parse(tokens[3])
                        );

                    cancellations.Add(cancellation);
                }
            }
            return cancellations;


        }
    }
}
