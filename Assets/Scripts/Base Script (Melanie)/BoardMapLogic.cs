using UnityEngine;
using System.Collections.Generic;

public static class BoardMap
{
    // Key = starting field ID
    // Each entry contains an array of directions
    // Each direction is an ordered list of fields (step 1 → 4)
    public static readonly Dictionary<int, int[][]> Paths =
        new Dictionary<int, int[][]>
    {
        {
            1,
            new int[][]
            {
                new int[] { 2, 3, 4, 5 },
                new int[] { 10, 16, 23, 31 },
                new int[] { 11, 18, 26, 35 },
            }
        },
        {
            2,
            new int[][]
            {
                new int[] { 1 },
                new int[] { 10, 17, 25, 34 },
                new int[] { 9, 15, 22, 30 },
                new int[] { 3, 4, 5 },
            }
        },
        {
            3,
            new int[][]
            {
                new int[] { 2, 1 },
                new int[] { 9, 16, 24, 33 },
                new int[] { 8, 14, 21, 29 },
                new int[] { 4, 5 },
            }
        },
        {
            4,
            new int[][]
            {
                new int[] { 5 },
                new int[] { 7, 13, 20, 28 },
                new int[] { 8, 15, 23, 32 },
                new int[] { 3, 2, 1 },
            }
        },
        {
            5,
            new int[][]
            {
                new int[] { 4, 3, 2, 1 },
                new int[] { 7, 14, 22, 31 },
                new int[] { 6, 12, 19, 27 },
            }
        },
        {
            6,
            new int[][]
            {
                new int[] { 5 },
                new int[] { 7, 8, 9, 10 },
                new int[] { 13, 21, 30, 39 },
                new int[] { 12, 19, 27 },
            }
        },
        {
            7,
            new int[][]
            {
                new int[] { 5 },
                new int[] { 4 },
                new int[] { 6 },
                new int[] { 13, 20, 28, 36 },
                new int[] { 14, 22, 31, 40 },
                new int[] { 8, 9, 10, 11 },
            }
        },
        {
            8,
            new int[][]
            {
                new int[] { 4 },
                new int[] { 3 },
                new int[] { 7, 6 },
                new int[] { 9, 10, 11 },
                new int[] { 14, 21, 29, 37 },
                new int[] { 15, 23, 32, 41 },
            }
        },
        {
            9,
            new int[][]
            {
                new int[] { 3 },
                new int[] { 2 },
                new int[] { 10, 11 },
                new int[] { 8, 7, 6 },
                new int[] { 15, 22, 30, 38 },
                new int[] { 16, 24, 33, 42 },
            }
        },
        {
            10,
            new int[][]
            {
                new int[] { 2 },
                new int[] { 1 },
                new int[] { 11 },
                new int[] { 17, 25, 34, 43 },
                new int[] { 16, 23, 31, 39 },
                new int[] { 9, 8, 7, 6 },
            }
        },
        {
            11,
            new int[][]
            {
                new int[] { 1 },
                new int[] { 10, 9, 8, 7 },
                new int[] { 17, 24, 32, 40 },
                new int[] { 18, 26, 35 },
            }
        },
        {
            12,
            new int[][]
            {
                new int[] { 6, 5 },
                new int[] { 13, 14, 15, 16 },
                new int[] { 20, 29, 38, 46 },
                new int[] { 19, 27 },
            }
        },
        {
            13,
            new int[][]
            {
                new int[] { 12 },
                new int[] { 6 },
                new int[] { 7, 4 },
                new int[] { 14, 15, 16, 17 },
                new int[] { 21, 30, 39, 47 },
                new int[] { 20, 28, 36 },
            }
        },
        {
            14,
            new int[][]
            {
                new int[] { 13, 12 },
                new int[] { 7, 5 },
                new int[] { 8, 3 },
                new int[] { 15, 16, 17, 18 },
                new int[] { 22, 31, 40, 48 },
                new int[] { 21, 29, 37, 44 },
            }
        },
        {
            15,
            new int[][]
            {
                new int[] { 8, 4 },
                new int[] { 9, 2 },
                new int[] { 16, 17, 18 },
                new int[] { 23, 32, 41, 49 },
                new int[] { 22, 30, 38, 45 },
                new int[] { 14, 13, 12 },
            }
        },
        {
            16,
            new int[][]
            {
                new int[] { 9, 3 },
                new int[] { 10, 1 },
                new int[] { 17, 18 },
                new int[] { 24, 33, 42, 50 },
                new int[] { 23, 31, 39, 46 },
                new int[] { 15, 14, 13, 12 },
            }
        },
        {
            17,
            new int[][]
            {
                new int[] { 11 },
                new int[] { 18 },
                new int[] { 25, 34, 43 },
                new int[] { 24, 32, 40, 47 },
                new int[] { 16, 15, 14, 13 },
                new int[] { 10, 2 },
            }
        },
        {
            18,
            new int[][]
            {
                new int[] { 11, 1 },
                new int[] { 17, 16, 15, 14 },
                new int[] { 25, 33, 41, 48 },
                new int[] { 26, 35 },
            }
        },
        {
            19,
            new int[][]
            {
                new int[] { 12, 6, 5 },
                new int[] { 20, 21, 22, 23 },
                new int[] { 28, 37, 45, 52 },
                new int[] { 27 },
            }
        },
        {
            20,
            new int[][]
            {
                new int[] { 19 },
                new int[] { 12 },
                new int[] { 13, 7, 4 },
                new int[] { 21, 22, 23, 24 },
                new int[] { 29, 38, 46, 53 },
                new int[] { 28, 36 },
            }
        },
        {
            21,
            new int[][]
            {
                new int[] { 13, 6 },
                new int[] { 14, 8, 3 },
                new int[] { 22, 23, 24, 25 },
                new int[] { 30, 39, 47, 54 },
                new int[] { 29, 37, 44 },
                new int[] { 20, 19 },
            }
        },
        {
            22,
            new int[][]
            {
                new int[] { 21, 20, 19 },
                new int[] { 14, 7, 5 },
                new int[] { 15, 9, 2 },
                new int[] { 23, 24, 25, 26 },
                new int[] { 31, 40, 48, 55 },
                new int[] { 30, 38, 45, 51 },
            }
        },
        {
            23,
            new int[][]
            {
                new int[] { 22, 21, 20, 19 },
                new int[] { 15, 8, 4 },
                new int[] { 16, 10, 1 },
                new int[] { 24, 25, 26 },
                new int[] { 32, 41, 49, 56 },
                new int[] { 31, 39, 46, 52 },
            }
        },
        {
            24,
            new int[][]
            {
                new int[] { 25, 26 },
                new int[] { 33, 42, 50 },
                new int[] { 32, 40, 47, 53 },
                new int[] { 23, 22, 21, 20 },
                new int[] { 16, 9, 3 },
                new int[] { 17, 11 },
            }
        },
        {
            25,
            new int[][]
            {
                new int[] { 26 },
                new int[] { 34, 43 },
                new int[] { 33, 41, 48, 54 },
                new int[] { 24, 23, 22, 21 },
                new int[] { 17, 10, 2 },
            }
        },
        {
            26,
            new int[][]
            {
                new int[] { 18, 11, 1},
                new int[] { 25, 24, 23, 22 },
                new int[] { 34, 42, 49, 55 },
                new int[] { 35 },
            }
        },
        {
            27,
            new int[][]
            {
                new int[] { 19, 12, 6, 5 },
                new int[] { 28, 29, 30, 31 },
                new int[] { 36, 44, 51, 57 },
            }
        },
        {
            28,
            new int[][]
            {
                new int[] { 27 },
                new int[] { 19 },
                new int[] { 20, 13, 7, 4 },
                new int[] { 29, 30, 31, 32 },
                new int[] { 37, 45, 52, 58 },
                new int[] { 36}
            }
        },
        {
            29,
            new int[][]
            {
                new int[] { 28, 27 },
                new int[] { 20, 12 },
                new int[] { 21, 14, 8, 3 },
                new int[] { 30, 31, 32, 33 },
                new int[] { 38, 46, 53, 59 },
                new int[] { 37, 44 },
            }
        },
        {
            30,
            new int[][]
            {
                new int[] { 29, 28, 27 },
                new int[] { 21, 13, 6 },
                new int[] { 22, 15, 9, 2 },
                new int[] { 31, 32, 33, 34 },
                new int[] { 39, 47, 54, 60 },
                new int[] { 38, 45, 51 },
            }
        },
        {
            31,
            new int[][]
            {
                new int[] { 22, 14, 7, 5 },
                new int[] { 23, 16, 10, 1 },
                new int[] { 32, 33, 34, 35 },
                new int[] { 40, 48, 55, 61 },
                new int[] { 39, 46, 52, 57 },
                new int[] { 30, 29, 28, 27 },
            }
        },
        {
            32,
            new int[][]
            {
                new int[] { 23, 15, 8, 4 },
                new int[] { 24, 17, 11 },
                new int[] { 33, 34, 35 },
                new int[] { 41, 49, 56 },
                new int[] { 40, 47, 53, 58 },
                new int[] { 31, 30, 29, 28 },
            }
        },
        {
            33,
            new int[][]
            {
                new int[] { 24, 16, 9, 3 },
                new int[] { 25, 18 },
                new int[] { 34, 35 },
                new int[] { 42, 50 },
                new int[] { 41, 48, 54, 59 },
                new int[] { 32, 31, 30, 29 },
            }
        },
        {
            34,
            new int[][]
            {
                new int[] { 35 },
                new int[] { 43 },
                new int[] { 42, 49, 55, 60 },
                new int[] { 33, 32, 31, 30 },
                new int[] { 25, 17, 10, 2 },
                new int[] { 26 },
            }
        },
        {
            35,
            new int[][]
            {
                new int[] { 26, 18, 11, 1 },
                new int[] { 43, 50, 56, 61 },
                new int[] { 34, 33, 32, 31 },
            }
        },
        {
            36,
            new int[][]
            {
                new int[] { 27 },
                new int[] { 28, 20, 13, 7 },
                new int[] { 37, 38, 39, 40 },
                new int[] { 44, 51, 57 },
            }
        },
        {
            37,
            new int[][]
            {
                new int[] { 36 },
                new int[] { 28, 19 },
                new int[] { 29, 21, 14, 8 },
                new int[] { 38, 39, 40, 41 },
                new int[] { 45, 52, 58 },
                new int[] { 44 },
            }
        },
        {
            38,
            new int[][]
            {
                new int[] { 29, 20, 12 },
                new int[] { 30, 22, 15, 9 },
                new int[] { 39, 40, 41, 42 },
                new int[] { 46, 53, 59 },
                new int[] { 45, 51 },
                new int[] { 37, 36 },
            }
        },
        {
            39,
            new int[][]
            {
                new int[] { 30, 21, 13, 6 },
                new int[] { 31, 23, 16, 10 },
                new int[] { 40, 41, 42, 43 },
                new int[] { 47, 54, 60 },
                new int[] { 46, 52, 57 },
                new int[] { 38, 37, 36 },
            }
        },
        {
            40,
            new int[][]
            {
                new int[] { 31, 22, 14, 7 },
                new int[] { 32, 24, 17, 11 },
                new int[] { 41, 42, 43 },
                new int[] { 48, 55, 61 },
                new int[] { 47, 53, 58 },
                new int[] { 39, 38, 37, 36 },
            }
        },
        {
            41,
            new int[][]
            {
                new int[] { 32, 23, 15, 8 },
                new int[] { 33, 25, 18 },
                new int[] { 42, 43 },
                new int[] { 49, 56 },
                new int[] { 48, 54, 59 },
                new int[] { 40, 39, 38, 37 },
            }
        },
        {
            42,
            new int[][]
            {
                new int[] { 43 },
                new int[] { 50 },
                new int[] { 49, 55, 60 },
                new int[] { 41, 40, 39, 38 },
                new int[] { 33, 24, 16, 9 },
                new int[] { 34, 26 },
            }
        },
        {
            43,
            new int[][]
            {
                new int[] { 35 },
                new int[] { 50, 56, 61 },
                new int[] { 34, 25, 17, 10 },
                new int[] { 42, 41, 40, 39 },
            }
        },
        {
            44,
            new int[][]
            {
                new int[] { 36, 27 },
                new int[] { 37, 29, 21, 14 },
                new int[] { 45, 46, 47, 48 },
                new int[] { 51, 57 },
            }
        },
        {
            45,
            new int[][]
            {
                new int[] { 44 },
                new int[] { 37, 28, 19 },
                new int[] { 38, 30, 22, 15 },
                new int[] { 46, 47, 48, 49 },
                new int[] { 52, 28 },
                new int[] { 51 },
            }
        },
        {
            46,
            new int[][]
            {
                new int[] { 45, 44 },
                new int[] { 38, 29, 20, 12 },
                new int[] { 39, 31, 23, 16 },
                new int[] { 47, 48, 49, 50 },
                new int[] { 53, 59 },
                new int[] { 52, 57 },
            }
        },
        {
            47,
            new int[][]
            {
                new int[] { 46, 45, 44 },
                new int[] { 39, 30, 21, 13 },
                new int[] { 40, 32, 24, 17 },
                new int[] { 48, 49, 50 },
                new int[] { 54, 60 },
                new int[] { 53, 58 },
            }
        },
        {
            48,
            new int[][]
            {
                new int[] { 47, 46, 45, 44 },
                new int[] { 40, 31, 22, 14 },
                new int[] { 41, 33, 25, 18 },
                new int[] { 49, 50 },
                new int[] { 55, 61 },
                new int[] { 54, 59 },
            }
        },
        {
            49,
            new int[][]
            {
                new int[] { 48, 47, 46, 45 },
                new int[] { 41, 32, 23, 15 },
                new int[] { 42, 34, 26 },
                new int[] { 50 },
                new int[] { 56 },
                new int[] { 55, 60 },
            }
        },
        {
            50,
            new int[][]
            {
                new int[] { 43, 35 },
                new int[] { 56, 61 },
                new int[] { 49, 48, 47, 46 },
                new int[] { 42, 33, 24, 16 },
            }
        },
        {
            51,
            new int[][]
            {
                new int[] { 44, 36, 27 },
                new int[] { 45, 38, 30, 22 },
                new int[] { 52, 53, 54, 55 },
                new int[] { 57 },
            }
        },
        {
            52,
            new int[][]
            {
                new int[] { 51 },
                new int[] { 45, 37, 28, 19 },
                new int[] { 46, 39, 31, 23 },
                new int[] { 53, 54, 55, 56 },
                new int[] { 58 },
                new int[] { 57 },
            }
        },
        {
            53,
            new int[][]
            {
                new int[] { 52, 51 },
                new int[] { 46, 38, 29, 20 },
                new int[] { 47, 40, 32, 24 },
                new int[] { 54, 55, 56 },
                new int[] { 59 },
                new int[] { 58 },
            }
        },
        {
            54,
            new int[][]
            {
                new int[] { 53, 52, 51 },
                new int[] { 47, 39, 30, 21 },
                new int[] { 48, 41, 33, 25 },
                new int[] { 55, 56 },
                new int[] { 60 },
                new int[] { 59 },
            }
        },
        {
            55,
            new int[][]
            {
                new int[] { 56 },
                new int[] { 61 },
                new int[] { 60 },
                new int[] { 54, 53, 52, 51 },
                new int[] { 48, 40, 31, 22 },
                new int[] { 49, 42, 34, 26 },
            }
        },
        {
            56,
            new int[][]
            {
                new int[] { 50, 43, 35 },
                new int[] { 61 },
                new int[] { 55, 54, 53, 52 },
                new int[] { 49, 41, 32, 23 },
            }
        },
        {
            57,
            new int[][]
            {
                new int[] { 51, 44, 36, 27 },
                new int[] { 52, 46, 39, 31 },
                new int[] { 58, 59, 60, 61 },
            }
        },
        {
            58,
            new int[][]
            {
                new int[] { 57 },
                new int[] { 52, 45, 37, 28 },
                new int[] { 53, 47, 40, 32 },
                new int[] { 59, 60, 61 },
            }
        },
        {
            59,
            new int[][]
            {
                new int[] { 58, 57 },
                new int[] { 53, 46, 38, 29 },
                new int[] { 54, 48, 41, 33 },
                new int[] { 60, 61 },
            }
        },
        {
            60,
            new int[][]
            {
                new int[] { 59, 58, 57 },
                new int[] { 54, 47, 39, 30 },
                new int[] { 55, 49, 42, 34 },
                new int[] { 61 },
            }
        },
        {
            61,
            new int[][]
            {
                new int[] {60, 59, 58, 57 },
                new int[] { 55, 48, 40, 31 },
                new int[] { 56, 50, 43, 35 },
            }
        },

    };
}
