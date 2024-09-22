using LangLang.Controllers;
using LangLang.Model;
using LangLang.ModelEnum;
using LangLang.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static LangLang.ModelEnum.LanguageEnum;
using static LangLang.ModelEnum.LanguageLevelEnum;

namespace LangLang.View
{
    public partial class ExamsDisplayForStudent : Window
    {
        public Student student;
        public List<Exam> exams;
        public DateTime selectedExamDate;
        public Language selectedLanguage;
        public LanguageLevel selectedLanguageLevel;
        public int selectedId;
        public int selectedNumOfStudents;
        private int selectedExamDuration;
        private int selectedNumOfAppliedStudents;
        public StudentWindow studentWindow;
        private ExamDisplayWindow examDisplayWindow;
        private CourseRepository courseRepository;
        private ExamRepository examRepository;
        private StudentExamCancellationRepository studentExamCancellation;
        private ExamController examController=new ExamController();
        private StudentController studentController=new StudentController();
        private StudentCourseRequestController studentCourseRequestController = new StudentCourseRequestController();
        private StudentExamCancellationController studentExamCancellationController=new StudentExamCancellationController();
        private AppliedExamController appliedExamController=new AppliedExamController();
        private NotificationController notificationController=new NotificationController();

        public ExamsDisplayForStudent(Student student)
        {
            InitializeComponent();
            examRepository = ExamRepository.GetInstance();
            this.exams = examRepository.GetAll();
            this.student = student;
            studentWindow = new StudentWindow(student);
            courseRepository = CourseRepository.GetInstance();
            examDisplayWindow = new ExamDisplayWindow();
            studentExamCancellation=StudentExamCancellationRepository.GetInstance();
            studentExamCancellation.AppliedForExam += UpdateAddedExam;
        }

        public void ExamsDisplayForStudent_Load(object sender, RoutedEventArgs e)
        {
            FillComboBoxWithLanguage();
            FillComboBoxWithLanguageLevel();

            registerExam();
        }

        private void UpdateAddedExam(object sender, EventArgs e)
        {
            LoadDataFromCSV(exams);
        }

        private void LoadDataFromCSV(List<Exam> filteredExams)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("ID", typeof(int));
            dataTable.Columns.Add("Language", typeof(string));
            dataTable.Columns.Add("LanguageLevel", typeof(string));
            dataTable.Columns.Add("ExamDate", typeof(DateTime));
            dataTable.Columns.Add("NumOfStudents", typeof(int));
            dataTable.Columns.Add("NumOfAppliedStudents", typeof(int));
            dataTable.Columns.Add("ExamDuration", typeof(int));

            foreach (Exam exam in filteredExams)
            {
                string language = exam.Language.ToString();
                string languageLevel = exam.LanguageLevel.ToString();
                dataTable.Rows.Add(
                    exam.Id,
                    language,
                    languageLevel,
                    exam.ExamDate,
                    exam.NumOfStudents,
                    exam.NumberOfAppliedStudents,
                    exam.ExamDuration
                );
            }

            dgExam.ItemsSource = dataTable.DefaultView;
            dgExam.UnselectAllCells();
            dgExam.IsReadOnly = true;
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (!e.Cancel && this.IsVisible)
            {
                e.Cancel = true;
                this.Hide();
                studentWindow.Show();
            }
        }

        private void FillComboBoxWithLanguage()
        {
            Array languages = Enum.GetValues(typeof(LanguageEnum.Language));

            foreach (var language in languages)
            {
                cbLanguage.Items.Add(language);
            }
        }

        private void FillComboBoxWithLanguageLevel()
        {
            Array languageLevels = Enum.GetValues(typeof(LanguageLevelEnum.LanguageLevel));

            foreach (var languageLevel in languageLevels)
            {
                cbLanguageLevel.Items.Add(languageLevel);
            }
        }

        private void tbDuration_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }

        private void DatePicker_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = true;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            LanguageEnum.Language? language = cbLanguage.SelectedItem as LanguageEnum.Language?;
            LanguageLevelEnum.LanguageLevel? languageLevel = cbLanguageLevel.SelectedItem as LanguageLevelEnum.LanguageLevel?;
            DateTime? start = dpStart.SelectedDate;
            if (language == null && languageLevel == null && start == null)
            {
                lblNoSearch.Content = "No search parameters have been entered.";
                registerExam();
            }
            else
            {
                List<Exam> filteredExams = FilterExams(language, languageLevel, start);
                LoadDataFromCSV(filteredExams);
            }
        }

        private List<Exam> FilterExams(LanguageEnum.Language? language, LanguageLevelEnum.LanguageLevel? languageLevel, DateTime? start)
        {
            return exams.Where(course =>
                (!language.HasValue || course.Language == language) &&
                (!languageLevel.HasValue || course.LanguageLevel == languageLevel) &&
                (!start.HasValue || course.ExamDate == start.Value)
            ).ToList();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            cbLanguage.SelectedItem = null;
            cbLanguageLevel.SelectedItem = null;

            dpStart.SelectedDate = null;
        }

        private void registerExam()
        {
            List<Exam> filteredExams = new List<Exam>();
            List<StudentCourseRequest> courseRequests = studentCourseRequestController.GetAll();

            foreach(var cId in courseRequests)
            {
                if (studentCourseRequestController.GetStatusForStudent(cId.Id) == StatusEnum.Status.Completed)
                {
                    foreach(var courseId in student.RegisteredCourses) {

                        var course = courseRepository.GetById(courseId);
                        if (course != null)
                        {
                            LanguageEnum.Language courseLanguage = course.Language;
                            LanguageLevelEnum.LanguageLevel courseLanguageLevel = course.LanguageLevel;

                            foreach (var exam in exams)
                            {
                                if (exam.Language == courseLanguage && exam.LanguageLevel == courseLanguageLevel)
                                {
                                    if (exam.NumOfStudents > exam.NumberOfAppliedStudents)
                                    {
                                        if (!filteredExams.Any(e => e.Id == exam.Id))
                                        {
                                            filteredExams.Add(exam);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    
                }

            }
              LoadDataFromCSV(filteredExams);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (dgExam.SelectedItem != null)
            {
                DataRowView row = (DataRowView)dgExam.SelectedItem;
                int examId = Convert.ToInt32(row["Id"]);
                Exam selectedExam = exams.FirstOrDefault(exam => exam.Id == examId);

                if (selectedExam == null)
                {
                    MessageBox.Show("Selected exam not found!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                TimeSpan timeUntilExam = selectedExam.ExamDate - DateTime.Today;
                if (timeUntilExam.TotalDays <= 10)
                {
                    MessageBox.Show("You cannot cancel an exam if there are less than 10 days left until the exam starts.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                StudentExamCancellation studentExam = new StudentExamCancellation(-1, examId, student.Id, false);
                studentExamCancellation.Create(studentExam);
                MessageBox.Show("Successfully cancelled in the exam!");
                selectedExam.NumberOfAppliedStudents--;
                student.RegisteredExams.Remove(examId);
                examController.Update(selectedExam);
                registerExam();
            }
            else
            {
                MessageBox.Show("First select the row that you want to cancel.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void dgExam_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dgExam.SelectedItem == null || dgExam.SelectedCells.Count == 0)
            {
                dgExam.UnselectAll();
                return;
            }

            if (dgExam.SelectedItem is DataRowView selectedRow)
            {
                selectedRow = (DataRowView)dgExam.SelectedItem;
                selectedId = (int)selectedRow["Id"];
                selectedLanguage = (LanguageEnum.Language)Enum.Parse(typeof(LanguageEnum.Language), selectedRow["Language"].ToString());
                selectedLanguageLevel = (LanguageLevelEnum.LanguageLevel)Enum.Parse(typeof(LanguageLevelEnum.LanguageLevel), selectedRow["LanguageLevel"].ToString());
                selectedExamDate = (DateTime)selectedRow["ExamDate"];
                selectedNumOfStudents = (int)selectedRow["NumOfStudents"];
                selectedNumOfAppliedStudents = (int)selectedRow["NumOfAppliedStudents"];
                selectedExamDuration = (int)selectedRow["ExamDuration"];
            }
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            if (dgExam.SelectedItem != null)
            {
                DataRowView row = (DataRowView)dgExam.SelectedItem;
                int examId = (int)row["ID"];

                var alreadyRegisteredExams = exams.Where(exam =>
                    student.RegisteredExams.Contains(exam.Id) &&
                    exam.Language == selectedLanguage &&
                    exam.LanguageLevel == selectedLanguageLevel
                );

                var notifications = notificationController.GetAll();
                foreach (var notification in notifications)
                {
                    if (notification.Read)
                    {
                        var appliedExams = appliedExamController.GetAll();
                        foreach (var appExam in appliedExams)
                        {
                            if (appExam.IdStudent == student.Id && appExam.Grade != 0 && appExam.IdExam == examId)
                            {
                                MessageBox.Show("This type of exam is graded!\nYou must enroll in a new course before registering for exams.", "Error");
                                student.AppliedForCourse = false;
                                return;
                            }
                        }
                        if (alreadyRegisteredExams.Any())
                        {
                            MessageBox.Show("Student is already registered for an exam with the same language and language level.", "Error");
                            return;
                        }
                        Exam selectedExam = exams.FirstOrDefault(exam => exam.Id == examId);
                        if (student.RegisteredExams.Contains(examId))
                        {
                            MessageBox.Show("Student is already registered for this exam.");
                            return;
                        }
                        Exam examSelect = examController.GetById(examId);
                        AppliedExam studentExamApply = new AppliedExam(-1, examSelect.Id, student.Id, examId, false, false, 0, 0, 0, 0, 0, 0);
                        appliedExamController.Create(studentExamApply);
                        selectedExam.NumberOfAppliedStudents++;
                        student.RegisteredExams.Add(examId);
                        studentController.Update(student);
                        examController.Update(selectedExam);
                        MessageBox.Show("Successfully applied in the exam!");
                        registerExam();
                    }
                    else
                    {
                        MessageBox.Show("You need to wait for approval email for passed exam.");
                        return;
                    }
                }
                
            }
            else
            {
                MessageBox.Show("Please select a row before clicking on button.");
            }
        }
        
    }
}

