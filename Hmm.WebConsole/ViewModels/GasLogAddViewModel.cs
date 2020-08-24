using AutoMapper;
using Hmm.DtoEntity.Api.GasLogNotes;

namespace Hmm.WebConsole.ViewModels
{
    public class GasLogAddViewModel
    {
        private readonly IMapper _mapper;

        public GasLogAddViewModel(IMapper mapper)
        {
            _mapper = mapper;
        }

        public ApiGasLog GasLog { get; set; }

        public void Create()
        {

        }
    }
}