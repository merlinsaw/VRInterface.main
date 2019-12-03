//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
//
//********* https://stackoverflow.com/questions/26280788/dictionary-enum-key-performance *********
//
//          dictionary enum key performance optimisation
//          concern about generic dictionaries using enums for keys.
//
//As stated at the below page, using enums for keys will allocate memory: 
//http://blogs.msdn.com/b/shawnhar/archive/2007/07/02/twin-paths-to-garbage-collector-nirvana.aspx

using System;
using System.Collections.Generic;
using System.Linq.Expressions;


#if not_jet_customised

namespace external.common.tools
{

    struct SteamVREnumEqualityComparer<TEnum> : IEqualityComparer<TEnum> where TEnum : struct
    {

        static class BoxAvoidance
        {
            static readonly Func<TEnum, int> _wrapper;

            public static int ToInt(TEnum enu)
            {
                return _wrapper(enu);
            }

            static BoxAvoidance()
            {
                var p = Expression.Parameter(typeof(TEnum), null);
                var c = Expression.ConvertChecked(p, typeof(int));

                _wrapper = Expression.Lambda<Func<TEnum, int>>(c, p).Compile();
            }
        }

        public bool Equals(TEnum firstEnum, TEnum secondEnum)
        {
            return BoxAvoidance.ToInt(firstEnum) == BoxAvoidance.ToInt(secondEnum);
        }

        public int GetHashCode(TEnum firstEnum)
        {
            return BoxAvoidance.ToInt(firstEnum);
        }
    }

    public struct SteamVR_Input_Sources_Comparer : IEqualityComparer<SteamVR_Input_Sources>
    {
        public bool Equals(SteamVR_Input_Sources x, SteamVR_Input_Sources y)
        {
            return x == y;
        }

        public int GetHashCode(SteamVR_Input_Sources obj)
        {
            return (int)obj;
        }

    }
}
#endif