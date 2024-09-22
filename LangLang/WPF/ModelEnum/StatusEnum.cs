using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.ModelEnum
{
    public class StatusEnum
    {
        public enum Status {
            Accepted,
            Declined,
            Pending,
            Completed,
            Waiting,
            Unknown,
            Expelled
        }
    }
}
