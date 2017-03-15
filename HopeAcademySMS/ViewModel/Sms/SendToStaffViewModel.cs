using System.ComponentModel.DataAnnotations;

namespace StAugustine.ViewModel.Sms
{
    public class SendToStaffViewModel
    {
        public string SenderId { get; set; }

        [Display(Name = "Message")]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }
    }
}