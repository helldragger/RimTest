using System;
using System.Runtime.Serialization;

namespace RimTest
{
    /// <summary>
    /// Used when the condition expected from an assertion is not reached. 
    /// </summary>
    /// <remarks>Throwing one indicates a test failure.</remarks>
    [Serializable]
    public class AssertionException : Exception
    {
        /// <summary>
        /// constructor directly based on the base Exception
        /// </summary>
        public AssertionException()
        {
        }

        /// <summary>
        /// constructor directly based on the base Exception
        /// </summary>
        public AssertionException(string message) : base(message)
        {
        }

        /// <summary>
        /// constructor directly based on the base Exception
        /// </summary>
        public AssertionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// constructor directly based on the base Exception
        /// </summary>
        protected AssertionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    /// <summary>
    /// Generic assertion grammar object.
    /// </summary>
    /// <remarks>
    /// Usage: Assert(value)[.To|.Do|.Be|.Is|.Not].Comparison([Parameters...])
    /// </remarks>
    public class Assertion
    {
        /// <summary>
        /// Entry point for value based assertions
        /// </summary>
        /// <param name="thing">A comparable value</param>
        /// <returns>An AssertValue, specialized in asserting values</returns>
        public static AssertValue Assert(IComparable thing)
        {
            return new AssertValue(thing);
        }

        /// <summary>
        /// Entry point for Function based assertions
        /// </summary>
        /// <param name="thing">A function</param>
        /// <returns>An AssertFunc, specialized in asserting Functions</returns>
        public static AssertFunc AssertFunc(Func<dynamic> thing)
        {
            if (thing == null) throw new ArgumentNullException("A function cannot be null");
            return new AssertFunc(thing);
        }

        /// <summary>
        /// Entry point for Action based assertions
        /// </summary>
        /// <param name="thing">An Action</param>
        /// <returns>An AssertAction, specialized in asserting Actions</returns>
        public static AssertAction AssertFunc(Action thing)
        {
            if (thing == null) throw new ArgumentNullException("A function cannot be null");
            return new AssertAction(thing);
        }
        /// <summary>
        /// Negation flag, can be double negated with multiple uses of the .Not grammar, used to negate a check expectation. (i.e. Assert(1).Not.To.Be.EqualTo(2))
        /// </summary>
        protected bool negated = false;
    }

    /// <summary>
    /// Specialized Assertion for Values
    /// </summary>
    /// <remarks>Allows the EqualTo, LessThan, GreaterThan, BetweenInclusive, BetweenExclusive, TheSame, Null, True and False checks.</remarks>
    public class AssertValue : Assertion
    {
        private readonly IComparable thing;
        /// <summary>
        /// base constructor
        /// </summary>
        /// <param name="thing">The value to check</param>
        public AssertValue(IComparable thing) : base()
        {
            this.thing = thing;
        }
        /// <summary>
        /// Negation grammar link, negates the current assertion.
        /// </summary>
        /// <remarks>All grammar links can be chained as needed.</remarks>
        public AssertValue Not
        {
            get
            {
                negated = !negated;
                return this;
            }
        }


        /// <summary>
        /// Grammar link, doesn't have any effect.
        /// </summary>
        /// <remarks>All grammar links can be chained as needed.</remarks>
        public AssertValue To
        {
            get
            {
                return this;
            }
        }


        /// <summary>
        /// Grammar link, doesn't have any effect.
        /// </summary>
        /// <remarks>All grammar links can be chained as needed.</remarks>
        public AssertValue Is
        {
            get
            {
                return this;
            }
        }


        /// <summary>
        /// Grammar link, doesn't have any effect.
        /// </summary>
        /// <remarks>All grammar links can be chained as needed.</remarks>
        public AssertValue Be
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Grammar link, doesn't have any effect.
        /// </summary>
        /// <remarks>All grammar links can be chained as needed.</remarks>
        public AssertValue Do
        {
            get
            {
                return this;
            }
        }
        /// <summary>
        /// Check: asserted value EQUALS TO otherThing
        /// </summary>
        /// <param name="otherThing">check value</param>
        public void EqualTo(IComparable otherThing)
        {
            bool result = thing.CompareTo(otherThing) == 0;
            if (negated) result = !result;
            if (!result) throw new AssertionException($"Expected {thing}{(negated ? " not" : "") } to be equal to {otherThing}.");
        }
        /// <summary>
        /// Check: asserted value LESS THAN otherThing
        /// </summary>
        /// <param name="otherThing">check value</param>
        public void LessThan(IComparable otherThing)
        {
            bool result = thing.CompareTo(otherThing) < 0;
            if (negated) result = !result;
            if (!result) throw new AssertionException($"Expected {thing}{(negated ? " not" : "") } to be less than {otherThing}.");
        }
        /// <summary>
        /// Check: asserted value GREATER THAN otherThing
        /// </summary>
        /// <param name="otherThing">check value</param>
        public void GreaterThan(IComparable otherThing)
        {
            bool result = thing.CompareTo(otherThing) > 0;
            if (negated) result = !result;
            if (!result) throw new AssertionException($"Expected {thing}{(negated ? " not" : "") } to be greater than {otherThing}.");
        }
        /// <summary>
        /// Check: minThing LESS THAN or EQUAL TO assertd value AND asserted value LESS THAN or EQUAL TO maxThing
        /// </summary>
        /// <param name="minThing">min check value</param>
        /// <param name="maxThing">max check value</param>
        public void BetweenInclusive(IComparable minThing, IComparable maxThing)
        {
            bool result = thing.CompareTo(minThing) >= 0 && thing.CompareTo(maxThing) <= 0;
            if (negated) result = !result;
            if (!result) throw new AssertionException($"Expected {thing}{(negated ? " not" : "") } to be between (inclusive) {minThing} and {maxThing}.");
        }
        /// <summary>
        /// Check: minThing LESS THAN assertd value AND asserted value LESS THAN maxThing
        /// </summary>
        /// <param name="minThing">min check value</param>
        /// <param name="maxThing">max check value</param>
        public void BetweenExclusive(IComparable minThing, IComparable maxThing)
        {
            bool result = thing.CompareTo(minThing) > 0 && thing.CompareTo(maxThing) < 0;
            if (negated) result = !result;
            if (!result) throw new AssertionException($"Expected {thing}{(negated ? " not" : "") } to be between (exclusive) {minThing} and {maxThing}.");
        }

        /// <summary>
        /// Check: asserted value SAME AS otherThing
        /// </summary>
        /// <param name="otherThing">check value</param>
        public void TheSame(IComparable otherThing)
        {
            bool result = thing.Equals(otherThing);
            if (negated) result = !result;
            if (!result) throw new AssertionException($"Expected {thing}{(negated ? " not" : "") } to be the same as {otherThing}.");
        }
        /// <summary>
        /// Check: thing IS null
        /// </summary>
        public void Null()
        {
            bool result = thing == null;
            if (negated) result = !result;
            if (!result) throw new AssertionException($"Expected {thing}{(negated ? " not" : "") } to be null.");
        }
        /// <summary>
        /// Check: thing IS true
        /// </summary>
        public void True()
        {
            bool result = thing.Equals(true);
            if (negated) result = !result;
            if (!result) throw new AssertionException($"Expected {thing}{(negated ? " not" : "") } to be true.");
        }
        /// <summary>
        /// Check: thing IS false
        /// </summary>
        public void False()
        {
            bool result = thing.Equals(false);
            if (negated) result = !result;
            if (!result) throw new AssertionException($"Expected {thing}{(negated ? " not" : "") } to be false.");
        }
    }


    /// <summary>
    /// Specialized Assertion for Functions
    /// </summary>
    /// <remarks>Allows the Throw check.</remarks>
    public class AssertFunc : Assertion
    {
        private readonly Func<dynamic> func;

        /// <summary>
        /// base constructor
        /// </summary>
        /// <param name="thing">The function to check</param>
        public AssertFunc(Func<dynamic> thing) : base()
        {
            func = thing ?? throw new NullReferenceException("AssertFunc function cannot be null");
        }
        /// <summary>
        /// Negation grammar link, negates the current assertion.
        /// </summary>
        /// <remarks>All grammar links can be chained as needed.</remarks>
        public AssertFunc Not
        {
            get
            {
                negated = !negated;
                return this;
            }
        }


        /// <summary>
        /// Grammar link, doesn't have any effect.
        /// </summary>
        /// <remarks>All grammar links can be chained as needed.</remarks>
        public AssertFunc To
        {
            get
            {
                return this;
            }
        }


        /// <summary>
        /// Grammar link, doesn't have any effect.
        /// </summary>
        /// <remarks>All grammar links can be chained as needed.</remarks>
        public AssertFunc Is
        {
            get
            {
                return this;
            }
        }


        /// <summary>
        /// Grammar link, doesn't have any effect.
        /// </summary>
        /// <remarks>All grammar links can be chained as needed.</remarks>
        public AssertFunc Be
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Grammar link, doesn't have any effect.
        /// </summary>
        /// <remarks>All grammar links can be chained as needed.</remarks>
        public AssertFunc Do
        {
            get
            {
                return this;
            }
        }
        /// <summary>
        /// Internal constructor for AssertAction usage
        /// </summary>
        /// <remarks>Do NOT use manually</remarks>
        protected AssertFunc() : base() { }
        
        /// <summary>
        /// Check: executing asserted callable throws an error.
        /// </summary>
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

    /// <summary>
    /// Specialized Assertion for Actions
    /// </summary>
    /// <remarks>Allows the Throw check.</remarks>
    public class AssertAction : AssertFunc
    {
        private readonly Action action;

        /// <summary>
        /// base constructor
        /// </summary>
        /// <param name="thing">The function to check</param>
        public AssertAction(Action thing) : base()
        {
            action = thing ?? throw new NullReferenceException("AssertFunc function cannot be null");
        }
        /// <summary>
        /// Negation grammar link, negates the current assertion.
        /// </summary>
        /// <remarks>All grammar links can be chained as needed.</remarks>
        public new AssertAction Not
        {
            get
            {
                negated = !negated;
                return this;
            }
        }


        /// <summary>
        /// Grammar link, doesn't have any effect.
        /// </summary>
        /// <remarks>All grammar links can be chained as needed.</remarks>
        public new AssertAction To
        {
            get
            {
                return this;
            }
        }


        /// <summary>
        /// Grammar link, doesn't have any effect.
        /// </summary>
        /// <remarks>All grammar links can be chained as needed.</remarks>
        public new AssertAction Is
        {
            get
            {
                return this;
            }
        }


        /// <summary>
        /// Grammar link, doesn't have any effect.
        /// </summary>
        /// <remarks>All grammar links can be chained as needed.</remarks>
        public new AssertAction Be
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Grammar link, doesn't have any effect.
        /// </summary>
        /// <remarks>All grammar links can be chained as needed.</remarks>
        public new AssertAction Do
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Check: executing asserted callable throws an error.
        /// </summary>
        public new void Throw()
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
