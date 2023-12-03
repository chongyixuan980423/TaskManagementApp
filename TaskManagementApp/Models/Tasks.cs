using System.ComponentModel.DataAnnotations;

namespace TaskManagementApp.Models
{
    public class Tasks
    {
        public Guid ID { get; set; }

        [Display(Name = "Task")]
        public string? Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        [Display(Name = "Assigned to")]
        public string? AssignedTo { get; set; }
        
        public Status Status { get; set; }

        [Display(Name = "Created at")]
        public DateTime SysCreated { get; set; }

        [Display(Name = "Modified at")]
        public DateTime SysModified { get; set; }
    }

    public enum Status
    {
        [Display(Name = "New")] New,
        [Display(Name = "In progress")] InProgress,
        [Display(Name = "On hold")] OnHold,
        [Display(Name = "Completed")] Completed
    }
}
