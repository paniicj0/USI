using LangLang.Controllers;
using LangLang.Model;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for TutorDeleteWindow.xaml
    /// </summary>
    public partial class TutorDeleteWindow : Window
    {
        private Tutor tutor;
        private TutorController tutorController;
        public event EventHandler<Tutor> TutorDeleted;
        private CourseController courseController;
        private ExamController examController;


        public TutorDeleteWindow(Tutor tutor)
        {
            InitializeComponent();
            tutorController = new TutorController();
            this.tutor = tutor;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (tutor != null)
            {
                tutorController.Delete(tutor.Id);
                courseController.DeleteTutorsCourses(tutor.Id);
                examController.DeleteTutorsExams(tutor.Id);
                MessageBox.Show("You have successfully deleted the tutor.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                TutorDeleted?.Invoke(this, tutor);
                Close();
            }
            else
            {
                MessageBox.Show("Error deleting tutor. Tutor is null.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

