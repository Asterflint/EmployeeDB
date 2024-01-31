#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeList_MVC.Models
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [MaxLength(20)]
        public string? NIK { get; set; }

        [MaxLength(255)]
        public string? FirstName { get; set; }

        [MaxLength(255)]
        public string? LastName { get; set; }

        [MaxLength(255)]
        public string? Address { get; set; }

        public char? Gender { get; set; }

        [MaxLength(255)]
        public string? PlaceOfBirth { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [MaxLength(255)]
        public string? Email { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        [ForeignKey("JobTitle")]
        public int? JobTitleID { get; set; }

        public DateTime? HireDate { get; set; }
    }
}
