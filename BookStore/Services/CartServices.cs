using BookStore.Data;
using BookStore.Models;

namespace BookStore.Services
{
    public class CartServices
    {
        private BookServices _bs;
        private StoreDbContext _db;
        public CartServices(StoreDbContext context)
        {
           _db = context;
        }



        public int CreateCart()
        {
            _db.Carts.Add(new Cart());
            _db.SaveChanges();
            return _db.Carts.OrderBy(o=>o.Id).Last().Id;



        }

        public double CalculateTotale(List<int> ids)
        {

            _bs = new BookServices(_db);
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
