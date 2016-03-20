using JobSkillsDb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPGMiner.Handler
{
    public class FPGTree
    {
        public FPGTreeNode Root { get; set; }

        public FPGTree()
        {
            Root = new FPGTreeNode(-1);
        }

        public void Add(SimplifiedVacancy transaction)
        {
            FPGTreeNode curNode = Root;
            List<int> list = transaction.SkillsIds.ToList();
            AddNode(curNode, list);
        }
        
        private void AddNode(FPGTreeNode curNode, List<int> skillsIds)
        {
            FPGTreeNode node = curNode.ChildNodes
                .SingleOrDefault(cn => cn.SkillId == skillsIds.First());
            if (node == null)
            {
                node = new FPGTreeNode()
                {
                    ParentNode = curNode,
                    SkillId = skillsIds.First()
                };
                curNode.ChildNodes.Add(node);
            }
            node.FrequencyCount++;
            skillsIds.RemoveAt(0);
            if (skillsIds.Count > 0)
            {
                AddNode(node, skillsIds);
            }
        }

        public List<List<int>> GetAssociations(int threshold, int skillId = -1)
        {
            List<List<int>> associations = new List<List<int>>();
            FPGTreeNode root = Root;
            if (skillId < 0)
            {
                
            }
            AddNodeContentToLists(root, threshold, new List<int>(), associations);
            return associations;
        }
        //private FPGTree GetConditionalBaseTree(int skillId)
        //{

        //}
        private void AddNodeContentToLists(FPGTreeNode node, int threshold, List<int> nodeList, List<List<int>> associations)
        {
            nodeList.Add(node.SkillId);
            foreach(FPGTreeNode child in node.ChildNodes)
            {
                AddNodeContentToLists(child, threshold, nodeList, associations);
            }
            if (node.ChildNodes.Count == 0)
            {
                associations.Add(nodeList);
            }
        }
    }
}
