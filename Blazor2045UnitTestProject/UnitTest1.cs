using Microsoft.VisualStudio.TestTools.UnitTesting;
using Blazor2048;
using System;
using System.Linq;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {

        public void RunTestCases((int[] initial, int[] moved)[] testCases, Action<Game2048> action)
        {
            var game = new Game2048() { NoAutoAdd = true };
            foreach (var testCase in testCases)
            {
                game.Cells = testCase.initial;
                action(game);
                Assert.IsTrue(Enumerable.SequenceEqual(game.Cells, testCase.moved));
            }
        }

        [TestMethod]
        public void TestMethodMoveRight()
        {
            var testPatterns = new (int[] initial, int[] moved)[] {
                (
                    new int[] {
                        2, 0, 0, 0,
                        0, 2, 0, 0,
                        0, 0, 2, 0,
                        0, 0, 0, 2
                    },
                    new int[] {
                        0, 0, 0, 2,
                        0, 0, 0, 2,
                        0, 0, 0, 2,
                        0, 0, 0, 2
                    }
                ),
                (
                    new int[] {
                        2, 2, 0, 0,
                        2, 0, 2, 0,
                        2, 0, 0, 2,
                        2, 2, 2, 0
                    },
                    new int[] {
                        0, 0, 0, 4,
                        0, 0, 0, 4,
                        0, 0, 0, 4,
                        0, 0, 2, 4
                    }
                )
                ,
                (
                    new int[] {
                        0, 0, 0, 0,
                        2, 2, 2, 2,
                        2, 2, 0, 2,
                        0, 2, 2, 0
                    },
                    new int[] {
                        0, 0, 0, 0,
                        0, 0, 4, 4,
                        0, 0, 2, 4,
                        0, 0, 0, 4
                    }
                )
                ,
                (
                    new int[] {
                        2, 4, 0, 0,
                        4, 2, 0, 0,
                        2, 2, 4, 0,
                        4, 0, 2, 2
                    },
                    new int[] {
                        0, 0, 2, 4,
                        0, 0, 4, 2,
                        0, 0, 4, 4,
                        0, 0, 0, 8
                    }
                )
            };
            RunTestCases(testPatterns, (g) => g.Right()); ;
        }
        [TestMethod]
        public void TestMethodMoveDown()
        {
            var testPatterns = new (int[] initial, int[] moved)[] {
                (
                    new int[] {
                        2, 0, 0, 0,
                        0, 2, 0, 0,
                        0, 0, 2, 0,
                        0, 0, 0, 2
                    },
                    new int[] {
                        0, 0, 0, 2,
                        0, 0, 0, 2,
                        0, 0, 0, 2,
                        0, 0, 0, 2
                    }
                ),
                (
                    new int[] {
                        2, 2, 0, 0,
                        2, 0, 2, 0,
                        2, 0, 0, 2,
                        2, 2, 2, 0
                    },
                    new int[] {
                        0, 0, 0, 4,
                        0, 0, 0, 4,
                        0, 0, 0, 4,
                        0, 0, 2, 4
                    }
                )
                ,
                (
                    new int[] {
                        0, 0, 0, 0,
                        2, 2, 2, 2,
                        2, 2, 0, 2,
                        0, 2, 2, 0
                    },
                    new int[] {
                        0, 0, 0, 0,
                        0, 0, 4, 4,
                        0, 0, 2, 4,
                        0, 0, 0, 4
                    }
                )
                ,
                (
                    new int[] {
                        2, 4, 0, 0,
                        4, 2, 0, 0,
                        2, 2, 4, 0,
                        4, 0, 2, 2
                    },
                    new int[] {
                        0, 0, 2, 4,
                        0, 0, 4, 2,
                        0, 0, 4, 4,
                        0, 0, 0, 8
                    }
                )
            };
            RunTestCases(testPatterns, (g)=>g.Down());
        }
    }
}
