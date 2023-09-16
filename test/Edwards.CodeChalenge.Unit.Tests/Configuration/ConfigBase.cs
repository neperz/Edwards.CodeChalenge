using AutoMapper;
using Edwards.CodeChalenge.API.AutoMapper;

namespace Edwards.CodeChalenge.Unit.Tests.Configuration
{
    public class ConfigBase
    {
        public readonly IMapper _mapper;

        public ConfigBase()
        {

            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new MappingProfiles());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }
    }
}
