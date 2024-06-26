using System;
using System.Linq;
using Nixill.CalcLib.Exception;
using Nixill.CalcLib.Functions;
using Nixill.CalcLib.Objects;
using Nixill.CalcLib.Operators;
using Nixill.CalcLib.Varaibles;
using static Nixill.CalcLib.Modules.Casting;

namespace Nixill.CalcLib.Modules
{
  public class MathModule
  {
    public static bool Loaded { get; private set; } = false;

    public const int FactPriority = MainModule.PowerPriority;

    private static CalcNumber numE;
    private static CalcNumber numPI;

    public static CLCodeFunction E { get; private set; }
    public static CLCodeFunction PI { get; private set; }

    public static CLCodeFunction Abs { get; private set; }
    public static CLCodeFunction Acos { get; private set; }
    public static CLCodeFunction Acosh { get; private set; }
    public static CLCodeFunction Asin { get; private set; }
    public static CLCodeFunction Asinh { get; private set; }
    public static CLCodeFunction Atan { get; private set; }
    public static CLCodeFunction Atan2 { get; private set; }
    public static CLCodeFunction Atanh { get; private set; }
    public static CLCodeFunction Ceiling { get; private set; }
    public static CLCodeFunction CopySign { get; private set; }
    public static CLCodeFunction Cos { get; private set; }
    public static CLCodeFunction Cosh { get; private set; }
    public static CLCodeFunction Floor { get; private set; }
    public static CLCodeFunction Log { get; private set; }
    public static CLCodeFunction Max { get; private set; }
    public static CLCodeFunction MaxMagnitude { get; private set; }
    public static CLCodeFunction Min { get; private set; }
    public static CLCodeFunction MinMagnitude { get; private set; }
    public static CLCodeFunction Sign { get; private set; }
    public static CLCodeFunction Sin { get; private set; }
    public static CLCodeFunction Sinh { get; private set; }
    public static CLCodeFunction Tan { get; private set; }
    public static CLCodeFunction Tanh { get; private set; }

    public static CLPostfixOperator PostFactorial { get; private set; }

    public static void Load(int factPriority = FactPriority)
    {
      LoadPostFactorial(factPriority);

      numE = new CalcNumber((decimal)Math.E);
      numPI = new CalcNumber((decimal)Math.PI);

      E = new CLCodeFunction("e", (pars, vars, context) => numE);
      PI = new CLCodeFunction("pi", (pars, vars, context) => numPI);

      Abs = new CLCodeFunction("abs", AbsFunction);
      Acos = new CLCodeFunction("acos", AcosFunction);
      Acosh = new CLCodeFunction("acosh", AcoshFunction);
      Asin = new CLCodeFunction("asin", AsinFunction);
      Asinh = new CLCodeFunction("asinh", AsinhFunction);
      Atan = new CLCodeFunction("atan", AtanFunction);
      Atan2 = new CLCodeFunction("atan2", Atan2Function);
      Atanh = new CLCodeFunction("atanh", AtanhFunction);
      Cos = new CLCodeFunction("cos", CosFunction);
      Cosh = new CLCodeFunction("cosh", CoshFunction);
      Sin = new CLCodeFunction("sin", SinFunction);
      Sinh = new CLCodeFunction("sinh", SinhFunction);
      Tan = new CLCodeFunction("tan", TanFunction);
      Tanh = new CLCodeFunction("tanh", TanhFunction);
      Ceiling = new CLCodeFunction("ceiling", CeilingFunction);
      CopySign = new CLCodeFunction("copysign", CopySignFunction);
      Floor = new CLCodeFunction("floor", FloorFunction);
      Log = new CLCodeFunction("log", LogFunction);
      Max = new CLCodeFunction("max", MaxFunction);
      MaxMagnitude = new CLCodeFunction("maxmagnitude", MaxMagnitudeFunction);
      Min = new CLCodeFunction("min", MinFunction);
      MinMagnitude = new CLCodeFunction("minmagnitude", MinMagnitudeFunction);
      Sign = new CLCodeFunction("sign", SignFunction);
    }

    // Raises one number to the power of another.
    public static CLPostfixOperator LoadPostFactorial(int priority = FactPriority)
    {
      PostFactorial = CLOperators.PostfixOperators.GetOrNull("!") ?? new CLPostfixOperator("!", FactPriority, true);
      PostFactorial.AddFunction(tNum, PostFactNumber);
      PostFactorial.AddFunction(tLst, (param, vars, context) => PostFactNumber(ListToNum(param), vars, context));

      return PostFactorial;
    }

    public static CalcValue PostFactNumber(CalcObject param, CLLocalStore vars, CLContextProvider context)
    {
      CalcNumber num = param as CalcNumber;
      int o = 1;

      for (int i = 2; i < num.Value; i++)
      {
        o *= i;
      }

      return new CalcNumber(o);
    }

    // Calculates the absolute value of a number.
    public static CalcValue AbsFunction(CalcObject[] pars, CLLocalStore vars, CLContextProvider context)
    {
      if (pars.Length == 0) throw new CLException("{!abs} requires a number.");

      CalcNumber num = NumberAt(pars, 0, "!abs", vars, context);
      return new CalcNumber(Math.Abs(num.Value));
    }

    // Calculates the angle that gives a specified cosine (-1 ≤ input ≤ 1).
    public static CalcValue AcosFunction(CalcObject[] pars, CLLocalStore vars, CLContextProvider context)
    {
      if (pars.Length == 0) throw new CLException("{!acos} requires a number.");

      CalcNumber num = NumberAt(pars, 0, "!acos", vars, context);
      return new CalcNumber((decimal)Math.Acos((double)num.Value));
    }

    // Calculates the angle that gives a specified hyperbolic cosine (1 ≤ input).
    public static CalcValue AcoshFunction(CalcObject[] pars, CLLocalStore vars, CLContextProvider context)
    {
      if (pars.Length == 0) throw new CLException("{!acosh} requires a number.");

      CalcNumber num = NumberAt(pars, 0, "!acosh", vars, context);
      return new CalcNumber((decimal)Math.Acosh((double)num.Value));
    }

    // Calculates the angle that gives a specified sine (-1 ≤ input ≤ 1).
    public static CalcValue AsinFunction(CalcObject[] pars, CLLocalStore vars, CLContextProvider context)
    {
      if (pars.Length == 0) throw new CLException("{!asin} requires a number.");

      CalcNumber num = NumberAt(pars, 0, "!asin", vars, context);
      return new CalcNumber((decimal)Math.Asin((double)num.Value));
    }

    // Calculates the angle that gives a specified hyperbolic sine.
    public static CalcValue AsinhFunction(CalcObject[] pars, CLLocalStore vars, CLContextProvider context)
    {
      if (pars.Length == 0) throw new CLException("{!asinh} requires a number.");

      CalcNumber num = NumberAt(pars, 0, "!asinh", vars, context);
      return new CalcNumber((decimal)Math.Asinh((double)num.Value));
    }

    // Calculates the angle that gives a specified tangent.
    public static CalcValue AtanFunction(CalcObject[] pars, CLLocalStore vars, CLContextProvider context)
    {
      if (pars.Length == 0) throw new CLException("{!atan} requires a number.");

      CalcNumber num = NumberAt(pars, 0, "!atan", vars, context);
      return new CalcNumber((decimal)Math.Atan((double)num.Value));
    }

    // Calculates the angle that gives a specified hyperbolic tangent (-1 ≤ input ≤ 1).
    public static CalcValue AtanhFunction(CalcObject[] pars, CLLocalStore vars, CLContextProvider context)
    {
      if (pars.Length == 0) throw new CLException("{!atanh} requires a number.");

      CalcNumber num = NumberAt(pars, 0, "!atanh", vars, context);
      return new CalcNumber((decimal)Math.Atanh((double)num.Value));
    }

    // Calculates the cosine of a given angle
    public static CalcValue CosFunction(CalcObject[] pars, CLLocalStore vars, CLContextProvider context)
    {
      if (pars.Length == 0) throw new CLException("{!cos} requires a number.");

      CalcNumber num = NumberAt(pars, 0, "!cos", vars, context);
      return new CalcNumber((decimal)Math.Cos((double)num.Value));
    }

    // Calculates the angle that gives a specified hyperbolic cosine (1 ≤ input).
    public static CalcValue CoshFunction(CalcObject[] pars, CLLocalStore vars, CLContextProvider context)
    {
      if (pars.Length == 0) throw new CLException("{!cosh} requires a number.");

      CalcNumber num = NumberAt(pars, 0, "!cosh", vars, context);
      return new CalcNumber((decimal)Math.Cosh((double)num.Value));
    }

    // Calculates the angle that gives a specified sine (-1 ≤ input ≤ 1).
    public static CalcValue SinFunction(CalcObject[] pars, CLLocalStore vars, CLContextProvider context)
    {
      if (pars.Length == 0) throw new CLException("{!sin} requires a number.");

      CalcNumber num = NumberAt(pars, 0, "!sin", vars, context);
      return new CalcNumber((decimal)Math.Sin((double)num.Value));
    }

    // Calculates the angle that gives a specified hyperbolic sine.
    public static CalcValue SinhFunction(CalcObject[] pars, CLLocalStore vars, CLContextProvider context)
    {
      if (pars.Length == 0) throw new CLException("{!sinh} requires a number.");

      CalcNumber num = NumberAt(pars, 0, "!sinh", vars, context);
      return new CalcNumber((decimal)Math.Sinh((double)num.Value));
    }

    // Calculates the angle that gives a specified tangent.
    public static CalcValue TanFunction(CalcObject[] pars, CLLocalStore vars, CLContextProvider context)
    {
      if (pars.Length == 0) throw new CLException("{!tan} requires a number.");

      CalcNumber num = NumberAt(pars, 0, "!tan", vars, context);
      return new CalcNumber((decimal)Math.Atan((double)num.Value));
    }

    // Calculates the angle that gives a specified hyperbolic tangent (-1 ≤ input ≤ 1).
    public static CalcValue TanhFunction(CalcObject[] pars, CLLocalStore vars, CLContextProvider context)
    {
      if (pars.Length == 0) throw new CLException("{!tanh} requires a number.");

      CalcNumber num = NumberAt(pars, 0, "!tanh", vars, context);
      return new CalcNumber((decimal)Math.Atanh((double)num.Value));
    }

    // Calculates the angle of a given set of coordinates.
    public static CalcValue Atan2Function(CalcObject[] pars, CLLocalStore vars, CLContextProvider context)
    {
      if (pars.Length < 2) throw new CLException("{!atan2} requires two numbers.");

      CalcNumber y = NumberAt(pars, 0, "!atan2", vars, context);
      CalcNumber x = NumberAt(pars, 1, "!atan2", vars, context);
      return new CalcNumber((decimal)Math.Atan2((double)y.Value, (double)x.Value));
    }

    // Returns the smallest integral value greater than or equal to the parameter. (Rounds towards +∞)
    public static CalcValue CeilingFunction(CalcObject[] pars, CLLocalStore vars, CLContextProvider context)
    {
      if (pars.Length == 0) throw new CLException("{!ceiling} requires a number.");

      CalcNumber num = NumberAt(pars, 0, "!ceiling", vars, context);
      return new CalcNumber(Math.Ceiling(num));
    }

    // Returns a number with magnitude x and sign of y
    public static CalcValue CopySignFunction(CalcObject[] pars, CLLocalStore vars, CLContextProvider context)
    {
      if (pars.Length < 2) throw new CLException("{!copysign} requires two numbers.");

      CalcNumber x = NumberAt(pars, 0, "!copysign", vars, context);
      CalcNumber y = NumberAt(pars, 1, "!copysign", vars, context);
      return new CalcNumber((decimal)Math.CopySign((double)x.Value, (double)y.Value));
    }

    // Returns the largest integral value less than or equal to the parameter. (Rounds towards -∞)
    public static CalcValue FloorFunction(CalcObject[] pars, CLLocalStore vars, CLContextProvider context)
    {
      if (pars.Length == 0) throw new CLException("{!floor} requires a number.");

      CalcNumber num = NumberAt(pars, 0, "!floor", vars, context);
      return new CalcNumber(Math.Floor(num));
    }

    // Returns the natural logarithm of the parameter.
    public static CalcValue LogFunction(CalcObject[] pars, CLLocalStore vars, CLContextProvider context)
    {
      if (pars.Length == 0) throw new CLException("{!log} requires a number.");

      CalcNumber num = NumberAt(pars, 0, "!floor", vars, context);
      return new CalcNumber((decimal)Math.Log((double)num.Value));
    }

    // Returns the maximum value out of the list.
    public static CalcValue MaxFunction(CalcObject[] pars, CLLocalStore vars, CLContextProvider context)
    {
      if (pars.Length == 0) throw new CLException("{!max} requires numbers.");

      decimal max = Decimal.MinValue;

      for (int i = 0; i < pars.Length; i++)
      {
        CalcNumber num = NumberAt(pars, i, "!max", vars, context);
        max = Math.Max(max, num.Value);
      }

      return new CalcNumber(max);
    }

    // Returns the maximum value out of the list.
    public static CalcValue MaxMagnitudeFunction(CalcObject[] pars, CLLocalStore vars, CLContextProvider context)
    {
      if (pars.Length == 0) throw new CLException("{!maxmagnitude} requires numbers.");

      decimal max = (decimal)0;

      for (int i = 0; i < pars.Length; i++)
      {
        CalcNumber num = NumberAt(pars, i, "!maxmagnitude", vars, context);
        if (Math.Abs(num.Value) > max)
        {
          max = num;
        }
      }

      return new CalcNumber(max);
    }

    // Returns the minimum value out of the list.
    public static CalcValue MinFunction(CalcObject[] pars, CLLocalStore vars, CLContextProvider context)
    {
      if (pars.Length == 0) throw new CLException("{!min} requires numbers.");

      decimal min = Decimal.MaxValue;

      for (int i = 0; i < pars.Length; i++)
      {
        CalcNumber num = NumberAt(pars, i, "!min", vars, context);
        min = Math.Min(min, num.Value);
      }

      return new CalcNumber(min);
    }

    // Returns the minimum value out of the list.
    public static CalcValue MinMagnitudeFunction(CalcObject[] pars, CLLocalStore vars, CLContextProvider context)
    {
      if (pars.Length == 0) throw new CLException("{!minmagnitude} requires numbers.");

      decimal min = (decimal)Decimal.MinValue;

      for (int i = 0; i < pars.Length; i++)
      {
        CalcNumber num = NumberAt(pars, i, "!minmagnitude", vars, context);
        if (Math.Abs(num.Value) < min)
        {
          min = num;
        }
      }

      return new CalcNumber(min);
    }

    public static CalcValue SignFunction(CalcObject[] pars, CLLocalStore vars, CLContextProvider context)
    {
      if (pars.Length == 0) throw new CLException("{!sign} requires a number.");

      CalcNumber num = NumberAt(pars, 0, "!sign", vars, context);
      return new CalcNumber(Math.Sign(num));
    }
  }
}
