using Microsoft.VisualStudio.TestTools.UnitTesting;
using Blazor2048;
using System;
using System.Linq;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTestGame2048
    {

        public static void RunTestCases((int[] initial, int[] moved)[] testCases, Action<Game2048> action)
        {
            foreach (var (initial, moved) in testCases)
            {
                var game = new Game2048() { NoAutoAdd = true };
                for (var i = 0; i < initial.Length; i++) game.Cells[i] = initial[i];
                action(game);
                Assert.IsTrue(Enumerable.SequenceEqual(game.Cells, moved));
            }
        }
        [TestMethod]
        public void TestMethodMoveAdd()
        {
            var game = new Game2048() { NoAutoAdd = true };
            do
            {
                game.Add();
                var empty = game.EmptyCells.ToArray();
                var wrong = empty.Where(x => game.Cells[x] != 0);
                Assert.IsFalse(wrong.Any());
            } while (game.EmptyCells.Any());

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
        public void TestMethodMoveLeft()
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
                        2, 0, 0, 0,
                        2, 0, 0, 0,
                        2, 0, 0, 0,
                        2, 0, 0, 0
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
                        4, 0, 0, 0,
                        4, 0, 0, 0,
                        4, 0, 0, 0,
                        4, 2, 0, 0
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
                        4, 4, 0, 0,
                        4, 2, 0, 0,
                        4, 0, 0, 0
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
                        2, 4, 0, 0,
                        4, 2, 0, 0,
                        8, 0, 0, 0,
                        4, 4, 0, 0
                    }
                )
            };
            RunTestCases(testPatterns, (g) => g.Left()); ;
        }

        [TestMethod]
        public void TestMethodMoveDown()
        {
            var testPatterns = new (int[] initial, int[] moved)[] {
                (
                    new int[] {
                        0, 2, 0, 0,
                        0, 0, 2, 0,
                        0, 0, 0, 2,
                        0, 0, 0, 0
                    },
                    new int[] {
                        0, 0, 0, 0,
                        0, 0, 0, 0,
                        0, 0, 0, 0,
                        0, 2, 2, 2
                    }
                )
                ,
                (
                    new int[] {
                        0, 2, 2, 2,
                        0, 2, 0, 0,
                        0, 0, 2, 0,
                        2, 0, 0, 2
                    },
                    new int[] {
                        0, 0, 0, 0,
                        0, 0, 0, 0,
                        0, 0, 0, 0,
                        2, 4, 4, 4
                    }
                )
                ,
                (
                    new int[] {
                        2, 2, 2, 2,
                        2, 2, 2, 4,
                        2, 0, 2, 0,
                        0, 2, 2, 0
                    },
                    new int[] {
                        0, 0, 0, 0,
                        0, 0, 0, 0,
                        2, 2, 4, 2,
                        4, 4, 4, 4
                    }
                )
                ,
                (
                    new int[] {
                        2, 2, 2, 2,
                        0, 0, 2, 2,
                        4, 0, 4, 0,
                        0, 4, 0, 4
                    },
                    new int[] {
                        0, 0, 0, 0,
                        0, 0, 0, 0,
                        2, 2, 4, 4,
                        4, 4, 4, 4
                    }
                )

            };
            RunTestCases(testPatterns, (g)=>g.Down());
        }

        [TestMethod]
        public void TestMethodMoveUp()
        {
            var testPatterns = new (int[] initial, int[] moved)[] {
                (
                    new int[] {
                        0, 2, 0, 0,
                        0, 0, 2, 0,
                        0, 0, 0, 2,
                        0, 0, 0, 0
                    },
                    new int[] {
                        0, 2, 2, 2,
                        0, 0, 0, 0,
                        0, 0, 0, 0,
                        0, 0, 0, 0
                    }
                )
                ,
                (
                    new int[] {
                        0, 2, 2, 2,
                        0, 2, 0, 0,
                        0, 0, 2, 0,
                        2, 0, 0, 2
                    },
                    new int[] {
                        2, 4, 4, 4,
                        0, 0, 0, 0,
                        0, 0, 0, 0,
                        0, 0, 0, 0
                    }
                )
                ,
                (
                    new int[] {
                        2, 2, 2, 2,
                        2, 2, 2, 4,
                        2, 0, 2, 0,
                        0, 2, 2, 0
                    },
                    new int[] {
                        4, 4, 4, 2,
                        2, 2, 4, 4,
                        0, 0, 0, 0,
                        0, 0, 0, 0
                    }
                )
                ,
                (
                    new int[] {
                        2, 2, 2, 2,
                        0, 0, 2, 2,
                        4, 0, 4, 0,
                        0, 4, 0, 4
                    },
                    new int[] {
                        2, 2, 8, 8,
                        4, 4, 0, 0,
                        0, 0, 0, 0,
                        0, 0, 0, 0
                    }
                )

            };
            RunTestCases(testPatterns, (g) => g.Up());
        }
    }
}
