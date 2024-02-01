#nullable enable
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeList_MVC.Models
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z0-9]{6}$", ErrorMessage = "NIK must be alphanumeric and 6 characters long.")]
        public string? NIK { get; set; }

        [Required]
        [MaxLength(20)]
        [DisplayName("First Name")]
        public string? FirstName { get; set; }
        [Required]
        [MaxLength(20)]
        [DisplayName("Last Name")]
        public string? LastName { get; set; }
        [Required]
        [MaxLength(255)]
        public string? Address { get; set; }

        [Required]
        public char? Gender { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("Place of Birth")]
        public string? PlaceOfBirth { get; set; }

        [Required]
        [DisplayName("Date of Birth")]
        public DateTime? DateOfBirth { get; set; }

        [MaxLength(128)]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "The Email field is not a valid email address.")]
        public string? Email { get; set; }

        [Required]
        [MaxLength(20)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone must contain only numeric characters.")]
        public string? Phone { get; set; }

        [Required]
        [DisplayName("Job Title")]
        public int? JobTitleID { get; set; }

        [ForeignKey("JobTitleID")]
        public virtual JobTitle? JobTitle { get; set; }

        [Required]
        [DisplayName("Hire Date")]
        public DateTime? HireDate { get; set; }
    }
}
