using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace Sample.Domain;
public class TodoContext : DbContext
{
    public TodoContext(DbContextOptions<TodoContext> options) : base(options)
    {
    }
    public DbSet<Todo> Tasks { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Document> Documents { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var serializerOptions = new JsonSerializerOptions();

        var converter = new ValueConverter<List<Guid>, string>(
         v => JsonSerializer.Serialize(v, serializerOptions),
         v => JsonSerializer.Deserialize<List<Guid>>(v, serializerOptions)!);

        modelBuilder.Entity<Todo>()
            .Property(x => x.Documents)
            .HasConversion(converter);

        modelBuilder.Entity<Todo>()
            .Property(x => x.Priority)
            .HasConversion(
                v => v.ToString(),
                v => (TodoPriority) Enum.Parse(typeof(TodoPriority), v));
    }
}