using Domain;
using MGH.Core.Domain.Buses.Commands;

namespace Application.Features.OutBoxes.Commands.UpdateProcessAt;

public class UpdateProcessAtCommand : ICommand
{
    public IEnumerable<Guid> Guids { get; set; }
}

public class UpdateProcessAtCommandHandler(IUow uow) : ICommandHandler<UpdateProcessAtCommand>
{
    public async Task Handle(UpdateProcessAtCommand command, CancellationToken cancellationToken)
    {
        uow.OutBox.Update(command.Guids);
        await uow.CompleteAsync(cancellationToken);
    }
}