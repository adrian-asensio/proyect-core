using Microsoft.EntityFrameworkCore;
using Dominio;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Persistencia
{
    public class CursosOnlineContext :IdentityDbContext<Usuario> //DbContext
    {
        public CursosOnlineContext(DbContextOptions options) :base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CursoInstructor>().HasKey(ci => new{ci.InstructorId, ci.CursoId});
        }

        public DbSet<Comentario> Comentarios {get;set;}
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<CursoInstructor> CursoInstructor { get; set; }
        public DbSet<Instructor> Instructores { get; set; }
        public DbSet<Precio> Precios { get; set; }
    }
}