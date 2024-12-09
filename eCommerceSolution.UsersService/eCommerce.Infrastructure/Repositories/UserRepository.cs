using Dapper;
using eCommerce.Core.Entities;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Infrastructure.DbContext;

namespace eCommerce.Infrastructure.Repositories;

internal class UserRepository : IUserRepository
{
    private readonly DapperDbContext _context;

    public UserRepository(DapperDbContext context)
    {
        _context = context;
    }

    public async Task<ApplicationUser?> AddUser(ApplicationUser user)
    {
        // Generate a new unique userID for the user
        user.UserID = Guid.NewGuid();

        // SQL query insert user data into the "Users" table
        string query = "INSERT INTO `ecommerceusers`.`Users`" +
            "(`UserID`, `Email`, `PersonName`, `Gender`, `Password`) " +
            "VALUES(@UserID, @Email, @PersonName, @Gender, @Password)";

        int rowCountAffected = await _context.DbConnection.ExecuteAsync(query, user);

        if (rowCountAffected > 0)
            return user;
        else
            return null;
    }

    public async Task<ApplicationUser?> GetUserByEmailAndPassword(string? email, string? password)
    {
        // SQL query to select a user by Email and Password
        string query = "SELECT * FROM `ecommerceusers`.`Users` " +
                       "WHERE `Email` = @Email AND `Password` = @Password";

        var parameters = new
        {
            Email = email,
            Password = password // Ensure this password is hashed before checking
        };

        ApplicationUser? user = await _context.DbConnection.QueryFirstOrDefaultAsync<ApplicationUser>(query, parameters);

        return user;

        // Uncomment below if you want to return a new user in case of null
        // return new ApplicationUser()
        // {
        //     UserID = Guid.NewGuid(),
        //     Email = email,
        //     Password = password,
        //     PersonName = "Person name",
        //     Gender = GenderOptions.Male.ToString()
        // };
    }

    public async Task<ApplicationUser?> GetUserByUserID(Guid? userID)
    {
        var query = "SELECT * FROM `ecommerceusers`.`Users` " + 
                    "WHERE `UserID` = @UserID";
        var parameters = new { UserID = userID };

        using var connection = _context.DbConnection;
        return await connection.QueryFirstOrDefaultAsync<ApplicationUser>(query, parameters);
    }
}
