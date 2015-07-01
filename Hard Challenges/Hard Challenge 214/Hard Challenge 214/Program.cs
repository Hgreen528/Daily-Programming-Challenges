//Author: Hunter Green
//Created: 6/27/2015
//This contains a failed attempt to implement a kd-tree
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Hard_Challenge_214
{
    class Program
    {
        static void Main(string[] args)
        {
            Vector2 ChesterPos;
            ChesterPos.x = 0.5;
            ChesterPos.y = 0.5;

            kd_tree positions = new kd_tree();

            //Console.WriteLine("Enter the number of treats that have magically appeared: ");
            //int treatcount = Int32.Parse(Console.ReadLine());

            //Console.WriteLine("Enter the treat positions with dimensions seperated by a space: ");
            //string[] inputs;
            //for (int i = 0; i < treatcount; i++)
            //{
            //    inputs = Console.ReadLine().Split(' ');
            //    Vector2 treat;
            //    treat.x = double.Parse(inputs[0]);
            //    treat.y = double.Parse(inputs[1]);
            //    positions.AddPosition(treat);
            //}

            string[] input = File.ReadAllLines("input2.txt");

            int treatCount = Int32.Parse(input[0]);
            List<Vector2> treats = new List<Vector2>();
            for(int i = 1; i <= treatCount; i++)
            {
                string[] position = input[i].Split(' ');
                Vector2 v;
                v.x = double.Parse(position[0]);
                v.y = double.Parse(position[1]);
                treats.Add(v);
            }

            positions.Initialize(treats);

            double totalDistance = 0;
            Vector2 nextPosition;
            while(positions.Count > 0)
            {
                positions.PrintTree();
                nextPosition = positions.GetClosestPoint(ChesterPos);
                totalDistance += Distance(nextPosition, ChesterPos);
                ChesterPos = nextPosition;
            }

            Console.WriteLine(totalDistance);

            Console.ReadLine();
        }

        public static double Distance(Vector2 a, Vector2 b)
        {
            return Math.Sqrt((b.x - a.x) * (b.x - a.x) + (b.y - a.y) * (b.y - a.y));
        }
    }

    struct Vector2
    {
        public double x;
        public double y;

        public static bool operator !=(Vector2 a, Vector2 b)
        {
            if (a.x != b.x || a.y != b.y)
                return true;
            else
                return false;
        }

        public static bool operator ==(Vector2 a, Vector2 b)
        {
            if (a.x == b.x && a.y == b.y)
                return true;
            else
                return false;
        }

        public static int CompareByX(Vector2 a, Vector2 b)
        {
            if (a.x > b.x)
                return 1;
            else
                return -1;
        }

        public static int CompareByY(Vector2 a, Vector2 b)
        {
            if (a.y > b.y)
                return 1;
            else
                return -1;
        }
    }

    class kd_tree
    {
        node root;

        int count;

        public int Count
        {
            get { return count; }
        }

        public kd_tree()
        {
            count = 0;
        }

        //Add a new position into the tree
        public void AddPosition(Vector2 pos)
        {
            if(root == null)
            {
                root = new node(pos);
                count++;
                return;
            }

            //Find where to put the new node
            node current = root;
            node next;

            if (pos.x > root.Position.x)
                next = root.Right;
            else
                next = root.Left;

            int dimension = 0;
            while(next != null)
            {
                //Cycle the dimension checked
                if (dimension == 0)
                    dimension = 1;
                else
                    dimension = 0;

                //Proceed to the next node
                current = next;
                if (dimension == 0)
                {
                    if(pos.x > current.Position.x)
                    {
                        next = current.Right;
                    }
                    else
                    {
                        next = current.Left;
                    }
                }
                else
                {
                    if (pos.y > current.Position.y)
                    {
                        next = current.Right;
                    }
                    else
                    {
                        next = current.Left;
                    }
                }
            }

            //Add the new node
            if (dimension == 0)
            {
                if (pos.x > current.Position.x)
                {
                    current.Right = new node(pos);
                }
                else
                {
                    current.Left = new node(pos);
                }
            }
            else
            {
                if (pos.y > current.Position.y)
                {
                    current.Right = new node(pos);
                }
                else
                {
                    current.Left = new node(pos);
                }
            }

            count++;
        }

        public void Initialize(List<Vector2> positions)
        {
            //First find the median of the positions to set as the root
            List<Vector2> positionsSortedByX = new List<Vector2>();
            List<Vector2> positionsSortedByY = new List<Vector2>();
            positions.Sort(Vector2.CompareByX);
            for(int i = 0; i < positions.Count; i++)
            {
                positionsSortedByX.Add(positions[i]);
            }
            positions.Sort(Vector2.CompareByY);
            for (int i = 0; i < positions.Count; i++)
            {
                positionsSortedByY.Add(positions[i]);
            }

            int dimension = 0;
            for (int i = 0; i < positions.Count; i++)
            {
                if (dimension == 0)
                {
                    AddPosition(positionsSortedByX[positionsSortedByX.Count/2]);
                    positionsSortedByY.Remove(positionsSortedByX[positionsSortedByX.Count / 2]);
                    positionsSortedByX.RemoveAt(positionsSortedByX.Count/2);
                }
                else
                {
                    AddPosition(positionsSortedByX[positionsSortedByY.Count/2]);
                    positionsSortedByX.Remove(positionsSortedByY[positionsSortedByX.Count / 2]);
                    positionsSortedByY.RemoveAt(positionsSortedByY.Count / 2);
                }
            }
        }

        //Get the closest point to the given position and removes that point from the tree
        public Vector2 GetClosestPoint(Vector2 pos)
        {
            Vector2 closestPoint = GetClosestPoint(pos, root.Position, root, 0);
            Remove(closestPoint);
            return closestPoint;
        }

        public Vector2 GetClosestPoint(Vector2 pos, Vector2 currentBest, node current, int dimension)
        {
            //recurse down the tree to a leaf node, save that leaf as the current best
            if (dimension == 0)
            {
                if (pos.x > current.Position.x)
                {
                    if (current.Right != null)
                        currentBest = GetClosestPoint(pos, currentBest, current.Right, 1);
                    else
                        return current.Position;
                }
                else
                {
                    if (current.Left != null)
                        currentBest = GetClosestPoint(pos, currentBest, current.Left, 1);
                    else
                        return current.Position;
                }
            }
            else
            {
                if (pos.y > current.Position.y)
                {
                    if (current.Right != null)
                        currentBest = GetClosestPoint(pos, currentBest, current.Right, 1);
                    else
                        return current.Position;
                }
                else
                {
                    if (current.Left != null)
                        currentBest = GetClosestPoint(pos, currentBest, current.Left, 1);
                    else
                        return current.Position;
                }
            }

            //While recursing back up...
            //check if the current node is closer than the current best
            if (Distance(pos, currentBest) > Distance(pos, current.Position))
                currentBest = current.Position;

            //See if there are any points on the other path which may be closer
            if(dimension == 0)
            {
                if(Math.Abs(pos.x - current.Position.x) < Distance(pos, currentBest))
                {
                    Vector2 potentialBest = new Vector2();
                    if (pos.x > current.Position.x)
                    {
                        if(current.Left != null)
                            potentialBest = GetClosestPoint(pos, currentBest, current.Left, 1);
                    }
                    else
                    {
                        if(current.Right != null)
                            potentialBest = GetClosestPoint(pos, currentBest, current.Right, 1);
                    }

                    if (Distance(potentialBest, pos) < Distance(currentBest, pos))
                        currentBest = potentialBest;
                }
            }
            else
            {
                if (Math.Abs(pos.y - current.Position.y) < Distance(pos, currentBest))
                {
                    Vector2 potentialBest = new Vector2();
                    if (pos.x > current.Position.y)
                    {
                        if (current.Left != null)
                            potentialBest = GetClosestPoint(pos, currentBest, current.Left, 0);
                    }
                    else
                    {
                        if (current.Right != null)
                            potentialBest = GetClosestPoint(pos, currentBest, current.Right, 0);
                    }

                    if (Distance(potentialBest, pos) < Distance(currentBest, pos))
                        currentBest = potentialBest;
                }
            }

            return currentBest;
        }

        void Remove(Vector2 pos)
        {
            //Find the one to remove
            node parent = root;
            node current = root;
            int dimension = 0;
            while(current.Position != pos)
            {
                parent = current;
                if (dimension == 0)
                {
                    if (pos.x > current.Position.x)
                    {
                        current = current.Right;
                    }
                    else
                    {
                        current = current.Left;
                    }
                }
                else
                {
                    if (pos.y > current.Position.y)
                    {
                        current = current.Right;
                    }
                    else
                    {
                        current = current.Left;
                    }
                }

                if (dimension == 0)
                    dimension = 1;
                else
                    dimension = 0;
            }

            //Create a list of all of its children
            List<Vector2> children = getChildren(current);

            //Remove all of its children
            current.Right = null;
            current.Left = null;

            //Delete the node
            if (parent.Left != null && parent.Left.Position == current.Position)
                parent.Left = null;
            else if (parent.Right != null && parent.Right.Position == current.Position)
                parent.Right = null;
            else if (current.Position == root.Position)
                root = null;

            current = null;

            //Add the list of children back in
            foreach(Vector2 v in children)
            {
                count--;
                AddPosition(v);
            }

            count--;
        }

        List<Vector2> getChildren(node parent)
        {
            List<Vector2> children = new List<Vector2>();

            List<node> closed = new List<node>();
            List<node> open = new List<node>();


            if (parent.Left != null)
                open.Add(parent.Left);
            if (parent.Right != null)
                open.Add(parent.Right);

            while(open.Count != 0)
            {
                for(int i = open.Count-1; i >= 0; i--)
                {
                    if (open[i].Left != null)
                        open.Add(open[i].Left);
                    if (open[i].Right != null)
                        open.Add(open[i].Right);

                    closed.Add(open[i]);
                    open.Remove(open[i]);
                }
            }

            foreach (node n in closed)
            {
                children.Add(n.Position);
            }

            return children;
        }

        double Distance(Vector2 a, Vector2 b)
        {
            return Math.Sqrt((b.x - a.x) * (b.x - a.x) + (b.y - a.y) * (b.y - a.y));
        }

        public void PrintTree()
        {
            Console.WriteLine("Tree: ");
            if (count == 0)
            {
                Console.Write("Empty");
                return;
            }

            PrintTree(root);
        }

        void PrintTree(node n)
        {
            Console.WriteLine("Node = " + n.Position.x + ", " + n.Position.y);
            if (n.Left != null)
            {
                Console.WriteLine("Left = " + n.Left.Position.x + ", " + n.Left.Position.y);
            }
            if (n.Right != null)
            {
                Console.WriteLine("Right = " + n.Right.Position.x + ", " + n.Right.Position.y);
            }

            if (n.Left != null)
            {
                PrintTree(n.Left);
            }
            if (n.Right != null)
            {
                PrintTree(n.Right);
            }
        }
    }

    class node
    {
        node left;
        node right;
        Vector2 position;

        public node() { }

        public node(Vector2 pos)
        {
            position = pos;
        }

        public node Left
        {
            get { return left; }
            set { left = value; }
        }

        public node Right
        {
            get { return right; }
            set { right = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
    }
}
