using AutoMapper;
using EVStation_basedRentalSystem.Services.CarAPI.Models;
using EVStation_basedRentalSystem.Services.CarAPI.Models.DTO;

public class CarProfile : Profile
{
    public CarProfile()
    {
        CreateMap<Car, CarDto>();
        // If CarDto has Station property, ignore it or map manually later
        // CreateMap<Car, CarDto>()
        //     .ForMember(dest => dest.Station, opt => opt.Ignore());
    }
}
