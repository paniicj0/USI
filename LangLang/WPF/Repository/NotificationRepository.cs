using LangLang.Model;
using LangLang.ModelEnum;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Repository
{
    public class NotificationRepository
    {
        private static NotificationRepository instance = null;
        private List<Notification> notifications;

        private NotificationRepository()
        {
            notifications = new List<Notification>();
            notifications = LoadFromFile();

        }

        public static NotificationRepository GetInstance()
        {
            if (instance == null)
            {
                instance = new NotificationRepository();
            }
            return instance;
        }

        public List<Notification> GetAll()
        {
            return new List<Notification>(notifications);
        }

        public Notification GetById(int id)
        {
            foreach (Notification notification in notifications)
            {
                if (notification.Id == id)
                {
                    return notification;
                }
            }

            return null;
        }

        public Notification GetByStudentId(int id)
        {
            foreach (Notification notification in notifications)
            {
                if (notification.Whom == id)
                {
                    return notification;
                }
            }

            return null;
        }

        private int GenerateId()
        {
            int maxId = 0;
            foreach (Notification notification in notifications)
            {
                if (notification.Id > maxId)
                {
                    maxId = notification.Id;
                }
            }

            return maxId + 1;
        }

        public void Save()
        {
            try
            {
                StreamWriter file = new StreamWriter("../../../WPF/Data/NotificationFile.csv", false);

                foreach (Notification notification in notifications)
                {
                    file.WriteLine(notification.StringToCsv());
                }

                file.Flush();
                file.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public void Delete(int id)
        {
            Notification notification = GetById(id);
            if (notification == null)
            {
                return;
            }

            notifications.Remove(notification);
            Save();
        }

        public void Update(Notification notification)
        {
            Notification oldNotification = GetById(notification.Id);
            if (oldNotification == null)
            {
                return;
            }

            oldNotification.Whom = notification.Whom;
            oldNotification.Message = notification.Message;
            oldNotification.IdCourse = notification.IdCourse;
            oldNotification.Read = notification.Read;
            Save();
        }

        public Notification Create(Notification notification)
        {
            notification.Id = GenerateId();
            notification.Read = false;
            notifications.Add(notification);
            Save();
            return notification;
        }

        public List<Notification> LoadFromFile()
        {

            string filename = "../../../WPF/Data/NotificationFile.csv";

            using (StreamReader reader = new StreamReader(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] tokens = line.Split('|');
                    if (tokens.Length < 5)
                    {
                        continue;
                    }

                    bool read;
                    if (!bool.TryParse(tokens[3], out read))
                    {
                        continue;
                    }

                    Notification notification = new Notification(
                        id: Int32.Parse(tokens[0]),
                        whom: Int32.Parse(tokens[1]),
                        idCourse: Int32.Parse(tokens[2]),
                        read: read,
                        message: tokens[4]);

                    notifications.Add(notification);
                }
            }
            return notifications;


        }
    }
}
