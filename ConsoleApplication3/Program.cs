using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApplication3
{
    class Solution
    {
        class Node
        {
            public int rightSplitIdx;
            public bool keepLeft;
            public int[] A;
            public List<Node> children;

            public int GetMaxDepth(int level = 1)
            {
                //Console.WriteLine($"{this.ToString()} {level}");

                int maxDepth = level;
                foreach (var item in children)
                {
                    int depth = item.GetMaxDepth(level + 1);
                    if (depth > maxDepth) maxDepth = depth;
                }

                return maxDepth;
            }

            public Node(int[] A, bool keepLeft, int rightSplitIdx)
            {
                this.A = A;
                this.keepLeft = keepLeft;
                this.rightSplitIdx = rightSplitIdx;
                children = new List<Node>();
            }

            public override string ToString()
            {
                string array = string.Empty;
                foreach (var item in A)
                {
                    array += string.Format($"{item} ");
                }
                string lr = keepLeft ? "LEFT" : "RIGHT";

                return string.Format($"{array}/{rightSplitIdx} {lr}");
            }
        }

        static void Main(String[] args)
        {
            /* Enter your code here. Read input from STDIN. Print output to STDOUT. Your class should be named Solution */
            string numTestCases = System.Console.ReadLine().Trim();
            List<int> result = new List<int>();

            for (int i = 0; i < int.Parse(numTestCases);i++)
            {
                int numArrayElements = int.Parse(Console.ReadLine().Trim());
                int[] A = Console.ReadLine().Trim().Split(new char[] { ' ' }).Select(x => int.Parse(x)).ToArray();

                List<Node> rootPossibles = GetPossibleSplits(A);

                if (rootPossibles.Count == 0)
                {
                    result.Add(0);
                }
                else
                {
                    result.Add(rootPossibles.Max(x => x.GetMaxDepth()));
                }
            }

            foreach (var item in result)
            {
                Console.WriteLine(item);
            }

            //Console.ReadLine();
        }

        private static List<Node> GetPossibleSplits(int[] A)
        {
            List<Node> posibilities = new List<Node>();
            for (int j = 1; j < A.Length; j++)
            {
                int leftSum = 0;
                int rightSum = 0;
                for (int k = 0; k < j; k++)
                {
                    leftSum += A[k];
                }

                for (int k = j; k < A.Length; k++)
                {
                    rightSum += A[k];
                }

                if (leftSum == rightSum)
                {
                    //it is a candidate split
                    Node nodeKeepLeft = new Node(A, true, j);
                    posibilities.Add(nodeKeepLeft);
                    Node nodeKeepRight = new Node(A, false, j);
                    posibilities.Add(nodeKeepRight);
                }
            }

            //generate children
            foreach (Node possibility in posibilities)
            {
                List<int> newList = new List<int>();

                if (possibility.keepLeft)
                {
                    for(int i=0;i<possibility.rightSplitIdx;i++)
                    {
                        newList.Add(possibility.A[i]);
                    }

                    possibility.children.AddRange(GetPossibleSplits(newList.ToArray()));
                }
                else
                {
                    for (int i = possibility.rightSplitIdx; i < possibility.A.Length; i++)
                    {
                        newList.Add(possibility.A[i]);
                    }

                    possibility.children.AddRange(GetPossibleSplits(newList.ToArray()));
                }
            }

            return posibilities;
        }
    }
}
