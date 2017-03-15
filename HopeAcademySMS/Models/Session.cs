using System.ComponentModel.DataAnnotations.Schema;

namespace StAugustine.Models
{
    public class Session
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SessionId { get; set; }

        public string SessionName { get; set; }
    }

}