using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using LangLang.Controllers;
using LangLang.Model;
using LangLang.ModelEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Reports
{
    class PenaltyPointsReport
    {
        private readonly StudentController studentController;
        private readonly CourseController courseController;
        private readonly NotificationController notificationController;
        private readonly AppliedExamController appliedExamController;
        private readonly List<Notification> notifications;
        private readonly List<Course> courses;
        private readonly List<AppliedExam> appliedExams;
        private readonly List<Student> students;
        private readonly Director director;

        public PenaltyPointsReport(Director director)
        {
            this.director = director;
            this.studentController = new StudentController();
            this.courseController = new CourseController();
            this.notificationController = new NotificationController();
            this.appliedExamController = new AppliedExamController();

            this.notifications = notificationController.GetAll();
            this.courses = courseController.GetAll();
            this.appliedExams = appliedExamController.GetAll();
            this.students = studentController.GetAll();
        }

        public Dictionary<int, int> CountPenaltyPoints()
        {
            Dictionary<int, int> reportDictionary = new Dictionary<int, int>();

            foreach (var course in courses)
            {
                int counter = 0;
                if ((DateTime.Now - course.Start).TotalDays < 365)
                {
                    List<Notification> selectedNotifications = notificationController.GetByCourseId(course.Id);
                    foreach (Notification notification in selectedNotifications)
                    {
                        bool isPenalty = IsNotificationPenaltyPoint(notification);
                        if (isPenalty)
                        {
                            counter++;
                        }
                    }
                    reportDictionary[course.Id] = counter;
                }
            }
            return reportDictionary;
        }

        private string GetEnumDescription(PenaltyPointMessageEnum.PenaltyPointMessage value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute == null ? value.ToString() : attribute.Description;
        }

        private bool IsNotificationPenaltyPoint(Notification notification)
        {
            string penalty1 = GetEnumDescription(PenaltyPointMessageEnum.PenaltyPointMessage.NotCompletingHomeworkAssignments);
            string penalty2 = GetEnumDescription(PenaltyPointMessageEnum.PenaltyPointMessage.Unknown);
            string penalty3 = GetEnumDescription(PenaltyPointMessageEnum.PenaltyPointMessage.StudentDidNotAttendClass);
            string penalty4 = GetEnumDescription(PenaltyPointMessageEnum.PenaltyPointMessage.DisturbingOthersDuringClass);

            return notification.Message == penalty1 ||
                   notification.Message == penalty2 ||
                   notification.Message == penalty3 ||
                   notification.Message == penalty4;
        }

        private int CalculateAveragePoints(int penaltyPoints)
        {
            int points = 0;
            int numberOfStudents = 0;

            foreach (Student student in students)
            {
                if (student.PenaltyPoints == penaltyPoints)
                {
                    foreach (AppliedExam appliedExam in appliedExams)
                    {
                        if (appliedExam.IdStudent == student.Id)
                        {
                            points += appliedExam.Reading + appliedExam.Writting + appliedExam.Listening + appliedExam.Speaking;
                            numberOfStudents++;
                        }
                    }
                }
            }
            return numberOfStudents == 0 ? 0 : points / numberOfStudents;
        }

        public void GenerateAndSendReport()
        {
            using (var ms = new MemoryStream())
            {
                using (var doc = new Document())
                {
                    PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                    doc.Open();

                    doc.Add(new Paragraph("Penalty Points Report"));
                    doc.Add(new LineSeparator(1f, 100f, BaseColor.BLACK, Element.ALIGN_CENTER, -1));
                    AddDataToDocument(doc);

                    doc.Close();
                    writer.Close();
                }

                SendReport(ms.ToArray());
            }
        }

        private void AddDataToDocument(Document doc)
        {
            Dictionary<int, int> dictionaryReport = CountPenaltyPoints();
            foreach (Course course in courses)
            {
                if (dictionaryReport.ContainsKey(course.Id))
                {
                    doc.Add(new Paragraph($"Course: {course.Language} {course.LanguageLevel} (id: {course.Id})"));
                    doc.Add(new Paragraph($"Numeber of penalty points: {dictionaryReport[course.Id]}"));
                    doc.Add(new Paragraph(" "));
                }
            }
            doc.Add(new Paragraph(" "));
            doc.Add(new Paragraph($"Average points for students with 0 penalty points: {CalculateAveragePoints(0)}"));
            doc.Add(new Paragraph($"Average points for students with 1 penalty points: {CalculateAveragePoints(1)}"));
            doc.Add(new Paragraph($"Average points for students with 2 penalty points: {CalculateAveragePoints(2)}"));
            doc.Add(new Paragraph($"Average points for students with 3 penalty points: {CalculateAveragePoints(3)}"));
        }

        private void SendReport(byte[] pdfReport)
        {
            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.Credentials = new NetworkCredential("testic.usii@gmail.com", "cgoxbbyhtduliywv");
                client.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("belicc.tt@gmail.com"),
                    Subject = "Penalty Points Report",
                    Body = "Penalty points report is attached.",
                };

                mailMessage.To.Add(director.Email);

                using (var ms = new MemoryStream(pdfReport))
                {
                    mailMessage.Attachments.Add(new Attachment(ms, "PenaltyPointReport.pdf", "application/pdf"));
                    client.Send(mailMessage);
                }
            }
        }
    }
}
