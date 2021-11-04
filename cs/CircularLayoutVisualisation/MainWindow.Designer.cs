
namespace CircularLayoutVisualisation
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Count = new System.Windows.Forms.TextBox();
            this.CountLabel = new System.Windows.Forms.Label();
            this.EnterButton = new System.Windows.Forms.Button();
            this.SizeScale = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Count
            // 
            this.Count.Location = new System.Drawing.Point(12, 36);
            this.Count.Name = "Count";
            this.Count.Size = new System.Drawing.Size(125, 27);
            this.Count.TabIndex = 0;
            // 
            // CountLabel
            // 
            this.CountLabel.AutoSize = true;
            this.CountLabel.Location = new System.Drawing.Point(12, 13);
            this.CountLabel.Name = "CountLabel";
            this.CountLabel.Size = new System.Drawing.Size(48, 20);
            this.CountLabel.TabIndex = 1;
            this.CountLabel.Text = "Count";
            // 
            // EnterButton
            // 
            this.EnterButton.Location = new System.Drawing.Point(143, 36);
            this.EnterButton.Name = "EnterButton";
            this.EnterButton.Size = new System.Drawing.Size(94, 29);
            this.EnterButton.TabIndex = 2;
            this.EnterButton.Text = "Enter";
            this.EnterButton.UseVisualStyleBackColor = true;
            // 
            // SizeScale
            // 
            this.SizeScale.Location = new System.Drawing.Point(12, 69);
            this.SizeScale.Name = "SizeScale";
            this.SizeScale.Size = new System.Drawing.Size(125, 27);
            this.SizeScale.TabIndex = 3;
            this.SizeScale.Text = "1";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.SizeScale);
            this.Controls.Add(this.EnterButton);
            this.Controls.Add(this.CountLabel);
            this.Controls.Add(this.Count);
            this.Name = "MainWindow";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Count;
        private System.Windows.Forms.Label CountLabel;
        private System.Windows.Forms.Button EnterButton;
        private System.Windows.Forms.TextBox SizeScale;
    }
}

