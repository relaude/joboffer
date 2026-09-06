
CREATE View [dbo].[vw_EmailTemplate]
As
Select etmp.Id,etmp.EmailSubject,etmp.IsActive
,etmp.CreatedAt,jouc.Name CreatedByName
,etmp.ModifiedAt,joum.Name ModifiedByName
From EmailTemplate etmp
Left Join JobOfferUsers jouc On jouc.Id=etmp.CreatedBy
Left Join JobOfferUsers joum On joum.Id=etmp.ModifiedBy;