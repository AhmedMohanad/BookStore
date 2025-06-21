using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BookStore.Models
{
    public class User
    {

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        [JsonIgnore]
        public string Password { get; set; } = string.Empty;
        public int CartId { get; set; }
        [ForeignKey("CartId")]
        public Cart Cart { get; set; }



    }
}
