using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Reflection;

namespace Persistence.DbContexts;

public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext(string[] args)
    {
        const string connectionString = "Server=NASIR-LAP;Database=SampleDemo;Integrated Security=true;TrustServerCertificate=true";
        var builder = new DbContextOptionsBuilder<DatabaseContext>();
        builder.UseSqlServer(connectionString,
            option => option.MigrationsAssembly(typeof(DatabaseContext).GetTypeInfo().Assembly.GetName().Name));
        return new DatabaseContext(builder.Options);
    }
}
