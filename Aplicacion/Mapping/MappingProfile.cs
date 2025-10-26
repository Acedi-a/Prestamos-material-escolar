using Aplication.DTOs;
using AutoMapper;
using Dominio.Entities;

namespace Aplication.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
          
            // Docente (ejemplo)
            CreateMap<Docente, DocenteDTO>().ReverseMap();

            // Nuevos mapeos según DB real
            CreateMap<Rol, RolDTO>().ReverseMap();
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
            CreateMap<TipoReporte, TipoReporteDTO>().ReverseMap();

            CreateMap<Usuario, UsuarioDTO>()
                .ForMember(d => d.RolId, o => o.MapFrom(s => s.RolId))
                .ReverseMap()
                .ForMember(d => d.Rol, o => o.Ignore())
                .ForMember(d => d.Contrasena, o => o.Ignore()); // Nunca mapear contraseñas desde DTO comunes

            CreateMap<Material, MaterialDTO>().ReverseMap()
                .ForMember(d => d.Categoria, o => o.Ignore());

            CreateMap<HistorialReparacion, HistorialReparacionDTO>().ReverseMap()
                .ForMember(d => d.Material, o => o.Ignore());

            CreateMap<Solicitud, SolicitudDTO>()
                .ForMember(d => d.Detalles, o => o.MapFrom(s => s.Detalles));
            CreateMap<SolicitudDTO, Solicitud>()
                .ForMember(d => d.Docente, o => o.Ignore())
                .ForMember(d => d.Prestamo, o => o.Ignore())
                .ForMember(d => d.Detalles, o => o.MapFrom(s => s.Detalles));

            CreateMap<SolicitudDetalle, SolicitudDetalleDTO>().ReverseMap()
                .ForMember(d => d.Solicitud, o => o.Ignore())
                .ForMember(d => d.Material, o => o.Ignore());

            CreateMap<Prestamo, PrestamoDTO>()
                .ForMember(d => d.Detalles, o => o.MapFrom(s => s.Detalles));
            CreateMap<PrestamoDTO, Prestamo>()
                .ForMember(d => d.Solicitud, o => o.Ignore())
                .ForMember(d => d.Detalles, o => o.MapFrom(s => s.Detalles))
                .ForMember(d => d.Devoluciones, o => o.Ignore())
                .ForMember(d => d.Movimientos, o => o.Ignore());

            CreateMap<PrestamoDetalle, PrestamoDetalleDTO>().ReverseMap()
                .ForMember(d => d.Prestamo, o => o.Ignore())
                .ForMember(d => d.Material, o => o.Ignore());

            CreateMap<Devolucion, DevolucionDTO>().ReverseMap()
                .ForMember(d => d.Prestamo, o => o.Ignore());

            CreateMap<Movimiento, MovimientoDTO>().ReverseMap()
                .ForMember(d => d.Material, o => o.Ignore())
                .ForMember(d => d.Prestamo, o => o.Ignore());

            CreateMap<RegistroReporte, RegistroReporteDTO>().ReverseMap()
                .ForMember(d => d.TipoReporte, o => o.Ignore())
                .ForMember(d => d.Usuario, o => o.Ignore())
                .ForMember(d => d.Movimiento, o => o.Ignore());
        }
    }
}
