using LangLang.Controllers;
using LangLang.Model;
using LangLang.ModelEnum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.RightsManagement;
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

namespace LangLang.View
{
    /// <summary>
    /// Interaction logic for TutorGradingWindow.xaml
    /// </summary>
    public partial class TutorGradingWindow : Window
    {
        private AppliedExamController appliedExamController = new AppliedExamController();
        private StudentController studentController = new StudentController();
        private StudentCourseRequestController requestController = new StudentCourseRequestController();
        private Tutor tutor;
        List<AppliedExam> appliedExams;
        List<Student> students;
        List<StudentCourseRequest> requests;
        private Student student;

        public int selectedCourseId;
        public int selectedId;
        public string selectedName;
        public string selectedSurname;
        public int selectedGrade;

        public TutorGradingWindow(Tutor tutor, int selectedCourseId)
        {
            InitializeComponent();
            this.tutor = tutor;
            this.selectedCourseId = selectedCourseId;
        }

        private void TutorGradingWindow_Loaded(object sender, RoutedEventArgs e)
        {

            foreach (DataGridColumn column in dgvStudents.Columns)
            {
                column.CanUserSort = true;
            }
            students = studentController.GetAll();
            appliedExams = appliedExamController.GetAll();
            requests = requestController.GetAll();
            LoadDataFromCSV(students, appliedExams, requests);

        }

        private void LoadDataFromCSV(List<Student> students, List<AppliedExam> appliedExams, List<StudentCourseRequest> requests)
        {
            try
            {
                using (DataTable dataTable = new DataTable())
                {
                    dataTable.Columns.Clear();
                    dataTable.Columns.Add("Id", typeof(int));
                    dataTable.Columns.Add("Name", typeof(string));
                    dataTable.Columns.Add("Surname", typeof(string));
                    dataTable.Columns.Add("Penalty points", typeof(int));
                    dataTable.Columns.Add("Reading", typeof(int));
                    dataTable.Columns.Add("Listening", typeof(int));
                    dataTable.Columns.Add("Speaking", typeof(int));
                    dataTable.Columns.Add("Grade", typeof(int));
                    
                   

                    dataTable.Rows.Clear();
                    foreach (StudentCourseRequest request in requests)
                    {
                        if (request.IdCourse == selectedCourseId)
                        {
                            foreach (Student student in students)
                            {
                                foreach(AppliedExam applied in appliedExams) { 
                                    if (request.IdStudent == student.Id && request.Status == StatusEnum.Status.Accepted && applied.IdCours==selectedCourseId && applied.IdStudent == student.Id && applied.Banned != true)
                                    {
                                        if (IsValidStudent(student))
                                        {
                                            dataTable.Rows.Add(
                                                applied.Id,
                                                student.Name,
                                                student.Surname,
                                                student.PenaltyPoints,
                                                applied.Reading,
                                                applied.Listening,
                                                applied.Speaking,
                                                applied.Grade);
                                        }

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

        private void btnGrade_Click(object sender, RoutedEventArgs e)
        {
            if (dgvStudents.SelectedItem != null)
            {
                if(selectedGrade == 0) 
                {
                    AppliedExam appliedExam = appliedExamController.GetById(selectedId);
                    string name = selectedName + " " + selectedSurname;

                    TutorGradeStudentWindow tutorGradeStudentWindow = new TutorGradeStudentWindow(appliedExam, name);
                    tutorGradeStudentWindow.ShowDialog();
                    LoadDataFromCSV(students, appliedExams, requests);

                }
                else 
                {
                    MessageBox.Show("You have already graded this student.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Please select the row you want to update first.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void dgvStudents_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dgvStudents.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)dgvStudents.SelectedItem;
                selectedId = (int)selectedRow["Id"];
                selectedGrade = (int)selectedRow["Grade"];
                selectedName = (string)selectedRow["Name"];
                selectedSurname = (string)selectedRow["Surname"];


            }
        }
    }
}
