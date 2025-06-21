using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models
{
    public class Cart
    {

        public int Id { get; set; }
       
      

        public List<int>? Books { get; set; } = new List<int>();

        public int? UserId { get; set; }
    
        
    }
}
