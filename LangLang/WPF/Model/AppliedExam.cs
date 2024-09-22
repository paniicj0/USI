using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Model
{
    public class AppliedExam
    {
        private int id;
        private int idCourse;
        private int idStudent;
        private int idExam;
        private bool banned;
        private bool passed;
        private int reading;
        private int writting;
        private int listening;
        private int speaking;
        private int grade;
        private int gradeActivity;

        public AppliedExam(int id, int idCourse, int idStudent, int idExam, bool banned, bool passed, int reading, int writting, int listening, int speaking, int grade, int gradeActivity)
        {
            this.id = id;
            this.idCourse = idCourse;
            this.idStudent = idStudent;
            this.idExam = idExam;
            this.banned = banned;
            this.passed = passed;
            this.reading = reading;
            this.writting = writting;
            this.listening = listening;
            this.speaking = speaking;
            this.grade = grade;
            this.gradeActivity = gradeActivity;
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int IdCours
        {
            get { return idCourse; }
            set { idCourse = value; }
        }

        public int IdStudent
        {
            get { return idStudent; }
            set { idStudent = value; }
        }

        public int IdExam
        {
            get { return idExam; }
            set { idExam = value; }
        }

        public bool Banned
        {
            get { return banned; }
            set { banned = value; }
        }

        public bool Passed
        {
            get { return passed; }
            set { passed = value; }
        }

        public int Reading
        {
            get { return reading; }
            set { reading = value; }

        }

        public int Writting
        {
            get { return writting; }
            set { writting = value; }
        }

        public int Listening
        {
            get { return listening; }
            set { listening = value; }
        }

        public int Speaking
        {
            get { return speaking; }
            set { speaking = value; }
        }

        public int Grade
        {
            get { return grade; }
            set { grade = value; }
        }

        public int GradeActivity
        {
            get { return gradeActivity; }
            set { gradeActivity = value; }
        }

        public String StringToCSV() { 
            return $"{id}|{idCourse}|{idStudent}|{idExam}|{banned}|{passed}|{reading}|{writting}|{listening}|{speaking}|{grade}|{gradeActivity}";

        }

    }
}
