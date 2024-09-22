using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Model
{
    public class GradeTutor
    {
        private int id;
        private int studentId;
        private int tutorId;
        private int courseId;
        private int grade;

        public GradeTutor(int id, int studentId, int tutorId, int courseId, int grade)
        {
            this.id = id;
            this.studentId = studentId;
            this.tutorId = tutorId;
            this.courseId = courseId;
            this.grade = grade;
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int StudentId
        {
            get { return studentId; }
            set { id = value; }
        }

        public int TutorId
        {
            get { return tutorId; }
            set { id = value; }
        }

        public int CourseId
        {
            get { return courseId; }
            set { id = value; }
        }

        public int Grade
        {
            get { return grade; }
            set { id = value; }
        }

        public String StringToCsv()
        {
            return $"{id}|{studentId}|{tutorId}|{courseId}|{grade}";
        }
    }
}
