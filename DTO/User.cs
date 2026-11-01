using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DTO.User;

namespace DTO
{
    public record UserDto(int Id, string Username, string Email);
    // 1. Profil definiálása
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserResponseDto>();
            CreateMap<CreateUserDto, User>();
        }
        
    }

    internal class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } 
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }


        public UserDto GetUser(int id)
        {
            User userEntity = database.FindUser(id);

            return new UserDto(userEntity.Id, userEntity.Username, userEntity.Email);
        }

        public record CreateUserDto(
            [Required, MaxLength(50)] string Username,
            [Required, EmailAddress] string Email,
            [Required, MinLength(8)] string Password
        );

        public record UserResponseDto(
            int Id,
            string Username,
            string Email,
            DateTime CreatedAt
        );


    }


}
