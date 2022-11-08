using NSE.Core.Utils;

namespace NSE.Core.DomainObjects
{
    public class Cpf
    {
        public const int CpfMaxLength = 11;

        public string Number { get; private set; }

        //Constructor EF
        protected Cpf() { }

        public Cpf(string number)
        {
            if (!Validate(number)) throw new DomainException("Invalid CPF");
            Number = number;
        }

        public static bool Validate(string cpf)
        {
            cpf = cpf.JustNumbers(cpf);

            if (cpf.Length > 11)
                return false;

            while (cpf.Length != 11)
                cpf = '0' + cpf;

            var same = true;
            for (var i = 1; i < 11 && same; i++)
                if (cpf[i] != cpf[0])
                    same = false;

            if (same || cpf == "12345678909")
                return false;

            var numbers = new int[11];

            for (var i = 0; i < 11; i++)
                numbers[i] = int.Parse(cpf[i].ToString());

            var soma = 0;
            for (var i = 0; i < 9; i++)
                soma += (10 - i) * numbers[i];

            var result = soma % 11;

            if (result == 1 || result == 0)
            {
                if (numbers[9] != 0)
                    return false;
            }
            else if (numbers[9] != 11 - result)
                return false;

            soma = 0;
            for (var i = 0; i < 10; i++)
                soma += (11 - i) * numbers[i];

            result = soma % 11;

            if (result == 1 || result == 0)
            {
                if (numbers[10] != 0)
                    return false;
            }
            else if (numbers[10] != 11 - result)
                return false;

            return true;
        }
    }
}
