using DapperExtensions.Mapper;

namespace DapperExtensions
{
    public class RoleMapper : ClassMapper<RoleModel>
    {
        public RoleMapper()
        {
            Schema("dbo");
            Table("Roles");
            Map(x => x.RoleId).Key(KeyType.Identity);
            AutoMap();
        }
    }
}
