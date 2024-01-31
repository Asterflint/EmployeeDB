using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeList_MVC.Models
{
    public class JobTitle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [MaxLength(255)]
        public string JobTitleName { get; set; }
        [ForeignKey("Department")]
        public int DepartmentID { get; set; }
    }
}
