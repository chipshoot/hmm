using System;
using Hmm.DomainEntity.Vehicle;
using Hmm.DtoEntity.Api.GasLogNotes;
using System.Collections.Generic;
using AutoMapper;
using Hmm.Utility.Validation;

namespace Hmm.WebConsole.ViewModels
{
    public class GasLogIndexViewModel
    {

        public GasLogIndexViewModel(IEnumerable<ApiGasLog> apiGaslogs, string address, IMapper mapper)
        {
            Guard.Against<ArgumentNullException>(apiGaslogs==null, nameof(apiGaslogs));
            Guard.Against<ArgumentNullException>(mapper==null, nameof(mapper));

            // ReSharper disable once PossibleNullReferenceException
            GasLogs =  mapper.Map<IEnumerable<GasLog>>(apiGaslogs);
            Address = address;

        }

        public IEnumerable<GasLog> GasLogs { get; }

        public string Address { get; }
    }
}