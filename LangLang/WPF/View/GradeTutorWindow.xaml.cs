using LangLang.Controllers;
using LangLang.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class GradeTutorWindow : Window
    {
        public int Grade { get; private set; }
        private int tutorId;
        private int courseId;

        public Student student;
        public CoursesCompletedWindow coursesCompletedWindow;

        public GradeTutorController gradeTutorController = new GradeTutorController();
        public GradeTutorWindow(Student student, int tutorId, int courseId)
        {
            InitializeComponent();
            PopulateComboBox();
            this.student = student;
            this.tutorId = tutorId;
            this.courseId = courseId;
            coursesCompletedWindow = new CoursesCompletedWindow(student);
        }

        private void PopulateComboBox()
        {
            for (int i = 0; i < 10; ++i)
            {
                cbGrades.Items.Add(i+1);
            }
        }

        private void btnSumbit_Click(object sender, RoutedEventArgs e)
        {
            object grade = cbGrades.SelectedItem;
            if (grade == null)
            {
                MessageBox.Show("You need to select a grade!");
                return;
            }
            Grade = (Int32)grade;
            GradeTutor gradeTutor = new GradeTutor(-1, student.Id, tutorId, courseId, Grade);
            gradeTutorController.Create(gradeTutor);
            MessageBox.Show("You have succesfully graded tutor!");
            this.Close();
        }
    }
}
