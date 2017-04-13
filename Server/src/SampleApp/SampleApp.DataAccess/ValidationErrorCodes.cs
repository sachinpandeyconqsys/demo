using SampleApp.DataAccess.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApp.DataAccess
{
    public enum EnumErrorCode : Int32
    {
        RoleAlreadyExist = 1000,
        UserAlreadyExist = 1001,
        GroupAlreadyExist = 1002
    }

    public class ValidationErrorCodes : BaseValidationErrorCodes
    {
        public ValidationErrorCodes()
        {
            this.InitializeErrorCodes();
        }

        protected override void InitializeErrorCodes()
        {
            base.InitializeErrorCodes();
            this.AddErrorCode(EnumErrorCode.RoleAlreadyExist, "Role already exists - {0}");//1000
            this.AddErrorCode(EnumErrorCode.UserAlreadyExist, "User already exists"); //1001
            this.AddErrorCode(EnumErrorCode.GroupAlreadyExist, "Group already exists"); //1002
        }

        public void AddErrorCode(EnumErrorCode errorCode, string message)
        {
            base.AddErrorCode((int)errorCode, message);
        }

        public KeyValuePair<int, string> this[EnumErrorCode errorCode, params object[] formatter]
        {
            get
            {
                return base[(int)errorCode, formatter];
            }
        }


    }
}
