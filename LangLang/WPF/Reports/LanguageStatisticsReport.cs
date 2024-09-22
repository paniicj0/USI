using iTextSharp.text.pdf.draw;
using iTextSharp.text.pdf;
using iTextSharp.text;
using LangLang.Controllers;
using LangLang.Model;
using LangLang.ModelEnum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Diagnostics;

namespace LangLang.Reports
{
    class LanguageStatisticsReport
    {
        private readonly AppliedExamController appliedExamController;
        private readonly ExamController examController;
        private readonly CourseController courseController;
        private readonly StudentController studentController;   
        private readonly Director director;

        public LanguageStatisticsReport(Director director)
        {
            this.director = director;
            appliedExamController = new AppliedExamController();
            examController = new ExamController();
            courseController = new CourseController();
            studentController = new StudentController();

        }
    
        private void GenerateLanguageStatistics(Document doc)
        {
            var oneYearAgo = DateTime.Now.AddYears(-1);
            var courses = courseController.GetAll().Where(c => c.Start >= oneYearAgo).ToList();
            var exams = examController.GetAll().Where(e => e.ExamDate >= oneYearAgo).ToList();
            var appliedExams = appliedExamController.GetAll().Where(ae => exams.Select(e => e.Id).Contains(ae.IdExam)).ToList();

            var courseCountByLanguage = courses.GroupBy(c => c.Language).ToDictionary(g => g.Key, g => g.Count());
            var examCountByLanguage = exams.GroupBy(e => e.Language).ToDictionary(g => g.Key, g => g.Count());

            var students = studentController.GetAll(); 
            var examPenaltiesByLanguage = new Dictionary<LanguageEnum.Language, double>();

            foreach (var exam in exams)
            {
                var appliedExamsForExam = appliedExams.Where(ae => ae.IdExam == exam.Id).ToList();
                var studentsForExam = students.Where(s => appliedExamsForExam.Any(ae => ae.IdStudent == s.Id)).ToList();
                var totalPenalties = studentsForExam.Sum(s => s.PenaltyPoints);
                var averagePenalties = studentsForExam.Count > 0 ? totalPenalties / (double)studentsForExam.Count : 0;
                examPenaltiesByLanguage[exam.Language] = averagePenalties;
            }

            var examPointsByLanguage = exams.GroupBy(e => e.Language)
                .ToDictionary(g => g.Key, g => g.Average(e => e.NumOfStudents));

            doc.Add(new Paragraph("Language Statistics Report"));
            doc.Add(new LineSeparator(1f, 100f, BaseColor.BLACK, Element.ALIGN_CENTER, -1));
            doc.Add(new Paragraph(" "));

            foreach (var language in courseCountByLanguage.Keys)
            {
                int numberOfExams = examCountByLanguage.ContainsKey(language) ? examCountByLanguage[language] : 0;
                double averagePenaltyPoints = examPenaltiesByLanguage.ContainsKey(language) ? examPenaltiesByLanguage[language] : 0;
                double averageExamPoints = examPointsByLanguage.ContainsKey(language) ? examPointsByLanguage[language] : 0;

                doc.Add(new Paragraph($"Language: {language}"));
                doc.Add(new Paragraph($"Number of Courses: {courseCountByLanguage[language]}"));
                doc.Add(new Paragraph($"Number of Exams: {numberOfExams}"));
                doc.Add(new Paragraph($"Average Penalty Points: {averagePenaltyPoints:F2}"));
                doc.Add(new Paragraph($"Average Exam Points: {averageExamPoints:F2}"));
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

                GenerateLanguageStatistics(doc);

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
                    From = new MailAddress("testic.usii@gmail.com"),
                    Subject = "Language Statistics Report",
                    Body = "Attached is the language statistics report.",
                };
                mailMessage.To.Add(director.Email);

                using (var ms = new MemoryStream(pdfReport))
                {
                    mailMessage.Attachments.Add(new Attachment(ms, "LanguageStatisticsReport.pdf", "application/pdf"));
                    client.Send(mailMessage);
                }
            }
        }
    }
}

