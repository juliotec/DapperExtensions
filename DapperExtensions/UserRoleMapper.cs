using DapperExtensions.Mapper;

namespace DapperExtensions
{
    public class UserRoleMapper : ClassMapper<UserRoleModel>
    {
        public UserRoleMapper()
        {
            Schema("dbo");
            Table("UserRoles");
            Map(x => x.UserId).Key(KeyType.Assigned);
            Map(x => x.RoleId).Key(KeyType.Assigned);
            AutoMap();
        }
    }
}
