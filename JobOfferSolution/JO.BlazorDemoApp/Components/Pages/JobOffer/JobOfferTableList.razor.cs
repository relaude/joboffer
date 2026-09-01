using JO.DataModel.View;
using JO.Service.Constants;
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
                    JOUserRole.TA => jobOffer.WorkFlowId switch
                    {
                        2 => JORoutes.TA.Analysis,
                        8 => JORoutes.TA.Discussion,
                        9 => JORoutes.TA.JobOfferComplete,
                        10 => JORoutes.TA.AnalysisSendBack,
                        _ => JORoutes.TA.JobOfferDetails
                    },

                    JOUserRole.TALead => jobOffer.WorkFlowId switch
                    {
                        3 => JORoutes.TALead.JOForReview,
                        10 => JORoutes.TALead.AnalysisSendBack,
                        _ => JORoutes.TALead.JobOfferDetails
                    },

                    JOUserRole.PEHead => jobOffer.WorkFlowId == 4
                        ? JORoutes.PEH.JOForApproval
                        : JORoutes.PEH.JobOfferDetails,

                    JOUserRole.DivisionHeadApproverL1 => jobOffer.WorkFlowId == 5
                        ? JORoutes.DHL1.JOForApproval
                        : JORoutes.DHL1.JobOfferDetails,

                    JOUserRole.HRODHeadApprover => jobOffer.WorkFlowId == 6
                        ? JORoutes.HRODHead.JOForApproval
                        : JORoutes.HRODHead.JobOfferDetails,

                    JOUserRole.President => jobOffer.WorkFlowId == 7
                        ? JORoutes.President.JOForApproval
                        : JORoutes.President.JobOfferDetails,

                    _ => throw new InvalidOperationException(
                        "Supply a supported Role or configure Routes and DefaultRoute for JobOfferTableList.")
                };
            }

            return $"{route.TrimEnd('/')}/{jobOffer.Id}";
        }
    }
}
