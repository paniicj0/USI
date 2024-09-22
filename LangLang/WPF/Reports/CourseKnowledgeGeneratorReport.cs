using iTextSharp.text.pdf.draw;
using iTextSharp.text.pdf;
using iTextSharp.text;
using LangLang.Controllers;
using LangLang.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LangLang.Reports
{
     class CourseKnowledgeGeneratorReport
    {
        private readonly StudentCourseRequestController requestController;
        private readonly GradeTutorController gradeTutorController;
        private readonly TutorController tutorController;
        private readonly AppliedExamController examController;
        private readonly CourseController courseController;
        private readonly Director director;

        public CourseKnowledgeGeneratorReport(Director director)
        {
            this.director = director;
            requestController = new StudentCourseRequestController();
            gradeTutorController = new GradeTutorController();
            tutorController = new TutorController();
            examController = new AppliedExamController();
            courseController = new CourseController();
        }

        private void AvgGrade(Document doc)
        {
            var oneYearAgo = DateTime.Now.AddYears(-1);
            var courses = courseController.GetAll().Where(e => e.Start >= oneYearAgo).ToList();

            foreach (var course in courses)
            {
                var courseRequests = requestController.GetAll()
                    .Where(ae => ae.IdCourse == course.Id && ae.Status == ModelEnum.StatusEnum.Status.Accepted)
                    .ToList();

                var courseExams = examController.GetAll()
                    .Where(ae => ae.IdCours == course.Id)
                    .ToList();

                double totalKnowledgeGrade = 0;
                double totalActivityGrade = 0;

                int examCount = courseExams.Count;

                foreach (var appliedExam in courseExams)
                {
                    totalKnowledgeGrade += appliedExam.Grade;
                    totalActivityGrade += appliedExam.GradeActivity;
                }

                double averageKnowledgeGrade = examCount > 0 ? totalKnowledgeGrade / examCount : 0;
                double averageActivityGrade = examCount > 0 ? totalActivityGrade / examCount : 0;

                doc.Add(new Paragraph($"Course language: {course.Language}"));
                doc.Add(new Paragraph($"Course level: {course.LanguageLevel}"));
                doc.Add(new Paragraph($"Average knowledge grade: {averageKnowledgeGrade:F2}"));
                doc.Add(new Paragraph($"Average activity grade: {averageActivityGrade:F2}"));
                doc.Add(new Paragraph("\n"));
            }
        }

        private void AvgTutorGrade(Document doc)
        {
            var oneYearAgo = DateTime.Now.AddYears(-1);
            var courses = courseController.GetAll().Where(e => e.Start >= oneYearAgo).ToList();

            foreach (var course in courses)
            {
                var tutorId = course.TutorId;

                var tutorGrades = gradeTutorController.GetByCourseTutorId(course.Id, tutorId);
               

                double totalGrade = tutorGrades.Sum(g => g.Grade);
                int gradeCount = tutorGrades.Count;

                Tutor tutor1 = tutorController.GetById(course.TutorId);

                double averageGrade = gradeCount > 0 ? totalGrade / gradeCount : 0;

                doc.Add(new Paragraph($"Course: {course.Language}"));
                doc.Add(new Paragraph($"Course level: {course.LanguageLevel}"));
                doc.Add(new Paragraph($"Tutor name: {tutor1.Name}"));
                doc.Add(new Paragraph($"Tutor surname: {tutor1.Surname}"));
                doc.Add(new Paragraph($"Average grade: {averageGrade:F2}"));
                doc.Add(new Paragraph("\n"));
            }
        }

        public void GenerateAndSendReport()
        {
            using (var ms = new MemoryStream())
            {
                Document doc = new Document();
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                doc.Open();

                doc.Add(new Paragraph("Average knowledge and activity grade during the course"));
                doc.Add(new LineSeparator(1f, 100f, BaseColor.BLACK, Element.ALIGN_CENTER, -1));
                AvgGrade(doc);

                doc.Add(new Paragraph(" "));
                doc.Add(new Paragraph("Average tutor grade in courses"));
                doc.Add(new LineSeparator(1f, 100f, BaseColor.BLACK, Element.ALIGN_CENTER, -1));
                AvgTutorGrade(doc);

                doc.Close();
                writer.Close();

                SendReport(ms.ToArray());
            }
        }

        private void SendReport(byte[] pdfReport)
        {
            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.Credentials = new NetworkCredential("testic.usii@gmail.com", "cgoxbbyhtduliywv");
                client.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("panicjovana11@gmail.com"),
                    Subject = "Courses grade Report",
                    Body = "Attached is the courses grade report.",
                };
                mailMessage.To.Add(director.Email);

                using (var ms = new MemoryStream(pdfReport))
                {
                    mailMessage.Attachments.Add(new Attachment(ms, "CourseGradeReport.pdf", "application/pdf"));
                    client.Send(mailMessage);
                }
            }
        }
    }
}
