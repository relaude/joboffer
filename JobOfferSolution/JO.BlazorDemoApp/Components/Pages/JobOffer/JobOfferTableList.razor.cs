using JO.DataModel.DTOs;
using JO.DataModel.View;
using JO.Service.Constants;
using JO.Service.Extensions;
using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.JobOffer
{
    public partial class JobOfferTableList
    {
        /// <summary>Pass the parent's filtered list so KPI selections update the rows.</summary>
        [Parameter, EditorRequired]
        public IReadOnlyList<VwJODboxCandidates> JobOffers { get; set; } = [];

        /// <summary>
        /// Selects link defaults only; authorization and eligibility remain in the parent.
        /// Supports TA, TALead, DivisionHeadApproverL1, HRODHeadApprover, PEHead, and President.
        /// </summary>
        [Parameter, EditorRequired]
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// Optional workflow ID to base-route overrides. The job offer ID is appended.
        /// These overrides take precedence over DefaultRoute and role defaults.
        /// </summary>
        [Parameter]
        public IReadOnlyDictionary<int, string> Routes { get; set; } = new Dictionary<int, string>();

        /// <summary>
        /// Optional base route for workflows absent from Routes. When omitted,
        /// the component uses the role's existing workflow navigation rules.
        /// Supply a base path, not a route template containing {jobOfferId:int}.
        /// </summary>
        [Parameter]
        public string? DefaultRoute { get; set; }

        [Parameter]
        public string Title { get; set; } = "Job Offers";

        private PagedResult<VwJODboxCandidates> pagedJobOffers = new() { Page = 1, PageSize = 10 };
        private int[] previousJobOfferIds = [];

        protected override void OnParametersSet()
        {
            var jobOfferIds = JobOffers.Select(jobOffer => jobOffer.Id).ToArray();
            var page = previousJobOfferIds.SequenceEqual(jobOfferIds) ? pagedJobOffers.Page : 1;
            previousJobOfferIds = jobOfferIds;
            ChangePage(page);
        }

        private void ChangePage(int page) =>
            pagedJobOffers = JobOffers.ToPagedResult(page, pagedJobOffers.PageSize);

        private void ChangePageSize(int pageSize) =>
            pagedJobOffers = JobOffers.ToPagedResult(1, pageSize);

        private string SetJOlink(VwJODboxCandidates jobOffer)
        {
            string route;
            if (jobOffer.WorkFlowId is int workflowId
                && Routes.TryGetValue(workflowId, out var workflowRoute)
                && !string.IsNullOrWhiteSpace(workflowRoute))
            {
                route = workflowRoute;
            }
            else if (!string.IsNullOrWhiteSpace(DefaultRoute))
            {
                route = DefaultRoute;
            }
            else
            {
                route = Role switch
                {
                    JOUserRole.TAPartner => jobOffer.WorkFlowId switch
                    {
                        2 => JORoutes.TAPartner.Analysis,
                        8 => JORoutes.TAPartner.Discussion,
                        9 or 12 => JORoutes.TAPartner.JobOfferComplete,
                        10 => JORoutes.TAPartner.SendBackAnalysis,
                        11 => JORoutes.TAPartner.ForNegotiation,
                        _ => JORoutes.TAPartner.JobOfferDetails
                    },

                    JOUserRole.TALead => jobOffer.WorkFlowId switch
                    {
                        3 => JORoutes.TALead.JOForReview,
                        8 => JORoutes.TALead.Discussion,
                        10 => JORoutes.TALead.SendBackAnalysis,
                        11 => JORoutes.TALead.ForNegotiation,
                        9 or 12 => JORoutes.TALead.JobOfferComplete,
                        _ => JORoutes.TALead.JobOfferDetails
                    },

                    JOUserRole.PEHead => jobOffer.WorkFlowId switch
                    {
                        4 => JORoutes.PEH.JOForApproval,
                        14 => JORoutes.PEH.JOForReview,
                        _ => JORoutes.PEH.JobOfferDetails
                    },

                    JOUserRole.DivisionHeadApproverL1 => jobOffer.WorkFlowId == 5
                        ? JORoutes.DHL1.JOForApproval
                        : JORoutes.DHL1.JobOfferDetails,

                    JOUserRole.HRODHeadApprover => jobOffer.WorkFlowId switch
                    {
                        6 or 15 => JORoutes.HRODHead.JOForApproval,
                        _ => JORoutes.HRODHead.JobOfferDetails
                    },

                    JOUserRole.DivisionHeadApproverL2 => jobOffer.WorkFlowId == 13
                        ? JORoutes.DHL2.JOForApproval
                        : JORoutes.DHL2.JobOfferDetails,

                    JOUserRole.President => jobOffer.WorkFlowId == 7
                        ? JORoutes.President.JOForApproval
                        : JORoutes.President.JobOfferDetails,

                    JOUserRole.Admin => JORoutes.Admin.JobOfferComplete,

                    _ => throw new InvalidOperationException(
                        "Supply a supported Role or configure Routes and DefaultRoute for JobOfferTableList.")
                };
            }

            return $"{route.TrimEnd('/')}/{jobOffer.Id}";
        }
    }
}
