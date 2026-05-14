using AutoMapper;
using MORENT.Application.DTOs;
using MORENT.Domain.Entities.Dbo;
using MORENT.Domain.Entities.Security;

namespace MORENT.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Security Mappings
            CreateMap<User, AuthResponse>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Name));
            CreateMap<RegisterRequest, User>();

            // Domain to DTO Mappings
            CreateMap<Car, CarDto>()
                .ForMember(dest => dest.CarType, opt => opt.MapFrom(src => src.CarType.Name))
                .ForMember(dest => dest.FuelType, opt => opt.MapFrom(src => src.FuelType.Name))
                .ForMember(dest => dest.SteeringType, opt => opt.MapFrom(src => src.SteeringType.Name))
                .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src =>
                    src.CarImages.FirstOrDefault(i => i.IsMain) != null
                    ? src.CarImages.FirstOrDefault(i => i.IsMain)!.ImageUrl
                    : string.Empty));

            CreateMap<Review, ReviewDto>()
                .ForMember(dest => dest.ReviewerName, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName))
                .ForMember(dest => dest.ReviewerJobTitle, opt => opt.MapFrom(src => "Client"));

            CreateMap<Rental, RentalDto>()
                .ForMember(dest => dest.CarBrand, opt => opt.MapFrom(src => src.Car.Brand))
                .ForMember(dest => dest.PickUpLocation, opt => opt.MapFrom(src => src.PickUpLocation.Name))
                .ForMember(dest => dest.DropOffLocation, opt => opt.MapFrom(src => src.DropOffLocation.Name))
                .ForMember(dest => dest.RentalStatus, opt => opt.MapFrom(src => src.RentalStatus.Name));

            // Dashboard Mappings
            CreateMap<Rental, RecentTransactionDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.CarName, opt => opt.MapFrom(src => src.Car.Brand))
                .ForMember(dest => dest.CarType, opt => opt.MapFrom(src => src.Car.CarType.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.TotalAmount))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src =>
                    src.Car.CarImages.FirstOrDefault(i => i.IsMain) != null
                    ? src.Car.CarImages.FirstOrDefault(i => i.IsMain)!.ImageUrl
                    : string.Empty));

            CreateMap<Rental, ActiveRentalDto>()
                .ForMember(dest => dest.CarName, opt => opt.MapFrom(src => src.Car.Brand))
                .ForMember(dest => dest.CarType, opt => opt.MapFrom(src => src.Car.CarType.Name))
                .ForMember(dest => dest.PickUpLocation, opt => opt.MapFrom(src => src.PickUpLocation.Name))
                .ForMember(dest => dest.DropOffLocation, opt => opt.MapFrom(src => src.DropOffLocation.Name))
                .ForMember(dest => dest.PickUpDate, opt => opt.MapFrom(src => src.PickUpDate))
                .ForMember(dest => dest.DropOffDate, opt => opt.MapFrom(src => src.DropOffDate))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalAmount))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src =>
                    src.Car.CarImages.FirstOrDefault(i => i.IsMain) != null
                    ? src.Car.CarImages.FirstOrDefault(i => i.IsMain)!.ImageUrl
                    : string.Empty));
        }
    }
}