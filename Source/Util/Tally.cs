using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RimTest.Util
{
    /// <summary>
    /// Dictionnary&lt;T, int&gt; auto defaulting to 0. Allows simple tallying syntax such as tally[element]++  
    /// </summary>
    /// <example>
    /// <![CDATA[
    /// // without Tally
    /// Dictionary<T, int> old = new Dictionary<T, int>();
    /// foreach(var k in Enum.GetValue(typeof(T))){ old[k] = 0; }
    /// // with Tally
    /// Tally<T> now = new Tally<T>();
    /// ]]>
    /// </example>
    /// <typeparam name="T">Enumeration</typeparam>
    public class Tally<T> : Dictionary<T, int> where T : Enum
    {
        /// <summary>
        /// </summary>
        public Tally()
        {
            foreach (T status in Enum.GetValues(typeof(T)))
            {
                this[status] = 0;
            }
        }
    }
}
