using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models
{
    public class Cart
    {

        public int Id { get; set; }
       
      

        public List<int>? Books { get; set; }

        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }

    }
}
