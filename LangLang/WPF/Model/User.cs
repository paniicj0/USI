using LangLang.WPF.ModelEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LangLang.ModelEnum.GenderEnum;
using static LangLang.ModelEnum.StudentEnum;

namespace LangLang.Model
{
    public class User
    {
        private int id;
        private string email;
        private string password;
        private RoleEnum.Role role;
        private int roleId;

        public User(int id, string email, string password, RoleEnum.Role role, int roleId)
        {
            this.id = id;
            this.email = email;
            this.password = password;
            this.role = role;
            this.roleId = roleId;
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public RoleEnum.Role Role
        {
            get { return role; }
            set { role = value; }
        }

        public int RoleId
        {
            get { return roleId; }
            set { roleId = value; }
        }

        public String StringToCsv()
        {
            return $"{id}|{email}|{password}|{role}|{roleId}";
        }
    }
}
