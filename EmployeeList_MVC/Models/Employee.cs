using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EmployeeList_MVC.Models
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [MaxLength(20)]
        [AllowNull]
        public string? NIK { get; set; }
        [MaxLength(255)]
        [AllowNull]
        public string? FirstName { get; set; }
        [MaxLength(255)]
        [AllowNull]
        public string? LastName { get; set; }
        [MaxLength(255)]
        [AllowNull]
        public string? Address { get; set; }
        [MaxLength(1)]
        [AllowNull]
        public char? Gender { get; set; }
        [MaxLength(255)]
        [AllowNull]
        public string? PlaceOfBirth { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [MaxLength(255)]
        [AllowNull]
        public string? Email { get; set; }
        [MaxLength(20)]
        [AllowNull]
        public string? Phone { get; set; }
        [ForeignKey("JobTitle")]
        [AllowNull]
        public int? JobTitleID { get; set; }
        [AllowNull]
        public DateTime? HireDate { get; set; }
    }
}
