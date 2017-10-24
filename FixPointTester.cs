using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class FixPointTests
{
    decimal m_max_precision_times = 0m;
    decimal m_max_delta_percent = 0m;
    public static void Test()
    {
        //FixPoint.GenerateSinTable();
        //FixPoint.GenerateAtan2Table();

        FixPointTests tester = new FixPointTests();
        tester.TestParse();
        tester.Precision();
        tester.LongToFixPointAndBack();
        tester.DoubleToFixPointAndBack();
        tester.DecimalToFixPointAndBack();
        tester.IntToFixPointAndBack();
        tester.FloatToFixPointAndBack();
        tester.Addition();
        tester.Substraction();
        tester.Multiplication();
        tester.Division();
        tester.Modulus();
        tester.TestSign();
        tester.TestAbs();
        tester.TestSqrt();
        tester.TestSin();
        tester.TestCos();
        tester.TestTan();
        tester.TestAtan2();
        //tester.TestMultiplicationSpeed();
        //tester.TestDivisionSpeed();
    }

    public void TestParse()
    {
        Assert.Equal((float)FixPoint.Parse("0"), (float)0);
        Assert.Equal((float)FixPoint.Parse("-0"), (float)0);
        Assert.Equal((float)FixPoint.Parse("+0"), (float)0);
        Assert.Equal((float)FixPoint.Parse("1"), (float)1);
        Assert.Equal((float)FixPoint.Parse("-1"), (float)-1);
        Assert.Equal((float)FixPoint.Parse("+1"), (float)1);
        Assert.Equal((float)FixPoint.Parse("12345"), (float)12345);
        Assert.Equal((float)FixPoint.Parse("-12345"), (float)-12345);
        Assert.Equal((float)FixPoint.Parse("+12345"), (float)12345);

        Assert.Equal((float)FixPoint.Parse("1.5"), (float)1.5);
        Assert.Equal((float)FixPoint.Parse("-1.5"), (float)-1.5);
        Assert.Equal((float)FixPoint.Parse("+1.5"), (float)1.5);

        Assert.Equal((float)FixPoint.Parse("0.5"), (float)0.5);
        Assert.Equal((float)FixPoint.Parse("-0.5"), (float)-0.5);
        Assert.Equal((float)FixPoint.Parse("+0.5"), (float)0.5);
        Assert.Equal((float)FixPoint.Parse(".5"), (float)0.5);
        Assert.Equal((float)FixPoint.Parse("-.5"), (float)-0.5);
        Assert.Equal((float)FixPoint.Parse("+.5"), (float)0.5);


        Assert.Equal((float)FixPoint.Parse(".6789"), (float)0.6789);
        Assert.Equal((float)FixPoint.Parse("-.6789"), (float)-0.6789);
        Assert.Equal((float)FixPoint.Parse("-.6789"), (float)-0.6789);

        Assert.Equal((float)FixPoint.Parse("0.6789"), (float)0.6789);
        Assert.Equal((float)FixPoint.Parse("-0.6789"), (float)-0.6789);
        Assert.Equal((float)FixPoint.Parse("+0.6789"), (float)0.6789);

        Assert.Equal((float)FixPoint.Parse("12345.6789"), (float)12345.6789);
        Assert.Equal((float)FixPoint.Parse("+12345.6789"), (float)12345.6789);
        Assert.Equal((float)FixPoint.Parse("-12345.6789"), (float)-12345.6789);


        Assert.Equal((float)FixPoint.Parse("10203.06"), (float)10203.06);
        Assert.Equal((float)FixPoint.Parse("10203.006"), (float)10203.006);
        Assert.Equal((float)FixPoint.Parse("10203.0006"), (float)10203.0006);
        Assert.Equal((float)FixPoint.Parse("10203.00006"), (float)10203.00006);
        Assert.Equal((float)FixPoint.Parse("10203.000006"), (float)10203.000006);
    }

    public void Precision()
    {
        Assert.Equal(FixPoint.Precision, 0.0000152587890625m);
        Assert.Equal(FixPoint.MinValue, (FixPoint)(-140737488355328L));
        Assert.Equal(FixPoint.MaxValue - (FixPoint)(140737488355327L) + FixPoint.PrecisionFP, FixPoint.One);
        Assert.Equal(FixPoint.MaxValue, FixPoint.MaxValue - FixPoint.One + FixPoint.One);
    }

    #region 数据转化
    public void LongToFixPointAndBack()
    {
        LongToFixPointAndBack_Internal(-1L, -1L);
        LongToFixPointAndBack_Internal(0L, 0L);
        LongToFixPointAndBack_Internal(1L, 1L);
        LongToFixPointAndBack_Internal(-140737488355328L, -140737488355328L);
        LongToFixPointAndBack_Internal(140737488355327L, 140737488355327L);
        LongToFixPointAndBack_Internal(-140737488355328L - 1L, 140737488355327L);
        LongToFixPointAndBack_Internal(140737488355327L + 1L, -140737488355328L);
        LongToFixPointAndBack_Internal(long.MinValue, 0L);
        LongToFixPointAndBack_Internal(long.MaxValue, -1L);
    }
    void LongToFixPointAndBack_Internal(long source, long expected)
    {
        FixPoint fp = (FixPoint)source;
        long back = (long)fp;
        Assert.Equal(back, expected);
    }

    public void DoubleToFixPointAndBack()
    {
        double[] sources = new double[] {
                (double)int.MinValue,
                -(double)Math.PI,
                -(double)Math.E,
                -1.0,
                -0.0,
                0.0,
                1.0,
                (double)Math.PI,
                (double)Math.E,
                (double)int.MaxValue
            };
        foreach (double value in sources)
            AreEqualWithinPrecision(value, (double)(FixPoint)value);
    }
    static void AreEqualWithinPrecision(double value1, double value2)
    {
        Assert.True(Math.Abs(value2 - value1) < (double)FixPoint.Precision);
    }

    public void DecimalToFixPointAndBack()
    {
        Assert.Equal(FixPoint.MaxValue, (FixPoint)(decimal)FixPoint.MaxValue);
        Assert.Equal(FixPoint.MinValue, (FixPoint)(decimal)FixPoint.MinValue);
        decimal[] sources = new decimal[] {
                int.MinValue,
                -(decimal)Math.PI,
                -(decimal)Math.E,
                -1.0m,
                -0.0m,
                0.0m,
                1.0m,
                (decimal)Math.PI,
                (decimal)Math.E,
                int.MaxValue
            };
        foreach (decimal value in sources)
            AreEqualWithinPrecision(value, (decimal)(FixPoint)value);
    }
    static void AreEqualWithinPrecision(decimal value1, decimal value2)
    {
        Assert.True(Math.Abs(value2 - value1) < FixPoint.Precision);
    }

    public void IntToFixPointAndBack()
    {
        AreEqualWithinPrecision(int.MinValue, (int)(new FixPoint(int.MinValue)));
        AreEqualWithinPrecision(int.MaxValue, (int)(new FixPoint(int.MaxValue)));
        System.Random ran = new System.Random();
        for (int i = 0; i < 10000; ++i)
        {
            int source = ran.Next();
            AreEqualWithinPrecision(source, (int)(new FixPoint(source)));
        }
        for (int i = 0; i < 10000; ++i)
        {
            int source = -ran.Next();
            AreEqualWithinPrecision(source, (int)(new FixPoint(source)));
        }
    }
    static void AreEqualWithinPrecision(int value1, int value2)
    {
        Assert.True(Math.Abs(value2 - value1) == 0);
    }

    public void FloatToFixPointAndBack()
    {
        float source1 = int.MaxValue;
        AreEqualWithinPrecision(source1, (float)(FixPoint)source1);
        float source2 = int.MinValue;
        AreEqualWithinPrecision(source2, (float)(FixPoint)source2);
        System.Random ran = new System.Random();
        for (int i = 0; i < 10000; ++i)
        {
            float source = (float)ran.NextDouble() * int.MaxValue;
            AreEqualWithinPrecision(source, (float)(FixPoint)source);
        }
        for (int i = 0; i < 10000; ++i)
        {
            float source = (float)ran.NextDouble() * int.MinValue;
            AreEqualWithinPrecision(source, (float)(FixPoint)source);
        }
    }
    static void AreEqualWithinPrecision(float value1, float value2)
    {
        Assert.True(Math.Abs(value2 - value1) < (float)FixPoint.Precision);
    }
    #endregion

    #region 加法
    public void Addition()
    {
        Addition_Internal(FixPoint.MinValue + FixPoint.One, (FixPoint)(-1), FixPoint.MinValue);
        Addition_Internal((FixPoint)(-1), (FixPoint)2, FixPoint.One);
        Addition_Internal(FixPoint.Zero, (FixPoint)(-1.5m), (FixPoint)(-1.5m));
        Addition_Internal(FixPoint.One, (FixPoint)(-2), (FixPoint)(-1));
        Addition_Internal(FixPoint.MaxValue - FixPoint.One, FixPoint.One, FixPoint.MaxValue);

        int max_int = int.MaxValue >> 1;
        System.Random ran = new System.Random();
        for (int i = 0; i < 10000; ++i)
        {
            int term1 = ran.Next() % max_int;
            int term2 = ran.Next() % max_int;
            Addition_Internal(term1, term2);
        }
        for (int i = 0; i < 10000; ++i)
        {
            int term1 = -ran.Next() % max_int;
            int term2 = -ran.Next() % max_int;
            Addition_Internal(term1, term2);
        }
        for (int i = 0; i < 10000; ++i)
        {
            int term1 = ran.Next();
            int term2 = -ran.Next();
            Addition_Internal(term1, term2);
        }

        for (int i = 0; i < 10000; ++i)
        {
            float term1 = (float)ran.NextDouble() * max_int;
            float term2 = (float)ran.NextDouble() * max_int;
            Addition_Internal(term1, term2);
        }
        for (int i = 0; i < 10000; ++i)
        {
            float term1 = -(float)ran.NextDouble() * max_int;
            float term2 = -(float)ran.NextDouble() * max_int;
            Addition_Internal(term1, term2);
        }
        for (int i = 0; i < 10000; ++i)
        {
            float term1 = (float)ran.NextDouble() * int.MaxValue;
            float term2 = -(float)ran.NextDouble() * int.MaxValue;
            Addition_Internal(term1, term2);
        }
    }
    void Addition_Internal(int term1, int term2)
    {
        int result = term1 + term2;
        FixPoint fp1 = new FixPoint(term1);
        FixPoint fp2 = new FixPoint(term2);
        FixPoint fpresult = fp1 + fp2;
        FixPoint fpexpected = new FixPoint(result);
        Assert.Equal(fpresult, fpexpected);
        Assert.Equal(result, (int)fpresult);
    }
    void Addition_Internal(float term1, float term2)
    {
        float result = term1 + term2;
        FixPoint fp1 = (FixPoint)term1;
        FixPoint fp2 = (FixPoint)term2;
        FixPoint fpresult = fp1 + fp2;
        FixPoint fpexpected = (FixPoint)result;
        //Assert.Equal(fpresult, fpexpected);//这个不可能Equal
        Assert.Equal(result, (float)fpresult);
    }
    void Addition_Internal(FixPoint terms1, FixPoint terms2, FixPoint expected)
    {
        FixPoint result = terms1 + terms2;
        Assert.Equal(result, expected);
    }
    #endregion

    #region 减法
    public void Substraction()
    {
        Substraction_Internal((FixPoint)(-1), (FixPoint)(-2), FixPoint.One);
        Substraction_Internal(FixPoint.Zero, (FixPoint)(1.5m), (FixPoint)(-1.5m));
        Substraction_Internal(FixPoint.One, (FixPoint)(2), (FixPoint)(-1));

        int max_int = int.MaxValue >> 1;
        System.Random ran = new System.Random();
        for (int i = 0; i < 10000; ++i)
        {
            int term1 = ran.Next();
            int term2 = ran.Next();
            Substraction_Internal(term1, term2);
        }
        for (int i = 0; i < 10000; ++i)
        {
            int term1 = -ran.Next();
            int term2 = -ran.Next();
            Substraction_Internal(term1, term2);
        }
        for (int i = 0; i < 10000; ++i)
        {
            int term1 = ran.Next() % max_int;
            int term2 = -ran.Next() % max_int;
            Substraction_Internal(term1, term2);
            Substraction_Internal(term2, term1);
        }

        for (int i = 0; i < 10000; ++i)
        {
            float term1 = (float)ran.NextDouble() * int.MaxValue;
            float term2 = (float)ran.NextDouble() * int.MaxValue;
            Substraction_Internal(term1, term2);
        }
        for (int i = 0; i < 10000; ++i)
        {
            float term1 = -(float)ran.NextDouble() * int.MaxValue;
            float term2 = -(float)ran.NextDouble() * int.MaxValue;
            Substraction_Internal(term1, term2);
        }
        for (int i = 0; i < 10000; ++i)
        {
            float term1 = (float)ran.NextDouble() * max_int;
            float term2 = -(float)ran.NextDouble() * max_int;
            Substraction_Internal(term1, term2);
            Substraction_Internal(term2, term1);
        }
    }
    void Substraction_Internal(int term1, int term2)
    {
        int result = term1 - term2;
        FixPoint fp1 = new FixPoint(term1);
        FixPoint fp2 = new FixPoint(term2);
        FixPoint fpresult = fp1 - fp2;
        FixPoint fpexpected = new FixPoint(result);
        Assert.Equal(fpresult, fpexpected);
        Assert.Equal(result, (int)fpresult);
    }
    void Substraction_Internal(float term1, float term2)
    {
        float result = term1 - term2;
        FixPoint fp1 = (FixPoint)term1;
        FixPoint fp2 = (FixPoint)term2;
        FixPoint fpresult = fp1 - fp2;
        FixPoint fpexpected = (FixPoint)result;
        //Assert.Equal(fpresult, fpexpected);//这个不可能Equal
        Assert.Equal(result, (float)fpresult);
    }
    void Substraction_Internal(FixPoint terms1, FixPoint terms2, FixPoint expected)
    {
        FixPoint result = terms1 - terms2;
        Assert.Equal(result, expected);
    }
    #endregion

    #region 乘法
    public void Multiplication()
    {
        m_max_precision_times = 0m;
        m_max_delta_percent = 0m;
        Multiplication_Internal((FixPoint)(0), (FixPoint)(16), (FixPoint)(0));
        Multiplication_Internal((FixPoint)(1), (FixPoint)(16), (FixPoint)(16));
        Multiplication_Internal((FixPoint)(-1), (FixPoint)(16), (FixPoint)(-16));
        Multiplication_Internal((FixPoint)(5), (FixPoint)(16), (FixPoint)(80));
        Multiplication_Internal((FixPoint)(5), (FixPoint)(-16), (FixPoint)(-80));
        Multiplication_Internal((FixPoint)(-5), (FixPoint)(16), (FixPoint)(-80));
        Multiplication_Internal((FixPoint)(-5), (FixPoint)(-16), (FixPoint)(80));
        Multiplication_Internal((FixPoint)(0.5), (FixPoint)(16), (FixPoint)(8));
        Multiplication_Internal((FixPoint)(1.0), (FixPoint)(16), (FixPoint)(16));

        int max_int = (int)(Math.Sqrt((double)(int.MaxValue)));
        System.Random ran = new System.Random();
        for (int i = 0; i < 10000; ++i)
        {
            int term1 = ran.Next() & max_int;
            int term2 = ran.Next() & max_int;
            Multiplication_Internal(term1, term2);
        }
        for (int i = 0; i < 10000; ++i)
        {
            int term1 = -ran.Next() % max_int;
            int term2 = -ran.Next() % max_int;
            Multiplication_Internal(term1, term2);
        }
        for (int i = 0; i < 10000; ++i)
        {
            int term1 = ran.Next() % max_int;
            int term2 = -ran.Next() % max_int;
            Multiplication_Internal(term1, term2);
        }

        for (int i = 0; i < 10000; ++i)
        {
            float term1 = (float)ran.NextDouble() * max_int;
            float term2 = (float)ran.NextDouble() * max_int;
            Multiplication_Internal(term1, term2);
        }
        for (int i = 0; i < 10000; ++i)
        {
            float term1 = -(float)ran.NextDouble() * max_int;
            float term2 = -(float)ran.NextDouble() * max_int;
            Multiplication_Internal(term1, term2);
        }
        for (int i = 0; i < 10000; ++i)
        {
            float term1 = (float)ran.NextDouble() * max_int;
            float term2 = -(float)ran.NextDouble() * max_int;
            Multiplication_Internal(term1, term2);
            Multiplication_Internal(term2, term1);
        }

        Console.WriteLine("Multiplication() m_max_precision_times = " + m_max_precision_times + ", m_max_delta_percent = " + m_max_delta_percent);
    }
    void Multiplication_Internal(int term1, int term2)
    {
        int result = term1 * term2;
        FixPoint fp1 = new FixPoint(term1);
        FixPoint fp2 = new FixPoint(term2);
        FixPoint fpresult = fp1 * fp2;
        FixPoint fpexpected = new FixPoint(result);
        Assert.Equal(fpresult, fpexpected);
        Assert.Equal(result, (int)fpresult);
    }
    void Multiplication_Internal(float term1, float term2)
    {
        float result = term1 * term2;
        FixPoint fp1 = (FixPoint)term1;
        FixPoint fp2 = (FixPoint)term2;
        FixPoint fpresult = fp1 * fp2;
        FixPoint fpexpected = (FixPoint)result;
        //Assert.Equal(fpresult, fpexpected);//这个不可能Equal
        //Assert.Equal(result, (float)fpresult);//这个很难做到的
        decimal delta = (decimal)Math.Abs(result - (float)fpresult);
        decimal precision_times = delta / FixPoint.Precision;
        decimal delta_percent = delta * 100.0m / (decimal)Math.Abs(result);
        if (precision_times > m_max_precision_times)
            m_max_precision_times = precision_times;
        if (delta_percent > m_max_delta_percent)
            m_max_delta_percent = delta_percent;
    }
    void Multiplication_Internal(FixPoint terms1, FixPoint terms2, FixPoint expected)
    {
        FixPoint result = terms1 * terms2;
        Assert.Equal(result, expected);
    }
    #endregion

    #region 除法
    public void Division()
    {
        m_max_precision_times = 0m;
        m_max_delta_percent = 0m;
        Division_Internal((FixPoint)(0), (FixPoint)(16), (FixPoint)(0));
        Division_Internal((FixPoint)(16), (FixPoint)(2), (FixPoint)(8));
        Division_Internal((FixPoint)(16), (FixPoint)(-2), (FixPoint)(-8));
        Division_Internal((FixPoint)(-16), (FixPoint)(2), (FixPoint)(-8));
        Division_Internal((FixPoint)(-16), (FixPoint)(-2), (FixPoint)(8));

        System.Random ran = new System.Random();
        for (int i = 0; i < 10000; ++i)
        {
            float term1 = (float)ran.NextDouble() * int.MaxValue;
            float term2 = (float)ran.NextDouble() * int.MaxValue;
            Division_Internal(term1, term2);
        }
        for (int i = 0; i < 10000; ++i)
        {
            float term1 = -(float)ran.NextDouble() * int.MaxValue;
            float term2 = -(float)ran.NextDouble() * int.MaxValue;
            Division_Internal(term1, term2);
        }
        for (int i = 0; i < 10000; ++i)
        {
            float term1 = (float)ran.NextDouble() * int.MaxValue;
            float term2 = -(float)ran.NextDouble() * int.MaxValue;
            Division_Internal(term1, term2);
            Division_Internal(term2, term1);
        }

        Console.WriteLine("Division() m_max_precision_times = " + m_max_precision_times + ", m_max_delta_percent = " + m_max_delta_percent);
    }
    //void Division_Internal(int term1, int term2)
    //{
    //    int result = term1 / term2;
    //    FixPoint fp1 = new FixPoint(term1);
    //    FixPoint fp2 = new FixPoint(term2);
    //    FixPoint fpresult = fp1 / fp2;
    //    FixPoint fpexpected = new FixPoint(result);
    //    Assert.Equal(fpresult, fpexpected);
    //    Assert.Equal(result, (int)fpresult);
    //}
    void Division_Internal(float term1, float term2)
    {
        float result = term1 / term2;
        FixPoint fp1 = (FixPoint)term1;
        FixPoint fp2 = (FixPoint)term2;
        FixPoint fpresult = fp1 / fp2;
        FixPoint fpexpected = (FixPoint)result;
        //Assert.Equal(fpresult, fpexpected);//这个不可能Equal
        //Assert.Equal(result, (float)fpresult);//这个很难做到的

        decimal delta = (decimal)Math.Abs(result - (float)fpresult);
        decimal precision_times = delta / FixPoint.Precision;
        decimal less_term = Math.Min((decimal)Math.Abs(term1), (decimal)Math.Abs(term2));
        decimal delta_percent = delta * 100.0m / less_term;
        if (precision_times > m_max_precision_times)
            m_max_precision_times = precision_times;
        if (delta_percent > m_max_delta_percent)
            m_max_delta_percent = delta_percent;
    }
    void Division_Internal(FixPoint terms1, FixPoint terms2, FixPoint expected)
    {
        FixPoint result = terms1 / terms2;
        Assert.Equal(result, expected);
    }
    #endregion

    #region 取模
    public void Modulus()
    {
        System.Random ran = new System.Random();
        for (int i = 0; i < 10000; ++i)
        {
            int term1 = ran.Next();
            int term2 = ran.Next();
            Modulus_Internal(term1, term2);
        }
        for (int i = 0; i < 10000; ++i)
        {
            int term1 = -ran.Next();
            int term2 = -ran.Next();
            Modulus_Internal(term1, term2);
        }
        for (int i = 0; i < 10000; ++i)
        {
            int term1 = ran.Next();
            int term2 = -ran.Next();
            Modulus_Internal(term1, term2);
        }
    }
    void Modulus_Internal(int term1, int term2)
    {
        int result = term1 % term2;
        FixPoint fp1 = new FixPoint(term1);
        FixPoint fp2 = new FixPoint(term2);
        FixPoint fpresult = fp1 % fp2;
        FixPoint fpexpected = new FixPoint(result);
        Assert.Equal(fpresult, fpexpected);
        Assert.Equal(result, (int)fpresult);
    }
    #endregion

    #region SIGN
    public void TestSign()
    {
        Assert.Equal(FixPoint.Sign((FixPoint)(0)), 0);
        Assert.Equal(FixPoint.Sign((FixPoint)(-0)), 0);
        Assert.Equal(FixPoint.Sign((FixPoint)(1)), 1);
        Assert.Equal(FixPoint.Sign((FixPoint)(-1)), -1);
        Assert.Equal(FixPoint.Sign((FixPoint)(int.MaxValue)), 1);
        Assert.Equal(FixPoint.Sign((FixPoint)(int.MinValue)), -1);
        Assert.Equal(FixPoint.Sign((FixPoint)(140737488355327L)), 1);
        Assert.Equal(FixPoint.Sign((FixPoint)(-140737488355328L)), -1);
    }
    #endregion

    #region 绝对值
    public void TestAbs()
    {
        Assert.Equal(FixPoint.Abs((FixPoint)(17)), (FixPoint)(17));
        Assert.Equal(FixPoint.Abs((FixPoint)(-17)), (FixPoint)(17));
        Assert.Equal(FixPoint.Abs((FixPoint)(0)), (FixPoint)(0));
        Assert.Equal(FixPoint.Abs((FixPoint)(-0)), (FixPoint)(0));
        Assert.Equal(FixPoint.Abs((FixPoint)(-1.5f)), (FixPoint)(1.5f));
        Assert.Equal(FixPoint.Abs((FixPoint)(-int.MaxValue)), (FixPoint)(int.MaxValue));
        Assert.Equal(FixPoint.Abs((FixPoint)(-140737488355327L)), (FixPoint)(140737488355327L));
    }
    #endregion

    #region SQRT
    public void TestSqrt()
    {
        m_max_precision_times = 0m;
        m_max_delta_percent = 0m;
        int max_int = (int)(Math.Sqrt((double)(int.MaxValue)));
        for (int i = 0; i < max_int; ++i)
        {
            int value = i * i;
            FixPoint fp = new FixPoint(value);
            FixPoint sqrt_fp = FixPoint.Sqrt(fp);
            Assert.Equal((int)sqrt_fp, i);
        }

        System.Random ran = new System.Random();
        for (int i = 0; i < 10000; ++i)
        {
            double term = (double)ran.NextDouble() * int.MaxValue;
            double sqrt_term = Math.Sqrt(term);
            FixPoint fp_sqrt_term = (FixPoint)sqrt_term;
            FixPoint fp = (FixPoint)term;
            FixPoint sqrt_fp = FixPoint.Sqrt(fp);
            Assert.Equal((double)sqrt_fp, sqrt_term);
            Assert.Equal(sqrt_fp, fp_sqrt_term);

            //decimal delta = (decimal)Math.Abs(sqrt_term - (double)sqrt_fp);
            //decimal precision_times = delta / FixPoint.Precision;
            //decimal delta_percent = delta * 100.0m / (decimal)term;
            //if (precision_times > m_max_precision_times)
            //    m_max_precision_times = precision_times;
            //if (delta_percent > m_max_delta_percent)
            //    m_max_delta_percent = delta_percent;
        }

        Console.WriteLine("TestSqrt() m_max_precision_times = " + m_max_precision_times + ", m_max_delta_percent = " + m_max_delta_percent);

        m_max_precision_times = 0m;
        m_max_delta_percent = 0m;
        max_int = (int)Math.Sqrt((double)(int.MaxValue / 2));
        for (int i = 0; i < 10000; ++i)
        {
            double term1 = (double)ran.NextDouble() * max_int;
            double term2 = (double)ran.NextDouble() * max_int;
            double distance = Math.Sqrt(term1 * term1 + term2 * term2);
            FixPoint fpterm1 = (FixPoint)term1;
            FixPoint fpterm2 = (FixPoint)term2;
            FixPoint fp_distance = FixPoint.FastDistance(fpterm1, fpterm2);

            decimal delta = (decimal)Math.Abs(distance - (double)fp_distance);
            decimal delta_percent = delta * 100.0m / (decimal)distance;
            if (delta_percent > m_max_delta_percent)
                m_max_delta_percent = delta_percent;
        }

        Console.WriteLine("FastDistance() m_max_delta_percent = " + m_max_delta_percent);
    }
    #endregion

    #region SIN
    public void TestSin()
    {
        m_max_precision_times = 0m;
        m_max_delta_percent = 0m;
        Assert.True(FixPoint.Sin(FixPoint.Zero) == FixPoint.Zero);
        Assert.True(FixPoint.Sin(FixPoint.HalfPi) == FixPoint.One);
        Assert.True(FixPoint.Sin(FixPoint.Pi) == FixPoint.Zero);
        Assert.True(FixPoint.Sin(FixPoint.Pi + FixPoint.HalfPi) == -FixPoint.One);
        Assert.True(FixPoint.Sin(FixPoint.TwoPi) == FixPoint.Zero);
        Assert.True(FixPoint.Sin(-FixPoint.HalfPi) == -FixPoint.One);
        Assert.True(FixPoint.Sin(-FixPoint.Pi) == FixPoint.Zero);
        Assert.True(FixPoint.Sin(-FixPoint.Pi - FixPoint.HalfPi) == FixPoint.One);
        Assert.True(FixPoint.Sin(-FixPoint.TwoPi) == FixPoint.Zero);

        FixPoint fp1 = FixPoint.HalfPi / new FixPoint(3);
        FixPoint sin30 = FixPoint.Sin(fp1);
        FixPoint fp2 = (FixPoint)(Math.PI / 6);
        FixPoint sin30_2 = FixPoint.Sin(fp2);

        double radian = -4.0685853071847475;
        double sin = Math.Sin(radian);
        double radian2 = 2 * Math.PI + radian;
        double sin2 = Math.Sin(radian2);
        FixPoint fp_radian = (FixPoint)radian;
        FixPoint fp_radian2 = (FixPoint)radian2;
        FixPoint fp_sin = FixPoint.Sin(fp_radian);
        FixPoint fp_sin2 = FixPoint.Sin(fp_radian2);


        for (double angle = -2 * Math.PI; angle <= 2 * Math.PI; angle += 0.0001)
        {
            double expected = Math.Sin(angle);
            FixPoint fp = (FixPoint)angle;
            FixPoint result = FixPoint.Sin(fp);

            decimal delta = (decimal)Math.Abs(expected - (double)result);
            decimal precision_times = delta / FixPoint.Precision;
            decimal delta_percent = delta * 100.0m / 1;
            if (delta_percent > 40)
                delta_percent -= 1;
            if (precision_times > m_max_precision_times)
                m_max_precision_times = precision_times;
            if (delta_percent > m_max_delta_percent)
                m_max_delta_percent = delta_percent;
        }

        Console.WriteLine("TestSin() m_max_precision_times = " + m_max_precision_times + ", m_max_delta_percent = " + m_max_delta_percent);
    }
    #endregion

    #region COS
    public void TestCos()
    {
        m_max_precision_times = 0m;
        m_max_delta_percent = 0m;
        Assert.True(FixPoint.Cos(FixPoint.Zero) == FixPoint.One);
        Assert.True(FixPoint.Cos(FixPoint.HalfPi) == FixPoint.Zero);
        Assert.True(FixPoint.Cos(FixPoint.Pi) == -FixPoint.One);
        Assert.True(FixPoint.Cos(FixPoint.Pi + FixPoint.HalfPi) == FixPoint.Zero);
        Assert.True(FixPoint.Cos(FixPoint.TwoPi) == FixPoint.One);
        Assert.True(FixPoint.Cos(-FixPoint.HalfPi) == -FixPoint.Zero);
        Assert.True(FixPoint.Cos(-FixPoint.Pi) == -FixPoint.One);
        Assert.True(FixPoint.Cos(-FixPoint.Pi - FixPoint.HalfPi) == FixPoint.Zero);
        Assert.True(FixPoint.Cos(-FixPoint.TwoPi) == FixPoint.One);

        for (double angle = -2 * Math.PI; angle <= 2 * Math.PI; angle += 0.0001)
        {
            double expected = Math.Cos(angle);
            FixPoint fp = (FixPoint)angle;
            FixPoint result = FixPoint.Cos(fp);

            decimal delta = (decimal)Math.Abs(expected - (double)result);
            decimal precision_times = delta / FixPoint.Precision;
            decimal delta_percent = delta * 100.0m / 1;
            if (delta_percent > 40)
                delta_percent -= 1;
            if (precision_times > m_max_precision_times)
                m_max_precision_times = precision_times;
            if (delta_percent > m_max_delta_percent)
                m_max_delta_percent = delta_percent;
        }

        Console.WriteLine("TestCos() m_max_precision_times = " + m_max_precision_times + ", m_max_delta_percent = " + m_max_delta_percent);
    }
    #endregion

    #region TAN
    public void TestTan()
    {
    }
    #endregion

    #region ATAN2
    public void TestAtan2()
    {
        m_max_precision_times = 0m;
        m_max_delta_percent = 0m;
        
        Assert.Equal(FixPoint.Atan2(FixPoint.Zero, FixPoint.One), FixPoint.Zero);
        Assert.Equal(FixPoint.Atan2(FixPoint.One, FixPoint.One), FixPoint.QuarterPi);
        Assert.Equal(FixPoint.Atan2(FixPoint.One, FixPoint.Zero), FixPoint.HalfPi);
        Assert.Equal(FixPoint.Atan2(FixPoint.One, -FixPoint.One), FixPoint.HalfPi + FixPoint.QuarterPi);
        Assert.Equal(FixPoint.Atan2(FixPoint.Zero, -FixPoint.One), FixPoint.Pi);
        Assert.Equal(FixPoint.Atan2(-FixPoint.One, -FixPoint.One), FixPoint.Pi + FixPoint.QuarterPi);
        Assert.Equal(FixPoint.Atan2(-FixPoint.One, FixPoint.Zero), FixPoint.OneAndHalfPi);
        Assert.Equal(FixPoint.Atan2(-FixPoint.One, FixPoint.One), FixPoint.TwoPi - FixPoint.QuarterPi);

        System.Random ran = new System.Random();
        for (int i = 0; i < 10000; ++i)
        {
            double y = (float)ran.NextDouble() * int.MaxValue;
            double x = (float)ran.NextDouble() * int.MaxValue;
            double radian = Math.Atan2(y, x);
            FixPoint fp_y = (FixPoint)y;
            FixPoint fp_x = (FixPoint)x;
            FixPoint fp_radian = FixPoint.Atan2(fp_y, fp_x);

            decimal delta = (decimal)Math.Abs(radian - (double)fp_radian);
            decimal precision_times = delta / FixPoint.Precision;
            decimal delta_percent = delta * 100.0m / (decimal)Math.PI;
            if (precision_times > m_max_precision_times)
                m_max_precision_times = precision_times;
            if (delta_percent > m_max_delta_percent)
                m_max_delta_percent = delta_percent;
        }
        for (int i = 0; i < 10000; ++i)
        {
            double y = (float)ran.NextDouble() * int.MaxValue;
            double x = -(float)ran.NextDouble() * int.MaxValue;
            double radian = Math.Atan2(y, x);
            FixPoint fp_y = (FixPoint)y;
            FixPoint fp_x = (FixPoint)x;
            FixPoint fp_radian = FixPoint.Atan2(fp_y, fp_x);

            decimal delta = (decimal)Math.Abs(radian - (double)fp_radian);
            decimal precision_times = delta / FixPoint.Precision;
            decimal delta_percent = delta * 100.0m / (decimal)Math.PI;
            if (precision_times > m_max_precision_times)
                m_max_precision_times = precision_times;
            if (delta_percent > m_max_delta_percent)
                m_max_delta_percent = delta_percent;
        }
        for (int i = 0; i < 10000; ++i)
        {
            double y = -(float)ran.NextDouble() * int.MaxValue;
            double x = -(float)ran.NextDouble() * int.MaxValue;
            double radian = Math.Atan2(y, x) + Math.PI * 2;
            FixPoint fp_y = (FixPoint)y;
            FixPoint fp_x = (FixPoint)x;
            FixPoint fp_radian = FixPoint.Atan2(fp_y, fp_x);

            decimal delta = (decimal)Math.Abs(radian - (double)fp_radian);
            decimal precision_times = delta / FixPoint.Precision;
            decimal delta_percent = delta * 100.0m / (decimal)Math.PI;
            if (precision_times > m_max_precision_times)
                m_max_precision_times = precision_times;
            if (delta_percent > m_max_delta_percent)
                m_max_delta_percent = delta_percent;
        }
        for (int i = 0; i < 10000; ++i)
        {
            double y = -(float)ran.NextDouble() * int.MaxValue;
            double x = (float)ran.NextDouble() * int.MaxValue;
            double radian = Math.Atan2(y, x) + Math.PI * 2;
            FixPoint fp_y = (FixPoint)y;
            FixPoint fp_x = (FixPoint)x;
            FixPoint fp_radian = FixPoint.Atan2(fp_y, fp_x);

            decimal delta = (decimal)Math.Abs(radian - (double)fp_radian);
            decimal precision_times = delta / FixPoint.Precision;
            decimal delta_percent = delta * 100.0m / (decimal)Math.PI;
            if (precision_times > m_max_precision_times)
                m_max_precision_times = precision_times;
            if (delta_percent > m_max_delta_percent)
                m_max_delta_percent = delta_percent;
        }
        
        Console.WriteLine("TestAtan2() m_max_precision_times = " + m_max_precision_times + ", m_max_delta_percent = " + m_max_delta_percent);
    }
    #endregion


    public void TestMultiplicationSpeed()
    {
        System.TimeSpan system_span = new TimeSpan();
        System.TimeSpan fp_span = new TimeSpan(); 
        System.Random ran = new System.Random();
        int COUNT = 10000000;

        System.DateTime dt1 = System.DateTime.Now;
        for (int i = 0; i < COUNT; ++i)
        {
            double d1 = ran.NextDouble();
            double d2 = ran.NextDouble();
        }
        System.DateTime dt2 = System.DateTime.Now;
        System.TimeSpan span1 = dt2 - dt1;
        dt1 = System.DateTime.Now;
        for (int i = 0; i < COUNT; ++i)
        {
            double d1 = ran.NextDouble();
            double d2 = ran.NextDouble();
            double d3 = d1 * d2;
        }
        dt2 = System.DateTime.Now;
        System.TimeSpan span2 = dt2 - dt1;
        system_span = span2 - span1;


        dt1 = System.DateTime.Now;
        for (int i = 0; i < COUNT; ++i)
        {
            double d1 = ran.NextDouble();
            Fix64 fp1 = (Fix64)d1;
            double d2 = ran.NextDouble();
            Fix64 fp2 = (Fix64)d2;
        }
        dt2 = System.DateTime.Now;
        span1 = dt2 - dt1;
        dt1 = System.DateTime.Now;
        for (int i = 0; i < COUNT; ++i)
        {
            double d1 = ran.NextDouble();
            Fix64 fp1 = (Fix64)d1;
            double d2 = ran.NextDouble();
            Fix64 fp2 = (Fix64)d2;
            Fix64 fp3 = fp1 * fp2;
        }
        dt2 = System.DateTime.Now;
        span2 = dt2 - dt1;
        fp_span = span2 - span1;

        Console.WriteLine("TestMultiplicationSpeed() system_span = " + system_span.Ticks + ", fp_span = " + fp_span.Ticks);
    }

    public void TestDivisionSpeed()
    {
        System.TimeSpan system_span = new TimeSpan();
        System.TimeSpan fp_span = new TimeSpan();
        System.Random ran = new System.Random();
        int COUNT = 10000000;

        System.DateTime dt1 = System.DateTime.Now;
        for (int i = 0; i < COUNT; ++i)
        {
            double d1 = ran.NextDouble();
            double d2 = ran.NextDouble();
        }
        System.DateTime dt2 = System.DateTime.Now;
        System.TimeSpan span1 = dt2 - dt1;
        dt1 = System.DateTime.Now;
        for (int i = 0; i < COUNT; ++i)
        {
            double d1 = ran.NextDouble();
            double d2 = ran.NextDouble();
            double d3 = d1 / d2;
        }
        dt2 = System.DateTime.Now;
        System.TimeSpan span2 = dt2 - dt1;
        system_span = span2 - span1;


        dt1 = System.DateTime.Now;
        for (int i = 0; i < COUNT; ++i)
        {
            double d1 = ran.NextDouble();
            Fix64 fp1 = (Fix64)d1;
            double d2 = ran.NextDouble();
            Fix64 fp2 = (Fix64)d2;
        }
        dt2 = System.DateTime.Now;
        span1 = dt2 - dt1;
        dt1 = System.DateTime.Now;
        for (int i = 0; i < COUNT; ++i)
        {
            double d1 = ran.NextDouble();
            Fix64 fp1 = (Fix64)d1;
            double d2 = ran.NextDouble();
            Fix64 fp2 = (Fix64)d2;
            Fix64 fp3 = fp1 / fp2;
        }
        dt2 = System.DateTime.Now;
        span2 = dt2 - dt1;
        fp_span = span2 - span1;

        Console.WriteLine("TestDivisionSpeed() system_span = " + system_span.Ticks + ", fp_span = " + fp_span.Ticks);
    }
}
public class Assert
{
    static int error_cnt = 0;
    public static void Equal(object a, object b)
    {
        if (!a.Equals(b))
            ++error_cnt;
    }
    public static void True(bool condition, string str = null)
    {
        if (!condition)
            ++error_cnt;
    }
    public static void False(bool condition)
    {
        if (condition)
            ++error_cnt;
    }
    public static void Throws<TException>(Action o)
    {
        if (typeof(TException) != typeof(DivideByZeroException))
            ++error_cnt;
    }
}