USE msdb;
GO

GRANT EXECUTE ON dbo.sp_send_dbmail TO joboffer_user;
GO

DECLARE @ReturnCode int;
EXEC @ReturnCode = dbo.sp_send_dbmail
    @profile_name = N'HRSMTP',
    @recipients   = N'c_relaude@unilab.com.ph',
    @subject      = N'Database Mail Permission Test',
    @body         = N'<h2>Database Mail test successful</h2>
                      <p>This email was sent using msdb.dbo.sp_send_dbmail.</p>',
    @body_format  = N'HTML',
    @importance   = N'Normal';
