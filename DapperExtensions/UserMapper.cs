using DapperExtensions.Mapper;

namespace DapperExtensions
{
    public class UserMapper : ClassMapper<UserModel>
    {
        public UserMapper()
        {
            Schema("dbo");
            Table("Users");
            Map(x => x.UserId).Key(KeyType.Identity);
            AutoMap();
        }
    }
}
