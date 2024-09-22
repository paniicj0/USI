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

namespace LangLang.View
{
    /// <summary>
    /// Interaction logic for TutorGradingExamWindow.xaml
    /// </summary>
    public partial class TutorGradingExamWindow : Window
    {
        private AppliedExamController appliedExamController = new AppliedExamController();
        private StudentController studentController = new StudentController();
        private ExamController examController = new ExamController();
        private Tutor tutor;
        List<AppliedExam> appliedExams;
        List<Student> students;
        List<Exam> exams;
        private Student student;

        public int selectedExamId;
        public int selectedId;
        public string selectedName;
        public string selectedSurname;
        public TutorGradingExamWindow(Tutor tutor, int selectedExamId)
        {
            InitializeComponent();
            this.tutor = tutor;
            this.selectedExamId = selectedExamId;
           
        }
        private void TutorGradingExamWindow_Loaded(object sender, RoutedEventArgs e)
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
                    
                    dataTable.Columns.Add("Reading", typeof(string));
                    dataTable.Columns.Add("Writting", typeof(string));
                    dataTable.Columns.Add("Listening", typeof(string));
                    dataTable.Columns.Add("Speaking", typeof(string));
                    dataTable.Columns.Add("Grade", typeof(string));


                    dataTable.Rows.Clear();
                    foreach (AppliedExam appliedExam in appliedExams)
                    {

                        if (appliedExam.IdExam == selectedExamId )
                        {
                            foreach (Student student in students)
                            {
                                if (appliedExam.IdStudent == student.Id && appliedExam.Banned != true)
                                {
                                    if (IsValidStudent(student))
                                    {
                                        dataTable.Rows.Add(
                                            appliedExam.Id,
                                            student.Name,
                                            student.Surname,
                                            appliedExam.Reading,
                                            appliedExam.Writting,
                                            appliedExam.Listening,
                                            appliedExam.Speaking,
                                            appliedExam.Grade);

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
                selectedName = (string)selectedRow["Name"];
                selectedSurname = (string)selectedRow["Surname"];


            }
        }

        private void btnGrade_Click(object sender, RoutedEventArgs e)
        {
            if (dgvStudents.SelectedItem != null)
            {

                AppliedExam appliedExam = appliedExamController.GetById(selectedId);
                string name = selectedName + " " + selectedSurname;

                TutorGradeStudentExamWindow tutorGradeStudentExamWindow = new TutorGradeStudentExamWindow(appliedExam);
                tutorGradeStudentExamWindow.ShowDialog();
                students = studentController.GetAll();
                exams = examController.GetAll();
                appliedExams = appliedExamController.GetAll();
                LoadDataFromCSV(students, exams, appliedExams);
            }
            else
            {
                MessageBox.Show("Please select the row you want to update first.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
