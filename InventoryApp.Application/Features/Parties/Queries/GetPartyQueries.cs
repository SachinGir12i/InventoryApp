using InventoryApp.Application.Features.Parties.DTOs;
using InventoryApp.Application.Interfaces;
using InventoryApp.Domain.Enums;
using MediatR;

namespace InventoryApp.Application.Features.Parties.Queries
{
    // helper method to avoid repeating mapping in every handler
    internal static class PartyMapper
    {
        public static PartyResponseDto ToDto(Domain.Entities.Party p) => new()
        {
            Id = p.Id,
            Name = p.Name,
            Type = p.Type.ToString(),
            Phone = p.Phone,
            Email = p.Email,
            Address = p.Address,
            City = p.City,
            PANNumber = p.PANNumber,
            VATNumber = p.VATNumber,
            OpeningBalance = p.OpeningBalance,
            CurrentBalance = p.CurrentBalance,
            CreditLimit = p.CreditLimit,
            CreditReminderEnabled = p.CreditReminderEnabled,
            ReminderDaysInterval = p.ReminderDaysInterval,
            CategoryName = p.Category?.Name,
            IsActive = p.IsActive,
            Remarks = p.Remarks,
            CreatedAt = p.CreatedAt
        };
    }

    // ─── Get All Parties ─────────────────────────────────────

    public class GetAllPartiesQuery : IRequest<List<PartyResponseDto>> { }

    public class GetAllPartiesQueryHandler : IRequestHandler<GetAllPartiesQuery, List<PartyResponseDto>>
    {
        private readonly IPartyRepository _repo;
        public GetAllPartiesQueryHandler(IPartyRepository repo) => _repo = repo;

        public async Task<List<PartyResponseDto>> Handle(GetAllPartiesQuery request, CancellationToken cancellationToken)
        {
            var parties = await _repo.GetAllAsync();
            return parties.Select(PartyMapper.ToDto).ToList();
        }
    }

    // ─── Get Party By Id ─────────────────────────────────────

    public class GetPartyByIdQuery : IRequest<PartyResponseDto?>
    {
        public int Id { get; set; }
    }

    public class GetPartyByIdQueryHandler : IRequestHandler<GetPartyByIdQuery, PartyResponseDto?>
    {
        private readonly IPartyRepository _repo;
        public GetPartyByIdQueryHandler(IPartyRepository repo) => _repo = repo;

        public async Task<PartyResponseDto?> Handle(GetPartyByIdQuery request, CancellationToken cancellationToken)
        {
            var party = await _repo.GetByIdAsync(request.Id);
            return party == null ? null : PartyMapper.ToDto(party);
        }
    }

    // ─── Get Parties By Type (Customer or Supplier) ──────────

    public class GetPartiesByTypeQuery : IRequest<List<PartyResponseDto>>
    {
        public PartyType Type { get; set; }
    }

    public class GetPartiesByTypeQueryHandler : IRequestHandler<GetPartiesByTypeQuery, List<PartyResponseDto>>
    {
        private readonly IPartyRepository _repo;
        public GetPartiesByTypeQueryHandler(IPartyRepository repo) => _repo = repo;

        public async Task<List<PartyResponseDto>> Handle(GetPartiesByTypeQuery request, CancellationToken cancellationToken)
        {
            var parties = await _repo.GetByTypeAsync(request.Type);
            return parties.Select(PartyMapper.ToDto).ToList();
        }
    }

    // ─── Search Parties ──────────────────────────────────────

    public class SearchPartiesQuery : IRequest<List<PartyResponseDto>>
    {
        public string SearchTerm { get; set; } = string.Empty;
    }

    public class SearchPartiesQueryHandler : IRequestHandler<SearchPartiesQuery, List<PartyResponseDto>>
    {
        private readonly IPartyRepository _repo;
        public SearchPartiesQueryHandler(IPartyRepository repo) => _repo = repo;

        public async Task<List<PartyResponseDto>> Handle(SearchPartiesQuery request, CancellationToken cancellationToken)
        {
            var parties = await _repo.SearchByNameOrAddressAsync(request.SearchTerm);
            return parties.Select(PartyMapper.ToDto).ToList();
        }
    }
}