using BookStore.Data;
using BookStore.Models;

namespace BookStore.Services
{
    public class CartServices
    {
        private BookServices _bs;
        public CartServices(StoreDbContext context)
        {
            _bs = new BookServices(context);
        }



        public double CalculateTotale(List<int> ids)
        {
            List<double> books = new List<double>();

            for (int i = 0; i < ids.Count; i++)
            {
                books.Add(_bs.GetBookById(ids[i]).Price);
            }
            double total = 0;
            foreach (int p in books)
            {
                total += p;
            }


            return total;
        }
    }
}
