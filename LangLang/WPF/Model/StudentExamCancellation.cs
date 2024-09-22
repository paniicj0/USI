using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LangLang.ModelEnum.StatusEnum;

namespace LangLang.Model
{
    public class StudentExamCancellation
    {
        private int id;
        private int examId;
        private int studentId;
        private bool applied;

        public StudentExamCancellation(int id, int examId, int studentId, bool applied)
        {
            this.id = id;
            this.examId = examId;
            this.studentId = studentId;
            this.applied = applied;
        }

        public int Id { get { return id; } set { id = value; } }
        public int ExamId { get {  return examId; } set {  examId = value; } }
        public int StudentId { get { return studentId; } set {  studentId = value; } }
        public bool Applied { get {  return applied; } set {  applied = value; } }


        public String StringToCsv()
        {
            return $"{id}|{examId}|{studentId}|{applied}";
        }
    }
}
