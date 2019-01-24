// Author: https://gist.github.com/fubar-coder/39b851a0f5f5d248767e

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3DS.Engine.Helpers
{
    public static class SliceExtensions
    {
        public static Slice<T> ToSlice<T>(this T[] array)
        {
            return new Slice<T>(array);
        }

        public static Slice<T> ToSlice<T>(this T[] array, int offset, int length)
        {
            return new Slice<T>(array, offset, length);
        }

        public static Slice<T> ToSlice<T>(this Slice<T> slice)
        {
            return slice;
        }

        public static Slice<T> ToSlice<T>(this Slice<T> slice, int offset, int length)
        {
            return new Slice<T>(slice, offset, length);
        }
    }
}
