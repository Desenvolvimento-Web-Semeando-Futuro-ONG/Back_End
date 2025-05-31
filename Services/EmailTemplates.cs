namespace Back_End.Services
{
    public static class EmailTemplates
    {
        public static string GetResetPasswordTemplate(string resetLink)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; }}
        .container {{ max-width: 600px; margin: auto; padding: 20px; }}
        .button {{ 
            display: inline-block; 
            padding: 10px 20px; 
            background-color: #4CAF50; 
            color: white; 
            text-decoration: none; 
            border-radius: 5px;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <h1>Redefinição de Senha</h1>
        <p>Você solicitou a redefinição de sua senha. Clique no botão abaixo para continuar:</p>
        <a href='{resetLink}' class='button'>Redefinir Senha</a>
        <p>Se você não solicitou esta redefinição, por favor ignore este e-mail.</p>
        <p>O link expirará em 1 hora.</p>
    </div>
</body>
</html>";
        }
    }
}