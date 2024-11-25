using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BingoBingoSimulatorApp {
    public partial class Form1 : Form {
        Label[] hitArray; 
        Label[] pickArray;
        Random rand;
        HashSet<int> hittingNum19;
        List<List<int>> alreadyBet;
        List<int> currPick;
        int superNum;
        int[][] winningArray;
        int[] winningCountBound;
        Dictionary<int, int> superNumBet;
        List<int> superBetKeyList;
        int betLarge = 0, betSmall = 0;
        int betOdd = 0, betEven = 0;
        public Form1() {
            InitializeComponent();
            // basic game variables
            hitArray = new Label[20];
            hitArray[0] = hit0;
            hitArray[1] = hit1;
            hitArray[2] = hit2;
            hitArray[3] = hit3;
            hitArray[4] = hit4;
            hitArray[5] = hit5;
            hitArray[6] = hit6;
            hitArray[7] = hit7;
            hitArray[8] = hit8;
            hitArray[9] = hit9;
            hitArray[10] = hit10;
            hitArray[11] = hit11;
            hitArray[12] = hit12;
            hitArray[13] = hit13;
            hitArray[14] = hit14;
            hitArray[15] = hit15;
            hitArray[16] = hit16;
            hitArray[17] = hit17;
            hitArray[18] = hit18;
            hitArray[19] = hit19;
            pickArray = new Label[10];
            pickArray[0] = pick0;
            pickArray[1] = pick1;
            pickArray[2] = pick2;
            pickArray[3] = pick3;
            pickArray[4] = pick4;
            pickArray[5] = pick5;
            pickArray[6] = pick6;
            pickArray[7] = pick7;
            pickArray[8] = pick8;
            pickArray[9] = pick9;
            rand = new Random();
            hittingNum19 = new HashSet<int>();
            superNum = 0;
            alreadyBet = new List<List<int>>();
            currPick = new List<int>();
            // SuperNumber game variables
            superNumBet = new Dictionary<int, int>();
            superBetKeyList = new List<int>();
            // basic game winning rate
            winningArray = new int[11][];
            winningArray[0] = new int[1]; // index 0 will not be used
            winningArray[1] = new int[] { 0, 2 }; 
            winningArray[2] = new int[] { 0, 1, 3 };
            winningArray[3] = new int[] { 0, 0, 2, 20 }; 
            winningArray[4] = new int[] { 0, 0, 1, 4, 40 };
            winningArray[5] = new int[] { 0, 0, 0, 2, 20, 300 };
            winningArray[6] = new int[] { 0, 0, 0, 1, 8, 40, 1000 };
            winningArray[7] = new int[] { 0, 0, 0, 1, 2, 12, 120, 3200 };
            winningArray[8] = new int[] { 1, 0, 0, 0, 1, 8, 40, 800, 20000 };
            winningArray[9] = new int[] { 1, 0, 0, 0, 1, 4, 20, 120, 4000, 40000 };
            winningArray[10] = new int[] { 1, 0, 0, 0, 0, 1, 10, 100, 1000, 10000, 200000 };
            winningCountBound = new int[] { 0, 1, 1, 2, 2, 3, 3, 3, 4, 4, 5 };
        }

        private void Form1_Load(object sender, EventArgs e) {
        }

        private void BtnAddManual_Click(object sender, EventArgs e) {
            if (currPick.Count >= 10) {
                MessageBox.Show("目前選號已達10個");
                txtAddManual.Clear();
                return;
            }
            try {
                int newNum = Convert.ToInt32(txtAddManual.Text);
                if (!(newNum <= 80 && newNum >= 1)) {
                    MessageBox.Show("請輸入1~80中的整數", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                } 
                else if (currPick.Contains(newNum)) {
                    MessageBox.Show("此數字已經選擇過了", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                } 
                else {
                    pickArray[currPick.Count].Text = txtAddManual.Text;
                    currPick.Add(newNum);
                }
            } 
            catch {
                MessageBox.Show("請輸入1~80中的整數", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            txtAddManual.Clear();
        }

        private void BtnClearOne_Click(object sender, EventArgs e) {
            if (currPick.Count <= 0) {
                MessageBox.Show("目前尚未選號");
                return;
            }
            pickArray[currPick.Count - 1].Text = "";
            currPick.RemoveAt(currPick.Count - 1);
        }

        private void BtnClearAll_Click(object sender, EventArgs e) {
            if (currPick.Count == 0) {
                MessageBox.Show("目前尚未選號");
                return;
            }
            foreach (Label lbl in pickArray) {
                lbl.Text = "";
            }
            currPick.Clear();
        }

        private void BtnAddAuto_Click(object sender, EventArgs e) {
            try {
                int upperBound = Convert.ToInt32(txtAddAuto.Text);
                if (!(upperBound >= 0 && upperBound <= 10)) {
                    MessageBox.Show("請輸入號碼數量上限，範圍為1~10之間的整數", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else if (upperBound <= currPick.Count) {
                    MessageBox.Show("目前已選數字已達號碼上限", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else {
                    while (currPick.Count < upperBound) {
                        int newNum = rand.Next(1, 81);
                        if (!currPick.Contains(newNum)) {
                            pickArray[currPick.Count].Text = newNum.ToString();
                            currPick.Add(newNum);
                        }
                    }
                }
            }
            catch {
                MessageBox.Show("請輸入號碼數量上限，範圍為1~10之間的整數", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            txtAddAuto.Clear();
        }

        private void BtnBet_Click(object sender, EventArgs e) {
            if (currPick.Count == 0) {
                MessageBox.Show("目前尚未選號");
                return;
            }
            currPick.Sort();
            alreadyBet.Add(new List<int>(currPick));
            string newBet = "[";
            for (int i = 0; i < currPick.Count; i++) {
                if (i != 0) {
                    newBet += ",";
                }
                newBet += $"{currPick[i]}";
            }
            newBet += "]";
            BtnClearAll_Click(sender, e); // this method will clear currPick
            listAtBet.Items.Add(newBet);
        }

        private void BtnCancelBet_Click(object sender, EventArgs e) {
            if (listAtBet.SelectedIndex < 0) {
                MessageBox.Show("請選擇欲移除的號碼組");
                return;
            }
            alreadyBet.RemoveAt(listAtBet.SelectedIndex);
            listAtBet.Items.RemoveAt(listAtBet.SelectedIndex);
        }

        private void BtnCancellAllBet_Click(object sender, EventArgs e) {
            alreadyBet.Clear();
            listAtBet.Items.Clear();

        }
        private void BtnAddSuper_Click(object sender, EventArgs e) {
            try {
                listSuperBet.Items.Clear();
                int newNum = Convert.ToInt32(txtSuperInput.Text);
                if (!(newNum <= 80 && newNum >= 1)) {
                    MessageBox.Show("請輸入1~80中的整數", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    
                } 
                else if (superNumBet.ContainsKey(newNum)) {
                    superNumBet[newNum] += 1;
                } 
                else {
                    superNumBet[newNum] = 1;
                    superBetKeyList.Add(newNum);
                }
                superBetKeyList.Sort();
                foreach (int key in superBetKeyList) {
                    listSuperBet.Items.Add($"[{key}], {superNumBet[key]}注");
                }
            } 
            catch {
                MessageBox.Show("請輸入1~80中的整數", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            txtSuperInput.Clear();
        }

        private void BtnCancellSelectedSuper_Click(object sender, EventArgs e) {
            if (listSuperBet.SelectedIndex < 0) {
                MessageBox.Show("請選擇欲移除的號碼組");
                return;
            }
            superNumBet.Remove(superBetKeyList[listSuperBet.SelectedIndex]);
            superBetKeyList.RemoveAt(listSuperBet.SelectedIndex);
            listSuperBet.Items.RemoveAt(listSuperBet.SelectedIndex);
        }

        private void BtnCancellAllSuperBet_Click(object sender, EventArgs e) {
            superNumBet.Clear();
            superBetKeyList.Clear();
            listSuperBet.Items.Clear();
        }
        private void BtnGenerateHit_Click(object sender, EventArgs e) {
            hittingNum19.Clear();
            superNum = 0; // superNum will not be sorted with other hitting numbers
            while (hittingNum19.Count < 19) {
                int currRandomNum = rand.Next(1, 81);
                if (superNum == 0) {
                    superNum = currRandomNum;
                }
                else if ((!hittingNum19.Contains(currRandomNum) && currRandomNum != superNum)) {
                    hittingNum19.Add(currRandomNum);
                }
            }
            int[] hitNums = hittingNum19.ToArray();
            Array.Sort(hitNums);
            hitArray[0].Text = superNum.ToString();
            for (int i = 1; i < hitArray.Length; i++) {
                hitArray[i].Text = hitNums[i - 1].ToString();
            }
            listHitted.Items.Clear();
        }

        private void BtnRedeem_Click(object sender, EventArgs e) {
            if (superNum == 0 || hittingNum19.Count < 19) {
                MessageBox.Show("尚未產生中獎號碼");
                return;
            }
            // build array of the winning rate
            listHitted.Items.Clear();
            int totalPrize = 0;
            int hittedBet = 0;
            // basic game
            foreach (List<int> bet in alreadyBet) {
                int countHit = 0;
                foreach (int picked in bet) {
                    if (picked == superNum || hittingNum19.Contains(picked)) {
                        countHit++;
                    }
                }
                // hit
                if (countHit >= winningCountBound[bet.Count] || (bet.Count >= 8 && countHit == 0)) {
                    hittedBet++;
                    string newHit = "[";
                    for (int i = 0; i < bet.Count; i++) {
                        if (i != 0) {
                            newHit += ",";
                        }
                        newHit += $"{bet[i]}";
                    }
                    newHit += "]";
                    listHitted.Items.Add(newHit);
                }
                totalPrize += 25 * winningArray[bet.Count][countHit];
            }
            // super number
            int superPrize = 0;
            int superBetWin = 0;
            int superBetCount = 0;
            foreach(int key in superNumBet.Keys) {
                superBetCount += superNumBet[key];
            }
            if (superNumBet.ContainsKey(superNum)) {
                superBetWin = superNumBet[superNum];
                superPrize += 1200 * superBetWin * 48;
            }
            int hitLarge = 0, hitSmall = 0, hitOdd = 0, hitEven = 0;
            bool winLarge = false, winSmall = false, winOdd = false, winEven = false; 
            if (superNum >= 41) {
                hitLarge++;
            }
            else {
                hitSmall++;
            }
            if (superNum % 2 == 0) {
                hitOdd++;
            }
            else {
                hitEven++;
            }
            foreach (int hnum in hittingNum19) {
                if (hnum >= 41) {
                    hitLarge++;
                }
                else {
                    hitSmall++;
                }
                if (hnum % 2 == 0) {
                    hitOdd++;
                }
                else {
                    hitEven++;
                }
            }
            int prizeLS = 0, prizeEO = 0;
            if (hitLarge >= 13) {
                prizeLS = 150 * betLarge * 6;
            }
            else if (hitSmall >= 13) {
                prizeLS = 150 * betSmall * 6;
            }
            if (hitOdd >= 13) {
                prizeEO = 150 * betOdd * 6;
            }
            else if (hitEven >= 13) {
                prizeEO = 150 * betEven * 6;
            }
            labelMessage.Text = $"基本玩法 : 本期共投注{alreadyBet.Count}注，投注金額{25 * alreadyBet.Count}元，共{hittedBet}注中獎，中獎金額{totalPrize}元。\n";
            labelMessage.Text += "-----------------------------------------------------------------------\n";
            labelMessage.Text += $"超級獎號: 本期共投注{superBetCount}注，投注金額{1200 * superBetCount}元，共{superBetWin}注中獎，中獎金額{superPrize}元。\n";
            labelMessage.Text += "-----------------------------------------------------------------------\n";
            labelMessage.Text += $"猜大小: 本期共投注猜大{betLarge}注，投注金額{betLarge * 150}元，投注猜小{betSmall}注，投注金額{betSmall * 150}元，\n";
            if (hitLarge >= 13) {
                labelMessage.Text += $"結果開大，";
            }
            else if (hitSmall >= 13) {
                labelMessage.Text += $"結果開小，";
            }
            else {
                labelMessage.Text += $"結果大小皆無中獎，";
            }
            labelMessage.Text += $"中獎金額{prizeLS}元。\n";
            labelMessage.Text += "-----------------------------------------------------------------------\n";
            labelMessage.Text += $"猜單雙: 本期共投注猜單{betOdd}注，投注金額{betOdd * 150}元，投注猜雙{betEven}注，投注金額{betEven * 150}元，\n";
            if (hitOdd >= 13) {
                labelMessage.Text += $"結果開單，";
            }
            else if (hitEven >= 13) {
                labelMessage.Text += $"結果開雙，";
            }
            else {
                labelMessage.Text += $"結果單雙皆無中獎，";
            }
            labelMessage.Text += $"中獎金額{prizeEO}元。\n";
        }

        private void BtnGuessLarge_Click(object sender, EventArgs e) {
            try {
                int currBetCount = Convert.ToInt32(txtGuessLS.Text);
                if (currBetCount < 0) {
                    MessageBox.Show("請輸入大於0的整數", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else {
                    betLarge += currBetCount;
                    lblLarge.Text = $"{betLarge}";
                }
            } 
            catch {
                MessageBox.Show("請輸入整數投注數量", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            txtGuessLS.Clear();
        }

        private void BtnGuessSmall_Click(object sender, EventArgs e) {
            try {
                int currBetCount = Convert.ToInt32(txtGuessLS.Text);
                if (currBetCount < 0) {
                    MessageBox.Show("請輸入大於0的整數", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else {
                    betSmall += currBetCount;
                    lblSmall.Text = $"{betSmall}";
                }
            } 
            catch {
                MessageBox.Show("請輸入整數投注數量", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            txtGuessLS.Clear();
        }

        private void BtnCancellLarge_Click(object sender, EventArgs e) {
            try {
                int cancelCount = Convert.ToInt32(txtGuessLS.Text);
                if (cancelCount < 0) {
                    MessageBox.Show("請輸入大於0的整數", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                } 
                else if (cancelCount > betLarge){
                    MessageBox.Show("欲取消數量超過已投注數，請重新操作", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else {
                    betLarge -= cancelCount;
                    lblLarge.Text = $"{betLarge}";
                }
            } catch {
                MessageBox.Show("請輸入為整數的欲取消數量", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            txtGuessLS.Clear();
        }

        private void BtnCancellSmall_Click(object sender, EventArgs e) {
            try {
                int cancelCount = Convert.ToInt32(txtGuessLS.Text);
                if (cancelCount < 0) {
                    MessageBox.Show("請輸入大於0的整數", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                } 
                else if (cancelCount > betSmall) {
                    MessageBox.Show("欲取消數量超過已投注數，請重新操作", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                } 
                else {
                    betSmall -= cancelCount;
                    lblSmall.Text = $"{betSmall}";
                }
            } catch {
                MessageBox.Show("請輸入為整數的欲取消數量", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            txtGuessLS.Clear();
        }

        private void BtnCleanBetLS_Click(object sender, EventArgs e) {
            betLarge = 0;
            betSmall = 0;
            lblLarge.Text = "0";
            lblSmall.Text = "0";
        }

        private void BtnGuessOdd_Click(object sender, EventArgs e) {
            try {
                int currBetCount = Convert.ToInt32(txtGuessEO.Text);
                if (currBetCount < 0) {
                    MessageBox.Show("請輸入大於0的整數", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                } 
                else {
                    betOdd += currBetCount;
                    lblOdd.Text = $"{betOdd}";
                }
            } 
            catch {
                MessageBox.Show("請輸入整數投注數量", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            txtGuessEO.Clear();
        }

        private void BtnGuessEven_Click(object sender, EventArgs e) {
            try {
                int currBetCount = Convert.ToInt32(txtGuessEO.Text);
                if (currBetCount < 0) {
                    MessageBox.Show("請輸入大於0的整數", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else {
                    betEven += currBetCount;
                    lblEven.Text = $"{betEven}";
                }
            }
            catch {
                MessageBox.Show("請輸入整數投注數量", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            txtGuessEO.Clear();
        }

        private void BtnCancellOdd_Click(object sender, EventArgs e) {
            try {
                int cancelCount = Convert.ToInt32(txtGuessLS.Text);
                if (cancelCount < 0) {
                    MessageBox.Show("請輸入大於0的整數", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (cancelCount > betOdd) {
                    MessageBox.Show("欲取消數量超過已投注數，請重新操作", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else {
                    betOdd -= cancelCount;
                    lblOdd.Text = $"{betOdd}";
                }
            }
            catch {
                MessageBox.Show("請輸入為整數的欲取消數量", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            txtGuessEO.Clear();
        }

        private void BtnCancellEven_Click(object sender, EventArgs e) {
            try {
                int cancelCount = Convert.ToInt32(txtGuessLS.Text);
                if (cancelCount < 0) {
                    MessageBox.Show("請輸入大於0的整數", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (cancelCount > betEven) {
                    MessageBox.Show("欲取消數量超過已投注數，請重新操作", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else {
                    betEven -= cancelCount;
                    lblEven.Text = $"{betEven}";
                }
            }
            catch {
                MessageBox.Show("請輸入為整數的欲取消數量", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            txtGuessEO.Clear();
        }

        private void BtnCleanBetEO_Click(object sender, EventArgs e) {
            betOdd = 0;
            betEven = 0;
            lblOdd.Text = "0";
            lblEven.Text = "0";
        }
    }
}
