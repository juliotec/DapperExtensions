using DapperExtensions.Mapper;

namespace DapperExtensions
{
    public class UserRolesViewMapper : ClassMapper<UserRolesViewModel>
    {
        public UserRolesViewMapper()
        {
            Schema("dbo");
            Table("UserRolesView");
            Map(x => x.UserId).Key(KeyType.NotAKey);
            Map(x => x.RoleId).Key(KeyType.NotAKey);
            AutoMap();
        }
    }
}
