using System.ComponentModel.DataAnnotations;

namespace OnlinePharmacy.DTOs
{
    public class AddPatientDTO
    {
      
        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(20, ErrorMessage = "First Name cannot exceed 30 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(20, ErrorMessage = "Last Name cannot exceed 100 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid Date format.")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(100, ErrorMessage = "Address cannot exceed 200 characters.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Phone Number is required.")]
        [RegularExpression(@"^0(10|11|12|15)\d{8}$", ErrorMessage = "Invalid Phone Number.")]
        [StringLength(50, ErrorMessage = "Phone Number cannot exceed 50 characters.")]
       
        public string PhoneNumber { get; set; }
      

        
    }
}
