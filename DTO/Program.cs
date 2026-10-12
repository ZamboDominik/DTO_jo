using static DTO.User;
using AutoMapper;

namespace DTO
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MappingProfile _mapper = new MappingProfile();

           



            // Hatékony: Az SQL lekérdezés csak a DTO-ban szereplő mezőket kéri le az adatbázistól
            var users = await _context.Users
                .Where(u => u.IsActive)
                .ProjectTo<UserResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
