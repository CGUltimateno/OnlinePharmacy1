using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlinePharmacy.Models
{
    public class MedicineRequest
    {
        [Key]
        public String RequestId { get; set; }
       
        public String MedicineName { get; set;}
        
        public string PrescriptionDetails { get; set; }

        public string RequestStatus { get; set; }

        
        
        [ForeignKey("AppUser")]
        public string PatientId { get; set; }

        

    }
}
