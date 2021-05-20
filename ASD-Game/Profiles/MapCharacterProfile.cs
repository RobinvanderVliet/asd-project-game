using AutoMapper;
using DatabaseHandler.Poco;
using DataTransfer.DTO.Character;

namespace ASD_project.Profiles
{
    public class MapCharacterProfile : Profile
    {
        public MapCharacterProfile()
        {
            CreateMap<MapCharacterDTO, PlayerPoco>();
        }
        
    }
}