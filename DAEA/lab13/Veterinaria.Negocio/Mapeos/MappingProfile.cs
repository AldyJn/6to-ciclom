using AutoMapper;
using Veterinaria.Entidades;
using Veterinaria.Entidades.Dtos;

namespace Veterinaria.Negocio.Mapeos;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Propietario, PropietarioDto>()
            .ForMember(d => d.TotalMascotas, o => o.MapFrom(s => s.Mascotas.Count));

        CreateMap<PropietarioCrearDto, Propietario>();

        CreateMap<PropietarioActualizarDto, Propietario>();

        CreateMap<Mascota, MascotaDto>()
            .ForMember(d => d.PropietarioNombre, o => o.MapFrom(s => s.Propietario.Nombre));

        CreateMap<MascotaCrearDto, Mascota>();

        CreateMap<MascotaActualizarDto, Mascota>();
    }
}
