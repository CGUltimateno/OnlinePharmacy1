using System.ComponentModel.DataAnnotations;

namespace OnlinePharmacy.DTOs
{
    public class RequestMedicineDTO
    {
        [Required(ErrorMessage = "Medicine Name is required.")]
        [StringLength(100, ErrorMessage = "Medicine Name cannot exceed 100 characters.")]
        public string MedicineName { get; set; }

        [Required(ErrorMessage = "Prescription Details are required.")]
        [StringLength(500, ErrorMessage = "Prescription Details cannot exceed 500 characters.")]
        public string PrescriptionDetails { get; set; }


    }
}
