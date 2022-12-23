using System.Linq;
using Microsoft.EntityFrameworkCore;
using shopapp.data.Abstract;
using shopapp.entity;

namespace shopapp.data.Concrete.EfCore
{
    public class EfCoreCartRepository : EfCoreGenericRepository<Cart>, ICartRepository
    {
        
        public EfCoreCartRepository(ShopContext context):base(context)
        {
            
        }
        private ShopContext  shopContext
        {
            get {return context as ShopContext;}
        }
        public void ClearCart(int cartId)
        {
            var cmd = @"delete from CartItems where CartId=@p0";
            shopContext.Database.ExecuteSqlRaw(cmd,cartId);
        }

        public void DeleteFromCart(int cartId, int productId)
        {
            var cmd = @"delete from CartItems where CartId=@p0 and ProductId=@p1";
            shopContext.Database.ExecuteSqlRaw(cmd,cartId,productId);
        }

        public Cart GetByUserId(string userId)
        {
            return shopContext.Carts
                        .Include(i=>i.CartItems)
                        .ThenInclude(i=>i.Product)
                        .FirstOrDefault(i=>i.UserId==userId);
        }

        public override void Update(Cart entity)
        {
            
               shopContext.Carts.Update(entity);
               shopContext.SaveChanges();
            
        } 
    }
}