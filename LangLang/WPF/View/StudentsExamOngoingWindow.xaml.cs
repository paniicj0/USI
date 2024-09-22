using LangLang.Controllers;
using LangLang.Model;
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
    /// Interaction logic for StudentsExamOngoingWindow.xaml
    /// </summary>
    public partial class StudentsExamOngoingWindow : Window
    {
        private ExamController examController = new ExamController();
        private StudentController studentController = new StudentController();
        private AppliedExamController appliedExamController = new AppliedExamController();
        private Tutor tutor;
        List<Student> students;
        List<Exam> exams;
        List<AppliedExam> appliedExams;

        public int selectedId;
        public StudentsExamOngoingWindow(int selectedCourseId, Tutor tutor)
        {
            InitializeComponent();
            this.tutor = tutor;
        }

        private void StudentsExamOngoingWindow_Loaded(object sender, RoutedEventArgs e)
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

        private void LoadDataFromCSV(List<Student> students, List<Exam> courses, List<AppliedExam> appliedExams)
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
                        foreach (Exam exam in exams)
                        {
                            foreach (Student student in students)
                            {
                                if (appliedExam.IdExam == exam.Id)
                                {
                                    if (tutor.Id == exam.TutorId)
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

        private void btnBan_Click(object sender, RoutedEventArgs e)
        {
            
            if (dgvStudents.SelectedItem != null)
            {
                
                DataRowView selectedRow = (DataRowView)dgvStudents.SelectedItem;
                int selectedStudentId = (int)selectedRow["Id"];

                bool success = appliedExamController.BanStudent(selectedStudentId);

                if (success)
                {
                    MessageBox.Show("Student has been banned from taking exams.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    
                    LoadDataFromCSV(students, exams, appliedExams);
                }
                else
                {
                    MessageBox.Show("Failed to ban student. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a student to ban.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
