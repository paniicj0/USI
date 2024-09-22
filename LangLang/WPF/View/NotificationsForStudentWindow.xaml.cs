using LangLang.Controllers;
using LangLang.Model;
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

namespace LangLang.View
{
    public partial class NotificationsForStudentWindow : Window
    {
        public Student student;
        public Notification notification;
        public List<Notification> notifications;
        public NotificationRepository notificationRepository;
        public NotificationController notificationController;
        public StudentWindow studentWindow;
        private Notification selectedNotification;

        public NotificationsForStudentWindow(Student student)
        {
            InitializeComponent();
            this.student = student;
            notificationRepository = NotificationRepository.GetInstance();
            notifications = notificationRepository.GetAll();
            studentWindow = new StudentWindow(student);
        }

        public void NotificationsForStudentWindow_Load(object sender, RoutedEventArgs e)
        {
            LoadDataFromCSV(notifications);
            notificationController.CountUnreadMessages(student.Id);
        }
        private void LoadDataFromCSV(List<Notification> notifications)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("ID", typeof(int));
            dataTable.Columns.Add("Student ID", typeof(int));
            dataTable.Columns.Add("Course ID", typeof(string));
            dataTable.Columns.Add("Message", typeof(string));

            notificationController = new NotificationController();

            foreach (Notification notification in notifications)
            {
                if (notification.Whom == student.Id && notification.Read == false)
                {
                    dataTable.Rows.Add(
                        notification.Id,
                        notification.Whom,
                        notification.IdCourse,
                        notification.Message
                    );

                }
            }

            dgNotifications.ItemsSource = dataTable.DefaultView;
            dgNotifications.UnselectAllCells();
            dgNotifications.IsReadOnly = true;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (!e.Cancel && this.IsVisible)
            {
                e.Cancel = true;
                this.Hide();
                studentWindow.SetNotificationCount();
                studentWindow.Show();
            }
        }

        private void btnRead_Click(object sender, RoutedEventArgs e)
        {
            if (dgNotifications.SelectedItem == null)
            {
                MessageBox.Show("Please select a row before clicking on 'Mark as read' button.");
                return;
            }
            DataRowView row = (DataRowView)dgNotifications.SelectedItem;

            int notificationId = (int)row["ID"];

            foreach (Notification notification in notifications)
            {
                if (notification.Id == notificationId)
                {
                    Notification updateNotification = new Notification(notification.Id, notification.Whom, notification.IdCourse, true, notification.Message);
                    notificationController.Update(updateNotification);
                    LoadDataFromCSV(notifications);
                }
            }
        }
    }
}