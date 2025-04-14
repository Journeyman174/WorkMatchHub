namespace MobyLabWebProgramming.Core.Constants;

/// <summary>
/// Here we have a class that provides HTML template for mail bodies. You ami add or change the template if you like.
/// </summary>
public static class MailTemplates
{
    public static string UserAddTemplate(string name) => $@"<!DOCTYPE html>
<html lang=""en"" xmlns=""http://www.w3.org/1999/xhtml"">
<head>
    <meta charset=""utf-8"" />
    <title>Mail</title>
    <style type=""text/css"">
        p {{
            margin: 0 0 5px 0;
        }}
    </style>
</head>
<body>
    <table style=""border: none; height: 48px;"" border=""0"" width=""676"" cellspacing=""0"" cellpadding=""10"" align=""center"" bgcolor=""#FFFFFF"">
        <tbody>
            <tr style=""height: 19px;"">
                <td style=""background: #ffffff; color: #000000; width: 660px; height: 19px;"" align=""left"" valign=""top"" bgcolor=""#ffffff"">
                    <p style=""font-size: 15px; margin: 0px; color: #003373; letter-spacing: 0.5px;""><strong> Dear mr./ms. {name},</strong></p>
                </td>
            </tr>
            <tr style=""height: 102px;"">
                <td style=""background: #ffffff; color: #000000; height: 29px; width: 660px;"" align=""center"" valign=""top"" bgcolor=""#ffffff"">
                    <table style=""border-top: 2px solid #3c87be; width: 660px;"" border=""0"" cellspacing=""0"" cellpadding=""0"">
                        <tbody>
                            <tr>
                                <td>Welcome to WorkMatchHub !</td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
</body>
</html>";
    public static string JobAssignmentAcceptedTemplate(string name, string jobTitle, string jobDescription) => $@"<!DOCTYPE html>
<html lang=""en"" xmlns=""http://www.w3.org/1999/xhtml"">
<head>
    <meta charset=""utf-8"" />
    <title>Job Assignment Notification</title>
    <style type=""text/css"">
        body {{
            font-family: Arial, sans-serif;
        }}
        .container {{
            width: 660px;
            margin: auto;
            padding: 20px;
            background-color: #ffffff;
            color: #000000;
        }}
        .job-title {{
            font-size: 16px;
            font-weight: bold;
            color: #2a6496;
        }}
        .job-description {{
            background-color: #f2f2f2;
            padding: 10px;
            border-radius: 5px;
            margin-top: 10px;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <p><strong>Dear {name},</strong></p>
        <p>Congratulations!</p>
        <p>You’ve been accepted for the job: <span class=""job-title"">{jobTitle}</span>.</p>
        <div class=""job-description"">
            <p><strong>Job Description:</strong></p>
            <p>{jobDescription}</p>
        </div>
        <p>Log into <strong>WorkMatchHub</strong> to see more details and prepare for your new opportunity.</p>
        <p style=""margin-top: 20px;"">Best regards,<br/><em>The WorkMatchHub Team</em></p>
    </div>
</body>
</html>";

}