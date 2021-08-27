using System;
using System.IO;

namespace TestTask
{
    class ListNode
    {
        public ListNode Previous;
        public ListNode Next;
        public ListNode Random;
        public string Data;
    }

    class ListRandom
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count;

        private const char SEPARATOR = ';';
        private enum NodeInfo { INDEXOFNODE, NODEDATA, NEXTNODE, PREVNODE, RANDNODE };

        public override bool Equals(object obj)
        {
            return obj is ListRandom lr && Equals(lr);
        }

        public bool Equals(ListRandom lr)
        {
            if (this.Count != lr.Count)
                return false;

            bool[,] AdjacencyMatrixOriginal = this.BuildAdjacencyMatrix();
            bool[,] AdjacencyMatrixCopy = lr.BuildAdjacencyMatrix();

            for (int i = 0; i < AdjacencyMatrixOriginal.GetLength(0); i++)
            {
                for (int j = 0; j < AdjacencyMatrixOriginal.GetLength(0); j++)
                {
                    if (AdjacencyMatrixOriginal[i, j] != AdjacencyMatrixCopy[i, j])
                        return false;
                }
            }

            ListNode curNodeOriginal = Head;
            ListNode curNodeCopy = lr.Head;
            while (curNodeOriginal != null && curNodeCopy != null)
            {
                if (curNodeOriginal.Data != curNodeCopy.Data)
                    return false;

                curNodeOriginal = curNodeOriginal.Next;
                curNodeCopy = curNodeCopy.Next;
            }

            return true;
        }

        public bool[,] BuildAdjacencyMatrix()
        {
            bool[,] AdjacencyMatrix = new bool[this.Count, this.Count];
            ListNode curNode = Head;
            while (curNode != null)
            {
                int curNodeIndex = this.IndexOf(curNode);
                if (curNode.Previous != null)
                {
                    AdjacencyMatrix[curNodeIndex, this.IndexOf(curNode.Previous)] = true;
                }
                if (curNode.Next != null)
                {
                    AdjacencyMatrix[curNodeIndex, this.IndexOf(curNode.Next)] = true;
                }
                if (curNode.Random != null)
                {
                    AdjacencyMatrix[curNodeIndex, this.IndexOf(curNode.Random)] = true;
                }
                curNode = curNode.Next;
            }

            return AdjacencyMatrix;
        }

        public void Serialize(Stream s)
        {
            System.Text.StringBuilder pickle = new System.Text.StringBuilder();
            ListNode currNode = Head;
            while (currNode != null)
            {
                pickle.Append(this.IndexOf(currNode)).Append(SEPARATOR);
                pickle.Append(currNode.Data).Append(SEPARATOR);
                pickle.Append(this.IndexOf(currNode.Next)).Append(SEPARATOR);
                pickle.Append(this.IndexOf(currNode.Previous)).Append(SEPARATOR);
                pickle.Append(this.IndexOf(currNode.Random));
                if (currNode.Next != null)
                    pickle.AppendLine();
                currNode = currNode.Next;
            }

            char[] charArray = pickle.ToString().ToCharArray();
            byte[] byteArray = new byte[charArray.Length];
            for (int i = 0; i < charArray.Length; ++i)
            {
                byteArray[i] = Convert.ToByte(charArray[i]);
            }

            s.Write(byteArray);
        }

        public void Deserialize(Stream s)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            while (true)
            {
                int value = s.ReadByte();
                if (value < 0)
                    break;
                sb.Append(Convert.ToChar(value));
            }

            string pickle = sb.ToString();
            string[] ListStr = pickle.Split('\n');
            ListNode[] ListNodeArray = new ListNode[ListStr.Length];
            for (int indexOfNode = 0; indexOfNode < ListNodeArray.Length; ++indexOfNode)
            {
                ListNodeArray[indexOfNode] = new ListNode();
            }

            foreach (string nodeStr in ListStr)
            {
                var nodeStrArray = nodeStr.Split(SEPARATOR);
                int indexOfNode = int.Parse(nodeStrArray[(int)NodeInfo.INDEXOFNODE]);

                string data = (nodeStrArray[(int)NodeInfo.NODEDATA]);
                ListNodeArray[indexOfNode].Data = data;

                int indexOfNextNode = int.Parse(nodeStrArray[(int)NodeInfo.NEXTNODE]);
                ListNodeArray[indexOfNode].Next = indexOfNextNode >= 0 ? ListNodeArray[indexOfNextNode] : null;

                int indexOfPrevNode = int.Parse(nodeStrArray[(int)NodeInfo.PREVNODE]);
                ListNodeArray[indexOfNode].Previous = indexOfPrevNode >= 0 ? ListNodeArray[indexOfPrevNode] : null;

                int indexOfRandNode = int.Parse(nodeStrArray[(int)NodeInfo.RANDNODE]);
                ListNodeArray[indexOfNode].Random = indexOfRandNode >= 0 ? ListNodeArray[indexOfRandNode] : null;
            }

            this.Head = ListNodeArray[0];
            this.Tail = ListNodeArray[^1];
            this.Count = ListNodeArray.Length;
        }

        public int IndexOf(ListNode node)
        {
            ListNode currNode = Head;
            int index = 0;
            while (currNode != null)
            {
                if (node == currNode)
                {
                    return index;
                }
                ++index;
                currNode = currNode.Next;
            }

            return -1;
        }
    }
}