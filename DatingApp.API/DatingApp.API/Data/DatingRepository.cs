using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DatingApp.API.Data
{
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext _context;

        public DatingRepository(DataContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<User> GetUser(int id)
        {
            //var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);
            var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
            var users = JsonConvert.DeserializeObject<List<User>>(userData);
            var user = await Task.Run(() =>
            {
                return users.Find(u => u.UserName == "Frieda");
            });


            byte[] passwordHash, passwordSalt;
            CreatePasswordHash("password", out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.UserName = user.UserName.ToLower();
            //_context.Users.Add(user);


            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            //var users = await _context.Users.Include(p => p.Photos).ToListAsync();

            var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
            var users = JsonConvert.DeserializeObject<List<User>>(userData);
            await Task.Run(() =>
            {
                foreach (var user in users)
                {
                    byte[] passwordHash, passwordSalt;
                    CreatePasswordHash("password", out passwordHash, out passwordSalt);
                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    user.UserName = user.UserName.ToLower();
                    //_context.Users.Add(user);
                }
            });

            return users;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

        }
    }
}