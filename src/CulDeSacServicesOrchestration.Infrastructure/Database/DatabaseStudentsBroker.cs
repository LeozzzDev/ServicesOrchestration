using CulDeSacServicesOrchestration.Core.BrokersInterfaces;
using CulDeSacServicesOrchestration.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CulDeSacServicesOrchestration.Infrastructure.Database;

public class DatabaseStudentsBroker : DbContext, IDatabaseStudentsBroker
{
    public DbSet<Student> Students { get; set; }

    public DatabaseStudentsBroker(DbContextOptions<DatabaseStudentsBroker> options)
        : base(options)
    {
        Database.Migrate();
        Database.EnsureCreated();
    }

    public async ValueTask<Student> InsertStudentAsync(Student student)
    {
        try
        {
            EntityEntry<Student> studentEntityEntry =
                await Students.AddAsync(student);

            await SaveChangesAsync();
            return studentEntityEntry.Entity;
        }
        catch (Exception exception)
        {
            string m = exception.Message;
            throw;
        }
    }
    
    public async ValueTask<IEnumerable<Student>> SelectStudentsAsync()
        => await Task.FromResult(Students.ToList());
}