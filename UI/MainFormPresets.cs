using System.Windows.Forms;

namespace FlowOptimization.UI
{
    static class MainFormPresets
    {
        public static void SetHorizontalLabel(System.Windows.Forms.Label label, string text)
        {
            //const int x = 15;
            //const int y = 5;
            //label.Location = new Point(x, y);
            //label.Location = new Point();
            label.Text = text;
        }

        public static void SetVerticalLabel(System.Windows.Forms.Label label, string text)
        {
            //const int x = 3;
            //const int y = 21;
            //label.Location = new Point(x, y);
            string temp = "";
            for (int i = 0; i < text.Length; i++)
                temp += text[i] + "\n";

            label.Text = temp;
            label.Anchor = AnchorStyles.Left;
        }
    }
}
