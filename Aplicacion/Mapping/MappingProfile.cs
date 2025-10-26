using Aplication.DTOs;
using AutoMapper;
using Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Entity -> DTO
            CreateMap<Curso, CursoDTO>()
                .ForMember(dest => dest.DocenteId, opt => opt.MapFrom(src => src.Id_docente))
                .ForMember(dest => dest.FechaInicio, opt => opt.MapFrom(src => src.Fecha_inicio))
                .ForMember(dest => dest.FechaFin, opt => opt.Ignore());

            // DTO -> Entity
            CreateMap<CursoDTO, Curso>()
                .ForMember(dest => dest.Id_docente, opt => opt.MapFrom(src => src.DocenteId))
                .ForMember(dest => dest.Fecha_inicio, opt => opt.MapFrom(src => src.FechaInicio))
                .ForMember(dest => dest.docente, opt => opt.Ignore());

            CreateMap<Docente, DocenteDTO>().ReverseMap();
        }
    }
}
