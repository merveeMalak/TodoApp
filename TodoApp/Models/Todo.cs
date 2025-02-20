using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;


namespace TodoApp.Models
{
    public enum TodoStatus
    {
        Waiting,
        InProgress,
        Completed
    }

    public class Todo
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        public TodoStatus Status { get; set; } = TodoStatus.Waiting;

        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }
        public bool IsArchived { get; set; } = false;

    }
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("todoConnection") { }

        public DbSet<Todo> Todos { get; set; }
    }
}

