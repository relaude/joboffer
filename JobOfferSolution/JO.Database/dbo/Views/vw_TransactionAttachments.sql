CREATE View [dbo].[vw_TransactionAttachments]
As
Select tac.*,jot.TransactionNumber,fty.TypeName 
From TransactionAttachments tac
Left Join JobOfferTransactions jot On jot.Id=tac.Transaction_Id
Left Join FileTypes fty On fty.Id=tac.FileType_Id