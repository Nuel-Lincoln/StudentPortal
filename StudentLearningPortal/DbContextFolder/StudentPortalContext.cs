using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;
using StudentLearningPortal.DbModels;

namespace StudentLearningPortal.DbContextFolder
{
    public class StudentPortalContext : DbContext
    {
        public DbSet<CourseMaterials> CourseMaterials { get; set; }

        public DbSet<Courses> Courses { get; set; } 


        public DbSet<CourseScores> CoursesScores { get; set;}


        public DbSet<Lecturer> Lecturers { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<NotificationBoard> NotificationBoards { get; set; }

        public DbSet<RegisteredCourses> RegisteredCourses { get; set; }


        public StudentPortalContext(DbContextOptions dbContextOptions):base(dbContextOptions)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CourseMaterials>(b =>
            {
                b.ToCollection("CourseMaterials");
                b.Property(c => c.ForCourseId);
            });

            modelBuilder.Entity<Courses>(b =>
            {
                b.ToCollection("Courses");
                b.Property(c => c.Id);
            });

            modelBuilder.Entity<RegisteredCourses>(b =>
            {
                b.ToCollection("RegisteredCourses");
                b.Property(c => c.CourseId);
            });

            modelBuilder.Entity<NotificationBoard>(b =>
            {
                b.ToCollection("NotificationBoards");
                b.Property(c => c.SentTime);
            });

            modelBuilder.Entity<Lecturer>(b =>
            {
                b.ToCollection("Lecturers");
                b.Property(c => c.StaffNumber);
            });

            modelBuilder.Entity<Student>(b =>
            {
                b.ToCollection("Students");
                b.Property(c => c.Department);
            });

            modelBuilder.Entity<CourseScores>(b =>
            {
                b.ToCollection("CourseScores");
                b.Property(c => c.ActualScore);
            });
        }

    }
}
