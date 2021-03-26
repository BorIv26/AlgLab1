using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace LabWork1
{
    class BigInt : ICloneable
    {
        public char sign; // символ + или -
        public List<int> number;
        public BigInt()
        {
            sign = '+';
            number = new List<int>() { 0 };
        }

        public BigInt(char Sign, List<int> Number)
        {
            sign = Sign;
            number = Number;
            if (number.Count > 1)
                this.RemoveZeroes();
        }

        public BigInt(int a)
        {
            if (Math.Abs(a) > 9) throw new ArgumentException();
            else
            {
                number.Add(a);
                if (a != 0)
                    sign = '-';
                else sign = '+';
            }
        }

        public BigInt(string fromStrToBigInt)
        {
            number = new List<int>();
            if (char.IsDigit(fromStrToBigInt[0]))
            {
                sign = '+';
            }
            else if (fromStrToBigInt[0] == '+')
            {
                fromStrToBigInt = fromStrToBigInt.Remove(0, 1);
                sign = '+';
            }
            else if (fromStrToBigInt[0] == '-')
            {
                fromStrToBigInt = fromStrToBigInt.Remove(0, 1);
                sign = '-';
            }
            else
                throw new Exception("wrong string");
            for (int i = 0; i < fromStrToBigInt.Length; i++)
                number.Add(int.Parse(fromStrToBigInt[i].ToString()));
            this.RemoveZeroes();
        }
        public void RemoveZeroes()
        {
            var size = number.Count;
            for (int i = 0; i < size; i++)
            {
                if (number[i] != 0)
                {
                    number.RemoveRange(0, i);
                    break;
                }
            }
            if (number.First() == 0 && size == number.Count) number = new List<int>() { 0 };
        }

        public void AddZeroesInBeginToMakeNumsEqual(int a)
        {
            if (a >= number.Count)
                while (number.Count < a)
                    number.InsertRange(0, new List<int> { 0 });
        }

        public void AddZeroesAndSetNumToIndex(int index, int val)
        {
            while (number.Count <= index)
            {
                number.Add(0);
            }

            number[index] = val;
        }
        public object Clone()
        {
            return new BigInt { number = new List<int>(this.number), sign = this.sign };
        }

        public static BigInt BigPlusBig(BigInt a, BigInt b)
        {
            var sum = new BigInt();
            var aNum = a.number;
            var bNum = b.number;
            var maxlength = Math.Max(a.number.Count, b.number.Count) - 1;
            var difference = 0;
            if (aNum.Count < bNum.Count)
            {
                a.AddZeroesInBeginToMakeNumsEqual(b.number.Count);
                sum.AddZeroesInBeginToMakeNumsEqual(b.number.Count);
            }
            else
            {
                b.AddZeroesInBeginToMakeNumsEqual(a.number.Count);
                sum.AddZeroesInBeginToMakeNumsEqual(a.number.Count);
            }
            for (var i = maxlength; i >= 0; i--)
            {
                var temp = aNum[i] + bNum[i] + difference;
                if (temp >= 10)
                {
                    difference = 1;
                    sum.number[i] = temp - 10;
                }
                else
                {
                    difference = 0;
                    sum.number[i] = temp;
                }
            }
            if (difference == 1)
                sum.number.Insert(0, 1);
            sum.RemoveZeroes();
            return sum;
        }

        public static BigInt BigMinusBig(BigInt a, BigInt b)
        {
            if (b.sign == '-')
                return BigPlusBig(a, b);
            a.RemoveZeroes();
            b.RemoveZeroes();
            b.AddZeroesInBeginToMakeNumsEqual(a.number.Count);
            var result = new BigInt();
            var aNum = new List<int>(a.number);
            var bNum = new List<int>(b.number);
            for (var i = a.number.Count - 1; i >= 0; i--)
            {
                var temp = aNum[i] - bNum[i];
                if (temp < 0)
                {
                    aNum[i - 1]--;
                    temp = 10 + temp;
                }
                result.AddZeroesAndSetNumToIndex(i, temp);
            }
            result.RemoveZeroes();
            return result;
        }
        public BigInt MultOnPow10InN(int n) // умножение на 10^n
        {
            this.number.AddRange(new int[n]);
            return this;
        }

        public static BigInt Mod(BigInt a, BigInt b)//result>0
        {
            var a1 = new BigInt('+', new List<int>(a.number));
            a1.RemoveZeroes();
            b.RemoveZeroes();
            if (a1 < b)
                return a1;
            else
            if (a1 == b)
                return new BigInt('+', new List<int> { 0 });
            else
            {
                while (a1 > b)
                {
                    var bTemp1 = new BigInt(b.sign, new List<int>(b.number));
                    BigInt temp1;
                    var aAndbLengthDifference = new BigInt((a1.number.Count - b.number.Count).ToString());
                    bTemp1 *= new BigInt("10").Pow(aAndbLengthDifference);
                    if (bTemp1 > a1)
                        temp1 = aAndbLengthDifference - new BigInt("1");
                    else
                        temp1 = aAndbLengthDifference;
                    var tempToPow = new BigInt("10").Pow(temp1);
                    var tempToMult = b * tempToPow.MultOnDigit(1);
                    for (var i = 1; b * tempToPow.MultOnDigit(i) < a1; i++)
                    {
                        var differenceToMultOnDig = tempToPow.MultOnDigit(i);
                        tempToMult = differenceToMultOnDig;
                    }
                    if (a1 > tempToMult * b)
                    {
                        if (a1 - tempToMult * b < b)
                        {

                            a1 -= tempToMult * b;
                            break;
                        }
                    }
                    else
                        break;
                    a1 -= tempToMult * b;
                }
                a1.RemoveZeroes();
                return a1;
            }
        }
        public static int CompareWithSign(BigInt V1, BigInt V2)
        {
            if (V1.sign == '+' && V2.sign == '-')
                return 1;
            else if (V2.sign == '+' && V1.sign == '-')
                return -1;
            else if (V1.sign == '+' && V2.sign == '+')
            {
                return CompareWithoutSign(V1, V2);
            }
            else
                return -1 * CompareWithoutSign(V1, V2);
        }

        public static int CompareWithoutSign(BigInt v1, BigInt v2)
        {
            v1.RemoveZeroes(); v2.RemoveZeroes();
            if (v1.number.Count > v2.number.Count)
                return 1;
            else if (v1.number.Count < v2.number.Count)
                return -1;
            else
            {
                for (var i = 0; i <= v1.number.Count - 1; i++)
                {
                    if (v1.number[i] > v2.number[i])
                        return 1;
                    else if (v1.number[i] < v2.number[i])
                        return -1;
                }
                return 0;
            }
        }

        public static BigInt EuqlEx(BigInt a, BigInt b, out BigInt x, out BigInt y)
        {
            if (a == new BigInt())
            {
                x = new BigInt();
                y = new BigInt("1");
                return b;
            }
            BigInt tempX, tempY;
            var d = EuqlEx(b % a, a, out tempX, out tempY);
            x = tempY - (b / a) * tempX;
            y = tempX;
            return d;
        }
        public static BigInt ReverseModule(BigInt a, BigInt b)
        {
            BigInt g = EuqlEx(a, b, out var x, out var y);
            if (g != new BigInt("1"))
                throw new Exception("No solution");
            return (x % b + b) % b;
        }

        public static BigInt Div(BigInt a, BigInt b) // округление происходит в левую сторону: (-10)/10 = -1
        {
            var a1 = new BigInt('+', new List<int>(a.number));
            a1.RemoveZeroes();
            b.RemoveZeroes();
            if (a1 < b)
                return new BigInt();
            else
            if (a1 == b)
                return new BigInt("1");
            else
            {
                var res = new List<BigInt>();
                while (a1 > b)
                {
                    var restemp = new BigInt();
                    var bTemp1 = new BigInt(b.sign, new List<int>(b.number));
                    var bTempToPow = new BigInt(b.sign, new List<int>(b.number));
                    var differenceForPow = 0;
                    var aAndbLengthDifference = a1.number.Count - b.number.Count;
                    bTempToPow = bTempToPow.MultOnPow10InN(aAndbLengthDifference);
                    if (bTempToPow > a1)
                        differenceForPow = aAndbLengthDifference - 1;
                    else
                        differenceForPow = aAndbLengthDifference;
                    var bTempToPowAndMult = new BigInt('+' + (Math.Pow(10, differenceForPow)).ToString());

                    for (var i = 1; b * bTempToPowAndMult.MultOnDigit(i) < a1; i++)
                    {
                        bTemp1 = bTempToPowAndMult.MultOnDigit(i);
                        restemp = bTemp1;
                    }
                    if (a1 > restemp * b)
                    {
                        if (a1 - restemp * b < b)
                        {
                            res.Add(bTemp1);
                            break;
                        }
                    }
                    else
                        break;
                    a1 -= restemp * b;
                    res.Add(bTemp1);

                }
                var result = new BigInt();
                foreach (var e in res)
                    result += e;
                result.sign = a1.sign == b.sign ? '+' : '-';
                return result;
            }
        }
        public BigInt MultOnDigit(int digit) 
        {
            var signOfDig = digit;
            digit = Math.Abs(digit);
            var result = new BigInt('+', new List<int>());
            int temp1 = 0;
            int temp2 = 0;
            for (var i = this.number.Count - 1; i >= 0; i--)
            {
                temp2 = this.number[i] * digit + temp1;
                {
                    temp1 = temp2 / 10;
                    temp2 %= 10;
                }
                result.AddZeroesAndSetNumToIndex(i, temp2);
            }
            if (temp1 != 0) result.number.Insert(0, temp1);
            result.RemoveZeroes();
            if ((this.sign == '+' && signOfDig > 0) || (this.sign == '-' && signOfDig < 0))
                return result;
            else
                return -result;

        }
        public BigInt Pow(BigInt a)
        {
            this.RemoveZeroes();
            a.RemoveZeroes();
            var res = new BigInt(sign, new List<int>(number));
            var a1 = new BigInt(a.sign, new List<int>(a.number));
            if (a1 == new BigInt()) return new BigInt("1");
            for (; a1 > new BigInt("1"); a1 -= new BigInt("1"))
                res *= this;
            return res;
        }

        //overriding
        public static BigInt operator +(BigInt V1, BigInt V2)  // *this + V
        {
            if (V1.sign == '+' && V2.sign == '+')
                return BigPlusBig(V1, V2);
            else if (V1.sign == '+' && V2.sign == '-')
                return V1 - -V2;
            else if (V2.sign == '+' && V1.sign == '-')
                return V2 - -V1;
            else
            {
                var sum = BigPlusBig(V1, V2);
                sum.sign = '-';
                return sum;
            }
        }

        public static BigInt operator *(BigInt a, BigInt b)  // *this * V
        {
            var result = new BigInt();
            a.RemoveZeroes();
            b.RemoveZeroes();
            result.AddZeroesInBeginToMakeNumsEqual(a.number.Count + b.number.Count);
            a.number.Reverse();
            b.number.Reverse();
            for (var i = 0; i < a.number.Count; ++i)
                for (int j = 0, k = 0; j < b.number.Count || k > 0; ++j)
                {
                    var temp = result.number[i + j] + a.number[i] * (j < b.number.Count ? b.number[j] : 0) + k;
                    result.number[i + j] = temp % 10;
                    k = temp / 10;
                }
            while (result.number.Count > 1 && result.number.Last() == 0)
                result.number.RemoveAt(result.number.Count - 1);
            if (a.sign == b.sign)
                result.sign = '+';
            else
                result.sign = '-';
            a.number.Reverse();
            b.number.Reverse();
            result.number.Reverse();
            result.RemoveZeroes();
            return result;
        }
        public static BigInt operator -(BigInt a, BigInt b)// *this - V
        {
            if (a >= b)
            {
                return BigMinusBig(a, b);
            }
            else
            {
                return -BigMinusBig(b, a);
            }
        }
        public static BigInt operator -(BigInt V)// унарный минус 
        {
            if (V.sign == '+')
                return new BigInt('-', V.number);
            else
                return new BigInt('+', V.number);
        }
        public static bool operator ==(BigInt a, BigInt b)
        {
            return a.sign == b.sign && a.number.SequenceEqual(b.number);
        }
        public static bool operator !=(BigInt a, BigInt b)
        {
            return a.sign != b.sign || !a.number.SequenceEqual(b.number);
        }

        public static BigInt operator %(BigInt a, BigInt b) => Mod(a, b);
        public static BigInt operator /(BigInt a, BigInt b) => Div(a, b);
        public static bool operator <(BigInt a, BigInt b) => CompareWithSign(a, b) < 0;
        public static bool operator <=(BigInt a, BigInt b) => CompareWithSign(a, b) <= 0;

        public static bool operator >(BigInt a, BigInt b) => CompareWithSign(a, b) > 0;

        public static bool operator >=(BigInt a, BigInt b) => CompareWithSign(a, b) >= 0;

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(this.sign);
            foreach (var e in number)
                builder.Append(e.ToString());
            return builder.ToString();
        }
    }
}
