using Domain.Entities.Libraries;
using MGH.Core.Domain.Buses.Commands;
using MGH.Core.Infrastructure.Persistence.Persistence.Base;

namespace Application.Features.OutBoxes.Commands.UpdateProcessAt;

public class UpdateProcessAtCommand : ICommand
{
    public IEnumerable<Guid> Guids { get; set; }
}

public class UpdateProcessAtCommandHandler(
    IOutBoxRepository outBoxRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateProcessAtCommand>
{
    public async Task Handle(UpdateProcessAtCommand command, CancellationToken cancellationToken)
    {
        outBoxRepository.Update(command.Guids);
        await unitOfWork.CompleteAsync(cancellationToken);
    }
}