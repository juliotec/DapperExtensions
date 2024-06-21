using DapperExtensions.Predicate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace DapperExtensions.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EjemplosController : ControllerBase
    {
        private readonly string _connectionString = "DATA SOURCE=.\\SQLEXPRESS; DATABASE=DapperExtensions; Integrated Security=True; TrustServerCertificate=True;";
        public EjemplosController()
        {

        }

        [HttpGet]
        [Route("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            await using var sqlConnection = new SqlConnection(_connectionString);
            long userId = 1;
            var userModel = await sqlConnection.GetAsync<UserModel>(userId);

            return Ok(userModel);
        }

        [HttpGet]
        [Route("GetList")]
        public async Task<IActionResult> GetList()
        {
            await using var sqlConnection = new SqlConnection(_connectionString);
            var predicate = Predicates.Field<UserModel>(x => x.IsActive, Operator.Eq, true);
            var userModels = await sqlConnection.GetListAsync<UserModel>(predicate);

            return Ok(userModels);
        }

        [HttpGet]
        [Route("GetListWhere")]
        public async Task<IActionResult> GetListWhere()
        {
            var userWhere = new PredicateGroup
            {
                Operator = GroupOperator.And,
                Predicates = new List<IPredicate>()
            };

            userWhere.Predicates.Add(Predicates.Field<UserModel>(x => x.IsActive, Operator.Eq, true));
            userWhere.Predicates.Add(Predicates.Field<UserModel>(x => x.FirstName, Operator.Like, "Jul%"));

            await using var sqlConnection = new SqlConnection(_connectionString);
            var userModels = await sqlConnection.GetListAsync<UserModel>(userWhere);

            return Ok(userModels);
        }

        [HttpGet]
        [Route("GetListSortBy")]
        public async Task<IActionResult> GetListSortBy()
        {
            var userSort = new List<ISort>() 
            { 
                Predicates.Sort<UserModel>(x => x.FirstName, true) 
            };
            var userWhere = new PredicateGroup
            {
                Operator = GroupOperator.And,
                Predicates = new List<IPredicate>()
            };

            userWhere.Predicates.Add(Predicates.Field<UserModel>(x => x.IsActive, Operator.Eq, true));
            userWhere.Predicates.Add(Predicates.Field<UserModel>(x => x.FirstName, Operator.Like, "Jul%"));

            await using var sqlConnection = new SqlConnection(_connectionString);
            var userModels = await sqlConnection.GetListAsync<UserModel>(userWhere, userSort);

            return Ok(userModels);
        }

        [HttpGet]
        [Route("GetListTransaction")]
        public async Task<IActionResult> GetListTransaction()
        {
            var userSort = new List<ISort>() 
            { 
                Predicates.Sort<UserModel>(x => x.FirstName, true) 
            };
            var userWhere = new PredicateGroup
            {
                Operator = GroupOperator.And,
                Predicates = new List<IPredicate>()
            };

            userWhere.Predicates.Add(Predicates.Field<UserModel>(x => x.IsActive, Operator.Eq, true));
            userWhere.Predicates.Add(Predicates.Field<UserModel>(x => x.FirstName, Operator.Like, "Jul%"));

            await using var sqlConnection = new SqlConnection(_connectionString);

            sqlConnection.Open();

            await using var transaction = await sqlConnection.BeginTransactionAsync();
            var userModels = await sqlConnection.GetListAsync<UserModel>(userWhere, transaction: transaction);

            if (userModels == null || !userModels.Any())
            {
                transaction.Rollback();

                return NoContent();
            }

            transaction.Commit();

            return Ok(userModels);
        }

        [HttpGet]
        [Route("GetListView")]
        public async Task<IActionResult> GetListView()
        {
            var userSort = new List<ISort>()
            {
                Predicates.Sort<UserRolesViewModel>(x => x.FirstName, true)
            };
            var userWhere = new PredicateGroup
            {
                Operator = GroupOperator.And,
                Predicates = new List<IPredicate>()
            };

            userWhere.Predicates.Add(Predicates.Field<UserRolesViewModel>(x => x.IsActive, Operator.Eq, true));
            userWhere.Predicates.Add(Predicates.Field<UserRolesViewModel>(x => x.RoleId, Operator.Eq, 1));

            await using var sqlConnection = new SqlConnection(_connectionString);
            var userModels = await sqlConnection.GetListAsync<UserRolesViewModel>(userWhere, userSort);

            return Ok(userModels);
        }

        [HttpPost]
        [Route("InsertUser")]
        public async Task<IActionResult> InsertUser()
        {
            await using var sqlConnection = new SqlConnection(_connectionString);
            var userModel = new UserModel
            {
                FirstName = "Rosa",
                LastName = "García",
                IsActive = true
            };
            var userData = await sqlConnection.InsertAsync(userModel);

            return Ok(userData);
        }

        [HttpPost]
        [Route("UpdateUser")]
        public async Task<IActionResult> UpdateUser()
        {
            await using var sqlConnection = new SqlConnection(_connectionString);
            long userId = 1;
            var userModel = await sqlConnection.GetAsync<UserModel>(userId);

            userModel.IsActive = false;

            var userData = await sqlConnection.UpdateAsync(userModel);

            return Ok(userData);
        }

        [HttpPost]
        [Route("DeleteUser")]
        public async Task<IActionResult> DeleteUser()
        {
            await using var sqlConnection = new SqlConnection(_connectionString);
            long userId = 4;
            var userData = await sqlConnection.DeleteAsync<UserModel>(userId);

            return Ok(userData);
        }
    }
}
