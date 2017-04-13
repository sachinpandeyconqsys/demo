using SampleApp.Base.Entities;
using SimpleStack.Orm;

namespace SampleApp.BusinessLogic.Repositories
{
    internal class JoinSqlBuilder<TUser, TGroup, TRole>
        where TUser : class, IUser, new()
        where TGroup : class, IGroup, new()
        where TRole : class, IRole, new()
    {
        private IDialectProvider dialectProvider;

        public JoinSqlBuilder(IDialectProvider dialectProvider)
        {
            this.dialectProvider = dialectProvider;
        }
    }
}