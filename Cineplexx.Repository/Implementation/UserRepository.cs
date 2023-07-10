using Cineplexx.Domain.Identity;
using Cineplexx.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cineplexx.Repository.Implementation
{
    public class UserRepository : IUserReopsitory
    {
        private readonly ApplicationDbContext context;
        private DbSet<CineplexxUser> entities;
        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities =  context.Set<CineplexxUser>();
        }
        public void Delete(CineplexxUser user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            entities.Remove(user);
            context.SaveChanges();
        }

        public CineplexxUser Get(string id)
        {
            return entities
               .Include(z => z.UserCart)
               .Include("UserCart.ProductInShoppingCarts")
               .Include("UserCart.ProductInShoppingCarts.Product")
               .SingleOrDefault(s => s.Id == id);
        }

        public IEnumerable<CineplexxUser> GetAll()
        {
           return entities.AsEnumerable();
        }

        public void Insert(CineplexxUser user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            entities.Add(user);
            context.SaveChanges();
        }

        public void Update(CineplexxUser user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            entities.Update(user);
            context.SaveChanges();
        }
    }
}
