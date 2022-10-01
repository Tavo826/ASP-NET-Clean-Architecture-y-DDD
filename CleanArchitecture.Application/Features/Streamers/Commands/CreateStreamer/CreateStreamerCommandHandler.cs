using AutoMapper;
using CleanArchitecture.Application.Contracts.Infrastructure;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Models;
using CleanArchitecture.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Features.Streamers.Commands.CreateStreamer
{
    public class CreateStreamerCommandHandler : IRequestHandler<CreateStreamerCommand, int>
    {

        private readonly ILogger<CreateStreamerCommandHandler> _logger;

        // Se reemplaza el repositorio por el UnitOfWork
        //private readonly IStreamerRepository _streamerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        // cada que se registra un streamer, se envía un correo electrónico
        private readonly IEmailService _emailService;

        public CreateStreamerCommandHandler(ILogger<CreateStreamerCommandHandler> logger, /*IStreamerRepository streamerRepository*/ IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService)
        {
            _logger = logger;
            //_streamerRepository = streamerRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailService = emailService;
        }

        public async Task<int> Handle(CreateStreamerCommand request, CancellationToken cancellationToken)
        {
            var streamerEntity = _mapper.Map<Streamer>(request);

            //var newStreamer = await _streamerRepository.AddAsync(streamerEntity);
            _unitOfWork.StreamerRepository.AddEntity(streamerEntity);
            var result = await _unitOfWork.Complete();

            if (result <= 0)
            {
                throw new Exception($"No se pudo insertar el record de Streamer");
            }

            //_logger.LogInformation($"Streamer {newStreamer.Id} creado");
            _logger.LogInformation($"Streamer {streamerEntity.Id} creado");

            //await SendEmail(newStreamer);
            await SendEmail(streamerEntity);

            //return newStreamer.Id;
            return streamerEntity.Id;
        }

        private async Task SendEmail(Streamer streamer)
        {
            var email = new Email
            {
                To = "9gagigor816@gmail.com",
                Subject = "Streamer Creado",
                Body = $"La compañía de streamer {streamer.Nombre} se creó correctamente"
            };

            try
            {
                await _emailService.SendEmail(email);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error enviando el email {streamer.Nombre} con mensaje {ex.Message}");
            }
        }
    }
}
