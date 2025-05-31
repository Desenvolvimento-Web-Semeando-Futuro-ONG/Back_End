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
        <h1>Redefini��o de Senha</h1>
        <p>Voc� solicitou a redefini��o de sua senha. Clique no bot�o abaixo para continuar:</p>
        <a href='{resetLink}' class='button'>Redefinir Senha</a>
        <p>Se voc� n�o solicitou esta redefini��o, por favor ignore este e-mail.</p>
        <p>O link expirar� em 1 hora.</p>
    </div>
</body>
</html>";
        }
    }
}