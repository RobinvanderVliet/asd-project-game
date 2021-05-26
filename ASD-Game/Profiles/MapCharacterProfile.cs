using AutoMapper;
using DatabaseHandler.POCO;
using DataTransfer.DTO.Character;

namespace ASD_project.Profiles
{
    public class MapCharacterProfile : Profile
    {
        public MapCharacterProfile()
        {
            CreateMap<MapCharacterDTO, PlayerPOCO>();
        }
        
    }
}