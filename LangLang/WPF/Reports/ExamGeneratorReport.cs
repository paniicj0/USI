using iTextSharp.text;
using iTextSharp.text.pdf;
using LangLang.Controllers;
using LangLang.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using iTextSharp.text.pdf.draw;

namespace LangLang.Reports
{
    class ExamGeneratorReport
    {
        private readonly AppliedExamController appliedExamController;
        private readonly ExamController examController;
        private readonly CourseController courseController;
        private readonly Director director;

        public ExamGeneratorReport(Director director)
        {
            this.director = director;
            appliedExamController = new AppliedExamController();
            examController = new ExamController();
            courseController = new CourseController();
        }

        private void AvgNumOfPoints(Document doc)
        {
            var oneYearAgo = DateTime.Now.AddYears(-1);
            var exams = examController.GetAll().Where(e => e.ExamDate >= oneYearAgo).ToList();
            var appliedExams = appliedExamController.GetAll().Where(ae => exams.Select(e => e.Id).Contains(ae.IdExam)).ToList();

            double totalReading = 0;
            double totalWriting = 0;
            double totalListening = 0;
            double totalSpeaking = 0;

            int count = appliedExams.Count;

            foreach (var appliedExam in appliedExams)
            {
                totalReading += appliedExam.Reading;
                totalWriting += appliedExam.Writting;
                totalListening += appliedExam.Listening;
                totalSpeaking += appliedExam.Speaking;
            }

            double averageReading = count > 0 ? totalReading / count : 0;
            double averageWriting = count > 0 ? totalWriting / count : 0;
            double averageListening = count > 0 ? totalListening / count : 0;
            double averageSpeaking = count > 0 ? totalSpeaking / count : 0;

            doc.Add(new Paragraph($"Average Reading Score: {averageReading:F2}"));
            doc.Add(new Paragraph($"Average Writing Score: {averageWriting:F2}"));
            doc.Add(new Paragraph($"Average Listening Score: {averageListening:F2}"));
            doc.Add(new Paragraph($"Average Speaking Score: {averageSpeaking:F2}"));
        }

        private void CalculateCourseStatistics(Document doc)
        {
            var oneYearAgo = DateTime.Now.AddYears(-1);
            var exams = examController.GetAll().Where(e => e.ExamDate >= oneYearAgo).ToList();
            var appliedExams = appliedExamController.GetAll().Where(ae => exams.Select(e => e.Id).Contains(ae.IdExam)).ToList();

            foreach (var course in courseController.GetAll())
            {
                int numberOfStudents = course.NumberOfStudents;

                int studentsWhoPassed = appliedExams
                    .Where(ae => ae.IdCours == course.Id && ae.Passed)
                    .Select(ae => ae.IdStudent)
                    .Distinct()
                    .Count();

                double passPercentage = numberOfStudents > 0 ? (studentsWhoPassed / (double)numberOfStudents) * 100 : 0;

                doc.Add(new Paragraph($"Course ID: {course.Id}"));
                doc.Add(new Paragraph($"Number of Students: {numberOfStudents}"));
                doc.Add(new Paragraph($"Students Who Passed: {studentsWhoPassed}"));
                doc.Add(new Paragraph($"Pass Percentage: {passPercentage:F2}%"));
                doc.Add(new Paragraph(" "));
            }
        }

        public void GenerateAndSendReport()
        {
            using (var ms = new MemoryStream())
            {
                Document doc = new Document();
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                doc.Open();

                doc.Add(new Paragraph("Average Number of Points"));
                doc.Add(new LineSeparator(1f, 100f, BaseColor.BLACK, Element.ALIGN_CENTER, -1));
                AvgNumOfPoints(doc);

                doc.Add(new Paragraph(" "));
                doc.Add(new Paragraph("Course Statistics"));
                doc.Add(new LineSeparator(1f, 100f, BaseColor.BLACK, Element.ALIGN_CENTER, -1));
                CalculateCourseStatistics(doc);

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
                    Subject = "Exam Report",
                    Body = "Attached is the exam report.",
                };
                mailMessage.To.Add(director.Email);

                using (var ms = new MemoryStream(pdfReport))
                {
                    mailMessage.Attachments.Add(new Attachment(ms, "ExamReport.pdf", "application/pdf"));
                    client.Send(mailMessage);
                }
            }
        }
    }
}
