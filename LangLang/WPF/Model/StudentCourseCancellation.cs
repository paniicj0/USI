using LangLang.ModelEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Model
{
    public class StudentCourseCancellation
    {
        private int id;
        private int idStudent;
        private int idCourse;
        private StatusEnum.Status status;
        private string message;

        public StudentCourseCancellation(int id, int idCourse, int idStudent, StatusEnum.Status status, string message)
        {
            this.id = id;
            this.idCourse = idCourse;
            this.idStudent = idStudent;
            this.status = status;
            this.message = message;
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int IdCourse
        {
            get { return idCourse; }
            set { idCourse = value; }
        }

        public int IdStudent
        {
            get { return idStudent; }
            set { idStudent = value; }
        }

        public StatusEnum.Status Status
        {
            get { return status; }
            set { status = value; }
        }

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        public String StringToCsv()
        {
            return $"{id}|{idCourse}|{idStudent}|{status}|{message}";
        }
    }
}
