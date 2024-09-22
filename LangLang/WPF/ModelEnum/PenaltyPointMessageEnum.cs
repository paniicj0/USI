using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.ModelEnum
{
    public class PenaltyPointMessageEnum
    {
        public enum PenaltyPointMessage
        {
            [Description("The student did not attend the class.")]
            StudentDidNotAttendClass,

            [Description("The student disturbed others during the class.")]
            DisturbingOthersDuringClass,

            [Description("The student did not complete homework assignments.")]
            NotCompletingHomeworkAssignments,

            [Description("Unknown penalty point message.")]
            Unknown
        }
    }
}
