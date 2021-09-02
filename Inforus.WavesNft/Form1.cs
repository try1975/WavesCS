using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using WavesCS;

namespace Inforus.WavesNft
{
    public partial class Form1 : Form
    {
        private char NetChainId
        {
            get
            {
                var text = cmbNodeChainId.Text;
                if (string.IsNullOrEmpty(text) || text.StartsWith("Test")) return Node.TestNetChainId;
                return Node.MainNetChainId;
            }
        }
        private PrivateKeyAccount account;
        private Node node;
        private readonly List<Asset> NftList;
        private readonly BindingSource Source;

        public PrivateKeyAccount Account
        {
            get => account;
            set
            {
                account = value;
                tbAdress.Text = account?.Address;
                NftList.Clear();
                linkLabel1.Text = $"https://{(Node.TestNetChainId == NetChainId ? "testnet." : "")}wavesexplorer.com/address/{account?.Address}/nft";
            }
        }
        public Node Node
        {
            get
            {
                if (node == null || node.ChainId != NetChainId)
                {
                    node = new Node(NetChainId);
                }
                return node;
            }
        }

        public Form1()
        {
            Source = new BindingSource();
            NftList = new List<Asset>();
            Source.DataSource = NftList;

            InitializeComponent();
            dgvNft.DataSource = Source;
            Http.Tracing = true;
            cmbNodeChainId.SelectedIndex = 0;
            btnAccountCreate.Click += BtnAccountCreate_Click;
            button2.Click += Button2_Click;
            btnNewNft.Click += BtnNewNft_Click;
            btnBurnNft.Click += BtnBurnNft_Click;
            btnTransferNft.Click += BtnTransferNft_Click;
            linkLabel1.Click += LinkLabel1_Click;
        }

        private PrivateKeyAccount AccountCreate(string accountKey, bool useSeed)
        {
            if (useSeed) return PrivateKeyAccount.CreateFromSeed(accountKey, NetChainId);
            return PrivateKeyAccount.CreateFromPrivateKey(accountKey, NetChainId);
        }

        private void UpdateAccountBalance()
        {
            var accountBalance = Node.GetBalance(Account.Address);
            tbAccountBalance.Text = $"{accountBalance}";
        }

        private void RefreshNft()
        {
            if (Account == null) return;
            NftList.Clear();
            var objects = Node.GetObjects("assets/nft/{0}/limit/100", Account.Address);
            foreach (var obj in objects)
            {
                var assetId = obj.FirstOrDefault(_ => _.Key.Equals("assetId")).Value.ToString();
                var asset = Node.GetAsset(assetId);
                NftList.Add(asset);
            }
            Source.ResetBindings(metadataChanged: false);
        }

        private void BtnAccountCreate_Click(object sender, EventArgs e)
        {
            dgvNft.DataSource = null;
            var accountKey = tbAccountKey.Text;
            Account = AccountCreate(accountKey, rbSeed.Checked);
            dgvNft.DataSource = Source;
            UpdateAccountBalance();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            RefreshNft();
        }

        private void BtnNewNft_Click(object sender, EventArgs e)
        {
            Asset asset = Node.IssueAsset(Account, tbNftName.Text, tbNftDesciption.Text, 1, 0, false, null, 0.001m);
            if (asset != null)
            {
                tbNftId.Text = asset.Id;
                Node.WaitTransactionConfirmation(asset.Id);
            }

            RefreshNft();
        }

        private void BtnBurnNft_Click(object sender, EventArgs e)
        {
            var assetId = tbNftId.Text;
            var asset = Node.GetAsset(assetId);
            var response = node.BurnAsset(Account, asset, 1);
            node.WaitTransactionConfirmationByResponse(response);

            RefreshNft();
        }

        private void BtnTransferNft_Click(object sender, EventArgs e)
        {
            if (Source.Current is Asset asset)
            {
                Node.Transfer(Account, tbTransferAccount.Text, asset, 1, "Shut up & take my NFT");
                RefreshNft();
            }

        }

        private void LinkLabel1_Click(object sender, EventArgs e)
        {
            var url = ((LinkLabel)sender).Text;
            Process.Start(url);
        }
        
        private void DgvNft_SelectionChanged(object sender, EventArgs e)
        {
            if (Source.Current is Asset asset)
            {
                tbNftId.Text = asset.Id;
                tbNftName.Text = asset.Name;
                tbNftDesciption.Text = asset.Description;
            }
        }
    }
}
