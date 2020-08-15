using System;
using System.Runtime.Serialization;

namespace RimTest
{
    [Serializable]
    public class AssertionException : Exception
    {
        public AssertionException()
        {
        }

        public AssertionException(string message) : base(message)
        {
        }

        public AssertionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AssertionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class Assertion
    {
        public static AssertValue Assert(IComparable thing)
        {
            return new AssertValue(thing);
        }
        public static AssertFunc AssertFunc(Func<dynamic> thing)
        {
            if (thing == null) throw new ArgumentNullException("A function cannot be null");
            return new AssertFunc(thing);
        }
        public static AssertAction AssertFunc(Action thing)
        {
            if (thing == null) throw new ArgumentNullException("A function cannot be null");
            return new AssertAction(thing);
        }

        protected bool negated = false;
    }

    public class AssertValue : Assertion
    {
        private readonly IComparable thing;

        public AssertValue(IComparable thing) : base()
        {
            this.thing = thing;
        }

        public AssertValue() : base()
        { }

        public AssertValue Not
        {
            get
            {
                negated = !negated;
                return this;
            }
        }


        public AssertValue To
        {
            get
            {
                return this;
            }
        }


        public AssertValue Is
        {
            get
            {
                return this;
            }
        }


        public AssertValue Be
        {
            get
            {
                return this;
            }
        }

        public AssertValue Do
        {
            get
            {
                return this;
            }
        }

        public void EqualTo(IComparable otherThing)
        {
            bool result = thing.CompareTo(otherThing) == 0;
            if (negated) result = !result;
            if (!result) throw new AssertionException($"Expected {thing}{(negated ? " not" : "") } to be equal to {otherThing}.");
        }
        public void LessThan(IComparable otherThing)
        {
            bool result = thing.CompareTo(otherThing) < 0;
            if (negated) result = !result;
            if (!result) throw new AssertionException($"Expected {thing}{(negated ? " not" : "") } to be less than {otherThing}.");
        }
        public void GreaterThan(IComparable otherThing)
        {
            bool result = thing.CompareTo(otherThing) > 0;
            if (negated) result = !result;
            if (!result) throw new AssertionException($"Expected {thing}{(negated ? " not" : "") } to be greater than {otherThing}.");
        }
        public void BetweenInclusive(IComparable minThing, IComparable maxThing)
        {
            bool result = thing.CompareTo(minThing) >= 0 && thing.CompareTo(maxThing) <= 0;
            if (negated) result = !result;
            if (!result) throw new AssertionException($"Expected {thing}{(negated ? " not" : "") } to be between (inclusive) {minThing} and {maxThing}.");
        }
        public void BetweenExclusive(IComparable minThing, IComparable maxThing)
        {
            bool result = thing.CompareTo(minThing) > 0 && thing.CompareTo(maxThing) < 0;
            if (negated) result = !result;
            if (!result) throw new AssertionException($"Expected {thing}{(negated ? " not" : "") } to be between (exclusive) {minThing} and {maxThing}.");
        }

        public void TheSame(IComparable otherThing)
        {
            bool result = thing.Equals(otherThing);
            if (negated) result = !result;
            if (!result) throw new AssertionException($"Expected {thing}{(negated ? " not" : "") } to be the same as {otherThing}.");
        }

        public void Null()
        {
            bool result = thing == null;
            if (negated) result = !result;
            if (!result) throw new AssertionException($"Expected {thing}{(negated ? " not" : "") } to be null.");
        }

        public void True()
        {
            bool result = thing.Equals(true);
            if (negated) result = !result;
            if (!result) throw new AssertionException($"Expected {thing}{(negated ? " not" : "") } to be true.");
        }

        public void False()
        {
            bool result = thing.Equals(false);
            if (negated) result = !result;
            if (!result) throw new AssertionException($"Expected {thing}{(negated ? " not" : "") } to be false.");
        }

    }


    public class AssertFunc : Assertion
    {
        private Func<dynamic> func; 

        public AssertFunc(Func<dynamic> thing) : base()
        {
            this.func = thing ?? throw new NullReferenceException("AssertFunc function cannot be null");
        }
        public AssertFunc Not
        {
            get
            {
                negated = !negated;
                return this;
            }
        }


        public AssertFunc To
        {
            get
            {
                return this;
            }
        }


        public AssertFunc Is
        {
            get
            {
                return this;
            }
        }


        public AssertFunc Be
        {
            get
            {
                return this;
            }
        }

        public new AssertFunc Do
        {
            get
            {
                return this;
            }
        }

        protected AssertFunc() : base() { }

        public void Throw()
        {
            if (func == null) throw new NullReferenceException("AssertFunc function cannot be null"); 
            try
            {
                func();
            }
            catch (Exception temp)
            {
                if (negated) throw new AssertionException($"Expected the function not to throw.", temp);
                else return;
            }
            if (negated) return;
            throw new AssertionException($"Expected the function to throw.");
        }
    }

    public class AssertAction : AssertFunc
    {
        private new Action action;

        public AssertAction(Action thing) : base()
        {
            this.action = thing ?? throw new NullReferenceException("AssertFunc function cannot be null");
        }
        public new AssertAction Not
        {
            get
            {
                negated = !negated;
                return this;
            }
        }


        public new AssertAction To
        {
            get
            {
                return this;
            }
        }


        public new AssertAction Is
        {
            get
            {
                return this;
            }
        }


        public new AssertAction Be
        {
            get
            {
                return this;
            }
        }

        public new AssertAction Do
        {
            get
            {
                return this;
            }
        }

        public void Throw()
        {
            if (action == null) throw new NullReferenceException("AssertFunc function cannot be null");
            try
            {
                action();
            }
            catch (Exception temp)
            {
                if (negated) throw new AssertionException($"Expected the function not to throw.", temp);
                else return;
            }
            if (negated) return;
            throw new AssertionException($"Expected the function to throw.");
        }
    }
}
