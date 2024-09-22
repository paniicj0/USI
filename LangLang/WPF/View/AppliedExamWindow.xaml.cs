using LangLang.Controllers;
using LangLang.Model;
using LangLang.ModelEnum;
using LangLang.Repository;
using System;
using System.Collections.Generic;
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
using static System.Net.Mime.MediaTypeNames;

namespace LangLang.View
{
    /// <summary>
    /// Interaction logic for AppliedExamWindow.xaml
    /// </summary>
    public partial class AppliedExamWindow : Window

    {
        private StartExamWindow startExamWindow;
        private ExamController examController = new ExamController();
        private StudentController studentController = new StudentController();
        private AppliedExamController appliedExamController = new AppliedExamController();
        private Tutor tutor;
        List<Student> students;
        List<Exam>exams;
        List<AppliedExam>appliedExams;
        private StudentCourseRequestController studentCourseRequestController;
        public int selectedId;
        public int selectedExamId;
        // Student loggedStudent=LogInWindow.getLoggedStudent();
        private CourseRepository courseRepository;
        private ExamDisplayWindow examDisplayWindow;


        public AppliedExamWindow(int selectedExamId, Tutor tutor, StartExamWindow startExamWindow)
        {
            InitializeComponent();
            this.tutor = tutor;
            this.selectedExamId = selectedExamId;
            this.startExamWindow = startExamWindow;
            studentCourseRequestController = new StudentCourseRequestController();
            courseRepository = CourseRepository.GetInstance();
            examDisplayWindow = new ExamDisplayWindow();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            StartExamWindow startExamWindow = new StartExamWindow(tutor);
            startExamWindow.ShowDialog();
        }

        private void AppliedExamWindow_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (DataGridColumn column in dgvStudents.Columns)
            {
                column.CanUserSort = true;
            }
            students = studentController.GetAll();
            exams = examController.GetAll();
            appliedExams = appliedExamController.GetAll();
            LoadDataFromCSV(students, exams, appliedExams);
           
        }

        private void LoadDataFromCSV(List<Student> students, List<Exam> exams, List<AppliedExam> appliedExams)
        {
            try
            {
                using (DataTable dataTable = new DataTable())
                {
                    dataTable.Columns.Clear();
                    dataTable.Columns.Add("Id", typeof(int));
                    dataTable.Columns.Add("Name", typeof(string));
                    dataTable.Columns.Add("Surname", typeof(string));
                    dataTable.Columns.Add("Gender", typeof(string));
                    dataTable.Columns.Add("Birthday", typeof(string));
                    dataTable.Columns.Add("Email", typeof(string));
                    dataTable.Columns.Add("Proffesion", typeof(string));
                    

                    dataTable.Rows.Clear();
                    foreach (AppliedExam appliedExam in appliedExams)
                    {
                        
                        if (appliedExam.IdExam == selectedExamId)
                        {                           
                                foreach (Student student in students)
                                { 
                                        if (appliedExam.IdStudent == student.Id)
                                        {
                                            if (IsValidStudent(student))
                                            {
                                                dataTable.Rows.Add(
                                                    appliedExam.Id,
                                                    student.Name,
                                                    student.Surname,
                                                    student.Gender.ToString(),
                                                    student.Birthday.ToString(),
                                                    student.Email,
                                                    student.Profession.ToString());

                                            }

                                        }
                                   
                                }
                        }
                    }
                    dgvStudents.ItemsSource = dataTable.DefaultView;
                    dgvStudents.UnselectAllCells();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool IsValidStudent(Student student)
        {

            return student != null;
        }

        private void dgvStudents_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dgvStudents.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)dgvStudents.SelectedItem;
                selectedId = (int)selectedRow["Id"];
                

            }
        }

        private bool isExamConfirmed = false; 

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (!isExamConfirmed) 
            {
                Exam examToConfirm = null;

                foreach (Exam exam in exams)
                {
                    if (exam.Id == selectedExamId)
                    {
                        examToConfirm = exam;

                        examToConfirm.Confirmed = true;
                        examController.Update(examToConfirm);
                        startExamWindow.LoadDataFromCSV();
                        MessageBox.Show("Exam successfully confirmed.", "Exam Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
                        

                        btnSubmit.IsEnabled = false; 
                        isExamConfirmed = true; 
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("The exam has already been confirmed", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

    }
}
