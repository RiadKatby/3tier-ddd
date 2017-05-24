using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core
{
    public static class Extensions
    {
        /// <summary>
        /// Determins if this value existed withen the given group.
        /// </summary>
        /// <param name="value">value to be checked.</param>
        /// <param name="group">group of values.</param>
        /// <returns></returns>
        public static bool In(this int value, params int[] group)
        {
            foreach (var item in group)
                if (value == item)
                    return true;

            return false;
        }

        /// <summary>
        /// Determins if this value existed withen the given group.
        /// </summary>
        /// <param name="value">value to be checked.</param>
        /// <param name="group">group of values.</param>
        /// <returns></returns>
        public static bool In(this byte value, params byte[] group)
        {
            foreach (var item in group)
                if (value == item)
                    return true;

            return false;
        }
    }
}
