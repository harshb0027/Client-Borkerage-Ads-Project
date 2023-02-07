using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab04.Models
{
    public class Advertisement
    {

        
        public string AdvertisementId
        {
            get;
            set;
        }

        public string BrokerId
        {
            get;
            set;
        }


        [Required]
        [Column("FileName")]
        [Display(Name = "File Name")]
        public string FileName
        {
            get;
            set;
        }

        [Required]
        [Column("Image")]
        [Display(Name = "Image")]
        public string Image
        {
            get;
            set;
        }
        public Brokerage Brokerage { get; set; }

    }
 
}

//Navigation property to brokerage