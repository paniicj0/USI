using iTextSharp.text.pdf.draw;
using iTextSharp.text.pdf;
using iTextSharp.text;
using LangLang.Controllers;
using LangLang.Model;
using LangLang.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace LangLang.Reports
{
    class ExamResultsReport
    {
        private readonly AppliedExamController appliedExamController;
        private readonly ExamController examController;
        private readonly StudentController studentController;
        private readonly Director director;

        public ExamResultsReport(Director director)
        {
            this.director = director;
            appliedExamController = new AppliedExamController();
            examController = new ExamController();
            studentController = new StudentController();
        }

        public void GenerateAndSendReports(int selectedId)
        {
            
            var appliedExams = appliedExamController.GetAll().Where(ae => ae.IdExam == selectedId).ToList();

            foreach (var appliedExam in appliedExams)
            {
                var student = studentController.GetById(appliedExam.IdStudent);
                if (student != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        Document doc = new Document();
                        PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                        doc.Open();

                        doc.Add(new Paragraph("Student Exam Report"));
                        doc.Add(new LineSeparator(1f, 100f, BaseColor.BLACK, Element.ALIGN_CENTER, -1));
                        doc.Add(new Paragraph(" "));

                        AddStudentExamResults(doc, appliedExam);

                        doc.Close();
                        writer.Close();

                        SendReport(ms.ToArray(), student.Email);
                    }
                }
            }
        }

        private void AddStudentExamResults(Document doc, AppliedExam exam)
        {
            doc.Add(new Paragraph($"Course ID: {exam.IdCours}"));
            doc.Add(new Paragraph($"Exam ID: {exam.IdExam}"));
            doc.Add(new Paragraph($"Passed: {(exam.Passed ? "Yes" : "No")}"));
            doc.Add(new Paragraph($"Reading: {exam.Reading} points"));
            doc.Add(new Paragraph($"Writing: {exam.Writting} points"));
            doc.Add(new Paragraph($"Listening: {exam.Listening} points"));
            doc.Add(new Paragraph($"Speaking: {exam.Speaking} points"));
            doc.Add(new Paragraph($"Overall Grade: {exam.Grade}"));
            doc.Add(new Paragraph(" "));
        }

        private void SendReport(byte[] pdfReport, string studentEmail)
        {
            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.Credentials = new NetworkCredential("testic.usii@gmail.com", "cgoxbbyhtduliywv");
                client.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("testic.usii@gmail.com"),
                    Subject = "Exam Report",
                    Body = "Attached is the exam report.",
                };

                mailMessage.To.Add(studentEmail);

                using (var ms = new MemoryStream(pdfReport))
                {
                    mailMessage.Attachments.Add(new Attachment(ms, "ExamReport.pdf", "application/pdf"));
                    client.Send(mailMessage);
                }
            }
        }
        /*
        private void SendNotificationWithGrades(AppliedExam selectedAppliedExam)
        {
            Course selectedCourse = courseController.GetById(selectedAppliedExam.IdCours);
            string message = "Your grade in course " + selectedCourse.Language + " " + selectedCourse.LanguageLevel + " is " + selectedAppliedExam.Grade + " !";
            Notification notification = new Notification(-1, selectedAppliedExam.IdStudent, selectedCourse.Id, false, message);
            notificationController.Create(notification);
        }
        */
    }
}