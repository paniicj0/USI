using LangLang.Controllers;
using LangLang.Model;
using LangLang.ModelEnum;
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
    /// Interaction logic for StudentDeclineWindow.xaml
    /// </summary>
    public partial class StudentDeclineWindow : Window
    {
        StudentCourseRequest request;
        StudentCourseRequestController requestController = new StudentCourseRequestController();
        NotificationController notificationController = new NotificationController();

        public StudentDeclineWindow(StudentCourseRequest request)
        {
            InitializeComponent();
            this.request = request;
        }

        private void Button_Yes(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtReason.Text))
            {
                MessageBox.Show("You must enter a reason for declining.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            request.Message = txtReason.Text;
            request.Status = StatusEnum.Status.Declined;
            requestController.Update(request);
            Notification notification = new Notification(-1, request.IdStudent, request.IdCourse, false, request.Message);
            notificationController.Create(notification);

            this.Close();

        }

        private void Button_No(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
