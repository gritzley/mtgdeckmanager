using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MtgDeckManagerView.ViewModels
{
    public static class PromptVM
    {
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                AutoSize = true,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            System.Windows.Forms.Label textLabel = new System.Windows.Forms.Label()
            {
                AutoSize = true,
                Text = text,
                Top = 20,
                Left = 50,
            };
            System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox()
            {
                Top = 50,
                Left = 50,
                AutoSize = true,
            };
            System.Windows.Forms.Button confirmation = new System.Windows.Forms.Button()
            {
                Top = 80,
                Left = 50,
                Text = "Ok",
                Width = 100,
                DialogResult = DialogResult.OK
            };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : null;
        }

        public static bool YesNo(string text, string caption)
        {
            Form prompt = new Form()
            {
                AutoSize = true,
                Padding = new Padding(50),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };

            Label textLabel = new Label()
            {
                Left = 50,
                Top = 20,
                AutoSize = true,
                Text = text
            };
            Button confirmation = new Button() 
            {
                Text = "Yes",
                Left = 50,
                Width = 100,
                Top = 50,
                DialogResult = DialogResult.Yes
            };
            Button declination = new Button()
            {
                Text = "No",
                Left = 200,
                Width = 100,
                Top = 50,
                DialogResult = DialogResult.No
            };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(declination);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;
            prompt.CancelButton = declination;

            return prompt.ShowDialog() == DialogResult.Yes;
        }
    }
}
