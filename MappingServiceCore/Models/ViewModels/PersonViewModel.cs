using System.ComponentModel.DataAnnotations;

namespace MappingServiceCore.Models.ViewModels
{
    public class PersonViewModel
    {
        [Key]
        public string? Id { get; set; }

        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفاً نام را وارد کنید.")]
        [StringLength(30, ErrorMessage = "نام نباید بیشتر از 30 کاراکتر باشد.")]
        public string? FirstName { get; set; }

        [Display(Name = "نام‌خانوادگی")]
        [Required(ErrorMessage = "لطفاً نام‌خانوادگی را وارد کنید.")]
        [StringLength(50, ErrorMessage = "نام‌خانوادگی نباید بیشتر از 50 کاراکتر باشد.")]
        public string? LastName { get; set; }

        [Display(Name = "شماره تلفن")]
        [Required(ErrorMessage = "لطفاً شماره تلفن را وارد کنید.")]
        [Phone(ErrorMessage = "لطفاً یک شماره تلفن معتبر وارد کنید.")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "تاریخ ثبت")]
        public string? CreateDate { get; set; }
    }
}
