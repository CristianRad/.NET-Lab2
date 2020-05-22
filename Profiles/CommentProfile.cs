using AutoMapper;
using Lab2.Models;
using Lab2.ViewModels;

namespace Lab2.Profiles
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, CommentDto>();

            CreateMap<CommentCreationDto, Comment>();
        }
    }
}
