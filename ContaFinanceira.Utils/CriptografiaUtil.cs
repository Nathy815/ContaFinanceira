using System;

namespace ContaFinanceira.Utils
{
    public class CriptografiaUtil
    {
        public static string CriptografarSenha(string senha)
        {
            return BCrypt.Net.BCrypt.HashPassword(senha);
        }

        public static bool VerificaSenhaCriptografada(string senhaCriptografada, string senha)
        {
            return BCrypt.Net.BCrypt.Verify(senha, senhaCriptografada);
        }
    }
}
