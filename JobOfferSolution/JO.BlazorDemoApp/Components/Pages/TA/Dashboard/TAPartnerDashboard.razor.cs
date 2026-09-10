namespace JO.BlazorDemoApp.Components.Pages.TA.Dashboard
{
    public partial class TAPartnerDashboard
    {
        [Microsoft.AspNetCore.Components.Inject]
        private Microsoft.AspNetCore.Components.NavigationManager Navigation { get; set; } = default!;

        [Microsoft.AspNetCore.Components.Inject]
        private JO.Service.Services.Contracts.IJODetailsService JODetailsService { get; set; } = default!;

        [Microsoft.AspNetCore.Components.Inject]
        private JO.Service.Services.Contracts.ICandidateService CandidateService { get; set; } = default!;

        private int candidateTotal;
        private int withResponse;
        private int withOutResponse;
        private int forJOCreation;
        private int joDraft;
        private int joCreated;

        private List<JO.DataModel.View.VwDboxCandidates> pendingCandidates = new();
        private JO.DataModel.DTOs.PagedResult<JO.DataModel.View.VwDboxCandidates> pagedCandidates = new() { Page = 1, PageSize = 10 };
        private bool isLoadingCandidates = true;

        private void ChangeCandidatePage(int page) =>
            pagedCandidates = JO.Service.Extensions.PaginationExtension.ToPagedResult(pendingCandidates, page, pagedCandidates.PageSize);

        private void ChangeCandidatePageSize(int pageSize) =>
            pagedCandidates = JO.Service.Extensions.PaginationExtension.ToPagedResult(pendingCandidates, 1, pageSize);

        private async Task OpenCandidate(JO.DataModel.View.VwDboxCandidates candidate)
        {
            var candidateLink = await CandidateService.GetCandidateLink(candidate);
            Navigation.NavigateTo(candidateLink);
        }

        private int total;
        private List<JO.DataModel.View.VwJODboxCandidates> pendingJobOffers = new();
        private JO.DataModel.DTOs.PagedResult<JO.DataModel.View.VwJODboxCandidates> pagedJobOffers = new() { Page = 1, PageSize = 10 };
        private bool isLoadingJobOffers = true;

        private void ChangeJobOfferPage(int page) =>
            pagedJobOffers = JO.Service.Extensions.PaginationExtension.ToPagedResult(pendingJobOffers, page, pagedJobOffers.PageSize);

        private void ChangeJobOfferPageSize(int pageSize) =>
            pagedJobOffers = JO.Service.Extensions.PaginationExtension.ToPagedResult(pendingJobOffers, 1, pageSize);

        // JOUserRole.TAPartner navigation rules from JobOfferTableList.
        private static string SetJOlink(JO.DataModel.View.VwJODboxCandidates jobOffer)
        {
            var route = jobOffer.WorkFlowId switch
            {
                2 or 10 => JO.Service.Constants.JORoutes.TAPartner.Analysis,
                8 => JO.Service.Constants.JORoutes.TAPartner.Discussion,
                9 or 12 => JO.Service.Constants.JORoutes.TAPartner.JobOfferComplete,
                11 => JO.Service.Constants.JORoutes.TAPartner.ForNegotiation,
                _ => JO.Service.Constants.JORoutes.TAPartner.JobOfferDetails
            };

            return $"{route.TrimEnd('/')}/{jobOffer.Id}";
        }

        private int countForReview;
        private int countSendBack;
        private int countForApproval;
        private int countForDiscussion;
        private int countAcccepted;
        private int countForNegotiation;
        private int countDeclined;

        protected override async Task OnInitializedAsync()
        {
            var jobOffers = await JODetailsService.GetVwJODboxCandidates();
            var candidates = await JODetailsService.GetVwTAPartnerDboxCandidates();
            var candidateIds = candidates.Select(candidate => candidate.DboxRefNum).ToHashSet();
            var trackableJobOffers = jobOffers
                .Where(offer => candidateIds.Contains(offer.DboxRefNum) && offer.WorkFlowId > 1)
                .ToList();

            total = trackableJobOffers.Count;
            countForReview = trackableJobOffers.Count(offer => offer.WorkFlowId == 3);
            countSendBack = trackableJobOffers.Count(offer => offer.WorkFlowId == 10);
            countForApproval = trackableJobOffers.Count(offer => offer.WorkFlowId is 4 or 5 or 6 or 7 or 13);
            countForDiscussion = trackableJobOffers.Count(offer => offer.WorkFlowId == 8);
            countAcccepted = trackableJobOffers.Count(offer => offer.WorkFlowId == 9);
            countForNegotiation = trackableJobOffers.Count(offer => offer.WorkFlowId == 11);
            countDeclined = trackableJobOffers.Count(offer => offer.WorkFlowId == 12);

            pendingJobOffers = trackableJobOffers
                .Where(offer => offer.WorkFlowId is 8 or 10 or 11)
                .OrderBy(offer => offer.WorkFlowId)
                .ThenBy(offer => offer.Id)
                .ToList();
            ChangeJobOfferPage(1);
            isLoadingJobOffers = false;

            var allCandidates = await CandidateService.GetVwDboxCandidates();
            var eligibleCandidates = allCandidates
                .Where(candidate => candidate.CSGId is not (1 or 109) && candidate.DivisionId != 3)
                .ToList();

            candidateTotal = eligibleCandidates.Count;
            withResponse = eligibleCandidates.Count(candidate => candidate.ResponseId > 0);
            withOutResponse = candidateTotal - withResponse;
            forJOCreation = eligibleCandidates.Count(candidate => candidate.StatusId is 1 or 4);
            joDraft = eligibleCandidates.Count(candidate => candidate.StatusId == 2);
            joCreated = eligibleCandidates.Count(candidate => candidate.StatusId == 3);

            pendingCandidates = eligibleCandidates
                .Where(candidate => candidate.StatusId is 1 or 2 or 4)
                .OrderBy(candidate => candidate.CandidateName)
                .ThenBy(candidate => candidate.Id)
                .ToList();
            ChangeCandidatePage(1);
            isLoadingCandidates = false;
        }

        private int PipelineWidth(int count) => total == 0 ? 0 : (int)Math.Round(100d * count / total);

        private int CandidatePipelineWidth(int count) => candidateTotal == 0 ? 0 : (int)Math.Round(100d * count / candidateTotal);

        // Response stages precede JO creation, draft and created; response and status counts overlap.
        private PipelineStage[] CandidatePipeline =>
        [
            new("No Response", withOutResponse, CandidatePipelineWidth(withOutResponse), "#f6c23e"),
            new("With Response", withResponse, CandidatePipelineWidth(withResponse), "#36b9cc"),
            new("For JO Creation", forJOCreation, CandidatePipelineWidth(forJOCreation), "#4e73df"),
            new("JO Draft", joDraft, CandidatePipelineWidth(joDraft), "#858796"),
            new("JO Created", joCreated, CandidatePipelineWidth(joCreated), "#1cc88a")
        ];

        private void GoToJobOffers() => Navigation.NavigateTo(JO.Service.Constants.JORoutes.TAPartner.JobOfferTracker);

        private void GoToCandidates() => Navigation.NavigateTo(JO.Service.Constants.JORoutes.TAPartner.Candidates);

        private void GoToEmailTemplates() => Navigation.NavigateTo(JO.Service.Constants.JORoutes.TAPartner.EmailTemplates);

        // Workflow ID order; approval combines workflow IDs 4, 5, 6, 7 and 13, as in JobOfferList.
        private PipelineStage[] Pipeline =>
        [
            new("For Review", countForReview, PipelineWidth(countForReview), "#4e73df"),
            new("For Approval", countForApproval, PipelineWidth(countForApproval), "#858796"),
            new("For Discussion", countForDiscussion, PipelineWidth(countForDiscussion), "#36b9cc"),
            new("Accepted", countAcccepted, PipelineWidth(countAcccepted), "#1cc88a"),
            new("Send Back", countSendBack, PipelineWidth(countSendBack), "#f6c23e"),
            new("For Negotiation", countForNegotiation, PipelineWidth(countForNegotiation), "#e74a3b"),
            new("Declined", countDeclined, PipelineWidth(countDeclined), "#c53d32")
        ];

        private sealed record PipelineStage(string Label, int Count, int Width, string Color);
    }
}
