using LangLang.Controllers;
using LangLang.Model;
using LangLang.ModelEnum;
using LangLang.Service;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace LangLang.Repository
{
    public class CourseRepository
    {
        private static CourseRepository instance = null;
        private List<Course> courses;

        public event EventHandler CourseAdded;

        private CourseRepository()
        {
            courses = new List<Course>();
            courses = LoadFromFile();

        }

        public static CourseRepository GetInstance()
        {
            if (instance == null)
            {
                instance = new CourseRepository();
            }
            return instance;
        }

        public List<Course> GetAll()
        {
            return new List<Course>(courses);
        }

        public Course GetById(int id)
        {
            foreach (Course course in courses)
            {
                if (course.Id == id)
                {
                    return course;
                }
            }

            return null;
        }

        public Course GetByTutorsId(int id)
        {
            foreach (Course course in courses)
            {
                if (course.TutorId == id)
                {
                    return course;
                }
            }

            return null;
        }

        private int GenerateId()
        {
            int maxId = 0;
            foreach (Course course in courses)
            {
                if (course.Id > maxId)
                {
                    maxId = course.Id;
                }
            }

            return maxId + 1;
        }

        public void Save()
        {
            try
            {
                StreamWriter file = new StreamWriter("../../../WPF/Data/CourseFile.csv", false);

                foreach (Course course in courses)
                {
                    file.WriteLine(course.StringToCsv());
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
            Course course = GetById(id);
            if (course == null)
            {
                return;
            }

            courses.Remove(course);
            Save();
        }

        public void DeleteTutorsCourses(int id)
        {
            Course course = GetByTutorsId(id);
            if (course == null)
            {
                return;
            }

            courses.Remove(course);
            Save();
        }

        public void Update(Course course)
        {
            Course oldCourse = GetById(course.Id);
            if (oldCourse == null)
            {
                return;
            }

            oldCourse.Language = course.Language;
            oldCourse.LanguageLevel = course.LanguageLevel;
            oldCourse.Duration = course.Duration;
            oldCourse.Days = course.Days;
            oldCourse.Start = course.Start;
            oldCourse.Realization = course.Realization;
            oldCourse.MaxStudents = course.MaxStudents;
            oldCourse.TutorId = course.TutorId;
            oldCourse.NumberOfStudents = course.NumberOfStudents;
            Save();
        }

        public Course Create(Course course)
        {
            course.Id = GenerateId();
            courses.Add(course);
            Save();
            CourseAdded?.Invoke(this, EventArgs.Empty);
            return course;
        }

        public List<Course> LoadFromFile()
        {

            string filename = "../../../WPF/Data/CourseFile.csv";

            using (StreamReader reader = new StreamReader(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] tokens = line.Split('|');
                    if (tokens.Length < 9)
                    {
                        continue;
                    }

                    List<DaysEnum.Days> days = new List<DaysEnum.Days>();
                    string[] dayTokens = tokens[4].Split(',');

                    foreach (string token in dayTokens)
                    {
                        days.Add((DaysEnum.Days)Enum.Parse(typeof(DaysEnum.Days), token.Trim()));
                    }

                    DateTime startDate;
                    if (!DateTime.TryParseExact(tokens[5], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
                    {
                        continue;
                    }

                    int maxStudents = string.IsNullOrWhiteSpace(tokens[7]) ? 0 : Convert.ToInt32(tokens[7]);

                    Course course = new Course(
                        id: Int32.Parse(tokens[0]),
                        language: (LanguageEnum.Language)Enum.Parse(typeof(LanguageEnum.Language), tokens[1]),
                        languageLevel: (LanguageLevelEnum.LanguageLevel)Enum.Parse(typeof(LanguageLevelEnum.LanguageLevel), tokens[2]),
                        duration: Convert.ToInt32(tokens[3]),
                        days: days,
                        startDate,
                        realization: (RealizationEnum.Realization)Enum.Parse(typeof(RealizationEnum.Realization), tokens[6]),
                        maxStudents: maxStudents,
                        tutorId: Convert.ToInt32(tokens[8]),
                        numberOfStudents: Convert.ToInt32(tokens[9]));

                    courses.Add(course);
                }
            }
            return courses;


        }

    }
}
