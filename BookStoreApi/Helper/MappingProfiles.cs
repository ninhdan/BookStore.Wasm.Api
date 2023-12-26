using AutoMapper;
using BookStoreApi.Models;
using BookStoreView.Models.Dtos;
using BookStoreView.Models.Dtos.DtoProductPortfolio;
using BookStoreView.Models.Dtos.DtoUser;

namespace BookStoreApi.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<SubCategory, SubCategoryDto>().ReverseMap();
            CreateMap<Supplier, SupplierDto>().ReverseMap();
            CreateMap<Layout,LayoutDto>().ReverseMap();
            CreateMap<Language, LanguageDto>().ReverseMap();
            CreateMap<PriceRange, PriceRangeDto>()
                        .ForMember(dest => dest.PriceRangeId, opt => opt.MapFrom(src => src.RangeId))
                        .ForMember(dest => dest.PriceRangeName, opt => opt.MapFrom(src => src.RangeName))
                        .ForMember(dest => dest.MinPrice, opt => opt.MapFrom(src => src.MinPrice))
                        .ForMember(dest => dest.MaxPrice, opt => opt.MapFrom(src => src.MaxPrice))
                        .ReverseMap();
            CreateMap<Book, BookDto>().ReverseMap();


            CreateMap<User, UserDto>().ReverseMap();


            CreateMap<Category, CategoryDtopp>()
     .ForMember(dest => dest.SubCategories, opt => opt.MapFrom(src => src.SubCategories));

            CreateMap<SubCategory, SubCategoryDtopp>()
                .ForMember(dest => dest.SubCategoryId, opt => opt.MapFrom(src => src.SubcategoryId))
                .ForMember(dest => dest.SubCategoryName, opt => opt.MapFrom(src => src.SubcategoryName))
                .ForMember(dest => dest.Books, opt => opt.MapFrom(src => src.Books));

            CreateMap<Book, BookDtopp>()
                .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.BookId))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.PathImage, opt => opt.MapFrom(src => src.PathImage));


            CreateMap<CategoryDtopp, Category>()
                .ForMember(dest => dest.SubCategories, opt => opt.MapFrom(src => src.SubCategories));

            CreateMap<SubCategoryDtopp, SubCategory>()
                .ForMember(dest => dest.SubcategoryId, opt => opt.MapFrom(src => src.SubCategoryId))
                .ForMember(dest => dest.SubcategoryName, opt => opt.MapFrom(src => src.SubCategoryName))
                .ForMember(dest => dest.Books, opt => opt.MapFrom(src => src.Books));

            CreateMap<BookDtopp, Book>()
                .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.BookId))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.PathImage, opt => opt.MapFrom(src => src.PathImage));


            CreateMap<User, UpdateUserDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Firstname, opt => opt.MapFrom(src => src.Firstname))
                .ForMember(dest => dest.Lastname, opt => opt.MapFrom(src => src.Lastname))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => src.Birthday))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
                .ReverseMap();

            CreateMap<Address, AddressDto>().ReverseMap();


        }

    }
}
