using System;
using System.Collections.Generic;

namespace NumberSystemApp
{
    class IntegerInBase
    {
        private int[] digits; // разряды числа (младший разряд первым)
        private int numberBase; // система счисления

        public int Base => numberBase;

        // Конструктор из строки и основания
        public IntegerInBase(string numberStr, int baseValue)
        {
            if (baseValue < 2 || baseValue > 36)
                throw new ArgumentException("Основание должно быть от 2 до 36");

            this.numberBase = baseValue;
            List<int> digitList = new List<int>();

            foreach (char c in numberStr.ToUpper())
            {
                int digit;
                if (char.IsDigit(c))
                    digit = c - '0';
                else if (c >= 'A' && c <= 'Z')
                    digit = c - 'A' + 10;
                else
                    throw new ArgumentException($"Недопустимый символ: {c}");

                if (digit >= baseValue)
                    throw new ArgumentException($"Цифра {digit} недопустима для основания {baseValue}");

                digitList.Insert(0, digit); // младшие разряды слева
            }

            digits = digitList.ToArray();
        }

        // Вывод числа
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = digits.Length - 1; i >= 0; i--)
            {
                if (digits[i] < 10)
                    sb.Append((char)('0' + digits[i]));
                else
                    sb.Append((char)('A' + digits[i] - 10));
            }
            return sb.ToString();
        }

        // Перевод числа в десятичную систему
        public int ToDecimal()
        {
            int result = 0;
            int power = 1;

            foreach (int digit in digits)
            {
                result += digit * power;
                power *= numberBase;
            }

            return result;
        }

        // Перевод из десятичной в другую систему
        public static IntegerInBase FromDecimal(int value, int targetBase)
        {
            if (targetBase < 2 || targetBase > 36)
                throw new ArgumentException("Основание должно быть от 2 до 36");

            List<int> resultDigits = new List<int>();

            if (value == 0)
                resultDigits.Add(0);

            while (value > 0)
            {
                resultDigits.Add(value % targetBase);
                value /= targetBase;
            }

            string resultStr = "";
            for (int i = resultDigits.Count - 1; i >= 0; i--)
            {
                if (resultDigits[i] < 10)
                    resultStr += (char)('0' + resultDigits[i]);
                else
                    resultStr += (char)('A' + resultDigits[i] - 10);
            }

            return new IntegerInBase(resultStr, targetBase);
        }

        // Сложение
        public static IntegerInBase operator +(IntegerInBase a, IntegerInBase b)
        {
            if (a.numberBase != b.numberBase)
                throw new InvalidOperationException("Числа должны быть в одной системе счисления");

            int decimalResult = a.ToDecimal() + b.ToDecimal();
            return FromDecimal(decimalResult, a.numberBase);
        }

        // Вычитание
        public static IntegerInBase operator -(IntegerInBase a, IntegerInBase b)
        {
            if (a.numberBase != b.numberBase)
                throw new InvalidOperationException("Числа должны быть в одной системе счисления");

            int decimalResult = a.ToDecimal() - b.ToDecimal();
            return FromDecimal(decimalResult, a.numberBase);
        }

        // Умножение
        public static IntegerInBase operator *(IntegerInBase a, IntegerInBase b)
        {
            if (a.numberBase != b.numberBase)
                throw new InvalidOperationException("Числа должны быть в одной системе счисления");

            int decimalResult = a.ToDecimal() * b.ToDecimal();
            return FromDecimal(decimalResult, a.numberBase);
        }

        // Деление
        public static IntegerInBase operator /(IntegerInBase a, IntegerInBase b)
        {
            if (a.numberBase != b.numberBase)
                throw new InvalidOperationException("Числа должны быть в одной системе счисления");

            int decimalResult = a.ToDecimal() / b.ToDecimal();
            return FromDecimal(decimalResult, a.numberBase);
        }

        // Остаток от деления
        public static IntegerInBase operator %(IntegerInBase a, IntegerInBase b)
        {
            if (a.numberBase != b.numberBase)
                throw new InvalidOperationException("Числа должны быть в одной системе счисления");

            int decimalResult = a.ToDecimal() % b.ToDecimal();
            return FromDecimal(decimalResult, a.numberBase);
        }

        // Равно
        public override bool Equals(object obj)
        {
            if (!(obj is IntegerInBase other)) return false;
            return this.ToDecimal() == other.ToDecimal();
        }

        public override int GetHashCode()
        {
            return ToDecimal().GetHashCode();
        }

        // Больше
        public static bool operator >(IntegerInBase a, IntegerInBase b)
        {
            return a.ToDecimal() > b.ToDecimal();
        }

        // Меньше
        public static bool operator <(IntegerInBase a, IntegerInBase b)
        {
            return a.ToDecimal() < b.ToDecimal();
        }

        // Больше или равно
        public static bool operator >=(IntegerInBase a, IntegerInBase b)
        {
            return a.ToDecimal() >= b.ToDecimal();
        }

        // Меньше или равно
        public static bool operator <=(IntegerInBase a, IntegerInBase b)
        {
            return a.ToDecimal() <= b.ToDecimal();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("Введите первое число:");
            string input1 = Console.ReadLine();

            Console.WriteLine("Введите его систему счисления (2-36):");
            int base1 = int.Parse(Console.ReadLine());

            Console.WriteLine("Введите второе число:");
            string input2 = Console.ReadLine();

            Console.WriteLine("Введите его систему счисления (2-36):");
            int base2 = int.Parse(Console.ReadLine());

            try
            {
                IntegerInBase num1 = new IntegerInBase(input1, base1);
                IntegerInBase num2 = new IntegerInBase(input2, base2);

                // Если системы не совпадают, переводим оба числа в одну
                if (num1.Base != num2.Base)
                {
                    num2 = IntegerInBase.FromDecimal(num2.ToDecimal(), num1.Base);
                    Console.WriteLine($"\nЧисло 2 переведено в систему {num1.Base}: {num2}");
                }

                Console.WriteLine($"\nnum1 = {num1}, num2 = {num2}");
                Console.WriteLine($"num1 + num2 = {num1 + num2}");
                Console.WriteLine($"num1 - num2 = {num1 - num2}");
                Console.WriteLine($"num1 * num2 = {num1 * num2}");
                Console.WriteLine($"num1 / num2 = {num1 / num2}");
                Console.WriteLine($"num1 % num2 = {num1 % num2}");
                Console.WriteLine($"num1 > num2? {num1 > num2}");
                Console.WriteLine($"num1 в десятичной: {num1.ToDecimal()}");
                Console.WriteLine($"num2 в десятичной: {num2.ToDecimal()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}
