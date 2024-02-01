using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeList_MVC.Models
{
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        [MaxLength(255)]
        [DisplayName("Department Name")]
        public string DepartmentName { get; set; }
        [MaxLength(10)]
        public string Abbreviation { get; set; }
    }
}
