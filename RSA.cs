using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace LabWork1
{
    class RSA
    {     
        BigInt fi;
        BigInt n;
        BigInt d;
        List<string> E;        
        string P;
        BigInt Zero => new BigInt();
        BigInt One = new BigInt("1");
        
        char[] dictionary = new char[] { '1', '2', '3', '4', '5', '6', '7',
                                         '8', '9', '0', 'A', 'B', 'C', 'D', 
                                         'E', 'F', 'G', 'H', 'I', 'J', 'K', 
                                         'L', 'M', 'N', 'O', 'P', 'Q', 'R', 
                                         'S', 'T', 'U', 'V', 'W', 'X', 'Y',
                                         'Z', 'А', 'Б', 'В', 'Г', 'Д', 'Е',
                                         'Ё', 'Ж', 'З', 'И', 'Й', 'К', 'Л',
                                         'М', 'Н', 'О', 'П', 'Р', 'С', 'Т', 
                                         'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ',
                                         'Ь', 'Ы', 'Ъ', 'Э', 'Ю', 'Я', ' ' };
        string text;

        public RSA(BigInt p, BigInt q, string str)
        {
            text = str.ToUpper();
            if (IsSimple(p) && IsSimple(q))
            {
                n = p * q;
                fi = ((p - One) * (q - One));                
            }
            else
            {
                Console.WriteLine("p и/или q не являются простыми числами");
                return ;
            }
            var e = CountE(fi);
            d = CountD(e, fi);
            E = Encode(text, e, n);
            P = Decode(E, d, n);
        }

        public bool IsSimple(BigInt toCheck)
        {
            if (toCheck == new BigInt("2"))
                return true;
            if (toCheck < new BigInt("2"))
                return false;      

            for (var i = new BigInt("2"); i < toCheck; i += new BigInt("1"))
            {
                if (toCheck % i == new BigInt())
                    return false;
            }
            return true;
        }

        public string Decode(List<string> E, BigInt d, BigInt n)
        {
            var P = "";
            foreach (var i in E)
            {
                var temp = new BigInt(i);
                temp = temp.Pow(d);
                temp %= n;
                var index = Convert.ToInt32(temp.ToString());
                P += dictionary[index].ToString();
            }
            return P;
        }

        public List<string> Encode(string text, BigInt e, BigInt n)
        {
            var E = new List<string>();
            var temp = new BigInt();
            for (var i = 0; i < text.Length; i++)          
            {
                var index = Array.IndexOf(dictionary, text[i]);
                temp = new BigInt(index.ToString());
                temp = temp.Pow(e);
                temp %= n;
                E.Add(temp.ToString());
            }
            return E;
        }

        public BigInt CountE(BigInt fi)
        {
            var e = fi - One;
            for (var i = new BigInt("2"); i <= fi; i += One)
                if ((fi % i == Zero) && (e % i == Zero))
                {
                    e -= One;
                    i = One;
                }
            return e;
        }

        public BigInt CountD(BigInt e, BigInt fi)
        {
            return BigInt.ReverseModule(e, fi);
        }
        public string ReturnResultToLower()
        {
            return P.ToLower();
        }
        public void Test()
        {
            Console.WriteLine("testing started");            
            var a1 = new BigInt();
            var a2 = new BigInt("+26");
            var a3 = new BigInt("26");
            var a4 = new BigInt("-0");
            var a5 = new BigInt('-', new List<int>() { 2, 6, 0, 3, 2, 5 });
            var a6 = new BigInt('+', new List<int>() { 1, 9, 1, 9, 0, 4 });

            if (a1.sign != '+' || !Enumerable.SequenceEqual(a1.number, new List<int> { 0 }))
                Console.WriteLine("a1 is wrong");
            if (a2.sign != '+' || !Enumerable.SequenceEqual(a2.number, new List<int> { 2, 6 }))
                Console.WriteLine("a2 is wrong");
            if (a3.sign != '+' || !Enumerable.SequenceEqual(a3.number, new List<int> { 2, 6 }))
                Console.WriteLine("a3 is wrong");
            if (a4.sign != '-' || !Enumerable.SequenceEqual(a4.number, new List<int> { 0 }))
                Console.WriteLine("a4 is wrong");
            if (a5.sign != '-' || !Enumerable.SequenceEqual(a5.number, new List<int> { 2, 6, 0, 3, 2, 5 }))
                Console.WriteLine("a5 is wrong");
            if (a6.sign != '+' || !Enumerable.SequenceEqual(a6.number, new List<int> { 1, 9, 1, 9, 0, 4 }))
                Console.WriteLine("a6 is wrong");


            //+ - Test
            var b1 = new BigInt("+262626");
            var b2 = new BigInt("-191919");

            var t1 = b1 + b2;//454 545
            var t2 = b1 - b2;//70 707
            var t3 = b2 - b1;//-70 707
            var t4= b2 + b1;//454 545


            if (t1.sign != '+' || !Enumerable.SequenceEqual(t1.number, new List<int> { 7, 0, 7, 0, 7 }))
                Console.WriteLine("t1 is wrong");
            if (t2.sign != '+' || !Enumerable.SequenceEqual(t2.number, new List<int> { 4, 5, 4, 5, 4, 5 }))
                Console.WriteLine("t2 is wrong");
            if (t3.sign != '-' || !Enumerable.SequenceEqual(t3.number, new List<int> { 4, 5, 4, 5, 4, 5 }))
                Console.WriteLine("t3 is wrong");
            if (t4.sign != '+' || !Enumerable.SequenceEqual(t4.number, new List<int> { 7, 0, 7, 0, 7 }))
                Console.WriteLine("t4 is wrong");

           
            var g = new BigInt("+443322");
            var h = new BigInt("+4433");
            var i = new BigInt("-4433");

            //var t5 = g / h;
            //var t6 = g % h;
            //var t7 = g * h;

            //if (t5.sign != '+' || !Enumerable.SequenceEqual(t5.number, new List<int> { 1, 0,0 }))
            //    Console.WriteLine("* / % 1 failed");
            //if (t6.sign != '-' || !Enumerable.SequenceEqual(t6.number, new List<int> { 2,2 }))
            //    Console.WriteLine("* / % 2 failed");
            //if (t7.sign != '+' || !Enumerable.SequenceEqual(t7.number, new List<int> { 9, 8, 7, 6, 5, 4, 3, 2, 1, 0, 0, 0, 0, 0 }))
            //    Console.WriteLine("* / % 3 failed");
           

            
            if (!(g > h))
                Console.WriteLine("compaire failed 1");
            if (!(h < g))
                Console.WriteLine("compaire failed 2");
            if (!(g != h))
                Console.WriteLine("compaire failed 3");
            if (!(h != i))
                Console.WriteLine("compaire failed 4");
            if ((h <= i))
                Console.WriteLine("compaire failed 5");

            
            var m = new BigInt("5");
            var n = new BigInt("12");

            var t10 = BigInt.ReverseModule(m, n);

            if (t10 != new BigInt("7"))
                Console.WriteLine("Mod Inverse in not working");

          
            var o = new BigInt("13");
            var p = new BigInt("7");

            var t11 = new RSA(o, p, "the pleasere is mine");

            if (t11.ReturnResultToLower() != "the pleasere is mine")
                Console.WriteLine("RSA is not working");

            Console.WriteLine("testing is over");
            Console.WriteLine("_______________");
        }
    }
}
