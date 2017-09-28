using System;
using System.Collections.Generic;
using System.Text;

namespace Gulijiang.KFramework.EasyUIExtensions
{
    public class TreeNode
    {
        private string tid;
        private string ttext;
        private string ticonCls;
        private List<TreeNode> tchildren = new List<TreeNode>();

        public string id
        {
            get { return tid; }
            set { tid = value; }
        }
        public string text
        {
            get { return ttext; }
            set { ttext = value; }
        }
        public string iconCls
        {
            get { return ticonCls; }
            set { ticonCls = value; }
        }
        public List<TreeNode> children
        {
            get { return tchildren; }
            set { tchildren = value; }
        }
    }
}
