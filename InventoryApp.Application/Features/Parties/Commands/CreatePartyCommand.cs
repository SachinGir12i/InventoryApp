using InventoryApp.Application.Features.Parties.DTOs;
using InventoryApp.Application.Interfaces;
using InventoryApp.Domain.Entities;
using MediatR;

namespace InventoryApp.Application.Features.Parties.Commands
{
    public class CreatePartyCommand : IRequest<int>
    {
        public CreatePartyDto Party { get; set; } = null!;
    }

    public class CreatePartyCommandHandler : IRequestHandler<CreatePartyCommand, int>
    {
        private readonly IPartyRepository _partyRepository;

        public CreatePartyCommandHandler(IPartyRepository partyRepository)
        {
            _partyRepository = partyRepository;
        }

        public async Task<int> Handle(CreatePartyCommand request, CancellationToken cancellationToken)
        {
            var party = new Party
            {
                Name = request.Party.Name,
                Type = request.Party.Type,
                Phone = request.Party.Phone,
                Email = request.Party.Email,
                Address = request.Party.Address,
                City = request.Party.City,
                PANNumber = request.Party.PANNumber,
                VATNumber = request.Party.VATNumber,
                OpeningBalance = request.Party.OpeningBalance,
                CurrentBalance = request.Party.OpeningBalance, // current balance starts at opening balance
                CreditLimit = request.Party.CreditLimit,
                CreditReminderEnabled = request.Party.CreditReminderEnabled,
                ReminderDaysInterval = request.Party.ReminderDaysInterval,
                PartyCategoryId = request.Party.PartyCategoryId,
                Remarks = request.Party.Remarks
            };

            var created = await _partyRepository.CreateAsync(party);
            return created.Id;
        }
    }
}